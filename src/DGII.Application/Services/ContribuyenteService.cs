using System.Text.RegularExpressions;
using DGII.Application.DTOs;
using DGII.Application.Exceptions;
using DGII.Application.Interfaces;
using DGII.Domain.Entities;
using DGII.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Services;

public class ContribuyenteService : IContribuyenteService
{
    private static readonly Regex RncFormatRegex = new(@"^(\d{9}|\d{11})$", RegexOptions.Compiled);
    private readonly IContribuyenteRepository _contribuyenteRepository;
    private readonly ILogger<ContribuyenteService> _logger;

    public ContribuyenteService(
        IContribuyenteRepository contribuyenteRepository,
        ILogger<ContribuyenteService> logger)
    {
        _contribuyenteRepository = contribuyenteRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ContribuyenteDto>> GetAllContribuyentesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var contribuyentes = await _contribuyenteRepository.GetAllAsync(cancellationToken);

            return contribuyentes.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Service} al obtener el listado de contribuyentes", nameof(ContribuyenteService));
            throw;
        }
    }

    public async Task<ContribuyenteDto> GetContribuyenteByRncAsync(string rncCedula, CancellationToken cancellationToken = default)
    {
        ValidateRncCedula(rncCedula);

        try
        {
            var contribuyente = await _contribuyenteRepository.GetByRncAsync(rncCedula, cancellationToken);
            if (contribuyente == null)
            {
                throw new NotFoundException(nameof(Contribuyente), rncCedula);
            }

            return MapToDto(contribuyente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Service} al obtener el contribuyente con RNC/Cédula {RncCedula}", nameof(ContribuyenteService), rncCedula);
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

    private static ContribuyenteDto MapToDto(Contribuyente entity)
    {
        return new ContribuyenteDto
        {
            RncCedula = entity.RncCedula,
            Nombre = entity.Nombre,
            Tipo = entity.Tipo,
            Estatus = entity.Estatus
        };
    }
}
