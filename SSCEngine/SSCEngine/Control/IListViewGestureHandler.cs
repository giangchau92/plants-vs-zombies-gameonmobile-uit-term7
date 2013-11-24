using Microsoft.Xna.Framework;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SCSEngine.Control
{
    public interface IListViewGestureHandler
    {
        void Move(Vector2 delta);
        void Release(Vector2 delta);

        void UpdateOffset(CRectangleF offset, CRectangleF content);
    }

    public class VerticalListViewGestureHandler : IListViewGestureHandler
    {
        private float velocity;
        private float deccelerator;
        private float baseDecelerator;
        private float lastVel;

        public VerticalListViewGestureHandler(float baseDecel)
        {
            this.baseDecelerator = baseDecel;
            this.velocity = 0f;
            this.deccelerator = 0f;
        }


        public void Move(Vector2 delta)
        {
            this.velocity = delta.Y;
            this.deccelerator = 0;
        }

        public void Release(Vector2 delta)
        {
            this.velocity = lastVel;
            this.deccelerator += this.velocity * baseDecelerator;
        }

        private const float capVel = 1E-4f;
        private const float baseReconstructDecel = 0.1f;
        public void UpdateOffset(CRectangleF offset, CRectangleF content)
        {
            if (this.velocity != 0)
            {
                //Debug.WriteLine("Offset:{0} - Velocity:{1} - Decel:{2}", offset.Position, velocity, deccelerator);
                offset.Position.Y += this.velocity;
                this.lastVel = this.velocity;
                if (this.deccelerator != 0)
                {
                    this.velocity *= (1f - baseDecelerator);
                    if (Math.Abs(this.velocity) <= capVel)
                    {
                        this.velocity = 0;
                        this.deccelerator = 0;
                    }
                }
                else
                {
                    this.velocity = 0;
                }
            }

            if (offset.Position.Y > 0)
            {
                offset.Position.Y -= baseReconstructDecel * offset.Position.X;
            }

            if (offset.Right < content.Width)
            {
                offset.Position.Y -= baseReconstructDecel * (offset.Right - content.Width);
            }
        }
    }

    public class HorizontalListViewGestureHandler : IListViewGestureHandler
    {
        private float velocity;
        private float deccelerator;
        private float baseDecelerator;
        private float lastVel;
        private float maxBounces;
        private float deDecel;

        public HorizontalListViewGestureHandler(float baseDecel, float maxBounces)
        {
            this.baseDecelerator = baseDecel;
            this.velocity = 0f;
            this.deccelerator = 0f;
            this.maxBounces = maxBounces;
        }

        public void Move(Vector2 delta)
        {
            this.UpdateVelocity(delta.X);
            this.deccelerator = 0;
        }

        public void Release(Vector2 delta)
        {
            this.velocity = lastVel;
            this.deccelerator += this.velocity * baseDecelerator;
        }

        private const float capVel = 1f;
        private const float baseReconstructDecel = 0.1f;
        public void UpdateOffset(CRectangleF offset, CRectangleF content)
        {
            if (this.velocity != 0)
            {
                offset.Position.X += this.velocity;
                if (this.deccelerator != 0)
                {
                    this.UpdateVelocity(this.velocity - deccelerator);
                    this.deccelerator *= (1 - baseDecelerator);
                    if (this.velocity * this.lastVel <= 0)
                    {
                        this.UpdateVelocity(0f);
                        this.deccelerator = 0;
                        this.deDecel = 0;
                    }
                }
                else
                {
                    this.UpdateVelocity(0f);
                }
            }

            if (offset.Position.X > 0)
            {
                if (offset.Position.X > maxBounces)
                {
                    offset.Position.X = maxBounces;
                    this.UpdateVelocity(0f);
                    this.deccelerator = 0;
                }
                else
                {
                    offset.Position.X -= baseReconstructDecel * offset.Position.X;
                }

                if (offset.Position.X <= capVel)
                {
                    offset.Position.X = 0;
                }
            }

            if (offset.Right < content.Width)
            {
                if (offset.Right < content.Width - maxBounces)
                {
                    offset.Position.X = content.Width - maxBounces - offset.Size.X;
                    this.UpdateVelocity(0f);
                    this.deccelerator = 0;
                }
                else
                {
                    offset.Position.X -= baseReconstructDecel * (offset.Right - content.Width);
                }

                if (offset.Position.X >= content.Width - capVel)
                {
                    offset.Position.X = content.Width;
                }
            }
        }

        private void UpdateVelocity(float newVelocity)
        {
            this.lastVel = this.velocity;
            this.velocity = newVelocity;
        }
    }
}
