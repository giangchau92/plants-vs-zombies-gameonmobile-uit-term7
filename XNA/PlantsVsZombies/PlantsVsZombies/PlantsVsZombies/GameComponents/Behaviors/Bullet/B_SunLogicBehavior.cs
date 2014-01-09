using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameComponents.Effect;
using PlantsVsZombies.GameComponents.Effect.Implements;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GrowSystem;
using SCSEngine.Audio;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Services;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Behaviors.Bullet
{
    public enum eSunState
    {
        JUMP, STAND, PLYING, COLLECT, COLLECT_COMPLETED
    }
    public class B_SunLogicBehavior : BaseLogicBehavior
    {
        private static TimeSpan _lastTimeSound;
        private static TimeSpan _timeDelaySound = TimeSpan.FromSeconds(1);
        private bool justCollect = false;
        private eSunState SunState { get; set; }

        private Sound _soundCollectSun;

        public B_SunLogicBehavior()
            : base()
        {
            SunState = eSunState.STAND;
            _soundCollectSun = SCSServices.Instance.ResourceManager.GetResource<Sound>("Sounds/SunClicked");
        }

        public void setState(eSunState state)
        {
            SunState = state;
            if (SunState == eSunState.JUMP)
            {
                IEffect effect = new SunRiseEffect();
                AddEffect(effect);
            }
            else if (SunState == eSunState.PLYING)
            {
                IEffect effect = new SunFlyDownEffect();
                AddEffect(effect);
            }
            
        }

        public void Collect()
        {
            SunState = eSunState.COLLECT;
            MoveComponent moveCom = this.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
            ////Debug.WriteLine("Add SunCollectEffect");
            AddEffect(new SunCollectEffect(moveCom.Position));
            
            justCollect = true;
        }

        public override void Update(IMessage<MessageType> msg, GameTime gameTime)
        {
            if (listEffect.Count == 0 && SunState == eSunState.PLYING)
                SunState = eSunState.STAND;

            if (justCollect)
            {
                if (gameTime.TotalGameTime - _lastTimeSound > _timeDelaySound)
                {
                    SCSServices.Instance.AudioManager.PlaySound(_soundCollectSun, false, true);
                    _lastTimeSound = gameTime.TotalGameTime;
                }
            }
            justCollect = false;

            if (listEffect.Count == 0 && SunState == eSunState.COLLECT)
            {
                // Self remove sun
                (SCSServices.Instance.Game.Services.GetService(typeof(PvZSunSystem)) as PvZSunSystem).RemoveSun(this.Owner.Owner as ObjectEntityGesture);
                //PZObjectManager.Instance.RemoveObject(this.Owner.Owner.ObjectId);
                //PvZHardCurrency._gestureDispatcher.RemoveTarget<Tap>(Owner.Owner as IGestureTarget<Tap>);
            }

      

            // Die
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            if (logicCOm.Health < 0)
            {
                PZObjectManager.Instance.RemoveObject(this.Owner.Owner.ObjectId);
            }
            base.Update(msg, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("PL_NormalLogicBehavior: message is not CollisionDetectedMsg");

            base.OnCollison(msg, gameTime);
        }

        public override IBehavior<MessageType> Clone()
        {
            var clone = new B_SunLogicBehavior();
            return clone;
        }
    }
}
