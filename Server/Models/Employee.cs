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
        [Key]
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
        };
    }
}
