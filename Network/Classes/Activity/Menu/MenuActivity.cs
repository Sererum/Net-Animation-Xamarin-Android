using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Network.Classes.Activity.Live;
using Network.Classes.Activity.Net;
using Network.Classes.Activity.Settings;
using Network.Classes.DataNet;

namespace Network.Classes.Activity.Menu
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MenuActivity : AppCompatActivity
    {
        private readonly float Width = Data.ScreenWidth;
        private readonly float Height = Data.ScreenHeight;

        private readonly int QuantityButtons = 3;

        private string[] _buttonsTexts;
        private float[] _buttonsCoords;
        private float _radiusButton;

        private float _lastTouchX;
        private float _lastTouchY;

#pragma warning disable CS0618 // Type or member is obsolete
        private AbsoluteLayout MenuLayout;
#pragma warning restore CS0618 // Type or member is obsolete

        private MenuView MenuView;

        private Intent NetIntent;
        private Intent LiveIntent;
        private Intent SettingsIntent;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_menu);

            NetIntent = new Intent(this, typeof(NetActivity));
            LiveIntent = new Intent(this, typeof(LiveActivity));
            SettingsIntent = new Intent(this, typeof(SettingsActivity));

#pragma warning disable CS0618 // Type or member is obsolete
        MenuLayout = FindViewById<AbsoluteLayout>(Resource.Id.MenuLayout);
#pragma warning restore CS0618 // Type or member is obsolete

            MenuView = new MenuView(this);

            MenuLayout.AddView(MenuView);

            InitButton();
            Server.LoadData();
        }

        private void InitButton ()
        {
            _buttonsTexts = new string[] {
                Resources.GetString(Resource.String.button_net),
                Resources.GetString(Resource.String.button_live), 
                Resources.GetString(Resource.String.button_settings)};

            _radiusButton = GetRadiusButton();
            _buttonsCoords = new float[QuantityButtons * 2];

            for (int i = 0; i < QuantityButtons; i++)
            {
                _buttonsCoords[i * 2] = Width / 4 + (Width / 2) * ((i + 1) % 2);
                _buttonsCoords[i * 2 + 1] = Height / 4 + Height / 4 * i;
            }

            MenuView.SetButtonProperties(_buttonsCoords, _buttonsTexts);
        }

        private void ButtonListener(float touchX, float touchY)
        {
            if (InButtonArea(_buttonsCoords[0], _buttonsCoords[1], touchX, touchY))
                StartActivity(NetIntent);

            if (InButtonArea(_buttonsCoords[2], _buttonsCoords[3], touchX, touchY))
                StartActivity(LiveIntent);

            if (InButtonArea(_buttonsCoords[4], _buttonsCoords[5], touchX, touchY))
                StartActivity(SettingsIntent);
        }

        public override bool OnTouchEvent (MotionEvent e)
        {
            if (_lastTouchX != e.GetX() && _lastTouchY != e.GetY())
                ButtonListener(e.GetX(), e.GetY());

            _lastTouchX = e.GetX();
            _lastTouchY = e.GetY();

            return base.OnTouchEvent(e);
        }

        protected override void OnResume ()
        {
            base.OnResume();
            MenuView.RadiusButton = GetRadiusButton();
            MenuView.Invalidate();
        }

        private bool InButtonArea(float buttonX, float buttonY, float touchX, float touchY)
        {
            return ((buttonX - touchX) * (buttonX - touchX) + (buttonY - touchY) * (buttonY - touchY)) <= _radiusButton * _radiusButton;
        }

        private float GetRadiusButton ()
        {
            return (Width / 6) + (Width / 111 * NetState.SizeParts);
        }
    }
}