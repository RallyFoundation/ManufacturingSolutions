using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DISOpenDataCloud.Models
{
    public class RoleOperationViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Role Operations")]
        public List<OperationViewModel> RoleOperations { get; set; }

        [Display(Name = "Operations")]
        public OperationListViewModel Operations { get; set; }
    }
}
