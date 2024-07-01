using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmsSolution.Entities
{
    public class MasterUsers
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
        [JsonIgnore]
        public string ImageUrl { get; set; }

        
    }
}
