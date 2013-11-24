using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlantsVsZombies.GameComponents.Components
{
    public class MoveComponentFactory
    {
        public static MoveComponent CreateComponent()
        {
            MoveComponent component = new MoveComponent();
            component.Position = Vector2.Zero;
            component.Velocity = Vector2.Zero;
            component.Acceleration = Vector2.Zero;

            return component;
        }
    }
}
