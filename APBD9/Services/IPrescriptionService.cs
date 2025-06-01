using APBD9.DTOs;
namespace APBD9.Services;
    public interface IPrescriptionService
    {
        Task<int> AddPrescriptionAsync(AddPrescriptionReqDto request);
        Task<PatientDetailsDto> GetPatientDetailsAsync(int idPatient);
    }