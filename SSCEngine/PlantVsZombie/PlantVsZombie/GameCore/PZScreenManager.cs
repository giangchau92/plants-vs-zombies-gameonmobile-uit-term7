using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement.Implement;
using Microsoft.Xna.Framework;
using PlantVsZombie.GameScreen.ScreenFactory;
using PlantVsZombie.GameScreen;

namespace PlantVsZombie.GameCore
{
    public class PZScreenManager : BaseGameScreenManager
    {
        public PZScreenManager(Game game)
            : base(game, BaseGameScreenManagerFactory.Instance)
        {
            // add test factory to bank
            TestScreenFactory testFactory = new TestScreenFactory(this);
            this.Bank.AddScreenFactory("Test", testFactory);
        }

    }
}
