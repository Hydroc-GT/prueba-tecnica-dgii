using System.Reflection;
using DGII.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DGII.Infrastructure.Context;

public class DgiiDbContext : DbContext
{
    public DgiiDbContext(DbContextOptions<DgiiDbContext> options) : base(options)
    {
    }

    public DbSet<Contribuyente> Contribuyentes => Set<Contribuyente>();
    public DbSet<ComprobanteFiscal> ComprobantesFiscales => Set<ComprobanteFiscal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
