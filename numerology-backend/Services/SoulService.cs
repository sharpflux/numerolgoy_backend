using Microsoft.Extensions.Options;
using NumerologystSolution.Models;
using NumerologystSolution.Services;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System.Data;
using System.Threading.Tasks;
using System;
using numerology_backend.Models;

namespace numerology_backend.Services
{
    public interface ISoulService
    {
        Task<string> SoulMasterInsertUpdate(SoulRequest model);
        Task<DataTable> SoulGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class SoulService: ISoulService

    {
        private readonly AppSettings _appSettings;
        private readonly SoulDBHelper _sqlDBHelper;
        public SoulService(IOptions<AppSettings> appSettings, SoulDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<string> SoulMasterInsertUpdate(SoulRequest model)
        {
            try
            {
                return await _sqlDBHelper.SoulMasterInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> SoulGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.SoulGET(new PaginationRequest
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
