using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SunriseBlazor.Models;

namespace SunriseBlazor.Services
{
    public class SunriseDbContext : DbContext
    {
        public SunriseDbContext(DbContextOptions<SunriseDbContext> options) : base(options)
        {

        }

        public DbSet<SunriseItem>? SunTime { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
