using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using AmphibianSoftware.VisualSail.Licensing;
using AmphibianSoftware.VisualSail.WebServices;

#if !NOLICENSE
using License;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class LicenseForm : Form
    {
        public string _activatedLicensePath;

        ActivationForm _activationForm;
        public LicenseForm(string activatedLicensePath)
        {
            _activatedLicensePath = activatedLicensePath;
            InitializeComponent();
            _activationForm = new ActivationForm();
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            if (Status.Evaluation_Lock_Enabled)
            {
                if (Status.Evaluation_Time_Current <= Status.Evaluation_Time)
                {
                    demoRB.Enabled = true;
                    demoRB.Checked = true;
                    demoInfoLBL.Text = "Day " + Status.Evaluation_Time_Current + " of " + Status.Evaluation_Time;
                }
                else
                {
                    demoInfoLBL.Text = "Trial Period Expired";
                    demoInfoLBL.ForeColor = Color.Red;
                    onlineRB.Checked = true;
                }
            }
            else
            {
                demoInfoLBL.Text = "";
                onlineRB.Checked = true;
            }
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            if (demoRB.Checked)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (onlineRB.Checked)
            {
                this.Enabled = false;
                if (_activationForm.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = null;
                    try
                    {
                        ActivationService activator = new ActivationService();
                        //activator.Url = "http://localhost:6422/Services/ActivationService.asmx";
                        string hardwareId = Status.HardwareID;

                        AmphibianSoftware.VisualSail.WebServices.ActivationResponse response =
                            activator.RequestActivation(hardwareId,
                            "VISUALSAIL1",
                            _activationForm.GoogleOrderNumber,
                            _activationForm.SerialNumber,
                            _activationForm.Email,
                            _activationForm.FirstName,
                            _activationForm.LastName,
                            _activationForm.BoatClass,
                            _activationForm.HomePort);

                        if (response.License != null && response.License.Length > 0)
                        {
                            fs = new FileStream(_activatedLicensePath, FileMode.OpenOrCreate,FileAccess.Write);
                            fs.Write(response.License, 0, response.License.Length);
                            fs.Flush();
                            fs.Close();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(response.Message);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Online Activation Unavailible at this time, please try again later."+Environment.NewLine+ex.Message);
                    }
                }
                this.Enabled = true;
            }
            else if (fileRB.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "License Files|*.license";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    bool exeptionOccured = false;
                    try
                    {
                        FileInfo fi = new FileInfo(ofd.FileName);
                        fi.CopyTo(_activatedLicensePath,true);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("License could not be loaded");
                    }
                }
            }
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void fileInfoLBL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When requesting a license file, you will need to provide the following information..."+Environment.NewLine+"Email Address"+Environment.NewLine+"Google Order Number"+Environment.NewLine+"Serial Number"+Environment.NewLine+"Hardware ID: "+Status.HardwareID+Environment.NewLine+Environment.NewLine+"Email this information to Support@AmphibianSoftware.com", "File Activation Information");
        }
    }
}
#endif