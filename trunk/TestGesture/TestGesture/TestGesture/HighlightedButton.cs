using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Utils.Adapters;
using SSCEngine.Control;
using SSCEngine.GestureHandling;
using SSCEngine.GestureHandling.Implements.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGesture
{
    public delegate void HighlightedButtonEventHandler(HighlightedButton button, FreeTap leaveGesture);
    public delegate void CooldownButtonEventHandler(HighlightedButton button);

    public interface IHighlightedButtonBrush
    {
        void DrawNormal(HighlightedButton button, GameTime gameTime);
        void DrawHighlight(HighlightedButton button, GameTime gameTime);
        void DrawCooldown(HighlightedButton button, GameTime gameTime, float cooldownPercent);
    }

    public class HighlightedButton : BaseUIControl, IGestureTarget<FreeTap>
    {
        #region Draw
        public IHighlightedButtonBrush Brush { get; set; }

        public bool IsOverlay = false;
        #endregion

        #region ButtonDrawState
        public enum HighlightedButtonState { Normal, Highlight};
        public HighlightedButtonState State { get; private set; }

        public bool IsCooldown { get; private set; }
        public float totalTime;
        public float currentTime;
        /// <summary>
        /// Cooldown button im time (miliseconds)
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
        public event HighlightedButtonEventHandler OnPressed;
        public event HighlightedButtonEventHandler OnMoveInside;
        public event HighlightedButtonEventHandler OnTouchLeave;
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
            if (IsCooldown)
            {
                if (State == HighlightedButtonState.Highlight)
                {
                    this.uiOnTouchLeave(gEvent);
                    this.State = HighlightedButtonState.Normal;
                }
            }
            else
            {
                if (this.Canvas.Bound.Contains(gEvent.Current))
                {
                    if (this.State != HighlightedButtonState.Highlight && gEvent.Touch.SystemTouch.State != TouchLocationState.Released)
                    {
                        this.uiOnPressed(gEvent);
                        this.State = HighlightedButtonState.Highlight;
                    }
                    else if (gEvent.Touch.SystemTouch.State != TouchLocationState.Released)
                    {
                        this.uiOnMoveInside(gEvent);
                    }
                    else
                    {
                        this.uiOnTouchLeave(gEvent);
                        this.State = HighlightedButtonState.Normal;
                    }
                }
                else if (this.State == HighlightedButtonState.Highlight)
                {
                    this.uiOnTouchLeave(gEvent);
                    this.State = HighlightedButtonState.Normal;
                }
            }

            return false;
        }

        public virtual bool IsHandleGesture(FreeTap gEvent)
        {
            return (!IsCooldown && (this.State == HighlightedButtonState.Highlight || this.Canvas.Bound.Contains(gEvent.Current)));
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

            if (this.IsCooldown)
            {
                this.Brush.DrawCooldown(this, gameTime, Math.Min(currentTime / totalTime, 1f));
            }
            else
            {
                if (this.IsOverlay || this.State == HighlightedButtonState.Normal)
                {
                    this.Brush.DrawNormal(this, gameTime);
                }

                if (this.State == HighlightedButtonState.Highlight)
                {
                    this.Brush.DrawHighlight(this, gameTime);
                }
            }

            base.Draw(gameTime);
        }

        public HighlightedButton(Game game)
            : base(game)
        {
            this.Priority = 0;
        }

        public HighlightedButton(Game game, IHighlightedButtonBrush brush)
            : base(game)
        {
            this.Brush = brush;
            this.Priority = 0;
        }

        public uint Priority { get; set; }
    }
}