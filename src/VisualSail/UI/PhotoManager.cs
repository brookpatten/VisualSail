using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class PhotoManager : Form
    {
        List<Photo> _photos;
        List<Boat> _boats;
        Lake _lake;
        Image _image;
        bool _disableEvents = false;

        public PhotoManager()
        {
            InitializeComponent();
            _lake = Lake.FindAll()[0];
            LoadBoats();
            LoadPhotos();
        }

        public void LoadBoats()
        {
            _boats = Boat.FindAll();
            boatsLV.Items.Clear();
            foreach (Boat b in _boats)
            {
                ListViewItem lvi = new ListViewItem(b.Number + " " + b.Name);
                boatsLV.Items.Add(lvi);
            }
        }

        public void LoadPhotos()
        {
            int selectedIndex = 0;
            if (photosLV.SelectedIndices.Count > 0)
            {
                selectedIndex = photosLV.SelectedIndices[0];
            }

            photosLV.Items.Clear();
            _photos = Photo.FindAll();
            double bytes = 0;
            foreach (Photo p in _photos)
            {
                string[] items = { p.Name, p.Time.ToLongTimeString(), p.Caption };
                ListViewItem lvi = new ListViewItem(items);
                photosLV.Items.Add(lvi);
                bytes = bytes + p.Jpg.Length;
            }

            if (photosLV.Items.Count > 0)
            {
                if (selectedIndex < photosLV.Items.Count)
                {
                    photosLV.Items[selectedIndex].Selected = true;
                }
                else
                {
                    photosLV.Items[photosLV.Items.Count - 1].Selected = true;
                }
            }

            if (bytes > (1024.0 * 1024.0 * 1024.0))
            {
                sizeLBL.Text = string.Format(" Total Photo Size: {0:f} gb", bytes / (1024.0 * 1024.0 * 1024.0));
            }
            else if (bytes > (1024.0 * 1024.0))
            {
                sizeLBL.Text = string.Format(" Total Photo Size: {0:f} mb", bytes / (1024.0 * 1024.0));
            }
            else if (bytes > (1024.0))
            {
                sizeLBL.Text = string.Format(" Total Photo Size: {0:f} kb", bytes / (1024.0));
            }
            else
            {
                sizeLBL.Text = string.Format(" Total Photo Size: {0:f} b", bytes);
            }
        }

        private void addFolderBTN_Click(object sender, EventArgs e)
        {
            if (folderBD.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo selected = new DirectoryInfo(folderBD.SelectedPath);
                List<FileInfo> files = new List<FileInfo>();
                string[] extensions = openFD.Filter.Substring(openFD.Filter.IndexOf('|') + 1).Split(";".ToCharArray());
                foreach(string extension in extensions)
                {
                    files.AddRange(selected.GetFiles(extension,SearchOption.AllDirectories));
                }
                LoadFileInfo(files.ToArray());
            }
        }

        private void addFilesBTN_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                List<FileInfo> files = new List<FileInfo>();
                foreach (string path in openFD.FileNames)
                {
                    files.Add(new FileInfo(path));
                }
                LoadFileInfo(files.ToArray());
                
            }
        }

        private void LoadFileInfo(FileInfo[] files)
        {
            //BusyDialogManager.Show("Importing and resizing pictures",this);
            foreach (FileInfo f in files)
            {
                BusyDialogManager.Show("Importing and resizing " + f.Name);
                Photo p = new Photo();
                p.Name = f.Name;
                //TimeZoneInfo tzi = _lake.TimeZone;
                //p.Time = TimeZoneInfo.ConvertTimeToUtc(f.LastWriteTime, tzi);
                ReadExif(f.FullName,p);
                p.Jpg = ConvertAndResize(f.FullName, 640, 480);
                p.Save();
            }
            LoadPhotos();
            BusyDialogManager.Hide();
        }

        private void ReadExif(string path,Photo p)
        {
            FileStream fs=null;
            //try
            //{
            //    fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            //    System.Windows.Media.Imaging.BitmapSource img = System.Windows.Media.Imaging.BitmapFrame.Create(fs);
            //    System.Windows.Media.Imaging.BitmapMetadata meta = (System.Windows.Media.Imaging.BitmapMetadata)img.Metadata;
            //    try
            //    {
            //        p.Caption = meta.Title + " " + meta.Subject + " " + meta.Comment;
            //    }
            //    catch
            //    {
            //        p.Caption = "";
            //    }
            //    try
            //    {
            //        p.Time = DateTime.Parse(meta.DateTaken);
            //    }
            //    catch
            //    {
            //        FileInfo fi = new FileInfo(path);
            //        p.Time = fi.LastWriteTime;
            //    }
            //    fs.Close();
            //}
            //catch
            //{
                if (fs != null)
                {
                    try
                    {
                        fs.Close();
                    }
                    catch { }
                }
                p.Caption = "";
                FileInfo fi = new FileInfo(path);
                p.Time = fi.LastWriteTime;
            //}
        }

        private byte[] ConvertAndResize(string path,int bigDimension,int littleDimension)
        {
            Image i = Image.FromFile(path);

            byte[] result;

            if (i.Width > i.Height)
            {
                //find the ratio for each axis
                double xRatio = (double)bigDimension / (double)i.Width;
                double yRatio = (double)littleDimension / (double)i.Height;

                double ratio;
                //now figure out which one we want to use
                //by finding the one that also fits in the other axis
                if ((double)i.Height * xRatio <= littleDimension)
                {
                    ratio = xRatio;
                }
                else
                {
                    ratio = yRatio;
                }

                Image n = new Bitmap(bigDimension, (int)((double)i.Height * ratio));
                Graphics ng = Graphics.FromImage(n);
                ng.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                ng.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                ng.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                ng.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                ng.DrawImage(i, 0, 0, n.Width, n.Height);
                ng.Flush();
                ng.Dispose();
                MemoryStream ms = new MemoryStream();
                n.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = ms.ToArray();
                ms.Close();
                n.Dispose();
            }
            else
            {
                //find the ratio for each axis
                double xRatio = (double)littleDimension / (double)i.Width;
                double yRatio = (double)bigDimension / (double)i.Height;

                double ratio;
                //now figure out which one we want to use
                //by finding the one that also fits in the other axis
                if ((double)i.Height * xRatio <= bigDimension)
                {
                    ratio = xRatio;
                }
                else
                {
                    ratio = yRatio;
                }

                Image n = new Bitmap((int)((double)i.Width * ratio), bigDimension);
                Graphics ng = Graphics.FromImage(n);
                ng.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                ng.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                ng.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                ng.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                ng.DrawImage(i, 0, 0, n.Width, n.Height);
                ng.Flush();
                ng.Dispose();
                MemoryStream ms = new MemoryStream();
                n.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = ms.ToArray();
                ms.Close();
                n.Dispose();
            }
            i.Dispose();
            return result;
        }

        private void rotateLeftBTN_Click(object sender, EventArgs e)
        {
            _image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            MemoryStream ms = new MemoryStream();
            _image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            SelectedPhoto.Jpg = ms.ToArray();
            ms.Close();
            drawPNL.Invalidate();
        }

        private void rotateRightBTN_Click(object sender, EventArgs e)
        {
            _image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            MemoryStream ms = new MemoryStream();
            _image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            SelectedPhoto.Jpg = ms.ToArray();
            ms.Close();
            drawPNL.Invalidate();
        }

        private void saveBTN_Click(object sender, EventArgs e)
        {
            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                _image.Save(saveFD.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void deleteBTN_Click(object sender, EventArgs e)
        {
            SelectedPhoto.Delete();
            LoadPhotos();
        }

        private void takenAtDTP_ValueChanged(object sender, EventArgs e)
        {
            if (!_disableEvents)
            {
                SelectedPhoto.Time = takenAtDTP.Value;
            }
        }

        private void captionTB_TextChanged(object sender, EventArgs e)
        {
            if (!_disableEvents)
            {
                SelectedPhoto.Caption = captionTB.Text;
            }
        }

        private Photo SelectedPhoto
        {
            get
            {
                if (photosLV.SelectedIndices.Count > 0)
                {
                    return _photos[photosLV.SelectedIndices[0]];
                }
                else
                {
                    return null;
                }
            }
        }

        private void photosLV_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableEvents = true;
            if (SelectedPhoto!=null)
            {
                editPhotoGB.Enabled = true;
                captionTB.Text = SelectedPhoto.Caption;
                filenameLBL.Text = SelectedPhoto.Name;
                takenAtDTP.Value = SelectedPhoto.Time;

                for (int i = 0; i < boatsLV.Items.Count; i++)
                {
                    boatsLV.Items[i].Checked = false;
                }

                foreach (Boat b in SelectedPhoto.Boats)
                {
                    for (int i = 0; i < _boats.Count; i++)
                    {
                        if (b.Id == _boats[i].Id)
                        {
                            boatsLV.Items[i].Checked = true;
                        }
                    }
                }

                MemoryStream ms = new MemoryStream(SelectedPhoto.Jpg);
                _image = Image.FromStream(ms);
                drawPNL.Invalidate();
            }
            else
            {
                editPhotoGB.Enabled = false;
                _image = null;
            }
            _disableEvents = false;
        }

        private void boatsLV_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!_disableEvents)
            {
                if (SelectedPhoto != null)
                {
                    List<Boat> selectedBoats = new List<Boat>();
                    for (int i = 0; i < boatsLV.Items.Count; i++)
                    {
                        if (boatsLV.Items[i].Checked)
                        {
                            selectedBoats.Add(_boats[i]);
                        }
                    }
                    SelectedPhoto.Boats = selectedBoats;
                }
            }
        }

        private void drawPNL_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, 0, 0, drawPNL.Width, drawPNL.Height);
            if (_image != null)
            {
                //find the ratio for each axis
                double xRatio = (double)drawPNL.Width / (double)_image.Width;
                double yRatio = (double)drawPNL.Height / (double)_image.Height;

                double ratio;
                int xOffset = 0;
                int yOffset = 0;
                //now figure out which one we want to use
                //by finding the one that also fits in the other axis
                if ((double)_image.Height * xRatio <= drawPNL.Height)
                {
                    ratio = xRatio;
                    yOffset = (drawPNL.Height - (int)((double)_image.Height * xRatio)) / 2;
                }
                else
                {
                    ratio = yRatio;
                    xOffset = (drawPNL.Width - (int)((double)_image.Width * yRatio)) / 2;
                }

                g.DrawImage(_image, xOffset, yOffset, (int)((double)_image.Width * ratio), (int)((double)_image.Height * ratio));
            }
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            foreach (Photo p in _photos)
            {
                p.Save();
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
