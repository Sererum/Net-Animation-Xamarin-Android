using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Network.Classes.DataNet;
using Network.Classes.NetStructure;
using System.Timers;

namespace Network.Classes.Activity.Net
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class NetActivity : AppCompatActivity
    {
        private FrameLayout LiveLayout;
        private NetView NetView;

        private Timer Timer;

        private NetParts NetParts;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_live);

            Window.AddFlags(Android.Views.WindowManagerFlags.KeepScreenOn);

            LiveLayout = FindViewById<FrameLayout>(Resource.Id.LiveLayout);

            NetParts = new NetParts();
            NetView = new NetView(this);

            NetParts.CreateNewNet();

            LiveLayout.AddView(NetView);

            InitTouchListener();
            InitTimer();
        }

        private void InitTimer ()
        {
            Timer = new Timer();
            Timer.Interval = 1000 / Data.FPS;
            Timer.Elapsed += OnTimedEvent;
            Timer.Enabled = true;
        }

        private void OnTimedEvent (object sender, System.Timers.ElapsedEventArgs e)
        {
            NetParts.UpdateStateNet();

            NetView.Parts = NetParts.ListParts;
            NetView.Invalidate();
        }

        private void InitTouchListener ()
        {
            LiveLayout.Click += delegate
            {
                if (Timer.Enabled == true)
                    Timer.Enabled = false;
                else
                    Timer.Enabled = true;
            };

            LiveLayout.LongClick += delegate
            {
                NetParts.CreateNewNet();
            };
        }
    }
}