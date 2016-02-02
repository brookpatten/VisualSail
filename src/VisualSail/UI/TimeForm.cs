using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class TimeForm : DockContent
    {
        private Replay _replay;
        private bool _showInRaceTime = true;
        private string _panelText = "";
        private Font _panelFont = null;
        private PointF _panelTextDrawLocation = default(PointF);

        public TimeForm(Replay replay)
        {
            _replay = replay;
            InitializeComponent();
            this.AutoScaleDimensions = new SizeF(182, 216);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(182, 216);
            //this.MinimumSize = new Size(182, 216);
        }

        private Replay Replay
        {
            get
            {
                return _replay;
            }
            set
            {
                _replay = value;
            }
        }

        private void speedTB_Scroll(object sender, EventArgs e)
        {
            int value = speedTB.Value - 8;
            value = (int)Math.Pow((value), 2.0);

            _replay.TargetTime = _replay.SimulationTime;

            if (speedTB.Value < 8)
            {
                value = -value;
            }
            if (value == 0)
            {
                //Engine.Pause();
                _replay.Speed = 0;
                statusLBL.Text = "Pause";
            }
            else
            {
                //Engine.Play();
                _replay.Speed = (double)value;

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

        public void UpdateTimeString()
        {
            if (_showInRaceTime)
            {
                TimeSpan span = _replay.SimulationTime - _replay.Race.UtcStart;
                _panelText = string.Format("{0}{1:00}:{2:00}:{3:00}", span.Ticks < 0 ? "-" : "", Math.Abs(span.Hours), Math.Abs(span.Minutes), Math.Abs(span.Seconds));
            }
            else
            {
                _panelText = TimeZoneInfo.ConvertTimeFromUtc(_replay.SimulationTime, _replay.Race.Lake.TimeZone).ToLongTimeString();
            }
            timeUDP.Invalidate();
        }

        public void UpdateTime()
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            //timeLBL.Text = Engine.SimulationTime.ToLongTimeString();

            UpdateTimeString();

            positionTB.Maximum = (int)((_replay.Race.UtcReplayEnd.Ticks / 10000000) - (_replay.Race.UtcReplayStart.Ticks / 10000000));
            positionTB.Minimum = 0;
            if (_replay.TargetTime != null)
            {
                positionTB.Value = (int)((_replay.TargetTime.Value.Ticks / 10000000) - (_replay.Race.UtcReplayStart.Ticks / 10000000));
            }
            else
            {
                positionTB.Value = (int)((_replay.SimulationTime.Ticks / 10000000) - (_replay.Race.UtcReplayStart.Ticks / 10000000));
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

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            speedTB.Value = 8;
            speedTB_Scroll(null, null);
        }

        private void positionTB_Scroll(object sender, EventArgs e)
        {
            long ticks = _replay.Race.UtcReplayStart.Ticks + (((long)positionTB.Value) * 10000000);
            DateTime skipToDate = new DateTime(ticks);
            _replay.TargetTime = skipToDate;
        }

        private void TimeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void timeUDP_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, timeUDP.ClientRectangle.Width, timeUDP.ClientRectangle.Height);
            if (_panelFont == null || _panelTextDrawLocation.X+e.Graphics.MeasureString(_panelText, _panelFont).Width > timeUDP.ClientRectangle.Width)
            {
                _panelFont = FindFontForArea(_panelText, timeUDP.ClientRectangle, e.Graphics);
                _panelTextDrawLocation = FindCenteredTextDrawPoint(_panelText, _panelFont, timeUDP.ClientRectangle, e.Graphics);
            }
            e.Graphics.DrawString(_panelText, _panelFont, new SolidBrush(Color.FromArgb(0,255,0)), _panelTextDrawLocation);
        }

        private void timeUDP_Resize(object sender, EventArgs e)
        {
            _panelFont = null;
        }

        private Font FindFontForArea(string text, Rectangle area, Graphics g)
        {
            int size = timeUDP.ClientRectangle.Height / 3;
            Font f=null;
            do
            {
                f = new Font(FontFamily.GenericMonospace, (float)size, GraphicsUnit.Pixel);
                size = size - 2;
            }
            while (g.MeasureString(text, f).Width > area.Width || g.MeasureString(text, f).Height > area.Height);
            return f;
        }
        private PointF FindCenteredTextDrawPoint(string text, Font font, Rectangle area, Graphics g)
        {
            float textWidth = g.MeasureString(text, font).Width;
            float textHeight = g.MeasureString(text, font).Height;

            float x = ((float)area.Width - textWidth) / 2f;
            float y = ((float)area.Height - textHeight) / 2f;
            return new PointF(x, y);
        }

        private void timeUDP_MouseClick(object sender, MouseEventArgs e)
        {
            _showInRaceTime = !_showInRaceTime;
            _panelFont = null;
            UpdateTimeString();
        }
    }
}
