namespace ClinicApp.Models
{
    public class MedicalProgram : BaseEntity
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<Patient> Patients { get; set; } = new HashSet<Patient>();
    }
}
