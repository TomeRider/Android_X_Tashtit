using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

using MODEL;
using HELPER;
using Android_X_Tashtit.SERVICES;


namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity : AppCompatActivity /*, IInitialization*/
    {
        protected abstract void InitializeViews();

        public static ISharedPreferences       sharedPreferences;
        public static ISharedPreferencesEditor editor;

        public static string PACKAGE_NAME;


        //public static Customer CurrentCustomer;

        private static TextView txtCartItemCount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            sharedPreferences = GetSharedPreferences("login", FileCreationMode.Private);
            editor            = sharedPreferences.Edit();

            PACKAGE_NAME = ApplicationInfo.PackageName;
        }

        private void ShowMenu(IMenu menu)
        {
            ShowHideMenu(menu, true);
        }

        private void HideMenu(IMenu menu)
        {
            ShowHideMenu(menu, false);
        }

        private void ShowHideMenu (IMenu menu, bool show)
        {
            int[] menuItems = new int[] { 
                                          Resource.Id.mnuCities
                                        };

            foreach (int menuItem in menuItems)
                menu.FindItem(menuItem).SetVisible((show) ? true : false);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            //IMenuItem menuItem = menu.FindItem(Resource.Id.mnuShopingCart);
            //SetupBadge(orderToys.ItemsOnCart);

            /*
            if (MainActivity.CurrentCustomer == null || !MainActivity.CurrentCustomer.IsManager)
            {
                HideMenu(menu);
            }
            else
            {
                if (MainActivity.CurrentCustomer != null && MainActivity.CurrentCustomer.IsManager)
                {
                    ShowMenu(menu);
                }
            }
            */

            /*
            if (CurrentCustomer == null)
                menu.FindItem(Resource.Id.mnuLogout).SetVisible(false);
            else
                menu.FindItem(Resource.Id.mnuLogout).SetVisible(true);
            */

            return base.OnCreateOptionsMenu(menu);
        }

        public static void SetupBadge(int itemCount)
        {
            if (txtCartItemCount != null)
            {
                if (itemCount == 0)
                {
                    if (txtCartItemCount.Visibility != ViewStates.Gone)
                    {
                        txtCartItemCount.Visibility = ViewStates.Gone;
                    }
                }
                else
                {
                    txtCartItemCount.Text = itemCount.ToString();
                    if (txtCartItemCount.Visibility != ViewStates.Visible)
                    {
                        txtCartItemCount.Visibility = ViewStates.Visible;
                    }
                }
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mnuCities:
                    {
                        StartActivity(new Intent(this, typeof(ACTIVITIES.CitiesActivity)));
                        break;
                    }

                case Resource.Id.mnuLogout:
                    {
                        Logout();
                        break;
                    }

                case Resource.Id.mnuMusicPlay:
                    {
                        Intent intent = new Intent(this, typeof(MediaService));
                        StartService(intent);
                        break;
                    }

                case Resource.Id.mnuMusicStop:
                    {
                        Intent intent = new Intent(this, typeof(MediaService));
                        StopService(intent);
                        break;
                    }

                case Resource.Id.mnuExit:
                    {
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                        break;
                    }
            }

            return base.OnOptionsItemSelected(item);
        }

        private void Logout()
        {
            Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);

            alertDiag.SetTitle("Logout");
            alertDiag.SetMessage("Logout and Exit (optional)");

            alertDiag.SetCancelable(true);

            alertDiag.SetPositiveButton("Logout", (senderAlert, args)
            => {
                editor.PutString("EMAIL", "");
                editor.PutString("PASSWORD", "");
                editor.Commit();

                //CurrentCustomer = null;

                InvalidateOptionsMenu();
            });

            alertDiag.SetNeutralButton("Lgout & Exit", (senderAlert, args)
            => {
                editor.PutString("EMAIL", "");
                editor.PutString("PASSWORD", "");
                editor.Commit();

                //CurrentCustomer = null;

                InvalidateOptionsMenu();

                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            });

            alertDiag.SetNegativeButton("Cancel", (senderAlert, args)
            => {
                alertDiag.Dispose();
            });

            Dialog diag = alertDiag.Create();
            diag.Show();
        }
    }
}