using Lab6.Models;
using Lab6.Repositories;

namespace UnitTests.Fakes;

public class FakeMedicamentRepository: IMedicamentRepository
{
    public Task<bool> CheckIfMedicamentsExist(List<int> ids, CancellationToken cancellationToken)
    {
        return Task.FromResult(ids.All(id => id == 1));
    }
}
