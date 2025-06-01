using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using APBD9.Data;
using APBD9.DTOs;
using APBD9.Models;

namespace APBD9.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly DatabaseContext _context;

    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<int> AddPrescriptionAsync(AddPrescriptionReqDto request)
    {
        await ValidateRequestAsync(request);

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var patient = await GetOrCreatePatientAsync(request.Patient);
            var prescription = await CreatePrescriptionAsync(request, patient.IdPatient);
            await CreatePrescriptionMedicamentsAsync(request.Medicaments, prescription.IdPrescription);
            
            await transaction.CommitAsync();
            return prescription.IdPrescription;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    private async Task ValidateRequestAsync(AddPrescriptionReqDto request)
    {
        if (request.Medicaments.Count > 10)
            throw new ValidationException("Prescription cannot contain more than 10 medicaments.");
            
        if (request.DueDate < request.Date)
            throw new ValidationException("Due date cannot be earlier than date.");

        var medicamentIds = request.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .CountAsync();

        if (existingMedicaments != medicamentIds.Count)
            throw new ValidationException("One or more medicaments do not exist.");
    }

    private async Task<Patient> GetOrCreatePatientAsync(PatientDto patientDto)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.IdPatient == patientDto.IdPatient);

        if (patient != null)
            return patient;

        patient = new Patient
        {
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            Birthdate = patientDto.BirthDate
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    private async Task<Prescription> CreatePrescriptionAsync(AddPrescriptionReqDto request, int patientId)
    {
        var doctorExists = await _context.Doctors.AnyAsync(d => d.IdDoctor == request.IdDoctor);
        if (!doctorExists)
            throw new ValidationException($"Doctor with ID {request.IdDoctor} does not exist.");

        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdPatient = patientId,
            IdDoctor = request.IdDoctor  
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
        return prescription;
    }
    private async Task CreatePrescriptionMedicamentsAsync(IEnumerable<PrescriptionMedicamentDto> medicaments, int prescriptionId)
    {
        var prescriptionMedicaments = medicaments.Select(med => new PrescriptionMedicament
        {
            IdPrescription = prescriptionId,
            IdMedicament = med.IdMedicament,
            Dose = med.Dose,
            Details = med.Description
        });

        _context.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
        await _context.SaveChangesAsync();
    }
    
    public async Task<PatientDetailsDto> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .AsNoTracking()
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
            throw new KeyNotFoundException($"Patient with ID {idPatient} not found.");

        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDetailsDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        Id = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionMedicaments
                        .Select(pm => new MedicamentDetailsDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Description = pm.Medicament.Description,
                            Type = pm.Medicament.Type,
                            Dose = pm.Dose,
                            Details = pm.Details
                        })
                        .ToList()
                })
                .ToList()
        };
    }
}