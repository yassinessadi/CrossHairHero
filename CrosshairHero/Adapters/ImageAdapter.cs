using Android.Content;
using Android.Runtime;
using Android.Views;
using CrosshairHero.Services;

namespace CrosshairHero.Adapters
{
    public class ImageAdapter : BaseAdapter
    {
        private readonly Context context;
        private readonly int[] imageArray;

        public event EventHandler<OnDetailsEventArgs> OnDetails;

        public class OnDetailsEventArgs : EventArgs
        {
            public int ImageLocation { get; set; }
        }
        public ImageAdapter(Context context, int[] imageArray)
        {
            this.context = context;
            this.imageArray = imageArray;
        }

        public override int Count => imageArray.Length;

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
            ImageView imageView;

            if (convertView == null)
            {
                imageView = new ImageView(context);
                imageView.LayoutParameters = new GridView.LayoutParams(100, 100); // Adjust the size as needed
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imageView.SetPadding(3,3, 3, 3);
            }
            else
            {
                imageView = (ImageView)convertView;
            }

            imageView.SetImageResource(imageArray[position]);

            imageView.Click += (sender, args) =>
            {
                // Raise the ImageClicked event with the image location
                OnDetails?.Invoke(this, new OnDetailsEventArgs { ImageLocation = imageArray[position] });
            };

            return imageView;
        }
    }
}
