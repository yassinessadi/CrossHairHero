using Android.Content;
using Android.Graphics;
using Android.Views;
using CrosshairHero;
using CrosshairHero.Services;
using static CrosshairHero.Adapters.ImageAdapter;

namespace ColorPicker
{
    public class ColorBoxAdapter : BaseAdapter
    {
        private readonly Context context;
        private readonly Color[] colors;

        public event EventHandler<OnColorEventArgs> OnColor;

        public class OnColorEventArgs : EventArgs
        {
            public int color { get; set; }
        }
        public ColorBoxAdapter(Context context, Color[] colors)
        {
            this.context = context;
            this.colors = colors;
        }

        public override int Count => colors.Length;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.FromContext(context).Inflate(Resource.Layout.color_box_item, null);
            ImageButton colorBox = view.FindViewById<ImageButton>(Resource.Id.colorBox);

            colorBox.SetBackgroundColor(colors[position]);

            colorBox.Click += (sender, args) =>
            {
                // Handle color selection
                OnColor?.Invoke(this, new OnColorEventArgs { color = colors[position] });
                //colorPickerButton.SetBackgroundColor(colors[position]);
            };

            return view;
        }
    }
}
