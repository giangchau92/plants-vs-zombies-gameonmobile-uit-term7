using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement;
using SCSEngine.GestureHandling;

namespace PlantsVsZombies.GameScreen.ScreenFactory
{
    public class GamePlayScreenFactory : IGameScreenFactory
    {
        private IGameScreenManager manager;
        private IGestureManager gesMan;

        public GamePlayScreenFactory(IGameScreenManager screenManager, IGestureManager gMan)
        {
            this.gesMan = gMan;
            this.manager = screenManager;
        }

        public IGameScreen CreateGameScreen()
        {
            return new GamePlayScreen(this.manager, this.gesMan);
        }
    }
}
