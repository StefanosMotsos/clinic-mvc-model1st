using ClinicApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ClinicMvcModelFirstContext Create()
        {
            DbContextOptions<ClinicMvcModelFirstContext> options;

            options = new DbContextOptionsBuilder<ClinicMvcModelFirstContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ClinicMvcModelFirstContext(options);
        }
    }
}
