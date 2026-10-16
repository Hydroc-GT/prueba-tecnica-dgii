using System.Globalization;
using System.Text.RegularExpressions;
using DGII.Application.DTOs;
using DGII.Application.Exceptions;
using DGII.Application.Interfaces;
using DGII.Domain.Entities;
using DGII.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Services;

public class ComprobanteFiscalService : IComprobanteFiscalService
{
    private static readonly Regex RncFormatRegex = new(@"^(\d{9}|\d{11})$", RegexOptions.Compiled);
    private readonly IComprobanteFiscalRepository _comprobanteRepository;
    private readonly IContribuyenteRepository _contribuyenteRepository;
    private readonly ILogger<ComprobanteFiscalService> _logger;

    public ComprobanteFiscalService(
        IComprobanteFiscalRepository comprobanteRepository,
        IContribuyenteRepository contribuyenteRepository,
        ILogger<ComprobanteFiscalService> logger)
    {
        _comprobanteRepository = comprobanteRepository;
        _contribuyenteRepository = contribuyenteRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ComprobanteFiscalDto>> GetAllComprobantesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var comprobantes = await _comprobanteRepository.GetAllAsync(cancellationToken);
            return comprobantes.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Service} al obtener todos los comprobantes fiscales", nameof(ComprobanteFiscalService));
            throw;
        }
    }

    public async Task<IReadOnlyList<ComprobanteFiscalDto>> GetComprobantesByRncAsync(string rncCedula, CancellationToken cancellationToken = default)
    {
        ValidateRncCedula(rncCedula);

        try
        {
            var exists = await _contribuyenteRepository.ExistsAsync(rncCedula, cancellationToken);
            if (!exists)
            {
                throw new NotFoundException(nameof(Contribuyente), rncCedula);
            }

            var comprobantes = await _comprobanteRepository.GetByRncAsync(rncCedula, cancellationToken);
            return comprobantes.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Service} al obtener comprobantes fiscales para el RNC/Cédula {RncCedula}", nameof(ComprobanteFiscalService), rncCedula);
            throw;
        }
    }

    public async Task<ContribuyenteComprobantesResumenDto> GetResumenPorContribuyenteAsync(string rncCedula, CancellationToken cancellationToken = default)
    {
        ValidateRncCedula(rncCedula);

        try
        {
            var contribuyente = await _contribuyenteRepository.GetByRncAsync(rncCedula, cancellationToken);
            if (contribuyente == null)
            {
                throw new NotFoundException(nameof(Contribuyente), rncCedula);
            }

            var comprobantes = await _comprobanteRepository.GetByRncAsync(rncCedula, cancellationToken);
            var dtos = comprobantes.Select(MapToDto).ToList();

            decimal totalItbis = comprobantes.Sum(c => c.Itbis18);
            string totalItbisFormateado = string.Format(CultureInfo.InvariantCulture, "${0:0.00}", totalItbis);

            return new ContribuyenteComprobantesResumenDto
            {
                RncCedula = contribuyente.RncCedula,
                Nombre = contribuyente.Nombre,
                Tipo = contribuyente.Tipo,
                Estatus = contribuyente.Estatus,
                TotalItbis = totalItbis,
                TotalItbisFormateado = totalItbisFormateado,
                Comprobantes = dtos
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Service} al calcular resumen de ITBIS para el contribuyente con RNC/Cédula {RncCedula}", nameof(ComprobanteFiscalService), rncCedula);
            throw;
        }
    }

    private static void ValidateRncCedula(string rncCedula)
    {
        if (string.IsNullOrWhiteSpace(rncCedula) || !RncFormatRegex.IsMatch(rncCedula.Trim()))
        {
            throw new ValidationException("El formato del RNC o Cédula es inválido. Debe contener exactamente 9 u 11 dígitos numéricos sin guiones ni caracteres especiales.");
        }
    }

    private static ComprobanteFiscalDto MapToDto(ComprobanteFiscal entity)
    {
        return new ComprobanteFiscalDto
        {
            RncCedula = entity.RncCedula,
            Ncf = entity.Ncf,
            Monto = entity.Monto.ToString("0.00", CultureInfo.InvariantCulture),
            Itbis18 = entity.Itbis18.ToString("0.00", CultureInfo.InvariantCulture)
        };
    }
}
