using Microsoft.Extensions.Options;
using numerology_backend.Models;
using NumerologystSolution.Models;
using NumerologystSolution.Services;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace numerology_backend.Services
{
    public interface IRepetitiveService
    {
        Task<string> RepetitiveNumbersSave(RepetitiveRequest model);
        Task<DataTable> RepetitiveNumbersGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class RepetitiveService : IRepetitiveService
    {
        private readonly AppSettings _appSettings;
        private readonly RepetitiveDBHelper _sqlDBHelper;
        public RepetitiveService(IOptions<AppSettings> appSettings, RepetitiveDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<string> RepetitiveNumbersSave(RepetitiveRequest model)
        {
            try
            {
                return await _sqlDBHelper.RepetitiveNumbersSave(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> RepetitiveNumbersGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.RepetitiveNumbersGET(new PaginationRequest
                {
                    StartIndex = model.StartIndex,
                    PageSize = model.PageSize,
                    SearchBy = model.SearchBy,
                    SearchCriteria = model.SearchCriteria
                }, idString);

                return dataTable;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct)
        {
            try
            {
                bool dataTable = await _sqlDBHelper.ALLInOneDeleteORInactiveTables(ObjStruct);

                return dataTable;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
