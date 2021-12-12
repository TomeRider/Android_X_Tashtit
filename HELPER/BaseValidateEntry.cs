using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HELPER
{
    public enum ErrorStatus { NONE, EMPTY, ERROR };

    public abstract class BaseValidateEntry
    {
        protected static string hebChars    = "אבגדהוזחטיכלמנסמעפצקרשת";
        protected static string hebEndChars = "ךםןףץ";
        protected static string otherChars  = @"""'- ";
        protected static string digitChars  = "0123456789";
        protected static string engChars    = "abcdefghijklmnopqrstwxyz";

        protected static string m_HebrewChars = "אבגדהוזחטיכלמנסעפצקרשתךםןףץ";
        protected static string m_EndHebrewChars = "ךםןףץ";
        protected static string m_LowerChars = "abcdefghigklmnopqrstuvwxyz";
        protected static string m_UpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        protected static string m_Digits = "0123456789";
        protected static string m_OtherChars = @"., -/'""";

        /// <summary>
        /// הופכת אותיות קטנות באנגלית לגדולות
        /// </summary>
        /// <returns></returns>
        protected static string EngCapitalChars()
        {
            return engChars.ToUpper();
        }
        
        /// <summary>
        /// האם מחרוזת מורכבת מקבוצת תווים מותרים
        /// </summary>
        /// <param name="str">המחרוזת לבדיקה</param>
        /// <param name="legalchars">קבוצת בתווים במותרים</param>
        /// <returns></returns>
        protected static bool IsLegalChars(string str, string legalchars)
        {
            bool isOK = true;

            foreach (char c in str)
                if (!legalchars.Contains(c))
                    isOK = false;

            return isOK;
        }

        /// <summary>
        /// áåã÷ àí îçøåæú îåøëáú îàåñó úååéí çå÷ééí
        /// </summary>
        /// <param name="s">äîçøåæú ìáãé÷ä</param>
        /// <param name="allowesChars">äúååé äçå÷ééí</param>
        /// <returns>àîú àí äîçøåæú îåøëáú îäúååéí äçå÷ééí àçøú ù÷ø</returns>
        protected static bool CheckString(string s, string allowesChars)
        {
            bool ret = true;

            foreach (char c in s)
            {
                if (allowesChars.IndexOf(c) == -1)
                    ret = false;
            }

            return ret;
        }

        /// <summary>
        /// áåã÷ àí äîçøåæú îåøëáú îàåúéåú òáøéåú
        /// </summary>
        /// <param name="s">îçøåæú ìáãé÷ä</param>
        /// <returns>àîø àí ëì äàåúéåú òáøéåú àçøú ù÷ø</returns>
        public static bool IsHebrewChars(string s)
        {
            return CheckString(s, m_HebrewChars);
        }

        /// <summary>
        /// áåã÷ àí äîç÷åæú îåøëáú îñôøåú
        /// </summary>
        /// <param name="s">îçøåæú ìáãé÷ä</param>
        /// <returns>àîú àí äîçøåæú îëéìä ñôøåú áìáã àç÷ú ù÷ø</returns>
        public static bool IsDigits(string s)
        {
            return CheckString(s, m_Digits);
        }

        /// <summary>
        /// áåã÷ àí äîç÷åæú îåøëáú îàåúéåú àðâìéåú ÷èðåú
        /// </summary>
        /// <param name="s">îçøåæú ìáãé÷ä</param>
        /// <returns>àîú àí äîçøåæú îëéìä ñôøåú áìáã àç÷ú ù÷ø</returns>
        public static bool IsLowerChars(string s)
        {
            return CheckString(s, m_LowerChars);
        }

        /// <summary>
        /// áåã÷ àí äîç÷åæú îåøëáú îàåúéåú àðâìéåú âãåìåú
        /// </summary>
        /// <param name="s">îçøåæú ìáãé÷ä</param>
        /// <returns>àîú àí äîçøåæú îëéìä ñôøåú áìáã àç÷ú ù÷ø</returns>
        public static bool IsUpperChars(string s)
        {
            return CheckString(s, m_UpperChars);
        }

        /// <summary>
        /// áåã÷ àí äîç÷åæú îåøëáú îñôøåú àå àåúéåú òáøéåú åñéîðéí
        /// </summary>
        /// <param name="s">îçøåæú ìáãé÷ä</param>
        /// <returns>àîú àí äîçøåæú îëéìä àåúéåú òáøéåú, ñôøåú åñéîðéí àçøú ù÷ø</returns>
        public static bool IsHebrewOrDigits(string s)
        {
            return CheckString(s, m_HebrewChars + m_Digits + m_OtherChars);
        }
    }
}
