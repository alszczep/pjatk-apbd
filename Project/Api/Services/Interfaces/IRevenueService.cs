using Api.DTOs;

namespace Api.Services.Interfaces;

public interface IRevenueService
{
    Task<decimal> GetCurrentRevenueAsync(RevenueDTO dto, CancellationToken cancellationToken);
    Task<decimal> GetPredictedRevenueAsync(RevenueDTO dto, CancellationToken cancellationToken);
}
