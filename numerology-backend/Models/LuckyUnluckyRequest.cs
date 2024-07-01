namespace NumerologystSolution.Models
{
    public class LuckyUnluckyRequest
    {
        public string luckyUnluckyId { get; set; }

        public string predictionsubNumber { get; set; }


        public string commaSeparatedNumbers { get; set; }

        public bool LuckyUnluckySelection { get; set; }
        public bool IsActive { get; set; }
    }
}
