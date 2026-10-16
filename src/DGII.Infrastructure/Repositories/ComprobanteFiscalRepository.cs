using DGII.Domain.Entities;
using DGII.Domain.Interfaces;
using DGII.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DGII.Infrastructure.Repositories;

public class ComprobanteFiscalRepository : IComprobanteFiscalRepository
{
    private readonly DgiiDbContext _context;
    private readonly ILogger<ComprobanteFiscalRepository> _logger;

    public ComprobanteFiscalRepository(DgiiDbContext context, ILogger<ComprobanteFiscalRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ComprobanteFiscal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.ComprobantesFiscales
                .AsNoTracking()
                .OrderBy(c => c.Ncf)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Repository} al consultar todos los comprobantes fiscales", nameof(ComprobanteFiscalRepository));
            throw;
        }
    }

    public async Task<IReadOnlyList<ComprobanteFiscal>> GetByRncAsync(string rncCedula, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.ComprobantesFiscales
                .AsNoTracking()
                .Where(c => c.RncCedula == rncCedula)
                .OrderBy(c => c.Ncf)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Repository} al consultar comprobantes fiscales para el RNC/Cédula {RncCedula}", nameof(ComprobanteFiscalRepository), rncCedula);
            throw;
        }
    }
}
