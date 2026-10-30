using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace fpdungeonCrawler.UI
{
    public class EnemyBar
    {
        private int barHeight;
        private int barWidth;

        private double scaleFactorX = 0.0025;
        private double scaleFactorY = 0.05;
        private double locationFactorX = 0.2;
        private double locationFactorY = 20;


        SolidBrush brush = new(Color.Red);
        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        StringFormat stringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };


        public void Draw(Entity entity, Graphics g, Layer layer)
        {
            barWidth = (int)Math.Round(layer.rec.Width * entity.healthpoints * scaleFactorX);
            barHeight = (int)Math.Round(layer.rec.Height * scaleFactorY);

            Rectangle rec = new((int)Math.Round(layer.rec.Location.X + locationFactorX * layer.rec.Width), (int)Math.Round(layer.rec.Location.Y - locationFactorY), barWidth, barHeight);
            g.FillRectangle(brush, rec);
            g.DrawString(entity.healthpoints.ToString(), drawFont, drawBrush, rec, stringFormat);
        }
    }
}
