using DGII.Application.DTOs;
using DGII.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DGII.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContribuyentesController : ControllerBase
{
    private readonly IContribuyenteService _contribuyenteService;
    private readonly IComprobanteFiscalService _comprobanteFiscalService;
    private readonly ILogger<ContribuyentesController> _logger;

    public ContribuyentesController(
        IContribuyenteService contribuyenteService,
        IComprobanteFiscalService comprobanteFiscalService,
        ILogger<ContribuyentesController> logger)
    {
        _contribuyenteService = contribuyenteService;
        _comprobanteFiscalService = comprobanteFiscalService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ContribuyenteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ContribuyenteDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _contribuyenteService.GetAllContribuyentesAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Controller} al procesar solicitud GetAll", nameof(ContribuyentesController));
            throw;
        }
    }

    [HttpGet("{rncCedula}")]
    [ProducesResponseType(typeof(ContribuyenteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContribuyenteDto>> GetByRnc(string rncCedula, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _contribuyenteService.GetContribuyenteByRncAsync(rncCedula, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Controller} al procesar solicitud GetByRnc para {RncCedula}", nameof(ContribuyentesController), rncCedula);
            throw;
        }
    }

    [HttpGet("{rncCedula}/comprobantes-fiscales")]
    [ProducesResponseType(typeof(IReadOnlyList<ComprobanteFiscalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<ComprobanteFiscalDto>>> GetComprobantes(string rncCedula, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _comprobanteFiscalService.GetComprobantesByRncAsync(rncCedula, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Controller} al procesar comprobantes para el contribuyente {RncCedula}", nameof(ContribuyentesController), rncCedula);
            throw;
        }
    }

    [HttpGet("{rncCedula}/resumen")]
    [ProducesResponseType(typeof(ContribuyenteComprobantesResumenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContribuyenteComprobantesResumenDto>> GetResumen(string rncCedula, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _comprobanteFiscalService.GetResumenPorContribuyenteAsync(rncCedula, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Controller} al procesar resumen de ITBIS para el contribuyente {RncCedula}", nameof(ContribuyentesController), rncCedula);
            throw;
        }
    }
}
