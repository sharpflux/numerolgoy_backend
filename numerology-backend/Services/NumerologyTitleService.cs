using Microsoft.Extensions.Options;
using NumerologystSolution.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NumerologystSolution.Services
{

    public interface INumerologyTitleService
    {
        Task<bool> NumerologyTitlesaveInsertUpdate(NumerologyTitleRequest model);
        Task<DataTable> NumerologyTitleGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class NumerologyTitleService : INumerologyTitleService
    {
        private readonly AppSettings _appSettings;
        private readonly NumerologyTitleDBHelper _sqlDBHelper;
        public NumerologyTitleService(IOptions<AppSettings> appSettings, NumerologyTitleDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<bool> NumerologyTitlesaveInsertUpdate(NumerologyTitleRequest model)
        {
            try
            {
                return await _sqlDBHelper.NumerologyTitlesaveInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> NumerologyTitleGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.NumerologyTitleGET(new PaginationRequest
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
