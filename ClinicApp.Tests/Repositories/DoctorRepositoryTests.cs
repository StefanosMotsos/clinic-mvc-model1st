
using ClinicApp.Data;
using ClinicApp.Models;
using ClinicApp.Repositories.DoctorRepo;
using ClinicApp.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Repositories
{
    public class DoctorRepositoryTests
    {
        private readonly ClinicMvcModelFirstContext _context;
        private readonly DoctorRepository _repository;

        private static CancellationToken Ct => TestContext.Current.CancellationToken;

        // @BeforeEach

        public DoctorRepositoryTests()
        {
            _context = TestDbContextFactory.Create();
            _repository = new DoctorRepository(_context);
        }

        [Fact]
        public async Task GetUserDoctorByUsernameAsync_WhenUserIsDoctor_ReturnsUserWithDoctor()
        {
            User doctorUser;
            User? user;

            doctorUser = CreateDoctorUser(
                username: "stef",
                specialty: "Oncologist"
                );

            await _context.Users.AddAsync(doctorUser, Ct);
            await _context.SaveChangesAsync(Ct);



            user = await _repository.GetUserDoctorByUsernameAsync("stef");



            Assert.NotNull(user);
            Assert.Equal("stef", user.Username);
            Assert.NotNull(user.Doctor);
            Assert.Equal("Oncologist", user.Doctor.Specialty);
        }

        [Fact]
        public async Task GetUserDoctorByUsernameAsync_WhenUsernameDoesNotExist_ReturnsNull()
        {
            User? user;

            user = await _repository.GetUserDoctorByUsernameAsync("nonexistent");

            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserDoctorByUsernameAsync_WhenUserExistsButIsNotDoctor_ReturnsNull()
        {
            User nonDoctorUser;
            User? user;

            nonDoctorUser = CreateNonDoctorUser(
                username: "georgie"
            );

            await _context.Users.AddAsync(nonDoctorUser, Ct);
            await _context.SaveChangesAsync(Ct);


            user = await _repository.GetUserDoctorByUsernameAsync("georgie");

            Assert.Null(user);
        }

        private static User CreateDoctorUser(string username, string specialty)
        {
            Doctor doctor;
            User user;

            doctor = new Doctor
            {
                Specialty = specialty,
                PhoneNumber = "6900000000"
            };

            user = new User
            {
                Username = username,
                Email = $"{username}@gmail.com",
                Password = "hashed_password",
                Firstname = "Stefanos",
                Lastname = "Motsos",
                RoleId = 2,
                Doctor = doctor
            };

            return user;
        }

        private static User CreateNonDoctorUser(string username)
        {
            User user;

            user = new User
            {
                Username = username,
                Email = $"{username}@gmail.com",
                Password = "hashed_password",
                Firstname = "George",
                Lastname = "Giwrgou",
                RoleId = 3,
                Doctor = null
            };

            return user;
        }

    }
}
