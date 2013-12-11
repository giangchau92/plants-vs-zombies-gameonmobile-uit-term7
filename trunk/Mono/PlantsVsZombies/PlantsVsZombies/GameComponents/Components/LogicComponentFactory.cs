using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Components
{
    public class LogicComponentFactory
    {
        public static LogicComponent CreateComponent()
        {
            LogicComponent component = new LogicComponent();

            return component;
        }
    }
}
