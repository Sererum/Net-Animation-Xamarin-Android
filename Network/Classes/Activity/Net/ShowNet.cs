using Android.Graphics;
using Network.Classes.DataNet;
using System;
using System.Collections.Generic;

namespace Network.Classes.NetStructure
{
    class ShowNet
    {
        private Paint Paint;

        private float _positionX;
        private float _positionY;

        public ShowNet ()
        {
            Paint = new Paint();
        }

        public virtual void DrawNet (Canvas canvas, List<Part> parts)
        {
            Paint.Color = Data.IdToColors[NetState.IdPartColor];
            Paint.StrokeWidth = NetState.SizeParts;

            canvas.DrawColor(Data.IdToColors[NetState.IdBackgroundColor]);

            if (parts == null)
                return;

            foreach (Part part in TryCopyPartList(parts))
            {
                _positionX = part.Position.X;
                _positionY = part.Position.Y;

                canvas.DrawCircle(_positionX, _positionY, NetState.SizeParts * 2, Paint);

                foreach (Part neighbour in TryCopyPartList(part.ListNeighbours))
                {
                    if (neighbour == null ||
                        NetState.RightLengthConnect(_positionX, _positionY, neighbour.Position.X, neighbour.Position.Y) == false)
                        continue;

                    canvas.DrawLine(_positionX, _positionY, neighbour.Position.X, neighbour.Position.Y, Paint);
                }
            }
        }

        private List<Part> TryCopyPartList (List<Part> parts)
        {
            try
            { return new List<Part>(parts); }
            catch (ArgumentException) { }

            return new List<Part>();
        }
    }
}