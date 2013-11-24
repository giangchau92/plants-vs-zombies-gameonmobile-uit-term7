using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Components
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
