using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Services;

namespace Models
{
    public class LoginBAL
    {

        LoginDAL dataAccess = new LoginDAL();
        LoginDAL.LoginStruct objStrucure = new LoginDAL.LoginStruct();

		public string accessToken
		{
			get
			{
				return objStrucure.accessToken;
			}
			set { objStrucure.accessToken = value; }
		}
		public string refreshToken
		{
			get
			{
				return objStrucure.refreshToken;
			}
			set { objStrucure.refreshToken = value; }
		}

		public List<int> roles
		{
			get
			{
				return objStrucure.roles;
			}
			set { objStrucure.roles = value; }
		}
		public string message
		{
			get
			{
				return objStrucure.message;
			}
			set { objStrucure.message = value; }
		}
		public bool error
		{
			get
			{
				return objStrucure.error;
			}
			set { objStrucure.error = value; }
		}
		public int SearchBy
        {
            get
            {
                return objStrucure.SearchBy;
            }
            set
            {
                objStrucure.SearchBy = value;
            }
        }
        public string SearchCriteria
        {
            get
            {
                return objStrucure.SearchCriteria;
            }
            set
            {
                objStrucure.SearchCriteria = value;
            }
        }
        public int PageIndex
        {
            get
            {
                return objStrucure.PageIndex;
            }
            set
            {
                objStrucure.PageIndex = value;
            }
        }
        public int PageSize
        {
            get
            {
                return objStrucure.PageSize;
            }
            set
            {
                objStrucure.PageSize = value;
            }
        }
        public int UserId
        {
            get
            {
                return objStrucure.UserId;
            }
            set
            {
                objStrucure.UserId = value;
            }
        }
        public int CompanyId
        {
            get
            {
                return objStrucure.CompanyId;
            }
            set
            {
                objStrucure.CompanyId = value;
            }
        }

        public int PlatId
        {
            get
            {
                return objStrucure.PlatId;
            }
            set
            {
                objStrucure.PlatId = value;
            }
        }
        public int DepartmentId
        {
            get
            {
                return objStrucure.DepartmentId;
            }
            set
            {
                objStrucure.DepartmentId = value;
            }
        }
        public string UserName
        {
            get
            {
                return objStrucure.UserName;
            }
            set
            {
                objStrucure.UserName = value;
            }
        }
        public string Passwords
        {
            get
            {
                return objStrucure.Passwords;
            }
            set
            {
                objStrucure.Passwords = value;
            }
        }
        public string FirstName
        {
            get
            {
                return objStrucure.FirstName;
            }
            set
            {
                objStrucure.FirstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return objStrucure.LastName;
            }
            set
            {
                objStrucure.LastName = value;
            }
        }
        public string EmailId
        {
            get
            {
                return objStrucure.EmailId;
            }
            set
            {
                objStrucure.EmailId = value;
            }
        }
        public string MobileNo
        {
            get
            {
                return objStrucure.MobileNo;
            }
            set
            {
                objStrucure.MobileNo = value;
            }
        }
        public string LastLogin
        {
            get
            {
                return objStrucure.LastLogin;
            }
            set
            {
                objStrucure.LastLogin = value;
            }
        }
        public string ImageUrl
        {
            get
            {
                return objStrucure.ImageUrl;
            }
            set
            {
                objStrucure.ImageUrl = value;
            }
        }


        // User Authentication

        public string ipAddress
        {
            get
            {
                return objStrucure.ipAddress;
            }
            set
            {
                objStrucure.ipAddress = value;
            }
        }

        public int RoleId
        {
            get
            {
                return objStrucure.RoleId;
            }
            set
            {
                objStrucure.RoleId = value;
            }
        }
        public object reportingTo
        {
            get
            {
                return objStrucure.reportingTo;
            }
            set
            {
                objStrucure.reportingTo = value;
            }
        }
        public object MiddleName
        {
            get
            {
                return objStrucure.MiddleName;
            }
            set
            {
                objStrucure.MiddleName = value;
            }
        }
        public bool IsActive
        {
            get
            {
                return objStrucure.IsActive;
            }
            set
            {
                objStrucure.IsActive = value;
            }
        }
        public int RequestedCode
        {
            get
            {
                return objStrucure.RequestedCode;
            }
            set
            {
                objStrucure.RequestedCode = value;
            }
        }
		public int id
		{
			get
			{
				return objStrucure.id;
			}
			set
			{
				objStrucure.id = value;
			}
		}
		public string fullname
		{
			get
			{
				return objStrucure.fullname;
			}
			set
			{
				objStrucure.fullname = value;
			}
		}
		public string pic
		{
			get
			{
				return objStrucure.pic;
			}
			set
			{
				objStrucure.pic = value;
			}
		}
		public DataTable MasterDepartmentsGET()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterDepartmentsGET(objStrucure);
        }

