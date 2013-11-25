using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombies.GameCore
{
    public class PZBoard
    {
        public int[,] Board { get; set; }
        public const int Width = 60;
        public const int Height = 101;

        public Vector2 Position { get; set; }

        public PZBoard(int column, int row)
        {
            Board = new int[row, column];
            Position = Vector2.Zero;
        }

        public Vector2 GetPositonAt(int row, int col)
        {
            Vector2 result = Position + new Vector2(col * Width, row * Height);
            return result;
        }

        public Vector2 GetPositionAtPoint(Vector2 point)
        {
            Vector2 delta = point - Position;

            Vector2 result = Position + new Vector2((int)(delta.X / Width ) * Width, (int)(delta.Y / Height + 1) * Height);
            return result;
        }
    }
}
