using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.Library
{
    public class FloatingObject
    {
        private ProjectedPoint _projectedPoint;
        public ProjectedPoint ProjectedPoint
        {
            get
            {
                return _projectedPoint;
            }
            set
            {
                _projectedPoint = value;
            }
        }
    }
}
