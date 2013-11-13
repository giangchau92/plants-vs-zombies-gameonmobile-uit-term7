using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using PlantVsZombie.GameComponents.Components;
using PlantVsZombie.GameComponents;
using PlantVsZombie.GameComponents.Behaviors.Zombie;
using PlantVsZombie.GameComponents.Behaviors.Plant;

namespace PlantVsZombie.GameObjects
{
    public class NormalZombie : ObjectEntity
    {
        public NormalZombie()
            :base()
        {
            // Move component
            MoveComponent moveCOm = MoveComponentFactory.CreateComponent();
            moveCOm.AddBehavior(eMoveBehaviorType.NORMAL_RUNNING, new Z_NormalRunBehavior());
            moveCOm.AddBehavior(eMoveBehaviorType.STANDING, new Z_NormalStandBehavior());
            this.AddComponent(moveCOm);
            // Render component
            RenderComponent renCOm = RenderComponentFactory.CreateComponent();
            renCOm.AddBehavior(eMoveRenderBehaviorType.ZO_NORMAL_RUNNING, new Z_NormalRunRenderBahavior());
            renCOm.AddBehavior(eMoveRenderBehaviorType.ZO_NORMAL_EATING, new Z_NormalEatingRenderBehavior());
            renCOm.AddBehavior(eMoveRenderBehaviorType.STANDING, new Z_NormalStandRenderBehavior());
            this.AddComponent(renCOm);
            // Physic component
            PhysicComponent phyCOm = PhysicComponentFactory.CreateComponent();
            phyCOm.Bound = new Microsoft.Xna.Framework.Rectangle(0, 0, 50, 100);
            this.AddComponent(phyCOm);
            // Logic component
            LogicComponent logicCOm = LogicComponentFactory.CreateComponent();
            logicCOm.LogicBehavior = new Z_NormalLogicBehavior();
            this.AddComponent(logicCOm);

            // Init data
            moveCOm.Position = new Microsoft.Xna.Framework.Vector2(600, 100);
        }
    }
}
