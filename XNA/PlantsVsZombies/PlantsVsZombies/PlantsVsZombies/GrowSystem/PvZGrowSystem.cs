using Microsoft.Xna.Framework;
using SCSEngine.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GrowSystem
{
    public class PvZGrowSystem : ISerializable
    {
        private Game game;
        private PvZPlantShadowFactoryBank shadowFB;

        public PvZPlantShadowFactoryBank ShadowFactoryBank
        {
            get { return shadowFB; }
            set { shadowFB = value; }
        }
        private PvZGrowButtonFactoryBank buttonFB;

        public PvZGrowButtonFactoryBank ButtonFactoryBank
        {
            get { return buttonFB; }
            set { buttonFB = value; }
        }

        public PvZGrowSystem(Game game)
        {
            this.game = game;
            this.shadowFB = new PvZPlantShadowFactoryBank(this.game);
            this.buttonFB = new PvZGrowButtonFactoryBank(this.shadowFB);
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            deserializer.Deserialize("PlantShadows", shadowFB);
            deserializer.Deserialize("Buttons", buttonFB);
        }
    }
}
