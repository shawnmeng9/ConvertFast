using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace ConvertFast
{
    public class FstModel
    {
        public HDModel hdModel { get; set; }
        public SDModel sdModel { get; set; }
        public MDModel mdModel { get; set; }

        public string fstFile { get; set; }
        public string filePath { get; set; }
        public string EDFile { get; set; }
        public string HydroFile { get; set; }
        public string AeroFile { get; set; }
        public string MooringFile { get; set; }
        public string SubFile { get; set; }

        public int? CompElast { get; set; }
        public int? CompAero { get; set; }
        public int? CompHydro { get; set; }
        public int? CompMooring { get; set; }
        public int? CompSub { get; set; }

        public bool convertElast { get; set; }
        public bool convertAero { get; set; }
        public bool convertHydro { get; set; }
        public bool convertSub { get; set; }
        public bool convertMooring { get; set; }

        public FstModel()
        {
            hdModel = new HDModel();
            sdModel = new SDModel();
            mdModel = new MDModel();

            convertElast = false;
            convertAero = false;
            convertHydro = false;
            convertSub = false;
            convertMooring = false;
        }


        public void ParseFstInputFile(string fileName_fst, string status)
        {
            fstFile = fileName_fst;
            var lines = File.ReadAllLines(fstFile);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] oneInput = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                if (oneInput.Length >= 2 && oneInput[1] == "CompElast")
                {
                    CompElast = Int32.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "CompAero")
                {
                    CompAero = Int32.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "CompHydro")
                {
                    CompHydro = Int32.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "CompMooring")
                {
                    CompMooring = Int32.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "CompSub")
                {
                    CompSub = Int32.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "EDFile")
                {
                    EDFile = oneInput[0];
                    EDFile = filePath + "\\" + EDFile.Replace("\"", "");
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "HydroFile")
                {
                    HydroFile = oneInput[0];
                    HydroFile = filePath + "\\" + HydroFile.Replace("\"", "");
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "AeroFile")
                {
                    AeroFile = oneInput[0];
                    AeroFile = filePath + "\\" + AeroFile.Replace("\"", "");
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "MooringFile")
                {
                    MooringFile = oneInput[0];
                    MooringFile = filePath + "\\" + MooringFile.Replace("\"", "");
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "SubFile")
                {
                    SubFile = oneInput[0];
                    SubFile = filePath + "\\" + SubFile.Replace("\"", "");
                }
                else
                {
                    //add more
                }
            }

        }

    }

    public class HDModel
    {
        int NJoints = 0;
        int NMembers = 0;
        int NPropSets = 0;
        public IDictionary<int, Vector3> jointList = new Dictionary<int, Vector3> { };
        public IDictionary<int, int[]> memberList = new Dictionary<int, int[]> { };
        public IDictionary<int, double[]> sectionList = new Dictionary<int, double[]> { };
        public string hdFile = "";

        public HDModel()
        {
            jointList = new Dictionary<int, Vector3> { };
            memberList = new Dictionary<int, int[]> { };
            sectionList = new Dictionary<int, double[]> { };
            hdFile = "";
        }

        public void ParseHDInputFile(string fileName_HD, string status)
        {
            hdFile = fileName_HD;
            var lines = File.ReadAllLines(hdFile);
            bool isJoint = false;
            int jointLineNo = 0;
            bool isMember = false;
            int memberLineNo = 0;
            bool isSection = false;
            int sectionLineNo = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] oneInput = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (oneInput.Length >= 2 && oneInput[1] == "NJoints")
                {
                    NJoints = Int32.Parse(oneInput[0]);
                    isJoint = true;
                    continue;
                }

                if (isJoint)
                {
                    if (jointLineNo < 2)
                    {
                        jointLineNo++;
                        continue;
                    }
                    else
                    {
                        int jointID = Int32.Parse(oneInput[0]);
                        float jointX = float.Parse(oneInput[1]);
                        float jointY = float.Parse(oneInput[2]);
                        float jointZ = float.Parse(oneInput[3]);
                        Vector3 jointCoord = new Vector3(jointX, jointY, jointZ);

                        jointList.Add(jointID, jointCoord);
                        jointLineNo++;


                        status = jointID.ToString() + " " + jointX.ToString() + " " + jointY.ToString() + " " + jointZ.ToString();

                        if (jointLineNo == NJoints + 2)
                        {
                            isJoint = false;
                        }
                    }
                }

                if (oneInput.Length >= 2 && oneInput[1] == "NMembers")
                {
                    NMembers = Int32.Parse(oneInput[0]);
                    isMember = true;
                    continue;
                }

                if (isMember)
                {
                    if (memberLineNo < 2)
                    {
                        memberLineNo++;
                        continue;
                    }
                    else
                    {
                        int memberID = Int32.Parse(oneInput[0]);
                        int jointID1 = Int32.Parse(oneInput[1]);
                        int jointID2 = Int32.Parse(oneInput[2]);
                        int sectID1 = Int32.Parse(oneInput[3]);
                        int sectID2 = Int32.Parse(oneInput[4]);

                        int[] memberProp = new int[] { jointID1, jointID2, sectID1, sectID2 };

                        memberList.Add(memberID, memberProp);
                        memberLineNo++;

                        status = memberID.ToString() + " " + jointID1.ToString() + " " + sectID1.ToString();

                        if (memberLineNo == NMembers + 2)
                        {
                            isMember = false;
                        }
                    }
                }


                if (oneInput.Length >= 2 && oneInput[1] == "NPropSets")
                {
                    NPropSets = Int32.Parse(oneInput[0]);
                    isSection = true;
                    continue;
                }

                if (isSection)
                {
                    if (sectionLineNo < 2)
                    {
                        sectionLineNo++;
                        continue;
                    }
                    else
                    {
                        int sectionID = Int32.Parse(oneInput[0]);
                        double diameter = double.Parse(oneInput[1]);
                        double thickness = double.Parse(oneInput[2]);

                        double[] sectionProp = new double[] { diameter, thickness };

                        sectionList.Add(sectionID, sectionProp);
                        sectionLineNo++;

                        status = sectionID.ToString() + " " + diameter.ToString() + " " + thickness.ToString();

                        if (sectionLineNo == NPropSets + 2)
                        {
                            isSection = false;
                        }
                    }
                }
            }
        }
    }

    public class SDModel
    {
        int NJoints = 0;
        int NMembers = 0;
        int NPropSets = 0;
        public IDictionary<int, Vector3> jointList = new Dictionary<int, Vector3> { };
        public IDictionary<int, int[]> memberList = new Dictionary<int, int[]> { };
        public IDictionary<int, double[]> sectionList = new Dictionary<int, double[]> { };
        public string sdFile = "";

        public SDModel()
        {

        }

        public void ParseSDInputFile(string fileName_SD, string status)
        {
            sdFile = fileName_SD;
            var lines = File.ReadAllLines(sdFile);
            bool isJoint = false;
            int jointLineNo = 0;
            bool isMember = false;
            int memberLineNo = 0;
            bool isPropSet = false;
            int propSetLineNo = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] oneInput = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (oneInput.Length >= 2 && oneInput[1] == "NJoints")
                {
                    NJoints = Int32.Parse(oneInput[0]);
                    isJoint = true;
                    continue;
                }

                if (isJoint)
                {
                    if (jointLineNo < 2)
                    {
                        jointLineNo++;
                        continue;
                    }
                    else
                    {
                        int jointID = Int32.Parse(oneInput[0]);
                        float jointX = float.Parse(oneInput[1]);
                        float jointY = float.Parse(oneInput[2]);
                        float jointZ = float.Parse(oneInput[3]);
                        Vector3 jointCoord = new Vector3(jointX, jointY, jointZ);

                        jointList.Add(jointID, jointCoord);
                        jointLineNo++;

                        status = jointID.ToString() + " " + jointX.ToString() + " " + jointY.ToString() + " " + jointZ.ToString();

                        if (jointLineNo == NJoints + 2)
                        {
                            isJoint = false;
                        }
                    }
                }

                if (oneInput.Length >= 2 && oneInput[1] == "NMembers")
                {
                    NMembers = Int32.Parse(oneInput[0]);
                    isMember = true;
                    continue;
                }

                if (isMember)
                {
                    if (memberLineNo < 2)
                    {
                        memberLineNo++;
                        continue;
                    }
                    else
                    {
                        int memberID = Int32.Parse(oneInput[0]);
                        int jointID1 = Int32.Parse(oneInput[1]);
                        int jointID2 = Int32.Parse(oneInput[2]);
                        int sectID1 = Int32.Parse(oneInput[3]);
                        int sectID2 = Int32.Parse(oneInput[4]);

                        int[] memberProp = new int[] { jointID1, jointID2, sectID1, sectID2 };

                        memberList.Add(memberID, memberProp);
                        memberLineNo++;

                        status = memberID.ToString() + " " + jointID1.ToString() + " " + sectID1.ToString();

                        if (memberLineNo == NMembers + 2)
                        {
                            isMember = false;
                        }
                    }
                }


                if (oneInput.Length >= 2 && oneInput[1] == "NPropSets")
                {
                    NPropSets = Int32.Parse(oneInput[0]);
                    isPropSet = true;
                    continue;
                }

                if (isPropSet)
                {
                    if (propSetLineNo < 2)
                    {
                        propSetLineNo++;
                        continue;
                    }
                    else
                    {
                        int sectionID = Int32.Parse(oneInput[0]);
                        double diameter = double.Parse(oneInput[4]);
                        double thickness = double.Parse(oneInput[5]);

                        double[] sectionProp = new double[] { diameter, thickness };

                        sectionList.Add(sectionID, sectionProp);
                        propSetLineNo++;

                        status = sectionID.ToString() + " " + diameter.ToString() + " " + thickness.ToString();

                        if (propSetLineNo == NPropSets + 2)
                        {
                            isPropSet = false;
                        }
                    }
                }

            }

        }
    }

    public class MDModel
    {
        public enum ConnectType
        {
            None,
            Fixed,
            Vessel
        }

        int NConnects = 0;
        public IDictionary<int, Vector3> connectList = new Dictionary<int, Vector3> { };
        public IDictionary<int, Vector3> connectFixedList = new Dictionary<int, Vector3> { };
        public IDictionary<int, Vector3> connectVesselList = new Dictionary<int, Vector3> { };
        int NLines = 0;
        public IDictionary<int, int[]> lineNodeList = new Dictionary<int, int[]> { };

        public string mdFile = "";

        public MDModel()
        {
            NConnects = 0;
            connectList = new Dictionary<int, Vector3> { };
            connectFixedList = new Dictionary<int, Vector3> { };
            connectVesselList = new Dictionary<int, Vector3> { };
            NLines = 0;
            lineNodeList = new Dictionary<int, int[]> { };
        }

        public void ParseMDInputFile(string fileName_MD, string status)
        {
            mdFile = fileName_MD;
            var lines = File.ReadAllLines(mdFile);
            bool isConnect = false;
            int connectLineNo = 0;
            bool isLine = false;
            int lineLineNo = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] oneInput = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                if (oneInput.Length >= 2 && oneInput[1] == "NConnects")
                {
                    NConnects = Int32.Parse(oneInput[0]);
                    isConnect = true;
                    continue;
                }

                if (isConnect)
                {
                    if (connectLineNo < 2)
                    {
                        connectLineNo++;
                        continue;
                    }
                    else
                    {
                        int connectID = Int32.Parse(oneInput[0]);
                        ConnectType connectType = GetConnectType(oneInput[1]);
                        float connectX = float.Parse(oneInput[2]);
                        float connectY = float.Parse(oneInput[3]);
                        float connectZ = float.Parse(oneInput[4]);
                        Vector3 connectCoord = new Vector3(connectX, connectY, connectZ);

                        connectList.Add(connectID, connectCoord);
                        if (connectType == ConnectType.Fixed)
                        {
                            connectFixedList.Add(connectID, connectCoord);
                        }
                        if (connectType == ConnectType.Vessel)
                        {
                            connectVesselList.Add(connectID, connectCoord);
                        }
                        connectLineNo++;

                        status = connectID.ToString() + " " + connectX.ToString() + " " + connectY.ToString() + " " + connectZ.ToString();

                        if (connectLineNo == NConnects + 2)
                        {
                            isConnect = false;
                        }
                    }
                }

                if (oneInput.Length >= 2 && oneInput[1] == "NLines")
                {
                    NLines = Int32.Parse(oneInput[0]);
                    isLine = true;
                    continue;
                }

                if (isLine)
                {
                    if (lineLineNo < 2)
                    {
                        lineLineNo++;
                        continue;
                    }
                    else
                    {
                        int lineID = Int32.Parse(oneInput[0]);
                        int lineNodeAnch = Int32.Parse(oneInput[4]);
                        int lineNodeFair = Int32.Parse(oneInput[5]);

                        int[] lineNodes = new int[] { lineNodeAnch, lineNodeFair };
                        lineNodeList.Add(lineID, lineNodes);

                        lineLineNo++;

                        status = lineID.ToString() + " " + lineNodeAnch.ToString() + " " + lineNodeFair.ToString();

                        if (lineLineNo == NLines + 2)
                        {
                            isLine = false;
                        }
                    }
                }



            }

        }



        private ConnectType GetConnectType(string strType)
        {
            ConnectType connectType = ConnectType.None;
            if (strType == "Fixed")
            {
                connectType = ConnectType.Fixed;
            }
            else if (strType == "Vessel")
            {
                connectType = ConnectType.Vessel;
            }
            else
            {
                //error, not supported yet
            }
            return connectType;
        }





    }



}
