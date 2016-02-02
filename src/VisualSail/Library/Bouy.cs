/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Data.Statistics;

namespace AmphibianSoftware.VisualSail.Library
{
    public class Bouy : FloatingObject
    {
        //private Model bouyModel;
        private float waterLevel;
        private CoordinatePoint _location;
        private string _name;

        public void LoadResources(GraphicsDevice device, ContentManager content)
        {
            //bouyModel = content.Load<Model>(ContentHelper.ContentPath+"bouy");
        }
        public float WaterLevel
        {
            get
            {
                return waterLevel;
            }
            set
            {
                waterLevel = value;
            }
        }
        //public void Draw(GraphicsDevice device, Camera camera)
        //{
        //    foreach (ModelMesh mesh in bouyModel.Meshes)
        //    {
        //        foreach (BasicEffect mfx in mesh.Effects)
        //        {
        //            //mfx.DiffuseColor = new Vector3(255, 128, 64);
        //            mfx.EnableDefaultLighting();
        //            mfx.AmbientLightColor = new Vector3(1, 0.5f, 0);
        //            mfx.DiffuseColor = new Vector3(1, 0.5f, 0);
        //            mfx.SpecularColor = new Vector3(1, 0.5f, 0);
        //            mfx.World = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(ProjectedPoint.ToWorld());
        //            camera.ConfigureBasicEffect(mfx);
        //        }
        //        mesh.Draw();
        //    }
        //}
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public CoordinatePoint Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                this.ProjectedPoint = _location.Project();
            }
        }
    }
}
*/