using OmsSolution.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsSolution.Models
{
    public class UserResponse
    {

        public int UserId { get; set; }
        public string CompanyId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string Passwords { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }

        public string MobileNo { get; set; }
 
        public string ImageUrl { get; set; }

        public UserResponse(MasterUsers user)
        {
            UserId = user.UserId;
            CompanyId = user.CompanyId;
            RoleId = user.RoleId;
            UserName = user.UserName;
            Passwords = user.Passwords;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            EmailId = user.EmailId;
            MobileNo = user.MobileNo;
            ImageUrl = user.ImageUrl;
        }

    }
}
