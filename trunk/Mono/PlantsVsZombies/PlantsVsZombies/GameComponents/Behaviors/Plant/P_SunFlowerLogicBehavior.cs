using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameCore;
using SCSEngine.Serialization;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameComponents.Behaviors.Plant
{
    public class P_SunFlowerLogicBehavior : BaseLogicBehavior
    {
        eNormalPlantState PlantState { get; set; }

        private TimeSpan _currentTime = TimeSpan.Zero;
        private double _timeGiveSun;
        public TimeSpan TimeGiveSun
        {
            get
            {
                return TimeSpan.FromSeconds(_timeGiveSun);
            }
            set
            {
                _timeGiveSun = value.TotalSeconds;
            }
        }

        public P_SunFlowerLogicBehavior()
            :base()
        {
        }


        public override void Update(IMessage<MessageType> msg, GameTime gameTime)
        {
            PlantState = eNormalPlantState.STANDING;


            if (_currentTime > TimeGiveSun)
            {
                PhysicComponent phyCom = this.Owner.Owner.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
                Point pos = phyCom.Frame.Center;
                PZObjectManager.Instance.AddObject(PZObjectFactory.Instance.createSun(new Vector2(pos.X, pos.Y)));
                _currentTime = TimeSpan.Zero;
            }
            else
                _currentTime += gameTime.ElapsedGameTime;
            
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
            var clone = new P_SunFlowerLogicBehavior();
            clone.TimeGiveSun = this.TimeGiveSun;
            return clone;
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            // CODE HERE
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            logicCOm.Health = deserializer.DeserializeInteger("Health");
            _timeGiveSun = deserializer.DeserializeDouble("TimeGiveSun");
        }
    }
}
