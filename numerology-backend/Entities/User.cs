using System.Text.Json.Serialization;

namespace OmsSolution.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string accessToken { get; set; }
        public string RoleId { get; set; }
        public string EmailId { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        public bool IsChangePassword { get; set; }
    }
}