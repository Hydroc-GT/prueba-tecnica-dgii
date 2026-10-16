using DGII.Application.DTOs;
using DGII.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DGII.API.Controllers;

[ApiController]
[Route("api/comprobantes-fiscales")]
[Produces("application/json")]
public class ComprobantesFiscalesController : ControllerBase
{
    private readonly IComprobanteFiscalService _comprobanteFiscalService;
    private readonly ILogger<ComprobantesFiscalesController> _logger;

    public ComprobantesFiscalesController(
        IComprobanteFiscalService comprobanteFiscalService,
        ILogger<ComprobantesFiscalesController> logger)
    {
        _comprobanteFiscalService = comprobanteFiscalService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ComprobanteFiscalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ComprobanteFiscalDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _comprobanteFiscalService.GetAllComprobantesAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en {Controller} al procesar solicitud GetAll comprobantes fiscales", nameof(ComprobantesFiscalesController));
            throw;
        }
    }
}
