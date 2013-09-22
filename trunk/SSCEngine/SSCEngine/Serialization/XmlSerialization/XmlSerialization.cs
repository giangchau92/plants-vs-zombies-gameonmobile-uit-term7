using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SSCEngine.Serialization.XmlSerialization
{
    public class XmlSerialization : ISerialization
    {
        public ISerializer CreateSerializer(System.IO.Stream s)
        {
        }

        public IDeserializer CreateDeserializer(System.IO.Stream s)
        {
            throw new NotImplementedException();
        }
    }
}
