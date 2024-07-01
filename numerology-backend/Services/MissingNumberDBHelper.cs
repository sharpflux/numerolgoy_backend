using NumerologystSolution.Models;
using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace NumerologystSolution.Services
{
    public class MissingNumberDBHelper
    {
        public async Task<bool> MissingNumberInsertUpdate(MissingNumberRequest ObjStruct)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
              {
                new SqlParameter("@MissingNo_Id",ObjStruct.missingNo_id),
                new SqlParameter("@MissingNumber",ObjStruct.missingNumber),
                new SqlParameter("@MissingName",ObjStruct.missingName),
                new SqlParameter("@MissingDescription",ObjStruct.missingDescription),            
                new SqlParameter("@IsActive",ObjStruct.IsActive)

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("[MissingNumberInsertUpdate]", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<DataTable> MissingnumberGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[MissingNumberGET]", CommandType.StoredProcedure, Parameters))
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
 
        public async Task<DataTable> AllDropdown(string searchTerm, int page, int pageSize, int type, int parentId)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@searchTerm",searchTerm),
                new SqlParameter("@page",page),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@type",type),
                new SqlParameter("@paretnid",parentId),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("AllDropdown", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
    }
}
