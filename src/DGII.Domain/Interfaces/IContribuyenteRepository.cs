using DGII.Domain.Entities;

namespace DGII.Domain.Interfaces;

public interface IContribuyenteRepository
{
    Task<IReadOnlyList<Contribuyente>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Contribuyente?> GetByRncAsync(string rncCedula, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string rncCedula, CancellationToken cancellationToken = default);
}
