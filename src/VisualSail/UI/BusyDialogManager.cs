using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using AmphibianSoftware.VisualSail.Library;

namespace AmphibianSoftware.VisualSail.UI
{
    public static class BusyDialogManager
    {
        private static SkipperMDI _parent;
        private static BusyDialog _bd;
        static BusyDialogManager()
        {
        }
        public static void SetParent(SkipperMDI parent)
        {
            _parent = parent;
        }
        public static void Show(string detail)
        {
            //_parent.ShowStatus(detail);
            //_parent.UseWaitCursor = true;
            _parent.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        }
        public static void Hide()
        {
            //_parent.UseWaitCursor = false;
            //_parent.HideStatus();
            _parent.Cursor = System.Windows.Forms.Cursors.Default;
        }
    }
}
