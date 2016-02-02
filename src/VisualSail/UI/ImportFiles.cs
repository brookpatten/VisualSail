using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using AmphibianSoftware.VisualSail.Data.Import;
using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class ImportFiles : Form
    {
        private Boat _boat;
        public ImportFiles(Boat b)
        {
            InitializeComponent();
            _boat = b;
            LoadFiles();
            fileInfoLBL.Text = "";
            boatLBL.Text = _boat.Name + " " + _boat.Number;
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void LoadFiles()
        {
            fileLB.Items.Clear();
            List<SensorFile> files = _boat.SensorFiles;
            foreach (SensorFile sf in files)
            {
                fileLB.Items.Add(sf);
            }
        }

        private void fileLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileLB.SelectedIndex >= 0)
            {
                SkipperDataSet.SensorReadingsDataTable srdt = ((SensorFile)fileLB.SelectedItem).SensorReadings;

                if (srdt.Rows.Count > 0)
                {
                    DateTime start = ((SkipperDataSet.SensorReadingsRow)srdt.Rows[0]).datetime;
                    DateTime end = ((SkipperDataSet.SensorReadingsRow)srdt.Rows[srdt.Rows.Count - 1]).datetime;

                    fileInfoLBL.Text = start.ToShortTimeString() + " to " + end.ToShortTimeString();
                    removeFileBTN.Enabled = true;
                }
                else
                {
                    fileInfoLBL.Text = "No Data from this file was imported";
                    removeFileBTN.Enabled = true;
                }
            }
            else
            {
                fileInfoLBL.Text = "";
                removeFileBTN.Enabled = false;
            }
        }

        private void importBTN_Click(object sender, EventArgs e)
        {
            openFD.ShowDialog();
            string path = openFD.FileName;
            if (File.Exists(path))
            {
                try
                {
                    FileImporter fi = FileImporter.DetectFileType(path);
                    BusyDialogManager.Show("Importing Data");
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.WorkerSupportsCancellation = true;
                    bw.WorkerReportsProgress = false;

                    

                    bw.DoWork += (s, args) =>
                    {
                        fi.ImportFile(((AddFileToBoatArgument)args.Argument).Path, ((AddFileToBoatArgument)args.Argument).Boat);
                    };

                    bw.RunWorkerCompleted += (s, args) =>
                    {
                        if (args.Error != null)
                        {
                            
                        }
                        else
                        {
                            LoadFiles();
                            BusyDialogManager.Hide();
                        }
                    };

                    

                    bw.RunWorkerAsync(new AddFileToBoatArgument() { Boat = _boat, Path = path });

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to import file." + ex.Message);
                }
            }
        }

        private void removeFileBTN_Click(object sender, EventArgs e)
        {
            ((SensorFile)fileLB.SelectedItem).Delete();
            LoadFiles();
        }
    }

    public class AddFileToBoatArgument
    {
        public string Path { get; set; }
        public Boat Boat { get; set; }
    }
}
