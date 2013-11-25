using Microsoft.Xna.Framework.Graphics;
using SCSEngine.ResourceManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.Resources
{
    public class AssetTexture2DLoader : IResourceLoader
    {
        string folderPath;
        GraphicsDevice graphicDevice;

        public AssetTexture2DLoader(GraphicsDevice gd, string assetFolder)
        {
            this.folderPath = assetFolder;
            this.graphicDevice = gd;
        }

        public object Load(string resourceName)
        {
            return null;
        }

        public Type ResourceType
        {
            get { return typeof(Texture2D); }
        }

        public bool IsResourceResuable
        {
            get { return true; }
        }
    }
}
