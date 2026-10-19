using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler.UI
{
    public class EnemyBar
    {
        public int barHeight;
        public int barWidth;

        public double scaleFactor = 0.1;
        public double locationFactorX = 0.1;
        public double locationFactorY = 0.1;

        SolidBrush brush = new(Color.Red);


        public void Draw(Entity entity, Graphics g, Layer layer)
        {
            barWidth = (int)Math.Round(layer.rec.Width * entity.healthpoints * scaleFactor);
            barHeight = (int)Math.Round(layer.rec.Height * scaleFactor);


            Rectangle rec = new((int)Math.Round(layer.rec.Location.X * locationFactorX), (int)Math.Round(layer.rec.Location.Y * locationFactorY), barWidth, barHeight);
            g.FillRectangle(brush, rec);
        }
    }
}
