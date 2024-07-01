using OmsSolution.Entities;

namespace OmsSolution.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string accessToken { get; set; }
        public string RoleId { get; set; }
        public bool IsChangePassword { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            accessToken = user.accessToken;
            RoleId = user.RoleId;
            Token = token;
            IsChangePassword=user.IsChangePassword;
        }
    }
}