using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace SBDA.Models.Models
{
    public class Member
    {
        public int MemberID { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50),EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Mobile { get; set; }
        [Required]
        [StringLength(500)]
        public string Address { get; set; }
        public string PhotoPath { get; set; }
        [Required]
        public int BloodGroupId { get; set; }
        public BloodGroup BloodGroups { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
