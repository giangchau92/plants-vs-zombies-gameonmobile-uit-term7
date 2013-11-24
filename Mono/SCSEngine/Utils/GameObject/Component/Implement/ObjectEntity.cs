using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace SSCEngine.Utils.GameObject.Component.Implement
{
    public class ObjectEntity : IEntity, IComponent
    {
        private IDictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();
        public IDictionary<Type, IComponent> Components
        {
            get { return _components; }
        }

        public bool AddComponent(IComponent component)
        {
            if (component == null)
                return false;
            if (_components.ContainsKey(component.GetType()))
                throw new Exception("SCSEngine: Duplicate component!");
            component.Owner = this;
            _components.Add(component.GetType(), component);
            return true;
        }

        public void RemoveComponent(IComponent component)
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

        public virtual void OnMessage(IMessage<> message, GameTime gameTime)
        {
            foreach (KeyValuePair<Type, IComponent> component in this._components)
            {
                component.Value.OnMessage(message, gameTime);
            }
        }

        public IEntity Owner
        {
            get { return null; }
        }


        public IComponent GetComponent(Type typeComponent)
        {
            if (Components.ContainsKey(typeComponent))
                return Components[typeComponent];
            return null;
        }
    }
}