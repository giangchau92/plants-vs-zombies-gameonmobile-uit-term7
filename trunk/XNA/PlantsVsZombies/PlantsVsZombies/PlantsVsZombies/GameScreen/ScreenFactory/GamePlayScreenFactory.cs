using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement;
using SCSEngine.GestureHandling;
using PlantsVsZombies.GrowSystem;
using SCSEngine.Serialization.XmlSerialization;
using System.IO;

namespace PlantsVsZombies.GameScreen.ScreenFactory
{
    public class GamePlayScreenFactory : IGameScreenFactory
    {
        private IGameScreenManager manager;
        private IGestureManager gesMan;
        private PvZGrowSystem growSystem;

        public GamePlayScreenFactory(IGameScreenManager screenManager, IGestureManager gMan)
        {
            this.gesMan = gMan;
            this.manager = screenManager;
        }

        public IGameScreen CreateGameScreen()
        {
            if (this.growSystem != null)
            {
                this.growSystem = new PvZGrowSystem(this.manager.Game);
                this.growSystem.Deserialize(XmlSerialization.Instance.Deserialize(File.Open(@"Xml\PlantGrowButtons.xml", FileMode.Open, FileAccess.Read, FileShare.None)));
            }

            return new GamePlayScreen(this.manager, this.gesMan, this.growSystem);
        }
    }
}
