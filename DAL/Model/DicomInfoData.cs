using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class DicomInfoData
    {
        [Required]
        [Key]
        public string ID { get; set; }

        [Required]
        public string PerformedProcedureStepID { get; set; }

        [Required]
        public string OperatorsName { get; set; }

        [Required]
        public DateTime PatientBirthDate { get; set; }

        [Required]
        public string PerformingPhysicansName { get; set; }

        [Required]
        public DateTime PerformedProcedureStepStartDate { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
