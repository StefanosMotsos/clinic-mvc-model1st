namespace ClinicApp.Models
{
    public class Doctor : BaseEntity
    {
        public int Id { get; set; }
        public string Specialty { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<MedicalProgram> Programs { get; set; } = new HashSet<MedicalProgram>();
    }
}
