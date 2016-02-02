using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class EditBoat : Form
    {
        Boat _boat;
        public EditBoat(Boat b)
        {
            _boat = b;
            InitializeComponent();
            LoadBoatTypes();
            nameTB.Text = _boat.Name;
            numberTB.Text = _boat.Number;
            colorBTN.BackColor = Color.FromArgb(_boat.Color);

            if(b.BoatType==null)
            {
                typeCB.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < typeCB.Items.Count; i++)
                {
                    if (((BoatType)typeCB.Items[i]).Id == _boat.BoatType.Id)
                    {
                        typeCB.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void LoadBoatTypes()
        {
            List<BoatType> types = BoatType.FindAll();
            typeCB.Items.Clear();
            foreach (BoatType t in types)
            {
                typeCB.Items.Add(t);
            }
        }

        private void colorBTN_Click(object sender, EventArgs e)
        {
            colorD.ShowDialog();
            colorBTN.BackColor = colorD.Color;
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            _boat.Name = nameTB.Text;
            _boat.Number = numberTB.Text;
            _boat.Color = colorBTN.BackColor.ToArgb();
            _boat.BoatType = (BoatType)typeCB.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public Boat Boat
        {
            get
            {
                return _boat;
            }
        }

        private void ValidateForm()
        {
            okBTN.Enabled = (nameTB.Text != string.Empty && numberTB.Text != string.Empty && typeCB.SelectedIndex >= 0);
        }

        private void nameTB_TextChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void numberTB_TextChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void typeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            _boat = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
