using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Library
{
    public interface ISensor
    {
        Notify OnUpdate { get; set; }
        RawReceive OnReceive { get; set; }
        void Start();
        void Stop();
        Dictionary<string, Dictionary<string, string>> Values { get; }
        string Name { get; }
    }
}
