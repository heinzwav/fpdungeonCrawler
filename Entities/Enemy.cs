using DungeonCrawlerMitHandy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Entities
{
    public class Enemy : Entity
    {
        public EnemyType Type { get; }
        public Enemy(string name, int maxHealth, int attackDamage, Direction facing, GridPos start, EnemyType type) 
            : base(name, maxHealth, attackDamage, facing, start)
        {
            Type = type;
        }

        
    }
}
