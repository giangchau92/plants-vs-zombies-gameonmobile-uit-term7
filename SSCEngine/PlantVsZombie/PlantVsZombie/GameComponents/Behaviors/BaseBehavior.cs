using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;

namespace PlantVsZombie.GameComponents.Behaviors
{
    public class BaseBehavior : IBehavior<MessageType>
    {
        public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        private IComponent<MessageType> _owner = null;
        public IComponent<MessageType> Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }
    }
}
