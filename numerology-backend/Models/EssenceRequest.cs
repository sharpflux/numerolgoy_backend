namespace numerology_backend.Models
{
    public class EssenceRequest
    {
        public string Essence_Id { get; set; }

        public string Essence_Name { get; set; }


        public string Essence_No { get; set; }

        public string Essence_Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; } // Or replace `object` with your specific data type
    }
}
