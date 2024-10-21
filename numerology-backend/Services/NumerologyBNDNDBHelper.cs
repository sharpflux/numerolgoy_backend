using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using NumerologystSolution.Models;
using System;
using System.Reflection;

namespace NumerologystSolution.Services
{
    public class NumerologyBNDNDBHelper
    {
        public async Task<DataTable> NumerologyBNDNGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[ClientMasterGET]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<DataTable> GetVowelSum(string inputName)
        {
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@inputName", inputName)
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[dbo].[GetVowelSum]", CommandType.StoredProcedure, parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<DataTable> CalculateLoShuGrid(string BirthDate, string Gender, string Client_id)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@BirthDate",BirthDate),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@Client_id",Client_id),

            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[CalculateLoShuGrid]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
        public async Task<bool> SaveNumerologyClientsDetails(ClientRequest ObjStruct)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
              {
                new SqlParameter("@ClientNumberID",ObjStruct.ClientNumberID),
                new SqlParameter("@Client_id",ObjStruct.Client_id),
                new SqlParameter("@First_Name",ObjStruct.First_Name),             
                new SqlParameter("@DOB",ObjStruct.DateOfBirth),
                new SqlParameter("@Vechile_No1",ObjStruct.vechileNo1),              
                new SqlParameter("@IsActive",ObjStruct.IsActive)

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("[NumerologyClientInsertUpdate]", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> GetCharacterValueAsync(string name)
        {
            string outputValue = string.Empty;

            // Define parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", name),
                new SqlParameter("@Output", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output }
            };

            // Execute the stored procedure using ExecuteNonQuery
            await SqlDBHelper.ExecuteNonQuery("[CalculateCharachterValue]", CommandType.StoredProcedure, parameters);

            // Retrieve the output parameter value
            outputValue = parameters[1].Value.ToString();

            return outputValue;
        }

    }
}