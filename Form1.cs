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

        enum FastModule
        {
            None,
            Aero,
            Elast,
            Hydro,
            Mooring,
            Sub,
            Servo
        }

        FstModel fstModel = new FstModel();
        static string fileName_fst = "";
        static string status = "";
        static OutFile outFile;
        static IDictionary<int, FastModule> dicFastModules = new Dictionary<int, FastModule> { };

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
                chkIncluded.Name = "colIncluded";
                chkIncluded.HeaderText = "Included";
                dgvFastInputFiles.Columns.Add(chkIncluded);

                dgvFastInputFiles.Columns.Add("colFeature", "Feature");
                dgvFastInputFiles.Columns.Add("colModule", "Module");
                dgvFastInputFiles.Columns.Add("colFileName", "File Name");

                bool included = false;
                string feature = "";
                string module = "";
                string fileName = "";
                int rowIndex = -1;
                dicFastModules.Clear();
                dgvFastInputFiles.Rows.Clear();

                if (GetCompElastInfo(fstModel.CompElast, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertElast = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                    rowIndex += 1;
                    dicFastModules.Add(rowIndex, FastModule.Elast);
                }
                if (GetCompAeroInfo(fstModel.CompAero, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertAero = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                    rowIndex += 1;
                    dicFastModules.Add(rowIndex, FastModule.Aero);
                }
                if (GetCompHydroInfo(fstModel.CompHydro, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertHydro = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                    rowIndex += 1;
                    dicFastModules.Add(rowIndex, FastModule.Hydro);
                }
                if (GetCompSubInfo(fstModel.CompSub, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertSub = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                    rowIndex += 1;
                    dicFastModules.Add(rowIndex, FastModule.Sub);
                }
                if (GetCompMooringInfo(fstModel.CompMooring, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertMooring = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                    rowIndex += 1;
                    dicFastModules.Add(rowIndex, FastModule.Mooring);
                }
                if (GetCompServoInfo(fstModel.CompServo, ref included, ref feature, ref module, ref fileName))
                {
                    fstModel.convertServo = included;
                    dgvFastInputFiles.Rows.Add(included, feature, module, fileName);
                    rowIndex += 1;
                    dicFastModules.Add(rowIndex, FastModule.Servo);
                }

                SetModifiableCells();


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
                fstModel.hdModel = new HDModel();
                fstModel.hdModel.ParseHDInputFile(fstModel.HydroFile, status);
            }
            if (fstModel.convertSub)
            {
                fstModel.sdModel = new SDModel();
                fstModel.sdModel.ParseSDInputFile(fstModel.SubFile, status);
            }
            if (fstModel.convertMooring)
            {
                fstModel.mdModel = new MDModel();
                fstModel.mdModel.ParseMDInputFile(fstModel.MooringFile, status);
            }
            if (fstModel.convertServo)
            {
                //add code
            }


            if (outFile == OutFile.SacsPy)
            {
                ToSacs.ConvertToSacs(fstModel, ref status);
                txtStatus.Text = status;
            }
            else if (outFile == OutFile.Ism)
            {
                ToIsm.ConvertToIsm(fstModel, ref status);
                txtStatus.Text = status;
            }
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
                    included = true;
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
            if (CompMooring == 0 || CompMooring > 4)
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
                    included = true;
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

        private bool GetCompServoInfo(int? CompServo, ref bool included, ref string feature, ref string module, ref string fileName)
        {
            included = false;
            feature = "";
            module = "";
            fileName = "";
            if (CompServo == 0 || CompServo > 1)
            {
                return false;
            }
            else
            {
                if (CompServo == 1)
                {
                    included = false;
                    feature = "Control and electrical-drive dynamics";
                    module = "ServoDyn";
                    fileName = fstModel.ServoFile;
                }
                return true;
            }
        }

        private void SetModifiableCells()
        {
            foreach (DataGridViewRow row in dgvFastInputFiles.Rows)
            {
                row.ReadOnly = true;
                row.Cells["colIncluded"].Style.BackColor = Color.Gray;
                if (dicFastModules[row.Index] == FastModule.Hydro || dicFastModules[row.Index] == FastModule.Sub || dicFastModules[row.Index] == FastModule.Mooring)
                {
                    row.Cells["colIncluded"].ReadOnly = false;
                    row.Cells["colIncluded"].Style.BackColor = Color.White;
                }
            }
            //dgvFastInputFiles.Refresh();
        }

        void dgvFastInputFiles_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvFastInputFiles.IsCurrentCellDirty)
            {
                dgvFastInputFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        public void dgvFastInputFiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvFastInputFiles.Columns[e.ColumnIndex].Name == "colIncluded")
            {
               DataGridViewCheckBoxCell checkCell =(DataGridViewCheckBoxCell)dgvFastInputFiles.Rows[e.RowIndex].Cells["colIncluded"];

                if (dicFastModules[e.RowIndex] == FastModule.Aero)
                {
                    fstModel.convertAero = (Boolean)checkCell.Value;
                }
                else if (dicFastModules[e.RowIndex] == FastModule.Elast)
                {
                    fstModel.convertElast = (Boolean)checkCell.Value;
                }
                else if (dicFastModules[e.RowIndex] == FastModule.Hydro)
                {
                    fstModel.convertHydro = (Boolean)checkCell.Value;
                }
                else if (dicFastModules[e.RowIndex] == FastModule.Sub)
                {
                    fstModel.convertSub = (Boolean)checkCell.Value;
                }
                else if (dicFastModules[e.RowIndex] == FastModule.Mooring)
                {
                    fstModel.convertMooring = (Boolean)checkCell.Value;
                }

                dgvFastInputFiles.Invalidate();
            }
        }

    }
}
