using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ApertureScience
{
    public class Employee
    {
        public Employee() { }

        public Employee(string email, string password, string firstName, string lastName, Roles role = Roles.ADMIN) :
            this(email, password, firstName, lastName, role, "", "")
        {
        }

        public Employee(string email, string password, string firstName, string lastName, Roles role, string phone, string photoNames)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Phone = phone;
            PhotoNames = photoNames;
        }

        public enum Roles
        {
            UNDEFINED = 0,
            STAFF,
            MANAGER,
            ADMIN
        }

        public Object Reduce() => new
        {
            Id,
            Role,
            Email,
            FirstName,
            LastName,
            Phone,
            PhotoNamesList
        };


        public int Id { get; set; }
        [Required]
        [Range((int)Roles.STAFF, (int)Roles.ADMIN)]
        public Roles Role { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Phone { get; set; } = "";
        public string PhotoNames { get; set; } = "";
        [NotMapped]
        public List<string> PhotoNamesList
        {
            set
            {
                if (value == null)
                    PhotoNames = null;
                else if (value.Count == 0)
                    PhotoNames = "";
                else
                    PhotoNames = value.Aggregate((pn1, pn2) => pn1 + "|" + pn2);
            }
            get
            {
                if (PhotoNames == null)
                    return null;
                if (PhotoNames == "")
                    return new List<string>();
                return PhotoNames.Split("|").ToList();
            }
        }
    }
}
