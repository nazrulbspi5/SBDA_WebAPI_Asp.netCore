using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDA.API.AuthModels
{
    public class RegisterViewModel
    {
        //[Required]
        //public string Name { get; set; }
        //[Required]
        //[StringLength(50)]
        //[EmailAddress]
        //public string Email { get; set; }
        //[Required]
        //[StringLength(50,MinimumLength =5)]
        //public string Password { get; set; }
        //[Required]
        //[StringLength(50, MinimumLength = 5)]
        //public string ConfirmPassword { get; set; }
        //[Required]
        //public string RoleName { get; set; }


        [Required]
        public int BloodGroupId { get; set; }
        [Required]
        [StringLength(50)]

        public string Name { get; set; }
        [Required]
        [StringLength(50)]

        public string Mobile { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(500)]

        public string Address { get; set; }
        [Required]
        [StringLength(50)]

        public string UserName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
