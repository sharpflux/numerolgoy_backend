namespace numerology_backend.Models
{
    public class ReportSave
    {
        public int ReportID { get; set; }
  
        public string prediction { get; set; }

        public string missingnumers { get; set; }
        public string DataPredictionReport { get; set; }
        public string DataMissingNumberReport { get; set; }
        public string DataRemediesReport { get; set; }

        public string remidies { get; set; }

        public string Client_id { get; set; }
        public bool IsActive { get; set; }
    }
}
