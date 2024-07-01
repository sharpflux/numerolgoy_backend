using BAL;
using OmsSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Service;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OmsSolution.Entities;
using OmsSolution.Helpers;
using OmsSolution.Models;
using static Services.LoginDAL;
using static Services.MenuDAL;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NumerologystSolution.Services;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();

        Task<IEnumerable<LoginBAL>> UserAuthenticationsByToken(string userToken);

        Task<IEnumerable<MenuBAL>> DynamicMenuAside(string RoleId);

        Task<List<DropdownOption>> RoleMasterGet();

        Task<bool> MasterUserInsertUpdate(UserRequest model);

        Task<bool> MasterUsersChangePassword(UserChangePasswordRequest model);

        Task<bool> VerifyEmail(string EmailId);
        Task<User> GetById(int id);

        Task<DataTable> MasterUsersGET(PaginationRequest model);

        Task<IEnumerable<MenuItem>> MenuSideBarGet(string roleId);

        Task<User> GetUserByAccessToken(string accessToken);
        Task<List<DropdownOption>> AllDropdown(string searchTerm, int page, int pageSize, int type, int parentId);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly LoginDAL _sqlDBHelper;
        //private readonly ClientDBHelper _sqlDBHelper;
        public UserService(IOptions<AppSettings> appSettings, LoginDAL sqlDBHelper, IConfiguration config)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
            _config = config;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.UserAuthentications(new LoginStruct
                {
                    UserName = model.Username,
                    Passwords = model.Password,
                    ipAddress = ""
                });

                if (dataTable.Rows.Count > 0)
                {
                    var user = new User
                    {
                        Id = int.Parse(dataTable.Rows[0]["UserId"].ToString()),
                        FirstName = dataTable.Rows[0]["FirstName"].ToString(),
                        LastName = dataTable.Rows[0]["LastName"].ToString(),
                        Username = dataTable.Rows[0]["UserName"].ToString(),
                        accessToken = dataTable.Rows[0]["accessToken"].ToString(),
                        RoleId = dataTable.Rows[0]["RoleId"].ToString(),
                        EmailId = dataTable.Rows[0]["EmailId"].ToString(),
                        IsChangePassword = Convert.ToBoolean(dataTable.Rows[0]["IsChangePassword1"].ToString())
                    };
                    var jwt = new JwtService(_config);
                    var token = jwt.GenerateSecurityToken(user);

                    return new AuthenticateResponse(user, token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            // Authentication failed
            return null;
        }
        public async Task<List<DropdownOption>> AllDropdown(string searchTerm, int page, int pageSize, int type, int parentId)
        {
            try
            {
                DataTable dataTable = await _sqlDBHelper.AllDropdown(searchTerm, page, pageSize, type, parentId);

                var dropdownOptions = new List<DropdownOption>();

                foreach (DataRow row in dataTable.Rows)
                {
                    var dropdownOption = new DropdownOption
                    {
                        Id = (int)row["Id"],
                        Text = row["Texts"].ToString()
                    };

                    dropdownOptions.Add(dropdownOption);
                }
                return dropdownOptions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> MasterUserInsertUpdate(UserRequest model)
        {
            try
            {

                model.passwords = GenerateRandomPassword(6);

                bool dataTable = await _sqlDBHelper.MasterUsersInsertUpdateAsync(new LoginStruct
                {

                    UserId = Convert.ToInt32(model.userId),
                    RoleId = Convert.ToInt32(model.roleId),
                    UserName = model.userName,
                    Passwords = model.passwords,
                    MobileNo = model.mobileNo,
                    FirstName = model.firstName,
                    MiddleName = model.middleName,
                    LastName = model.lastName,
                    EmailId = model.emailId,
                    IsActive= model.IsActive,
                });

                return dataTable;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<User> GetUserByAccessToken(string accessToken)
        {
            try
            {
                await Task.Delay(0);

                var jwtService = new JwtService(_config); // Create an instance of your JwtService
                var tokenValidationResult = jwtService.ValidateToken(accessToken);

                if (tokenValidationResult.IsValid)
                {

                    // The token is valid, so we can extract user information from the token payload
                    var userIdClaim = tokenValidationResult.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                    var emailClaim = tokenValidationResult.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
                    var roleIdClaim = tokenValidationResult.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

                    var fistnameClaim = tokenValidationResult.Claims.FirstOrDefault(claim => claim.Type == "FirstName");
                    var lastNameClaim = tokenValidationResult.Claims.FirstOrDefault(claim => claim.Type == "LastName");
                    var usernameClaim = tokenValidationResult.Claims.FirstOrDefault(claim => claim.Type == "Username");
                    var isChangePasswordClaim = tokenValidationResult.Claims.FirstOrDefault(claim => claim.Type == "isChangePassword");

                    if (userIdClaim != null && emailClaim != null && roleIdClaim != null)
                    {
                        var userId = userIdClaim.Value;
                        var email = emailClaim.Value;
                        var roleId = roleIdClaim.Value;

                        var firstname = fistnameClaim.Value;
                        var lastname = lastNameClaim.Value;
                        var username = usernameClaim.Value;
                        var isChangePassword = isChangePasswordClaim.Value;
                        // You can retrieve additional user information as needed from the token payload

                        return new User
                        {
                            Id = int.Parse(userId),
                            EmailId = email,
                            RoleId = roleId,
                            FirstName = firstname,
                            LastName = lastname,
                            Username = username,
                            IsChangePassword = Convert.ToBoolean(isChangePassword),

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Token validation failed or user not found
            return null;
        }

        public async Task<bool> MasterUsersChangePassword(UserChangePasswordRequest model)
        {
            try
            {

                bool dataTable = await _sqlDBHelper.MasterUsersChangePasswordNew(new LoginStruct
                {
                    UserId = Convert.ToInt32(model.userId),
                    Passwords = model.passwords,
                });

                return dataTable;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        string GenerateJWTTokenOld(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim("fullName", userInfo.FirstName.ToString()),
                new Claim("role",userInfo.RoleId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJwtToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim("fullName", userInfo.FirstName.ToString()),
                new Claim("role", userInfo.RoleId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<IEnumerable<LoginBAL>> UserAuthenticationsByToken(string userToken)
        {
            try
            {
                var dataTable = await _sqlDBHelper.UserAuthenticationsByTokenAsync(new LoginStruct
                {
                    accessToken = userToken,
                });

                if (dataTable.Rows.Count > 0)
                {

                    List<LoginBAL> lst = new List<LoginBAL>();
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        LoginBAL emp = new LoginBAL();
                        emp.roles = new List<int>();
                        emp.id = Convert.ToInt32(dr["UserId"].ToString());
                        emp.UserId = Convert.ToInt32(dr["UserId"].ToString());
                        emp.UserName = dr["UserName"].ToString();
                        emp.MiddleName = dr["MiddleName"].ToString();
                        emp.fullname = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                        emp.LastName = dr["LastName"].ToString();
                        emp.ImageUrl = "";
                        emp.EmailId = dr["EmailId"].ToString();
                        emp.MobileNo = dr["MobileNo"].ToString();
                        emp.accessToken = dr["accessToken"].ToString();
                        emp.pic = "./assets/media/users/300_25.jpg";
                        emp.roles.Add(1);
                        lst.Add(emp);
                    }



                    return lst;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            // Authentication failed
            return null;
        }

        public List<DynamicSubMenu> GetSubMenus(DataTable dt, int MenuId)
        {
            List<DynamicSubMenu> submenus = new List<DynamicSubMenu>();
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["ParentId"].ToString()) == MenuId)
                {
                    DynamicSubMenu submenusItem = new DynamicSubMenu();
                    submenusItem.title = dr["title"].ToString();
                    submenusItem.root = Convert.ToBoolean(dr["root"].ToString());
                    submenusItem.bullet = dr["bullet"].ToString();
                    submenusItem.icon = dr["icon"].ToString();
                    submenusItem.page = dr["page"].ToString();
                    submenus.Add(submenusItem);
                }
            }
            return submenus;
        }

        public async Task<IEnumerable<MenuBAL>> DynamicMenuAside(string RoleId)
        {
            try
            {
                DataSet dataTable = await _sqlDBHelper.DynamicMenuAside(new MenuDAL.DynamicMenuStruct
                {
                    RoleId = Convert.ToInt32(RoleId)
                });

                List<MenuBAL> lst = new List<MenuBAL>();
                foreach (DataRow dr in dataTable.Tables[0].Rows)
                {
                    MenuBAL item = new MenuBAL();
                    item.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                    item.section = dr["section"].ToString();
                    item.title = dr["title"].ToString();
                    item.root = Convert.ToBoolean(dr["root"].ToString());
                    item.bullet = dr["bullet"].ToString();
                    item.icon = dr["icon"].ToString();
                    item.page = dr["page"].ToString();
                    item.submenu = GetSubMenus(dataTable.Tables[1], Convert.ToInt32(dr["MenuId"].ToString()));
                    lst.Add(item);
                }

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<DropdownOption>> RoleMasterGet()
        {
            try
            {
                DataSet dataTable = await _sqlDBHelper.MasterRolesGet(new MenuDAL.DynamicMenuStruct
                {
                    RoleId = 0
                });

                var dropdownOptions = new List<DropdownOption>();

                foreach (DataRow row in dataTable.Tables[0].Rows)
                {
                    var dropdownOption = new DropdownOption
                    {
                        Id = (int)row["RoleId"],
                        Text = row["RoleCode"].ToString()
                    };

                    dropdownOptions.Add(dropdownOption);
                }



                return dropdownOptions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<DataTable> MasterUsersGET(PaginationRequest model)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.MasterUsersGET(new PaginationRequest
                {
                    StartIndex = model.StartIndex,
                    PageSize = model.PageSize,
                    SearchBy = model.SearchBy,
                    SearchCriteria = model.SearchCriteria
                });

                return dataTable;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }

        public async Task<bool> VerifyEmail(string EmailId)
        {
            try
            {
                bool dataTable = await _sqlDBHelper.MasterUsersVerfiyEmail(EmailId);
                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(int id)
        {
            try
            {
                var dataTable = await _sqlDBHelper.UserAuthenticationsGetById(id);

                if (dataTable.Rows.Count > 0)
                {
                    var user = new User
                    {
                        Id = int.Parse(dataTable.Rows[0]["EmployeeId"].ToString()),
                        FirstName = dataTable.Rows[0]["FirstName"].ToString(),
                        LastName = dataTable.Rows[0]["LastName"].ToString(),
                        Username = dataTable.Rows[0]["EmployeeCode"].ToString(),
                        accessToken = dataTable.Rows[0]["accessToken"].ToString(),
                        RoleId = dataTable.Rows[0]["RoleId"].ToString(),

                    };
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<MenuItem>> MenuSideBarGet(string roleId)
        {
            try
            {
                DataTable dataTable = await _sqlDBHelper.Menu_Sidebar_GET(roleId);

                List<MenuItem> lst = new List<MenuItem>();
                foreach (DataRow dr in dataTable.Rows)
                {
                    MenuItem item = new MenuItem();
                    if (dr["ParentId"] == DBNull.Value)
                    {
                        item.Id = dr["SidebarMenuId"].ToString();
                        item.Title = dr["Name"].ToString();
                        item.Icon = dr["Icon"].ToString();
                        item.Link = dr["Route"].ToString();
                        item.SubItems = MenuSubItems(dataTable, Convert.ToInt32(dr["SidebarMenuId"].ToString()));
                        lst.Add(item);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<SubItem> MenuSubItems(DataTable dt, int MenuId)
        {
            List<SubItem> submenus = new List<SubItem>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ParentId"] != DBNull.Value)
                {
                    if (Convert.ToInt32(dr["ParentId"].ToString()) == MenuId)
                    {
                        SubItem submenusItem = new SubItem();
                        submenusItem.Link = dr["Route"].ToString();
                        submenusItem.Title = dr["Name"].ToString();
                        submenus.Add(submenusItem);
                    }
                }
            }
            return submenus;
        }


        public string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            return password.ToString();
        }






    }
}
public class DropdownOption
{
    public int Id { get; set; }
    public string Text { get; set; }
}