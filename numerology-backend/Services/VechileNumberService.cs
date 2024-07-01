using Microsoft.Extensions.Options;
using NumerologystSolution.Models;
using OmsSolution.Helpers;
using OmsSolution.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NumerologystSolution.Services
{
    public interface IVechileNumberService
    {

        Task<string> CalculateVehicleNumber( string Vechile_No1);



    }
    public class VechileNumberService: IVechileNumberService
    {
        private readonly AppSettings _appSettings;
        private readonly VechileNumberDBHelper _sqlDBHelper;
        public VechileNumberService(IOptions<AppSettings> appSettings, VechileNumberDBHelper sqlDBHelper)
        {
            _appSettings = appSettings.Value;
            _sqlDBHelper = sqlDBHelper;
        }

      
  

        public async Task<string> CalculateVehicleNumber( string Vechile_No1)
        {
            try
            {
                return await _sqlDBHelper.CalculateVehicleNumber(Vechile_No1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
