using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using NumerologystSolution.Models;

namespace NumerologystSolution.Services
{
    public class ClientDBHelper
    {
        public async Task<bool> ClientMasterInsertUpdate(ClientRequest ObjStruct)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
              {
                new SqlParameter("@Client_id",ObjStruct.Client_id),
                new SqlParameter("@First_Name",ObjStruct.firstName),
                new SqlParameter("@Middle_Name",ObjStruct.middleName),
                new SqlParameter("@Last_Name",ObjStruct.lastName),
                new SqlParameter("@DOB",ObjStruct.DateOfBirth),
                new SqlParameter("@Gender",ObjStruct.Gender),
                new SqlParameter("@Mobile_No1",ObjStruct.mobileNo1),
                new SqlParameter("@Mobile_No2",ObjStruct.mobileNo2),
                new SqlParameter("@Mobile_No3",ObjStruct.mobileNo3),
                new SqlParameter("@Vechile_No1",ObjStruct.vechileNo1),
                new SqlParameter("@Vechile_No2",ObjStruct.vechileNo2),
                new SqlParameter("@Vechile_No3",ObjStruct.vechileNo3),
                new SqlParameter("@House_No1",ObjStruct.houseNo1),
                new SqlParameter("@House_No2",ObjStruct.houseNo2),
                new SqlParameter("@House_No3",ObjStruct.houseNo3),
                new SqlParameter("@Email_Id",ObjStruct.emailId),
                new SqlParameter("@IsActive",ObjStruct.IsActive)

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("ClientMasterInsertUpdate", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<DataTable> ClientGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[ClientMasterGET]", CommandType.StoredProcedure, Parameters))
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
        public async Task<DataTable> ClientGetById(string Client_id)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Client_id",Client_id),
            };
            return await SqlDBHelper.ExecuteDataTableWithParametersAsync("dbo.[ClientGetById]", CommandType.StoredProcedure, Parameters);

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
