using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SCSEngine.Serialization.XmlSerialization
{
    internal class XmlDeserializer : IDeserializer
    {
        public XmlDeserializer(XElement elem)
        {
            this.elem = elem;
        }

        private XElement elem;

        public void Deserialize(string serName, ISerializable ser)
        {
            XElement subElem = this.elem.Element(serName);

            ser.Deserialize(new XmlDeserializer(subElem));
        }

        public string DeserializeString(string serName)
        {
            return this.elem.Element(serName).Value;
        }

        public int DeserializeInteger(string serName)
        {
            return int.Parse(this.elem.Element(serName).Value);
        }

        public double DeserializeDouble(string serName)
        {
            return this.parseStringToDouble(this.elem.Element(serName).Value);
        }

        // device 520 error
        private double parseStringToDouble(string s)
        {
            try
            {

                double rez = 0, fact = 1;
                int i = 0;
                if (s[i] == '-')
                {
                    ++i;
                    fact = -1;
                }

                for (bool point_seen = false; i < s.Length; ++i)
                {
                    if (s[i] == '.')
                    {
                        point_seen = true;
                        continue;
                    }
                    int d = s[i] - '0';
                    if (d >= 0 && d <= 9)
                    {
                        if (point_seen) fact /= 10.0;
                        rez = rez * 10.0 + (double)d;
                    }
                }

                return rez * fact;
            }
            catch
            {
                return 0.0;
            }
        }

        public ICollection<IDeserializer> DeserializeAll(string serName)
        {
            ICollection<IDeserializer> deserializers = new List<IDeserializer>();

            IEnumerable<XElement> elements = this.elem.Elements();

            foreach (var subElem in elements)
            {
                if (subElem.Name == serName)
                    deserializers.Add(new XmlDeserializer(subElem));
            }

            return deserializers;
        }

        public IDeserializer SubDeserializer(string serName)
        {
            return new XmlDeserializer(this.elem.Element(serName));
        }
    }
}
