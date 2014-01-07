using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Behaviors.Bullet;
using PlantsVsZombies.GameCore;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GrowSystem
{
    public interface IPvZGameCurrency
    {
        int CurrentMoney { get; }
    }

    public class PvZSunSystem : DrawableGameComponent, IPvZGameCurrency
    {
        public static IGestureDispatcher _gestureDispatcher = null;
        private int MoneyPerSun = 25;
        private TimeSpan _currentTime = TimeSpan.Zero;
        private TimeSpan _timeGiveSun = TimeSpan.FromSeconds(5);

        public PvZSunSystem(Game game, int money, IGestureDispatcher gestureDispatcher)
            : base(game)
        {
            this.CurrentMoney = money;
            _collectionPoint = new Vector2(0, 0);
            _gestureDispatcher = gestureDispatcher;
        }

        public int CurrentMoney
        {
            get;
            private set;
        }


        Random rand = new Random();

        public override void Update(GameTime gameTime)
        {
            if (_currentTime > _timeGiveSun)
            {
                // Give sun
                float posX = (float)(rand.NextDouble() * 800);

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
            SCSServices.Instance.SpriteBatch.DrawString(SCSServices.Instance.DebugFont, CurrentMoney.ToString(), new Vector2(100, 50), Color.Yellow, 0, new Vector2(0.5f, 0.5f), 3, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1);
        }


        private static Vector2 _collectionPoint;
        public static Vector2 CollectionPoint
        {
            get { return _collectionPoint; }
        }
    }
}
