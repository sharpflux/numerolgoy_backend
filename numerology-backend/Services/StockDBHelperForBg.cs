using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace OmsSolution.Services
{
    public class StockDBHelperForBg
    {
        public async Task<DataTable> FGInspectionGetLastId(string startIndex)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Param",startIndex),

            };
            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("dbo.FGInspectionGetLastId", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<DataTable> FGInspectionGetDataForOms(string StartRecordId)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@StartRecordId",StartRecordId),

            };
            using (DataTable dt = await ImsSQLDBHelper.ExecuteDataTableWithParametersAsync("dbo.FGInspectionGetDataForOms", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<bool> InsertFGInspectionData(DataTable table)
        {
            return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQueryWithTableParameter("dbo.InsertFGInspectionData", CommandType.StoredProcedure, table, "@FGInspectionData", "dbo.FGInspectionType"));
        }



        //pRODUCT
        public async Task<DataTable> ProductsGetLastId(string startIndex)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Param",startIndex),

            };
            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("dbo.ProductsGetLastId", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
        public async Task<DataTable> ProductMasterGetForOms(string StartIndex)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@StartIndex",StartIndex),

            };
            using (DataTable dt = await ImsSQLDBHelper.ExecuteDataTableWithParametersAsync("dbo.ProductMasterGetForOms", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }



    }
}
