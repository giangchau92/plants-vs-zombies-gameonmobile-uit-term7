using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Behaviors.Zombie;
using PlantsVsZombies.GameComponents.Behaviors.Plant;
using PlantsVsZombies.GameComponents.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SCSEngine.ResourceManagement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using PlantsVsZombies.GameComponents.Behaviors.Implements;

namespace PlantsVsZombies.GameObjects.Implements
{
    public class NormalPlant : BasePlant
    {
        public NormalPlant()
            : base()
        {
            IResourceManager resourceManager = SCSServices.Instance.ResourceManager;
            // Move component
            MoveComponent moveCOm = MoveComponentFactory.CreateComponent();
            // Stand
            MoveBehavior moveBehavior = new MoveBehavior();
            moveBehavior.Velocity = Vector2.Zero;
            moveCOm.AddBehavior(eMoveBehaviorType.STANDING, moveBehavior);
            this.AddComponent(moveCOm);
            // Render component
            RenderComponent renCOm = RenderComponentFactory.CreateComponent();
            // Stand
            RenderBehavior renBehavior = new RenderBehavior();
            renBehavior.Sprite = resourceManager.GetResource<ISprite>("Plants/DoublePea/DoublePea");
            renBehavior.SpriteBound = new Rectangle(0, 0, 100, 55);
            renCOm.AddBehavior(eMoveRenderBehaviorType.STANDING, renBehavior);
            this.AddComponent(renCOm);
            // Physic component
            PhysicComponent phyCOm = PhysicComponentFactory.CreateComponent();

            this.AddComponent(phyCOm);
            // Logic component
            LogicComponent logicCOm = LogicComponentFactory.CreateComponent();
            logicCOm.LogicBehavior = new P_NormalLogicBehavior();
            this.AddComponent(logicCOm);

            renCOm.ChangeBehavior(eMoveRenderBehaviorType.STANDING);

        }
    }
}
