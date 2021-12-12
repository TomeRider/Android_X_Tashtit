using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace HELPER
{
    public class Keyboard
    {
        //public static void HideKeyboardOnCreate(Android.App.Activity activity)
        //{
        //    activity.Window.SetSoftInputMode(SoftInput.StateHidden); // getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
        //}

        public static void HideKeyboard(Android.App.Activity activity, bool onCreate = false)
        {
            if (!onCreate)
            {
                // Check if no view has focus:
                View view = activity.CurrentFocus; // GetCurrentFocus();
                if (view != null)
                {
                    Android.Views.InputMethods.InputMethodManager imm = (Android.Views.InputMethods.InputMethodManager)activity.GetSystemService(Context.InputMethodService /*INPUT_METHOD_SERVICE*/);
                    imm.HideSoftInputFromWindow(view.WindowToken /*GetWindowToken()*/, 0);
                }
            }
            else
            {
                activity.Window.SetSoftInputMode(SoftInput.StateHidden); // getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
            }
        }

        //public static void HideKeyboard(Activity activity)
        //{
        //    //Find the currently focused view, so we can grab the correct window token from it.
        //    View view = activity.CurrentFocus;

        //    //If no view currently has focus, create a new one, just so we can grab a window token from it
        //    if (view == null)
        //    {
        //        view = new View(activity);
        //    }

        //    InputMethodManager imm = (InputMethodManager)activity.GetSystemService(Activity.InputMethodService);
        //    imm.HideSoftInputFromWindow(view.WindowToken, 0);
        //}

        public static void HideKeyboardFrom(Context context, View view)
        {
            InputMethodManager imm = (InputMethodManager)context.GetSystemService(Activity.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }
    }
}