using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HELPER
{
    public class Global
    {
        public static void ToastCenteredText(Context context, string message, ToastLength toastLength)
        {
            Toast toast = Toast.MakeText(context, message, toastLength);
            LinearLayout layout = (LinearLayout)toast.View;

            if (layout.ChildCount > 0)
            {
                TextView tv = (TextView)layout.GetChildAt(0);
                tv.Gravity = GravityFlags.Center;
            }
            toast.Show();
        }

        public static void ToastCenteredText(Context context, int message, ToastLength toastLength)
        {
            ToastCenteredText(context, context.Resources.GetString(message), toastLength);
        }

        public static void ExitApp(Context context, bool isHebrew = true, bool askPermision = true)
        {
            if (askPermision)
            {
                // set dialog message
                AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(context);

                alertDialogBuilder.SetTitle((isHebrew) ? "יציאה" : "Exit");
                alertDialogBuilder.SetMessage((isHebrew) ? "האם ברצונך לצאת ?" : "Do yo really want to exit ?");
                alertDialogBuilder.SetCancelable(false);

                alertDialogBuilder.SetPositiveButton(((isHebrew) ? "כן" : "YES"), (senderAlert, args)
                    =>
                {
                    QuitApp(context);
                });

                alertDialogBuilder.SetNeutralButton(((isHebrew) ? "ביטול" : "CANCEL"), (senderAlert, args)
                    =>
                {
                    alertDialogBuilder.Dispose();
                });

                alertDialogBuilder.SetNegativeButton(((isHebrew) ? "לא" : "NO"), (senderAlert, args)
                   =>
                {
                    alertDialogBuilder.Dispose();
                });

                Dialog alertDialog = alertDialogBuilder.Create();
                alertDialog.Show();
            }
            else
            {
                QuitApp(context);
            }
        }

        private static void QuitApp(Context context)
        {
            // what to do if YES is tapped
            ///////FinishAffinity();
            ///////System.Environment.Exit(0);

            //1.If you want to quit application in an Activity use this code snippet:

            if ((int)Build.VERSION.SdkInt >= 16 && (int)Build.VERSION.SdkInt < 21)
            {
                ((Activity)context).FinishAffinity();
            }
            else if ((int)Build.VERSION.SdkInt >= 21)
            {
                ((Activity)context).FinishAndRemoveTask();
            }

            System.Environment.Exit(0);

            //************************************************************************

            //2.If you want to quit the application in a non Activity class which has access to Activity then use this code snippet:

            //if(Build.VERSION.SDK_INT>=16 && Build.VERSION.SDK_INT<21){
            //    getActivity().finishAffinity();
            // } else if(Build.VERSION.SDK_INT>=21){
            //    getActivity().finishAndRemoveTask();
            // }

            //************************************************************************
        }


        private static readonly int REQUEST_CAMERA = 0;
        private static readonly int SELECT_FILE = 1;

        public static void TakePicture(Context context, bool isHebrew = true)
        {
            string[] items = { "Take Photo", "Choose from Library", "Cancel" };
            string[] itemsHebrew = { "צילום תמונה", "בחירה מגלריה", "ביטול" };

            using (var dialogBuilder = new AlertDialog.Builder(context))
            {
                dialogBuilder.SetTitle((isHebrew) ? "תמונה" : "Photo");
                dialogBuilder.SetItems(((isHebrew) ? itemsHebrew : items), (d, args) => {
                    //Take photo
                    if (args.Which == 0)
                    {
                        Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
                        ((Activity)context).StartActivityForResult(intent, REQUEST_CAMERA);
                    }
                    //Choose from gallery
                    else if (args.Which == 1)
                    {
                        Intent imageIntent = new Intent();
                        imageIntent.SetType("image/*");
                        imageIntent.SetAction(Intent.ActionGetContent);
                        ((Activity)context).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), SELECT_FILE);
                    }
                });

                dialogBuilder.Show();
            }
        }

        /*
        public static void GetDate(Context context, Action<DatePickerDialog.DateSetEventArgs> callback, int year, int month, int day)
        {
            DatePickerDialog datePicker = new DatePickerDialog(context, //Context 
                                                                callback?.Invoke((DatePickerDialog.DateSetEventArgs)datePicker), // ***
                                                                year,  // שנה
                                                                month - 1, //חודש
                                                                day);  // יום
        }
        */

        /// <summary>
        /// Get all views of Activity
        /// </summary>
        /// <param name="v"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        /// USAGE: List<View> l = Global.GetChildren(Window.DecorView, new List<View>(), new List<string> { "EditText", "ImageView" });
        public static List<View> GetChildren(View v, List<View> allViews, List<string> filter = null)
        {
            ViewGroup viewgroup = (ViewGroup)v;
            for (int i = 0; i < viewgroup.ChildCount; i++)
            {
                View v1 = viewgroup.GetChildAt(i);
                if (v1 is ViewGroup) GetChildren(v1, allViews, filter);
                if (filter == null)
                    //if (v1 is Android.Views.View)
                    allViews.Add(v1);
                else
                    if (FindInList(filter, v1.GetType().ToString()))
                    allViews.Add(v1);
            }

            return allViews;
        }

        private static bool FindInList(List<string> filter, string type)
        {
            bool found = false;

            foreach (string s in filter)
                if (type.ToUpper().IndexOf(s.ToUpper()) > 0)
                    found = true;

            return found;
        }

        /// <summary>
        /// Displays an alert dialog with a positive and a negative button.
        /// </summary>
        /// <param name="callback">This is called when the user picks an option. True if positive, false if negative.</param>
        public static void YesNoAlertDialog(Context context,
                                            string title,
                                            string message,
                                            string positive,
                                            string negative,
                                            Action<bool> callback,
                                            bool cancelable = false)
        {
            using (var alertDiag = new AlertDialog.Builder(context))
            {
                alertDiag.SetTitle(title)
                    .SetMessage(message)
                    .SetCancelable(cancelable)
                    .SetPositiveButton(positive, (s, e) => callback?.Invoke(true))
                    .SetNegativeButton(negative, (s, e) => callback?.Invoke(false));

                alertDiag.Create().Show();
            }
        }

        public static ProgressDialog SetProgress(Context context, 
                                                 bool    indeterminate     = true, 
                                                 ProgressDialogStyle style = ProgressDialogStyle.Spinner, 
                                                 string message            = "Loading Data...", 
                                                 bool   cancalable         = false)
        {
            ProgressDialog progress = new ProgressDialog(context);
            progress.Indeterminate  = indeterminate;
            progress.SetProgressStyle(style);
            progress.SetMessage(message);
            progress.SetCancelable(cancalable);
            progress.Show();
            return progress;
        }
        /// <summary>
        /// Get the table name involved in the sql query
        /// </summary>
        /// <param name="sql">SQL statemenr</param>
        /// <returns>Table Name</returns>
        public static string ParseTableNameFromSQL(string sql)
        {
            string[] words;
            string tableName = "";
            int index;

            words = sql.ToUpper().Split();

            if (words[0].Equals("SELECT"))
            {
                // find FROM
                index = Array.FindIndex<string>(words, s => s.ToUpper().Equals("FROM"));

                // skip all spaces
                index = Array.FindIndex<string>(words, index + 1, s => !s.Equals(""));

                tableName = words[index];
            }
            else
                if (words[0].Equals("DELETE") || words[0].Equals("INSERT"))
                tableName = words[2];
            else
                tableName = words[1];   // UPDATE

            return tableName;
        }

        /// <summary>
        /// בדיקה האם תאריך
        /// </summary>
        /// <param name="date">מחרוזת התאריך לבדיקה</param>
        /// <returns>נכון/לא נכון</returns>
        public static bool IsDate(object date)
        {
            DateTime tryDate;
            if (DateTime.TryParse(date.ToString(), out tryDate))
                return true;
            else
                return false;
        }

        /// <summary>
        /// בדיקה אם שעה
        /// </summary>
        /// <param name="time">מחרוזת השעה לבדיקה</param>
        /// <returns>נכון/לא נכון</returns>
        public static bool IsTime(object time)
        {
            TimeSpan ts;
            string t;

            if (IsDate(time))
                t = time.ToString().Substring(11);  // יש לחלץ את השעה מהתאריך
            else
                t = time.ToString();                //          התקבלה שעה בלבד  

            if (TimeSpan.TryParse(t, out ts))
                return true;
            else
                return false;
        }

        /// <summary>
        /// מחשב גיל
        /// </summary>
        /// <param name="birthDate">תאריך לידה</param>
        /// <returns>גיל</returns>
        public static double Age(DateTime birthDate)
        {
            double age;

            if (!IsDate(birthDate.ToString()))
                age = -1;
            else
            {
                TimeSpan ts = DateTime.Today - birthDate;
                age = ts.Days / 365.25;
            }

            return age;
        }

        /// <summary>
        /// מחשב גיל
        /// </summary>
        /// <param name="birthDate">תאריך לידה</param>
        /// <param name="separator">מפריד בין שנים לחודשים</param>
        /// <returns>גיל בפורמט שנים וחודשים</returns>
        public static string AgeString(DateTime birthDate, char separator = '|')
        {
            double age = Age(birthDate);
            string[] ages = age.ToString().Split('.');
            if (ages.Length == 1)
            {
                ages = new string[2];
                ages[0] = age.ToString().Split('.')[0];
                ages[1] = "0";
            }

            if ((int)(Convert.ToDouble("0." + ages[1]) * 12) == 0)
                return ages[0];
            else
                return ages[0] + separator + ((int)(Convert.ToDouble("0." + ages[1]) * 12)).ToString();
        }



        /// <summary>
        /// בדיקה אם מספר שלם נמצא בטווח מספרים
        /// </summary>
        /// <param name="dateToCheck">המספר לבדיקה</param>
        /// <param name="startDate">מםפר התחלת הטווח</param>
        /// <param name="endDate">מספר סיום הטווח</param>
        /// <returns>בטווח/איננו בטווח</returns>
        public static bool IsNumberInRange(int numToCheck, int minValue, int maxValue)
        {
            return minValue <= numToCheck && numToCheck <= maxValue;
        }

        /// <summary>
        /// בדיקה אם מספר עשרוני נמצא בטווח מספרים
        /// </summary>
        /// <param name="dateToCheck">המספר לבדיקה</param>
        /// <param name="startDate">מםפר התחלת הטווח</param>
        /// <param name="endDate">מספר סיום הטווח</param>
        /// <returns>בטווח/איננו בטווח</returns>
        public static bool IsNumberInRange(double numToCheck, double minValue, double maxValue)
        {
            return minValue <= numToCheck && numToCheck <= maxValue;
        }

        /// <summary>
        /// בדיקה אם מספר דצימאלי נמצא בטווח מספרים
        /// </summary>
        /// <param name="dateToCheck">המספר לבדיקה</param>
        /// <param name="startDate">מםפר התחלת הטווח</param>
        /// <param name="endDate">מספר סיום הטווח</param>
        /// <returns>בטווח/איננו בטווח</returns>
        public static bool IsNumberInRange(decimal numToCheck, decimal minValue, decimal maxValue)
        {
            return minValue <= numToCheck && numToCheck <= maxValue;
        }

        /// <summary>
        /// בודקת ומתקנת כל מילה עברית שלא תתחיל באות סופית
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FixHebrewString(string s)
        {
            int i;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] oldWords = s.Split(delimiterChars);
            string[] newWords = s.Split(delimiterChars);

            for (i = 0; i < newWords.Length; i++)
                if (newWords[i].StartsWith("ך")) newWords[i] = "כ" + newWords[i].Substring(1);
                else if (newWords[i].StartsWith("ם")) newWords[i] = "מ" + newWords[i].Substring(1);
                else if (newWords[i].StartsWith("ן")) newWords[i] = "נ" + newWords[i].Substring(1);
                else if (newWords[i].StartsWith("ף")) newWords[i] = "פ" + newWords[i].Substring(1);
                else if (newWords[i].StartsWith("ץ")) newWords[i] = "צ" + newWords[i].Substring(1);

            for (i = 0; i < oldWords.Length; i++)
                s = s.Replace(oldWords[i], newWords[i]);

            return s;
        }

        /// <summary>
        /// Check if a string is URL
        /// </summary>
        /// <param name="url">String to check</param>
        /// <returns>TRUE if URL otherwise FASLE</returns>
        public static bool IsUrl(string url)
        {
            return System.Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Calculate Sum of digits of a given number according to Luhn Algorithm (MOD10)
        /// https://en.wikipedia.org/wiki/Luhn_algorithm
        /// </summary>
        /// <param name="number">The number to calculate it's sum of digits</param>
        /// <returns>Sum of digits</returns>
        private static int SumOfDigits(string number)
        {
            int len = number.Length;
            int multiplier = 1;
            int sumOfDigits = 0;
            int digitSum;

            for (int i = len - 1; i >= 0; i--)
            {
                if (number[i] >= '0' && number[i] <= '9')
                {
                    digitSum = multiplier * (number[i] - '0');
                    sumOfDigits += digitSum / 10 + digitSum % 10;
                    multiplier = 3 - multiplier;
                }
            }

            return sumOfDigits;
        }

        /// <summary>
        /// Check if number matches Luhn Algorithm (Mod10)
        /// https://en.wikipedia.org/wiki/Luhn_algorithm
        /// </summary>
        /// <param name="number">Tje number to check</param>
        /// <returns>TRUE if MOD10 otherwise FALSE</returns>
        public static bool IsLuhn(string number)
        {
            return SumOfDigits(number) % 10 == 0;
        }

        /// <summary>
        /// Check if number matches Luhn Algorithm (Mod10)
        /// https://en.wikipedia.org/wiki/Luhn_algorithm
        /// </summary>
        /// <param name="number">Tje number to check</param>
        /// <returns>TRUE if MOD10 otherwise FALSE</returns>
        public static bool IsMod10(string number)
        {
            return IsLuhn(number);
        }

        /// <summary>
        /// Calculate check digit according to Luhn Algorithm (MOD10)
        /// https://en.wikipedia.org/wiki/Luhn_algorithm
        /// </summary>
        /// <param name="number">The number to calculate check digit</param>
        /// <returns>Check Digit</returns>
        public static char CalculateCheckDigit(string number)
        {
            int sumOfDigits = SumOfDigits(number);
            string s = (sumOfDigits * 9).ToString();

            return Convert.ToChar(s.Substring(s.Length - 1, 1));
        }
    }
}
