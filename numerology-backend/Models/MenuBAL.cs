using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Services;

namespace BAL
{
    public class MenuBAL
    {
        MenuDAL dataAccess = new MenuDAL();

        MenuDAL.MenuStruct ObjStrucure = new MenuDAL.MenuStruct();
        MenuDAL.DynamicMenuStruct DynamicMenuStructObj = new MenuDAL.DynamicMenuStruct();
        public int RoleId
        {
            get
            {
                return DynamicMenuStructObj.RoleId;
            }
            set { DynamicMenuStructObj.RoleId = value; }
        }
        public string section
        {
            get
            {
                return DynamicMenuStructObj.section;
            }
            set { DynamicMenuStructObj.section = value; }
        }
        public string title
        {
            get
            {
                return DynamicMenuStructObj.title;
            }
            set { DynamicMenuStructObj.title = value; }
        }


        public bool root
        {
            get
            {
                return DynamicMenuStructObj.root;
            }
            set { DynamicMenuStructObj.root = value; }
        }
        public string bullet
        {
            get
            {
                return DynamicMenuStructObj.bullet;
            }
            set { DynamicMenuStructObj.bullet = value; }
        }
        public string icon
        {
            get
            {
                return DynamicMenuStructObj.icon;
            }
            set { DynamicMenuStructObj.icon = value; }
        }
        public string page
        {
            get
            {
                return DynamicMenuStructObj.page;
            }
            set { DynamicMenuStructObj.page = value; }
        }

        public List<DynamicSubMenu> submenu
        {
            get
            {
                return DynamicMenuStructObj.submenu;
            }
            set { DynamicMenuStructObj.submenu = value; }
        }
        public int MenuId
        {
            get
            {
                return DynamicMenuStructObj.MenuId;
            }
            set { DynamicMenuStructObj.MenuId = value; }
        }
        public string Search
        {
            get
            {
                return ObjStrucure.Search;
            }
            set
            {
                ObjStrucure.Search = value;
            }
        }
       
        public int UserId
        {
            get
            {
                return ObjStrucure.UserId;
            }
            set
            {
                ObjStrucure.UserId = value;
            }
        }

        public int PlantId
        {
            get
            {
                return ObjStrucure.PlantId;
            }
            set
            {
                ObjStrucure.PlantId = value;
            }
        }
        public string URL
        {
            get
            {
                return ObjStrucure.URL;
            }
            set
            {
                ObjStrucure.URL = value;
            }
        }

        public string message
        {
            get
            {
                return ObjStrucure.message;
            }
            set { ObjStrucure.message = value; }
        }
    
        public bool error
        {
            get
            {
                return ObjStrucure.error;
            }
            set { ObjStrucure.error = value; }
        }
        public bool Iserror
        {
            get
            {
                return ObjStrucure.error;
            }

        }


        public List<MenuBAL> DynamicMenuAside()
        {
            dataAccess.StuructMenuAll = DynamicMenuStructObj;
            DataSet dt = new DataSet();
            dt = dataAccess.DynamicMenuAside(DynamicMenuStructObj);
            List<MenuBAL> lst = new List<MenuBAL>();
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                MenuBAL item = new MenuBAL();
                item.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                item.section = dr["section"].ToString();
                item.title = dr["title"].ToString();
                item.root = Convert.ToBoolean(dr["root"].ToString());
                item.bullet = dr["bullet"].ToString();
                item.icon = dr["icon"].ToString();
                item.page = dr["page"].ToString();
                item.submenu = GetSubMenus(dt.Tables[1], Convert.ToInt32(dr["MenuId"].ToString()));
                lst.Add(item);
            }

            return lst;
        }

        public List<DynamicSubMenu> GetSubMenus(DataTable dt, int MenuId)
        {
            List<DynamicSubMenu> submenus = new List<DynamicSubMenu>();
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["ParentId"].ToString()) == MenuId)
                {
                    DynamicSubMenu submenusItem = new DynamicSubMenu();
                    submenusItem.title = dr["title"].ToString();
                    submenusItem.root = Convert.ToBoolean(dr["root"].ToString());
                    submenusItem.bullet = dr["bullet"].ToString();
                    submenusItem.icon = dr["icon"].ToString();
                    submenusItem.page = dr["page"].ToString();
                    submenus.Add(submenusItem);
                }
            }
            return submenus;
        }

        public DataSet MasterModules_Menu()
        {
            dataAccess.MenuStructall = ObjStrucure;
            return dataAccess.MasterModules_Menu(ObjStrucure);
        }
        public DataSet MasterModules_MenuJs()
        {
            dataAccess.MenuStructall = ObjStrucure;
            return dataAccess.MasterModules_MenuJs(ObjStrucure);
        }
        public DataTable CheckAddAuthority()
        {
            dataAccess.MenuStructall = ObjStrucure;
            return dataAccess.CheckAddAuthority(ObjStrucure);
        }
  
        public List<OccupationsLists> ModulesGet()
        {
            dataAccess.MenuStructall = ObjStrucure;
            List<OccupationsLists> lisOccu = new List<OccupationsLists>();
            DataTable dt = new DataTable();
            dt = dataAccess.ModulesGet(ObjStrucure);
            foreach (DataRow dr in dt.Rows)
            {
                OccupationsLists pln = new OccupationsLists();
                pln.text = dr["ModuleName"].ToString();
                pln.id = Convert.ToInt32(dr["ModuleId"].ToString());
                lisOccu.Add(pln);
            }
            return lisOccu;
        }

    }
}

public class OccupationsLists
{
    public int id { get; set; }
    public string text { get; set; }
    public bool IsFavorite { get; set; }

    public string IsFavoriteString { get; set; }
    public string DriverStatus { get; set; }

    public string Leave { get; set; }
}
