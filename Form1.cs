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
        enum FastFile
        {
            None,
            Fast,
            HD,
            ED
        }

        enum OutFile
        {
            None,
            SacsPy,
            Ism
        }

        static string fileName_HD = "";
        static string status = "";
        static FastFile fastFile;
        static OutFile outFile;

        public Form1()
        {
            InitializeComponent();
            cbFastFile.SelectedIndex = 0;
            cbOutputFile.SelectedIndex = 1;
        }

        private void bSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openD = new OpenFileDialog();
            if (fastFile == FastFile.HD)
            {
                openD.DefaultExt = ".dat";
                openD.Filter = "HydroDyn Input File (.dat)|*.dat";
                if (openD.ShowDialog() == DialogResult.OK)
                {
                    fileName_HD = openD.FileName;
                }
            }
            else
            {
                //add more
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if ((fastFile == FastFile.HD) && (outFile == OutFile.SacsPy))
            {
                HDModel hdModel = new HDModel();
                hdModel.ParseHDInputFile(fileName_HD, status);

                ToSacs.ConvertHDToSacs(hdModel, ref status);
                txtStatus.Text = status;
            }
            else if ((fastFile == FastFile.HD) && (outFile == OutFile.Ism))
            {
                HDModel hdModel = new HDModel();
                hdModel.ParseHDInputFile(fileName_HD, status);

                ToIsm.ConvertHDToIsm(hdModel, ref status);
                txtStatus.Text = status;
            }
        }

        private FastFile HandleFastFile()
        {
            if (cbFastFile.SelectedIndex == 0)
            {
                return FastFile.HD;
            }
            else
            {
                //error
                return FastFile.None;
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

        private void cbFastFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            fastFile = HandleFastFile();
        }

        private void cbOutputFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            outFile = HandleOutFile();
        }
    }
}
