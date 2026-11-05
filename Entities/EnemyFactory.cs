using DungeonCrawlerMitHandy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Entities
{
    public class EnemyFactory
    {
        private Dictionary<EnemyType, EnemyDefinition> DefinitionsByType { get; }

        public EnemyFactory(Dictionary<EnemyType, EnemyDefinition> definitionsByType)
        {
            DefinitionsByType = definitionsByType;
        }
        public Enemy Create(EnemyType type, GridPos start, Direction facing, Random random)
        {
            EnemyDefinition def = DefinitionsByType[type];
            int maxHealth = random.Next(def.MinHealth, def.MaxHealth +1);

            return new Enemy(def.Name, maxHealth, def.AttackDamage,
                facing, start, type);
        }
    }
}
