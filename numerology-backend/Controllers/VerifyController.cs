using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services;

namespace OmsSolution.Controllers
{
    public class VerifyController : Controller
    {
        private readonly IDataProtector _dataProtector;
        private IUserService _userService;
        public VerifyController(IDataProtectionProvider dataProtectionProvider, IUserService userService)
        {
            _userService = userService;
            _dataProtector = dataProtectionProvider.CreateProtector("EmailProtection");
        }

        [HttpGet]
        public IActionResult Index(string email)
        {
            TempData["dataKey"] = email;
            return View();
        }
        public IActionResult NotActivate()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Verify()
        {
            var data = TempData["dataKey"];
            string decryptedEmail = string.Empty;

            if (data != null) { 
                decryptedEmail = _dataProtector.Unprotect(data.ToString());
                var response = await _userService.VerifyEmail(decryptedEmail);
                if (response == true)
                {
                    string externalUrl = "http://localhost:4200/auth/logins";
                    return Redirect(externalUrl);
                }
                else
                {
                    return RedirectToAction("NotActivate");
                }
      
            }

            return RedirectToAction("Index");
        }
    }
}
