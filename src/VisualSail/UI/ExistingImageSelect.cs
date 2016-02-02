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
    public partial class ExistingImageSelect : Form
    {
        List<string> _paths;
        private string _selectedPath;
        public ExistingImageSelect(List<string> paths)
        {
            _paths = paths;
            InitializeComponent();
            LoadImages();
        }
        public void LoadImages()
        {
            imageGV.Rows.Clear();
            foreach (string path in _paths)
            {
                Image i=Image.FromFile(path);
                Image t=ImageThumbnail(i, 300);
                object[] parms={t};
                imageGV.Rows.Add(parms);
            }
        }

        private Image ImageThumbnail(Image source, int size)
        {
            Image destination = new Bitmap(size, size);
            Graphics destG = Graphics.FromImage(destination);
            destG.FillRectangle(Brushes.White, 0, 0, size, size);

            double sWidth = (double)source.Width;
            double sHeight = (double)source.Height;

            int dWidth;
            int dHeight;

            if (sWidth > sHeight)
            {
                dWidth = size;
                dHeight = (int)(((double)size / sWidth) * sHeight);
                destG.DrawImage(source, 0, (size - dHeight) / 2, dWidth, dHeight);
            }
            else
            {
                dWidth = (int)(((double)size / sHeight) * sWidth);
                dHeight = size;
                destG.DrawImage(source, (size - dWidth) / 2, 0, dWidth, dHeight);
            }
            destG.Dispose();
            return destination;
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imageGV_SelectionChanged(object sender, EventArgs e)
        {
            if (imageGV.SelectedRows.Count > 0)
            {
                _selectedPath = _paths[imageGV.SelectedRows[0].Index];
            }
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            _selectedPath = "";
            this.Close();
        }

        public string SelectedPath
        {
            get
            {
                return _selectedPath;
            }
        }
    }
}
