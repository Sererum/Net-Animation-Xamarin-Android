using Android.Graphics;
using Android.Views;
using Android.Widget;
using Network.Classes.DataNet;

namespace Network.Classes.Activity.Settings
{
    class ColorArrayAdapter : BaseAdapter<int>
    {
        private int[] _arrayIndexColors;
        private string[] _arrayNameColors;
        private Color[] _arrayColors;

        Android.App.Activity _context;

        public ColorArrayAdapter (Android.App.Activity context, int[] arrayIndexColors, string[] arrayNameColors) : base()
        {
            _arrayColors = new Color[arrayIndexColors.Length];

            for (int i = 0; i < arrayIndexColors.Length; i++)
                _arrayColors[i] = Data.IdToColors[arrayIndexColors[i]];

            _arrayIndexColors = arrayIndexColors;
            _arrayNameColors = arrayNameColors;
            _context = context;
        }

        public override int this[int position]
        {
            get { return _arrayIndexColors[position]; }
        }

        public override int Count
        {
            get { return _arrayIndexColors.Length; }
        }

        public override long GetItemId (int position)
        {
            return position;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_color, null);

            view.FindViewById<TextView>(Resource.Id.TextColor).Text = _arrayNameColors[position];
            view.FindViewById<TextView>(Resource.Id.FieldForColor).SetBackgroundColor(_arrayColors[position]);

            return view;
        }
    }
}