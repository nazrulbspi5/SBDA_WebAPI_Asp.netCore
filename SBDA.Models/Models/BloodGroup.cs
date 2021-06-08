using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SBDA.Models.Models
{
    public class BloodGroup
    {
        public int BloodGroupID { get; set; }
        [Required]
        [StringLength(10)]
        public string BloodGroupName { get; set; }
    }
}
