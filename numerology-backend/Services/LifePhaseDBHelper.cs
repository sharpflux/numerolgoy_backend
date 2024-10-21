using NumerologystSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using numerology_backend.Models;
using OmsSolution.Models;

namespace numerology_backend.Services
{
    public class LifePhaseDBHelper
    {
        public async Task<string> LifePhasesPredictionsInsertUpdate(LifeServiceRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@LifePhasePredictionId",ObjStruct.LifePhasePredictionId),
                new SqlParameter("@LifePhaseTypeId",ObjStruct.LifePhaseTypeId),
                new SqlParameter("@NumberId",ObjStruct.NumberId),
                new SqlParameter("@LifePhase_Description",ObjStruct.LifePhase_Description),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                new SqlParameter("@NewNo", SqlDbType.VarChar,30),
            };
            Parameters[5].Direction = ParameterDirection.Output;
            await SqlDBHelper.ExecuteNonQuery("[dbo].[LifePhasesPredictionsInsertUpdate]", CommandType.StoredProcedure, Parameters);
            return Parameters[5].Value.ToString();

        }
        public async Task<DataTable> LifePhasesPredictionsGET(PaginationRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria)
          
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[dbo].[LifePhasesPredictionsGET]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
    }
}
