using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace numerology_backend.Services
{
    public class DOBCalculateDBHelper
    {
        public async Task<DataTable> DOBCalculateGET(string DateOfBirth)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@dob",DateOfBirth),

                };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[DOBCalculateGET]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }


        }
    }
}
