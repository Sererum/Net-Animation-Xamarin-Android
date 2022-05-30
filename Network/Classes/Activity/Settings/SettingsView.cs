using Android.Content;
using Android.Graphics;
using Android.Views;
using Network.Classes.DataNet;

namespace Network.Classes.Activity.Settings
{
    class SettingsView : View
    {
        private Paint _paint;
        public SettingsView (Context context) : base(context)
        {
            _paint = new Paint();
        }
        protected override void OnDraw (Canvas canvas)
        {
            base.OnDraw(canvas);

            canvas.DrawColor(Data.IdToColors[NetState.IdBackgroundColor]);

            float meanCordinateX = Data.ScreenWidth / 6;
            float meanCordinateY = Data.ScreenHeight / 2;
            float halfLengthConnect = NetState.LengthConnect / 2;

            _paint.Color = Data.IdToColors[NetState.IdPartColor];
            _paint.StrokeWidth = NetState.SizeParts;

            canvas.DrawCircle(meanCordinateX, meanCordinateY - halfLengthConnect, NetState.SizeParts * 2, _paint);
            canvas.DrawCircle(meanCordinateX, meanCordinateY + halfLengthConnect, NetState.SizeParts * 2, _paint);
            canvas.DrawLine(meanCordinateX, meanCordinateY - halfLengthConnect, meanCordinateX, meanCordinateY + halfLengthConnect, _paint);
        }
    }
}