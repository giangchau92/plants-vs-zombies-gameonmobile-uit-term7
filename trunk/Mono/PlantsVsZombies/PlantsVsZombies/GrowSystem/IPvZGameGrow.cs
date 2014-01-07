using Microsoft.Xna.Framework;
using PlantsVsZombies.GameCore;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GrowSystem
{
    public interface IPvZGameGrow
    {
        CRectangleF CellContains(CRectangleF growRect);
        void GrowPlant(string plantName, CRectangleF growRect);
    }

    public class DoNothingGameGrow : IPvZGameGrow
    {
        private PZBoard _gameBoard;

        public DoNothingGameGrow(PZBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public CRectangleF CellContains(CRectangleF growRect)
        {
            Vector2 lefBot = growRect.Position;
            lefBot.Y += growRect.Size.Y;
            CRectangleF rect = _gameBoard.GetRectAtPoint(growRect.Position);
            //Debug.WriteLine("CellContains at " + rect.X + " " + rect.Y);
            //rect.Position.Y += rect.Size.Y;
            return rect;
            
        }

        public void GrowPlant(string plantName, CRectangleF growRect)
        {
            // call on release touch
            Vector2 lefBot = growRect.Position;
            lefBot.Y += growRect.Size.Y;
            CRectangleF rect = _gameBoard.GetRectAtPoint(growRect.Position);

            //Debug.WriteLine("GrowPlant at " + rect.X + " " + rect.Y);
            PZObjectManager.Instance.AddObject(PZObjectFactory.Instance.createPlant(new Vector2(rect.Left, rect.Bottom)));
        }
    }

    
}
