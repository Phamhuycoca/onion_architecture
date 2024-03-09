using Microsoft.EntityFrameworkCore;
using onion_architecture.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Infrastructure.Context
{
    public class onion_architecture_Context:DbContext
    {
        public onion_architecture_Context(DbContextOptions<onion_architecture_Context> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(e => e.UserId);
                e.ToTable("User");
            });
        }
    }
}
