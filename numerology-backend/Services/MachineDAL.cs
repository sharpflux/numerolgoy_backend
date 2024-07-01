using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Web;

namespace Services
{
    public class MachineDAL
    {

        public struct MachineStruct
        {
            public int MachineId;
            public string MachineCode;
            public string MachineDescription;
            public string CsvLocation;
            public bool IsActive;


            public int PageIndex;
            public int PageSize;
            public int searchBy;
            public string Search;


            public int ProuctionInputId;
            public DateTime dateandtime;
            public string machinno;
            public int OKcount;
            public int NOTOKcount;
            public int PowerON;
            public int Ideal;


        }

        MachineStruct objMacine = new MachineStruct();

        public MachineStruct StructureAll
        {
            get
            {
                return objMacine;
            }
            set
            {
                objMacine = value;
            }
        }


        public string MachinesInsertUpdate(MachineStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
             {
             new SqlParameter("@MachineId", ObjStruct.MachineId),
             new SqlParameter("@MachineCode", ObjStruct.MachineCode),
             new SqlParameter("@MachineDescription", ObjStruct.MachineDescription),
             new SqlParameter("@CsvLocation", ObjStruct.CsvLocation),
             new SqlParameter("@NewNo", SqlDbType.VarChar,30)
             };
            Parameters[4].Direction = ParameterDirection.Output;
            SqlDBHelperOld.ExecuteNonQuery("MachinesInsertUpdate", CommandType.StoredProcedure, Parameters);
            return Parameters[4].Value.ToString();
        }
        public DataTable MachinesGET(MachineStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
              
                new SqlParameter("@StartIndex",ObjStruct.PageIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.searchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.Search),
            };
            using (DataTable dt = SqlDBHelperOld.ExecuteDataTableWithParameters("MachinesGET", CommandType.StoredProcedure, Parameters))
            {
                return dt;
            }
        }

        public bool MachinesInActive(MachineStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@MachineId",ObjStruct.MachineId),
            };
            return SqlDBHelperOld.ExecuteNonQuery("MachinesInActive", CommandType.StoredProcedure, Parameters);

        }


        public string ProudctionInputInsert(MachineStruct ObjStruct)
        {
            SqlParameter[] Parameters = new SqlParameter[]
             {
                 new SqlParameter("@ProuctionInputId", ObjStruct.ProuctionInputId),
                 new SqlParameter("@dateandtime", ObjStruct.dateandtime),
                 new SqlParameter("@machinno", ObjStruct.machinno),
                 new SqlParameter("@OKcount", ObjStruct.OKcount),
                 new SqlParameter("@NOTOKcount", ObjStruct.NOTOKcount),
                 new SqlParameter("@PowerON", ObjStruct.PowerON),
                 new SqlParameter("@Ideal", ObjStruct.Ideal),
                 new SqlParameter("@NewNo", SqlDbType.VarChar,30)
             };
            Parameters[7].Direction = ParameterDirection.Output;
            SqlDBHelperOld.ExecuteNonQuery("ProudctionInputInsert", CommandType.StoredProcedure, Parameters);
            return Parameters[7].Value.ToString();
        }

    }
}


