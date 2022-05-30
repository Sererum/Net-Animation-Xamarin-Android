using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Network.Classes.DataNet;
using Network.Classes.NetStructure;
using System.Timers;

namespace Network.Classes.Activity.Live
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class LiveActivity : AppCompatActivity
    {
        private FrameLayout LiveLayout;
        private LiveView LiveView;

        private Timer Timer;

        private NetParts NetParts;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_live);

            Window.AddFlags(Android.Views.WindowManagerFlags.KeepScreenOn);

            LiveLayout = FindViewById<FrameLayout>(Resource.Id.LiveLayout);
            NetParts = new NetParts();
            LiveView = new LiveView(this);

            LiveLayout.AddView(LiveView);

            InitTouchListener();
            InitTimer();

            NetParts.CreateNewNet();
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
            LiveView.Parts = NetParts.ListParts;
            LiveView.Invalidate();

            NetParts.IterrateLiveCircle();
            NetParts.UpdateStateNet();
            
            if (NetParts.ListParts.Count == 0)
                LiveView.EndGame();
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
                LiveView.ResumeGame();
            };
        }
    }
}