using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.ScreenManagement;
using SCSEngine.Services;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GrowSystem
{
    public delegate void PlayBackgroundEventHandler(PlayBackground background);

    public class PlayBackground : DrawableGameComponent
    {
        private enum PlayBackgroundState
        {
            NONE,
            LEFT,
            MOVE_LEFT_TO_RIGHT,
            RIGHT,
            MOVE_RIGHT_TO_CENTER,
            CENTER
        }

        private Texture2D Background { get; set; }
        private RectangleF Frame;
        private SpriteBatch sprBatch;

        private float leftPosition = 0f, centerPosition = 275f, rightPosition = 760f;
        private float moveDuration = 1.2f, delayDuration = 0.8f;
        private float moveTime, delayTime, moveVelocity;

        private PlayBackgroundState state = PlayBackgroundState.NONE;

        public event PlayBackgroundEventHandler OnAnimatingCompleted;

        public PlayBackground(Game game, Texture2D background)
            : base(game)
        {
            this.sprBatch = SCSServices.Instance.SpriteBatch;
            this.Background = background;
        }

        public override void Initialize()
        {
            this.Frame.X = leftPosition;
            this.Frame.Y = 0f;
            this.Frame.Width = 800f;
            this.Frame.Height = 480f;

            base.Initialize();
        }

        public void StartAnimate()
        {
            this.StayAtLeft();
        }

        public void StayAtLeft()
        {
            if (this.state == PlayBackgroundState.NONE)
            {
                this.state = PlayBackgroundState.LEFT;
                this.delayTime = this.delayDuration;
                this.Frame.X = leftPosition;
            }
        }

        public void MoveLeftToRight()
        {
            if (this.state == PlayBackgroundState.LEFT)
            {
                this.state = PlayBackgroundState.MOVE_LEFT_TO_RIGHT;
                float delta = rightPosition - this.Frame.Position.X;
                this.moveVelocity = delta / this.moveDuration;
                this.moveTime = this.moveDuration;
            }
        }

        public void StayAtRight()
        {
            if (this.state == PlayBackgroundState.MOVE_LEFT_TO_RIGHT)
            {
                this.state = PlayBackgroundState.RIGHT;
                this.delayTime = this.delayDuration;
                this.Frame.X = rightPosition;
            }
        }

        public void MoveRightToCenter()
        {
            if (this.state == PlayBackgroundState.RIGHT)
            {
                this.state = PlayBackgroundState.MOVE_RIGHT_TO_CENTER;
                float delta = centerPosition - this.Frame.Position.X;
                this.moveVelocity = delta / this.moveDuration;
                this.moveTime = this.moveDuration;
            }
        }

        public void StayAtCenter()
        {
            // call event
            if (this.OnAnimatingCompleted != null)
            {
                this.OnAnimatingCompleted(this);
            }

            this.state = PlayBackgroundState.NONE;
        }

        public override void Update(GameTime gameTime)
        {
            switch (this.state)
            {
                case PlayBackgroundState.LEFT:
                case PlayBackgroundState.RIGHT:
                    this.delayTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (this.delayTime < 0)
                    {
                        if (this.state == PlayBackgroundState.LEFT)
                        {
                            this.MoveLeftToRight();
                        }
                        else
                        {
                            this.MoveRightToCenter();
                        }
                    }
                    break;
                case PlayBackgroundState.MOVE_LEFT_TO_RIGHT:
                case PlayBackgroundState.MOVE_RIGHT_TO_CENTER:
                    float velRate = this.moveVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.Frame.X += velRate;
                    this.moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (this.moveTime < 0)
                    {
                        if (this.state == PlayBackgroundState.MOVE_LEFT_TO_RIGHT)
                        {
                            this.StayAtRight();
                        }
                        else
                        {
                            this.StayAtCenter();
                        }
                    }
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.sprBatch != null && this.Background != null)
            {
                this.sprBatch.Draw(this.Background, Vector2.Zero, this.Frame.Rectangle, Color.White);
            }

            base.Draw(gameTime);
        }
    }
}
