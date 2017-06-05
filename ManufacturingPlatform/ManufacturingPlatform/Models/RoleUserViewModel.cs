using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DISOpenDataCloud.Models
{
    public class RoleUserViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Role Users")]
        public List<UserViewModel> RoleUsers { get; set; }

        [Display(Name = "Users")]
        public List<UserViewModel> Users { get; set; }
    }
}