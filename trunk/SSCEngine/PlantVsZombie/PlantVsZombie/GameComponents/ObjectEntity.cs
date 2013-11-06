using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SSCEngine.Utils.GameObject.Component;


namespace PlantVsZombie.GameComponents
{
    public class ObjectEntity : IEntity<MessageType>, IComponent<MessageType>
    {
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

        public virtual void OnMessage(IMessage<MessageType> message, GameTime gameTime)
        {
            foreach (KeyValuePair<Type, IComponent<MessageType>> component in this._components)
            {
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
    }
}