using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SSCEngine.Serialization
{
    public interface ISerialization
    {
        ISerializer CreateSerializer(Stream s);
        IDeserializer CreateDeserializer(Stream s);
    }
}