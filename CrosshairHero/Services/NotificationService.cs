using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Xamarin.Essentials;

namespace CrosshairHero.Services
{
    [Service]
    public class NotificationService : Service 
    {
        private const int NOTIFICATION_ID = 1000; 
        //private bool isServiceRunning = false;
        RemoteViews customLayout;


        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            CreateNotificationChannel();
            Notification notification = CreateNotification();
            StartForeground(NOTIFICATION_ID, notification);
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        // Notification channel creation
        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel("CrosshairHeroChannel", "Crosshairo hero Channel", NotificationImportance.Default);
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        // Notification creation
        private Notification CreateNotification()
        {

            customLayout = new RemoteViews(this.PackageName, Resource.Layout.main_notification);


            //open contols
            Intent OpenControls = new Intent(this, typeof(MainFloatingWindow));
            OpenControls.SetAction("OpenControls");
            PendingIntent pendingIntent = PendingIntent.GetService(Application.Context, 0, OpenControls, PendingIntentFlags.Immutable);
            customLayout.SetOnClickPendingIntent(Resource.Id.Open_control_notification, pendingIntent);


            //Open mainActivity
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            var pendingIntentOpenMainActivity = PendingIntent.GetActivity(Application.Context, 10,intent, PendingIntentFlags.Immutable);
            customLayout.SetOnClickPendingIntent(Resource.Id.Open_MainActivity, pendingIntentOpenMainActivity);



            var serviceIntent = new Intent(this, typeof(MainFloatingWindow));
            serviceIntent.PutExtra("CloseMainFloatingWindow", "CloseApp");
            var pendingIntentCloseMainActivity = PendingIntent.GetService(Application.Context, 100, serviceIntent, PendingIntentFlags.Immutable);
            customLayout.SetOnClickPendingIntent(Resource.Id.close_notification, pendingIntentCloseMainActivity);


            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "CrosshairHeroChannel")
                .SetCustomContentView(customLayout)
                .SetSmallIcon(Resource.Drawable.crosshair_long);

            return builder.Build();
        }
    }
}
