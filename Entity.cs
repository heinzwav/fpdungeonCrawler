using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Entity
    {
        public TableLayoutPanelCellPosition currentPos;
        public int healthpoints { get; set; }
        public int damage { get; set; }

        public void TakeDamage(int damage)
        {
            healthpoints -= damage;
        }

        public void DoDamage(int damage, Entity entity)
        {
            entity.healthpoints -= damage;
        }
    }

}
