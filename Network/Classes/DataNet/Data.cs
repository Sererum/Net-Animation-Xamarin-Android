using Android.Graphics;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace Network.Classes.DataNet
{
    public static class Data
    {
        public static readonly int FPS = 30;

        public static readonly float ScreenWidth = (float) DeviceDisplay.MainDisplayInfo.Width;
        public static readonly float ScreenHeight = (float) DeviceDisplay.MainDisplayInfo.Height;

        public static readonly int MaxLengthConnect = 1500;
        public static readonly int MaxSpeedParts = 10;
        public static readonly int MaxQuantityParts = 300;
        public static readonly int MaxSizeParts = 10;

        public static readonly int DuplicationRate = 100;
        public static readonly int DeathRate = 10;

        public static readonly byte Death = 0, Live = 1, Duplicate = 2;

        public static Dictionary<int, Color> IdToColors = new Dictionary<int, Color>()
        {
            {Resource.String.White, Color.White},
            {Resource.String.Black, Color.Black},
            {Resource.String.Blue, Color.Blue },
            {Resource.String.Aqua, Color.Aqua },
            {Resource.String.Aquamarine, Color.Aquamarine },
            {Resource.String.Green, Color.Green },
            {Resource.String.Gold, Color.Gold },
            {Resource.String.MistyRose, Color.MistyRose },
            {Resource.String.Red, Color.Red },
            {Resource.String.Fuchsia, Color.Fuchsia }
        };
    }
}