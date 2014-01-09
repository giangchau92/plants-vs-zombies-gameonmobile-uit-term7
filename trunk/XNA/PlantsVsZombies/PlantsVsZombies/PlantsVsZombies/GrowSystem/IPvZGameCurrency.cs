using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Behaviors.Bullet;
using PlantsVsZombies.GameCore;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Mathematics;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GrowSystem
{
    public interface IPvZGameCurrency
    {
        int CurrentMoney { get; set; }
    }

    public class PvZSunSystem : DrawableGameComponent, IPvZGameCurrency
    {
        public static IGestureDispatcher _gestureDispatcher = null;
        private int MoneyPerSun = 25;
        private TimeSpan _currentTime = TimeSpan.Zero;
        private TimeSpan _timeGiveSun = TimeSpan.FromSeconds(20);

        public PvZSunSystem(Game game, int money, IGestureDispatcher gestureDispatcher)
            : base(game)
        {
            this.CurrentMoney = money;
            _collectionPoint = new Vector2(20, 430);
            _gestureDispatcher = gestureDispatcher;
            this.DrawOrder = 2;
        }

        public int CurrentMoney
        {
            get;
            set;
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentTime > _timeGiveSun)
            {
                // Give sun
                float posX = GRandom.RandomFloat(800f);

                AddSun(new Vector2(posX, 0), eSunState.PLYING);
                _currentTime = TimeSpan.Zero;
            }
            else
                _currentTime += gameTime.ElapsedGameTime;
        }

        public void AddSun(Vector2 position, eSunState state)
        {
            ObjectEntityGesture sun = PZObjectFactory.Instance.createSun(position, state) as ObjectEntityGesture;
            PZObjectManager.Instance.AddObject(sun);
            _gestureDispatcher.AddTarget<Tap>(sun);
        }

        public void RemoveSun(ObjectEntityGesture obj)
        {
            PZObjectManager.Instance.RemoveObject(obj.ObjectId);
            PvZSunSystem._gestureDispatcher.RemoveTarget<Tap>(obj as IGestureTarget<Tap>);
            CurrentMoney += MoneyPerSun;
        }

        public override void Draw(GameTime gameTime)
        {
            SCSServices.Instance.SpriteBatch.DrawString(SCSServices.Instance.DebugFont, CurrentMoney.ToString(), new Vector2(20, 420), Color.Red, 0, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1);
        }

        private static Vector2 _collectionPoint;
        public static Vector2 CollectionPoint
        {
            get { return _collectionPoint; }
        }
    }
}
