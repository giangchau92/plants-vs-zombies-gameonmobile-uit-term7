using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDesignTool.Models
{
    class MovementComponent : IComponent
    {
        public IDictionary<string, MovementBehavior> UsedBehaviors { get; private set; }
        public List<string> ExistBehaviors { get; private set; }

        public MovementComponent()
        {
            UsedBehaviors = new Dictionary<string, MovementBehavior>();
            ExistBehaviors = new List<string>();
        }

        public void UseBehavior(string behName, Vector2 behVel)
        {
        }
    }
}