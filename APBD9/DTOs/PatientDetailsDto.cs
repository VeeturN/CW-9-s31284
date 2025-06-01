using System.ComponentModel.DataAnnotations;

namespace APBD9.DTOs
{
    public class PatientDetailsDto
    {
        public int IdPatient { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [Required]
        public DateTime BirthDate { get; set; }
        
        [Required]
        public List<PrescriptionDetailsDto> Prescriptions { get; set; }
        
    }

    public class PrescriptionDetailsDto
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; }
        public List<MedicamentDetailsDto> Medicaments { get; set; }
    }

    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
    
    public class MedicamentDetailsDto
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? Dose { get; set; }
        public string Details { get; set; }
    }
    
    
}