        public DataSet MasterUsersDropdowns()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersDropdowns(objStrucure);
        }
        public DataTable MasterUOMGetForDropDown()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUOMGetForDropDown(objStrucure);
        }
        public bool MasterUsersInsertUpdate()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersInsertUpdate(objStrucure);
        }
        public bool MasterUsersChangePassword()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersChangePassword(objStrucure);

        }
        public bool MasterUsersChangePasswordWithCode()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersChangePasswordWithCode(objStrucure);
        }
        public bool MasterUsersChangeImage()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersChangeImage(objStrucure);
        }
        public DataTable MasterUsersGetData()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersGetData(objStrucure);
        }
        public DataTable MasterUsersGetReportDataByDepartment()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersGetReportDataByDepartment(objStrucure);
        }
        public bool CheckUserName()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.CheckUserName(objStrucure);
        }
        public DataTable RequestChangePasswordCheckExpiredOrNot()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.RequestChangePasswordCheckExpiredOrNot(objStrucure);
        }
        public DataTable CheckUserEmail()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.CheckUserEmail(objStrucure);
        }
        public DataTable MasterUsers_Get_User_By_Id()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsers_Get_User_By_Id(objStrucure);
        }
        public bool MasterUsers_Delete()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsers_Delete(objStrucure);
        }
        public DataTable MasterUsersGetDataById()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersGetDataById(objStrucure);
        }
        public bool RequestChangePasswordInsertUpdate()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.RequestChangePasswordInsertUpdate(objStrucure);
        }

        // USER AUTHENTICATION
        //public DataTable UserAuthentications()
        //{
        //    dataAccess.StuructAll = objStrucure;
        //    return dataAccess.UserAuthentications(objStrucure);
        //}

		 

		public List<LoginBAL> UserAuthenticationsGet()
		{
			dataAccess.StuructAll = objStrucure;
			DataTable dt = new DataTable();
			//dt = dataAccess.UserAuthentications(objStrucure);
			List<LoginBAL> lst = new List<LoginBAL>();
			foreach (DataRow dr in dt.Rows)
			{
				LoginBAL emp = new LoginBAL();
				emp.roles = new List<int>();
				emp.id= Convert.ToInt32(dr["EmployeeId"].ToString());
				emp.UserId = Convert.ToInt32(dr["EmployeeId"].ToString());
				emp.UserName = dr["FirstName"].ToString();
				emp.MiddleName = dr["MiddleName"].ToString();
				emp.fullname = dr["FirstName"].ToString()+" " + dr["LastName"].ToString();
				emp.LastName = dr["LastName"].ToString();
				emp.ImageUrl = "";
				emp.EmailId = dr["EmailId"].ToString();
				emp.MobileNo = dr["MobileNumber"].ToString();
				emp.accessToken = dr["accessToken"].ToString();
                emp.RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                emp.pic = "./assets/media/users/300_25.jpg";
				emp.roles.Add(1);
				lst.Add(emp);
			}

			return lst;
		}

		public List<LoginBAL> UserAuthenticationsByToken()
		{
			dataAccess.StuructAll = objStrucure;
			DataTable dt = new DataTable();
			dt = dataAccess.UserAuthenticationsByToken(objStrucure);
			List<LoginBAL> lst = new List<LoginBAL>();
			foreach (DataRow dr in dt.Rows)
			{
				LoginBAL emp = new LoginBAL();
				emp.roles = new List<int>();
				emp.id = Convert.ToInt32(dr["EmployeeId"].ToString());
				emp.UserId = Convert.ToInt32(dr["EmployeeId"].ToString());
				emp.UserName = dr["FirstName"].ToString();
				emp.MiddleName = dr["MiddleName"].ToString();
				emp.fullname = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
				emp.LastName = dr["LastName"].ToString();
				emp.ImageUrl = "";
				emp.EmailId = dr["EmailId"].ToString();
				emp.MobileNo = dr["MobileNumber"].ToString();
				emp.accessToken = dr["accessToken"].ToString();
				emp.pic = "./assets/media/users/300_25.jpg";
				emp.roles.Add(1);
				lst.Add(emp);
			}

			return lst;
		}

		public DataTable UserAuthenticationsById()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.UserAuthenticationsById(objStrucure);
        }
        public DataTable MasterPlant_Get()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterPlant_Get(objStrucure);
        }
        public DataTable MasterUsersDropdownsWebMethod()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersDropdownsWebMethod(objStrucure);
        }
        public DataTable MasterUserReportingToWebMethod()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUserReportingToWebMethod(objStrucure);
        }
        //--------------new user Authentication--------------------------
        public string MasterUsersAuthentication()
        {
            dataAccess.StuructAll = objStrucure;
            return dataAccess.MasterUsersAuthentication(objStrucure);
        }

    }

    public class UserSession
    {
        public string UserId
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }
        public string RoleCode
        {
            get;
            set;
        }

        public string CompanyName
        {
            get;
            set;
        }


        public string CompanyAddress
        {
            get;
            set;
        }

        public string CompanyPhone
        {
            get;
            set;
        }

        public string LogoImageUrl
        {
            get;
            set;
        }

        public string MobileNo1
        {
            get;
            set;
        }
        public string MobileNo2
        {
            get;
            set;
        }

        public string UserImageUrl
        {
            get;
            set;
        }
        public string POAmend
        {
            get;
            set;
        }

        public int RoleId
        {
            get;
            set;
        }

        public int CompanyId
        {
            get;
            set;
        }

        public int PlantId
        {
            get;
            set;
        }

        public string Add
        {
            get;
            set;
        }
        public string Update
        {
            get;
            set;
        }
        public string Delete
        {
            get;
            set;
        }
        public int ModuleId
        {
            get;
            set;
        }

        public string View
        {
            get;
            set;
        }


        public DataSet dsMenuSession
        {
            get;
            set;
        }




    }

    public class UserAuthentication
    {
        public int iUserId;
        public int iRoleId;
        private string msUserName;
        private string msIPAddress;
        private string msPassword;
        private bool bIsValid;
        
        private DataTable moDataSet;
        LoginBAL blluser = new LoginBAL();

        public UserAuthentication(string username, string password, string Ipaddress)
        {
            msUserName = username;
            msPassword = password;
            msIPAddress = Ipaddress;
            CheckIsUserValid(1);
        }
        public UserAuthentication(int UserId, string Ipaddress)
        {
            iUserId = UserId;
            msIPAddress = Ipaddress;
            CheckIsUserValid(0);
        }

        /// <summary>
        /// User begin authenticated is valid or not.
        /// </summary>
        public Boolean ValidUser
        {
            get { return bIsValid; }
        }

        /// to get user role.
        /// </summary>
        public string UserRoles
        {
            get;
            set;
        }

        //public static string ProcessorIdOld()
        //{
        //    System.Management.ManagementObjectCollection mbsList = null;
        //    System.Management.ManagementObjectSearcher mbs = new System.Management.ManagementObjectSearcher("Select * From Win32_processor");
        //    mbsList = mbs.Get();
        //    string id = "";
        //    foreach (System.Management.ManagementObject mo in mbsList)
        //    {
        //        id = mo["ProcessorID"].ToString();
        //    }
        //    return id;
        //}

        /// <summary>
        /// Get current user ip address.
        /// </summary>
        /// <returns>The IP Address</returns>
    



        /// <summary>
        /// Check that if user begin authenticated is valid or not.
        /// </summary> 
        /// <returns></returns>
        private Boolean CheckIsUserValid(int IdOrUserName)
        {
            blluser.UserName = msUserName;
            blluser.Passwords = msPassword;
            blluser.ipAddress = msIPAddress;
            blluser.UserId = iUserId;
         //   CheckUserByIdOrIdPassword(IdOrUserName);
            return bIsValid;
        }
        //public bool CheckUserByIdOrIdPassword(int IdOrUserName)
        //{
        //    if (IdOrUserName == 1)
        //        moDataSet = blluser.UserAuthentications();
        //    else
        //        moDataSet = blluser.UserAuthenticationsById();


        //    if (moDataSet != null && moDataSet.Rows.Count > 0)
        //    {
        //        UserSession userSession = new UserSession();
        //        userSession.UserId = moDataSet.Rows[0]["UserId"].ToString();
        //        userSession.UserName = moDataSet.Rows[0]["FirstName"].ToString() + " " + moDataSet.Rows[0]["LastName"].ToString();
        //        userSession.FirstName = moDataSet.Rows[0]["FirstName"].ToString();
        //        userSession.LastName = moDataSet.Rows[0]["LastName"].ToString();
        //        userSession.MobileNo1 = moDataSet.Rows[0]["MobileNo1"].ToString();
        //        userSession.MobileNo2 = moDataSet.Rows[0]["MobileNo2"].ToString();
        //        userSession.RoleCode = moDataSet.Rows[0]["RoleCode"].ToString();
        //        UserRoles = moDataSet.Rows[0]["RoleCode"].ToString();
        //        userSession.RoleId = int.Parse(moDataSet.Rows[0]["RoleId"].ToString());
        //        userSession.CompanyId = int.Parse(moDataSet.Rows[0]["CompanyId"].ToString());
        //        userSession.CompanyName = moDataSet.Rows[0]["CompanyName"].ToString();
        //        userSession.UserImageUrl = moDataSet.Rows[0]["UserImage"].ToString();
        //        iUserId = int.Parse(moDataSet.Rows[0]["UserId"].ToString());
        //        iRoleId = userSession.RoleId;
        //      //  HttpContext.Current.Session["USER"] = userSession;
        //        bIsValid = true;
        //    }
        //    else
        //        bIsValid = false;

        //    return bIsValid;
        //}
    }
}
