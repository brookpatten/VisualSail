using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AmphibianSoftware.VisualSail
{
    public static class VertexDeclarationHelper
    {
        private static Dictionary<Type, VertexDeclaration> _declarations;
        static VertexDeclarationHelper()
        {
            _declarations = new Dictionary<Type, VertexDeclaration>();
        }
        public static void Clear()
        {
            _declarations.Clear();
        }
        public static void Add(Type vertexType,VertexDeclaration dec)
        {
            if(!_declarations.Keys.Contains(vertexType))
            {
                _declarations.Add(vertexType, dec);
            }
        }
        public static bool HasDeclaration(Type vertexType)
        {
            return _declarations.Keys.Contains(vertexType);
        }
        public static VertexDeclaration Get(Type vertexType)
        {
            if (_declarations.Keys.Contains(vertexType))
            {
                return _declarations[vertexType];
            }
            else
            {
                throw new Exception("This VertexDeclaration has not been defined yet");
            }
        }
    }
}
