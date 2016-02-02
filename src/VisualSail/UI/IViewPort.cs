using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using AmphibianSoftware.VisualSail.Library;

namespace AmphibianSoftware.VisualSail.UI
{
    public interface IViewPort
    {
        Control RenderTarget { get; }
        CameraMan CameraMan { get; set; }
        /*Dictionary<ReplayBoat, int>*/
        void SetBoatList(List<ReplayBoat> boats);
        ShutdownViewPort Shutdown { set; }
        Point? ClickedPoint { get; set; }
        void SetSelectedBoatIndex(int index);
        AmphibianSoftware.VisualSail.Data.Statistics.StatisticUnitType StatisticUnitType{ get; }
        DateTime CreatedAt { get; }
        void SetMaxSize(int width,int height);
        bool HasHandle { get; }
        RecorderState Record { get; set; }
        Size RecordingSize { get; set; }
        string RecordingPath { get; set; }
        string ScreenshotPath { get; set; }
    }
    public enum RecorderState { Ready, Recording, Paused, Disabled };
}
