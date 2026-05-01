namespace ClinicApp.Models
{
    public class Patient : BaseEntity
    {
        public int Id { get; set; }
        public string Amka { get; set; } = null!;
        public string BloodType { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<MedicalProgram> Programs { get; set; } = new HashSet<MedicalProgram>();
    }
}
