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
            if (fstModel.convertElast)
            {
                //add code
            }
            if (fstModel.convertAero)
            {
                //add code
            }


            sacsFile.WriteLine("model.SaveAs(r\"" + fstModel.filePath + "\\" + fileName + ".inp\")");
            sacsFile.Close();

            status = "Good";

        }

        private static void ConvertHDToSacs(HDModel hdModel, StreamWriter sacsFile, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in hdModel.jointList)
            {
                string jointID = "id=\"" + entry.Key.ToString() + "\"";
                string jointCoord = "coord=(" + entry.Value.X.ToString() + "," + entry.Value.Y.ToString() + "," + entry.Value.Z.ToString() + ")";
                sacsFile.WriteLine("j" + entry.Key.ToString() + " = " + "model.AddJoint(" + jointID + "," + jointCoord + ")");


                //string[] jointValues = new string[] { "JOINT", entry.Key.ToString(), entry.Value.X.ToString(), entry.Value.Y.ToString(), entry.Value.Z.ToString()};
                //string oneLine = string.Join(' ', jointValues);
            }

            sacsFile.WriteLine("");

            foreach (KeyValuePair<int, List<double>> entry in hdModel.sectionList)
            {
                string sectionID = "mg" + entry.Key.ToString();
                double diameterInCm = entry.Value[0] * 100.0;
                double thicknessInCm = entry.Value[1] * 100.0;
                sacsFile.WriteLine(sectionID + " = model.AddMemberGroup(\"MG" + entry.Key.ToString() + "\")");
                sacsFile.WriteLine(sectionID + ".AddSegment(Tube={\'OD\':" + diameterInCm.ToString() + ", \"T\":" + thicknessInCm.ToString() + "})");
            }

            foreach (KeyValuePair<int, List<int>> entry in hdModel.memberList)
            {
                sacsFile.WriteLine("model.AddMember(" + "j" + entry.Value[0].ToString() + ", j" + entry.Value[1].ToString() + ", " + "mg" + entry.Value[2].ToString() + ")");
            }
        }

        private static void ConvertSDToSacs(SDModel sdModel, StreamWriter sacsFile, ref string status)
        {
            foreach (KeyValuePair<int, Vector3> entry in sdModel.jointList)
            {
                string jointID = "id=\"" + entry.Key.ToString() + "\"";
                string jointCoord = "coord=(" + entry.Value.X.ToString() + "," + entry.Value.Y.ToString() + "," + entry.Value.Z.ToString() + ")";
                sacsFile.WriteLine("j" + entry.Key.ToString() + " = " + "model.AddJoint(" + jointID + "," + jointCoord + ")");


                //string[] jointValues = new string[] { "JOINT", entry.Key.ToString(), entry.Value.X.ToString(), entry.Value.Y.ToString(), entry.Value.Z.ToString()};
                //string oneLine = string.Join(' ', jointValues);
            }

            sacsFile.WriteLine("");

            foreach (KeyValuePair<int, List<double>> entry in sdModel.sectionList)
            {
                string sectionID = "mg" + entry.Key.ToString();
                double diameterInCm = entry.Value[0] * 100.0;
                double thicknessInCm = entry.Value[1] * 100.0;
                sacsFile.WriteLine(sectionID + " = model.AddMemberGroup(\"MG" + entry.Key.ToString() + "\")");
                sacsFile.WriteLine(sectionID + ".AddSegment(Tube={\'OD\':" + diameterInCm.ToString() + ", \"T\":" + thicknessInCm.ToString() + "})");
            }

            foreach (KeyValuePair<int, List<int>> entry in sdModel.memberList)
            {
                sacsFile.WriteLine("model.AddMember(" + "j" + entry.Value[0].ToString() + ", j" + entry.Value[1].ToString() + ", " + "mg" + entry.Value[2].ToString() + ")");
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
            sacsFile.WriteLine(sectionID + ".AddSegment(Tube={\'OD\':" + "10" + ", \"T\":" + "1" + "})");

            foreach (KeyValuePair<int, List<int>> entry in mdModel.lineNodeList)
            {
                sacsFile.WriteLine("model.AddMember(" + "c" + entry.Value[0].ToString() + ", c" + entry.Value[1].ToString() + ", " + "mlGroup" + ")");
            }

        }
    }
}
