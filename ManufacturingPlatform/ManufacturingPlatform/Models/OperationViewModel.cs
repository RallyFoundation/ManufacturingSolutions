using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DISOpenDataCloud.Models
{
    public class OperationViewModel
    {
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Data Type")]
        public string DataType { get; set; }

        [Display(Name = "Roles")]
        public List<RoleViewModel> Roles { get; set; }
    }
}