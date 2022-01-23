using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace ConvertFast
{
    public static class ToSacs
    {
        public static void ConvertToSacs(FstModel fstModel, ref string status)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(fstModel.fstFile);
            string sacsFileName = fstModel.filePath + "\\" + fileName + ".py";
            var sacsFile = new StreamWriter(sacsFileName);

            sacsFile.WriteLine("import SACS");
            sacsFile.WriteLine("model = SACS.Model(1)");

            if (fstModel.convertHydro)
            {
                ConvertHDToSacs(fstModel.hdModel, sacsFile, ref status);
            }
            if (fstModel.convertSub)
            {
                ConvertSDToSacs(fstModel.sdModel, sacsFile, ref status);
            }
            if (fstModel.convertMooring)
            {
                ConvertMDToSacs(fstModel.mdModel, sacsFile, ref status);
            }
            if (fstModel.convertAero)
            {
                ConvertADToSacs(fstModel.adModel, fstModel.edModel, sacsFile, ref status);
            }
            if (fstModel.convertElast)
            {
                ConvertEDToSacs(fstModel.edModel, sacsFile, ref status);
            }



            sacsFile.WriteLine("model.SaveAs(r\"" + fstModel.filePath + "\\" + fileName + ".inp\")");
            sacsFile.Close();

            status = "Good";

        }

        private static void ConvertHDToSacs(HDModel hdModel, StreamWriter sacsFile, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in hdModel.jointList)
            {
                string jointID = "id=\"H" + entry.Key.ToString() + "\"";
                string jointCoord = "coord=(" + entry.Value.X.ToString() + "," + entry.Value.Y.ToString() + "," + entry.Value.Z.ToString() + ")";
                sacsFile.WriteLine("j" + entry.Key.ToString() + " = " + "model.AddJoint(" + jointID + "," + jointCoord + ")");


                //string[] jointValues = new string[] { "JOINT", entry.Key.ToString(), entry.Value.X.ToString(), entry.Value.Y.ToString(), entry.Value.Z.ToString()};
                //string oneLine = string.Join(' ', jointValues);
            }

            sacsFile.WriteLine("");

            foreach (KeyValuePair<int, List<double>> entry in hdModel.sectionList)
            {
                string sectionID = "hg" + entry.Key.ToString();
                double diameterInCm = entry.Value[0] * 100.0;
                double thicknessInCm = entry.Value[1] * 100.0;
                sacsFile.WriteLine(sectionID + " = model.AddMemberGroup(\"HG" + entry.Key.ToString() + "\")");
                sacsFile.WriteLine(sectionID + ".AddSegment(Tube={\'OD\':" + diameterInCm.ToString() + ", \"T\":" + thicknessInCm.ToString() + "})");
            }

            foreach (KeyValuePair<int, List<int>> entry in hdModel.memberList)
            {
                sacsFile.WriteLine("model.AddMember(" + "j" + entry.Value[0].ToString() + ", j" + entry.Value[1].ToString() + ", " + "hg" + entry.Value[2].ToString() + ")");
            }
        }

        private static void ConvertSDToSacs(SDModel sdModel, StreamWriter sacsFile, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in sdModel.jointList)
            {
                string jointID = "id=\"S" + entry.Key.ToString() + "\"";
                string jointCoord = "coord=(" + entry.Value.X.ToString() + "," + entry.Value.Y.ToString() + "," + entry.Value.Z.ToString() + ")";
                sacsFile.WriteLine("j" + entry.Key.ToString() + " = " + "model.AddJoint(" + jointID + "," + jointCoord + ")");


                //string[] jointValues = new string[] { "JOINT", entry.Key.ToString(), entry.Value.X.ToString(), entry.Value.Y.ToString(), entry.Value.Z.ToString()};
                //string oneLine = string.Join(' ', jointValues);
            }

            sacsFile.WriteLine("");

            foreach (KeyValuePair<int, List<double>> entry in sdModel.sectionList)
            {
                string sectionID = "sg" + entry.Key.ToString();
                double diameterInCm = entry.Value[0] * 100.0;
                double thicknessInCm = entry.Value[1] * 100.0;
                sacsFile.WriteLine(sectionID + " = model.AddMemberGroup(\"SG" + entry.Key.ToString() + "\")");
                sacsFile.WriteLine(sectionID + ".AddSegment(Tube={\'OD\':" + diameterInCm.ToString() + ", \'T\':" + thicknessInCm.ToString() + "})");
            }

            foreach (KeyValuePair<int, List<int>> entry in sdModel.memberList)
            {
                sacsFile.WriteLine("model.AddMember(" + "j" + entry.Value[0].ToString() + ", j" + entry.Value[1].ToString() + ", " + "sg" + entry.Value[2].ToString() + ")");
            }
        }

        private static void ConvertMDToSacs(MDModel mdModel, StreamWriter sacsFile, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in mdModel.connectList)
            {
                string connectID = "id=\"" + "C" + entry.Key.ToString() + "\"";
                string connectCoord = "coord=(" + entry.Value.X.ToString() + "," + entry.Value.Y.ToString() + "," + entry.Value.Z.ToString() + ")";
                sacsFile.WriteLine("c" + entry.Key.ToString() + " = " + "model.AddJoint(" + connectID + "," + connectCoord + ")");
            }

            //fake member group for mooring lines
            string sectionID = "mlGroup";
            sacsFile.WriteLine(sectionID + " = model.AddMemberGroup(\"MLG" + "\")");
            sacsFile.WriteLine(sectionID + ".AddSegment(Tube={\'OD\':" + "10" + ", \'T\':" + "1" + "})");

            foreach (KeyValuePair<int, List<int>> entry in mdModel.lineNodeList)
            {
                sacsFile.WriteLine("model.AddMember(" + "c" + entry.Value[0].ToString() + ", c" + entry.Value[1].ToString() + ", " + "mlGroup" + ")");
            }

        }


        private static void ConvertADToSacs(ADModel adModel, EDModel edModel, StreamWriter sacsFile, ref string status)
        {
            foreach (KeyValuePair<int, List<double>> entry in adModel.twrNodeList)
            {
                string twrNodeID = "id=\"" + "T" + entry.Key.ToString() + "\"";
                string twrNodeCoord = "coord=(" + "0.0" + "," + "0.0" + "," + entry.Value[0].ToString() + ")";
                sacsFile.WriteLine("t" + entry.Key.ToString() + " = " + "model.AddJoint(" + twrNodeID + "," + twrNodeCoord + ")");
            }


            for (int i = 0; i < adModel.twrNodeList.Count - 1; i++)
            {
                var item1 = adModel.twrNodeList.ElementAt(i);
                var item2 = adModel.twrNodeList.ElementAt(i + 1);

                double diameterInCm1 = item1.Value[1] * 100.0;
                double diameterInCm2 = item2.Value[1] * 100.0;

                string groupID = "tg" + item1.Key.ToString();
                sacsFile.WriteLine(groupID + " = model.AddMemberGroup(\"T" + item1.Key.ToString() + "\")");
                sacsFile.WriteLine(groupID + ".AddSegment(TaperOption = SACS.TaperOption.Begin, " 
                                                           +"StartTube = {\'OD\':" + diameterInCm1.ToString() + ", \'T\':" + 10.0.ToString() + "}," 
                                                           + "EndTube = {\'OD\':" + diameterInCm2.ToString() + ", \'T\':" + 10.0.ToString() + "})");
                sacsFile.WriteLine("model.AddMember(" + "t" + item1.Key.ToString() + ", t" + item2.Key.ToString() + ", " + groupID + ")");

            }


            ConvertADBladesToSacs(adModel, edModel, sacsFile, ref status);

        }

        private static void ConvertADBladesToSacs(ADModel adModel, EDModel edModel, StreamWriter sacsFile, ref string status)
        {
            if (edModel != null)
            {
                var twrTopNode = adModel.twrNodeList.ElementAt(adModel.twrNodeList.Count - 1);

                string rotorNodeID1 = "id=\"RTR1\"";
                string rotorCoord1 = "coord=(" + edModel.NacCMxn.ToString() + "," + "0.0" + "," + (twrTopNode.Value[0] + edModel.NacCMzn).ToString() + ")";
                sacsFile.WriteLine("rotorNd1 = model.AddJoint(" + rotorNodeID1 + "," + rotorCoord1 + ")");

                string rotorNodeID2 = "id=\"RTR2\"";
                string rotorCoord2 = "coord=(" + (-twrTopNode.Value[1]).ToString() + "," + "0.0" + "," + (twrTopNode.Value[0] + edModel.NacCMzn).ToString() + ")";
                sacsFile.WriteLine("rotorNd2 = model.AddJoint(" + rotorNodeID2 + "," + rotorCoord2 + ")");

                string groupID = "rotorGroup";
                sacsFile.WriteLine(groupID + " = model.AddMemberGroup(\"RTRG" + "\")");
                sacsFile.WriteLine(groupID + ".AddSegment(Tube={\'OD\':" + (edModel.HubRad*2.0*100.0) + ", \'T\':" + "10" + "})");
                sacsFile.WriteLine("model.AddMember(" + "rotorNd1" + ", rotorNd2" + ", " + groupID + ")");

                sacsFile.WriteLine("");

                if (edModel.NumBl == 3)
                {
                    //Blade_1
                    var bldNdProp11 = adModel.adBldProp1.ElementAt(0);
                    var bldNdProp1N = adModel.adBldProp1.ElementAt(adModel.adBldProp1.Count - 1);
                    double bldChord11 = bldNdProp11.Value[5];
                    double bldChord1N = bldNdProp1N.Value[5];
                    double bldSpn11 = bldNdProp11.Value[0];
                    double bldSpn1N = bldNdProp1N.Value[0] - bldSpn11;

                    string bldNode0 = "id=\"BLD0\"";
                    string bldCoord0 = "coord=(" + (edModel.NacCMxn + bldChord11 / 2.0).ToString() + "," + "0.0" + "," + (twrTopNode.Value[0] + edModel.NacCMzn).ToString() + ")";
                    sacsFile.WriteLine("bldNd0 = model.AddJoint(" + bldNode0 + "," + bldCoord0 + ")");

                    string bldNode1 = "id=\"BLD1\"";
                    string bldCoord1 = "coord=(" + (edModel.NacCMxn + bldChord11 / 2.0).ToString() + "," + "0.0" + "," + (twrTopNode.Value[0] + edModel.NacCMzn + bldSpn1N).ToString() + ")";
                    sacsFile.WriteLine("bldNd1 = model.AddJoint(" + bldNode1 + "," + bldCoord1 + ")");

                    //fake member group for blades
                    string bldGroupID = "bldGrp";
                    sacsFile.WriteLine(bldGroupID + " = model.AddMemberGroup(\"BLDG" + "\")");
                    //sacsFile.WriteLine(bldGroupID + ".AddSegment(Tube={\'OD\':" + "10" + ", \'T\':" + "1" + "})");
                    sacsFile.WriteLine("model.AddMember(" + "bldNd0" + ", bldNd1" + ", " + bldGroupID + ")");

                    //Blade_2
                    var bldNdProp21 = adModel.adBldProp2.ElementAt(0);
                    var bldNdProp2N = adModel.adBldProp2.ElementAt(adModel.adBldProp2.Count - 1);
                    double bldChord21 = bldNdProp21.Value[5];
                    double bldChord2N = bldNdProp2N.Value[5];
                    double bldSpn21 = bldNdProp21.Value[0];
                    double bldSpn2N = bldNdProp2N.Value[0] - bldSpn21;

                    string bldNode2 = "id=\"BLD2\"";
                    string bldCoord2 = "coord=(" 
                        + (edModel.NacCMxn + bldChord21 / 2.0).ToString() + "," 
                        + (bldSpn2N * Math.Cos(Math.PI * 30.0 / 180.0)).ToString() + "," 
                        + (twrTopNode.Value[0] + edModel.NacCMzn - bldSpn2N * Math.Sin(Math.PI * 30.0 / 180.0)).ToString() + ")";
                    sacsFile.WriteLine("bldNd2 = model.AddJoint(" + bldNode2 + "," + bldCoord2 + ")");

                    sacsFile.WriteLine("model.AddMember(" + "bldNd0" + ", bldNd2" + ", " + bldGroupID + ")");

                    //Blade_3
                    var bldNdProp31 = adModel.adBldProp3.ElementAt(0);
                    var bldNdProp3N = adModel.adBldProp3.ElementAt(adModel.adBldProp3.Count - 1);
                    double bldChord31 = bldNdProp31.Value[5];
                    double bldChord3N = bldNdProp3N.Value[5];
                    double bldSpn31 = bldNdProp31.Value[0];
                    double bldSpn3N = bldNdProp3N.Value[0] - bldSpn31;

                    string bldNode3 = "id=\"BLD3\"";
                    string bldCoord3 = "coord=(" 
                        + (edModel.NacCMxn + bldChord31 / 2.0).ToString() + ","
                        + (-bldSpn3N * Math.Cos(Math.PI * 30.0 / 180.0)).ToString() + ","
                        + (twrTopNode.Value[0] + edModel.NacCMzn - bldSpn3N * Math.Sin(Math.PI * 30.0 / 180.0)).ToString() + ")";
                    sacsFile.WriteLine("bldNd3 = model.AddJoint(" + bldNode3 + "," + bldCoord3 + ")");

                    sacsFile.WriteLine("model.AddMember(" + "bldNd0" + ", bldNd3" + ", " + bldGroupID + ")");

                }

            }
            else
            {
                //ED Model is null. Add error information that blades can't be visualized.
                return;
            }

        }



        private static void ConvertEDToSacs(EDModel edModel, StreamWriter sacsFile, ref string status)
        {

        }
    }
}
