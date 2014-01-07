using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameCore
{
    public class PZBoard
    {
        public int[,] Board { get; set; }
        public const int CELL_WIDTH = 60;
        public const int CELL_HEIGHT = 90;

        public Vector2 Position { get; private set; }
        public Rectangle Bound { get; private set; }

        PZObjectManager objectManager;

        public PZBoard(int column, int row, PZObjectManager objMan)
        {
            Board = new int[row, column];
            Position = Vector2.Zero;

            objectManager = objMan;
        }

        public Vector2 GetPositonAt(int row, int col)
        {
            Vector2 result = Position + new Vector2(col * CELL_WIDTH, (row + 1) * CELL_HEIGHT);
            return result;
        }

        public Vector2 GetPositionAtPoint(Vector2 point)
        {
            Vector2 delta = point - Position;

            Vector2 result = Position + new Vector2((int)(delta.X / CELL_WIDTH ) * CELL_WIDTH, (int)(delta.Y / CELL_HEIGHT + 1) * CELL_HEIGHT);
            return result;
        }

        public CRectangleF GetRectAtPoint(Vector2 point)
        {
            Vector2 pos = GetPositionAtPoint(point);
            return new CRectangleF(pos.X, pos.Y - CELL_HEIGHT, CELL_WIDTH, CELL_HEIGHT);
        }

        public void AddObjectAt(ObjectEntity obj, int row, int col)
        {
            MoveComponent moveCom = obj.GetComponent(typeof(MoveComponent)) as MoveComponent;
            moveCom.Position = GetPositonAt(row, col);

            objectManager.AddObject(obj);
        }
    }
}
