using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ResourceManagement.Implement;
using SCSEngine.Sprite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.Resources;

namespace PlantVsZombie.GameCore
{
    public class PZResourceManager : BaseResourceManager
    {
        public PZResourceManager(GraphicsDevice graphicDevice)
            : base()
        {
            this.AddResourceLoader(new AssetTexture2DLoader(graphicDevice, @"Assets\"));
            this.AddResourceLoader(new SpriteResourceLoader(this, SpriteFramesBank.Instance));
        }
    }
}