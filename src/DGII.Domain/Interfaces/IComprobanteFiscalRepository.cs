using DGII.Domain.Entities;

namespace DGII.Domain.Interfaces;

public interface IComprobanteFiscalRepository
{
    Task<IReadOnlyList<ComprobanteFiscal>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComprobanteFiscal>> GetByRncAsync(string rncCedula, CancellationToken cancellationToken = default);
}
