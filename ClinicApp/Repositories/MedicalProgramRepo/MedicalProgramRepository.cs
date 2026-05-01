using ClinicApp.Data;
using ClinicApp.Models;
using ClinicApp.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Repositories.MedicalProgramRepo
{
    public class MedicalProgramRepository : BaseRepository<MedicalProgram>, IMedicalProgramRepository
    {

        public MedicalProgramRepository(ClinicMvcModelFirstContext context) : base(context) { }
        public async Task<Doctor?> GetProgramDoctorAsync(int programId)
        {
            var program = await _context.MedicalProgram
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.Id == programId);

            return program?.Doctor;
        }

        public async Task<List<Patient>> GetProgramPatientsAsync(int programId)
        {
            return await _context.MedicalProgram
                .Where(p => p.Id == programId)
                .SelectMany(p => p.Patients)
                .ToListAsync();
        }
    }
}
