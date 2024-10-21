using Microsoft.Extensions.Options;
using numerology_backend.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace numerology_backend.Services
{
    public interface IPersonlaYearNumberService
    {
        Task<string> PersonalYearNumbersSave(PersonalYearNumberRequest model);
        Task<string> NameNumbersSave(PersonalYearNumberRequest model);
        Task<DataTable> PersonalYearNumbersGET(PaginationRequest model, string idstring);
        Task<DataTable> NameNumbersGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
    }
    public class PersonalYearNumberService: IPersonlaYearNumberService
    {
        private readonly AppSettings _appSettings;
        private readonly PersonalYearNumberDBHelper _sqlDBHelper;
        public PersonalYearNumberService(IOptions<AppSettings> appSettings, PersonalYearNumberDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<string> PersonalYearNumbersSave(PersonalYearNumberRequest model)
        {
            try
            {
                return await _sqlDBHelper.PersonalYearNumbersSave(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> NameNumbersSave(PersonalYearNumberRequest model)
        {
            try
            {
                return await _sqlDBHelper.NameNumbersSave(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> PersonalYearNumbersGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.PersonalYearNumbersGET(new PaginationRequest
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
        public async Task<DataTable> NameNumbersGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.NameNumbersGET(new PaginationRequest
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
