using NumerologystSolution.Models;
using OmsSolution.Models;
using Service;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using numerology_backend.Models;

namespace NumerologystSolution.Services
{
    public class PredictionDBHelper
    {
        public async Task<bool> PredictionMasterInsertUpdate(PredictionRequest ObjStruct)
        {

            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
            {


                new SqlParameter("@Prediction_Id",ObjStruct.prediction_id),
                new SqlParameter("@Prediction_Name",ObjStruct.predictionName),
                new SqlParameter("@Prediction_Description",ObjStruct.predictionDescription),
                new SqlParameter("@Prediction_No",ObjStruct.predictionNumber),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                //new SqlParameter("@NewNo", SqlDbType.VarChar,30),

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("[PredictionMasterInsertUpdate]", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<bool> MindPredictionNumberInsertUpdate(PredictionRequest ObjStruct)
        {

            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
            {


                new SqlParameter("@MindId",ObjStruct.MindId),
                new SqlParameter("@CombinationId",ObjStruct.CombinationId),
                new SqlParameter("@MindTitle",ObjStruct.MindTitle),
                new SqlParameter("@MIndNoDescription",ObjStruct.MIndNoDescription),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                //new SqlParameter("@NewNo", SqlDbType.VarChar,30),

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("[mindPredictionNumbersaveInserUpdate]", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> saveDataPredictionReport(ReportSave ObjStruct)
        {

            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
            {


                new SqlParameter("@ReportID",ObjStruct.ReportID),
                new SqlParameter("@Client_id",ObjStruct.Client_id),
                new SqlParameter("@DataPredictionReport",ObjStruct.DataPredictionReport),
                new SqlParameter("@DataMissingNumberReport",ObjStruct.DataMissingNumberReport),
                new SqlParameter("@DataRemediesReport",ObjStruct.DataRemediesReport),
               // new SqlParameter("@IsActive",ObjStruct.IsActive),
                //new SqlParameter("@NewNo", SqlDbType.VarChar,30),

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("[SaveReportInsertUpdate]", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> SubPredictionMasterInsertUpdate(PredictionRequest ObjStruct)
        {

            try
            {
                SqlParameter[] Parameters = new SqlParameter[]
            {


                new SqlParameter("@SubPrediction_Id",ObjStruct.predictionsubId),
                new SqlParameter("@Prediction_Id",ObjStruct.predictionsubNumber),
                new SqlParameter("@SubPrediction_No",ObjStruct.predictionSubName),
                new SqlParameter("@SubPrediction_Description",ObjStruct.predictionSubDescription),
                new SqlParameter("@IsActive",ObjStruct.IsActive),
                //new SqlParameter("@NewNo", SqlDbType.VarChar,30),

              };
                return Convert.ToBoolean(await SqlDBHelper.ExecuteNonQuery("[SubPredictionMasterInsertUpdate]", CommandType.StoredProcedure, Parameters));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<DataTable> PredictionsGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[PredictionMasterGET]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }




        public async Task<DataTable> PredictionsSubGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[PredictionMasterSubGET]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }
    
        public async Task<DataTable> MindNumberPredictionGET(PaginationRequest ObjStruct, string idString)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {

                new SqlParameter("@StartIndex",ObjStruct.StartIndex),
                new SqlParameter("@PageSize",ObjStruct.PageSize),
                new SqlParameter("@SearchBy",ObjStruct.SearchBy),
                new SqlParameter("@SearchCriteria",ObjStruct.SearchCriteria),
                new SqlParameter("@categoryid",idString),
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[MindNumberPredictionGET]", CommandType.StoredProcedure, Parameters))
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
        public async Task<DataTable> PredictionPlanetsGET(string BirthDate, string Gender,string Client_id)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@DOB",BirthDate),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@Client_id",Client_id)
            };

            using (DataTable dt = await SqlDBHelper.ExecuteDataTableWithParametersAsync("[GetMatchingUnMatchingDigits]", CommandType.StoredProcedure, Parameters))
            {
                return dt.Copy();
            }
        }

    }
}
