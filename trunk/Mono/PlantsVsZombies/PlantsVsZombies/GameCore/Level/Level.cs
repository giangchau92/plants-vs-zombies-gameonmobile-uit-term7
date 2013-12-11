using SCSEngine.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameCore.Level
{
    public class Level : ISerializable
    {
        public string Name { get; set; }
        public List<string> Zombies { get; set; }
        public int NumberFrom { get; set; }
        public int NumberTo { get; set; }
        public double TimeNextWave { get; set; }

        public Level()
        {
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            Name = deserializer.DeserializeString("Name");

            var waveDers = deserializer.DeserializeAll("Wave");
            foreach (var item in waveDers)
            {
                
            }
        }
    }
}
