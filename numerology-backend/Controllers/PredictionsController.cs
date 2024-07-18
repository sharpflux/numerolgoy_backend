using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using numerology_backend.Models;
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
    public class PredictionsController : ControllerBase
    {
        private IPredictionService _service;

        public PredictionsController(IPredictionService service)
        {
            _service = service;
        }


        [HttpPost("predictionsave")]
        public async Task<IActionResult> Predictionsave(PredictionRequest model)
        {

         
            var response = await _service.PredictionMasterInsertUpdate(model);
            if (response == null)
                return BadRequest(new { message = "Bad Request" });
            return Ok(response);



        }
        [HttpPost("mindPredictionNumbersave")]
        public async Task<IActionResult> MindPredictionNumbersave(PredictionRequest model)
        {


            var response = await _service.MindPredictionNumbersave(model);
            if (response == null)
                return BadRequest(new { message = "Bad Request" });
            return Ok(response);



        }

        [HttpPost("subpredictionsave")]
        public async Task<IActionResult> SubPredictionsave(PredictionRequest model)
        {
            var response = await _service.SubPredictionMasterInsertUpdate(model);
            if (response == null)
                return BadRequest(new { message = "Bad Request" });
            return Ok(response);
        }
        [HttpGet("PredictionsGET")]
        public async Task<IActionResult> PredictionsGET(string startIndex, string pageSize, string searchBy, string searchCriteria, string tableDataJsons)
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
                DataTable response = await _service.PredictionsGET(model, idString);
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

        [HttpGet("PredictionsSubGET")]
        public async Task<IActionResult> PredictionsSubGET(string startIndex, string pageSize, string searchBy, string searchCriteria, string tableDataJsons)
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
                DataTable response = await _service.PredictionsSubGET(model, idString);
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

        [HttpGet("mindNumberPredictionGET")]
        public async Task<IActionResult> MindNumberPredictionGET(string startIndex, string pageSize, string searchBy, string searchCriteria, string tableDataJsons)
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
                DataTable response = await _service.MindNumberPredictionGET(model, idString);
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
        [HttpPost("deletePredictionFromTable")]
        public async Task<IActionResult> ALLInOneDeleteORInactiveTables(DeleteModelRequest model)
        {
            var response = await _service.ALLInOneDeleteORInactiveTables(model);

            if (response == false)
                return BadRequest(new { message = "Error" });

            return Ok(response);
        }
        [HttpGet("PredictionPlanetsGET")]
        public async Task<IActionResult> PredictionPlanetsGET(string BirthDate, string Gender, string Client_id)
        {

            try
            {

                DataTable response = await _service.PredictionPlanetsGET(BirthDate, Gender, Client_id);
                var lst = response.AsEnumerable()
                       .Select(r => r.Table.Columns.Cast<DataColumn>()
                       .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                    ).ToDictionary(z => z.Key, z => z.Value)
                 ).ToList();

                return Ok(lst);

            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        [HttpPost("saveDataPredictionReport")]
        public async Task<IActionResult> saveDataPredictionReport(ReportSave model)
        {


            var response = await _service.saveDataPredictionReport(model);
            if (response == null)
                return BadRequest(new { message = "Bad Request" });
            return Ok(response);



        }
    }
}
