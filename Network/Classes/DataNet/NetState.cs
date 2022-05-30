using System;

namespace Network.Classes.DataNet
{
    public static class NetState
    {

        private static int _lengthConnect;
        private static int _speedParts;
        private static int _quantityParts;
        private static int _sizeParts;

        private static int _idBackgroundColor = Resource.String.Black;
        private static int _idPartColor = Resource.String.Aquamarine;

        private static int liveStart, liveEnd, duplicateStart, duplicateEnd;

        public static int LengthConnect
        {
            get { return _lengthConnect; }
            set
            {
                if (value < 0 || value > Data.MaxLengthConnect)
                    throw new ArgumentOutOfRangeException();

                _lengthConnect = value;
            }
        }

        public static int SpeedParts
        {
            get { return _speedParts; }
            set
            {
                if (value < 0 || value > Data.MaxSpeedParts)
                    throw new ArgumentOutOfRangeException();

                _speedParts = value;
            }
        }

        public static int QuantityParts
        {
            get { return _quantityParts; }
            set
            {
                if (value < 0 || value > Data.MaxQuantityParts)
                    throw new ArgumentOutOfRangeException();

                _quantityParts = value;
            }
        }

        public static int SizeParts
        {
            get { return _sizeParts; }
            set
            {
                if (value < 0 || value > Data.MaxSizeParts)
                    throw new ArgumentOutOfRangeException();

                _sizeParts = value;
            }
        }

        public static int IdPartColor
        {
            get { return _idPartColor; }
            set
            {
                if (Data.IdToColors.ContainsKey(value) == false)
                    throw new ArgumentOutOfRangeException();

                _idPartColor = value;
            }
        }

        public static int IdBackgroundColor
        {
            get { return _idBackgroundColor; }
            set
            {
                if (Data.IdToColors.ContainsKey(value) == false)
                    throw new ArgumentOutOfRangeException();

                _idBackgroundColor = value;
            }
        }

        public static byte GetStatePart (int neighbours)
        {
            liveStart = _lengthConnect / 800 + 2;
            liveEnd = liveStart * 2;

            if (liveStart <= neighbours && neighbours <= liveEnd) // Live
                return Data.Live;

            duplicateStart = liveEnd + 1;
            duplicateEnd = duplicateStart + liveEnd - liveStart;

            if (duplicateStart <= neighbours && neighbours <= duplicateEnd) // Duplicate
                return Data.Duplicate;

            liveStart = duplicateEnd + 1;
            liveEnd = liveStart + duplicateEnd - duplicateStart;

            if (liveStart <= neighbours && neighbours <= liveEnd) // Live
                return Data.Live;

            return Data.Death;
        }

        public static bool RightLengthConnect(float x1, float y1, float x2, float y2)
        {
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) <= _lengthConnect * _lengthConnect;
        }
    }
}