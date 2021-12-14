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

namespace MODEL
{

   public class Player
    {
        private int p_identifyNumber;
        private string p_picture;
        private string p_name;
        private int p_age;
        private int p_salary;
        private string p_position;

        public string P_picture { get => p_picture; set => p_picture = value; }
        public string P_name { get => p_name; set => p_name = value; }
        public int P_age { get => p_age; set => p_age = value; }
        public int P_salary { get => p_salary; set => p_salary = value; }
        public string P_position { get => p_position; set => p_position = value; }
        public int P_identifyNumber { get => p_identifyNumber; set => p_identifyNumber = value; }

        public Player(int p_id, string p_picture, string p_name, int p_age, int p_salary, string p_position)
        {
            this.P_identifyNumber = p_id;
            this.P_picture = p_picture;
            this.P_name = p_name;
            this.P_age = p_age;
            this.P_salary = p_salary;
            this.P_position = p_position;
        }
        public Player()
        {

        }
    }
    
}