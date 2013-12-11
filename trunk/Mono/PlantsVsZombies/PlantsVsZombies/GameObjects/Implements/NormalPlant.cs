using PlantVsZombies.GameComponents;
using PlantVsZombies.GameComponents.Behaviors.Zombie;
using PlantVsZombies.GameComponents.Behaviors.Plant;
using PlantVsZombies.GameComponents.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SCSEngine.ResourceManagement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using PlantVsZombies.GameComponents.Behaviors.Implements;

namespace PlantVsZombies.GameObjects.Implements
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
