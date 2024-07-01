using Microsoft.Extensions.Options;
using NumerologystSolution.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System.Data;
using System.Threading.Tasks;
using System;

namespace NumerologystSolution.Services
{

    public interface IMissingNumberService
    {
        Task<bool> MissingNumberInsertUpdate(MissingNumberRequest model);
        Task<DataTable> MissingnumberGET(PaginationRequest model, string idString);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class MissingNumberService : IMissingNumberService
    {
        private readonly AppSettings _appSettings;
        private readonly MissingNumberDBHelper _sqlDBHelper;
        public MissingNumberService(IOptions<AppSettings> appSettings, MissingNumberDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<bool> MissingNumberInsertUpdate(MissingNumberRequest model)
        {
            try
            {
                return await _sqlDBHelper.MissingNumberInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
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

        public async Task<DataTable> MissingnumberGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.MissingnumberGET(new PaginationRequest
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
    }
}
