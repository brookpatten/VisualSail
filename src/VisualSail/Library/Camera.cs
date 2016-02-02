using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AmphibianSoftware.VisualSail.Library
{
    public class Camera
    {
        private float _currentX;
        private float _currentZ;
        private float _currentY;
        private float _currentLookAtX;
        private float _currentLookAtZ;
        private float _currentLookAtY;

        private float _targetX;
        private float _targetZ;
        private float _targetY;
        private float _targetLookAtX;
        private float _targetLookAtZ;
        private float _targetLookAtY;

        //private float _turnRate=0.25f;
        private float _moveRate=0.25f;

        private float _viewWidth;
        private float _viewHeight;

        private bool _onTarget = false;

        private BoundingBox _worldBounds;

        public Camera()
        {
        }
        public Camera(BoundingBox worldBounds)
        {
            _worldBounds = worldBounds;
            float startX=_worldBounds.Min.X+((_worldBounds.Max.X-_worldBounds.Min.X)/2);
            float startY=_worldBounds.Min.Y+((_worldBounds.Max.Y-_worldBounds.Min.Y)/2);
            float startZ=_worldBounds.Min.Z+((_worldBounds.Max.Z-_worldBounds.Min.Z)/2);
            this.MoveInstantly(startX, startY, startZ);
        }
        public void Resize(int height, int width)
        {
            _viewHeight = height;
            _viewWidth = width;
        }
        public void MoveInstantly(float x,float y,float z)
        {
            _currentX = x;
            _currentY = y;
            _currentZ = z;
            _onTarget = false;
        }
        public void MoveInstantly(float x, float z, float lookAtX, float lookAtZ)
        {
            _currentX = x;
            _targetX = x;
            _currentZ = z;
            _targetZ = z;
            _currentLookAtX = lookAtX;
            _targetLookAtX = lookAtX;
            _currentLookAtZ = lookAtZ;
            _targetLookAtZ = lookAtZ;
            _onTarget = false;
        }
        public void MoveInstantly(float x, float y,float z, float lookAtX, float lookAtY,float lookAtZ)
        {
            _currentX = x;
            _targetX = x;
            _currentY = y;
            _targetY = y;
            _currentZ = z;
            _targetZ = z;
            _currentLookAtX = lookAtX;
            _targetLookAtX = lookAtX;
            _currentLookAtY = lookAtY;
            _targetLookAtY = lookAtY;
            _currentLookAtZ = lookAtZ;
            _targetLookAtZ = lookAtZ;
            _onTarget = true;
        }
        public void MoveSmoothly(float x, float z, float lookAtX, float lookAtZ)
        {
            _targetX = x;
            _targetZ = z;
            _targetLookAtX = lookAtX;
            _targetLookAtZ = lookAtZ;
        }
        public void MoveSmoothly(float x, float y,float z, float lookAtX, float lookAtY,float lookAtZ)
        {
            _targetX = x;
            _targetY = y;
            _targetZ = z;
            _targetLookAtX = lookAtX;
            _targetLookAtZ = lookAtZ;
            _targetLookAtY = lookAtY;
        }
        public void UpdatePosition()
        {
            _currentX = UpdateValue(_currentX, _targetX, _moveRate);
            _currentZ = UpdateValue(_currentZ, _targetZ, _moveRate);
            _currentY = UpdateValue(_currentY, _targetY, _moveRate);
            _currentLookAtX = UpdateValue(_currentLookAtX, _targetLookAtX, _moveRate);
            _currentLookAtZ = UpdateValue(_currentLookAtZ, _targetLookAtZ, _moveRate);
            _currentLookAtY = UpdateValue(_currentLookAtY, _targetLookAtY, _moveRate);

            _onTarget = (_currentX == _targetX && _currentY == _targetY && _currentZ == _targetZ && _currentLookAtX == _targetLookAtX && _currentLookAtY == _targetLookAtY && _currentLookAtZ == _targetLookAtZ);
        }
        public bool OnTarget
        {
            get
            {
                return _onTarget;
            }
        }
        private float UpdateValue(float current, float target, float rate)
        {
            if (current != target)
            {
                if (current < target)
                {
                    if (target - current >= rate)
                    {
                        current = current + ((target - current) * rate);
                    }
                    else
                    {
                        current = target;
                    }
                }
                else if (current > target)
                {
                    if (current-target >= rate)
                    {
                        current = current - ((current - target) * rate);
                    }
                    else
                    {
                        current = target;
                    }
                }
            }
            return current;
        }
        public Matrix ViewMatrix
        {
            get
            {
                //return Matrix.CreateLookAt(new Vector3(_currentX, _currentY, _currentZ), new Vector3(_currentLookAtX, _currentLookAtY, _currentLookAtZ), new Vector3(0, 1, 0));
                return Matrix.CreateLookAt(Location, LookAt, new Vector3(0, 1, 0));
            }
        }
        //public Matrix WorldMatrix
        //{
        //    get
        //    {
        //        return Matrix.Identity;
        //    }
        //}
        public static float FarClipDistance
        {
            get
            {
                return 2000f;
            }
        }
        public Matrix ProjectionMatrix
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1f/* _viewWidth / _viewHeight*/, 0.1f, FarClipDistance);
            }
        }
        public void ConfigureBasicEffect(BasicEffect effect)
        {
            effect.View = ViewMatrix;
            effect.Projection = ProjectionMatrix;
            //effect.World = WorldMatrix;
        }
        private Vector3 BoundedLocation
        {
            get
            {
                if (_worldBounds != null)
                {
                    return Vector3.Clamp(new Vector3(_currentX, _currentY, _currentZ),_worldBounds.Min,_worldBounds.Max);
                }
                else
                {
                    return new Vector3(_currentX, _currentY, _currentZ);
                }
            }
        }
        public Vector3 Location
        {
            get
            {
                Location = BoundedLocation;
                return new Vector3(_currentX, _currentY, _currentZ);
            }
            set
            {
                _currentX = value.X;
                _currentY = value.Y;
                _currentZ = value.Z;
            }
        }
        public Vector3 LookAt
        {
            get
            {
                return new Vector3(_currentLookAtX, _currentLookAtY, _currentLookAtZ);
            }
            set
            {
                _currentLookAtX = value.X;
                _currentLookAtY = value.Y;
                _currentLookAtZ = value.Z;
            }
        }
    }
}
