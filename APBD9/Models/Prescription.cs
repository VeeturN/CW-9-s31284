using System.ComponentModel.DataAnnotations;

namespace APBD9.Models
{
    public class Prescription
    {
        public int IdPrescription { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        [Required]
        public int IdPatient { get; set; }
        
        [Required]
        public int IdDoctor { get; set; }
        
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
    }
}