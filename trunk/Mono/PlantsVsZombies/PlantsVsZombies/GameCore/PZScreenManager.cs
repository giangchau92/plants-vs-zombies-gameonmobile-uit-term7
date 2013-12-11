using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement.Implement;
using Microsoft.Xna.Framework;
using PlantVsZombies.GameScreen.ScreenFactory;
using PlantVsZombies.GameScreen;

namespace PlantVsZombies.GameCore
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
