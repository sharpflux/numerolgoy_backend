using iTextSharp.text.pdf;
using Microsoft.Extensions.Options;
using numerology_backend.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace numerology_backend.Services
{

    public interface ILifePhaseService
    {
        Task<string> LifePhasesPredictionsInsertUpdate(LifeServiceRequest model);
        Task<DataTable> LifePhasesPredictionsGET(PaginationRequest model);
    }
    public class LifePhaseService: ILifePhaseService
    {
        private readonly AppSettings _appSettings;
        private readonly LifePhaseDBHelper _sqlDBHelper;
        public LifePhaseService(IOptions<AppSettings> appSettings, LifePhaseDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

        public async Task<DataTable> LifePhasesPredictionsGET(PaginationRequest model)
        {
            try
            {
                var dataTable = await _sqlDBHelper.LifePhasesPredictionsGET(model);

               return dataTable;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<string> LifePhasesPredictionsInsertUpdate(LifeServiceRequest model)
        {
            try
            {
                return await _sqlDBHelper.LifePhasesPredictionsInsertUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
