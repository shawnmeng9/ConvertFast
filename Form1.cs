using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertFast
{
    public partial class Form1 : Form
    {
        enum OutFile
        {
            None,
            SacsPy,
            Ism
        }

        FstModel fstModel = new FstModel();
        HDModel hdModel = new HDModel(); 
        SDModel sdModel = new SDModel();
        static string fileName_fst = "";
        static string status = "";
        static OutFile outFile;

        public Form1()
        {
            InitializeComponent();
            cbOutputFile.SelectedIndex = 1;
        }

        private void bSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openD = new OpenFileDialog();

            openD.DefaultExt = ".fst";
            openD.Filter = "FAST Input File (.fst)|*.fst";
            if (openD.ShowDialog() == DialogResult.OK)
            {
                fileName_fst = openD.FileName;
                fstModel.filePath = System.IO.Path.GetDirectoryName(openD.FileName);
                fstModel.ParseFstInputFile(fileName_fst, status);

                DataGridViewCheckBoxColumn chkIncluded = new DataGridViewCheckBoxColumn();
                chkIncluded.ValueType = typeof(bool);
                chkIncluded.Name = "coliIncluded";
                chkIncluded.HeaderText = "Included";
                chkIncluded.ReadOnly = true;
                dgvFastInputFiles.Columns.Add(chkIncluded);

                dgvFastInputFiles.Columns.Add("colFeature", "Feature");
                dgvFastInputFiles.Columns.Add("colModule", "Module");
                dgvFastInputFiles.Columns.Add("colFileName", "File Name");

                bool included = false;
                string feature = "";
                string module = "";
                string fileName = "";

                if (GetCompElastInfo(fstModel.CompElast, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertElast = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                }
                if (GetCompAeroInfo(fstModel.CompAero, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertAero = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                }
                if (GetCompHydroInfo(fstModel.CompHydro, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertHydro = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                }
                if (GetCompSubInfo(fstModel.CompSub, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertSub = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                }
                if (GetCompMooringInfo(fstModel.CompMooring, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertMooring = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                }



            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (fstModel.convertElast)
            {
                //add code
            }
            if (fstModel.convertAero)
            {
                //add code
            }
            if (fstModel.convertHydro)
            {
                hdModel.ParseHDInputFile(fstModel.HydroFile, status);
            }
            if (fstModel.convertSub)
            {               
                sdModel.ParseSDInputFile(fstModel.SubFile, status);
            }
            if (fstModel.convertMooring)
            {
                //add code
            }



            if (outFile == OutFile.SacsPy)
            {
                ToSacs.ConvertHDToSacs(hdModel, ref status);
                txtStatus.Text = status;
            }
            else if (outFile == OutFile.Ism)
            {
                ToIsm.ConvertHDToIsm(hdModel, ref status);
                txtStatus.Text = status;
            }



            //if ((fastFile == FastFile.HD) && (outFile == OutFile.SacsPy))
            //{
            //    HDModel hdModel = new HDModel();
            //    hdModel.ParseHDInputFile(fileName_HD, status);

            //    ToSacs.ConvertHDToSacs(hdModel, ref status);
            //    txtStatus.Text = status;
            //}
            //else if ((fastFile == FastFile.HD) && (outFile == OutFile.Ism))
            //{
            //    HDModel hdModel = new HDModel();
            //    hdModel.ParseHDInputFile(fileName_HD, status);

            //    ToIsm.ConvertHDToIsm(hdModel, ref status);
            //    txtStatus.Text = status;
            //}
            //else if ((fastFile == FastFile.SD) && (outFile == OutFile.SacsPy))
            //{
            //    SDModel sdModel = new SDModel();
            //    sdModel.ParseSDInputFile(fileName_SD, status);

            //    ToSacs.ConvertSDToSacs(sdModel, ref status);
            //    txtStatus.Text = status;
            //}
            //else if ((fastFile == FastFile.SD) && (outFile == OutFile.Ism))
            //{
            //    SDModel sdModel = new SDModel();
            //    sdModel.ParseSDInputFile(fileName_SD, status);

            //    ToIsm.ConvertSDToIsm(sdModel, ref status);
            //    txtStatus.Text = status;
            //}
            //else
            //{
            //    //add more
            //}

        }

        private OutFile HandleOutFile()
        {
            if (cbOutputFile.SelectedIndex == 0)
            {
                return OutFile.SacsPy;
            }
            else if (cbOutputFile.SelectedIndex == 1)
            {
                return OutFile.Ism;
            }
            else
            {
                //error
                return OutFile.None;
            }
        }

        private void cbOutputFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            outFile = HandleOutFile();
        }

        private bool GetCompElastInfo(int? CompElast, ref bool included, ref string feature, ref string module, ref string fileName)
        {
            included = false;
            feature = "";
            module = "";
            fileName = "";
            if (CompElast == 0 || CompElast > 2)
            {
                return false;
            }
            else
            {
                if (CompElast == 1)
                {
                    included = false;
                    feature = "Elastic Dynamics";
                    module = "ElastoDyn";
                    fileName = fstModel.EDFile;
                }
                else if (CompElast == 2)
                {
                    included = false;
                    feature = "Elastic Dynamics";
                    module = "ElastoDyn + BeamDyn for blades";
                    fileName = fstModel.EDFile;
                }
                return true;
            } 
        }

        private bool GetCompAeroInfo(int? CompAero, ref bool included, ref string feature, ref string module, ref string fileName)
        {
            included = false;
            feature = "";
            module = "";
            fileName = "";
            if (CompAero == 0 || CompAero > 2)
            {
                return false;
            }
            else
            {
                if (CompAero == 1)
                {
                    included = false;
                    feature = "Aerodynamics";
                    module = "AeroDyn v14";
                    fileName = fstModel.AeroFile;
                }
                else if (CompAero == 2)
                {
                    included = false;
                    feature = "Aerodynamics";
                    module = "AeroDyn v14";
                    fileName = fstModel.AeroFile;
                }
                return true;
            }
        }

        private bool GetCompHydroInfo(int? CompHydro, ref bool included, ref string feature, ref string module, ref string fileName)
        {
            included = false;
            feature = "";
            module = "";
            fileName = "";
            if (CompHydro == 0 || CompHydro > 1)
            {
                return false;
            }
            else
            {
                if (CompHydro == 1)
                {
                    included = true;
                    feature = "Hydrodynamics";
                    module = "HydroDyn";
                    fileName = fstModel.HydroFile;
                }
                return true;
            }
        }

        private bool GetCompSubInfo(int? CompSub, ref bool included, ref string feature, ref string module, ref string fileName)
        {
            included = false;
            feature = "";
            module = "";
            fileName = "";
            if (CompSub == 0 || CompSub > 2)
            {
                return false;
            }
            else
            {
                if (CompSub == 1)
                {
                    included = false;
                    feature = "Sub-structural dynamics";
                    module = "SubDyn";
                    fileName = fstModel.SubFile;
                }
                else if (CompSub == 2)
                {
                    included = false;
                    feature = "Sub-structural dynamics";
                    module = "External Platform MCKF";
                    fileName = fstModel.SubFile;
                }
                return true;
            }
        }

        private bool GetCompMooringInfo(int? CompMooring, ref bool included, ref string feature, ref string module, ref string fileName)
        {
            included = false;
            feature = "";
            module = "";
            fileName = "";
            if (CompMooring == 0 || CompMooring > 5)
            {
                return false;
            }
            else
            {
                if (CompMooring == 1)
                {
                    included = false;
                    feature = "Mooring system";
                    module = "MAP++";
                    fileName = fstModel.MooringFile;
                }
                else if (CompMooring == 2)
                {
                    included = false;
                    feature = "Mooring system";
                    module = "FEAMooring";
                    fileName = fstModel.MooringFile;
                }
                else if (CompMooring == 3)
                {
                    included = false;
                    feature = "Mooring system";
                    module = "MoorDyn";
                    fileName = fstModel.MooringFile;
                }
                else if (CompMooring == 4)
                {
                    included = false;
                    feature = "Mooring system";
                    module = "OrcaFlex";
                    fileName = fstModel.MooringFile;
                }
                return true;
            }
        }


    }
}
