using DungeonCrawlerMitHandy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Entities
{
    public class Player : Entity
    {
        public Player(string name, int maxHealth, int attackDamage, Direction facing, GridPos start, bool irgendeinwert) 
            : base(name, maxHealth, attackDamage, facing, start)
        {
        }
    }
}
