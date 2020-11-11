using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebLaba.Models
{
    public class WebLabaContext : DbContext
    {
        public virtual DbSet<Corporations> Corporations { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<Players> Players { get; set; }

        public WebLabaContext(DbContextOptions<WebLabaContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
