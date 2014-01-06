using Microsoft.Xna.Framework;
using PlantsVsZombies.GameCore;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
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
        public CRectangleF CellContains(CRectangleF growRect)
        {
            return null;
        }

        public void GrowPlant(string plantName, CRectangleF growRect)
        {
        }
    }

    public class GameGrowUtil : IPvZGameGrow
    {

        public CRectangleF CellContains(CRectangleF growRect, PZBoard board)
        {
            return board.GetRectAtPoint(growRect.Center);

        }

        public void GrowPlant(string plantName, CRectangleF growRect)
        {
            // call on release touch
            PZObjectManager.Instance.AddObject(PZObjectFactory.Instance.createPlant(new Vector2(growRect.Left, growRect.Bottom)));
        }

        public CRectangleF CellContains(CRectangleF growRect)
        {
            throw new NotImplementedException();
        }
    }
}
