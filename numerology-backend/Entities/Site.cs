using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsSolution.Entities
{
    public class Site
    {
        [Required]
        public string siteMasterId { get; set; }
        [Required]
        public string sitename { get; set; }
        [Required]
        public string sitelocation { get; set; }
        [Required]
        public string startdate { get; set; }
        [Required]
        public int userAssignedId { get; set; }
  
        
    }
}
