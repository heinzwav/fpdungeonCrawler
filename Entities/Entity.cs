using DungeonCrawlerMitHandy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Entities
{
    public abstract class Entity
    {
        public string Name { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; private set; }
        public int AttackDamage { get; private set; }
        public Direction Facing { get; private set; }
        public GridPos StartPosition { get; }
        public bool IsDead => CurrentHealth == 0;

        public Entity(string name, int maxHealth, int attackDamage, 
            Direction facing, GridPos startPosition)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            AttackDamage = attackDamage;
            Facing = facing;
            StartPosition = startPosition;
        }

        public void TakeDamage(int damage)
        {
            // Minus Damage würde die Entity heilen
            if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage));

            CurrentHealth = Math.Max(CurrentHealth - damage, 0);
        }

        public void TurnLeft()
        {
            Facing = Facing.TurnLeft();
        }

        public void TurnRight()
        {
            Facing = Facing.TurnRight();
        }
    }
}
