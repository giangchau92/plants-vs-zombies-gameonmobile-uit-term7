using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Behaviors.Bullet;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GrowSystem;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Serialization;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies
{
    public class ObjectEntityGesture : ObjectEntity, IGestureTarget<Tap>
    {
        public ObjectEntityGesture()
            : base()
        {
        }

        public bool ReceivedGesture(Tap gEvent)
        {
            B_SunLogicBehavior behavior = ((GetComponent(typeof(LogicComponent)) as LogicComponent).LogicBehavior as B_SunLogicBehavior);
            behavior.Collect();
            return true;
        }

        public bool IsHandleGesture(Tap gEvent)
        {
            Rectangle bound = (GetComponent(typeof(PhysicComponent)) as PhysicComponent).Frame;

            bool result = bound.Contains((int)gEvent.Touch.Positions.Current.X, (int)gEvent.Touch.Positions.Current.Y);

            return result;
        }

        public uint Priority
        {
            get { return 0; }
        }

        public bool IsGestureCompleted
        {
            get { return false; }
        }
    }

    public class ObjectEntityGestureFactory : IObjectEntityFactory
    {
        private const string tagName = "Name";
        private const string tagComponents = "Components";
        private const string tagComponent = "Component";

        ICollection<IComponent<MessageType>> components = new List<IComponent<MessageType>>();
        string name;
        public eObjectType ObjectType;

        IComponentFactory componentFactory = new ComponentFactory();

        public void Serialize(ISerializer serializer)
        {

        }

        public void Deserialize(IDeserializer deserializer)
        {
            // Save name
            this.name = deserializer.DeserializeString(tagName);
            string objectType = deserializer.DeserializeString("Type");
            if (objectType == "Zombie")
                ObjectType = eObjectType.ZOMBIE;
            else if (objectType == "Plant")
                ObjectType = eObjectType.PLANT;
            else if (objectType == "Bullet")
                ObjectType = eObjectType.BULLET;
            else
                ObjectType = eObjectType.SUN;

            // Deserializer Components
            IDeserializer componentsDeser = deserializer.SubDeserializer(tagComponents);

            // Deserializer Component
            var componentDesers = componentsDeser.DeserializeAll(tagComponent);
            foreach (var cDeser in componentDesers)
            {
                string type = cDeser.DeserializeString("Type");
                var component = componentFactory.CreateComponent(type);

                component.Deserialize(cDeser);

                this.components.Add(component);
            }

        }

        public ObjectEntity CreateEntity()
        {
            ObjectEntityGesture entity = new ObjectEntityGesture();
            entity.ObjectType = ObjectType;
            foreach (var component in this.components)
            {
                entity.AddComponent(component.Clone());
            }
            entity.OnComplete();

            return entity;
        }
    }
}
