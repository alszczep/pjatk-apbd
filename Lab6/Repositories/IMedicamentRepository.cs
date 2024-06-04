using Lab6.Models;

namespace Lab6.Repositories;

public interface IMedicamentRepository
{
    Task<bool> CheckIfMedicamentsExist(List<int> ids, CancellationToken cancellationToken);
}
