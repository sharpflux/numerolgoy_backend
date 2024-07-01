﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NumerologystSolution.Models;
using NumerologystSolution.Services;
using OmsSolution.Entities;
using OmsSolution.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace NumerologystSolution.Controllers
{
    [EnableCors("DefaultCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class PlanetController : ControllerBase
    {
        private IPlanetService _service;

        public PlanetController(IPlanetService service)
        {
            _service = service;
        }


        [HttpPost("planetsave")]
        public async Task<IActionResult> planetsave(PlanetRequest model)
        {

            //try
            //{
              //  var data = _service.PlanetMasterInsertUpdate(model);
                var response = await _service.PlanetMasterInsertUpdate(model);
                if (response == null)
                    return BadRequest(new { message = "Bad Request" });
                return Ok(response);
            
            //catch (System.Exception)
            //{

            //    throw;
            //}
            //var response = await _userService.RoleMasterGet();

            //if (response == null)
            //    return BadRequest(new { message = "Bad Request" });


            //return Ok(response);


        }
        [HttpGet("planetGET")]
        public async Task<IActionResult> PlanetGET(string startIndex, string pageSize, string searchBy, string searchCriteria, string tableDataJsons)
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

                List<TableDataItem> tableData = JsonConvert.DeserializeObject<List<TableDataItem>>(tableDataJsons);
                List<string> ids = tableData.Select(item => item.id).ToList();
                string idString = string.Join(",", ids);
                DataTable response = await _service.PlanetGET(model, idString);
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
        //[Authorize]
        [HttpPost("deletePlanetFromTable")]
        public async Task<IActionResult> ALLInOneDeleteORInactiveTables(DeleteModelRequest model)
        {
            var response = await _service.ALLInOneDeleteORInactiveTables(model);

            if (response == false)
                return BadRequest(new { message = "Error" });

            return Ok(response);
        }
    }
}
