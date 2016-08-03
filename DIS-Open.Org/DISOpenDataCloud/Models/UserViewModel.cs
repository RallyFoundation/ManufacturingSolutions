using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DISOpenDataCloud.Models
{
    public class UserViewModel
    {
        [Display(Name = "ID")]
        public string ID { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [Phone]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Nick Name")]
        public string NickName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "SID")]
        public string SID { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Education")]
        public string Education { get; set; }

        [Display(Name = "Industry")]
        public string Industry { get; set; }

        [Display(Name = "Headline")]
        public string Headline { get; set; }

        [Display(Name = "HeadlImageID")]
        public string HeadlImageID { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MaxLength(300)]
        public string Description { get; set; }

        [Display(Name = "Roles")]
        public List<RoleViewModel> Roles { get; set; }
    }
}