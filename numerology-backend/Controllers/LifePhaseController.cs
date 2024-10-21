using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using numerology_backend.Models;
using numerology_backend.Services;
using OmsSolution.Entities;
using OmsSolution.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace numerology_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LifePhaseController : ControllerBase
    {
        private ILifePhaseService _service;

        public LifePhaseController(ILifePhaseService service)
        {
            _service = service;
        }

        [HttpPost("lifePhasesPredictionsInsertUpdate")]
        public async Task<IActionResult> LifePhasesPredictionsInsertUpdate(LifeServiceRequest model)
        {
            var response = await _service.LifePhasesPredictionsInsertUpdate(model);
            if (response == null)
                return BadRequest(new { message = "Bad Request" });
            return Ok(response);
        }

        [HttpGet("LifePhasesPredictionsGET")]
        public async Task<IActionResult> LifePhasesPredictionsGET(string startIndex, string pageSize, string searchBy, string searchCriteria)
        {
            try
            {
                PaginationRequest model = new PaginationRequest();
                model.StartIndex = startIndex;
                model.PageSize = pageSize;
                model.SearchBy = searchBy;
                model.SearchCriteria = searchCriteria;

                if (!string.IsNullOrWhiteSpace(searchCriteria) && searchCriteria != "undefined")
                {
                    model.SearchBy = "1"; // Set searchBy to '1'
                }
                else
                {
                    model.SearchBy = "0"; // Set searchBy to '0'
                }
 
                DataTable response = await _service.LifePhasesPredictionsGET(model);
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

    }
}
