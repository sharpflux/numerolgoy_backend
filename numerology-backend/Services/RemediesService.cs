using Microsoft.Extensions.Options;
using NumerologystSolution.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System.Data;
using System.Threading.Tasks;
using System;

namespace NumerologystSolution.Services
{

    public interface IRemediesService
    {
        Task<bool> RemediesMasterInsertUpdate(RemediesRequest model);
        Task<DataTable> RemediesGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class RemediesService: IRemediesService
    {
        private readonly AppSettings _appSettings;
        private readonly RemediesDBHelper _sqlDBHelper;
        public RemediesService(IOptions<AppSettings> appSettings, RemediesDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<bool> RemediesMasterInsertUpdate(RemediesRequest model)
        {
            try
            {
                return await _sqlDBHelper.RemediesMasterInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> RemediesGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.RemediesGET(new PaginationRequest
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