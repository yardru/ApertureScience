using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ApertureScience
{
    [Serializable]
    public class Employee
    {
        public Employee() { }

        public Employee(string email, string password, string firstName, string lastName, Roles role = Roles.ADMIN) :
            this(email, password, firstName, lastName, role, null, null)
        {
        }

        public Employee(string email, string password, string firstName, string lastName, Roles role, string phone, string[] photoNames)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Phone = phone;
            if (photoNames != null)
                PhotoNamesArr = (string[])photoNames.Clone();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public enum Roles
        {
            STAFF,
            MANAGER,
            ADMIN
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role { get; set; }

        public string Phone { get; set; }

        [NotMapped]
        public string[] PhotoNamesArr { get; set; }
        [JsonIgnore]
        public string PhotoNames
        {
            get
            {
                if (PhotoNamesArr == null)
                    return null;
                return PhotoNamesArr.Aggregate((pn1, pn2) => pn1 + "|" + pn2);
            }
            set
            {
                if (value != null)
                    PhotoNamesArr = value.Split("|");
                else
                    PhotoNamesArr = null;
            }
        }
    }
}
