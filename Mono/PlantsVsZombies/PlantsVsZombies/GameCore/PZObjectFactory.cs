using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameCore
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

        public ObjectEntity createPlant(Vector2 pos)
        {
            ObjectEntity plant = GameObjectCenter.Instance.CreateObject("xml_Plant_DoublePea");

            MoveComponent moveCom = plant.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return plant;
        }

        public ObjectEntity createIcePlant(Vector2 pos)
        {
            ObjectEntity plant = GameObjectCenter.Instance.CreateObject("xml_Plant_IcePea");

            MoveComponent moveCom = plant.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return plant;
        }

        public ObjectEntity createZombie(Vector2 pos)
        {
            ObjectEntity zom = GameObjectCenter.Instance.CreateObject("xml_stand_zombie");

            MoveComponent moveCom = zom.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return zom;
        }

        public ObjectEntity createNormalBullet(Vector2 pos)
        {
            // BAasd
            ObjectEntity zom = GameObjectCenter.Instance.CreateObject("xml_stand_Bullet");

            MoveComponent moveCom = zom.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return zom;
        }

        public ObjectEntity createIceBullet(Vector2 pos)
        {
            ObjectEntity zom = GameObjectCenter.Instance.CreateObject("xml_ice_Bullet");

            MoveComponent moveCom = zom.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = pos;

            return zom;
        }
    }
}
