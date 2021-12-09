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
        public static void ConvertHDToSacs(HDModel hdModel, ref string status)
        {
            string filePath = System.IO.Path.GetDirectoryName(hdModel.hdFile);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(hdModel.hdFile);
            string sacsFileName = filePath + "\\" + fileName + ".py";
            var sacsFile = new StreamWriter(sacsFileName);

            sacsFile.WriteLine("import SACS");
            sacsFile.WriteLine("model = SACS.Model(1)");

            foreach (KeyValuePair<int, Vector3> entry in hdModel.jointList)
            {
                string jointID = "id=\"" + entry.Key.ToString() + "\"";
                string jointCoord = "coord=(" + entry.Value.X.ToString() + "," + entry.Value.Y.ToString() + "," + entry.Value.Z.ToString() + ")";
                sacsFile.WriteLine("j" + entry.Key.ToString() + " = " + "model.AddJoint(" + jointID + "," + jointCoord + ")");


                //string[] jointValues = new string[] { "JOINT", entry.Key.ToString(), entry.Value.X.ToString(), entry.Value.Y.ToString(), entry.Value.Z.ToString()};
                //string oneLine = string.Join(' ', jointValues);
            }


            sacsFile.WriteLine("");

            foreach (KeyValuePair<int, double[]> entry in hdModel.sectionList)
            {
                string sectionID = "mg" + entry.Key.ToString();
                double diameterInCm = entry.Value[0] * 100.0;
                double thicknessInCm = entry.Value[1] * 100.0;
                sacsFile.WriteLine(sectionID + " = model.AddMemberGroup(\"MG" + entry.Key.ToString() + "\")");
                sacsFile.WriteLine(sectionID + ".AddSegment(Tube={\'OD\':" + diameterInCm.ToString() + ", \"T\":" + thicknessInCm.ToString() + "})");
            }

            foreach (KeyValuePair<int, int[]> entry in hdModel.memberList)
            {
                sacsFile.WriteLine("model.AddMember(" + "j" + entry.Value[0].ToString() + ", j" + entry.Value[1].ToString() + ", " + "mg" + entry.Value[2].ToString() + ")");
            }

            sacsFile.WriteLine("model.SaveAs(r\"" + filePath + "\\" + fileName + ".inp\")");
            sacsFile.Close();

            status = "Good";
        }


    }
}
