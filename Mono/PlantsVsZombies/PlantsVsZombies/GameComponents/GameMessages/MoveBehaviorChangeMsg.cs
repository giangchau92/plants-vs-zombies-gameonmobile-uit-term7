using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantVsZombie.GameComponents;
using PlantVsZombie.GameComponents.Components;

namespace PlantVsZombie.GameComponents.GameMessages
{
    public class MoveBehaviorChangeMsg : GameMessage
    {
        public MoveBehaviorChangeMsg(MessageType msgType, object sender)
            : base(msgType, sender)
        {
        }

        public eMoveBehaviorType MoveBehaviorType { get; set; }
    }
}
