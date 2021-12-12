using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Android_X_Tashtit.NOTIFICATIONS
{
    public class NotificationHelper : ContextWrapper
    {
        // This is the Notification Channel ID. More about this in the next section
        private const string CHANNEL_ID = "channel_id";

        //User visible Channel Name
        private string CHANNEL_NAME = "Notification Channel";

        int SmallIcon = Android.Resource.Drawable.StatNotifyChat;

        private NotificationManager manager;

        public NotificationManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = (NotificationManager)GetSystemService(NotificationService);
                }
                return manager;
            }
        }

        public NotificationHelper(Context context) : base(context)
        {
            //Notification channel should only be created for devices running Android 26
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default);

                //Boolean value to set if lights are enabled for Notifications from this Channel
                notificationChannel.EnableLights(true);

                //Boolean value to set if vibration are enabled for Notifications from this Channel
                notificationChannel.EnableVibration(true);

                //Sets the color of Notification Light
                notificationChannel.LightColor = Color.White;

                //Set the vibration pattern for notifications. Pattern is in milliseconds with the format {delay,play,sleep,play,sleep...}
                notificationChannel.SetVibrationPattern(new long[] {
                    500,
                    500,
                    500,
                    500,
                    500
                });

                //Sets whether notifications from these Channel should be visible on Lockscreen or not
                notificationChannel.LockscreenVisibility = NotificationVisibility.Public;

                Manager.CreateNotificationChannel(notificationChannel);
            }
        }

        public NotificationCompat.Builder GetNotification(string title, string body)
        {
            return new NotificationCompat.Builder(ApplicationContext, CHANNEL_ID)
                      .SetContentTitle(title)
                      .SetStyle(new NotificationCompat.BigTextStyle().BigText(body))
                      /*.SetContentText(body)*/
                      .SetSmallIcon(SmallIcon)
                      .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.lupa))
                      .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                      .SetAutoCancel(true);
        }

        public void PrepareNotification(int id, NotificationCompat.Builder notification)
        {
            Manager.Notify(id, notification.Build());
        }

        public static void Notify(Context context, string title, string message, int notificationId = 1)
        {
            NotificationHelper nh = new NotificationHelper(context);
            NotificationCompat.Builder builder = nh.GetNotification(title, message);
            if (builder != null)
                nh.PrepareNotification(notificationId, builder);
        }
    }
}