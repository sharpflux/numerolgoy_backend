namespace numerology_backend.Models
{
    public class SoulRequest
    {
        public string Soul_Id { get; set; }

        public string Soul_No { get; set; }


        public string Soul_Name { get; set; }

        public string Soul_Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; } // Or replace `object` with your specific data type
    }
}
