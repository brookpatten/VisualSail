using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using AmphibianSoftware.Skipper.Library;
using AmphibianSoftware.Skipper.Library.IO;
using AmphibianSoftware.Skipper.Library.Nmea;
using AmphibianSoftware.Skipper.Data;
using AmphibianSoftware.Skipper.Data.Import;

namespace AmphibianSoftware.Skipper.UI
{
    public partial class SkipperForm : Form
    {
        Thread _drawThread;
        bool _redraw = true;
        bool _updateStatistics = true;

        

        public SkipperForm()
        {
            try
            {
                //throw new Exception("Don't load from the file during debugging");
                Persistance.LoadFromFile();
            }
            catch
            {
                Persistance.CreateNew();
            }

            SelectRace sr = new SelectRace();
            sr.ShowDialog();
            Race r = sr.SelectedRace;
            EditRace er = new EditRace(r);
            er.ShowDialog();
            //Persistance.SaveToFile();
            InitializeComponent();
            viewPanel.Initialize(r,new Notify(this.RequestStatisticsUpdate));
            foreach (AmphibianSoftware.Skipper.Data.Boat b in r.Boats)
            {
                boatsLB.Items.Add(b);
            }
            boatsLB.SelectedIndex = 0;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            _drawThread = new Thread(new ThreadStart(drawLoop));
            _drawThread.Start();
        }

        public void drawLoop()
        {
            while (_redraw)
            {
                viewPanel.Invalidate();
                UpdateStatistics();
                timeLBL.Text = viewPanel.RenderTime.ToLongTimeString();
                Thread.Sleep(10);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Reading Count: ");
            sb.Append(SensorArray.ReadingCount.ToString());
            sb.Append(Environment.NewLine);
            foreach (ISensor s in SensorArray.Sensors)
            {
                sb.Append(s.Name);
                sb.Append(Environment.NewLine);
                foreach (string sentence in s.Values.Keys)
                {
                    sb.Append("\t");
                    sb.Append(sentence);
                    sb.Append(Environment.NewLine);
                    foreach (string part in s.Values[sentence].Keys)
                    {
                        sb.Append("\t\t");
                        sb.Append(part);
                        sb.Append(": ");
                        sb.Append(s.Values[sentence][part]);
                        sb.Append(Environment.NewLine);
                    }
                }
            }
            MessageBox.Show(sb.ToString());
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            speedTB.Value = 8;
            speedTB_Scroll(null, null);
        }

        private void SkipperForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _redraw = false;
            SensorArray.Stop();
        }

        private void boatsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boatsLB.SelectedIndex >= 0)
            {
                viewPanel.SelectedBoat = boatsLB.SelectedIndex;
            }
        }

        private void RequestStatisticsUpdate()
        {
            _updateStatistics = true;
        }
        private void UpdateStatistics()
        {
            if (_updateStatistics)
            {
                ListView.CheckForIllegalCrossThreadCalls = false;
                DataTable dt = viewPanel.Statistics;

                if (statsLV.Columns.Count == 0)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        statsLV.Columns.Add(dc.ColumnName);
                    }
                }

                statsLV.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {

                    string[] subitems = new string[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        subitems[i] = dr[i].ToString();
                    }
                    ListViewItem lvi = new ListViewItem(subitems);
                    statsLV.Items.Add(lvi);
                }
                _updateStatistics = false;
            }
        }

        private void speedTB_Scroll(object sender, EventArgs e)
        {
            int value = speedTB.Value - 8;
            value = (int)Math.Pow((value), 2.0);

            if (speedTB.Value < 8)
            {
                value = -value;
            }
            if (value == 0)
            {
                viewPanel.Play = false;
                statusLBL.Text = "Pause";
            }
            else
            {
                viewPanel.Play = true;
                viewPanel.Speed = (double)value;

                if (value == 1)
                {
                    statusLBL.Text = ">";
                }
                else if (value == -1)
                {
                    statusLBL.Text = "<";
                }
                else if (value > 1)
                {
                    statusLBL.Text = ">> " + value + "x";
                }
                else if (value < -1)
                {
                    statusLBL.Text = "<< " + Math.Abs(value) + "x";
                }
            }
        }

        private void forwardBTN_Click(object sender, EventArgs e)
        {
            if (speedTB.Value < 16)
            {
                speedTB.Value = speedTB.Value + 1;
                speedTB_Scroll(null, null);
            }
        }

        private void reverseBTN_Click(object sender, EventArgs e)
        {
            if (speedTB.Value > 0)
            {
                speedTB.Value = speedTB.Value - 1;
                speedTB_Scroll(null, null);
            }
        }

        private void moveUpBTN_Click(object sender, EventArgs e)
        {
            viewPanel.CameraUp();
        }

        private void moveDownBTN_Click(object sender, EventArgs e)
        {
            viewPanel.CameraDown();
        }

        private void moveLeftBTN_Click(object sender, EventArgs e)
        {
            viewPanel.CameraLeft();
        }

        private void moveRightBTN_Click(object sender, EventArgs e)
        {
            viewPanel.CameraRight();
        }

        private void zoomOutBTN_Click(object sender, EventArgs e)
        {
            viewPanel.CameraOut();
        }

        private void zoomInBTN_Click(object sender, EventArgs e)
        {
            viewPanel.CameraIn();
        }

        private void gridCB_CheckedChanged(object sender, EventArgs e)
        {
            viewPanel.DrawGridLines = gridCB.Checked;
        }

        private void angleCB_CheckedChanged(object sender, EventArgs e)
        {
            viewPanel.DrawAngleToMark = angleCB.Checked;
        }
    }
}