using DGII.Domain.Entities;
using DGII.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DGII.Infrastructure.Seeding;

public static class DgiiDataSeeder
{
    public static async Task SeedAsync(DgiiDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Contribuyentes.AnyAsync())
            {
                return;
            }

            logger.LogInformation("Iniciando carga de datos iniciales (Seed Data)...");

            var contribuyentes = new List<Contribuyente>
            {
                new()
                {
                    RncCedula = "98754321012",
                    Nombre = "JUAN PEREZ",
                    Tipo = "PERSONA FISICA",
                    Estatus = "activo"
                },
                new()
                {
                    RncCedula = "123456789",
                    Nombre = "FARMACIA TU SALUD",
                    Tipo = "PERSONA JURIDICA",
                    Estatus = "inactivo"
                },
                new()
                {
                    RncCedula = "101001234",
                    Nombre = "SUPERMERCADO CENTRAL SRL",
                    Tipo = "PERSONA JURIDICA",
                    Estatus = "activo"
                },
                new()
                {
                    RncCedula = "00119876543",
                    Nombre = "ANA VALDEZ MARTINEZ",
                    Tipo = "PERSONA FISICA",
                    Estatus = "activo"
                }
            };

            await context.Contribuyentes.AddRangeAsync(contribuyentes);
            await context.SaveChangesAsync();

            var comprobantes = new List<ComprobanteFiscal>
            {
                new()
                {
                    RncCedula = "98754321012",
                    Ncf = "E310000000001",
                    Monto = 200.00m,
                    Itbis18 = 36.00m
                },
                new()
                {
                    RncCedula = "98754321012",
                    Ncf = "E310000000002",
                    Monto = 1000.00m,
                    Itbis18 = 180.00m
                },
                new()
                {
                    RncCedula = "101001234",
                    Ncf = "B010000000001",
                    Monto = 5000.00m,
                    Itbis18 = 900.00m
                },
                new()
                {
                    RncCedula = "00119876543",
                    Ncf = "E310000000003",
                    Monto = 1500.00m,
                    Itbis18 = 270.00m
                }
            };

            await context.ComprobantesFiscales.AddRangeAsync(comprobantes);
            await context.SaveChangesAsync();

            logger.LogInformation("Datos iniciales insertados: 4 Contribuyentes, 4 Comprobantes Fiscales.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al inicializar la base de datos y cargar el seed data");
            throw;
        }
    }
}
