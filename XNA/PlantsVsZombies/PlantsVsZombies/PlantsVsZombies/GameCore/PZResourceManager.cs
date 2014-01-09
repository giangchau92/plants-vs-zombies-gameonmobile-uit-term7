using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ResourceManagement.Implement;
using SCSEngine.Sprite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.ResourceManagement;

namespace PlantsVsZombies.GameCore
{
    public class PZResourceManager : SCSResourceManager
    {
        public PZResourceManager(ContentManager content)
            : base(content)
        {
        }
    }
}