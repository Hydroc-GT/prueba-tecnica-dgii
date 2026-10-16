using DGII.Application.DTOs;

namespace DGII.Application.Interfaces;

public interface IContribuyenteService
{
    Task<IReadOnlyList<ContribuyenteDto>> GetAllContribuyentesAsync(CancellationToken cancellationToken = default);
    Task<ContribuyenteDto> GetContribuyenteByRncAsync(string rncCedula, CancellationToken cancellationToken = default);
}
