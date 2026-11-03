using DungeonCrawlerMitHandy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.World
{
    public readonly record struct GeneratedMap(Tile[,] Tiles, GridPos Start, GridPos Stairs)
    {
    }
}
