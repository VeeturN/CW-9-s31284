using System.ComponentModel.DataAnnotations;

namespace APBD9.Models
{
    public class PrescriptionMedicament
    {
        public int IdMedicament { get; set; }
        public int IdPrescription { get; set; }
        
        [Required]
        public int Dose { get; set; }
        
        [MaxLength(100)]
        public string Details { get; set; }
        
        public virtual Medicament Medicament { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}