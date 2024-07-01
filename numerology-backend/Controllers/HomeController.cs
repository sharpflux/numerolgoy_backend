using BAL;
using OmsSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using OmsSolution.Utilities;
using Microsoft.AspNetCore.Http;

namespace OmsSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly EmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
          //  _emailSender = new EmailSender(_hostingEnvironment, _httpContextAccessor);
        }

        public IActionResult Index()
        {
         
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ContentResult GetMenus(int PrTypeId)
        {
            DataSet ds = new DataSet();
            try
            {

                MenuBAL bllMenu = new MenuBAL();
                bllMenu.RoleId = 1;

                ds = bllMenu.MasterModules_MenuJs();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].TableName = "MainPages";
                        ds.Tables[1].TableName = "Modules";
                        ds.Tables[2].TableName = "Pages";
                        ds.Tables[3].TableName = "Request";
                    }
                }
            }
            catch (Exception d)
            {

                throw d;
            }
            return Content(ds.GetXml().ToString());
        }
    }
}
