using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SCSEngine.Utils.GameObject.Component;
using SCSEngine.Serialization;
using PlantVsZombies.GameComponents.Components;


namespace PlantVsZombies.GameComponents
{
    public enum eObjectType { PLANT, ZOMBIE}
    public class ObjectEntity : IEntity<MessageType>, IComponent<MessageType>
    {
        public eObjectType ObjectType { get; set; }
        private IDictionary<Type, IComponent<MessageType>> _components = new Dictionary<Type, IComponent<MessageType>>();
        public IDictionary<Type, IComponent<MessageType>> Components
        {
            get { return _components; }
        }

        public bool AddComponent(IComponent<MessageType> component)
        {
            if (component == null)
                return false;
            if (_components.ContainsKey(component.GetType()))
                throw new Exception("SCSEngine: Duplicate component!");
            component.Owner = this;
            _components.Add(component.GetType(), component);
            return true;
        }

        public void RemoveComponent(IComponent<MessageType> component)
        {
            if (component == null)
                return;
            if (_components.ContainsKey(component.GetType()))
                RemoveComponent(component.GetType());
        }

        public void RemoveComponent(Type typeComponent)
        {
            if (typeComponent == null)
                return;
            if (_components.ContainsKey(typeComponent))
                _components.Remove(typeComponent);
        }

        public ObjectEntity()
        {
        }

        public ObjectEntity(ObjectEntity other)
        {
            ObjectType = other.ObjectType;
            // Clone component
            foreach (var item in other._components)
            {
                
            }
        }

        public virtual void OnMessage(IMessage<MessageType> message, GameTime gameTime)
        {
            foreach (var component in this._components)
            {
                if (message.Handled)
                    break;
                component.Value.OnMessage(message, gameTime);
            }
        }


        public IComponent<MessageType> GetComponent(Type typeComponent)
        {
            if (Components.ContainsKey(typeComponent))
                return Components[typeComponent];
            return null;
        }

        public IEntity<MessageType> Owner
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public ulong ObjectId
        {
            get;
            set;
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            throw new NotImplementedException();
        }


        public IComponent<MessageType> Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectEntityFactory : ISerializable
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
            // 
            IDeserializer componentsDeser = deserializer.SubDeserializer(tagComponents);

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
            ObjectEntity entity = new ObjectEntity();
            entity.ObjectType = ObjectType;
            foreach (var component in this.components)
            {
                entity.AddComponent(component.Clone());
            }
            return entity;
        }
    }

    public interface IComponentFactory
    {
        IComponent<MessageType> CreateComponent(string type);
    }


    public class ComponentFactory : IComponentFactory
    {
        public IComponent<MessageType> CreateComponent(string type)
        {
            switch (type)
            {
                case "xml_move":
                    return MoveComponentFactory.CreateComponent();
                case "xml_render":
                    return RenderComponentFactory.CreateComponent();
                case "xml_physic":
                    return PhysicComponentFactory.CreateComponent();
                case "xml_logic":
                    return LogicComponentFactory.CreateComponent();
                default:
                    break;
            }
            return null;
        }
    }
}