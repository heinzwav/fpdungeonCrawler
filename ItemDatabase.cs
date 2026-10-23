using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public static class ItemDatabase
    {
        public static Weapon Axe = new()
        {
            name = "Axe of Fembube",
            damageValue = 13
        };

        public static Weapon Sword = new()
        {
            name = "Sword of Fembube",
            damageValue = 10
        };

        public static Weapon Unarmed = new()
        {
            name = "Fists",
            damageValue = 0
        };
    }
}
