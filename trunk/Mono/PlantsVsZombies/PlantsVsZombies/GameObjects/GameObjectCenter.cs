using PlantVsZombie.GameComponents;
using PlantVsZombie.GameComponents.Behaviors.Implements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.Components;
using SCSEngine.ResourceManagement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using PlantVsZombie.GameComponents.Behaviors.Zombie;
using SCSEngine.Serialization.XmlSerialization;

namespace PlantVsZombie.GameObjects
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

        private const string zombie_config = "PlantVsZombie.Xml.Zombies.xml";

        private IDictionary<string, ObjectEntityFactory> objectTemplates = new Dictionary<string, ObjectEntityFactory>();

        public void InitEnity()
        {
            // Init zombie
            loadZombie();
        }

        public ObjectEntity CreateObject(string te)
        {
            if (objectTemplates.ContainsKey(te))
            {
                return objectTemplates[te].CreateEntity();
            }
            return null;
        }

        private XDocument getXml(string config_url)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlantVsZombie.Xml.Zombies.xml");
            XDocument mXDocument = new XDocument();
            mXDocument = XDocument.Load(stream);
            return mXDocument;
        }

        private void loadZombie()
        {
            IResourceManager resourceManager = SCSServices.Instance.ResourceManager;

            XDocument docs = getXml(zombie_config);
            foreach (XElement item in docs.Root.Nodes())
            {
                string name = item.Element("Name").Value;
                ObjectEntityFactory objectFac = new ObjectEntityFactory();
                //XmlDeserializer deser = new XmlDeserializer(item);
                objectFac.CreateEntity();

                this.objectTemplates.Add(name, objectFac);
            }
        }

    }
}
