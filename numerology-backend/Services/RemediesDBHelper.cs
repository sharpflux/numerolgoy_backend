using NumerologystSolution.Models;
using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace NumerologystSolution.Services
{
    public class RemediesDBHelper
    {
        public async Task<bool> RemediesMasterInsertUpdate(RemediesRequest ObjStruct)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
              {
                new SqlParameter("@Remedies_Id",ObjStruct.Remedies_Id),
                new SqlParameter("@Remedies_No",ObjStruct.remediesNo),
                new SqlParameter("@Remedies_Title",ObjStruct.remediesTitle),
                new SqlParameter("@Remedies_Description",ObjStruct.remediesDescription),
                new SqlParameter("@IsActive",ObjStruct.IsActive)

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("[RemediesMasterInsertUpdate]", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<DataTable> RemediesGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[RemediestMasterGET]", CommandType.StoredProcedure, Parameters))
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