using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents;
using PlantVsZombie.GameComponents.Behaviors.Bullet;
using PlantVsZombie.GameComponents.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GameObjects
{
    public class NormalBullet : ObjectEntity
    {
        public NormalBullet()
            : base()
        {
            // Move component
            MoveComponent moveCOm = MoveComponentFactory.CreateComponent();
            moveCOm.AddBehavior(eMoveBehaviorType.NORMAL_FLYING, new B_NormalFlyBehavior());
            this.AddComponent(moveCOm);
            // Render component
            RenderComponent renCOm = RenderComponentFactory.CreateComponent();
            renCOm.AddBehavior(eMoveRenderBehaviorType.B_FLYING, new B_NormalFlyRenderBehavior());
            this.AddComponent(renCOm);
            // Physic component
            PhysicComponent phyCOm = PhysicComponentFactory.CreateComponent();
            phyCOm.Bound = new Microsoft.Xna.Framework.Rectangle(0, 0, 10, 10);
            this.AddComponent(phyCOm);
            // Logic component
            LogicComponent logicCOm = LogicComponentFactory.CreateComponent();
            logicCOm.LogicBehavior = new B_NormalLogicBehavior();
            this.AddComponent(logicCOm);

            // Init data
            
        }

        public void SetPosition(Vector2 pos)
        {
            MoveComponent moveCOm = this.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCOm.Position = pos;
        }
    }
}
