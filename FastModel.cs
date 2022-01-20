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
        public ADModel adModel { get; set; }
        public EDModel edModel { get; set; }

        public string fstFile { get; set; }
        public string filePath { get; set; }
        public string EDFile { get; set; }
        public string HydroFile { get; set; }
        public string AeroFile { get; set; }
        public string MooringFile { get; set; }
        public string SubFile { get; set; }
        public string ServoFile { get; set; }

        public int? CompElast { get; set; }
        public int? CompAero { get; set; }
        public int? CompHydro { get; set; }
        public int? CompMooring { get; set; }
        public int? CompSub { get; set; }
        public int? CompServo { get; set; }

        public bool convertElast { get; set; }
        public bool convertAero { get; set; }
        public bool convertHydro { get; set; }
        public bool convertSub { get; set; }
        public bool convertMooring { get; set; }
        public bool convertServo { get; set; }

        public FstModel()
        {
            hdModel = new HDModel();
            sdModel = new SDModel();
            mdModel = new MDModel();
            adModel = new ADModel();

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
                else if (oneInput.Length >= 2 && oneInput[1] == "CompServo")
                {
                    CompServo = Int32.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "EDFile")
                {
                    EDFile = oneInput[0];
                    EDFile = filePath + "\\" + EDFile.Replace("\"", "");
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "HydroFile")
                {
                    HydroFile = oneInput[0];
                    //HydroFile = filePath + "\\" + HydroFile.Replace("\"", "");
                    HydroFile = HydroFile.Replace("\"", "");
                    HydroFile = Path.GetFullPath(Path.Combine(filePath, HydroFile));
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "AeroFile")
                {
                    AeroFile = oneInput[0];
                    //AeroFile = filePath + "\\" + AeroFile.Replace("\"", "");
                    AeroFile = AeroFile.Replace("\"", "");
                    AeroFile = Path.GetFullPath(Path.Combine(filePath, AeroFile));
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "MooringFile")
                {
                    MooringFile = oneInput[0];
                    //MooringFile = filePath + "\\" + MooringFile.Replace("\"", "");
                    MooringFile = MooringFile.Replace("\"", "");
                    MooringFile = Path.GetFullPath(Path.Combine(filePath, MooringFile));
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "SubFile")
                {
                    SubFile = oneInput[0];
                    //SubFile = filePath + "\\" + SubFile.Replace("\"", "");
                    SubFile = SubFile.Replace("\"", "");
                    SubFile = Path.GetFullPath(Path.Combine(filePath, SubFile));
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "ServoFile")
                {
                    ServoFile = oneInput[0];
                    //ServoFile = filePath + "\\" + ServoFile.Replace("\"", "");
                    ServoFile = ServoFile.Replace("\"", "");
                    ServoFile = Path.GetFullPath(Path.Combine(filePath, ServoFile));
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
        int NJoints { get; set; }
        int NMembers { get; set; }
        int NPropSets { get; set; }
        public IDictionary<int, Vector3> jointList { get; set; }
        public IDictionary<int, List<int>> memberList { get; set; }
        public IDictionary<int, List<double>> sectionList { get; set; }
        string hdFile { get; set; }

        public HDModel()
        {
            NJoints = 0;
            NMembers = 0;
            NPropSets = 0;
            jointList = new Dictionary<int, Vector3> { };
            memberList = new Dictionary<int, List<int>> { };
            sectionList = new Dictionary<int, List<double>> { };
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

                        var memberProp = new List<int> { jointID1, jointID2, sectID1, sectID2 };

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

                        var sectionProp = new List<double> { diameter, thickness };

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
        int NJoints { get; set; }
        int NMembers { get; set; }
        int NPropSets { get; set; }
        public IDictionary<int, Vector3> jointList { get; set; }
        public IDictionary<int, List<int>> memberList { get; set; }
        public IDictionary<int, List<double>> sectionList { get; set; }
        string sdFile { get; set; }

        public SDModel()
        {
            NJoints = 0;
            NMembers = 0;
            NPropSets = 0;
            jointList = new Dictionary<int, Vector3> { };
            memberList = new Dictionary<int, List<int>> { };
            sectionList = new Dictionary<int, List<double>> { };
            sdFile = "";
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

                        var memberProp = new List<int> { jointID1, jointID2, sectID1, sectID2 };

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

                        var sectionProp = new List<double> { diameter, thickness };

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

        int NConnects { get; set; }
        public IDictionary<int, Vector3> connectList { get; set; }
        public IDictionary<int, Vector3> connectFixedList { get; set; }
        public IDictionary<int, Vector3> connectVesselList { get; set; }
        int NLines { get; set; }
        public IDictionary<int, List<int>> lineNodeList { get; set; }
        string mdFile { get; set; }

        public MDModel()
        {
            NConnects = 0;
            connectList = new Dictionary<int, Vector3> { };
            connectFixedList = new Dictionary<int, Vector3> { };
            connectVesselList = new Dictionary<int, Vector3> { };
            NLines = 0;
            lineNodeList = new Dictionary<int, List<int>> { };
            mdFile = "";
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

                        var lineNodes = new List<int> { lineNodeAnch, lineNodeFair };
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

    public class ADModel
    {
        int NumTwrNds { get; set; }
        public IDictionary<int, List<double>> twrNodeList { get; set; }
        string adFile { get; set; }
        string ADBlFile1 { get; set; }
        string ADBlFile2 { get; set; }
        string ADBlFile3 { get; set; }

        public IDictionary<int, List<double>> adBldProp1 { get; set; }
        public IDictionary<int, List<double>> adBldProp2 { get; set; }
        public IDictionary<int, List<double>> adBldProp3 { get; set; }


        public ADModel()
        {
            NumTwrNds = 0;
            twrNodeList = new Dictionary<int, List<double>> { };
            adFile = "";
            ADBlFile1 = "";
            ADBlFile2 = "";
            ADBlFile3 = "";
            adBldProp1 = new Dictionary<int, List<double>> { };
            adBldProp2 = new Dictionary<int, List<double>> { };
            adBldProp3 = new Dictionary<int, List<double>> { };
        }

        public void ParseADInputFile(string fileName_AD, string status)
        {
            adFile = fileName_AD;
            string filePath = Path.GetDirectoryName(adFile);
            var lines = File.ReadAllLines(adFile);
            bool isTwrNd = false;
            int twrNdLineNo = 0;
            int twrNdID = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] oneInput = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (oneInput.Length >= 2 && oneInput[1] == "NumTwrNds")
                {
                    NumTwrNds = Int32.Parse(oneInput[0]);
                    isTwrNd = true;
                    continue;
                }

                if (isTwrNd)
                {
                    if (twrNdLineNo < 2)
                    {
                        twrNdLineNo++;
                        continue;
                    }
                    else
                    {
                        double TwrElev = 0.0;
                        double TwrDiam = 0.0;
                        var twrNdProp = new List<double> { TwrElev, TwrDiam };
                        if (twrNdID == 0)
                        {
                            double TwrElev0 = 0.0;
                            double TwrDiam0 = double.Parse(oneInput[1]);
                            var twrNdProp0 = new List<double> { TwrElev0, TwrDiam0 };
                            twrNodeList.Add(twrNdID, twrNdProp0);
                            twrNdID++;
                        }

                        TwrElev = double.Parse(oneInput[0]);
                        TwrDiam = double.Parse(oneInput[1]);
                        twrNdProp[0] = TwrElev;
                        twrNdProp[1] = TwrDiam;
                        twrNodeList.Add(twrNdID, twrNdProp);
                        twrNdID++;
                        twrNdLineNo++;

                        if (twrNdLineNo == NumTwrNds + 2)
                        {
                            isTwrNd = false;
                        }
                    }
                }


                if (oneInput.Length >= 2 && oneInput[1] == "ADBlFile(1)")
                {
                    ADBlFile1 = oneInput[0];
                    ADBlFile1 = ADBlFile1.Replace("\"", "");
                    ADBlFile1 = Path.GetFullPath(Path.Combine(filePath, ADBlFile1));
                    adBldProp1 = ParseADBladeFile(ADBlFile1);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "ADBlFile(2)")
                {
                    ADBlFile2 = oneInput[0];
                    ADBlFile2 = ADBlFile2.Replace("\"", "");
                    ADBlFile2 = Path.GetFullPath(Path.Combine(filePath, ADBlFile2));
                    adBldProp2 = ParseADBladeFile(ADBlFile2);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "ADBlFile(3)")
                {
                    ADBlFile3 = oneInput[0];
                    ADBlFile3 = ADBlFile3.Replace("\"", "");
                    ADBlFile3 = Path.GetFullPath(Path.Combine(filePath, ADBlFile3));
                    adBldProp3 = ParseADBladeFile(ADBlFile3);
                }

            }


        }

        private static IDictionary<int, List<double>> ParseADBladeFile(string BldFile)
        {
            IDictionary<int, List<double>> bldProp = new Dictionary<int, List<double>> { };
            var lines = File.ReadAllLines(BldFile);
            int NumBlNds = 0;
            bool isBlNd = false;
            int BlNdLineNo = 0;
            int ID = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] oneInput = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (oneInput.Length >= 2 && oneInput[1] == "NumBlNds")
                {
                    NumBlNds = Int32.Parse(oneInput[0]);
                    isBlNd = true;
                    continue;
                }

                if (isBlNd)
                {
                    if (BlNdLineNo < 2)
                    {
                        BlNdLineNo++;
                        continue;
                    }
                    else
                    {
                        double BlSpn = double.Parse(oneInput[0]);
                        double BlCrvAC = double.Parse(oneInput[1]);
                        double BlSwpAC = double.Parse(oneInput[2]);
                        double BlCrvAng = double.Parse(oneInput[3]);
                        double BlTwist = double.Parse(oneInput[4]);
                        double BlChord = double.Parse(oneInput[5]);

                        var oneProp = new List<double> { BlSpn, BlCrvAC, BlSwpAC, BlCrvAng, BlTwist, BlChord };

                        bldProp.Add(ID, oneProp);
                        BlNdLineNo++;
                        ID++;

                        if (BlNdLineNo == NumBlNds + 2)
                        {
                            isBlNd = false;
                        }
                    }
                }
            }

            return bldProp;
        }


    }

    public class EDModel
    {
        string edFile { get; set; }
        public int NumBl { get; set; }
        public double HubRad { get; set; }
        public double NacCMxn { get; set; }
        public double NacCMyn { get; set; }
        public double NacCMzn { get; set; }
        string BldFile1 { get; set; }
        string BldFile2 { get; set; }
        string BldFile3 { get; set; }

        public EDModel()
        {
            edFile = "";
            NumBl = 0;
            HubRad = 0.0;
            NacCMxn = 0.0;
            NacCMyn = 0.0;
            NacCMzn = 0.0;
        }

        public void ParseEDInputFile(string fileName_ED, string status)
        {
            edFile = fileName_ED;
            string filePath = Path.GetDirectoryName(edFile);
            var lines = File.ReadAllLines(edFile);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] oneInput = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (oneInput.Length >= 2 && oneInput[1] == "NumBl")
                {
                    NumBl = Int32.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "HubRad")
                {
                    HubRad = double.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "NacCMxn")
                {
                    NacCMxn = double.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "NacCMyn")
                {
                    NacCMyn = double.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "NacCMzn")
                {
                    NacCMzn = double.Parse(oneInput[0]);
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "BldFile(1)")
                {
                    BldFile1 = oneInput[0];
                    BldFile1 = BldFile1.Replace("\"", "");
                    BldFile1 = Path.GetFullPath(Path.Combine(filePath, BldFile1));
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "BldFile(2)")
                {
                    BldFile2 = oneInput[0];
                    BldFile2 = BldFile2.Replace("\"", "");
                    BldFile2 = Path.GetFullPath(Path.Combine(filePath, BldFile2));
                }
                else if (oneInput.Length >= 2 && oneInput[1] == "BldFile(3)")
                {
                    BldFile3 = oneInput[0];
                    BldFile3 = BldFile3.Replace("\"", "");
                    BldFile3 = Path.GetFullPath(Path.Combine(filePath, BldFile3));
                }
                else
                {
                    //add more
                }

            }
        }

    }



}
