using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HELPER
{
    public class ValidationException : Exception
    {
        private string message;

        public override string Message { get { return message; } }

        public ValidationException()
            : base("שגיאת קלט")
        {
        }

        public ValidationException(ErrorStatus status, string description)
        {
            if (status == ErrorStatus.EMPTY)
                message = "יש למלא " + description;
            else
                if (status == ErrorStatus.ERROR)
                    message = description + " שגוי";
        }

        public ValidationException(string description)
        {
            message = description;
        }
    }
}
