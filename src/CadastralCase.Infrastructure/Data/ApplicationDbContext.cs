using CadastralCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CadastralCase.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NaturalPerson> NaturalPersons { get; set; }
    public DbSet<LegalPerson> LegalPersons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NaturalPerson>(entity =>
        {
            entity.ToTable("NaturalPersons");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.TaxId)
                .IsRequired()
                .HasMaxLength(11);

            entity.HasIndex(e => e.TaxId)
                .IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(200);

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .IsRequired();

            entity.HasOne(e => e.Address)
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<LegalPerson>(entity =>
        {
            entity.ToTable("LegalPersons");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(e => e.TradeName)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(e => e.TaxId)
                .IsRequired()
                .HasMaxLength(14);

            entity.HasIndex(e => e.TaxId)
                .IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(200);

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .IsRequired();

            entity.HasOne(e => e.Address)
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.SetNull);
        });

    }
}
