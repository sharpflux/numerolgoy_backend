using Microsoft.Extensions.Options;
using numerology_backend.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System.Data;
using System.Threading.Tasks;
using System;

namespace numerology_backend.Services
{
    public interface IEssenceService
    {
        Task<string> EssenceMasterInsertUpdate(EssenceRequest model);
        Task<DataTable> EssenceGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class EssenceService: IEssenceService
    {
    
        private readonly AppSettings _appSettings;
        private readonly EssenceDBHelper _sqlDBHelper;
        public EssenceService(IOptions<AppSettings> appSettings, EssenceDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<string> EssenceMasterInsertUpdate(EssenceRequest model)
        {
            try
            {
                return await _sqlDBHelper.EssenceMasterInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> EssenceGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.EssenceGET(new PaginationRequest
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
