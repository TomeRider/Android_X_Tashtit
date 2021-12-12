using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace MODEL
{
    [Table ("Cities")]
    public class City : BaseEntity
    {
        private string name;

        public City() { }

        public City(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }

        public override bool Equals(object obj)
        {
            return obj is City city &&
                   base.Equals(obj) &&
                   name == city.name;
        }

        public static bool operator ==(City left, City right)
        {
            return EqualityComparer<City>.Default.Equals(left, right);
        }

        public static bool operator !=(City left, City right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return name;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}
