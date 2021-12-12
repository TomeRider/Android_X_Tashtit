using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HELPER;

namespace MODEL
{
    public class User : BaseEntity
    {
        private string family;
        private string name;
        private string image;
        private DateTime birthDate;
        private string email;
        private string phone;

        public User() { }

        public string   Family    { get => family; set => family = value; }
        public string   Name      { get => name; set => name = value; }
        public string   Image     { get => image; set => image = value; }
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }
        public string   Email     { get => email; set => email = value; }
        public string   Phone     { get => phone; set => phone = value; }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(family) &&
                   !string.IsNullOrEmpty(name) &&
                   !string.IsNullOrEmpty(image) &&
                   ValidateEntry.CheckEMail(email, true) == ErrorStatus.NONE &&
                   ValidateEntry.CheckPhone(phone, true) == ErrorStatus.NONE &&
                   DateTimeUtil.IsValidDate(birthDate.ToShortDateString()) &&
                   Convert.ToInt32(DateTimeUtil.Age(birthDate).Substring(0,2)) >= 12;
        }

        public string FullName
        {
            get { return family + " " + name; }
        }

        public override string ToString()
        {
            return FullName;
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   base.Equals(obj) &&
                   family == user.family &&
                   name == user.name &&
                   image == user.image &&
                   birthDate == user.birthDate &&
                   email == user.email &&
                   phone == user.phone;
        }

        public static bool operator ==(User left, User right)
        {
            return EqualityComparer<User>.Default.Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !(left == right);
        }
    }
}
