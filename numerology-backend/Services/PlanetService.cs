using Microsoft.Extensions.Options;
using NumerologystSolution.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using Service;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NumerologystSolution.Services
{

    public interface IPlanetService
    {
        Task<string> PlanetMasterInsertUpdate(PlanetRequest model);
        Task<DataTable> PlanetGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class PlanetService : IPlanetService
    {
        private readonly AppSettings _appSettings;
        private readonly PlanetDBHelper _sqlDBHelper;
        public PlanetService(IOptions<AppSettings> appSettings, PlanetDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<string> PlanetMasterInsertUpdate(PlanetRequest model)
        {
            try
            {
                return await _sqlDBHelper.PlanetMasterInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> PlanetGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.PlanetGET(new PaginationRequest
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
