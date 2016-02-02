using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmphibianSoftware.VisualSail.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AmphibianSoftware.VisualSail.Library
{
    public delegate void Notify();
    public delegate void RawReceive(string line);
    public delegate Vector3 ProjectedPointToWorld(ProjectedPoint point,CameraMan cameraMan);
    public delegate ProjectedPoint ProjectedPointFromWorld(Vector3 v);
}
