using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Layer
    {
        public bool isVisible = true;
        public Rectangle rec;
        public Image img;

        public Rectangle CreateRec(Size size, Point point)
        {
            Rectangle rec = new();
            rec.Size = size;
            rec.Location = point;
            return rec;
        }

        public Layer InitializeLayer(Size size, Point point, Image img)
        {
            Layer layer = new Layer();
            layer.rec = CreateRec(size, point);
            layer.img = img;
            return layer;
        }
    }


}
