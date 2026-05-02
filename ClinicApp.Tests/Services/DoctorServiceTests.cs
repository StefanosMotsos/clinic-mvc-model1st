using AutoMapper;
using Castle.Core.Logging;
using ClinicApp.DTO;
using ClinicApp.Exceptions;
using ClinicApp.Models;
using ClinicApp.Repositories.UoW;
using ClinicApp.Security;
using ClinicApp.Services.DoctorService;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEncryptionUtil _encryptionUtil;
        private readonly ILogger<DoctorService> _logger;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _encryptionUtil = Substitute.For<IEncryptionUtil>();
            _logger = Substitute.For<ILogger<DoctorService>>();

            _service = new DoctorService(_unitOfWork, _mapper, _encryptionUtil, _logger);
        }

        [Fact]
        public async Task SignupUserAsync_WhenUsernameIsNew_AddsUserAndDoctorAndSaves()
        {
            DoctorSignupDTO doctorSignupDTO;
            Doctor mappedDoctor;
            User mappedUser;

            doctorSignupDTO = CreateValidSignupDTO("stef");
            mappedDoctor = new Doctor { Specialty = "Oncologist" };
            mappedUser = new User { Username = "stef", Password = "Cod1ngF@" };

            _mapper.Map<Doctor>(doctorSignupDTO).Returns(mappedDoctor);
            _mapper.Map<User>(doctorSignupDTO).Returns(mappedUser);
            _unitOfWork.UserRepository.GetUserByUsernameAsync("stef").Returns((User?)null);
            _encryptionUtil.Encrypt("Cod1ngF@").Returns("encrypted_password");

            await _service.SignUpUserAsync(doctorSignupDTO);

            await _unitOfWork.UserRepository.Received(1).AddAsync(mappedUser);
            await _unitOfWork.DoctorRepository.Received(1).AddAsync(mappedDoctor);
            await _unitOfWork.Received(1).SaveAsync();
        }

        [Fact]
        public async Task SignUpUserAsync_WhenUsernameAlreadyExists_ThrowsEntityAlreadyExistsException()
        {
            DoctorSignupDTO doctorSignupDTO;
            User existingUser;
            Doctor mappedDoctor;
            User mappedUser;

            doctorSignupDTO = CreateValidSignupDTO("stef");
            mappedDoctor = new Doctor { Specialty = "Oncologist" };
            mappedUser = new User { Username = "stef" };
            existingUser = new User { Id = 1, Username = "stef" };

            _mapper.Map<Doctor>(doctorSignupDTO).Returns(mappedDoctor);
            _mapper.Map<User>(doctorSignupDTO).Returns(mappedUser);
            _unitOfWork.UserRepository.GetUserByUsernameAsync("stef").Returns(existingUser);

            await Assert.ThrowsAsync<EntityAlreadyExistsException>(
                () => _service.SignUpUserAsync(doctorSignupDTO));

            await _unitOfWork.UserRepository.DidNotReceive().AddAsync(Arg.Any<User>());
            await _unitOfWork.DoctorRepository.DidNotReceive().AddAsync(Arg.Any<Doctor>());
            await _unitOfWork.DidNotReceive().SaveAsync();
        }

        [Fact]
        public async Task SignUpUserAsync_WhenUsernameIsNew_EncryptsPasswordBeforeSaving()
        {
            DoctorSignupDTO doctorSignupDTO;
            Doctor mappedDoctor;
            User mappedUser;

            doctorSignupDTO = CreateValidSignupDTO("stef");
            mappedDoctor = new Doctor { Specialty = "Oncologist" };
            mappedUser = new User { Username = "stef", Password = "Cod1ngF@" };

            _mapper.Map<Doctor>(doctorSignupDTO).Returns(mappedDoctor);
            _mapper.Map<User>(doctorSignupDTO).Returns(mappedUser);
            _unitOfWork.UserRepository.GetUserByUsernameAsync("stef").Returns((User?)null);
            _encryptionUtil.Encrypt("Cod1ngF@").Returns("encrypted_password");

            await _service.SignUpUserAsync(doctorSignupDTO);

            _encryptionUtil.Received(1).Encrypt("Cod1ngF@");
            Assert.Equal("encrypted_password", mappedUser.Password);
        }

        private static DoctorSignupDTO CreateValidSignupDTO(string username)
        {
            DoctorSignupDTO request = new()
            {
                Username = username,
                Email = $"{username}@gmail.com",
                Password = "Cod1ngF@",
                Firstname = "Stef",
                Lastname = "Motsos",
                PhoneNumber = "6900000000",
                Specialty = "Oncologist",
                RoleId = 2
            };

            return request;
        }
    }
}
