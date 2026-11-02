using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Core
{
    public struct GridPos
    {
        public int X { get; init; }
        public int Y { get; init; }

        public GridPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static GridPos operator +(GridPos a, GridPos b)
        {
            return new GridPos(a.X + b.X, a.Y + b.Y);
        }

        public static GridPos operator *(GridPos a, int multiplicator)
        {
            return new GridPos(a.X * multiplicator, a.Y * multiplicator);
        }
    }
}
