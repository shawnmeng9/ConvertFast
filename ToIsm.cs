using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using Bentley.Structural.Ism.Api_8_1;

namespace ConvertFast
{
    public static class ToIsm
    {
        public static void ConvertToIsm(FstModel fstModel, ref string status)
        {
            IIsmModel ismModel = null;
            IIsmApi ismApi = null;

            IsmEntryPoint.CheckIsmInstallation();
            ismApi = IsmEntryPoint.GetIsmApi();
            ismApi.SetApplicationName("FAST");
            ismApi.SetApiUnits(IsmLengthUnit.Meter, IsmForceUnit.Newton, IsmMassUnit.Kilogram, IsmAngleUnit.Radian, IsmTemperatureUnit.Kelvin);

            ismModel = ismApi.CreateModel();

            if (fstModel.convertHydro)
            {
                ConvertHDToIsm(fstModel.hdModel, ismApi, ref ismModel, ref status);
            }
            if (fstModel.convertSub)
            {
                ConvertSDToIsm(fstModel.sdModel, ismApi, ref ismModel, ref status);
            }
            if (fstModel.convertMooring)
            {
                ConvertMDToIsm(fstModel.mdModel, ismApi, ref ismModel, ref status);
            }
            if (fstModel.convertAero)
            {
                //add code
            }
            if (fstModel.convertElast)
            {
                //add code
            }

            string repository = fstModel.filePath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fstModel.fstFile) + ".ism.dgn";
            //string historyMessage = "";
            if (File.Exists(repository))
            {
                File.Delete(repository);
            }
            //ismApi.CreateRepositoryWithoutUi(repository, ismModel, historyMessage);
            ismApi.CreateRepositoryWithUi(repository, ismModel);
        }

        private static void ConvertHDToIsm(HDModel hdModel, IIsmApi ismApi, ref IIsmModel ismModel, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in hdModel.jointList)
            {
                IIsmNode ismNode;
                ismNode = ismModel.AddNode(null);
                ismNode.Name = "JOINT" + entry.Key.ToString();
                ismNode.Location = ismApi.GeometryApi.NewPoint3D(entry.Value.X, entry.Value.Y, entry.Value.Z);
            }

            foreach (KeyValuePair<int, double[]> entry in hdModel.sectionList)
            {
                IIsmParametricSection ismSection;
                ismSection = ismModel.AddParametricSection(null, IsmParametricShapeKind.HollowCircle);
                ismSection.SetHollowCircleDimensions(entry.Value[0], entry.Value[1]);
                ismSection.Name = "SECTION" + entry.Key.ToString();
            }

