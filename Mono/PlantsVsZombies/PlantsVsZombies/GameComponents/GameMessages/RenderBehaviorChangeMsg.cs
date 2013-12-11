using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantsVsZombies.GameComponents.Components;

namespace PlantsVsZombies.GameComponents.GameMessages
{
    class RenderBehaviorChangeMsg : GameMessage
    {
        public eMoveRenderBehaviorType RenderBehaviorType { get; set; }

        public RenderBehaviorChangeMsg(MessageType msgType, object sender)
            : base(msgType, sender)
        {
        }

        
    }
}
