using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data;

using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;

namespace AmphibianSoftware.VisualSail.Library
{
    public class GdiCameraMan : CameraMan
    {
        private const float ZOOM_MIN = 0.1f;
        private const float ZOOM_MAX = 5f;
        private const float ZOOM_INTERVAL = 0.1f;
        private const float ZOOM_DIVISOR=500f;

        private const float ROTATION_INTEVAL = 1f;
        private const float ROTATION_DIVISOR = 3f;

        private float _rotation=0f;
        private float _zoom = ZOOM_MIN;

        private float _x;
        private float _y;

        public GdiCameraMan()
        {
        }

        public override void FollowBoat(Vector3 boatPosition)
        {
            _x = boatPosition.X;
            _y = boatPosition.Y;
        }
        public override void CameraRight()
        {
            _rotation = _rotation + ROTATION_INTEVAL;
        }
        public override void CameraLeft()
        {
            _rotation = _rotation - ROTATION_INTEVAL;
        }
        public override void CameraDown()
        {
        }
        public override void CameraUp()
        {
        }
        public override void CameraIn()
        {
            _zoom = Clamp(ZOOM_MIN, ZOOM_MAX, _zoom - ZOOM_INTERVAL);
        }
        public override void CameraOut()
        {
            
            _zoom = Clamp(ZOOM_MIN, ZOOM_MAX, _zoom + ZOOM_INTERVAL);
        }
        public override void CameraMove(int x, int y)
        {
            _rotation = _rotation + x/ROTATION_DIVISOR;
        }
        public override void CameraZoom(int z)
        {
            _zoom = Clamp(ZOOM_MIN, ZOOM_MAX, _zoom + ((float)z/ZOOM_DIVISOR));
        }

        public float Clamp(float min, float max, float value)
        {
            if (min >= max)
                throw new ArgumentException("Min must be less than max");

            if (value < min)
                return min;
            else if (value >= min && value <= max)
                return value;
            else if (value > max)
                return max;
            else
                throw new InvalidOperationException("Paramaters could not be clamped");
        }

        public float Rotation
        {
            get
            {
                return _rotation;
            }
        }
        public float Zoom
        {
            get
            {
                return _zoom;
            }
        }
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
    }
}
