using Microsoft.Extensions.Options;
using numerology_backend.Models;
using NumerologystSolution.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NumerologystSolution.Services
{

    public interface IPredictionService
    {
        Task<bool> PredictionMasterInsertUpdate(PredictionRequest model);
        Task<bool> saveDataPredictionReport(ReportSave model);
        Task<bool> MindPredictionNumbersave(PredictionRequest model);
        Task<bool> SubPredictionMasterInsertUpdate(PredictionRequest model);
        Task<DataTable> PredictionsGET(PaginationRequest model, string idstring);
        Task<DataTable> PredictionsSubGET(PaginationRequest model, string idstring);
        Task<DataTable> MindNumberPredictionGET(PaginationRequest model, string idstring);
        Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct);
        Task<DataTable> PredictionPlanetsGET(string BirthDate, string Gender);
    }
    public class PredictionService: IPredictionService
    {
        private readonly AppSettings _appSettings;
        private readonly PredictionDBHelper _sqlDBHelper;
        public PredictionService(IOptions<AppSettings> appSettings, PredictionDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<bool> PredictionMasterInsertUpdate(PredictionRequest model)
        {
            try
            {
                return await _sqlDBHelper.PredictionMasterInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> saveDataPredictionReport(ReportSave model)
        {
            try
            {
                return await _sqlDBHelper.saveDataPredictionReport(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> MindPredictionNumbersave(PredictionRequest model)
        {
            try
            {
                return await _sqlDBHelper.MindPredictionNumberInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> SubPredictionMasterInsertUpdate(PredictionRequest model)
        {
            try
            {
                return await _sqlDBHelper.SubPredictionMasterInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> PredictionsGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.PredictionsGET(new PaginationRequest
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

        public async Task<DataTable> PredictionsSubGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.PredictionsSubGET(new PaginationRequest
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
        public async Task<DataTable> MindNumberPredictionGET(PaginationRequest model, string idString)
        {
            // Call the UserAuthentications method and pass the necessary parameters
            try
            {
                var dataTable = await _sqlDBHelper.MindNumberPredictionGET(new PaginationRequest
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
        public async Task<DataTable> PredictionPlanetsGET(string BirthDate, string Gender)
        {
            try
            {
                return await _sqlDBHelper.PredictionPlanetsGET(BirthDate, Gender);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
