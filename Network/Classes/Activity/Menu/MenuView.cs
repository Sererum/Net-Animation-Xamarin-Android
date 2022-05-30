using Android.Content;
using Android.Graphics;
using Android.Views;
using Network.Classes.DataNet;

namespace Network.Classes.Activity.Menu
{
    class MenuView : View
    {
        private float[] _coordinatesButtons;
        private float _radiusButton;

        private float _textCoordY;

        private string[] _textsButtons;

        private Paint PaintPart;
        private Paint PaintBack;
        public MenuView (Context context) : base(context)
        {
            PaintPart = new Paint();
            PaintBack = new Paint();
        }

        public float RadiusButton
        {
            set { _radiusButton = value; }
        }

        public void SetButtonProperties(float[] coordinates, string[] texts)
        {
            _coordinatesButtons = coordinates;
            _textsButtons = texts;
        }

        protected override void OnDraw (Canvas canvas)
        {
            base.OnDraw(canvas);

            PaintBack.Color = Data.IdToColors[NetState.IdBackgroundColor];
            PaintBack.TextSize = 60 + NetState.SizeParts * 3;
            PaintBack.TextAlign = Paint.Align.Center;

            canvas.DrawColor(PaintBack.Color);

            PaintPart.Color = Data.IdToColors[NetState.IdPartColor];
            PaintPart.StrokeWidth = 100 + NetState.SizeParts * 10;

            for (int i = 1; i < _coordinatesButtons.Length / 2; i++)
                canvas.DrawLine(_coordinatesButtons[2 * i - 2], _coordinatesButtons[2 * i - 1], _coordinatesButtons[2 * i], _coordinatesButtons[2 * i + 1], PaintPart);

            for (int i = 0; i < _coordinatesButtons.Length / 2; i++)
            {
                Rect bounds = new Rect();
                PaintBack.GetTextBounds(_textsButtons[i], 0, _textsButtons[i].Length, bounds);
                _textCoordY = _coordinatesButtons[2 * i + 1] + bounds.Height() * (1f/4);

                canvas.DrawCircle(_coordinatesButtons[2 * i], _coordinatesButtons[2 * i + 1], _radiusButton + NetState.SizeParts * (2f/3), PaintBack);
                canvas.DrawCircle(_coordinatesButtons[2 * i], _coordinatesButtons[2 * i + 1], _radiusButton, PaintPart);

                canvas.DrawText(_textsButtons[i], _coordinatesButtons[2 * i], _textCoordY, PaintBack);
            }
        }
    }
}