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
                ConvertADToIsm(fstModel.adModel, fstModel.edModel, ismApi, ref ismModel, ref status);
            }
            if (fstModel.convertElast)
            {
                ConvertEDToIsm(fstModel.edModel, ismApi, ref ismModel, ref status);
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
                ismNode.Name = "HD" + entry.Key.ToString();
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
                ismNode.Name = "SD" + entry.Key.ToString();
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


        private static void ConvertADToIsm(ADModel adModel, EDModel edModel, IIsmApi ismApi, ref IIsmModel ismModel, ref string status)
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

            ConvertADBladesToIsm(adModel, edModel, ismApi, ref ismModel, ref status);

        }


        private static void ConvertADBladesToIsm(ADModel adModel, EDModel edModel, IIsmApi ismApi, ref IIsmModel ismModel, ref string status)
        {
            if (edModel != null)
            {
                var twrTopNode = adModel.twrNodeList.ElementAt(adModel.twrNodeList.Count - 1);

                IIsmNode ismNodeRotor1;
                ismNodeRotor1 = ismModel.AddNode(null);
                ismNodeRotor1.Name = "RTRND1";
                ismNodeRotor1.Location = ismApi.GeometryApi.NewPoint3D(edModel.NacCMxn, 0.0, twrTopNode.Value[0] + edModel.NacCMzn);

                IIsmNode ismNodeRotor2;
                ismNodeRotor2 = ismModel.AddNode(null);
                ismNodeRotor2.Name = "RTRND2";
                ismNodeRotor2.Location = ismApi.GeometryApi.NewPoint3D(-twrTopNode.Value[1], 0.0, twrTopNode.Value[0] + edModel.NacCMzn);

                IIsmParametricSection ismSectRotor;
                ismSectRotor = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidCircle);
                ismSectRotor.SetSolidCircleDimensions(edModel.HubRad * 2.0);
                ismSectRotor.Name = "RTRSECT";

                IIsmCurveMember ismBeamRotor;
                ismBeamRotor = ismModel.AddCurveMember(null);
                ismBeamRotor.Name = "RTRMEM";

                IIsmPoint3D startPointTemp = ismApi.GeometryApi.NewPoint3D(ismNodeRotor1.Location.X, ismNodeRotor1.Location.Y, ismNodeRotor1.Location.Z);
                IIsmPoint3D endPointTemp = ismApi.GeometryApi.NewPoint3D(ismNodeRotor2.Location.X, ismNodeRotor2.Location.Y, ismNodeRotor2.Location.Z);
                ismBeamRotor.Location = ismApi.GeometryApi.NewLineSegment3D(startPointTemp, endPointTemp);
                ismNodeRotor1.AddMember(ismBeamRotor);
                ismNodeRotor2.AddMember(ismBeamRotor);

                ismBeamRotor.Section = ismSectRotor;
                ismBeamRotor.Use = IsmCurveMemberUse.Column;
                ismBeamRotor.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                ismBeamRotor.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                ismBeamRotor.LoadResistance = IsmLoadResistance.GravityAndLateral;
                ismBeamRotor.MirrorShapeAboutYAxis = false;

                float x = 0;
                float y = 0;
                float z = 1;
                if ((Math.Abs(startPointTemp.X - endPointTemp.X) < 0.000001) && (Math.Abs(startPointTemp.Y - endPointTemp.Y) < 0.000001) && (Math.Abs(startPointTemp.Z - endPointTemp.Z) > 0.000001))
                {
                    x = 1;
                    y = 0;
                    z = 0;
                }
                ismBeamRotor.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);

                if (edModel.NumBl == 3)
                {
                    //Blade_1
                    var bldNdProp11 = adModel.adBldProp1.ElementAt(0);
                    var bldNdProp1N = adModel.adBldProp1.ElementAt(adModel.adBldProp1.Count - 1);
                    double bldChord11 = bldNdProp11.Value[5];
                    double bldChord1N = bldNdProp1N.Value[5];
                    double bldSpn11 = bldNdProp11.Value[0];
                    double bldSpn1N = bldNdProp1N.Value[0] - bldSpn11;

                    IIsmNode ismNodeBlade0;
                    ismNodeBlade0 = ismModel.AddNode(null);
                    ismNodeBlade0.Name = "BLDND0";
                    ismNodeBlade0.Location = ismApi.GeometryApi.NewPoint3D(ismNodeRotor1.Location.X + bldChord11 / 2.0, ismNodeRotor1.Location.Y, ismNodeRotor1.Location.Z);

                    IIsmNode ismNodeBlade1;
                    ismNodeBlade1 = ismModel.AddNode(null);
                    ismNodeBlade1.Name = "BLDND1";
                    ismNodeBlade1.Location = ismApi.GeometryApi.NewPoint3D(ismNodeRotor1.Location.X + bldChord11/2.0, ismNodeRotor1.Location.Y, ismNodeRotor1.Location.Z + bldSpn1N);

                    IIsmCurveMember ismBeamBlade1;
                    ismBeamBlade1 = ismModel.AddCurveMember(null);
                    ismBeamBlade1.Name = "BLDBEAM1";

                    IIsmParametricSection ismSectBlade11;
                    ismSectBlade11 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
                    ismSectBlade11.SetSolidRectangleDimensions(bldChord11, 0.2);
                    ismSectBlade11.Name = "BLDSECT11";

                    IIsmParametricSection ismSectBlade12;
                    ismSectBlade12 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
                    ismSectBlade12.SetSolidRectangleDimensions(bldChord1N, 0.2);
                    ismSectBlade12.Name = "BLDSECT12";

                    IIsmVaryingSection ismVarySectBlade1;
                    ismVarySectBlade1 = ismModel.AddVaryingSection(null);
                    ismVarySectBlade1.Name = "BLDVSECT1";

                    IIsmVaryingSectionSegment ismSegmentBlade1;
                    ismSegmentBlade1 = ismVarySectBlade1.AddSegment();
                    ismSegmentBlade1.Name = "BLDSEG1";
                    ismSegmentBlade1.StartSection = ismSectBlade11;
                    ismSegmentBlade1.EndSection = ismSectBlade12;

                    IIsmPoint3D startPointBlade1 = ismApi.GeometryApi.NewPoint3D(ismNodeBlade0.Location.X, ismNodeBlade0.Location.Y, ismNodeBlade0.Location.Z);
                    IIsmPoint3D endPointBlade1 = ismApi.GeometryApi.NewPoint3D(ismNodeBlade1.Location.X, ismNodeBlade1.Location.Y, ismNodeBlade1.Location.Z);
                    ismBeamBlade1.Location = ismApi.GeometryApi.NewLineSegment3D(startPointBlade1, endPointBlade1);
                    ismNodeBlade0.AddMember(ismBeamBlade1);
                    ismNodeBlade1.AddMember(ismBeamBlade1);

                    ismBeamBlade1.Section = ismVarySectBlade1;
                    ismBeamBlade1.Use = IsmCurveMemberUse.Column;
                    ismBeamBlade1.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                    ismBeamBlade1.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                    ismBeamBlade1.LoadResistance = IsmLoadResistance.GravityAndLateral;
                    ismBeamBlade1.MirrorShapeAboutYAxis = false;

                    x = 0;
                    y = 0;
                    z = 1;
                    if ((Math.Abs(startPointBlade1.X - endPointBlade1.X) < 0.000001) && (Math.Abs(startPointBlade1.Y - endPointBlade1.Y) < 0.000001) && (Math.Abs(startPointBlade1.Z - endPointBlade1.Z) > 0.000001))
                    {
                        x = 1;
                        y = 0;
                        z = 0;
                    }
                    ismBeamBlade1.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);


                    //Blade_2
                    var bldNdProp21 = adModel.adBldProp2.ElementAt(0);
                    var bldNdProp2N = adModel.adBldProp2.ElementAt(adModel.adBldProp2.Count - 1);
                    double bldChord21 = bldNdProp21.Value[5];
                    double bldChord2N = bldNdProp2N.Value[5];
                    double bldSpn21 = bldNdProp21.Value[0];
                    double bldSpn2N = bldNdProp2N.Value[0] - bldSpn21;

                    IIsmNode ismNodeBlade2;
                    ismNodeBlade2 = ismModel.AddNode(null);
                    ismNodeBlade2.Name = "BLDND2";
                    ismNodeBlade2.Location = ismApi.GeometryApi.NewPoint3D(
                        ismNodeRotor1.Location.X + bldChord21 / 2.0, 
                        ismNodeRotor1.Location.Y + bldSpn2N * Math.Cos(Math.PI * 30.0 / 180.0), 
                        ismNodeRotor1.Location.Z - bldSpn2N * Math.Sin(Math.PI * 30.0 / 180.0));

                    IIsmParametricSection ismSectBlade21;
                    ismSectBlade21 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
                    ismSectBlade21.SetSolidRectangleDimensions(0.2, bldChord21);
                    ismSectBlade21.Name = "BLDSECT21";

                    IIsmParametricSection ismSectBlade22;
                    ismSectBlade22 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
                    ismSectBlade22.SetSolidRectangleDimensions(0.2, bldChord2N);
                    ismSectBlade22.Name = "BLDSECT22";

                    IIsmVaryingSection ismVarySectBlade2;
                    ismVarySectBlade2 = ismModel.AddVaryingSection(null);
                    ismVarySectBlade2.Name = "BLDVSECT2";

                    IIsmVaryingSectionSegment ismSegmentBlade2;
                    ismSegmentBlade2 = ismVarySectBlade2.AddSegment();
                    ismSegmentBlade2.Name = "BLDSEG2";
                    ismSegmentBlade2.StartSection = ismSectBlade21;
                    ismSegmentBlade2.EndSection = ismSectBlade22;

                    IIsmCurveMember ismBeamBlade2;
                    ismBeamBlade2 = ismModel.AddCurveMember(null);
                    ismBeamBlade2.Name = "BLDBEAM2";

                    IIsmPoint3D startPointBlade2 = ismApi.GeometryApi.NewPoint3D(ismNodeBlade0.Location.X, ismNodeBlade0.Location.Y, ismNodeBlade0.Location.Z);
                    IIsmPoint3D endPointBlade2 = ismApi.GeometryApi.NewPoint3D(ismNodeBlade2.Location.X, ismNodeBlade2.Location.Y, ismNodeBlade2.Location.Z);
                    ismBeamBlade2.Location = ismApi.GeometryApi.NewLineSegment3D(startPointBlade2, endPointBlade2);
                    ismNodeBlade0.AddMember(ismBeamBlade2);
                    ismNodeBlade2.AddMember(ismBeamBlade2);

                    ismBeamBlade2.Section = ismVarySectBlade2;
                    ismBeamBlade2.Use = IsmCurveMemberUse.Column;
                    ismBeamBlade2.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                    ismBeamBlade2.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                    ismBeamBlade2.LoadResistance = IsmLoadResistance.GravityAndLateral;
                    ismBeamBlade2.MirrorShapeAboutYAxis = false;

                    x = 0;
                    y = 0;
                    z = 1;
                    if ((Math.Abs(startPointBlade2.X - endPointBlade2.X) < 0.000001) && (Math.Abs(startPointBlade2.Y - endPointBlade2.Y) < 0.000001) && (Math.Abs(startPointBlade2.Z - endPointBlade2.Z) > 0.000001))
                    {
                        x = 1;
                        y = 0;
                        z = 0;
                    }
                    ismBeamBlade2.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);


                    //Blade_3
                    var bldNdProp31 = adModel.adBldProp3.ElementAt(0);
                    var bldNdProp3N = adModel.adBldProp3.ElementAt(adModel.adBldProp3.Count - 1);
                    double bldChord31 = bldNdProp31.Value[5];
                    double bldChord3N = bldNdProp3N.Value[5];
                    double bldSpn31 = bldNdProp31.Value[0];
                    double bldSpn3N = bldNdProp3N.Value[0] - bldSpn31;

                    IIsmNode ismNodeBlade3;
                    ismNodeBlade3 = ismModel.AddNode(null);
                    ismNodeBlade3.Name = "BLDND3";
                    ismNodeBlade3.Location = ismApi.GeometryApi.NewPoint3D(
                        ismNodeRotor1.Location.X + bldChord31 / 2.0,
                        ismNodeRotor1.Location.Y - bldSpn3N * Math.Cos(Math.PI * 30.0 / 180.0),
                        ismNodeRotor1.Location.Z - bldSpn3N * Math.Sin(Math.PI * 30.0 / 180.0));

                    IIsmParametricSection ismSectBlade31;
                    ismSectBlade31 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
                    ismSectBlade31.SetSolidRectangleDimensions(0.2, bldChord31);
                    ismSectBlade31.Name = "BLDSECT31";

                    IIsmParametricSection ismSectBlade32;
                    ismSectBlade32 = ismModel.AddParametricSection(null, IsmParametricShapeKind.SolidRectangle);
                    ismSectBlade32.SetSolidRectangleDimensions(0.2, bldChord3N);
                    ismSectBlade32.Name = "BLDSECT32";

                    IIsmVaryingSection ismVarySectBlade3;
                    ismVarySectBlade3 = ismModel.AddVaryingSection(null);
                    ismVarySectBlade3.Name = "BLDVSECT3";

                    IIsmVaryingSectionSegment ismSegmentBlade3;
                    ismSegmentBlade3 = ismVarySectBlade3.AddSegment();
                    ismSegmentBlade3.Name = "BLDSEG3";
                    ismSegmentBlade3.StartSection = ismSectBlade31;
                    ismSegmentBlade3.EndSection = ismSectBlade32;

                    IIsmCurveMember ismBeamBlade3;
                    ismBeamBlade3 = ismModel.AddCurveMember(null);
                    ismBeamBlade3.Name = "BLDBEAM3";

                    IIsmPoint3D startPointBlade3 = ismApi.GeometryApi.NewPoint3D(ismNodeBlade0.Location.X, ismNodeBlade0.Location.Y, ismNodeBlade0.Location.Z);
                    IIsmPoint3D endPointBlade3 = ismApi.GeometryApi.NewPoint3D(ismNodeBlade3.Location.X, ismNodeBlade3.Location.Y, ismNodeBlade3.Location.Z);
                    ismBeamBlade3.Location = ismApi.GeometryApi.NewLineSegment3D(startPointBlade3, endPointBlade3);
                    ismNodeBlade0.AddMember(ismBeamBlade3);
                    ismNodeBlade3.AddMember(ismBeamBlade3);

                    ismBeamBlade3.Section = ismVarySectBlade3;
                    ismBeamBlade3.Use = IsmCurveMemberUse.Column;
                    ismBeamBlade3.SystemKind = IsmCurveMemberSystemKind.SteelRolled;
                    ismBeamBlade3.PlacementPoint = IsmSectionPlacementPoint.Centroid;
                    ismBeamBlade3.LoadResistance = IsmLoadResistance.GravityAndLateral;
                    ismBeamBlade3.MirrorShapeAboutYAxis = false;

                    x = 0;
                    y = 0;
                    z = 1;
                    if ((Math.Abs(startPointBlade3.X - endPointBlade3.X) < 0.000001) && (Math.Abs(startPointBlade3.Y - endPointBlade3.Y) < 0.000001) && (Math.Abs(startPointBlade3.Z - endPointBlade3.Z) > 0.000001))
                    {
                        x = 1;
                        y = 0;
                        z = 0;
                    }
                    ismBeamBlade3.Orientation = ismApi.GeometryApi.NewPoint3D(x, y, z);

                }

            }
            else
            {
                //ED Model is null. Add error information that blades can't be visualized.
                return;
            }


        }

        private static void ConvertEDToIsm(EDModel edModel, IIsmApi ismApi, ref IIsmModel ismModel, ref string status)
        {
            
        }
    }
}
