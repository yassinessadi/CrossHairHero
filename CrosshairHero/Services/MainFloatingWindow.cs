using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xamarin.Essentials;

namespace CrosshairHero.Services
{
    [Service]
    public class MainFloatingWindow : Service, View.IOnTouchListener
    {

        WindowManagerLayoutParams layoutParams;
        WindowManagerLayoutParams layoutParams1;
        IWindowManager windowManager;
        View main_cross_icon;
        View main_controls;

        ImageButton reset;
        ImageButton Close_Controls;
        ImageButton moveLeft;
        ImageButton moveDown;
        ImageButton moveRight;
        ImageButton moveTop;
        TextView minusMove;
        TextView ValueOfMove;
        TextView AddMove;
        SeekBar SizeControl;
        ImageView layout;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnCreate()
        {
            base.OnCreate();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent != null && intent.Extras != null)
            {
                if (intent.Extras.ContainsKey("ImageResource"))
                {
                    int newImageResource = intent.Extras.GetInt("ImageResource");
                    ChangeImage(newImageResource);
                }
                if (intent.Extras.ContainsKey("ColorImage"))
                {
                    int newImageResource = intent.Extras.GetInt("ColorImage");
                    ChangeColor(newImageResource);
                }
                if (intent.Extras.ContainsKey("CloseMainFloatingWindow"))
                {
                    if (main_controls != null)
                    {
                        windowManager.RemoveView(main_controls);
                    }
                    windowManager.RemoveView(main_cross_icon);
                    StopSelf();
                    StopService(new Intent(this, typeof(NotificationService)));
                    System.Environment.Exit(0);
                }
            }
            else
            {
                if (main_cross_icon == null)
                {
                    MainWindow();
                }
                if (main_controls == null)
                {
                    MainControls();
                }
            }
            return StartCommandResult.NotSticky;
        }

        private void ChangeColor(int newImageResource)
        {
            if (layout == null)
                return;
            layout.SetColorFilter(new Color(newImageResource), PorterDuff.Mode.SrcIn);
        }

        private void ChangeImage(int newImageResource)
        {
            if (layout == null)
                return;
            layout.SetImageResource(newImageResource);
            
        }

