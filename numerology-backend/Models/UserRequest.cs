using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsSolution.Models
{
    public class UserRequest
    {
        [Required]
        public string userId { get; set; }

        [Required]
        public string roleId { get; set; }

        [Required]
        public string userName { get; set; }
        
        public string passwords { get; set; }
         
        public string confirmPassword { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string middleName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string emailId { get; set; }
        [Required]
        public string mobileNo { get; set; }

        public bool IsActive { get; set; }

        public bool IsEditMode { get; set; }

    }


    public class UserChangePasswordRequest
    {
        [Required]
        public string userId { get; set; }
 

        public string passwords { get; set; }
 
    }
}
