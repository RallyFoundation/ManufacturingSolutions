using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DISOpenDataCloud.Models
{
    public class RoleViewModel
    {

        public string ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(26)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MaxLength(300)]
        public string Description { get; set; }

        public bool IsSelected { get; set; }
    }
}