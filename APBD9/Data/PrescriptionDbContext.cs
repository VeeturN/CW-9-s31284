using Microsoft.EntityFrameworkCore;
using APBD9.Models;

namespace APBD9.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.ToTable("Medicament");
                entity.HasKey(e => e.IdMedicament);
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.Description)
                    .HasMaxLength(100);
                entity.Property(e => e.Type)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctor");
                entity.HasKey(e => e.IdDoctor);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");
                entity.HasKey(e => e.IdPatient);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.Birthdate)
                    .IsRequired();
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.ToTable("Prescription");
                entity.HasKey(e => e.IdPrescription);
                entity.Property(e => e.Date)
                    .IsRequired();
                entity.Property(e => e.DueDate)
                    .IsRequired();

                entity
                    .HasOne(e => e.Doctor)
                    .WithMany(d => d.Prescriptions)
                    .HasForeignKey(e => e.IdDoctor);

                entity
                    .HasOne(e => e.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(e => e.IdPatient);
            });

            modelBuilder.Entity<PrescriptionMedicament>(entity =>
            {
                entity.ToTable("Prescription_Medicament");
                entity.HasKey(e => new { e.IdPrescription, e.IdMedicament });

                entity.Property(e => e.Dose)
                    .IsRequired();
                entity.Property(e => e.Details)
                    .HasMaxLength(100);

                entity
                    .HasOne(pm => pm.Prescription)
                    .WithMany(p => p.PrescriptionMedicaments)
                    .HasForeignKey(pm => pm.IdPrescription);

                entity
                    .HasOne(pm => pm.Medicament)
                    .WithMany(m => m.PrescriptionMedicaments)
                    .HasForeignKey(pm => pm.IdMedicament);
            });
        }
    }
}