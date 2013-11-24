using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.Serialization
{
    public interface ISerializable
    {
        void Serialize(ISerializer serializer);
        void Deserialize(IDeserializer deserializer);
    }
}