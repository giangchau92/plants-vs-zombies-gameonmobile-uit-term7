using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDesignTool.Models
{
    public struct MovementBehavior : IBehavior
    {
        public string Name;
        public Vector2 Velocity;
    }
}
