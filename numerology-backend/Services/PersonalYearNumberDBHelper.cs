using numerology_backend.Models;
using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace numerology_backend.Services
{
    public class PersonalYearNumberDBHelper
    {
        public async Task<string> PersonalYearNumbersSave(PersonalYearNumberRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@PersonalYearId",ObjStruct.PersonalYearId),
                new SqlParameter("@PersonalYearNumberId",ObjStruct.PersonalYearNumber),
                new SqlParameter("@PersonalYearNumber_Description",ObjStruct.Description),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                new SqlParameter("@NewNo", SqlDbType.VarChar,30),

                };
            Parameters[4].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[PersonalYearNumbersSaveInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[4].Value.ToString();

        }
        public async Task<DataTable> PersonalYearNumbersGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[PersonalYearNumbersGET]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
        public async Task<string> NameNumbersSave(PersonalYearNumberRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@NameNumberId",ObjStruct.NameNumberId),
                new SqlParameter("@NameNumbersID",ObjStruct.NameNumbersID),
                new SqlParameter("@NameNumber_Description",ObjStruct.NameNumber_Description),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                new SqlParameter("@NewNo", SqlDbType.VarChar,30),

                };
            Parameters[4].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[NameNumbersSaveInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[4].Value.ToString();

        }
        public async Task<DataTable> NameNumbersGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[NameNumbersGET]", CommandType.StoredProcedure, Parameters))
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
