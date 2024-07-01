using BAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace OmsSolution.Controllers
{



    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
 
    public class LoginController : ControllerBase
    {
        private static readonly List<PersonResource> _people = new List<PersonResource>();

        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }



        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



        [HttpPost]
        public IActionResult Post([FromBody] LoginBAL com)
        {
            //if (person.Role == PersonRole.Owner && _people.Any(p => p.Role == PersonRole.Owner))
            //{
            //	return BadRequest("Only one owner is allowed");
            //}
            //_people.Add(person);

            return Ok();
        }

        [Route("api/LoginApi/Authontication")]
        [HttpPost]
        public IActionResult Authontication([FromBody] LoginBAL com)
        {
            List<LoginBAL> lst = new List<LoginBAL>();
            try
            {
                if (com != null)
                {
                    lst = com.UserAuthenticationsGet();
                    com.error = false;
                    com.message = "Success";
                }
                else
                {
                    com.message = "NULL";
                    com.error = false;
                    com.UserId = 0;
                }
            }
            catch (Exception e)
            {
                com.UserId = 0;
                com.error = true;
                com.message = e.Message;
            }

            if (lst.Count > 0)
            {
                return Ok(lst);
            }
            else
            {
                return NotFound();
            }




        }

        [Route("api/LoginApi/getUserByToken")]
        [HttpGet]

        public IActionResult getUserByToken(string userToken)
        {
            List<LoginBAL> lst = new List<LoginBAL>();
            LoginBAL bal = new LoginBAL();
            try
            {
                if (userToken != null)
                {
                    bal.accessToken = userToken;
                    lst = bal.UserAuthenticationsByToken();
                    bal.error = false;
                    bal.message = "Success";
                }
                else
                {
                    bal.message = "NULL";
                    bal.error = false;
                    bal.UserId = 0;
                }
            }
            catch (Exception e)
            {
                bal.UserId = 0;
                bal.error = true;
                bal.message = e.Message;
            }

            if (lst.Count > 0)
            {
                return Ok(lst);
            }
            else
            {
                return NotFound();
            }


        }



        [Route("api/LoginController/DynamicMenuAsideGET")]
        [HttpGet]
        public IActionResult DynamicMenuAside(string RoleId)
        {
            MenuBAL MsBAL = new MenuBAL();
            List<MenuBAL> lst = new List<MenuBAL>();
            MsBAL.RoleId = Convert.ToInt32(RoleId);
            try
            {
                lst = MsBAL.DynamicMenuAside();
                MsBAL.error = false;
                MsBAL.message = "Success";

            }
            catch (Exception e)
            {
                MsBAL.error = true;
                MsBAL.message = e.Message;
            }


            if (lst.Count > 0)
            {
                return Ok(lst);
            }
            else
            {
                return NotFound();
            }


        }

    }
}
public class PersonResource
{

    public Guid? Id { get; }

    public string Name { get; }

    public PersonRole? Role { get; }
}
public enum PersonRole
{
    Owner,
    Admin,
    User
}

public class MyModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Age { get; set; }
    [Required]
    public string IsActive { get; set; }
}