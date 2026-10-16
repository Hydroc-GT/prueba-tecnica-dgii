using DGII.Domain.Entities;
using DGII.Domain.Interfaces;
using DGII.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DGII.Infrastructure.Repositories;

public class ContribuyenteRepository : IContribuyenteRepository
{
    private readonly DgiiDbContext _context;
    private readonly ILogger<ContribuyenteRepository> _logger;

    public ContribuyenteRepository(DgiiDbContext context, ILogger<ContribuyenteRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Contribuyente>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Contribuyentes
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Repository} al consultar todos los contribuyentes desde la base de datos", nameof(ContribuyenteRepository));
            throw;
        }
    }

    public async Task<Contribuyente?> GetByRncAsync(string rncCedula, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Contribuyentes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.RncCedula == rncCedula, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Repository} al consultar contribuyente con RNC/Cédula {RncCedula}", nameof(ContribuyenteRepository), rncCedula);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string rncCedula, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Contribuyentes
                .AsNoTracking()
                .AnyAsync(c => c.RncCedula == rncCedula, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Repository} al verificar existencia de contribuyente con RNC/Cédula {RncCedula}", nameof(ContribuyenteRepository), rncCedula);
            throw;
        }
    }
}
