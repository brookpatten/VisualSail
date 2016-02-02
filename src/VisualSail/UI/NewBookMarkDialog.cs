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
    public partial class NewBookMarkDialog : Form
    {
        public NewBookMarkDialog()
        {
            InitializeComponent();
        }

        private void NewBookMarkDialog_Load(object sender, EventArgs e)
        {
            nameTB.Focus();
            nameTB.SelectAll();
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string BookmarkName
        {
            get
            {
                return nameTB.Text;
            }
        }

        private void nameTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                okBTN_Click(null, null);
            }
        }
    }
}
