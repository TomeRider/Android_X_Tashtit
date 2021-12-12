using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HELPER
{
    /// <summary>
    /// בדיקות תקינות קלט
    /// </summary>
    public class ValidateEntry : BaseValidateEntry
    {
        /// <summary>
        /// בדיקת שם עברי
        /// </summary>
        /// <param name="name">השם לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckHebrewName(string name, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (name == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                // האם מכיל את התווים החוקיים ותו ראשון שונה מאות סופית ולפחות 2 תווים
                if (!IsLegalChars(name, hebChars + hebEndChars + otherChars)    ||
                     IsLegalChars(name[0].ToString(), hebEndChars + otherChars) ||
                     name.Length < 2)
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת שם אנגלי
        /// </summary>
        /// <param name="name">השם לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckEnglishName(string name, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (name == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                // האם מכיל את התווים החוקיים ולפחות 2 תווים
                if (!IsLegalChars(name, m_LowerChars + m_UpperChars + otherChars) || name.Length < 2)
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// תבדיקת כתובת עברי
        /// </summary>
        /// <param name="name">השם לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckHebrewAddress(string name, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (name == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                // האם מכיל את התווים החוקיים ותו ראשון שונה מאות סופית
                if (!IsHebrewOrDigits(name) || IsLegalChars(name[0].ToString(), hebEndChars + otherChars))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מס' תעודת זהות
        /// </summary>
        /// <param name="id">ת.ז.</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckID(string id, bool notEmpty)
        {
            //int i;
            //int sum = 0;
            //int mult;

            ErrorStatus status = ErrorStatus.NONE;

            if (id == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                //if (!IsLegalChars(id, digitChars))
                //{
                //    status = ErrorStatus.ERROR;
                //}
                //else
                //{
                //    if (id.Length < 9)
                //        id = new string('0', 9 - id.Length) + id;

                //    for (i = 0; i < 9; i++)
                //    {
                //        mult = Convert.ToInt32(id[i].ToString()) * ((i % 2 == 0) ? 1 : 2);
                //        if (mult < 10)
                //            sum += mult;
                //        else
                //            sum += (mult / 10 + mult % 10);
                //    }

                //    status = (sum % 10 == 0) ? ErrorStatus.NONE : ErrorStatus.ERROR;
                //}

                if (id.Length >= 2 && id.Length <= 9 && Global.IsLuhn(id))
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// מנקה מחרוזת מספרים מכל תו שונה מסי]רה
        /// </summary>
        /// <param name="number">המספר לטיפול</param>
        /// <returns>מחרוזת המורכבת מספרות בלבד</returns>
        private static string JustDigits(string number)
        {
            string cc = "";
            foreach (char c in number)
                if (c >= '0' && c <= '9')
                    cc += c;

            return cc;
        }

        public enum CreditCardCompanies { VISA, VISA_LEUMI, ISRACARD, MASTERCARD, DINERS, AMERICANEXPRESS };

        /// <summary>
        /// בדיקת מס' כרטיס אשראי
        /// </summary>
        /// <param name="creditCard">(מס' כרטיס האשראי (ללא ישראכארד</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        private static ErrorStatus CheckCreditCardNumber(string creditCard, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (creditCard == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                string cc = JustDigits(creditCard);

                if (cc.Length >= 11 && cc.Length <= 19 && Global.IsLuhn(cc))
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מס' כרטיס אשראי כולל ישראכארד
        /// http://halemo.net/info/isracard/index.html
        /// </summary>
        /// <param name="creditCard">מס' כרטיס האשראי</param>
        /// <param name="isIsraCard">האם כרטיס אשראי ישראכארד</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckCreditCardNumber(string creditCard, CreditCardCompanies company, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (company != CreditCardCompanies.ISRACARD)
            {
                status = CheckCreditCardNumber(creditCard, notEmpty);

                if (status == ErrorStatus.NONE)
                {
                    string cc = JustDigits(creditCard);

                    switch (company)
                    {
                        case CreditCardCompanies.AMERICANEXPRESS:
                            {
                                if (cc.Length != 15 || !(new List<string> { "24", "37" }).Contains(cc.Substring(0, 2)))
                                    status = ErrorStatus.ERROR;
                                break;
                            }

                        case CreditCardCompanies.DINERS:
                            {
                                if (cc.Length != 14 || !(new List<string> { "30", "36", "38" }).Contains(cc.Substring(0, 2)))
                                    status = ErrorStatus.ERROR;
                                break;
                            }

                        case CreditCardCompanies.MASTERCARD:
                            {
                                if (cc.Length != 16 || !(new List<string> { "51", "52", "53", "54", "55" }).Contains(cc.Substring(0, 2)))
                                    status = ErrorStatus.ERROR;
                                break;
                            }

                        case CreditCardCompanies.VISA:
                        case CreditCardCompanies.VISA_LEUMI:
                            {
                                if (cc.Length != 16 || cc[0] != '4')
                                    status = ErrorStatus.ERROR;
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }
            else
            {
                string cc = JustDigits(creditCard);

                if (cc.Length < 8 || cc.Length > 9)
                    status = ErrorStatus.ERROR;
                else
                {
                    if (cc.Length == 8)
                        cc = "0" + creditCard;

                    int multiplier  = 1;
                    int sumOfDigits = 0;

                    for (int i = cc.Length - 1; i >= 0; i--)
                        sumOfDigits += (cc[i] - '0') * multiplier++;

                    status = (sumOfDigits % 11 == 0) ? ErrorStatus.NONE : ErrorStatus.ERROR;
                }
            }

            return status;
        }

        /// <summary>
        /// בדיקת איזור חיוג
        /// </summary>
        /// <param name="areaCode">א. חיוג</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckAreaCode(string areaCode, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (areaCode == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                string regEx = @"\b[0][0,2-9]{1,2}\b";
                if (!Regex.IsMatch(areaCode, regEx))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מס' טלפון
        /// </summary>
        /// <param name="phone">מס' טלפון</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckPhone(string phone, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (phone == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                string regEx = @"\b[2-9][0-9]{6}\b";
                if (!Regex.IsMatch(phone, regEx))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת כתובת דואר אלקטרוני
        /// </summary>
        /// <param name="email">כתובת דוא"ל</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckEMail(string email, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (email == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                string regEx = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
                if (!Regex.IsMatch(email, regEx, RegexOptions.IgnoreCase))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        public static ErrorStatus CheckURL(string url, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (url == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                if (!Global.IsUrl(url))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מיקוד
        /// </summary>
        /// <param name="zip">מיקוד</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>תקין/שגוי</returns>
        public static ErrorStatus CheckZip(string zip, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (zip == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                string regEx = @"\b[2-9][0-9]{6}\b";
                if (!Regex.IsMatch(zip, regEx))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת שדה תאריך
        /// </summary>
        /// <param name="date">מחרוזת התאריך לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>ב</returns>
        public static ErrorStatus CheckDate(object date, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (date.ToString() == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                if (!Global.IsDate(date))
                    status = ErrorStatus.ERROR;
                else
                    if (notEmpty && DateTime.Parse(date.ToString()) == DateTime.MinValue)
                        status = ErrorStatus.EMPTY;
            }

            return status;
        }

        /// <summary>
        /// בדיקת שדה תאריך בטווח תאריכים
        /// </summary>
        /// <param name="date">מחרוזת התאריך לבדיקה</param>
        /// <param name="startDate">תאריך התחלה</param>
        /// <param name="endDate">תאריך סוף</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>ב</returns>
        public static ErrorStatus CheckDate(object date, DateTime startDate, DateTime endDate, bool notEmpty)
        {
            DateTime dateToCheck;

            ErrorStatus status = CheckDate(date, notEmpty);

            if (status != ErrorStatus.EMPTY)
            {
                dateToCheck = Convert.ToDateTime(date);

                if (DateTimeUtil.IsDateInRange(dateToCheck, startDate, endDate))
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת שדה תאריך בטווח תאריכים
        /// </summary>
        /// <param name="date">מחרוזת התאריך לבדיקה</param>
        /// <param name="startDate">תאריך התחלה</param>
        /// <param name="endDate">תאריך סוף</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>ב</returns>
        public static ErrorStatus CheckDate(object date, string startDate, string endDate, bool notEmpty)
        {
            DateTime dateStart;
            DateTime dateEnd;
            bool     isDateOK = true;

            if (Global.IsDate(startDate))
                dateStart = Convert.ToDateTime(startDate);
            else
            {
                isDateOK = false;
                dateStart = DateTime.MinValue;
            }

            if (Global.IsDate(endDate))
                dateEnd = Convert.ToDateTime(endDate);
            else
            {
                isDateOK = false;
                dateEnd = DateTime.MinValue;
            }

            if (isDateOK)
                return CheckDate(date, dateStart, dateEnd, notEmpty);
            else
                return ErrorStatus.ERROR;
        }

        /// <summary>
        /// בדיקת שדה תאריך בטווח גילאים
        /// </summary>
        /// <param name="date">מחרוזת התאריך לבדיקה</param>
        /// <param name="starminYeartDate">יל מינימום</param>
        /// <param name="maxYear">גיל מקסימום</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>ב</returns>
        public static ErrorStatus CheckDate(object date, double minYear, double maxYear, bool notEmpty)
        {
            DateTime dateToCheck;
            double   age;

            ErrorStatus status = CheckDate(date, notEmpty);

            if (status != ErrorStatus.EMPTY)
            {
                dateToCheck = Convert.ToDateTime(date);
                age = Global.Age(dateToCheck);

                if (Global.IsNumberInRange(age, minYear, maxYear))
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת שדה זמן
        /// </summary>
        /// <param name="time">מחרוזת הזמן לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>האם חייב להיות מלא</returns>
        public static ErrorStatus CheckTime(object time, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (Convert.ToString(time) == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                if (!Global.IsTime(time))
                    status = ErrorStatus.ERROR;
                else
                    if (!notEmpty)
                        status = ErrorStatus.EMPTY;

            }

            return status;
        }

        /// <summary>
        /// בדיקת שדה זמן בטווח זמנים
        /// </summary>
        /// <param name="time">מחרוזת הזמן לבדיקה</param>
        /// <param name="startTime">זמן התחלה</param>
        /// <param name="endTime">זמן סוף</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>ב</returns>
        public static ErrorStatus CheckTime(object time, DateTime startTime, DateTime endTime, bool notEmpty)
        {
            DateTime timeToCheck;

            ErrorStatus status = CheckTime(time, notEmpty);

            if (status != ErrorStatus.EMPTY)
            {
                timeToCheck = Convert.ToDateTime(time);

                if (DateTimeUtil.IsDateInRange(timeToCheck, startTime, endTime))
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת שדה זמן בטווח זמנים
        /// </summary>
        /// <param name="time">מחרוזת הזמן לבדיקה</param>
        /// <param name="startTime">זמן התחלה</param>
        /// <param name="endTime">זמן סוף</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns>ב</returns>
        public static ErrorStatus CheckTime(object date, string startTime, string endTime, bool notEmpty)
        {
            DateTime timeStart;
            DateTime timeEnd;
            bool     isTimeOK = true;

            if (Global.IsDate(startTime))
                timeStart = Convert.ToDateTime(startTime);
            else
            {
                isTimeOK = false;
                timeStart = DateTime.MinValue;
            }

            if (Global.IsDate(endTime))
                timeEnd = Convert.ToDateTime(endTime);
            else
            {
                isTimeOK = false;
                timeEnd = DateTime.MinValue;
            }

            if (isTimeOK)
                return CheckDate(date, timeStart, timeEnd, notEmpty);
            else
                return ErrorStatus.ERROR;
        }

        /// <summary>
        /// ComboBox-בדיקת אם נבחר איבר חוקי ב
        /// </summary>
        /// <param name="value">ComboBox של slectedValue-ה</param>
        /// <returns></returns>
        public static ErrorStatus CheckCombo(int value)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (value == 0)
                status = ErrorStatus.EMPTY;

            return status;
        }

        /// <summary>
        /// בדיקת מספר שלם
        /// </summary>
        /// <param name="num">המספר לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns></returns>
        public static ErrorStatus CheckInteger(object num, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (Convert.ToString(num) == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                if (!(num is Int16 || num is Int32 || num is Int64))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מספר שלם בטווח
        /// </summary>
        /// <param name="num">המספר לבדיקה</param>
        /// <param name="minValue">ערך מינימום</param>
        /// <param name="maxValue">ערך מקסימום</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns></returns>
        public static ErrorStatus CheckInteger(object num, int minValue, int maxValue, bool notEmpty)
        {
            ErrorStatus status = CheckInteger(num, notEmpty);

            if (status == ErrorStatus.NONE)
            {
                int n = int.Parse(num.ToString());
                
                if (Global.IsNumberInRange(n, minValue, maxValue))
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מספר עשרוני
        /// </summary>
        /// <param name="num">המספר לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns></returns>
        public static ErrorStatus CheckDouble(object num, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (Convert.ToString(num) == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                if (!(num is double))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת נספר עשרוני בטווח
        /// </summary>
        /// <param name="num">המספר לבדיקה</param>
        /// <param name="minValue">ערך מינימום</param>
        /// <param name="maxValue">ערך מקסימום</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns></returns>
        public static ErrorStatus CheckDouble(object num, double minValue, double maxValue, bool notEmpty)
        {
            ErrorStatus status = CheckDouble(num, notEmpty);

            if (status == ErrorStatus.NONE)
            {
                double n = double.Parse(num.ToString());
                
                if (Global.IsNumberInRange(n, minValue, maxValue))
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מספר דצימאלי
        /// </summary>
        /// <param name="num">המספר לבדיקה</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns></returns>
        public static ErrorStatus CheckDecimal(object num, bool notEmpty)
        {
            ErrorStatus status = ErrorStatus.NONE;

            if (Convert.ToString(num) == "")
            {
                if (notEmpty)
                    status = ErrorStatus.EMPTY;
            }
            else
            {
                if (!(num is decimal))
                    status = ErrorStatus.ERROR;
            }

            return status;
        }

        /// <summary>
        /// בדיקת מספר דצימאלי בטווח
        /// </summary>
        /// <param name="num">המספר לבדיקה</param>
        /// <param name="minValue">ערך מינימום</param>
        /// <param name="maxValue">ערך מקסימום</param>
        /// <param name="notEmpty">האם חייב להיות מלא</param>
        /// <returns></returns>
        public static ErrorStatus CheckDecimal(object num, decimal minValue, decimal maxValue, bool notEmpty)
        {
            ErrorStatus status = CheckDecimal(num, notEmpty);

            if (status == ErrorStatus.NONE)
            {
                decimal n = decimal.Parse(num.ToString());

                if (n > minValue && n <= maxValue)
                    status = ErrorStatus.NONE;
                else
                    status = ErrorStatus.ERROR;
            }

            return status;
        }
    }
}