        public void MainControls()
        {
            windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
            LayoutInflater mLayoutInflater = LayoutInflater.From(ApplicationContext);
            main_controls = mLayoutInflater.Inflate(Resource.Layout.main_controls, null);
            main_controls.SetOnTouchListener(this);
            //controls
            reset = (ImageButton)main_controls.FindViewById(Resource.Id.reset);
            Close_Controls = (ImageButton)main_controls.FindViewById(Resource.Id.Close_Controls);
            moveLeft = (ImageButton)main_controls.FindViewById(Resource.Id.moveLeft);
            moveDown = (ImageButton)main_controls.FindViewById(Resource.Id.moveDown);
            moveRight = (ImageButton)main_controls.FindViewById(Resource.Id.moveRight);
            moveTop = (ImageButton)main_controls.FindViewById(Resource.Id.moveTop);
            minusMove = (TextView)main_controls.FindViewById(Resource.Id.minusMove);
            ValueOfMove = (TextView)main_controls.FindViewById(Resource.Id.ValueOfMove);
            AddMove = (TextView)main_controls.FindViewById(Resource.Id.AddMove);
            SizeControl = (SeekBar)main_controls.FindViewById(Resource.Id.SizeControl);

            //main icon
            layout = (ImageView)main_cross_icon.FindViewById(Resource.Id.layout);



            reset.Click += Reset_Click;
            Close_Controls.Click += Close_Controls_Click;
            moveLeft.Click += MoveLeft_Click;
            moveDown.Click += MoveDown_Click;
            moveRight.Click += MoveRight_Click;
            moveTop.Click += MoveTop_Click;
            minusMove.Click += MinusMove_Click;
            AddMove.Click += AddMove_Click;
            SizeControl.ProgressChanged += SizeControl_ProgressChanged;
            layoutParams1 = new WindowManagerLayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent,
                WindowManagerTypes.Phone,
                WindowManagerFlags.NotFocusable,
                Format.Translucent)
            {
                Gravity = GravityFlags.Bottom | GravityFlags.End,
            };

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                layoutParams1.Type = WindowManagerTypes.ApplicationOverlay;
            }
            else
            {
                layoutParams1.Type = WindowManagerTypes.Phone;
            }
 
            layoutParams1.Width = WindowManagerLayoutParams.WrapContent;
            layoutParams1.Height = WindowManagerLayoutParams.WrapContent;
            //layoutParams1.X = 130;
            //layoutParams1.Y = 250;

            windowManager.AddView(main_controls, layoutParams1);

        }


        private void MainWindow()
        {

            windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();

            LayoutInflater mLayoutInflater = LayoutInflater.From(ApplicationContext);
            main_cross_icon = mLayoutInflater.Inflate(Resource.Layout.main_cross_icon, null);

            layoutParams = new WindowManagerLayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent,
                WindowManagerTypes.Phone,
                WindowManagerFlags.NotFocusable,
                Format.Translucent);


            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                layoutParams.Type = WindowManagerTypes.ApplicationOverlay;
            }
            else
            {
                layoutParams.Type = WindowManagerTypes.Phone;
            }

            
            layoutParams.Width = 100;
            layoutParams.Height = 100;
            layoutParams.Flags = WindowManagerFlags.LayoutInScreen;
            layoutParams.Flags = WindowManagerFlags.NotFocusable;

            layoutParams.X = 0;
            layoutParams.Y = 0;



            windowManager.AddView(main_cross_icon, layoutParams);
        }

        private void Close_Controls_Click(object sender, EventArgs e)
        {
            if (main_controls != null)
            {
                windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                windowManager.RemoveView(main_controls);
                main_controls = null;
            }
        }

        private void SizeControl_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {

                layoutParams.Width = e.Progress;
                layoutParams.Height = e.Progress;

                windowManager.UpdateViewLayout(main_cross_icon, layoutParams);
            });
        }

        private void AddMove_Click(object sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (int.Parse(ValueOfMove.Text) > 1)
                {
                    minusMove.Enabled = true;
                }
                ValueOfMove.Text = (int.Parse(ValueOfMove.Text) + 1).ToString();
            });
        }

        private void MinusMove_Click(object sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (int.Parse(ValueOfMove.Text) == 1)
                {
                    minusMove.Enabled = false;
                    return;
                }
                ValueOfMove.Text = (int.Parse(ValueOfMove.Text) - 1).ToString();
            });
        }

        private void MoveTop_Click(object sender, EventArgs e)
        {
            //layoutParams.X = 0;
            layoutParams.Y -= int.Parse(ValueOfMove.Text);

            windowManager.UpdateViewLayout(main_cross_icon, layoutParams);
        }

        private void MoveRight_Click(object sender, EventArgs e)
        {
            layoutParams.X += int.Parse(ValueOfMove.Text);
            //layoutParams.Y = 0;

            windowManager.UpdateViewLayout(main_cross_icon, layoutParams);
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            layoutParams.Y += int.Parse(ValueOfMove.Text);

            windowManager.UpdateViewLayout(main_cross_icon, layoutParams);
        }

        private void MoveLeft_Click(object sender, EventArgs e)
        {
            layoutParams.X -= int.Parse(ValueOfMove.Text);
            //layoutParams.Y = 0;

            windowManager.UpdateViewLayout(main_cross_icon, layoutParams);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            layoutParams.X = 0;
            layoutParams.Y = 0;

            layoutParams.Width = 100;
            layoutParams.Height = 100;

            windowManager.UpdateViewLayout(main_cross_icon, layoutParams);
        }

        private int x;
        private int y;
        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {

                case MotionEventActions.Down:
                    x = (int)e.RawX;
                    y = (int)e.RawY;
                    break;

                case MotionEventActions.Move:
                    int nowX = (int)e.RawX;
                    int nowY = (int)e.RawY;
                    int movedX = nowX - x;
                    int movedY = nowY - y;
                    x = nowX;
                    y = nowY;
                    layoutParams1.X = layoutParams1.X - movedX;
                    layoutParams1.Y = layoutParams1.Y - movedY;


                    windowManager.UpdateViewLayout(main_controls, layoutParams1);
                    break;

                default:
                    break;
            }
            return false;
        }
    }
}
