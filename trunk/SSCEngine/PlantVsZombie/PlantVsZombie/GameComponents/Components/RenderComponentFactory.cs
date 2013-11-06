using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GameComponents.Components
{
    public class RenderComponentFactory
    {
        public static RenderComponent CreateComponent()
        {
            RenderComponent component = new RenderComponent();

            return component;
        }
    }
}
