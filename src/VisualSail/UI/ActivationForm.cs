using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class ActivationForm : Form
    {
        public ActivationForm()
        {
            InitializeComponent();
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        public string Email
        {
            get
            {
                return emailTB.Text;
            }
            set
            {
                emailTB.Text = value;
            }
        }
        public string FirstName
        {
            get
            {
                return firstNameTB.Text;
            }
            set
            {
                firstNameTB.Text = value;
            }
        }
        public string LastName
        {
            get
            {
                return lastNameTB.Text;
            }
            set
            {
                lastNameTB.Text = value;
            }
        }
        public string BoatClass
        {
            get
            {
                return boatClassTB.Text;
            }
            set
            {
                boatClassTB.Text = value;
            }
        }
        public string HomePort
        {
            get
            {
                return homePortTB.Text;
            }
            set
            {
                homePortTB.Text = value;
            }
        }
        public string GoogleOrderNumber
        {
            get
            {
                return googleOrderNumberTB.Text;
            }
            set
            {
                googleOrderNumberTB.Text = value;
            }
        }
        public string SerialNumber
        {
            get
            {
                return serialNumberTB.Text;
            }
            set
            {
                serialNumberTB.Text = value;
            }
        }
        private bool ValidateFields()
        {
            emailTB.Text = emailTB.Text.Trim();
            firstNameTB.Text = firstNameTB.Text.Trim();
            lastNameTB.Text = lastNameTB.Text.Trim();
            boatClassTB.Text = boatClassTB.Text.Trim();
            homePortTB.Text = homePortTB.Text.Trim();
            googleOrderNumberTB.Text = googleOrderNumberTB.Text.Trim();
            serialNumberTB.Text = serialNumberTB.Text.Trim();

            bool result = true;
            if (emailTB.Text == string.Empty)
            {
                result = false;
                emailTB.BackColor = Color.Yellow;
            }
            else
            {
                emailTB.BackColor = TextBox.DefaultBackColor;
            }

            if (firstNameTB.Text == string.Empty)
            {
                result = false;
                firstNameTB.BackColor = Color.Yellow;
            }
            else
            {
                firstNameTB.BackColor = TextBox.DefaultBackColor;
            }

            if (lastNameTB.Text == string.Empty)
            {
                result = false;
                lastNameTB.BackColor = Color.Yellow;
            }
            else
            {
                lastNameTB.BackColor = TextBox.DefaultBackColor;
            }

            if (googleOrderNumberTB.Text == string.Empty)
            {
                result = false;
                googleOrderNumberTB.BackColor = Color.Yellow;
            }
            else
            {
                googleOrderNumberTB.BackColor = TextBox.DefaultBackColor;
            }

            if (serialNumberTB.Text == string.Empty)
            {
                result = false;
                serialNumberTB.BackColor = Color.Yellow;
            }
            else
            {
                serialNumberTB.BackColor = TextBox.DefaultBackColor;
            }

            return result;
        }


    }
}
