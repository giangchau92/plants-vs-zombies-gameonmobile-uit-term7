using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using PlantVsZombie.GameComponents.Components;
using PlantVsZombie.GameComponents;

namespace PlantVsZombie.GameObjects
{
    public class NormalZombie : ObjectEntity
    {
        public NormalZombie()
            :base()
        {
            this.AddComponent(MoveComponentFactory.CreateComponent());
            this.AddComponent(PhysicComponentFactory.CreateComponent());
            this.AddComponent(RenderComponentFactory.CreateComponent());
        }
    }
}
