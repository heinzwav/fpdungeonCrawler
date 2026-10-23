using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Item
    {
        public enum Type { Consumable, Weapon, Armor, Scroll }
        public Type type;

        public string name;
        public string description;
        public Image inventoryicon;
    }
}
