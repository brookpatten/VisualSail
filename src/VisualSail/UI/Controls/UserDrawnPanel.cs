using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AmphibianSoftware.VisualSail.UI.Controls
{
    public class UserDrawnPanel:Panel
    {
        public UserDrawnPanel():base()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }
    }
}
