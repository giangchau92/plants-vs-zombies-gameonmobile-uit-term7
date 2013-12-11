using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.GameMessages
{
    public class CollisionDetectedMsg : GameMessage
    {
        public CollisionDetectedMsg(MessageType msgType, object sender)
            : base(msgType, sender)
        {
            TargetCollision = null;
        }

        public object TargetCollision { get; set; }
    }
}
