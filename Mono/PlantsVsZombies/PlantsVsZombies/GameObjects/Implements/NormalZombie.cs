using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.Utils.GameObject.Component;
using PlantVsZombies.GameComponents.Components;
using PlantVsZombies.GameComponents;
using PlantVsZombies.GameComponents.Behaviors.Zombie;
using PlantVsZombies.GameComponents.Behaviors.Plant;
using Microsoft.Xna.Framework;
using SCSEngine.Services;
using SCSEngine.ResourceManagement;
using SCSEngine.Sprite;
using PlantVsZombies.GameComponents.Behaviors.Implements;

namespace PlantVsZombies.GameObjects.Implements
{
    public class NormalZombie : BaseZombie
    {
        public NormalZombie()
            :base()
        {
            IResourceManager resourceManager = SCSServices.Instance.ResourceManager;
            // Move component
            MoveComponent moveCOm = MoveComponentFactory.CreateComponent();
            // Stand
            MoveBehavior moveBehavior = new MoveBehavior();
            moveBehavior.Velocity = Vector2.Zero;
            moveCOm.AddBehavior(eMoveBehaviorType.STANDING, moveBehavior);
            // Run
            moveBehavior = new MoveBehavior();
            moveBehavior.Velocity = new Vector2(-100, 0);
            moveCOm.AddBehavior(eMoveBehaviorType.RUNNING, moveBehavior);
            this.AddComponent(moveCOm);
            
            // Physic component
            PhysicComponent phyCOm = PhysicComponentFactory.CreateComponent();
            this.AddComponent(phyCOm);
            // Logic component
            LogicComponent logicCOm = LogicComponentFactory.CreateComponent();
            logicCOm.LogicBehavior = new Z_NormalLogicBehavior();
            this.AddComponent(logicCOm);
            // Render component
            RenderComponent renCOm = RenderComponentFactory.CreateComponent();
            // Run
            RenderBehavior renBehavior = new RenderBehavior();
            renBehavior.Sprite = resourceManager.GetResource<ISprite>("Zombies/Nameless/Walk");
            renBehavior.SpriteBound = new Rectangle(0, 0, 73, 100);
            renCOm.AddBehavior(eMoveRenderBehaviorType.ZO_NORMAL_RUNNING, renBehavior);
            // Eat
            renBehavior = new RenderBehavior();
            renBehavior.Sprite = resourceManager.GetResource<ISprite>("Zombies/Nameless/Attack");
            renBehavior.SpriteBound = new Rectangle(0, 0, 89, 101);
            renCOm.AddBehavior(eMoveRenderBehaviorType.ZO_NORMAL_EATING, renBehavior);
            this.AddComponent(renCOm);
        }
    }
}
