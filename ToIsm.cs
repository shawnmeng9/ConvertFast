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
                ConvertADToIsm(fstModel.adModel, ismApi, ref ismModel, ref status);
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


        private static void ConvertADToIsm(ADModel adModel, IIsmApi ismApi, ref IIsmModel ismModel, ref string status)
        {
            var ismTwrSectList = new List<IIsmParametricSection>();

            foreach (KeyValuePair<int, double[]> entry in adModel.twrNodeList)
            {
                IIsmNode ismNode;
                ismNode = ismModel.AddNode(null);
                ismNode.Name = "TWRND" + entry.Key.ToString();
                ismNode.Location = ismApi.GeometryApi.NewPoint3D(0.0, 0.0, entry.Value[0]);

                IIsmParametricSection ismSection;
                ismSection = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidCircle);
                ismSection.SetSolidCircleDimensions(entry.Value[1]);
                ismSection.Name = "TWRSECT" + entry.Key.ToString();
                ismTwrSectList.Add(ismSection);
            }
            
            for (int i = 0; i < adModel.twrNodeList.Count-2; i++)
            {
                var item = adModel.twrNodeList.ElementAt(i);
                var item2 = adModel.twrNodeList.ElementAt(i+1);
                
                IIsmVaryingSection ismVarySect;
                ismVarySect = ismModel.AddVaryingSection(null);
                ismVarySect.Name = "TWRVSECT" + item.Key.ToString();

                IIsmVaryingSectionSegment ismSegment;
                ismSegment = ismVarySect.AddSegment();
                ismSegment.Name = "TWRSEG" + item.Key.ToString();
                ismSegment.StartSection = ismTwrSectList[i];
                ismSegment.EndSection = ismTwrSectList[i + 1];

                IIsmCurveMember ismBeam;
                ismBeam = ismModel.AddCurveMember(null);
                ismBeam.Name = "TWRMEM" + item.Key.ToString();

                IIsmPoint3D startPoint = ismApi.GeometryApi.NewPoint3D(0.0, 0.0, item.Value[0]);
                IIsmPoint3D endPoint = ismApi.GeometryApi.NewPoint3D(0.0, 0.0, item2.Value[0]);
                ismBeam.Location = ismApi.GeometryApi.NewLineSegment3D(startPoint, endPoint);

                IEnumerator<IIsmNode> nodeIterator = ismModel.GetNodes().GetEnumerator();
                while (nodeIterator.MoveNext())
                {
                    if (nodeIterator.Current.Name == "TWRND" + item.Key.ToString() || nodeIterator.Current.Name == "TWRND" + item2.Key.ToString())
                    {
                        nodeIterator.Current.AddMember(ismBeam);
                    }
                }

                ismBeam.Section = ismVarySect;
                ismBeam.Use = IsmCurveMemberUse.Column;
                ismBeam.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                ismBeam.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                ismBeam.LoadResistance = IsmLoadResistance.GravityAndLateral;
                ismBeam.MirrorShapeAboutYAxis = false;

                float x = 0;
                float y = 0;
                float z = 1;
                if ((Math.Abs(startPoint.X - endPoint.X) < 0.000001) && (Math.Abs(startPoint.Y - endPoint.Y) < 0.000001) && (Math.Abs(startPoint.Z - endPoint.Z) > 0.000001))
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
