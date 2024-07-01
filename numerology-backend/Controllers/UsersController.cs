using OmsSolution.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services;
using static Services.LoginDAL;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using OmsSolution.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.DataProtection;
 
using OmsSolution.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace OmsSolution.Controllers
{
    [EnableCors("DefaultCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly EmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IDataProtector _dataProtector;
        public UsersController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IUserService userService, IUrlHelperFactory urlHelperFactory, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _urlHelperFactory = urlHelperFactory;
            _dataProtector = dataProtectionProvider.CreateProtector("EmailProtection");
            _emailSender = new EmailSender(_hostingEnvironment, _httpContextAccessor);
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }


        [HttpGet("getUserByAccessToken")]
        public async Task<IActionResult> GetUserByAccessToken(string AccessToken)
        {


            var response = await _userService.GetUserByAccessToken(AccessToken);

            if (response == null)
                return BadRequest(new { message = "Token is Invalid" });

            return Ok(response);
        }



        [Authorize]
        [HttpPost("MasterUsersChangePassword")]
        public async Task<IActionResult> MasterUsersChangePassword(UserChangePasswordRequest model)
        {

            try
            {
                var response = await _userService.MasterUsersChangePassword(model);

                if (response == true)
                    return Ok(true);

                if (response == false)
                    return BadRequest(new { message = "User already exists" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(true);


        }


        [Authorize]
        [HttpPost("masterUserInsertUpdate")]
        public async Task<IActionResult> MasterUserInsertUpdate(UserRequest model)
        {

            try
            {
                var response = await _userService.MasterUserInsertUpdate(model);

                if (response == true)
                {
                    if (model.IsEditMode == false)
                    {
                        var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);
                        string encryptedEmail = _dataProtector.Protect(model.emailId);
                        string verificationUrl = urlHelper.Action("Index", "Verify", new { email = encryptedEmail }, HttpContext.Request.Scheme);
                        _emailSender.SendEmailPaymentNotFound(model.emailId, verificationUrl, model.emailId, model.passwords, model.firstName);


                        //var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);
                        //string encryptedEmail = _dataProtector.Protect(model.emailId);
                        //string verificationUrl = urlHelper.Action("Index", "Verify", new { email = encryptedEmail }, Request.Scheme);
                        //_emailSender.SendEmailPaymentNotFound(model.emailId, verificationUrl, model.emailId, model.passwords, model.firstName);
                    }

                }

                if (response == false)
                    return BadRequest(new { message = "User already exists" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(true);


        }

        //  [Authorize]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        //[Authorize]
        [HttpGet("getUserByTokenAsync")]
        public async Task<IActionResult> getUserByTokenAsync(string userToken)
        {
            var response = await _userService.UserAuthenticationsByToken(userToken);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("dynamicMenuaside")]

        public async Task<IActionResult> DynamicMenuAside(string RoleId)
        {
            var response = await _userService.DynamicMenuAside(RoleId);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("roleMasterGet")]
        public async Task<IActionResult> RoleMasterGet(string Client_id)
        {
            var response = await _userService.RoleMasterGet();

            if (response == null)
                return BadRequest(new { message = "Bad Request" });


            return Ok(response);

            //return Ok(response);
        }

        [Authorize]
        [HttpGet("getDynamicMenuData")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetDynamicMenuData(string RoleId)
        {
            var response = await _userService.MenuSideBarGet(RoleId);

            if (response == null)
                return BadRequest(new { message = "Bad Request" });
            return Ok(response);

        }


        [HttpGet("verifyemail")]
        public async Task<IActionResult> VerifyEmail(string RoleId)
        {
            var response = await _userService.RoleMasterGet();

            if (response == null)
                return BadRequest(new { message = "Bad Request" });


            return Ok(response);

            //return Ok(response);
        }

        [HttpGet("masterUsersGET")]
        public async Task<IActionResult> MasterUsersGET(string startIndex, string pageSize, string searchBy, string searchCriteria)
        {
            try
            {
                PaginationRequest model = new PaginationRequest();
                model.StartIndex = startIndex;
                model.PageSize = pageSize;
                model.SearchBy = searchBy;
                model.SearchCriteria = searchCriteria;



                DataTable response = await _userService.MasterUsersGET(model);

                var lst = response.AsEnumerable()
                       .Select(r => r.Table.Columns.Cast<DataColumn>()
                       .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                    ).ToDictionary(z => z.Key, z => z.Value)
                 ).ToList();

                return Ok(lst);
            }
            catch (System.Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("allDropdown")]
        public async Task<List<DropdownOption>> allDropdown(string searchTerm, int page, int type, int parentId)
        {

            if (searchTerm == null)
                searchTerm = "";


            int pageSize = 500; // Set the desired page size

            int offset = (page - 1) * pageSize;
            if (offset <= 0)
                offset = 1;


            var response = await _userService.AllDropdown(searchTerm, offset, pageSize, type, parentId);
            return response;
        }
    }
}
