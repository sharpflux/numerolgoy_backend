using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using numerology_backend.Services;
using NumerologystSolution.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
namespace numerology_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DOBCalculateController : ControllerBase
    {
        private IDOBCalculateService _service;

        public DOBCalculateController(IDOBCalculateService service)
        {
            _service = service;
        }
        [HttpGet("DOBCalculateGET")]
        public async Task<IActionResult> DOBCalculateGET(string DateOfBirth)
        {
            try
            {
                if (!DateTime.TryParse(DateOfBirth, out DateTime dob))
                {
                    return BadRequest(new { Error = "Invalid date format. Please provide a valid date." });
                }

       
                DataTable response = await _service.DOBCalculateGET(dob.ToString("yyyy-MM-dd"));
                var lst = response.AsEnumerable()
                       .Select(r => r.Table.Columns.Cast<DataColumn>()
                       .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                    ).ToDictionary(z => z.Key, z => z.Value)
                 ).ToList();

                return Ok(lst);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }



    }
}
