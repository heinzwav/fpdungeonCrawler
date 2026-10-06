using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class DamageAnimation : Animation
    {
        Rectangle rec;


        public void DoDamageAnimation(Graphics graphics)
        {
            if (isActive)
            {
                rec = new Rectangle();
                rec.Size = new System.Drawing.Size(820, 690);
                rec.Location = new System.Drawing.Point(0, 0);
                using SolidBrush brush = new SolidBrush(Color.FromArgb(100, 186, 17, 17));
                graphics.FillRectangle(brush, rec);
            }
        }
    }
}
