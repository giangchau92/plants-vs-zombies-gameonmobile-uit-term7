using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Components
{
    public class PhysicComponentFactory
    {
        public static PhysicComponent CreateComponent()
        {
            PhysicComponent component = new PhysicComponent();

            return component;
        }
    }
}
