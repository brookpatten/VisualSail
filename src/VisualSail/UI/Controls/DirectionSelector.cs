using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AmphibianSoftware.VisualSail.UI.Controls
{
    public partial class DirectionSelector : UserControl
    {
        private bool _mouseDown = false;
        private double _angle = 0f;
        private int _diameter;
        private double _centerX;
        private double _centerY;
        private bool _enabled = true;
        //public delegate void EventHandler(object sender, System.EventArgs e);
        private EventHandler _eventHandlerDelegate;

        public DirectionSelector()
        {
            InitializeComponent();
        }

        private void DirectionSelector_Paint(object sender, PaintEventArgs e)
        {
            Pen circlePen;
            Pen linePen;
            Brush textBrush;
            Brush headingBrush;

            int degrees = (int)((_angle / (Math.PI * 2.0)) * 360.0);
            if (degrees < 0)
            {
                degrees = 360 + degrees;
            }
            string degreeString = degrees.ToString() + "°";
            
            if (_enabled)
            {
                circlePen = new Pen(Color.Blue);
                linePen = new Pen(Color.Red);
                textBrush = Brushes.Black;
                headingBrush = Brushes.Green;
            }
            else
            {
                circlePen = new Pen(Color.LightGray);
                linePen = new Pen(Color.DarkGray);
                textBrush = Brushes.LightGray;
                headingBrush = Brushes.DarkGray;
            }

            if (this.Width < this.Height)
            {
                _diameter = this.Width;
            }
            else
            {
                _diameter = this.Height;
            }

            e.Graphics.DrawEllipse(circlePen, new Rectangle(0, 0, _diameter - 5, _diameter - 5));
            Font headings=new Font(FontFamily.GenericSansSerif,10f);
            e.Graphics.DrawString("N", headings, textBrush, _diameter / 2 - 8, 0);
            e.Graphics.DrawString("S", headings, textBrush, _diameter / 2 - 8, _diameter - 5 - 10 - 8);
            e.Graphics.DrawString("E", headings, textBrush, _diameter - 5 - 8 - 8, _diameter / 2 - 10);
            e.Graphics.DrawString("W", headings, textBrush, 8, _diameter / 2 - 10);
            
            _centerX = _diameter / 2;
            _centerY = _diameter / 2;
            double pointX = 0;
            double pointY = 0;
            double pointAX = 0;
            double pointAY = 0;
            double pointBX = 0;
            double pointBY = 0;

            pointX = _centerX + (((double)_diameter / 3.0) * Math.Cos(_angle - (Math.PI / 2.0)));
            pointY = _centerY + (((double)_diameter / 3.0) * Math.Sin(_angle - (Math.PI / 2.0)));

            pointAX = _centerX + (((double)_diameter / 3.5) * Math.Cos(_angle - (Math.PI / 2.0) + .2));
            pointAY = _centerY + (((double)_diameter / 3.5) * Math.Sin(_angle - (Math.PI / 2.0) + .2));

            pointBX = _centerX + (((double)_diameter / 3.5) * Math.Cos(_angle - (Math.PI / 2.0) - .2));
            pointBY = _centerY + (((double)_diameter / 3.5) * Math.Sin(_angle - (Math.PI / 2.0) - .2));

            e.Graphics.DrawLine(linePen, (float)_centerX, (float)_centerY, (float)pointX, (float)pointY);
            e.Graphics.DrawLine(linePen, (float)pointAX, (float)pointAY, (float)pointX, (float)pointY);
            e.Graphics.DrawLine(linePen, (float)pointBX, (float)pointBY, (float)pointX, (float)pointY);

            e.Graphics.DrawString(degreeString, headings, headingBrush, _diameter / 2 - 10, _diameter / 2 - 10);
        }

        public double Value
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
            }
        }

        new public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                this.Invalidate();
            }
        }

        private void DirectionSelector_MouseDown(object sender, MouseEventArgs e)
        {
            if (_enabled)
            {
                _mouseDown = true;
            }
        }

        private void DirectionSelector_MouseUp(object sender, MouseEventArgs e)
        {
            if (_enabled)
            {
                _mouseDown = false;
            }
        }

        private void DirectionSelector_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown&&_enabled)
            {
                _angle = Math.Atan2((double)e.Y - (double)_centerY, (double)e.X - (double)_centerX);
                _angle = _angle + (Math.PI / 2.0);
                //_angle = Math.Atan2((double)_centerY - (double)e.Y, (double)_centerX - (double)e.X);
                this.Invalidate();
                _eventHandlerDelegate(sender, new EventArgs());
            }
        }

        public event EventHandler ValueChanged
        {
            add
            {
                _eventHandlerDelegate += value;
            }
            remove
            {
                _eventHandlerDelegate -= value;
            }
        }
    }
}
