using Microsoft.Extensions.Options;
using NumerologystSolution.Services;
using OmsSolution.Helpers;
using System.Threading.Tasks;
using System;
using System.Data;

namespace numerology_backend.Services
{
    public interface IDOBCalculateService
    {

        Task<DataTable> DOBCalculateGET(string DateOfBirth);



    }
    public class DOBCalculateService: IDOBCalculateService
    {
        private readonly AppSettings _appSettings;
        private readonly DOBCalculateDBHelper _sqlDBHelper;
        public DOBCalculateService(IOptions<AppSettings> appSettings, DOBCalculateDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }




        public async Task<DataTable> DOBCalculateGET(string DateOfBirth)
        {
            try
            {
                return await _sqlDBHelper.DOBCalculateGET(DateOfBirth);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
