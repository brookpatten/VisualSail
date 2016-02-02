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
    public abstract class CameraMan
    {
        [DoNotObfuscate()]
        public enum PhotoDisplayMode { Disabled, SelectedBoat, BoatsInView, All };

        public int SelectedBoat;

        public bool DrawSatelliteImagery = true;
        public bool DrawGrid = false;
        public bool DrawAngleToMark = false;
        public bool DrawAngleToWind = false;
        public bool DrawRelativeAngleReference = false;
        public bool DrawAbsoluteAngleReference = false;
        public bool DrawPastPath = true;
        public bool DrawFuturePath = true;
        public bool ShowMarkNames = false;
        public bool DrawPlaybackSpeed = false;

        public bool ShowName = true;
        public bool ShowNumber = false;
        public bool ShowPosition = true;
        public bool ShowSpeed = false;
        public bool ShowVMGToCourse = false;
        public bool ShowDistanceToMark = false;
        public bool ShowDistanceToCourse = false;
        public bool ShowAngleToMark = false;
        public bool ShowAngleToWind = false;
        public bool ShowAngleToCourse = false;

        public int DrawPathLength = 60;
        public int DrawGridSize = 10;

        public PhotoDisplayMode _photoMode = PhotoDisplayMode.BoatsInView;
        public double _photoRatio = 0.25;

        public Texture2D CurrentPhotoTexture = null;
        public Photo CurrentPhoto = null;
        public Rectangle CurrentPhotoRectangle = default(Rectangle);
        public DateTime LastPhotoQuery = DateTime.MinValue;

        public bool ShowAnyIdentifiers
        {
            get
            {
                return ShowName || ShowNumber || ShowPosition || ShowSpeed || ShowVMGToCourse || ShowDistanceToMark || ShowDistanceToCourse || ShowAngleToMark || ShowAngleToCourse || ShowAngleToWind;
            }
        }

        public abstract void FollowBoat(Vector3 boatPosition);
        
        protected void ResetPhoto()
        {
            lock (this)
            {
                if (CurrentPhotoTexture != null)
                {
                    CurrentPhotoTexture.Dispose();
                    CurrentPhotoTexture = null;
                }
                CurrentPhoto = null;
                CurrentPhotoRectangle = default(Rectangle);
            }
        }
        public PhotoDisplayMode PhotoMode
        {
            get
            {
                return _photoMode;
            }
            set
            {
                _photoMode = value;
                ResetPhoto();
            }
        }
        public double PhotoViewportRatio
        {
            get
            {
                return _photoRatio;
            }
            set
            {
                _photoRatio = value;
                ResetPhoto();
            }
        }
        public abstract void CameraRight();
        public abstract void CameraLeft();
        public abstract void CameraDown();
        public abstract void CameraUp();
        public abstract void CameraIn();
        public abstract void CameraOut();
        public abstract void CameraMove(int x, int y);
        public abstract void CameraZoom(int z);
    }
}
