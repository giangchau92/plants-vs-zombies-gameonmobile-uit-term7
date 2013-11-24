using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;

namespace PlantVsZombie.GameComponents.Behaviors
{
    public class BaseBehavior : IBehavior<MessageType>
    {
        public virtual void Update(IMessage<MessageType> message, GameTime gameTime)
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

        public virtual void OnLoad()
        {
            
        }


        public virtual void UnLoad()
        {

        }
    }
}
