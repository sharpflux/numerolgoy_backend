using System.ComponentModel.DataAnnotations;

namespace NumerologystSolution.Models
{
    public class ClientRequest
    {

        public string Client_id { get; set; }
        public string ClientNumberID { get; set; }
     
        public string firstName { get; set; }
        public string First_Name { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }
        public string mobileNo1 { get; set; }

        public string mobileNo2 { get; set; }


        public string mobileNo3 { get; set; }

        public string Gender { get; set; }
        public string DateOfBirth { get; set; }

        public string vechileNo1 { get; set; }
        public string Vechile_No1 { get; set; }


        public string vechileNo2 { get; set; }

        public string vechileNo3 { get; set; }
        public string houseNo1 { get; set; }

        public string houseNo2 { get; set; }
        public string houseNo3 { get; set; }

        public string emailId { get; set; }

        public bool IsActive { get; set; }
    }
}
