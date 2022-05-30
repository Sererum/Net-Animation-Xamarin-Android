using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Network.Classes.DataNet;
using System;
using System.Linq;

namespace Network.Classes.Activity.Settings
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class SettingsActivity : AppCompatActivity
    {
        private TextView TextCount;
        private TextView TextLength;
        private TextView TextSize;
        private TextView TextSpeed;

        private SeekBar SeekBarQuantity;
        private SeekBar SeekBarLength;
        private SeekBar SeekBarSize;
        private SeekBar SeekBarSpeed;

        private Spinner ListColorPoint;
        private Spinner ListColorBackground;

        private ColorArrayAdapter ColorAdapter;

        private SettingsView SettingsView;
        private FrameLayout SettingsLayout;

        private int[] _arrayIndexColors;
        private string[] _arrayNameColors;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);

            SettingsLayout = FindViewById<FrameLayout>(Resource.Id.SettingsLayout);
            SettingsView = new SettingsView(this);
            SettingsLayout.AddView(SettingsView);

            TextCount = FindViewById<TextView>(Resource.Id.TextCount);
            TextLength = FindViewById<TextView>(Resource.Id.TextLength);
            TextSize = FindViewById<TextView>(Resource.Id.TextSize);
            TextSpeed = FindViewById<TextView>(Resource.Id.TextSpeed);

            SeekBarQuantity = FindViewById<SeekBar>(Resource.Id.SeekBarCount);
            SeekBarLength = FindViewById<SeekBar>(Resource.Id.SeekBarLength);
            SeekBarSize = FindViewById<SeekBar>(Resource.Id.SeekBarSize);
            SeekBarSpeed = FindViewById<SeekBar>(Resource.Id.SeekBarSpeed);

            ListColorPoint = FindViewById<Spinner>(Resource.Id.ListColorPoint);
            ListColorBackground = FindViewById<Spinner>(Resource.Id.ListColorBackground);

            InitSeekBar(seekBar: SeekBarLength, textView: TextLength,
                max: Data.MaxLengthConnect, step: 100,
                text: Resources.GetString(Resource.String.length_line_connect), property: "LengthConnect");

            InitSeekBar(seekBar: SeekBarQuantity, textView: TextCount,
                max: 300 / (SeekBarLength.Progress + 1), step: 1,
                text: Resources.GetString(Resource.String.count_points), property: "QuantityParts");

            InitSeekBar(seekBar: SeekBarSize, textView: TextSize,
                max: Data.MaxSizeParts, step: 1,
                text: Resources.GetString(Resource.String.size_point), property: "SizeParts");

            InitSeekBar(seekBar: SeekBarSpeed, textView: TextSpeed,
                max: Data.MaxSpeedParts, step: 1,
                text: Resources.GetString(Resource.String.speed_point), property: "SpeedParts");

            _arrayIndexColors = Data.IdToColors.Keys.ToArray<int>();
            _arrayNameColors = new string[_arrayIndexColors.Length];

            for (int i = 0; i < _arrayIndexColors.Length; i++)
                _arrayNameColors[i] = Resources.GetString(_arrayIndexColors[i]);

            ColorAdapter = new ColorArrayAdapter(this, _arrayIndexColors, _arrayNameColors);

            InitSpinner(spinner: ListColorPoint, anotherSpinner: ListColorBackground,
                property: "IdPartColor", anotherProperty: "IdBackgroundColor");
            InitSpinner(spinner: ListColorBackground, anotherSpinner: ListColorPoint,
                property: "IdBackgroundColor", anotherProperty: "IdPartColor");
        }

        private void InitSeekBar (SeekBar seekBar, TextView textView, int max, int step, string text, string property)
        {
            seekBar.Max = max / step - 1;
            seekBar.Min = 0;
            seekBar.Progress = (int) typeof(NetState).GetProperty(property).GetValue(typeof(NetState)) / step - 1;
            textView.Text = text + " " + (seekBar.Progress + 1);
            seekBar.ProgressChanged += (o, e) =>
            {
                if (seekBar == SeekBarLength)
                    SeekBarQuantity.Max = 300 / (seekBar.Progress + 1) - 1;
                textView.Text = text + " " + (seekBar.Progress + 1);
                typeof(NetState).GetProperty(property).SetValue(typeof(NetState), (seekBar.Progress + 1) * step);
                SettingsView.Invalidate();
            };
        }

        private void InitSpinner (Spinner spinner, Spinner anotherSpinner, string property, string anotherProperty)
        {
            spinner.Adapter = ColorAdapter;
            spinner.SetSelection(GetIndexOfColor(property));

            spinner.ItemSelected += delegate
            {
                int selectedItem = (int) spinner.SelectedItem;

                if (selectedItem == (int) typeof(NetState).GetProperty(anotherProperty).GetValue(typeof(NetState), null))
                {
                    anotherSpinner.SetSelection(GetIndexOfColor(property));
                    typeof(NetState).GetProperty(anotherProperty).SetValue(typeof(NetState), (int) anotherSpinner.SelectedItem);
                }

                typeof(NetState).GetProperty(property).SetValue(typeof(NetState), selectedItem);
                SettingsView.Invalidate();
            };
        }

        private int GetIndexOfColor (string property)
        {
            int idColorInSettings = (int) typeof(NetState).GetProperty(property).GetValue(typeof(NetState), null);
            return Array.IndexOf(Data.IdToColors.Keys.ToArray(), idColorInSettings);
        }

        protected override void OnPause ()
        {
            base.OnPause();
            Server.SaveData();
        }
    }
}