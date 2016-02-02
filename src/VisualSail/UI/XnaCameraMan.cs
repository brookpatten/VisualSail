using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.PostBuild;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AmphibianSoftware.VisualSail.Library
{
    public class XnaCameraMan:CameraMan
    {
        private Camera _camera;
        private float _horizontalRotation;
        private float _verticalRotation;
        private float _zoom;

        public XnaCameraMan(Camera camera,float horizontal,float vertical,float zoom)
        {
            _camera = camera;
            _horizontalRotation = horizontal;
            _verticalRotation = vertical;
            _zoom = zoom;
        }

        public override void FollowBoat(Vector3 boatPosition)
        {
            Vector3 pos = new Vector3(0, 0, Zoom);
            pos = Vector3.Transform(pos, Matrix.CreateRotationX(VerticalRotation) * Matrix.CreateRotationY(HorizontalRotation));
            pos.X = pos.X + (boatPosition.X);
            pos.Y = pos.Y + (boatPosition.Y);
            pos.Z = pos.Z + (boatPosition.Z);

            if (Camera.Location == new Vector3(0, 0, 0) || Camera.Location == null)
            {
                Camera.MoveInstantly(pos.X, pos.Y, pos.Z, boatPosition.X, boatPosition.Y, boatPosition.Z);
            }
            else
            {
                Camera.MoveSmoothly(pos.X, pos.Y, pos.Z, boatPosition.X, boatPosition.Y, boatPosition.Z);
            }
        }
        public override void CameraRight()
        {
            _horizontalRotation += (MathHelper.Pi) / 20f;
        }
        public override void CameraLeft()
        {
            _horizontalRotation -= (MathHelper.Pi) / 20f;
        }
        public override void CameraDown()
        {
            if (_verticalRotation - (MathHelper.Pi) / 20f >= MathHelper.Pi)
            {
                _verticalRotation -= MathHelper.Pi / 20f;
            }
        }
        public override void CameraUp()
        {
            if (_verticalRotation + (MathHelper.Pi) / 20f < MathHelper.Pi + MathHelper.PiOver2)
            {
                _verticalRotation += (MathHelper.Pi) / 20f;
            }
        }
        public override void CameraIn()
        {
            if (_zoom - 2 > 0)
            {
                _zoom -= 2;
            }
        }
        public override void CameraOut()
        {
            _zoom += 2;
        }
        public override void CameraMove(int x, int y)
        {
            _horizontalRotation += ((MathHelper.Pi) / 200f) * (float)x;

            if ((_verticalRotation + (((MathHelper.Pi) / 200f) * (float)y) >= MathHelper.Pi) && (_verticalRotation + (((MathHelper.Pi) / 200f) * (float)y) < MathHelper.Pi + MathHelper.PiOver2))
            {
                _verticalRotation += ((MathHelper.Pi) / 200f) * (float)y;
            }
        }
        public override void CameraZoom(int z)
        {
            if ((z < 0 && _zoom + z > 0) || z > 0)
            {
                _zoom = _zoom + z;
            }
        }

        public Camera Camera
        {
            get
            {
                return _camera;
            }
        }
        public float HorizontalRotation
        {
            get
            {
                return _horizontalRotation;
            }
        }
        public float VerticalRotation
        {
            get
            {
                return _verticalRotation;
            }
        }
        public float Zoom
        {
            get
            {
                return _zoom;
            }
        }
    }
}
