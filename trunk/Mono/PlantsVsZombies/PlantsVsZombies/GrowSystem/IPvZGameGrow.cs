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
}
