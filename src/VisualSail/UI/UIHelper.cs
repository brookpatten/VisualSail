using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using WeifenLuo.WinFormsUI.Docking;

namespace AmphibianSoftware.VisualSail.UI
{
    public static class UIHelper
    {
        public static void AutoResizeListViewColumns(ListView lv)
        {
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                int headerWidth;
                int contentWidth;
                lv.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                contentWidth = lv.Columns[i].Width;
                lv.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                headerWidth = lv.Columns[i].Width;
                if (contentWidth > headerWidth)
                {
                    lv.Columns[i].Width = contentWidth;
                }
                else
                {
                    lv.Columns[i].Width = headerWidth;
                }
            }
        }
        public static Rectangle FindCenteredPosition(DockPanel parent, DockContent child)
        {
            int x;
            int y;
            int width;
            int height;

            if (child.Width < parent.Width && child.Height < parent.Height)
            {
                width = child.Width;
                height = child.Height;
                x = (parent.Width - child.Width) / 2;
                y = (parent.Height - child.Height) / 2;
            }
            else
            {
                width = parent.Width;
                height = parent.Height;
                x = 0;
                y = 0;
            }

            return new Rectangle(x, y, width, height);
        }
    }
}
