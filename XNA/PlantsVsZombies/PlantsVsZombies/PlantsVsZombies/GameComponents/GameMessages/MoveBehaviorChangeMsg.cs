using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Components;

namespace PlantsVsZombies.GameComponents.GameMessages
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
