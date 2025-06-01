namespace APBD9.DTOs
{
    public class AddPrescriptionReqDto
    {
        public PatientDto Patient { get; set; }
        public List<PrescriptionMedicamentDto> Medicaments { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set;}
        public int IdDoctor { get; set; }   
    }

    public class PatientDto
    {
        public int? IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class PrescriptionMedicamentDto
    {
        public int IdMedicament { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }
}