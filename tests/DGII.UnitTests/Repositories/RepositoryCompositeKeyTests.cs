using DGII.Domain.Entities;
using DGII.Infrastructure.Context;
using DGII.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace DGII.UnitTests.Repositories;

public class RepositoryCompositeKeyTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<DgiiDbContext> _options;
    private readonly DgiiDbContext _context;

    public RepositoryCompositeKeyTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<DgiiDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new DgiiDbContext(_options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task ComprobanteFiscal_ShouldSupportMultipleReceiptsForSameRnc_WithDifferentNcf()
    {
        // Arrange
        var repo = new ComprobanteFiscalRepository(_context, NullLogger<ComprobanteFiscalRepository>.Instance);
        var rnc = "98754321012";

        _context.Contribuyentes.Add(new Contribuyente
        {
            RncCedula = rnc,
            Nombre = "JUAN PEREZ",
            Tipo = "PERSONA FISICA",
            Estatus = "activo"
        });

        _context.ComprobantesFiscales.AddRange(
            new ComprobanteFiscal { RncCedula = rnc, Ncf = "E310000000001", Monto = 200.00m, Itbis18 = 36.00m },
            new ComprobanteFiscal { RncCedula = rnc, Ncf = "E310000000002", Monto = 1000.00m, Itbis18 = 180.00m }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await repo.GetByRncAsync(rnc);

        // Assert
        result.Should().HaveCount(2);
        result.Sum(c => c.Itbis18).Should().Be(216.00m);
    }

    [Fact]
    public async Task ComprobanteFiscal_ShouldEnforceCompositePrimaryKey_WhenDuplicateRncAndNcfInserted()
    {
        // Arrange
        var rnc = "98754321012";
        _context.Contribuyentes.Add(new Contribuyente
        {
            RncCedula = rnc,
            Nombre = "JUAN PEREZ",
            Tipo = "PERSONA FISICA",
            Estatus = "activo"
        });

        _context.ComprobantesFiscales.Add(
            new ComprobanteFiscal { RncCedula = rnc, Ncf = "E310000000001", Monto = 200.00m, Itbis18 = 36.00m }
        );
        await _context.SaveChangesAsync();

        // Act: En un nuevo contexto sobre la misma conexión SQLite, intentar insertar la misma clave compuesta (RncCedula, Ncf)
        using var separateContext = new DgiiDbContext(_options);
        separateContext.ComprobantesFiscales.Add(
            new ComprobanteFiscal { RncCedula = rnc, Ncf = "E310000000001", Monto = 500.00m, Itbis18 = 90.00m }
        );

        // Assert: SQLite relacional debe arrojar DbUpdateException por violación de restricción de clave primaria compuesta
        var act = async () => await separateContext.SaveChangesAsync();
        await act.Should().ThrowAsync<DbUpdateException>();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
