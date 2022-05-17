using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class DicomGeneralInfo
    {
        [Key]
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public static readonly string TotalCount = "TotalCount";
    }
}
