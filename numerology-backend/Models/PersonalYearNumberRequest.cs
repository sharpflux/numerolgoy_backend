namespace numerology_backend.Models
{
    public class PersonalYearNumberRequest
    {
        public string PersonalYearId { get; set; }

        public string PersonalYearNumber { get; set; }


        public string Description { get; set; }

        //  public string RepeatPredictionId { get; set; }

        public string NameNumberId { get; set; }

        public string NameNumbersID { get; set; }


        public string NameNumber_Description { get; set; }
        public bool IsActive { get; set; }
    }
}