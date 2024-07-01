using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using NumerologystSolution.Models;

namespace NumerologystSolution.Services
{
    public class NumerologyTitleDBHelper
    {
        public async Task<bool> NumerologyTitlesaveInsertUpdate(NumerologyTitleRequest ObjStruct)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
              {
                new SqlParameter("@Numerology_Id",ObjStruct.Numerology_Id),
                new SqlParameter("@Numerology_Title",ObjStruct.titleName),
                new SqlParameter("@Numerology_Description",ObjStruct.titleDescription),
                new SqlParameter("@IsActive",ObjStruct.IsActive)

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("NumerologyTitleInsertUpdate", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<DataTable> NumerologyTitleGET(PaginationRequest ObjStruct, string idString)
                {
                     SqlParameter[] Parameters = new SqlParameter[]
                             {
                                 new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                                 new SqlParameter("@PageSize",ObjStruct.PageSize),
                                 new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                                 new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                                 new SqlParameter("@categoryid",idString),
                              };

                  using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("NumerologyTitleMasterGET", CommandType.StoredProcedure, Parameters))
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
