using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class HUD
    {
        public Layer LayerCombatTick = new()
        {
            rec = new Rectangle()
            {
                Size = new Size(30,30),
                Location = new Point(300,30)
            },            
        };
    }
}
