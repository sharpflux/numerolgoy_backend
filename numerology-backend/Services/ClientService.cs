using Microsoft.Extensions.Options;
using OmsSolution.Helpers;
using OmsSolution.Models;
using OmsSolution.Services;
using System.Threading.Tasks;
using System;
using NumerologystSolution.Models;
using System.Data;

namespace NumerologystSolution.Services
{

    public interface IClientService
    {
        Task<bool> ClientMasterInsertUpdate(ClientRequest model);
        Task<DataTable> ClientGetById(string ProductRentId);
        Task<DataTable> ClientGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class ClientService : IClientService
    {

        private readonly AppSettings _appSettings;
        private readonly ClientDBHelper _sqlDBHelper;
        public ClientService(IOptions<AppSettings> appSettings, ClientDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<bool> ClientMasterInsertUpdate(ClientRequest model)
        {
            try
            {
                return await _sqlDBHelper.ClientMasterInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> ClientGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.ClientGET(new PaginationRequest
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
        public async Task<DataTable> ClientGetById(string Client_id)
        {
            try
            {
                return await _sqlDBHelper.ClientGetById(Client_id);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
