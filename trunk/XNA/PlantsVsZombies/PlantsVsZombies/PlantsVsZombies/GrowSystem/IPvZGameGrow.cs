using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GrowSystem
{
    public interface IPvZGameGrow
    {
        CRectangleF CellContains(CRectangleF growRect);
        bool GrowPlant(string plantName, CRectangleF growRect);
    }

    public class PvZGameGrow : IPvZGameGrow
    {
        private PZBoard _gameBoard;

        public PvZGameGrow(PZBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public CRectangleF CellContains(CRectangleF growRect)
        {
            if (!_gameBoard.Bound.Contains(new Point((int)growRect.Center.X, (int)growRect.Center.Y)))
            {
                return null;
            }

            if (getPlantAt(growRect) != null)
                return null;

            CRectangleF rect = _gameBoard.GetRectAtPoint(growRect.Center);
            ////Debug.WriteLine("CellContains at " + rect.X + " " + rect.Y);
            //rect.Center.Y += rect.Size.Y;
            return rect;
            
        }

        private ObjectEntity getPlantAt(CRectangleF rect)
        {
            IDictionary<ulong, ObjectEntity> fullList = PZObjectManager.Instance.GetObjects();
            IDictionary<ulong, ObjectEntity> listPlant = new Dictionary<ulong, ObjectEntity>();

            foreach (var item in fullList)
            {
                if (item.Value.ObjectType != eObjectType.PLANT)
                    continue;
                PhysicComponent phyCom = item.Value.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
                Rectangle frame = phyCom.Frame;

                if (frame.Contains((int)rect.Center.X, (int)rect.Center.Y))
                    return item.Value;
            }
            return null;
        }

        public bool GrowPlant(string plantName, CRectangleF growRect)
        {
            // call on release touch
            if (!_gameBoard.Bound.Contains(new Point((int)growRect.Center.X, (int)growRect.Center.Y)))
            {
                return false;
            }

            if (getPlantAt(growRect) != null)
                return false;

            CRectangleF rect = _gameBoard.GetRectAtPoint(growRect.Center);

            ////Debug.WriteLine("GrowPlant at " + rect.X + " " + rect.Y);

            var plant = PZObjectFactory.Instance.createPlant(plantName, new Vector2(rect.Left, rect.Bottom));
            if (plant != null)
            {
                PZObjectManager.Instance.AddObject(plant);

                return true;
            }

            return false;
        }
    }

    
}
