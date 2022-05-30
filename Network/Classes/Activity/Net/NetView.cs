using Android.Content;
using Android.Graphics;
using Android.Views;
using Network.Classes.NetStructure;
using System.Collections.Generic;

namespace Network.Classes.Activity.Net
{
    class NetView : View
    {
        private ShowNet ShowNet;
        private List<Part> _parts;

        public NetView (Context context) : base(context)
        {
            ShowNet = new ShowNet();
        }

        public List<Part> Parts
        {
            set {  _parts = value; }
        }

        protected override void OnDraw (Canvas canvas)
        {
            base.OnDraw(canvas);
            ShowNet.DrawNet(canvas, _parts);
        }
    }
}