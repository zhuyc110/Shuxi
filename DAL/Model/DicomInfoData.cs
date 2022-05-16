using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class DicomInfoData
    {
        [Required]
        [Key]
        public string ID { get; set; }

        public string PerformedProcedureStepID { get; set; }

        public DateTime PatientBirthDate { get; set; }

        public DateTime PerformedProcedureStepStartDate { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
