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
                gbFstSimCon.Visible = true;
                gbFstFeature.Visible = false;

                getFstSimulationControl();
            }
            else if (cbFstInput.SelectedIndex == 1)
            {
                gbFstFeature.Visible = true;
                gbFstSimCon.Visible = false;

                getFstFeatureSwitchesAndFlags();
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

        private void getFstFeatureSwitchesAndFlags()
        {
            if (fstInput.CompElast.value == 1)
            {
                cbFstCompElast.SelectedIndex = 0;
            }
            else if (fstInput.CompElast.value == 1)
            {
                cbFstCompElast.SelectedIndex = 1;
            }
            else
            {
                //error
            }

            if (fstInput.CompInflow.value == 0)
            {
                cbFstCompInflow.SelectedIndex = 0;
            }
            else if (fstInput.CompInflow.value == 1)
            {
                cbFstCompInflow.SelectedIndex = 1;
            }
            else if (fstInput.CompInflow.value == 2)
            {
                cbFstCompInflow.SelectedIndex = 2;
            }
            else
            {
                //error
            }

            if (fstInput.CompAero.value == 0)
            {
                cbFstCompAero.SelectedIndex = 0;
            }
            else if (fstInput.CompAero.value == 1)
            {
                cbFstCompAero.SelectedIndex = 1;
            }
            else if (fstInput.CompAero.value == 2)
            {
                cbFstCompAero.SelectedIndex = 2;
            }
            else
            {
                //error
            }

            if (fstInput.CompServo.value == 0)
            {
                cbFstCompServo.SelectedIndex = 0;
            }
            else if (fstInput.CompServo.value == 1)
            {
                cbFstCompServo.SelectedIndex = 1;
            }
            else
            {
                //error
            }

            if (fstInput.CompHydro.value == 0)
            {
                cbFstCompHydro.SelectedIndex = 0;
            }
            else if (fstInput.CompHydro.value == 1)
            {
                cbFstCompHydro.SelectedIndex = 1;
            }
            else
            {
                //error
            }

            if (fstInput.CompSub.value == 0)
            {
                cbFstCompSub.SelectedIndex = 0;
            }
            else if (fstInput.CompSub.value == 1)
            {
                cbFstCompSub.SelectedIndex = 1;
            }
            else if (fstInput.CompSub.value == 2)
            {
                cbFstCompSub.SelectedIndex = 2;
            }
            else
            {
                //error
            }

            if (fstInput.CompMooring.value == 0)
            {
                cbFstCompMooring.SelectedIndex = 0;
            }
            else if (fstInput.CompMooring.value == 1)
            {
                cbFstCompMooring.SelectedIndex = 1;
            }
            else if (fstInput.CompMooring.value == 2)
            {
                cbFstCompMooring.SelectedIndex = 2;
            }
            else if (fstInput.CompMooring.value == 3)
            {
                cbFstCompMooring.SelectedIndex = 3;
            }
            else if (fstInput.CompMooring.value == 4)
            {
                cbFstCompMooring.SelectedIndex = 4;
            }
            else
            {
                //error
            }

            if (fstInput.CompIce.value == 0)
            {
                cbFstCompIce.SelectedIndex = 0;
            }
            else if (fstInput.CompIce.value == 1)
            {
                cbFstCompIce.SelectedIndex = 1;
            }
            else if (fstInput.CompIce.value == 2)
            {
                cbFstCompIce.SelectedIndex = 2;
            }
            else
            {
                //error
            }

            if (fstInput.MHK.value == 0)
            {
                cbFstMHK.SelectedIndex = 0;
            }
            else if (fstInput.MHK.value == 1)
            {
                cbFstMHK.SelectedIndex = 1;
            }
            else if (fstInput.MHK.value == 2)
            {
                cbFstMHK.SelectedIndex = 2;
            }
            else
            {
                //error
            }
        }


    }
}
