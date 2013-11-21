using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.Components;
using PlantVsZombie.GameObjects;
using PlantVsZombie.GameObjects.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GameCore
{
    public class PZObjectFactory
    {
        public static PZObjectFactory _instance = null;
        public static PZObjectFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PZObjectFactory();
                return _instance;
            }
        }

        private PZObjectFactory() { }

        public NormalPlant createPlant(Vector2 pos)
        {
            NormalPlant plant = new NormalPlant();

            MoveComponent moveCom = plant.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return plant;
        }

        public IcePlant createIcePlant(Vector2 pos)
        {
            IcePlant plant = new IcePlant();

            MoveComponent moveCom = plant.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return plant;
        }

        public NormalZombie createZombie(Vector2 pos)
        {
            NormalZombie zom = new NormalZombie();

            MoveComponent moveCom = zom.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return zom;
        }
    }
}
