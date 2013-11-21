using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.Behaviors.Bullet;
using PlantVsZombie.GameComponents.Behaviors.Implements;
using PlantVsZombie.GameComponents.Components;
using SCSEngine.ResourceManagement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GameObjects.Implements
{
    public class IceBullet : BaseBullet
    {
        public IceBullet()
            : base()
        {
            IResourceManager resourceManager = SCSServices.Instance.ResourceManager;
            // Move component
            MoveComponent moveCOm = MoveComponentFactory.CreateComponent();
            MoveBehavior moveBehavior = new MoveBehavior();
            moveBehavior.Velocity = new Vector2(200, 0);
            moveCOm.AddBehavior(eMoveBehaviorType.NORMAL_FLYING, moveBehavior);
            this.AddComponent(moveCOm);

            // Physic component
            PhysicComponent phyCOm = PhysicComponentFactory.CreateComponent();
            phyCOm.Bound = new Microsoft.Xna.Framework.Rectangle(0, 0, 10, 10);
            this.AddComponent(phyCOm);
            // Logic component
            LogicComponent logicCOm = LogicComponentFactory.CreateComponent();
            logicCOm.LogicBehavior = new B_IceBulletLogicBehavior();
            this.AddComponent(logicCOm);

            // Render component
            RenderComponent renCOm = RenderComponentFactory.CreateComponent();
            RenderBehavior renBehavior = new RenderBehavior();
            renBehavior.Sprite = resourceManager.GetResource<ISprite>("Bullets/B_IcePea");
            renBehavior.SpriteBound = new Rectangle(0, 0, 29, 22);
            renCOm.AddBehavior(eMoveRenderBehaviorType.B_FLYING, renBehavior);
            this.AddComponent(renCOm);
        }

        public void SetPosition(Vector2 pos)
        {
            MoveComponent moveCOm = this.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCOm.Position = pos;
            PhysicComponent physCom = this.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
            physCom.UpdateFrame();
        }
    }
}
