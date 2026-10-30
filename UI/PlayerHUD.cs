using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler.UI
{
    public class PlayerHUD
    {
        public int barHeight;
        public int barWidth;

        public double scaleFactorX = 0.0025;
        public double scaleFactorY = 0.05;
        public double locationFactorX = 0.02;
        public double locationFactorY = 500;

        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        SolidBrush redbrush = new(Color.Red);
        SolidBrush greenbrush = new(Color.Green);
        SolidBrush bluebrush = new(Color.Blue);

        StringFormat stringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public void Draw(Player player, Graphics g, Layer layer)
        {
            barWidth = (int)Math.Round(layer.rec.Width * player.healthpoints * scaleFactorX);
            barHeight = (int)Math.Round(layer.rec.Height * scaleFactorY);

            Rectangle healthbar = new((int)Math.Round(layer.rec.Location.X + locationFactorX * layer.rec.Width), (int)Math.Round(layer.rec.Location.Y + locationFactorY), barWidth, barHeight);
            
            g.FillRectangle(redbrush, healthbar);
            g.DrawString(player.healthpoints.ToString(), drawFont, drawBrush, healthbar, stringFormat);
        }
    }
}
