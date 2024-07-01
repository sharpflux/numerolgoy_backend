
using OmsSolution.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Services.MenuDAL;

namespace Services
{
    public class LoginDAL
    {
        public struct LoginStruct
        {
            public int id;
            public int UserId;
            public int CompanyId;
            public int PlatId;
            public int DepartmentId;
            public string UserName;
            public string Passwords;
            public string FirstName;
            public string LastName;
            public string EmailId;
            public string MobileNo;
            public string LastLogin;
            public string ipAddress;
            public string ImageUrl;
            public string fullname;
            public string pic;
            public string accessToken;
            public string refreshToken;
            public int RoleId;
            public object reportingTo;

            public int PageIndex;
            public int PageSize;


            public string message;
            public bool error;

            public int SearchBy;
            public string SearchCriteria;
            public object MiddleName;
            public bool IsActive;
            public int RequestedCode;
            public List<int> roles;
        }
        LoginStruct structObj = new LoginStruct();

        public LoginStruct StuructAll
        {
            get
            {
                return structObj;
            }
            set
            {
                structObj = value;
            }
        }
        public DataTable MasterDepartmentsGET(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@CompanyId",ObjStruct.CompanyId),
                new SqlParameter("@PlantId",ObjStruct.PlatId),

            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterDepartmentsGET", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataSet MasterUsersDropdowns(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@CompanyId",ObjStruct.CompanyId),
            };
            using (DataSet dt = SqlDBHelperOld.ExecuteWithParametersDataSet("MasterUsersDropdowns", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }




        public DataTable MasterUOMGetForDropDown(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@temp",ObjStruct.CompanyId),

            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterUOMGetForDropDown", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }
        public DataTable MasterUsers_Get_User_By_Id(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId",ObjStruct.UserId),

            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterUsers_Get_User_By_Id", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public bool CheckUserName(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName",ObjStruct.UserName),
                new SqlParameter("@Result", DbType.Boolean),

            };
            Parameters[1].Direction = ParameterDirection.Output;

            SqlDBHelperOld.ExecuteNonQuery("CheckUserName", CommandType.StoredProcedure, Parameters);

            if (Parameters[1].Value.ToString() == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable CheckUserEmail(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@EmailId",ObjStruct.UserName),

            };

            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("CheckUserEmail", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }

        }

        public DataTable RequestChangePasswordCheckExpiredOrNot(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@RequestCode",ObjStruct.RequestedCode),
             };

            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("RequestChangePasswordCheckExpiredOrNot", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataTable MasterUsersGetData(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@StartIndex",ObjStruct.PageIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterUsersGetDataDynamic", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataTable MasterPlant_Get(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@CompanyId",ObjStruct.CompanyId),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterPlant_Get", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataTable MasterUsersDropdownsWebMethod(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@CompanyId",ObjStruct.CompanyId),
                new SqlParameter("@PlantId",ObjStruct.PlatId),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterUsersDropdownsWebMethod", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataTable MasterUserReportingToWebMethod(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@CompanyId",ObjStruct.CompanyId),
                new SqlParameter("@PlantId",ObjStruct.PlatId),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterUserReportingToWebMethod", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataTable MasterUsersGetReportDataByDepartment(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@PlantId",ObjStruct.PlatId),
                new SqlParameter("@DepartmentId",ObjStruct.DepartmentId),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterUsersGetReportDataByDepartment", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }


        public DataTable MasterUsersGetDataById(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                 new SqlParameter("@UserId",ObjStruct.UserId),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MasterUsersGetDataById", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public bool MasterUsersInsertUpdate(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@CompanyId",ObjStruct.CompanyId),
                new SqlParameter("@RoleId",ObjStruct.RoleId),
                new SqlParameter("@UserName",ObjStruct.UserName),
                new SqlParameter("@Passwords",SqlDBHelperOld.Encrypt_Decrypt.Encrypt(ObjStruct.Passwords,SqlDBHelperOld.EncKey)),
                new SqlParameter("@FirstName",ObjStruct.FirstName),
                new SqlParameter("@MiddleName",ObjStruct.MiddleName),
                new SqlParameter("@LastName",ObjStruct.LastName),
                new SqlParameter("@EmailId",ObjStruct.EmailId),
                new SqlParameter("@MobileNo",ObjStruct.MobileNo),
                new SqlParameter("@ImageUrl",ObjStruct.ImageUrl),
                new SqlParameter("@IsActive",ObjStruct.IsActive),

            };
            return SqlDBHelperOld.ExecuteNonQuery("MasterUsersInsertUpdate", CommandType.StoredProcedure, Parameters);
        }

        public bool MasterUsersChangePassword(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@Passwords",SqlDBHelperOld.Encrypt_Decrypt.Encrypt(ObjStruct.Passwords,SqlDBHelperOld.EncKey)),

            };
            return SqlDBHelperOld.ExecuteNonQuery("MasterUsersChangePassword", CommandType.StoredProcedure, Parameters);
        }
        public bool MasterUsersChangePasswordWithCode(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@Passwords",SqlDBHelperOld.Encrypt_Decrypt.Encrypt(ObjStruct.Passwords,SqlDBHelperOld.EncKey)),
                new SqlParameter("@Code",ObjStruct.RequestedCode),

            };
            return SqlDBHelperOld.ExecuteNonQuery("MasterUsersChangePasswordWithCode", CommandType.StoredProcedure, Parameters);
        }

        public bool MasterUsersChangeImage(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@ImageURL",ObjStruct.ImageUrl),

            };
            return SqlDBHelperOld.ExecuteNonQuery("MasterUsersChangeImage", CommandType.StoredProcedure, Parameters);
        }



        public bool MasterUsers_Delete(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@UserId",ObjStruct.UserId),

            };
            return SqlDBHelperOld.ExecuteNonQuery("MasterUsers_Delete", CommandType.StoredProcedure, Parameters);
        }

        public bool RequestChangePasswordInsertUpdate(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@RequestCode",ObjStruct.RequestedCode),
                new SqlParameter("@ProvidedEmail",ObjStruct.EmailId),
            };
            return SqlDBHelperOld.ExecuteNonQuery("RequestChangePasswordInsertUpdate", CommandType.StoredProcedure, Parameters);
        }



        // USER AUTHENTICATION

        public async Task<DataTable> UserAuthentications(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", ObjStruct.UserName),
                new SqlParameter("@Password", SqlDBHelperOld.Encrypt_Decrypt.Encrypt(ObjStruct.Passwords, SqlDBHelperOld.EncKey)),
                new SqlParameter("@IPAddress", ObjStruct.ipAddress),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("UserAuthentications", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<DataTable> UserAuthenticationsGetById(int Id)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", Id),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("UserAuthenticationsGetById", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

 

        public async Task<DataTable> Menu_Sidebar_GET(string roleId)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                 new SqlParameter("@RoleId",roleId),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("dbo.Menu_Sidebar_GET", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<DataTable> UserAuthenticationsByTokenAsync(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@accessToken",ObjStruct.accessToken),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("UserAuthenticationsByToken", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
        public async Task<bool> MasterUsersInsertUpdateAsync(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@CompanyId",ObjStruct.CompanyId),
                new SqlParameter("@RoleId",ObjStruct.RoleId),
                new SqlParameter("@UserName",ObjStruct.UserName),
                new SqlParameter("@Passwords",SqlDBHelperOld.Encrypt_Decrypt.Encrypt(ObjStruct.Passwords,SqlDBHelperOld.EncKey)),
                new SqlParameter("@FirstName",ObjStruct.FirstName),
                new SqlParameter("@MiddleName",ObjStruct.MiddleName),
                new SqlParameter("@LastName",ObjStruct.LastName),
                new SqlParameter("@EmailId",ObjStruct.EmailId),
                new SqlParameter("@MobileNo",ObjStruct.MobileNo),
                new SqlParameter("@ImageUrl",ObjStruct.ImageUrl),
                new SqlParameter("@IsActive",ObjStruct.IsActive),

            };
            return  Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("MasterUsersInsertUpdate", CommandType.StoredProcedure, Parameters));
        }

        public async Task<bool> MasterUsersChangePasswordNew(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@Passwords",SqlDBHelperOld.Encrypt_Decrypt.Encrypt(ObjStruct.Passwords,SqlDBHelperOld.EncKey)),
              
            };
            return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("dbo.MasterUsersChangePassword", CommandType.StoredProcedure, Parameters));
        }

        public async Task<bool> MasterUsersVerfiyEmail(string EmailId)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@EmailId",EmailId),
            };
            return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("MasterUsersVerfiyEmail", CommandType.StoredProcedure, Parameters));
        }

        public async Task<DataSet> DynamicMenuAside(DynamicMenuStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@RoleId",ObjStruct.RoleId),
            };

            using (DataSet dt = await SqlDBHelper.ExecuteDataSetWithParametersAsync("DynamicMenuAsideGET", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<DataTable> MasterUsersGET(PaginationRequest ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@CompanyId","0"),
                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("MasterUsersGET", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

        public async Task<DataSet> MasterRolesGet(DynamicMenuStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Param",ObjStruct.Client_id),
            };

            using (DataSet dt = await SqlDBHelper.ExecuteDataSetWithParametersAsync("[ClientDropdownGet]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }


        //public DataTable UserAuthentications(LoginStruct ObjStruct)
        //{
        //	SqlParameter[] Parameters = new SqlParameter[]
        //	{
        //		new SqlParameter("@UserName",ObjStruct.UserName),
        //		new SqlParameter("@Password",SqlDBHelperOld.Encrypt_Decrypt.Encrypt(ObjStruct.Passwords,SqlDBHelperOld.EncKey)),
        //		new SqlParameter("@IPAddress",ObjStruct.ipAddress),
        //	};
        //	using (DataTable dt = SqlDBHelper.ExecuteDataTableWithParametersAsync("UserAuthentications", CommandType.StoredProcedure, Parameters))
        //	{
        //		return dt;
        //	}
        //}
        public DataTable UserAuthenticationsByToken(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@accessToken",ObjStruct.accessToken),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("UserAuthenticationsByToken", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataTable UserAuthenticationsById(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId",ObjStruct.UserId),
                new SqlParameter("@IPAddress",ObjStruct.ipAddress),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteWithParametersDataTable("UserAuthenticationsById", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }
        //--------------new user Authentication--------------------------
        public string MasterUsersAuthentication(LoginStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
               new SqlParameter("@MobileNo",ObjStruct.MobileNo),
               new SqlParameter("@Passwords",ObjStruct.Passwords),
               new SqlParameter("@NewNo", SqlDbType.VarChar,30)
            };
            Parameters[2].Direction = ParameterDirection.Output;
            SqlDBHelperOld.ExecuteNonQuery("MasterUsersAuthentication", CommandType.StoredProcedure, Parameters);
            return Parameters[2].Value.ToString();
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




