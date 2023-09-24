using Android.Content;
using AndroidX.AppCompat.App;
using CrosshairHero.Services;
using Xamarin.Essentials;
using Android.Provider;
using CrosshairHero.Adapters;
using Android.Views;
using Google.Android.Material.BottomSheet;
using Android.Graphics;
using ColorPicker;
using Android.OS;

namespace CrosshairHero
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ImageButton show_main_cross;
        ImageButton changecrosscolor;
        ImageButton changecrosshair;
        ImageButton moveRight_wars;
        Color[] colors;

        private int[] imageArray = 
        { 
            Resource.Drawable.cross_1,
            Resource.Drawable.cross_2,
            Resource.Drawable.cross_3,
            Resource.Drawable.cross_4,
            Resource.Drawable.cross_5,
            Resource.Drawable.cross_6,
            Resource.Drawable.cross_7,
            Resource.Drawable.cross_8,
            Resource.Drawable.cross_9,
            Resource.Drawable.cross_10,
            Resource.Drawable.cross_11,
            Resource.Drawable.cross_12,
            Resource.Drawable.cross_13,
            Resource.Drawable.cross_14,
            Resource.Drawable.cross_15,
            Resource.Drawable.cross_16,
            Resource.Drawable.cross_17,
            Resource.Drawable.cross_18,
            Resource.Drawable.cross_19,
            Resource.Drawable.cross_20,
            Resource.Drawable.cross_21,
        }; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Views();
        }
        //private AppCloseReceiver appCloseReceiver;
        public void Views()
        {
            show_main_cross = (ImageButton)FindViewById(Resource.Id.show_main_cross);
            changecrosshair = (ImageButton)FindViewById(Resource.Id.changecrosshair);
            changecrosscolor = (ImageButton)FindViewById(Resource.Id.changecrosscolor);
            moveRight_wars = (ImageButton)FindViewById(Resource.Id.moveRight_wars);
            show_main_cross.Click += Show_main_cross_Click;
            changecrosshair.Click += Changecrosshair_Click;
            changecrosscolor.Click += Changecrosscolor_Click;
            moveRight_wars.Click += MoveRight_wars_Click;
            //appCloseReceiver = new AppCloseReceiver();
            //RegisterReceiver(appCloseReceiver, new IntentFilter("CloseApp"));
        }

        private void MoveRight_wars_Click(object sender, EventArgs e)
        {

        }

        private void Changecrosscolor_Click(object sender, EventArgs e)
        {
            ShowBottomSheetDialogColors();
        }

        private void ShowBottomSheetDialogColors()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.main_controls_colors, null);

            colors = new Color[]
            {
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Yellow,
                Color.Orange,
                Color.Purple,
                Color.Cyan,
                Color.Magenta,
                Color.Gray,
                Color.Crimson,
                Color.Violet
           };


            GridView colorGrid = view.FindViewById<GridView>(Resource.Id.colorGrid);
            ColorBoxAdapter adapter = new ColorBoxAdapter(this, colors);

            colorGrid.Adapter = adapter;
            adapter.OnColor += Adapter_OnColor;
            BottomSheetDialog bottomSheetDialog = new BottomSheetDialog(this);
            bottomSheetDialog.SetContentView(view);
            bottomSheetDialog.Show();
        }

        private void Adapter_OnColor(object sender, ColorBoxAdapter.OnColorEventArgs e)
        {
            var newColor = e.color;
            var serviceIntent = new Intent(this, typeof(MainFloatingWindow));
            serviceIntent.PutExtra("ColorImage", newColor);
            StartService(serviceIntent);
        }

        private void Changecrosshair_Click(object sender, EventArgs e)
        {
            ShowBottomSheetDialog();
        }

        private void ShowBottomSheetDialog()
        {
            // Inflate the bottom sheet layout
            View view = LayoutInflater.Inflate(Resource.Layout.bottom_sheet_layout, null);

            GridView gridView = view.FindViewById<GridView>(Resource.Id.gridView);
            ImageAdapter adapter = new ImageAdapter(this, imageArray);
            gridView.Adapter = adapter;
            adapter.OnDetails += Adapter_OnDetails;
            BottomSheetDialog bottomSheetDialog = new BottomSheetDialog(this);
            bottomSheetDialog.SetContentView(view);
            // Show the bottom sheet dialog
            bottomSheetDialog.Show();
        }

        private void Adapter_OnDetails(object sender, ImageAdapter.OnDetailsEventArgs e)
        {
            var newImageResource = e.ImageLocation;
            var serviceIntent = new Intent(this, typeof(MainFloatingWindow));
            serviceIntent.PutExtra("ImageResource", newImageResource);
            StartService(serviceIntent);
        }

        private void Show_main_cross_Click(object sender, EventArgs e)
        {

            Intent serviceIntent = new Intent(this, typeof(NotificationService));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                StartForegroundService(serviceIntent);
            }
            else
            {
                StartService(serviceIntent);
            }

            if (!Settings.CanDrawOverlays(this))
            {

                StartActivityForResult(new Intent(Settings.ActionManageOverlayPermission, Android.Net.Uri.Parse("package:" + Platform.CurrentActivity.PackageName)), 0);
            }
            else
            {
                StartService(new Intent(this, typeof(MainFloatingWindow)));
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //private void CloseApp()
        //{
        //    //Finish();
        //    StopService(new Intent(this, typeof(NotificationService)));
        //    //System.Environment.Exit(0);
        //}
        //public class AppCloseReceiver : BroadcastReceiver
        //{
        //    public override void OnReceive(Context context, Intent intent)
        //    {
        //        if (intent.Action == "CloseApp")
        //        {
        //            ((MainActivity)context).CloseApp();
        //        }
        //    }
        //}
    }
}