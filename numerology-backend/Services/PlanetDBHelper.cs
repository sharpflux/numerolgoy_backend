using System.Data.SqlClient;
using System.Data;
using NumerologystSolution.Models;
using System.Threading.Tasks;
using Service;
using System;
using OmsSolution.Models;

namespace NumerologystSolution.Services
{
    public class PlanetDBHelper
    { 
      public async Task<string> PlanetMasterInsertUpdate(PlanetRequest ObjStruct)
      {
        SqlParameter[] Parameters = new SqlParameter[]
        {


                new SqlParameter("@Planet_Id",ObjStruct.Planet_id),
                new SqlParameter("@Planet_Name",ObjStruct.planetName),
                new SqlParameter("@Planet_Description",ObjStruct.planetDescription),
                new SqlParameter("@Planet_Number",ObjStruct.planetNumber),         
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                new SqlParameter("@NewNo", SqlDbType.VarChar,30),

            };
            Parameters[5].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[PlanetMasterInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[5].Value.ToString();

        }
        public async Task<DataTable> PlanetGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("PlanetMasterGET", CommandType.StoredProcedure, Parameters))
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
