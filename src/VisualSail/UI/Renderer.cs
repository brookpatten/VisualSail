using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.UI
{
    public abstract class Renderer
    {
        private Replay _replay;

        protected Renderer()
        {
        }

        protected Race Race
        {
            get
            {
                return _replay.Race;
            }
        }
        protected Replay Replay
        {
            get
            {
                return _replay;
            }
        }
        public virtual void Initialize(Replay replay)
        {
            _replay = replay;
        }
        public abstract void Reset();
        public abstract void Resize();
        //void LoadContent();
        public abstract void Shutdown();
        public abstract void AddViewPort(IViewPort viewport);
        public abstract void RemoveViewPort(IViewPort viewport);
        //IBoundingBox DrawPhoto(Photo photo,Rectangle location,IViewPort vp);
        public abstract void RenderAll();
    }

    public struct AgnosticBoundingBox
    {
        public float XMin;
        public float YMin;
        public float ZMin;

        public float XMax;
        public float YMax;
        public float ZMax;
    }
}
