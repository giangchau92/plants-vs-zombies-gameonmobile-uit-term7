using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
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
        #region Draw
        public IPvZGrowButtonBrush Brush { get; set; }

        public bool IsOverlay = false;
        #endregion

        #region ButtonDrawState
        public enum PvZGrowButtonState { Normal, Highlight};
        public PvZGrowButtonState State { get; private set; }

        public bool IsCooldown { get; private set; }
        public float totalTime;
        public float currentTime;
        /// <summary>
        /// Cooldown button in time (miliseconds)
        /// if button is Cooldown-ing, cooldown timer will be reseted
        /// </summary>
        /// <param name="time">Time to cooldown</param>
        public void CooldownInTime(float time)
        {
            this.IsCooldown = true;
            this.totalTime = time;
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
                if (currentTime >= totalTime)
                {
                    this.currentTime = 0;
                    this.totalTime = 0;
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
                this.Brush.DrawCooldown(this, gameTime, Math.Min(currentTime / totalTime, 1f));
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
            return null;
        }
    }

    public class PvZGrowButtonFactory : ISerializable
    {
        public string Name { get; private set; }
        IPvZGrowButtonBrushFactory brushF;

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            this.Name = deserializer.DeserializeString("Name");
            IDeserializer dcDeser = deserializer.SubDeserializer("DrawComponent");
            string dcType = dcDeser.DeserializeString("Type");
            brushF = GrowButtonBrushFactoryFactory.Instance.CreateFactory(dcType);
            dcDeser.Deserialize("Data", brushF);
            this.shadowF = this.shadowFB.GetPlantShadowFactory(deserializer.DeserializeString("Shadow"));
        }
    }
}