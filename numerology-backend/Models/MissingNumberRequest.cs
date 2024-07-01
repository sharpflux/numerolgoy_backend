namespace NumerologystSolution.Models
{
    public class MissingNumberRequest
    {
        public string missingNo_id { get; set; }

        public string missingNumber { get; set; }


        public string missingName { get; set; }

        public string missingDescription { get; set; }
  
        public bool IsActive { get; set; }
    }
}
