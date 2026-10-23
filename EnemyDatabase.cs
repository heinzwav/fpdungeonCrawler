using fpdungeonCrawler.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public static class EnemyDatabase
    {
        //SUCCUBUS//
        public static Entity CreateSuccubus(fpdungeonCrawler.Map.Map map)
        {
            Entity succubus = new Entity()
            {
                CurrentPosition = (4,5),
                StartPosition = (4,5),
                name = "Succubus",
                healthpoints = 300,
                baseDamage = 4,
                isEnemy = true,
                mapIcon = Properties.Resources.Succubus_Idle1,
                idleAnimation = AnimationDatabase.succubusIdle,
                attackAnimation = AnimationDatabase.succubusAttack,
                deathAnimation = AnimationDatabase.succubusDeath,
                activeAnimation = AnimationDatabase.succubusIdle
            };
            map.UpdateEntityLocation(succubus);
            return succubus;
        }

        //ORC//
        public static Entity CreateOrc(fpdungeonCrawler.Map.Map map)
        {
            Entity orc = new Entity()
            {
                CurrentPosition = (1, 5),
                StartPosition = (1, 5),
                name = "orc",
                healthpoints = 300,
                baseDamage = 2,
                isEnemy = true,
                mapIcon = Properties.Resources.OrkIdle1,
                idleAnimation = AnimationDatabase.orcIdle,
                attackAnimation = AnimationDatabase.orcAttack,
                deathAnimation = AnimationDatabase.orcDeath,
                activeAnimation = AnimationDatabase.orcIdle
            };
            map.UpdateEntityLocation(orc);
            return orc;
        }
    }
}
