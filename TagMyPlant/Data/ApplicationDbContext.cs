using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TagMyPlant.Models;

namespace TagMyPlant.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
    }
}
