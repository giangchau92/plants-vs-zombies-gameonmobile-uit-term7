using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SSCEngine.Control;
using SSCEngine.GestureHandling;
using SSCEngine.GestureHandling.Implements.Events;
using SSCEngine.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GrowSystem
{
    public class PvZPlantShadow : BaseUIControl, IGestureTarget<FreeTap>
    {
        public ISprite PlanShadowImage { get; set; }
        public string PlantName { get; set; }
        
        private SpritePlayer spritePlayer;

        public PvZPlantShadow(Game game, string plantName)
            : base(game)
        {
            this.spritePlayer = ((SCSServices)game.Services.GetService(typeof(SCSServices))).SpritePlayer;
            this.PlantName = plantName;
        }

        public override void RegisterGestures(SSCEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
            dispatcher.AddTarget<FreeTap>(this);
        }

        public override void LeaveGestures(SSCEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
            this.IsGestureCompleted = true;
        }

        public override void Draw(GameTime gameTime)
        {
            // draw adjusted plant
            this.spritePlayer.Draw(this.PlanShadowImage, this.Canvas.Bound.Rectangle, 0f, this.Canvas.Bound.Size / 2f, Color.White, SpriteEffects.None, 0f);

            base.Draw(gameTime);
        }

        private Vector2 touchPos;
        public bool ReceivedGesture(FreeTap gEvent)
        {
            if (gEvent.Touch.SystemTouch.State == TouchLocationState.Pressed)
            {
                touchPos = gEvent.Touch.Positions.Current - this.Canvas.Bound.Position;
            }
            else
            {
                this.Canvas.Bound.Position  = gEvent.Touch.Positions.Current - touchPos;
            }

            if (gEvent.Touch.SystemTouch.State == TouchLocationState.Released)
            {
                // if (game cell avaiable)
                // add plant to game
                return false;
            }

            return true;
        }

        public bool IsHandleGesture(FreeTap gEvent)
        {
            return this.Canvas.Bound.Contains(gEvent.Current);
        }
    }

    public class PvZPlantShadowFactory : ISerializable
    {
        public string Name { get; set;}
        private string picture;
        private string plantName;

        private Game game;
        public PvZPlantShadowFactory(Game game)
        {
            this.game = game;
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            this.Name = deserializer.DeserializeString("Name");
            this.picture = deserializer.DeserializeString("Picture");
            this.plantName = deserializer.DeserializeString("Plant");
        }

        public void CreatePlantShadow()
        {
            PvZPlantShadow shadow = new PvZPlantShadow(this.game, this.plantName);
            shadow.PlanShadowImage = SCSServices.Instance.ResourceManager.GetResource<ISprite>(this.picture);
        }
    }

    public class PvZPlantShadowFactoryBank : ISerializable
    {
        private IDictionary<string, PvZPlantShadowFactory> factories;
        private Game game;

        public PvZPlantShadowFactoryBank(Game game)
        {
            this.game = game;
        }

        public PvZPlantShadowFactory GetPlantShadowFactory(string name)
        {
            return this.factories[name];
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            var psDesers = deserializer.DeserializeAll("Shadow");
            foreach (var psDeser in psDesers)
            {
                PvZPlantShadowFactory factory = new PvZPlantShadowFactory(this.game);
                factory.Deserialize(psDeser);

                this.factories.Add(factory.Name, factory);
            }
        }
    }
}