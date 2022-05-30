using Android.Graphics;
using Network.Classes.DataNet;
using Network.Classes.NetStructure;
using System;
using System.Collections.Generic;

namespace Network.Classes.Activity.Live
{
    class ShowLive : ShowNet
    {
        private Paint Paint;

        private float _positionX;
        private float _positionY;

        private byte _statePart;

        private Random Random;

        public ShowLive () : base()
        {
            Paint = new Paint();
            Random = new Random();
        }

        public override void DrawNet (Canvas canvas, List<Part> parts)
        {
            canvas.DrawColor(Data.IdToColors[NetState.IdBackgroundColor]);

            Paint.StrokeWidth = NetState.SizeParts;

            if (parts == null)
                return;

            foreach (Part part in TryCopyPartList(parts))
            {
                if (part == null)
                    continue;

                _positionX = part.Position.X;
                _positionY = part.Position.Y;

                _statePart = NetState.GetStatePart(part.CountNeighbours);

                Paint.Color = GetStateColor(_statePart);
                
                canvas.DrawCircle(_positionX, _positionY, NetState.SizeParts * 2, Paint);

                foreach (Part neighbour in TryCopyPartList(part.ListNeighbours))
                {
                    if (neighbour == null || 
                        NetState.RightLengthConnect(_positionX, _positionY, neighbour.Position.X, neighbour.Position.Y) == false)
                        continue;

                    Paint.Color = GetConnectColor(_statePart, NetState.GetStatePart(neighbour.CountNeighbours));
                    canvas.DrawLine(_positionX, _positionY, neighbour.Position.X, neighbour.Position.Y, Paint);
                }
            };
        }

        private Color GetConnectColor(byte statePart, byte stateNeighbour)
        {
            if (statePart == Data.Duplicate || stateNeighbour == Data.Duplicate)
                return Color.Green;
            if (statePart == Data.Live || stateNeighbour == Data.Live)
                return Color.Yellow;
            return Color.DarkRed;
        }

        private Color GetStateColor(byte state)
        {
            if (state == Data.Duplicate)
                return Color.Green;
            if (state == Data.Live)
                return Color.Yellow;
            return Color.DarkRed;
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