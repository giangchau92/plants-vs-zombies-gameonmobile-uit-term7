using PlantVsZombie.GameComponents;
using PlantVsZombie.GameComponents.Behaviors.Zombie;
using PlantVsZombie.GameComponents.Behaviors.Plant;
using PlantVsZombie.GameComponents.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GameObjects
{
    public class NormalPlant : ObjectEntity
    {
        public NormalPlant()
            : base()
        {
            // Move component
            MoveComponent moveCOm = MoveComponentFactory.CreateComponent();
            moveCOm.AddBehavior(eMoveBehaviorType.STANDING, new Z_NormalStandBehavior());
            this.AddComponent(moveCOm);
            // Render component
            RenderComponent renCOm = RenderComponentFactory.CreateComponent();
            renCOm.AddBehavior(eMoveRenderBehaviorType.PL_SHOOTING, new PL_NormalShootRenderBehavior());
            renCOm.AddBehavior(eMoveRenderBehaviorType.STANDING, new PL_NormalStandRenderBehavior());
            this.AddComponent(renCOm);
            // Physic component
            PhysicComponent phyCOm = PhysicComponentFactory.CreateComponent();
            phyCOm.Bound = new Microsoft.Xna.Framework.Rectangle(0, 0, 50, 100);
            this.AddComponent(phyCOm);
            // Logic component
            LogicComponent logicCOm = LogicComponentFactory.CreateComponent();
            logicCOm.LogicBehavior = new PL_NormalLogicBehavior();
            this.AddComponent(logicCOm);

            // Init data
            moveCOm.Position = new Microsoft.Xna.Framework.Vector2(50, 100);
        }
    }
}
