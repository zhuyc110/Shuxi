using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class DicomInfoData
    {
        [Required]
        [Key]
        public string StudyInstanceId { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        public string PatientSex { get; set; }

        [Required]
        public string PatientAge { get; set; }

        [Required]
        public DateTime StatyDate { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
