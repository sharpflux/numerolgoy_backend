using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Service;

namespace Services
{
    public class MenuDAL
    {


        public struct DropdownBinder
        {
            public int id;
            public string text;
        }

        public struct MenuStruct
        {
            public int RoleId;
            public int UserId;
            public int PlantId;
            public string URL;
            public string Search;
            public string message;
            public bool error;
            public bool Iserror;
        }

        public struct DynamicMenuStruct
        {
            public int RoleId;
            public int Client_id;
            public string section;
            public string title;
            public bool root;
            public string bullet;
            public string icon;
            public string page;
            public int MenuId;
            public string message;
            public bool error;
            public bool Iserror;
            public List<DynamicSubMenu> submenu;
        }

        MenuStruct objMenu = new MenuStruct();

        public MenuStruct MenuStructall
        {
            get
            {
                return objMenu;
            }
            set
            {
                objMenu = value;
            }
        }
        DynamicMenuStruct objMenuDynamic = new DynamicMenuStruct();

        public DynamicMenuStruct StuructMenuAll
        {
            get
            {
                return objMenuDynamic;
            }
            set
            {
                objMenuDynamic = value;
            }
        }
        public DataSet MasterModules_Menu(MenuStruct objMenu)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
              
                new SqlParameter("@PlantId",objMenu.PlantId),
                new SqlParameter("@RoleId",objMenu.RoleId),
                new SqlParameter("@UserId",objMenu.UserId),
            };
            using (DataSet dt = SqlDBHelperOld.ExecuteWithParametersDataSet("MasterModules_Menu", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataSet MasterModules_MenuJs(MenuStruct objMenu)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
               
                new SqlParameter("@RoleId",objMenu.RoleId),
                new SqlParameter("@UserId",objMenu.UserId),
            };
            using (DataSet dt = SqlDBHelperOld.ExecuteWithParametersDataSet("MasterModules_MenuJs", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

     
        public DataTable CheckAddAuthority(MenuStruct objMenu)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@RoleId",objMenu.RoleId),
                new SqlParameter("@URL",objMenu.URL),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("CheckAddAuthority", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }


        public DataTable ModulesGet(MenuStruct objMenu)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@ModuleName",objMenu.Search),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("ModulesGet", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public DataSet DynamicMenuAside(DynamicMenuStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@RoleId",ObjStruct.RoleId),
            };
            using (DataSet dt = SqlDBHelperOld.ExecuteWithParametersDataSet("DynamicMenuAsideGET", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }
    }
}
public class DynamicSubMenu
{
	public string section { get; set; }
	public string title { get; set; }
	public bool root { get; set; }
	public string bullet { get; set; }
	public string icon { get; set; }
	public string page { get; set; }
 
}