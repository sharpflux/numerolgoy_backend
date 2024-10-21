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
    public class SoulDBHelper
    {
        public async Task<string> SoulMasterInsertUpdate(SoulRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {


                new SqlParameter("@Soul_Id",ObjStruct.Soul_Id),
                new SqlParameter("@Soul_Name",ObjStruct.Soul_Name),
                new SqlParameter("@Soul_Description",ObjStruct.Soul_Description),
                new SqlParameter("@Soul_No",ObjStruct.Soul_No),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                new SqlParameter("@NewNo", SqlDbType.VarChar,30),

                };
            Parameters[5].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[SoulMasterInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[5].Value.ToString();

        }
        public async Task<DataTable> SoulGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("SoulMasterGET", CommandType.StoredProcedure, Parameters))
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
