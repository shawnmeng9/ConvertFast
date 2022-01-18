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

            foreach (KeyValuePair<int, List<double>> entry in hdModel.sectionList)
            {
                IIsmParametricSection ismSection;
                ismSection = ismModel.AddParametricSection(null, IsmParametricShapeKind.HollowCircle);
                ismSection.SetHollowCircleDimensions(entry.Value[0], entry.Value[1]);
                ismSection.Name = "SECTION" + entry.Key.ToString();
            }

            foreach (KeyValuePair<int, List<int>> entry in hdModel.memberList)
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

            foreach (KeyValuePair<int, List<double>> entry in sdModel.sectionList)
            {
                IIsmParametricSection ismSection;
                ismSection = ismModel.AddParametricSection(null, IsmParametricShapeKind.HollowCircle);
                ismSection.SetHollowCircleDimensions(entry.Value[0], entry.Value[1]);
                ismSection.Name = "SECTION" + entry.Key.ToString();
            }

            foreach (KeyValuePair<int, List<int>> entry in sdModel.memberList)
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

            foreach (KeyValuePair<int, List<int>> entry in mdModel.lineNodeList)
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

            foreach (KeyValuePair<int, List<double>> entry in adModel.twrNodeList)
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
            
            for (int i = 0; i < adModel.twrNodeList.Count-1; i++)
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

                float x1 = 0;
                float y1 = 0;
                float z1 = 1;
                if ((Math.Abs(startPoint.X - endPoint.X) < 0.000001) && (Math.Abs(startPoint.Y - endPoint.Y) < 0.000001) && (Math.Abs(startPoint.Z - endPoint.Z) > 0.000001))
                {
                    x1 = 1;
                    y1 = 0;
                    z1 = 0;
                }
                ismBeam.Orientation = ismApi.GeometryApi.NewPoint3D(x1, y1, z1);

            }


            //TestFunction(adModel, ismApi, ref ismModel);
        }



        private static void TestFunction(ADModel adModel, IIsmApi ismApi, ref IIsmModel ismModel)
        {

            var tempItem = adModel.twrNodeList.ElementAt(adModel.twrNodeList.Count - 1);
            IIsmNode ismNodeTemp1;
            ismNodeTemp1 = ismModel.AddNode(null);
            ismNodeTemp1.Name = "TEMP1";
            ismNodeTemp1.Location = ismApi.GeometryApi.NewPoint3D(1.9, 0.0, tempItem.Value[0] + 1.75);

            IIsmNode ismNodeTemp2;
            ismNodeTemp2 = ismModel.AddNode(null);
            ismNodeTemp2.Name = "TEMP2";
            ismNodeTemp2.Location = ismApi.GeometryApi.NewPoint3D(1.9 - 5.0, 0.0, tempItem.Value[0] + 1.75);

            IIsmParametricSection ismSectionTemp;
            ismSectionTemp = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidCircle);
            ismSectionTemp.SetSolidCircleDimensions(3.0);
            ismSectionTemp.Name = "TEMPSECT";

            IIsmCurveMember ismBeamTemp;
            ismBeamTemp = ismModel.AddCurveMember(null);
            ismBeamTemp.Name = "TEMPMEM";

            IIsmPoint3D startPointTemp = ismApi.GeometryApi.NewPoint3D(ismNodeTemp1.Location.X, ismNodeTemp1.Location.Y, ismNodeTemp1.Location.Z);
            IIsmPoint3D endPointTemp = ismApi.GeometryApi.NewPoint3D(ismNodeTemp2.Location.X, ismNodeTemp2.Location.Y, ismNodeTemp2.Location.Z);
            ismBeamTemp.Location = ismApi.GeometryApi.NewLineSegment3D(startPointTemp, endPointTemp);
            ismNodeTemp1.AddMember(ismBeamTemp);
            ismNodeTemp2.AddMember(ismBeamTemp);

            ismBeamTemp.Section = ismSectionTemp;
            ismBeamTemp.Use = IsmCurveMemberUse.Column;
            ismBeamTemp.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
            ismBeamTemp.PlacementPoint = IsmSectionPlacementPoint.Centroid;
            ismBeamTemp.LoadResistance = IsmLoadResistance.GravityAndLateral;
            ismBeamTemp.MirrorShapeAboutYAxis = false;

            float x = 0;
            float y = 0;
            float z = 1;
            if ((Math.Abs(startPointTemp.X - endPointTemp.X) < 0.000001) && (Math.Abs(startPointTemp.Y - endPointTemp.Y) < 0.000001) && (Math.Abs(startPointTemp.Z - endPointTemp.Z) > 0.000001))
            {
                x = 1;
                y = 0;
                z = 0;
            }
            ismBeamTemp.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);

            IIsmNode ismNodeTemp11;
            ismNodeTemp11 = ismModel.AddNode(null);
            ismNodeTemp11.Name = "TEMP11";
            ismNodeTemp11.Location = ismApi.GeometryApi.NewPoint3D(ismNodeTemp1.Location.X + 1.771, ismNodeTemp1.Location.Y, ismNodeTemp1.Location.Z);

            IIsmNode ismNodeTemp3;
            ismNodeTemp3 = ismModel.AddNode(null);
            ismNodeTemp3.Name = "TEMP3";
            ismNodeTemp3.Location = ismApi.GeometryApi.NewPoint3D(ismNodeTemp1.Location.X+1.771, ismNodeTemp1.Location.Y+62.5, ismNodeTemp1.Location.Z-30.0);

            IIsmParametricSection ismSection1;
            ismSection1 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
            ismSection1.SetSolidRectangleDimensions(0.2, 3.542);
            ismSection1.Name = "BLDSECT1";

            IIsmParametricSection ismSection2;
            ismSection2 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
            ismSection2.SetSolidRectangleDimensions(0.2, 1.419);
            ismSection2.Name = "BLDSECT2";

            IIsmVaryingSection ismVarySect;
            ismVarySect = ismModel.AddVaryingSection(null);
            ismVarySect.Name = "TEMPVSECT";

            IIsmVaryingSectionSegment ismSegment;
            ismSegment = ismVarySect.AddSegment();
            ismSegment.Name = "TEMPSEG";
            ismSegment.StartSection = ismSection1;
            ismSegment.EndSection = ismSection2;

            IIsmCurveMember ismBeamTemp2;
            ismBeamTemp2 = ismModel.AddCurveMember(null);
            ismBeamTemp2.Name = "TEMPMEM2";

            IIsmPoint3D startPointTemp2 = ismApi.GeometryApi.NewPoint3D(ismNodeTemp11.Location.X, ismNodeTemp11.Location.Y, ismNodeTemp11.Location.Z);
            IIsmPoint3D endPointTemp2 = ismApi.GeometryApi.NewPoint3D(ismNodeTemp3.Location.X, ismNodeTemp3.Location.Y, ismNodeTemp3.Location.Z);
            ismBeamTemp2.Location = ismApi.GeometryApi.NewLineSegment3D(startPointTemp2, endPointTemp2);
            ismNodeTemp11.AddMember(ismBeamTemp2);
            ismNodeTemp3.AddMember(ismBeamTemp2);

            ismBeamTemp2.Section = ismVarySect;
            ismBeamTemp2.Use = IsmCurveMemberUse.Column;
            ismBeamTemp2.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
            ismBeamTemp2.PlacementPoint = IsmSectionPlacementPoint.Centroid;
            ismBeamTemp2.LoadResistance = IsmLoadResistance.GravityAndLateral;
            ismBeamTemp2.MirrorShapeAboutYAxis = false;

            x = 0;
            y = 0;
            z = 1;
            if ((Math.Abs(startPointTemp2.X - endPointTemp2.X) < 0.000001) && (Math.Abs(startPointTemp2.Y - endPointTemp2.Y) < 0.000001) && (Math.Abs(startPointTemp2.Z - endPointTemp2.Z) > 0.000001))
            {
                x = 1;
                y = 0;
                z = 0;
            }
            ismBeamTemp2.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);



            IIsmNode ismNodeTemp4;
            ismNodeTemp4 = ismModel.AddNode(null);
            ismNodeTemp4.Name = "TEMP4";
            ismNodeTemp4.Location = ismApi.GeometryApi.NewPoint3D(ismNodeTemp1.Location.X+1.771, ismNodeTemp1.Location.Y-62.5, ismNodeTemp1.Location.Z-30.0);

            IIsmCurveMember ismBeamTemp3;
            ismBeamTemp3 = ismModel.AddCurveMember(null);
            ismBeamTemp3.Name = "TEMPMEM3";

            IIsmPoint3D startPointTemp3 = ismApi.GeometryApi.NewPoint3D(ismNodeTemp11.Location.X, ismNodeTemp11.Location.Y, ismNodeTemp11.Location.Z);
            IIsmPoint3D endPointTemp3 = ismApi.GeometryApi.NewPoint3D(ismNodeTemp4.Location.X, ismNodeTemp4.Location.Y, ismNodeTemp4.Location.Z);
            ismBeamTemp3.Location = ismApi.GeometryApi.NewLineSegment3D(startPointTemp3, endPointTemp3);
            ismNodeTemp11.AddMember(ismBeamTemp3);
            ismNodeTemp4.AddMember(ismBeamTemp3);

            ismBeamTemp3.Section = ismVarySect;
            ismBeamTemp3.Use = IsmCurveMemberUse.Column;
            ismBeamTemp3.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
            ismBeamTemp3.PlacementPoint = IsmSectionPlacementPoint.Centroid;
            ismBeamTemp3.LoadResistance = IsmLoadResistance.GravityAndLateral;
            ismBeamTemp3.MirrorShapeAboutYAxis = false;

            x = 0;
            y = 0;
            z = 1;
            if ((Math.Abs(startPointTemp2.X - endPointTemp2.X) < 0.000001) && (Math.Abs(startPointTemp2.Y - endPointTemp2.Y) < 0.000001) && (Math.Abs(startPointTemp2.Z - endPointTemp2.Z) > 0.000001))
            {
                x = 1;
                y = 0;
                z = 0;
            }
            ismBeamTemp3.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);


            IIsmNode ismNodeTemp5;
            ismNodeTemp5 = ismModel.AddNode(null);
            ismNodeTemp5.Name = "TEMP5";
            ismNodeTemp5.Location = ismApi.GeometryApi.NewPoint3D(ismNodeTemp1.Location.X+1.771, ismNodeTemp1.Location.Y, ismNodeTemp1.Location.Z + 62.5);

            IIsmCurveMember ismBeamTemp4;
            ismBeamTemp4 = ismModel.AddCurveMember(null);
            ismBeamTemp4.Name = "TEMPMEM4";

            IIsmParametricSection ismSection3;
            ismSection3 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
            ismSection3.SetSolidRectangleDimensions(3.542, 0.2);
            ismSection3.Name = "BLDSECT3";

            IIsmParametricSection ismSection4;
            ismSection4 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
            ismSection4.SetSolidRectangleDimensions(1.419, 0.2);
            ismSection4.Name = "BLDSECT4";

            IIsmVaryingSection ismVarySect2;
            ismVarySect2 = ismModel.AddVaryingSection(null);
            ismVarySect2.Name = "TEMPVSECT2";

            IIsmVaryingSectionSegment ismSegment2;
            ismSegment2 = ismVarySect2.AddSegment();
            ismSegment2.Name = "TEMPSEG2";
            ismSegment2.StartSection = ismSection3;
            ismSegment2.EndSection = ismSection4;

            IIsmPoint3D startPointTemp4 = ismApi.GeometryApi.NewPoint3D(ismNodeTemp11.Location.X, ismNodeTemp11.Location.Y, ismNodeTemp11.Location.Z);
            IIsmPoint3D endPointTemp4 = ismApi.GeometryApi.NewPoint3D(ismNodeTemp5.Location.X, ismNodeTemp5.Location.Y, ismNodeTemp5.Location.Z);
            ismBeamTemp4.Location = ismApi.GeometryApi.NewLineSegment3D(startPointTemp4, endPointTemp4);
            ismNodeTemp11.AddMember(ismBeamTemp4);
            ismNodeTemp5.AddMember(ismBeamTemp4);

            ismBeamTemp4.Section = ismVarySect2;
            ismBeamTemp4.Use = IsmCurveMemberUse.Column;
            ismBeamTemp4.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
            ismBeamTemp4.PlacementPoint = IsmSectionPlacementPoint.Centroid;
            ismBeamTemp4.LoadResistance = IsmLoadResistance.GravityAndLateral;
            ismBeamTemp4.MirrorShapeAboutYAxis = false;

            x = 0;
            y = 0;
            z = 1;
            if ((Math.Abs(startPointTemp4.X - endPointTemp4.X) < 0.000001) && (Math.Abs(startPointTemp4.Y - endPointTemp4.Y) < 0.000001) && (Math.Abs(startPointTemp4.Z - endPointTemp4.Z) > 0.000001))
            {
                x = 1;
                y = 0;
                z = 0;
            }
            ismBeamTemp4.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);
        }
    }
}
