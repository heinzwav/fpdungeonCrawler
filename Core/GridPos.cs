using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Core
{
    public readonly record struct GridPos(int X, int Y)
    {
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
