using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Effect;
using SCSEngine.Serialization;

namespace PlantsVsZombies.GameComponents.Behaviors
{
    public class BaseLogicBehavior : BaseBehavior, ISerializable
    {
        IDictionary<Type, IEffect> listEffect = new Dictionary<Type, IEffect>();

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {
            IDictionary<Type, IEffect> backList = new Dictionary<Type, IEffect>(listEffect);
            foreach (var item in backList)
            {
                item.Value.Update(gameTime);
            }

 	        base.Update(message, gameTime);
        }

        public virtual void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
        }

        public void AddEffect(IEffect effect)
        {
            if (effect != null)
            {
                if (listEffect.ContainsKey(effect.GetType())) // If effect exist
                {
                    listEffect.Remove(effect.GetType());
                }
                effect.Owner = this;
                listEffect.Add(effect.GetType(), effect);
            }
        }

        public void RemoveEffect(IEffect effect)
        {
            if (effect != null)
                listEffect.Remove(effect.GetType());
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public virtual void Deserialize(IDeserializer deserializer)
        {
        }
    }
}