            foreach (KeyValuePair<int, int[]> entry in hdModel.memberList)
            {
                IIsmCurveMember ismBeam;
                ismBeam = ismModel.AddCurveMember(null);
                ismBeam.Name = "MEMBER" + entry.Key.ToString();

                Vector3 node1 = hdModel.jointList[entry.Value[0]];
                Vector3 node2 = hdModel.jointList[entry.Value[1]];
                IIsmPoint3D startPoint = ismApi.GeometryApi.NewPoint3D(node1.X, node1.Y, node1.Z);
                IIsmPoint3D endPoint = ismApi.GeometryApi.NewPoint3D(node2.X, node2.Y, node2.Z);
                ismBeam.Location = ismApi.GeometryApi.NewLineSegment3D(startPoint, endPoint);

                IEnumerator<IIsmNode> nodeIterator = ismModel.GetNodes().GetEnumerator();
                while (nodeIterator.MoveNext())
                {
                    if (nodeIterator.Current.Name == "JOINT" + entry.Value[0] || nodeIterator.Current.Name == "JOINT" + entry.Value[1])
                    {
                        nodeIterator.Current.AddMember(ismBeam);
                    }
                }              
                
                IEnumerator<IIsmParametricSection> parametricIterator = ismModel.GetParametricSections().GetEnumerator();
                while (parametricIterator.MoveNext())
                {
                    if (parametricIterator.Current.Name == "SECTION" + entry.Value[2])
                    {
                        ismBeam.Section = parametricIterator.Current;
                        break;
                    }
                }

                ismBeam.Use = IsmCurveMemberUse.Column;
                ismBeam.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                ismBeam.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                ismBeam.LoadResistance = IsmLoadResistance.GravityAndLateral;
                ismBeam.MirrorShapeAboutYAxis = false;

                float x = 0;
                float y = 0;
                float z = 1;
                if ((Math.Abs(node1.X - node2.X) < 0.000001) && (Math.Abs(node1.Y - node2.Y) < 0.000001) && (Math.Abs(node1.Z - node2.Z) > 0.000001))
                {
                    x = 1;
                    y = 0;
                    z = 0;
                }

                ismBeam.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);
            }
        }


        private static void ConvertSDToIsm(SDModel sdModel, IIsmApi ismApi, ref IIsmModel ismModel, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in sdModel.jointList)
            {
                IIsmNode ismNode;
                ismNode = ismModel.AddNode(null);
                ismNode.Name = "JOINT" + entry.Key.ToString();
                ismNode.Location = ismApi.GeometryApi.NewPoint3D(entry.Value.X, entry.Value.Y, entry.Value.Z);
            }

            foreach (KeyValuePair<int, double[]> entry in sdModel.sectionList)
            {
                IIsmParametricSection ismSection;
                ismSection = ismModel.AddParametricSection(null, IsmParametricShapeKind.HollowCircle);
                ismSection.SetHollowCircleDimensions(entry.Value[0], entry.Value[1]);
                ismSection.Name = "SECTION" + entry.Key.ToString();
            }

            foreach (KeyValuePair<int, int[]> entry in sdModel.memberList)
            {
                IIsmCurveMember ismBeam;
                ismBeam = ismModel.AddCurveMember(null);
                ismBeam.Name = "MEMBER" + entry.Key.ToString();

                Vector3 node1 = sdModel.jointList[entry.Value[0]];
                Vector3 node2 = sdModel.jointList[entry.Value[1]];
                IIsmPoint3D startPoint = ismApi.GeometryApi.NewPoint3D(node1.X, node1.Y, node1.Z);
                IIsmPoint3D endPoint = ismApi.GeometryApi.NewPoint3D(node2.X, node2.Y, node2.Z);
                ismBeam.Location = ismApi.GeometryApi.NewLineSegment3D(startPoint, endPoint);

                IEnumerator<IIsmNode> nodeIterator = ismModel.GetNodes().GetEnumerator();
                while (nodeIterator.MoveNext())
                {
                    if (nodeIterator.Current.Name == "JOINT" + entry.Value[0] || nodeIterator.Current.Name == "JOINT" + entry.Value[1])
                    {
                        nodeIterator.Current.AddMember(ismBeam);
                    }
                }

                IEnumerator<IIsmParametricSection> parametricIterator = ismModel.GetParametricSections().GetEnumerator();
                while (parametricIterator.MoveNext())
                {
                    if (parametricIterator.Current.Name == "SECTION" + entry.Value[2])
                    {
                        ismBeam.Section = parametricIterator.Current;
                        break;
                    }
                }

                ismBeam.Use = IsmCurveMemberUse.Column;
                ismBeam.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                ismBeam.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                ismBeam.LoadResistance = IsmLoadResistance.GravityAndLateral;
                ismBeam.MirrorShapeAboutYAxis = false;

                float x = 0;
                float y = 0;
                float z = 1;
                if ((Math.Abs(node1.X - node2.X) < 0.000001) && (Math.Abs(node1.Y - node2.Y) < 0.000001) && (Math.Abs(node1.Z - node2.Z) > 0.000001))
                {
                    x = 1;
                    y = 0;
                    z = 0;
                }

                ismBeam.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);
            }
        }

        private static void ConvertMDToIsm(MDModel mdModel, IIsmApi ismApi, ref IIsmModel ismModel, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in mdModel.connectList)
            {
                IIsmNode ismNode;
                ismNode = ismModel.AddNode(null);
                ismNode.Name = "CONNECT" + entry.Key.ToString();
                ismNode.Location = ismApi.GeometryApi.NewPoint3D(entry.Value.X, entry.Value.Y, entry.Value.Z);


            }

            foreach (KeyValuePair<int, int[]> entry in mdModel.lineNodeList)
            {
                IIsmCurveMember ismBeam;
                ismBeam = ismModel.AddCurveMember(null);
                ismBeam.Name = "LINE" + entry.Key.ToString();

                Vector3 node1 = mdModel.connectList[entry.Value[0]];
                Vector3 node2 = mdModel.connectList[entry.Value[1]];
                IIsmPoint3D startPoint = ismApi.GeometryApi.NewPoint3D(node1.X, node1.Y, node1.Z);
                IIsmPoint3D endPoint = ismApi.GeometryApi.NewPoint3D(node2.X, node2.Y, node2.Z);
                ismBeam.Location = ismApi.GeometryApi.NewLineSegment3D(startPoint, endPoint);

                IEnumerator<IIsmNode> nodeIterator = ismModel.GetNodes().GetEnumerator();
                while (nodeIterator.MoveNext())
                {
                    if (nodeIterator.Current.Name == "CONNECT" + entry.Value[0] || nodeIterator.Current.Name == "CONNECT" + entry.Value[1])
                    {
                        nodeIterator.Current.AddMember(ismBeam);
                    }
                }

                ismBeam.Use = IsmCurveMemberUse.Column;
                ismBeam.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                ismBeam.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                ismBeam.LoadResistance = IsmLoadResistance.GravityAndLateral;
                ismBeam.MirrorShapeAboutYAxis = false;

                float x = 0;
                float y = 0;
                float z = 1;
                if ((Math.Abs(node1.X - node2.X) < 0.000001) && (Math.Abs(node1.Y - node2.Y) < 0.000001) && (Math.Abs(node1.Z - node2.Z) > 0.000001))
                {
                    x = 1;
                    y = 0;
                    z = 0;
                }
                ismBeam.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);
            }




        }
    }
}
