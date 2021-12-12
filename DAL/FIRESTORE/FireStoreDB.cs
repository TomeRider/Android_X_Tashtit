using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Firebase;
using Plugin.CloudFirestore;
using Xamarin.Essentials;

namespace DAL.FIRESTORE
{
    public class FireStoreDB
    {
        private static FireStoreDB instance = null;
        public         FirebaseApp app;
        private static IFirestore  connection = null;

        private static readonly object padlock = new object();

        private FireStoreDB()
        {
            // app = FirebaseApp.InitializeApp((Context)AppInfo.PackageName);

            FirebaseOptions options = new FirebaseOptions.Builder()
                .SetProjectId("")
                .SetApplicationId("")
                .SetApiKey("")
                .SetStorageBucket("")
                .Build();

            app = FirebaseApp.InitializeApp(Application.Context, options);

            connection = CrossCloudFirestore.Current.Instance;
        }

        public static IFirestore Connection
        {
            get
            {
                if (connection == null)
                {
                    lock (padlock)
                    {
                        if (connection == null)
                        {
                            instance = new FireStoreDB();
                        }
                    }
                }

                return connection;
            }
        }

        public static FireStoreDB Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new FireStoreDB();
                        }
                    }
                }
                return instance;
            }
        }
    }
}