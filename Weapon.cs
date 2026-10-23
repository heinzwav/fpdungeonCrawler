using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Weapon : Item
    {
        public int ID;
        public Weapon()
        {
            type = Item.Type.Weapon;
        }
        public int damageValue;
        public int attackSpeed;
        public Image ViewpointImage;
    }
}
