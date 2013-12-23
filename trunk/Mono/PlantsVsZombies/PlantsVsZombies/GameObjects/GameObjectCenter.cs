using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Behaviors.Implements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.ResourceManagement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using PlantsVsZombies.GameComponents.Behaviors.Zombie;
using SCSEngine.Serialization.XmlSerialization;
using SCSEngine.Serialization;

namespace PlantsVsZombies.GameObjects
{
    public class GameObjectCenter
    {
        private static GameObjectCenter _instance = null;
        public static GameObjectCenter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObjectCenter();
                return _instance;
            }
        }

        private const string zombie_config = "Xml/Zombies.xml";
        private const string plant_config = "Xml/Plants.xml";
        private const string bullet_config = "Xml/Bullets.xml";

        private IDictionary<string, ObjectEntityFactory> objectTemplates = new Dictionary<string, ObjectEntityFactory>();

        public void InitEnity()
        {
            // Init zombie
            loadZombie();
            // Init plant
            loadPlant();
            // Init Bullet
            loadBullet();
        }

        public ObjectEntity CreateObject(string te)
        {
            if (objectTemplates.ContainsKey(te))
            {
                return objectTemplates[te].CreateEntity();
            }
            return null;
        }

        private Stream getXml(string config_url)
        {
            Stream stream = new FileStream(config_url, FileMode.Open, FileAccess.Read);//Assembly.GetExecutingAssembly().GetManifestResourceStream("PlantVsZombies.Xml.Zombies.xml");
            //XDocument mXDocument = new XDocument();
            //mXDocument = XDocument.Load(stream);
            return stream;
        }

        private void loadZombie()
        {
            IResourceManager resourceManager = SCSServices.Instance.ResourceManager;

            Stream docs = getXml(zombie_config);
            IDeserializer deser = XmlSerialization.Instance.Deserialize(docs);
            //IDeserializer deserZombiess = deser.SubDeserializer("Zombies");
            var deserZombies = deser.DeserializeAll("Zombie");

            foreach (var item in deserZombies)
            {
                string name = item.DeserializeString("Name");
                ObjectEntityFactory objectFac = new ObjectEntityFactory();
                objectFac.Deserialize(item);
                this.objectTemplates.Add(name, objectFac);
            }
        }

        private void loadPlant()
        {
            IResourceManager resourceManager = SCSServices.Instance.ResourceManager;

            Stream docs = getXml(plant_config);
            IDeserializer deser = XmlSerialization.Instance.Deserialize(docs);
            var deserZombies = deser.DeserializeAll("Plant");

            foreach (var item in deserZombies)
            {
                string name = item.DeserializeString("Name");
                ObjectEntityFactory objectFac = new ObjectEntityFactory();
                objectFac.Deserialize(item);
                this.objectTemplates.Add(name, objectFac);
            }
        }

        private void loadBullet()
        {
            IResourceManager resourceManager = SCSServices.Instance.ResourceManager;

            Stream docs = getXml(bullet_config);
            IDeserializer deser = XmlSerialization.Instance.Deserialize(docs);
            var deserZombies = deser.DeserializeAll("Bullet");

            foreach (var item in deserZombies)
            {
                string name = item.DeserializeString("Name");
                ObjectEntityFactory objectFac = new ObjectEntityFactory();
                objectFac.Deserialize(item);
                this.objectTemplates.Add(name, objectFac);
            }
        }

    }
}