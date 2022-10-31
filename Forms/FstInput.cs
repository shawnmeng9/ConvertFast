using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertFast
{
    public partial class Form1 : Form
    {
        private void cbFstInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFstInput.SelectedIndex == 0)
            {
                gbSimulationControl.Visible = true;

                getFstSimulationControl();

            }
            else if (cbFstInput.SelectedIndex == 1)
            {
                gbSimulationControl.Visible = false;
            }
        }



        private void getFstSimulationControl()
        {
            txtFstTitle.Text = fstInput.title.value;

            if (fstInput.Echo.value == true)
            {
                chkFstEcho.Checked = true;
            }
            else
            {
                chkFstEcho.Checked = false;
            }

            txtFstAbortLevel.Text = fstInput.AbortLevel.value;
            txtFstTMax.Text = fstInput.TMax.value.ToString();
            txtFstDT.Text = fstInput.DT.value.ToString();
            txtFstInterpOrder.Text = fstInput.InterpOrder.value.ToString();
            txtFstNumCrctn.Text = fstInput.NumCrctn.value.ToString();
            txtFstDT_UJac.Text = fstInput.DT_UJac.value.ToString();
            txtFstUJacSclFact.Text = fstInput.UJacSclFact.value.ToString();

        }


    }
}
