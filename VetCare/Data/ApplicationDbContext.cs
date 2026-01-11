using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  
using Microsoft.EntityFrameworkCore;
using VetCare.Models;

namespace VetCare.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animale { get; set; }
        public DbSet<Programare> Programari { get; set; }
        public DbSet<Consultatie> Consultatii { get; set; }
        public DbSet<Vaccinare> Vaccinuri { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public object Programare { get; internal set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Programare>()
                .HasOne(p => p.Medic)
                .WithMany(u => u.ProgramariMedic)
                .HasForeignKey(p => p.VetId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Programare>()
                .HasOne(p => p.Animal)
                .WithMany(a => a.Programari)
                .HasForeignKey(p => p.PetId)
                .OnDelete(DeleteBehavior.Cascade); 


            builder.Entity<Review>()
                .HasOne(r => r.Client)
                .WithMany(u => u.ReviewuriDate)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Medic)
                .WithMany(u => u.ReviewuriPrimite)
                .HasForeignKey(r => r.VetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
