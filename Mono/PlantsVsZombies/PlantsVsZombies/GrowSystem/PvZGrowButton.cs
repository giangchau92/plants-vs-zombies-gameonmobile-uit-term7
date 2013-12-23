using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
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
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GrowSystem
{
    public delegate void PvZGrowButtonEventHandler(PvZGrowButton button, FreeTap leaveGesture);
    public delegate void CooldownButtonEventHandler(PvZGrowButton button);

    public interface IPvZGrowButtonBrush
    {
        void DrawDisabled(PvZGrowButton button, GameTime gameTime);
        void DrawNormal(PvZGrowButton button, GameTime gameTime);
        void DrawHighlight(PvZGrowButton button, GameTime gameTime);
        void DrawCooldown(PvZGrowButton button, GameTime gameTime, float cooldownPercent);
    }

    public class PvZGrowButton : BaseUIControl, IGestureTarget<FreeTap>
    {
        public PvZPlantShadowFactory ShadowFactory { get; internal set; }

        #region Draw
        public IPvZGrowButtonBrush Brush { get; set; }

        public bool IsOverlay = false;
        #endregion

        #region ButtonDrawState
        public enum PvZGrowButtonState { Normal, Highlight};
        public PvZGrowButtonState State { get; private set; }

        public bool IsCooldown { get; private set; }
        public float Delay { get; set; }
        private float currentTime;
        /// <summary>
        /// Cooldown button in time (miliseconds)
        /// if button is Cooldown-ing, cooldown timer will be reseted
        /// </summary>
        /// <param name="time">Time to cooldown</param>
        public void Cooldown()
        {
            this.IsCooldown = true;
            this.currentTime = 0f;

            this.uiOnBeginCooldown();
        }
        #endregion

        #region Events
        public event PvZGrowButtonEventHandler OnPressed;
        public event PvZGrowButtonEventHandler OnMoveInside;
        public event PvZGrowButtonEventHandler OnTouchLeave;
        public event CooldownButtonEventHandler OnBeginCooldown;
        public event CooldownButtonEventHandler OnCooldownEnd;
        #endregion

        protected void uiOnPressed(FreeTap ge)
        {
            if (this.OnPressed != null)
            {
                this.OnPressed(this, ge);
            }
        }

        protected void uiOnMoveInside(FreeTap ge)
        {
            if (this.OnMoveInside != null)
            {
                this.OnMoveInside(this, ge);
            }
        }

        protected void uiOnTouchLeave(FreeTap ge)
        {
            if (this.OnTouchLeave != null)
            {
                this.OnTouchLeave(this, ge);
            }
        }

        protected void uiOnBeginCooldown()
        {
            if (this.OnBeginCooldown != null)
            {
                this.OnBeginCooldown(this);
            }
        }

        protected void uiOnCooldownEnd()
        {
            if (this.OnCooldownEnd != null)
            {
                this.OnCooldownEnd(this);
            }
        }

        public override void RegisterGestures(IGestureDispatcher dispatcher)
        {
            dispatcher.AddTarget<FreeTap>(this);
        }

        public override void LeaveGestures(IGestureDispatcher dispatcher)
        {
            this.IsGestureCompleted = true;
        }

        public virtual bool ReceivedGesture(FreeTap gEvent)
        {
            if (!Enabled || IsCooldown)
            {
                if (State == PvZGrowButtonState.Highlight)
                {
                    this.uiOnTouchLeave(gEvent);
                    this.State = PvZGrowButtonState.Normal;
                }
            }
            else
            {
                if (this.Canvas.Bound.Contains(gEvent.Current))
                {
                    if (this.State != PvZGrowButtonState.Highlight && gEvent.Touch.SystemTouch.State != TouchLocationState.Released)
                    {
                        this.uiOnPressed(gEvent);
                        this.State = PvZGrowButtonState.Highlight;
                    }
                    else if (gEvent.Touch.SystemTouch.State != TouchLocationState.Released)
                    {
                        this.uiOnMoveInside(gEvent);
                    }
                    else
                    {
                        this.uiOnTouchLeave(gEvent);
                        this.State = PvZGrowButtonState.Normal;
                    }
                }
                else if (this.State == PvZGrowButtonState.Highlight)
                {
                    this.uiOnTouchLeave(gEvent);
                    this.State = PvZGrowButtonState.Normal;
                }
            }

            return false;
        }

        public virtual bool IsHandleGesture(FreeTap gEvent)
        {
            return ((this.State == PvZGrowButtonState.Highlight) || (this.Enabled && !IsCooldown && this.Canvas.Bound.Contains(gEvent.Current)));
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsCooldown)
            {
                if (currentTime >= Delay)
                {
                    this.currentTime = 0f;
                    this.IsCooldown = false;
                    this.uiOnCooldownEnd();
                }
                else
                {
                    currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.Brush == null)
                return;

            if (!this.Enabled)
            {
                this.Brush.DrawDisabled(this, gameTime);
            }
            else if (this.IsCooldown)
            {
                this.Brush.DrawCooldown(this, gameTime, Math.Min(currentTime / Delay, 1f));
            }
            else
            {
                if (this.IsOverlay || this.State == PvZGrowButtonState.Normal)
                {
                    this.Brush.DrawNormal(this, gameTime);
                }

                if (this.State == PvZGrowButtonState.Highlight)
                {
                    this.Brush.DrawHighlight(this, gameTime);
                }
            }

            base.Draw(gameTime);
        }

        public PvZGrowButton(Game game)
            : base(game)
        {
            this.Priority = 0;
        }

        public PvZGrowButton(Game game, IPvZGrowButtonBrush brush)
            : base(game)
        {
            this.Brush = brush;
            this.Priority = 0;
        }

        public uint Priority { get; set; }
    }

    public interface IPvZGrowButtonBrushFactory : ISerializable
    {
        IPvZGrowButtonBrush CreateBrush();
    }

    class StandardGrowButtonBrush : IPvZGrowButtonBrush
    {
        public ISprite Picture { get; set; }
        SpritePlayer sprPlayer;
        SpriteFont cooldownFont;
        SpriteBatch sprBatch;

        public StandardGrowButtonBrush(SpritePlayer sp, SpriteFont sf, SpriteBatch sb)
        {
            this.sprPlayer = sp;
            this.cooldownFont = sf;
            this.sprBatch = sb;
        }

        public void DrawDisabled(PvZGrowButton button, GameTime gameTime)
        {
            sprPlayer.Draw(Picture, button.Canvas.Bound.Rectangle, Color.DarkGray);
        }

        public void DrawNormal(PvZGrowButton button, GameTime gameTime)
        {
            sprPlayer.Draw(Picture, button.Canvas.Bound.Rectangle, Color.White);
        }
        
        public void DrawHighlight(PvZGrowButton button, GameTime gameTime)
        {
            sprPlayer.Draw(Picture, button.Canvas.Bound.Rectangle, Color.LightGreen);
        }

        public void DrawCooldown(PvZGrowButton button, GameTime gameTime, float cooldownPercent)
        {
            sprPlayer.Draw(Picture, button.Canvas.Bound.Rectangle, Color.DarkGray);
            string cooldownText = string.Format("{0}%", (int)(Math.Round(cooldownPercent * 100f)));
            Vector2 cooldownTextSize = cooldownFont.MeasureString(cooldownText);
            Vector2 cooldownTextPosition = new Vector2((button.Canvas.Bound.Width - cooldownTextSize.X) / 2,
                                                    (button.Canvas.Bound.Height - cooldownTextSize.Y) / 2);
            sprBatch.DrawString(cooldownFont, cooldownText, cooldownTextPosition, Color.White);
        }
    }

    class StandardGrowButtonBrushFactory : IPvZGrowButtonBrushFactory
    {
        string pictureName;
        SpriteFont defaultFont;

        public StandardGrowButtonBrushFactory()
        {
            defaultFont = SCSServices.Instance.ResourceManager.GetResource<SpriteFont>("GrowCooldownFont");
        }

        public IPvZGrowButtonBrush CreateBrush()
        {
            StandardGrowButtonBrush brush = new StandardGrowButtonBrush(SCSServices.Instance.SpritePlayer, defaultFont, SCSServices.Instance.SpriteBatch);
            brush.Picture = SCSServices.Instance.ResourceManager.GetResource<ISprite>(pictureName);

            return brush;
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            this.pictureName = deserializer.DeserializeString("Picture");
        }
    }

    public class GrowButtonBrushFactoryFactory
    {
        private GrowButtonBrushFactoryFactory()
        {
        }

        public static GrowButtonBrushFactoryFactory Instance { get; private set; }

        static GrowButtonBrushFactoryFactory()
        {
            Instance = new GrowButtonBrushFactoryFactory();
        }

        public IPvZGrowButtonBrushFactory CreateFactory(String factType)
        {
            if (factType == "Standard")
            {
                return new StandardGrowButtonBrushFactory();
            }

            return null;
        }
    }

    public class PvZGrowButtonFactory : ISerializable
    {
        public string Name { get; private set; }
        IPvZGrowButtonBrushFactory brushF;
        PvZPlantShadowFactory shadowF;
        PvZPlantShadowFactoryBank shadowFBank;
        float delay;

        public PvZGrowButtonFactory(PvZPlantShadowFactoryBank shadowFB)
        {
            this.shadowFBank = shadowFB;
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            this.Name = deserializer.DeserializeString("Name");
            delay = (float) deserializer.DeserializeDouble("Delay");
            IDeserializer dcDeser = deserializer.SubDeserializer("DrawComponent");
            string dcType = dcDeser.DeserializeString("Type");
            brushF = GrowButtonBrushFactoryFactory.Instance.CreateFactory(dcType);
            dcDeser.Deserialize("Data", brushF);
            this.shadowF = this.shadowFBank.GetPlantShadowFactory(deserializer.DeserializeString("Shadow"));
        }

        public PvZGrowButton CreateButton(Game game)
        {
            PvZGrowButton button = new PvZGrowButton(game, brushF.CreateBrush());
            button.Delay = delay;
            button.ShadowFactory = this.shadowF;

            return button;
        }
    }

    public class PvZGrowButtonFactoryBank : Dictionary<string, PvZGrowButtonFactory>, ISerializable
    {
        private PvZPlantShadowFactoryBank shadowFB;

        public PvZGrowButtonFactoryBank(PvZPlantShadowFactoryBank shadowFactoryBank)
        {
            this.shadowFB = shadowFactoryBank;
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            var btDesers = deserializer.DeserializeAll("Button");
            foreach (var btDeser in btDesers)
            {
                PvZGrowButtonFactory pvzF = new PvZGrowButtonFactory(this.shadowFB);
                pvzF.Deserialize(btDeser);

                this.Add(pvzF.Name, pvzF);
            }
        }
    }
}