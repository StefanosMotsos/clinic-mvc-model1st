using ClinicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Data
{
    public class ClinicMvcModelFirstContext : DbContext
    {

        public ClinicMvcModelFirstContext(DbContextOptions<ClinicMvcModelFirstContext> options) : base(options) { }

        public DbSet<Capability> Capabilities { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<MedicalProgram> MedicalProgram { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Capability>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.HasIndex(i => i.Name, "UQ_Capabilities_Name").IsUnique();
            });

            modelBuilder.Entity<MedicalProgram>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Programs)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_Programs_DoctorId");

                entity.HasMany(p => p.Patients)
                    .WithMany(mp => mp.Programs)
                    .UsingEntity("PatientsPrograms");

                entity.HasIndex(e => e.Description, "IX_MedicalPrograms_Description");
                entity.HasIndex(e => e.DoctorId, "IX_MedicalPrograms_DoctorId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.HasMany(d => d.Capabilities)
                    .WithMany(p => p.Roles)
                    .UsingEntity("RolesCapabilities", j =>
                    {
                        j.HasIndex("CapabilitiesId")
                        .HasDatabaseName("IX_RolesCapabilities_CapabilityId");
                    });

                entity.HasIndex(i => i.Name, "UQ_Roles_Name").IsUnique();
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Amka)
                    .HasMaxLength(11)
                    .HasColumnName("AMKA");
                entity.Property(e => e.BloodType).HasMaxLength(20);
                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.HasOne(u => u.User)
                    .WithOne(p => p.Patient)
                    .HasForeignKey<Patient>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Patients_UserId");

                entity.HasIndex(i => i.Amka, "IX_Patients_AMKA").IsUnique();
                entity.HasIndex(i => i.BloodType, "IX_Patients_BloodType");
                entity.HasIndex(i => i.UserId, "IX_Patients_UserId").IsUnique();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.Specialty).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasOne(u => u.User)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Doctors_UserId");

                entity.HasIndex(e => e.Specialty, "IX_Doctors_Specialty");
                entity.HasIndex(e => e.UserId, "IX_Doctors_UserId").IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Firstname).HasMaxLength(50);
                entity.Property(e => e.Lastname).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(100);
                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(r => r.Role)
                    .WithMany(u => u.Users)
                    .HasForeignKey(r => r.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Users_RoleId");

                entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
                entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");
                entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

            });
        }

    }
}
