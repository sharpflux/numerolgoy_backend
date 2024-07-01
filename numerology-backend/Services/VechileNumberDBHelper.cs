using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using NumerologystSolution.Models;

namespace NumerologystSolution.Services
{
    public class VechileNumberDBHelper
    {

        public async Task<string> CalculateVehicleNumber(string Vechile_No1)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@VehicleNumber",Vechile_No1),
               
                new SqlParameter("@Output", SqlDbType.VarChar,30),

                };
            Parameters[1].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[CalculateVehicleNumber]", CommandType.StoredProcedure, Parameters);
            return Parameters[1].Value.ToString();

        }

   
    }
}
