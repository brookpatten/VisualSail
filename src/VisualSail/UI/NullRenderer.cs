using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Threading;

using Microsoft.Xna.Framework;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.Video;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Data.Statistics;

namespace AmphibianSoftware.VisualSail.UI
{
    public class NullRenderer:Renderer
    {
        public override void Initialize(Replay replay)
        {
            base.Initialize(replay);
        }
        public override void Reset() 
        { 
        }
        public override void Resize() 
        { 
        }
        public override void Shutdown() 
        { 
        }
        public override void AddViewPort(IViewPort viewport) 
        { 
        }
        public override void RemoveViewPort(IViewPort viewport) 
        { 
        }
        public override void RenderAll() 
        { 
        }
        private Vector3 ProjectedPointToWorld(ProjectedPoint point, CameraMan cameraMan)
        {
            Vector3 v = new Vector3((float)point.Northing, (float)point.Height, (float)point.Easting);
            return v;
        }
    }
}
