using Microsoft.Xna.Framework;
using SCSEngine.Control;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GrowSystem
{
    public delegate void ChooseButtonAvatarEventHandler(PvZChooseButtonAvatar button);

    public class PvZChooseButtonAvatar : BaseUIControl
    {
        private enum AnimateState
        {
            NONE,
            ZOOM,
            MOVE
        }

        public PvZChooseButton Parent { get; set; }

        public PvZChooseButtonAvatar(Game game)
            : base(game)
        {
            this.spritePlayer = SCSServices.Instance.SpritePlayer;
        }

        public PvZChooseButtonAvatar(Game game, float zoomRate, float zoomDur, float moveDur)
            : base(game)
        {
            this.zoomRatio = zoomRate;
            this.zoomDuration = zoomDur;
            this.moveDuration = moveDur;
            this.spritePlayer = SCSServices.Instance.SpritePlayer;
        }

        private AnimateState state;

        private AnimateState State
        {
            get { return state; }
        }

        private float zoomRatio = 0.2f, zoomDuration = 0.2f;

        public float ZoomDuration
        {
            get { return zoomDuration; }
            set { zoomDuration = value; }
        }

        public float ZoomRatio
        {
            get { return zoomRatio; }
            set { zoomRatio = value; }
        }
        private float moveDuration = 0.5f;

        public float MoveDuration
        {
            get { return moveDuration; }
            set { moveDuration = value; }
        }
        private float zoomTime, moveTime;
        private Vector2 zoomOriginSize, moveOriginVelocity;
        private RectangleF destRect;

        private SpritePlayer spritePlayer;
        public ISprite Background { get; set; }

        public event ChooseButtonAvatarEventHandler OnCompleteAnimating;

        public override void RegisterGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
        }

        public override void LeaveGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
        }

        public void StartAnimating(RectangleF destRect)
        {
            if (this.state == AnimateState.NONE)
            {
                this.destRect = destRect;
                this.startZoom();
            }
        }

        private void startZoom()
        {
            this.state = AnimateState.ZOOM;
            this.zoomTime = zoomDuration;
            this.zoomOriginSize = this.Canvas.Bound.Size;
        }

        private void startMove()
        {
            this.state = AnimateState.MOVE;
            this.zoomTime = zoomDuration;
            this.moveTime = moveDuration;
            this.moveOriginVelocity = destRect.Center - this.Canvas.Bound.Center;
            this.moveOriginVelocity /= this.moveDuration;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.state == AnimateState.ZOOM)
            {
                float zoomRate = zoomRatio * (float) gameTime.ElapsedGameTime.TotalSeconds;
                this.scaleCanvas(zoomRate);
                this.zoomTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.zoomTime < 0)
                {
                    this.startMove();
                }
            }
            else if (this.state == AnimateState.MOVE)
            {
                this.moveCanvas((float)gameTime.ElapsedGameTime.TotalSeconds);
                this.moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.zoomTime > 0)
                {
                    float zoomRate = zoomRatio * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.scaleCanvas(-zoomRate);
                    this.zoomTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (this.zoomTime < 0)
                {
                    this.Canvas.Bound.Size = destRect.Size;
                }

                if (this.moveTime < 0)
                {
                    this.Canvas.Bound.Position = destRect.Position;
                    this.state = AnimateState.NONE;

                    if (this.OnCompleteAnimating != null)
                    {
                        this.OnCompleteAnimating(this);
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.Background != null)
            {
                this.spritePlayer.Draw(this.Background, this.Canvas.Bound.Rectangle, Color.White);
            }

            base.Draw(gameTime);
        }

        private void scaleCanvas(float ratio)
        {
            Vector2 scaleSize = this.zoomOriginSize * ratio;
            this.Canvas.Bound.Size += scaleSize;
            this.Canvas.Bound.Position -= scaleSize / 2f;
        }

        private void moveCanvas(float ratio)
        {
            Vector2 delta = this.moveOriginVelocity * ratio;
            this.Canvas.Bound.Position += delta;
        }

        public void KillMe()
        {
            this.IsUICompleted = true;
        }
    }
}
