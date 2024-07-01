using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NumerologystSolution.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NumerologystSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VechileNumberController : ControllerBase
    {
        private IVechileNumberService _service;

        public VechileNumberController(IVechileNumberService service)
        {
            _service = service;
        }
        [HttpGet("vechileNumberGET")]
        public async Task<IActionResult> VechileNumberGET(string Vechile_No1)
        {
            try
            {
                string response = await _service.CalculateVehicleNumber(Vechile_No1);
                var jsonResponse = new { Result = response };
                return Ok(jsonResponse);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

    }
}
