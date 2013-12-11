using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ResourceManagement.Implement;
using SCSEngine.Sprite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlantsVsZombies.GameCore
{
    public class PZResourceManager : BaseResourceManager
    {
        public PZResourceManager(ContentManager content)
            : base()
        {
            this.AddResourceLoader(new GameContentResourceLoader<Texture2D>(content));
            this.AddResourceLoader(new SpriteResourceLoader(this, SpriteFramesBank.Instance));
        }
    }
}