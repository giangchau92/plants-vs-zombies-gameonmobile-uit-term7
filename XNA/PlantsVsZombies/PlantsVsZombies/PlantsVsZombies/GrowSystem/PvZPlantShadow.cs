using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.Audio;
using SCSEngine.Control;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Serialization;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;

namespace PlantsVsZombies.GrowSystem
{
    public delegate void PvZPlantShadowEventHandler(PvZPlantShadow shadow);

    public class PvZPlantShadow : BaseUIControl, IGestureTarget<FreeTap>
    {
        private const string GROWPLANT_SOUNDNAME = "Sounds/Plant";

        public PvZGrowButton CreatorButton { get; set; }

        public ISprite PlanShadowImage { get; set; }
        public string PlantName { get; set; }
        
        private SpritePlayer spritePlayer;
        private Sound growPlantSound;

        private IPvZGameGrow gameGrow;
        public event PvZPlantShadowEventHandler OnGrowNewPlant;

        public PvZPlantShadow(Game game, string plantName, IPvZGameGrow gg)
            : base(game)
        {
            this.spritePlayer = ((SCSServices)game.Services.GetService(typeof(SCSServices))).SpritePlayer;
            this.PlantName = plantName;
            this.gameGrow = gg;
            this.growPlantSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(GROWPLANT_SOUNDNAME);
        }

        public override void RegisterGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
            dispatcher.AddTarget<FreeTap>(this);
        }

        public override void LeaveGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
            dispatcher.RemoveTarget<FreeTap>(this);
        }

        public override void Draw(GameTime gameTime)
        {
            // draw adjusted plant
            CRectangleF cellRect = this.gameGrow.CellContains(this.Canvas.Bound);
            if (cellRect != null)
            {
                cellRect.Position.Y += cellRect.Height - this.PlanShadowImage.CurrentFrame.Height;
                cellRect.Width *= cellRect.Height / this.PlanShadowImage.CurrentFrame.Height;
                cellRect.Height = this.PlanShadowImage.CurrentFrame.Height;
                this.spritePlayer.Draw(this.PlanShadowImage, cellRect.Position, Color.Gray);
            }
            this.spritePlayer.Draw(this.PlanShadowImage, this.Canvas.Bound.Rectangle, Color.White);

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
                if (this.gameGrow.GrowPlant(this.PlantName, this.Canvas.Bound))
                {
                    SCSServices.Instance.AudioManager.PlaySound(this.growPlantSound, false, true);
                    this.CreatorButton.Cooldown();

                    if (this.OnGrowNewPlant != null)
                    {
                        this.OnGrowNewPlant(this);
                    }
                }
                // remove shadow
                this.IsUICompleted = true;
                return false;
            }

            return true;
        }

        public bool IsHandleGesture(FreeTap gEvent)
        {
            return this.Canvas.Bound.Contains(gEvent.Current);
        }

        public uint Priority
        {
            get { return 0; }
        }
    }

    public class PvZPlantShadowFactory : ISerializable
    {
        public string Name { get; set;}
        private string picture;
        private string plantName;

        private Game game;
        private IPvZGameGrow gameGrow;

        public PvZPlantShadowFactory(Game game, IPvZGameGrow gg)
        {
            this.game = game;
            this.gameGrow = gg;
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

        public PvZPlantShadow CreatePlantShadow()
        {
            PvZPlantShadow shadow = new PvZPlantShadow(this.game, this.plantName, this.gameGrow);
            shadow.PlanShadowImage = SCSServices.Instance.ResourceManager.GetResource<ISprite>(this.picture);

            return shadow;
        }
    }

    public class PvZPlantShadowFactoryBank : ISerializable
    {
        private IDictionary<string, PvZPlantShadowFactory> factories = new Dictionary<string, PvZPlantShadowFactory>();
        private IPvZGameGrow gameGrow;
        private Game game;

        public PvZPlantShadowFactoryBank(Game game, IPvZGameGrow gameGrow)
        {
            this.game = game;
            this.gameGrow = gameGrow;
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
                PvZPlantShadowFactory factory = new PvZPlantShadowFactory(this.game, this.gameGrow);
                factory.Deserialize(psDeser);

                this.factories.Add(factory.Name, factory);
            }
        }
    }
}