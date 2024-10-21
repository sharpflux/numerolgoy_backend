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


    public interface INumerologyBNDNService
    {
        Task<bool> SaveNumerologyClientsDetails(ClientRequest model);
        Task<DataTable> NumerologyBNDNGET(PaginationRequest model, string idstring);
        Task<DataTable> CalculateLoShuGrid(string BirthDate, string Gender ,string Client_id);

        Task<string> GetCharacterValueAsync(string name);
        Task<DataTable> GetVowelSum(string inputName);

        }
    public class NumerologyBNDNService : INumerologyBNDNService
    {
        private readonly AppSettings _appSettings;
        private readonly NumerologyBNDNDBHelper _sqlDBHelper;
        public NumerologyBNDNService(IOptions<AppSettings> appSettings, NumerologyBNDNDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<bool> SaveNumerologyClientsDetails(ClientRequest model)
        {
            try
            {
                return await _sqlDBHelper.SaveNumerologyClientsDetails(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> NumerologyBNDNGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.NumerologyBNDNGET(new PaginationRequest
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

        public async Task<DataTable> CalculateLoShuGrid(string BirthDate, string Gender, string Client_id)
        {
            try
            {
                return await _sqlDBHelper.CalculateLoShuGrid(BirthDate,Gender, Client_id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> GetCharacterValueAsync(string name)
        {
            try
            {
                return await _sqlDBHelper.GetCharacterValueAsync(name);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<DataTable> GetVowelSum(string inputName)
        {
            try
            {
                return await _sqlDBHelper.GetVowelSum(inputName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}