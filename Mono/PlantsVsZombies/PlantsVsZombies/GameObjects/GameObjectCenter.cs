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

        private IDictionary<string, ObjectEntity> objectTemplates = new Dictionary<string, ObjectEntity>();

        public void InitEnity()
        {
            // Init zombie
            loadZombie();
        }

        public ObjectEntity CreateObject(string te)
        {
            if (objectTemplates.ContainsKey(te))
            {
                if (objectTemplates[te].ObjectType == eObjectType.ZOMBIE)
                {
                    BaseZombie clone = new BaseZombie(objectTemplates[te]);
                }
            }
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
                BaseZombie zombie = new BaseZombie();

                string name = item.Attribute("name").Value;
                XElement components = item.Element("Components");
                foreach (var component in components.Elements("Component"))
                {
                    string type = component.Attribute("type").Value;
                    var behaviors = component.Elements("Behavior");

                    switch (type)
                    {
                        case "xml_move":
                            // MOVE COMPONNET
                            MoveComponent moveComponent = MoveComponentFactory.CreateComponent();
                            foreach (XElement behavior in behaviors)
                            {
                                string typeBe = behavior.Attribute("type").Value;
                                MoveBehavior moveBeha = MoveBehavior.CreateBehavior();
                                int velX = Int32.Parse(behavior.Attribute("velocityX").Value);
                                int velY = Int32.Parse(behavior.Attribute("velocityY").Value);
                                moveBeha.Velocity = new Vector2(velX, velY);
                                if (typeBe == "xml_move_stand")
                                    moveComponent.AddBehavior(eMoveBehaviorType.STANDING, moveBeha);
                                else if (typeBe == "xml_move_run")
                                    moveComponent.AddBehavior(eMoveBehaviorType.RUNNING, moveBeha);
                            }
                            zombie.AddComponent(moveComponent);
                            break;
                        case "xml_render":
                            // RENDER COMPONENT
                            RenderComponent renderComponent = RenderComponentFactory.CreateComponent();
                            foreach (XElement behavior in behaviors)
                            {
                                string typeBe = behavior.Attribute("type").Value;
                                RenderBehavior renderBeha = RenderBehavior.CreateBehavior();
                                string resource = behavior.Attribute("resourceName").Value;
                                renderBeha.Sprite = resourceManager.GetResource<ISprite>(resource);
                                if (typeBe == "xml_render_eat")
                                    renderComponent.AddBehavior(eMoveRenderBehaviorType.ZO_NORMAL_EATING, renderBeha);
                                else if (typeBe == "xml_render_run")
                                    renderComponent.AddBehavior(eMoveRenderBehaviorType.ZO_NORMAL_RUNNING, renderBeha);
                            }
                            zombie.AddComponent(renderComponent);
                            break;
                        case "xml_physic":
                            // PHYSIC COMPONENT
                            PhysicComponent physicComponent = PhysicComponentFactory.CreateComponent();
                            zombie.AddComponent(physicComponent);
                            break;
                        case "xml_logic":
                            // LOGIC COMPONENT
                            LogicComponent logicComponent = LogicComponentFactory.CreateComponent();
                            string typeCo = component.Attribute("behavior").Value;
                            if (typeCo == "xml_NormalZombie")
                                logicComponent.LogicBehavior = new Z_NormalLogicBehavior();
                            zombie.AddComponent(logicComponent);
                            break;
                        default:
                            break;
                    }
                    
                }

                zombie.ObjectType = eObjectType.ZOMBIE;
                this.objectTemplates.Add(name, zombie);
            }
        }

    }
}
