using Api.DTOs;

namespace Api.Services.Interfaces;

public interface IContractsService
{
    Task CreateContractAsync(CreateContractDTO dto, CancellationToken cancellationToken);
    Task MakePaymentAsync(ContractPaymentDTO dto, CancellationToken cancellationToken);
}
