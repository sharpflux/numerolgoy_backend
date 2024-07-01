using Microsoft.Extensions.Options;
using NumerologystSolution.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System.Data;
using System.Threading.Tasks;
using System;

namespace NumerologystSolution.Services
{

    public interface ILuckyUnluckyNumbersService
    {
        Task<string> LuckyUnluckyNumbersSave(LuckyUnluckyRequest model);
        Task<DataTable> LuckyUnLuckyGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class LuckyUnluckyService: ILuckyUnluckyNumbersService
    {
        private readonly AppSettings _appSettings;
        private readonly LuckyUnluckyDBHelper _sqlDBHelper;
        public LuckyUnluckyService(IOptions<AppSettings> appSettings, LuckyUnluckyDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<string> LuckyUnluckyNumbersSave(LuckyUnluckyRequest model)
        {
            try
            {
                return await _sqlDBHelper.LuckyUnluckyNumbersSave(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> LuckyUnLuckyGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.LuckyUnLuckyGET(new PaginationRequest
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
