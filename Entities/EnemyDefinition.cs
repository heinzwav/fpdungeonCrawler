using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Entities
{
    public readonly record struct EnemyDefinition(EnemyType Type, string Name, int MinHealth,
        int MaxHealth, int AttackDamage);
}
