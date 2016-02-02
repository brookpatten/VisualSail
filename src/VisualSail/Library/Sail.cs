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
    public struct VertexPositionNormalColored
        {
            public Vector3 Position;
            public Color Color;
            public Vector3 Normal;

            public static int SizeInBytes = 7 * 4;
            public static VertexElement[] VertexElements = new VertexElement[]
              {
                  new VertexElement( 0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0 ),
                  new VertexElement( 0, sizeof(float) * 3, VertexElementFormat.Color, VertexElementMethod.Default, VertexElementUsage.Color, 0 ),
                  new VertexElement( 0, sizeof(float) * 4, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0 ),
              };
        }
    public class Sail
    {
        public enum SailType{Main,Jib,Spinnaker}
        private SailType _sailType;
        private float _height;
        private float _width;
        private float _depth;
        private float _offset;
        private float _boomHeight;
        private bool _needsBuild = false;
        //VertexPositionColor
        private VertexPositionNormalColored[] _vertexes;

        public Sail(SailType sailType,float height, float width, float boomHeight)
        {
            _sailType = sailType;
            _boomHeight = boomHeight;
            _height = height;
            _width = width;
            _depth = 0;
            _offset = 0;
            _needsBuild = true;
        }
        public Sail(SailType sailType,float height, float width,float depth,float boomHeight)
        {
            _sailType = sailType;
            _boomHeight = boomHeight;
            _height = height;
            _width = width;
            _depth = depth;
            _offset = 0;
            _needsBuild = true;
        }
        private void RebuildVertices(GraphicsDevice dev)
        {
            int segCount = 50;
            List<Vector3> controlPoints = new List<Vector3>();
            List<Vector3> curvePoints;

            if (_sailType == SailType.Main)
            {
                controlPoints.Add(new Vector3(0, _boomHeight + _height, 0));
                controlPoints.Add(new Vector3(_depth, ((_boomHeight + _height) / 10f) * 9f, _width));
                controlPoints.Add(new Vector3(0, _boomHeight, _width));
                curvePoints = BezierHelper.CreateBezier(segCount, controlPoints);

                curvePoints.Insert(0, new Vector3(0, _boomHeight, 0));
                _vertexes = new VertexPositionNormalColored[curvePoints.Count];
                for (int i = 0; i < curvePoints.Count; i++)
                {
                    _vertexes[i].Position = curvePoints[i];
                    _vertexes[i].Color = Microsoft.Xna.Framework.Graphics.Color.WhiteSmoke;
                }
            }
            else if (_sailType == SailType.Jib)
            {
                controlPoints.Add(new Vector3(0, _boomHeight+_height, 0));//top
                controlPoints.Add(new Vector3(_depth, _boomHeight + (_height / 2), 0));//middle leech
                controlPoints.Add(new Vector3(_offset, _boomHeight, 0));//bottom back
                curvePoints = BezierHelper.CreateBezier(segCount, controlPoints);

                curvePoints.Insert(0, new Vector3(0, _boomHeight, -_width));//bottom front
                _vertexes = new VertexPositionNormalColored[curvePoints.Count];
                for (int i = 0; i < curvePoints.Count; i++)
                {
                    _vertexes[i].Position = curvePoints[i];
                    _vertexes[i].Color = Microsoft.Xna.Framework.Graphics.Color.WhiteSmoke;
                }
            }

            //set up normals
            for (int i = 1; i < _vertexes.Length-1; i++)
            {
                Vector3 first = _vertexes[i].Position - _vertexes[i + 1].Position;
                Vector3 second = _vertexes[0].Position - _vertexes[i].Position;
                Vector3 normal = Vector3.Cross(first, second);
                normal.Normalize();
                _vertexes[0].Normal += normal;
                _vertexes[i].Normal += normal;
                _vertexes[i + 1].Normal += normal;
            }

            for (int i = 0; i < _vertexes.Length; i++)
            {
                _vertexes[i].Normal.Normalize();
                _vertexes[i].Color = Color.White;
            }


            //_vbuffer = new VertexBuffer(dev, VertexPositionColor.SizeInBytes * _vertexes.Length, BufferUsage.WriteOnly);
            //_vbuffer.SetData(_vertexes);

            

            _needsBuild = false;
        }
        public void Draw(GraphicsDevice dev)
        {
            dev.VertexDeclaration = VertexDeclarationHelper.Get(typeof(VertexPositionNormalColored));
            dev.DrawUserPrimitives<VertexPositionNormalColored>(PrimitiveType.TriangleFan, _vertexes, 0, _vertexes.Length-2);
        }
        public void Draw(GraphicsDevice dev,float depth,float offset)
        {
            if (depth != _depth || _needsBuild || offset != _offset)
            {
                _offset = offset;
                _depth = depth;
                RebuildVertices(dev);
            }
            Draw(dev);
        }
    }
}
