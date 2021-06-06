using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }    
        public bool IsActive { get; set; }
        public string Birth { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName) : base(roleName)
        {
            
        }

        public ApplicationRole(string roleName, string description, int grade) : base(roleName)
        {
            this.Description = description;
            this.Grade = grade;
        }

        [Display(Name = "상세 설명")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "등급")]
        [Required]
        public int Grade { get; set; }
    }



}
