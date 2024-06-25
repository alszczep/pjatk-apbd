using Api.DTOs;

namespace Api.Services.Interfaces;

public interface ISubscriptionsService
{
    Task CreateSubscriptionAsync(CreateSubscriptionDTO dto, CancellationToken cancellationToken);
    Task MakePaymentAsync(SubscriptionPaymentDTO dto, CancellationToken cancellationToken);
}
