using NumerologystSolution.Models;
using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using numerology_backend.Models;

namespace numerology_backend.Services
{
    public class RepetitiveDBHelper
    {
        public async Task<string> RepetitiveNumbersSave(RepetitiveRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                 new SqlParameter("@RepeatPredictionId",ObjStruct.RepeatPredictionId),
                new SqlParameter("@RepeateId",ObjStruct.RepeateId),
                new SqlParameter("@Description",ObjStruct.Description),
                // new SqlParameter("@IsActive",ObjStruct.IsActive),
                  new SqlParameter("@NewNo", SqlDbType.VarChar,30),

                };
            Parameters[3].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[RepetitiveNumbersSaveInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[3].Value.ToString();

        }
        public async Task<DataTable> RepetitiveNumbersGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[RepetitiveNumbersGET]", CommandType.StoredProcedure, Parameters))
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
