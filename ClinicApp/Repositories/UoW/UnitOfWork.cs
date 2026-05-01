using ClinicApp.Data;
using ClinicApp.Repositories.DoctorRepo;
using ClinicApp.Repositories.MedicalProgramRepo;
using ClinicApp.Repositories.PatientRepo;
using ClinicApp.Repositories.UserRepo;

namespace ClinicApp.Repositories.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicMvcModelFirstContext _context;
        public IUserRepository UserRepository { get; }
        public IPatientRepository PatientRepository { get; }
        public IDoctorRepository DoctorRepository { get; }
        public IMedicalProgramRepository MedicalProgramRepository { get; }

        public UnitOfWork(ClinicMvcModelFirstContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
            DoctorRepository = new DoctorRepository(context);
            PatientRepository = new PatientRepository(context);
            MedicalProgramRepository = new MedicalProgramRepository(context);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
