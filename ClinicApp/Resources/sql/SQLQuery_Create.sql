USE [ClinicMVCModelFirstDockerDB];
GO
 
-- ============================================
-- 1. ROLES
-- ============================================
CREATE TABLE [dbo].[Roles] (
    [Id]        INT             IDENTITY(1, 1) NOT NULL,
    [Name]      NVARCHAR(50)    NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Roles_Name] UNIQUE ([Name])
);
GO
 
CREATE NONCLUSTERED INDEX [IX_Roles_Name]
    ON [dbo].[Roles]([Name] ASC);
GO
 
-- ============================================
-- 2. CAPABILITIES
-- ============================================
CREATE TABLE [dbo].[Capabilities] (
    [Id]            INT             IDENTITY(1, 1) NOT NULL,
    [Name]          NVARCHAR(100)   NOT NULL,
    [Description]   NVARCHAR(255)   NULL,
    CONSTRAINT [PK_Capabilities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Capabilities_Name] UNIQUE ([Name])
);
GO
 
CREATE NONCLUSTERED INDEX [IX_Capabilities_Name]
    ON [dbo].[Capabilities]([Name] ASC);
GO
 
-- ============================================
-- 3. ROLES_CAPABILITIES (Many-to-Many)
-- ============================================
CREATE TABLE [dbo].[RolesCapabilities] (
    [RolesId]       INT NOT NULL,
    [CapabilitiesId] INT NOT NULL,
    CONSTRAINT [PK_RolesCapabilities] PRIMARY KEY CLUSTERED ([RolesId], [CapabilitiesId]),
 
    CONSTRAINT [FK_RolesCapabilities_Roles]
        FOREIGN KEY ([RolesId]) REFERENCES [dbo].[Roles]([Id])
        ON DELETE CASCADE,
 
    CONSTRAINT [FK_RolesCapabilities_Capabilities]
        FOREIGN KEY ([CapabilitiesId]) REFERENCES [dbo].[Capabilities]([Id])
        ON DELETE CASCADE
);
GO
 
CREATE NONCLUSTERED INDEX [IX_RolesCapabilities_CapabilityId]
    ON [dbo].[RolesCapabilities]([CapabilitiesId] ASC);
GO
 
-- ============================================
-- 4. USERS
-- ============================================
CREATE TABLE [dbo].[Users] (
    [Id]            INT             IDENTITY(1, 1) NOT NULL,
    [Username]      NVARCHAR(50)    NOT NULL,
    [Email]         NVARCHAR(50)    NOT NULL,
    [Password]      NVARCHAR(100)   NOT NULL,
    [Firstname]     NVARCHAR(50)    NOT NULL,
    [Lastname]      NVARCHAR(50)    NOT NULL,
    [RoleId]        INT             NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
 
    CONSTRAINT [FK_Users_Roles]
        FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
        ON DELETE NO ACTION
);
GO
 
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username]
    ON [dbo].[Users]([Username] ASC);
GO
 
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email]
    ON [dbo].[Users]([Email] ASC);
GO
 
CREATE NONCLUSTERED INDEX [IX_Users_RoleId]
    ON [dbo].[Users]([RoleId] ASC);
GO
 
-- ============================================
-- 5. DOCTORS
-- ============================================
CREATE TABLE [dbo].[Doctors] (
    [Id]            INT             IDENTITY(1, 1) NOT NULL,
    [Specialty]     NVARCHAR(50)    NOT NULL,
    [PhoneNumber]   NVARCHAR(20)    NULL,
    [UserId]        INT             NOT NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY CLUSTERED ([Id] ASC),
 
    CONSTRAINT [FK_Doctors_UserId]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
        ON DELETE CASCADE
);
GO
 
CREATE NONCLUSTERED INDEX [IX_Doctors_Specialty]
    ON [dbo].[Doctors]([Specialty] ASC);
GO
 
CREATE UNIQUE NONCLUSTERED INDEX [IX_Doctors_UserId]
    ON [dbo].[Doctors]([UserId] ASC);
GO
 
-- ============================================
-- 6. PATIENTS
-- ============================================
CREATE TABLE [dbo].[Patients] (
    [Id]            INT             IDENTITY(1, 1) NOT NULL,
    [AMKA]          NVARCHAR(11)    NOT NULL,
    [BloodType]     NVARCHAR(20)    NOT NULL,
    [DateOfBirth]   DATE            NULL,
    [UserId]        INT             NOT NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED ([Id] ASC),
 
    CONSTRAINT [FK_Patients_UserId]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
        ON DELETE CASCADE
);
GO
 
CREATE UNIQUE NONCLUSTERED INDEX [IX_Patients_AMKA]
    ON [dbo].[Patients]([AMKA] ASC);
GO
 
CREATE UNIQUE NONCLUSTERED INDEX [IX_Patients_UserId]
    ON [dbo].[Patients]([UserId] ASC);
GO
 
CREATE NONCLUSTERED INDEX [IX_Patients_BloodType]
    ON [dbo].[Patients]([BloodType] ASC);
GO
 
-- ============================================
-- 7. MEDICALPROGRAMS
-- ============================================
CREATE TABLE [dbo].[MedicalProgram] (
    [Id]            INT             IDENTITY(1, 1) NOT NULL,
    [Description]   NVARCHAR(255)   NULL,
    [DoctorId]      INT             NULL,
    CONSTRAINT [PK_MedicalProgram] PRIMARY KEY CLUSTERED ([Id] ASC),
 
    CONSTRAINT [FK_Programs_DoctorId]
        FOREIGN KEY ([DoctorId]) REFERENCES [dbo].[Doctors]([Id])
);
GO
 
CREATE NONCLUSTERED INDEX [IX_MedicalPrograms_Description]
    ON [dbo].[MedicalProgram]([Description] ASC);
GO
 
CREATE NONCLUSTERED INDEX [IX_MedicalPrograms_DoctorId]
    ON [dbo].[MedicalProgram]([DoctorId] ASC);
GO
 
-- ============================================
-- 8. PATIENTSPROGRAMS
-- ============================================
CREATE TABLE [dbo].[PatientsPrograms] (
    [ProgramsId]    INT NOT NULL,
    [PatientsId]    INT NOT NULL,
    CONSTRAINT [PK_PatientsPrograms] PRIMARY KEY CLUSTERED ([ProgramsId], [PatientsId]),
 
    CONSTRAINT [FK_PatientsPrograms_MedicalProgram]
        FOREIGN KEY ([ProgramsId]) REFERENCES [dbo].[MedicalProgram]([Id]),
 
    CONSTRAINT [FK_PatientsPrograms_Patients]
        FOREIGN KEY ([PatientsId]) REFERENCES [dbo].[Patients]([Id])
);
GO
 
CREATE INDEX [IX_PatientsPrograms_ProgramsId]
    ON [dbo].[PatientsPrograms]([ProgramsId]);
GO
 
CREATE INDEX [IX_PatientsPrograms_PatientsId]
    ON [dbo].[PatientsPrograms]([PatientsId]);
GO