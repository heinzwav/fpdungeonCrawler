using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Core
{
    public static class DirectionExtensions
    {
        public static Direction TurnRight(this Direction direction)
        {
            return (Direction)(((int)direction + 1) % 4);
        }

        public static Direction TurnLeft(this Direction direction)
        {
            return (Direction)(((int)direction + 3) % 4);
        }

        public static Direction TurnOpposite(this Direction direction)
        {
            return (Direction)(((int)direction + 2) % 4);
        }

        public static GridPos ToStep(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return new GridPos(0, -1);
                case Direction.Right:
                    return new GridPos(1, 0);
                case Direction.Down:
                    return new GridPos(0, 1);
                case Direction.Left:
                    return new GridPos(-1, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }
    }
}
