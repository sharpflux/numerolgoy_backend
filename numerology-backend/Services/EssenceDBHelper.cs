using numerology_backend.Models;
using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace numerology_backend.Services
{
    public class EssenceDBHelper
    {
        public async Task<string> EssenceMasterInsertUpdate(EssenceRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {


                new SqlParameter("@Essence_Id",ObjStruct.Essence_Id),
                new SqlParameter("@Essence_Name",ObjStruct.Essence_Name),
                new SqlParameter("@Essence_Description",ObjStruct.Essence_Description),
                new SqlParameter("@Essence_No",ObjStruct.Essence_No),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                new SqlParameter("@NewNo", SqlDbType.VarChar,30),

                };
            Parameters[5].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[EssenceMasterInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[5].Value.ToString();

        }
        public async Task<DataTable> EssenceGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("EssenceMasterGET", CommandType.StoredProcedure, Parameters))
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
