namespace numerology_backend.Models
{
    public class LifeServiceRequest
    {
        public string LifePhasePredictionId { get; set; }
        public string LifePhaseTypeId { get; set; }
        public string NumberId { get; set; }
        public string LifePhase_Description { get; set; }
        public bool IsActive { get; set; }
    }
}
