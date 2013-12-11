using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Behaviors.Implements;
using PlantsVsZombies.GameComponents.Behaviors.Plant;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.ResourceManagement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameObjects.Implements
{
    public class IcePlant : BasePlant
    {
        public IcePlant()
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
            renBehavior.Sprite = resourceManager.GetResource<ISprite>("Plants/IcePea/IcePea");
            renBehavior.SpriteBound = new Rectangle(0, 0, 118, 63);
            renCOm.AddBehavior(eMoveRenderBehaviorType.STANDING, renBehavior);
            this.AddComponent(renCOm);
            // Physic component
            PhysicComponent phyCOm = PhysicComponentFactory.CreateComponent();

            this.AddComponent(phyCOm);
            // Logic component
            LogicComponent logicCOm = LogicComponentFactory.CreateComponent();
            logicCOm.LogicBehavior = new P_IcePlantLogicBehavior();
            this.AddComponent(logicCOm);

            renCOm.ChangeBehavior(eMoveRenderBehaviorType.STANDING);
        }
    }
}
