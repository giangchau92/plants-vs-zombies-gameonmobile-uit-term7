using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.Utils.GameObject.Component
{
    public interface IEntity<T>
    {
        IDictionary<Type, IComponent<T>> Components { get; }
        bool AddComponent(IComponent<T> component);
        void RemoveComponent(IComponent<T> component);
        void RemoveComponent(Type typeComponent);
        IComponent<T> GetComponent(Type typeComponent);
    }
}
