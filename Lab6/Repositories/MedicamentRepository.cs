using Lab6.Context;
using Lab6.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Repositories;

public class MedicamentRepository : IMedicamentRepository
{
    private PrescriptionsContext prescriptionsContext;

    public MedicamentRepository(PrescriptionsContext prescriptionsContext)
    {
        this.prescriptionsContext = prescriptionsContext;
    }

    public async Task<bool> CheckIfMedicamentsExist(List<int> ids, CancellationToken cancellationToken)
    {
        return (await this.prescriptionsContext.Medicaments
            .Where(m => ids.Contains(m.IdMedicament))
            .CountAsync(cancellationToken)) == ids.Count;
    }
}
