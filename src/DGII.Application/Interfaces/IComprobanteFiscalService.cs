using DGII.Application.DTOs;

namespace DGII.Application.Interfaces;

public interface IComprobanteFiscalService
{
    Task<IReadOnlyList<ComprobanteFiscalDto>> GetAllComprobantesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComprobanteFiscalDto>> GetComprobantesByRncAsync(string rncCedula, CancellationToken cancellationToken = default);
    Task<ContribuyenteComprobantesResumenDto> GetResumenPorContribuyenteAsync(string rncCedula, CancellationToken cancellationToken = default);
}
