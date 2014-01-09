using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement.Implement;
using Microsoft.Xna.Framework;
using PlantsVsZombies.GameScreen.ScreenFactory;
using PlantsVsZombies.GameScreen;
using SCSEngine.GestureHandling;

namespace PlantsVsZombies.GameCore
{
    public class PZScreenManager : BaseGameScreenManager
    {
        public PZScreenManager(Game game, IGestureManager gMan)
            : base(game, BaseGameScreenManagerFactory.Instance)
        {
            // add test factory to bank
            GamePlayScreenFactory playScreen = new GamePlayScreenFactory(this, gMan);
            this.Bank.AddScreenFactory("PlayScreen", playScreen);
            var mainMenu = new MainMenuScreenFactory(this, gMan);
            this.Bank.AddScreenFactory("MainMenu", mainMenu);
            var win = new MessageGameScreenFactory(this, @"Images\Controls\WinGame", "WinGame");
            var lose = new MessageGameScreenFactory(this, @"Images\Controls\LoseGame", "LoseGame");
            this.Bank.AddScreenFactory("WinGame", win);
            this.Bank.AddScreenFactory("LoseGame", lose);
            this.Bank.AddScreenFactory("Option", new OptionScreenFactory(this, gMan));
            this.Bank.AddScreenFactory("Exit", new ExitScreenFactory(this, gMan));
            this.Bank.AddScreenFactory("Help", new HelpScreenFactory(this, gMan));
            this.Bank.AddScreenFactory("Introduction", new IntroductionScreenFactory(this));
        }
    }
}
