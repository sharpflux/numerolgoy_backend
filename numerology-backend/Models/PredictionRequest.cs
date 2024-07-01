namespace NumerologystSolution.Models
{
    public class PredictionRequest
    {

        public string prediction_id { get; set; }
        public string predictionNumber { get; set; }
        public string predictionName { get; set; }
        public string predictionDescription { get; set;}

        public string MindId { get; set; }
        public string CombinationId { get; set; }
        public string MindTitle { get; set; }
        public string MIndNoDescription { get; set; }

        public string predictionsubId { get; set; }
        public string predictionsubNumber { get; set; }
        public string predictionSubName { get; set; }
        public string predictionSubDescription { get; set; }
        //  public string HtmlContent2 { get; set; }
        public bool IsActive { get; set; }
    }
}
