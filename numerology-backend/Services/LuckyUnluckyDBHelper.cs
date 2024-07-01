using NumerologystSolution.Models;
using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace NumerologystSolution.Services
{
    public class LuckyUnluckyDBHelper
    {
        public async Task<string> LuckyUnluckyNumbersSave(LuckyUnluckyRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {


                new SqlParameter("@LuckyUnluckyId",ObjStruct.luckyUnluckyId),
                new SqlParameter("@Prediction_Id",ObjStruct.predictionsubNumber),
                new SqlParameter("@Numbers",ObjStruct.commaSeparatedNumbers),
                new SqlParameter("@IsLucky",ObjStruct.LuckyUnluckySelection),
                 new SqlParameter("@IsActive",ObjStruct.IsActive),
                  new SqlParameter("@NewNo", SqlDbType.VarChar,30),

                };
            Parameters[5].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[LuckyUnluckyNumbersInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[5].Value.ToString();

        }
        public async Task<DataTable> LuckyUnLuckyGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("LuckyUnLuckyMasterGET", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
        public async Task<bool> ALLInOneDeleteORInactiveTables(DeleteModelRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@TypeId",ObjStruct.TypeId),
                new SqlParameter("@Id",ObjStruct.Id),
            };
            return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("dbo.ALLInOneDeleteORInactiveTables", CommandType.StoredProcedure, Parameters));
        }

    }
}
