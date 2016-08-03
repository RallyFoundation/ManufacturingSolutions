using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DISOpenDataCloud.Models
{
    public class UserRoleViewModel
    {
        [Display(Name = "ID")]
        public string ID { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [Phone]
        public string UserName { get; set; }

        [Display(Name = "User Roles")]
        public List<RoleViewModel> UserRoles { get; set; }

        [Display(Name = "Roles")]
        public List<RoleViewModel> Roles { get; set; }
    }
}