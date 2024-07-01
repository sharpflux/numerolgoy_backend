using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsSolution.Models
{
    public class DataModel
    {
 
        public string IntervalData { get; set; }
        public string THD { get; set; }
        public string TND { get; set; }
        public string TCD { get; set; }
        public string TCUP { get; set; }
        public string TCOIN { get; set; }
        public string TQC { get; set; }
        public string TBC { get; set; }
        public string TC { get; set; }
        public string ERR { get; set; }
        public string PID { get; set; }
        public DateTime Datetime { get; set; }
    }
}
