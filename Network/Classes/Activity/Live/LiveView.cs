using Android.Content;
using Android.Graphics;
using Android.Views;
using Network.Classes.DataNet;
using Network.Classes.NetStructure;
using System.Collections.Generic;

namespace Network.Classes.Activity.Live
{
    class LiveView : View
    {
        private ShowLive ShowLive;
        private List<Part> _parts;

        private bool _gameActive;
        private Paint Paint;
        
        public LiveView (Context context) : base(context)
        {
            ShowLive = new ShowLive();
            _gameActive = true;
        }

        public List<Part> Parts
        {
            set { _parts = value; }
        }

        public void EndGame ()
        {
            _gameActive = false;
            Paint = new Paint();
            Paint.Color = Data.IdToColors[NetState.IdPartColor];
            Paint.TextSize = 80;
            Paint.TextAlign = Paint.Align.Center;
        }

        public void ResumeGame ()
        {
            _gameActive = true;

        }

        protected override void OnDraw (Canvas canvas)
        {
            base.OnDraw(canvas);
            
            if (_gameActive)
            {
                ShowLive.DrawNet(canvas, _parts);
            }
            else
            {
                canvas.DrawColor(Data.IdToColors[NetState.IdBackgroundColor]);
                canvas.DrawText(
                    text: Resources.GetString(Resource.String.pinch_screen),
                    x: Data.ScreenWidth / 2,
                    y: Data.ScreenHeight / 2,
                    paint: Paint);
            }
        }
    }
}