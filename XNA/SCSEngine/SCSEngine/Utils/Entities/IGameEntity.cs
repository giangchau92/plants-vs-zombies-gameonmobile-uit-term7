using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.Utils.Entities
{
    public interface IGameEntity
    {
        bool AddComponent(IGameComponent component);

        bool RemoveComponent(IGameComponent component);
        bool RemoveComponent<GameComponent>() where GameComponent : IGameComponent;

        GameComponent GetComponent<GameComponent>() where GameComponent : IGameComponent;

        IEnumerable<IGameComponent> Components { get; }
    }
}
