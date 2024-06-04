using Lab6.DTOs;
using Lab6.Models;

namespace Lab6.Services;

public interface IPrescriptionService
{
    Task AddPrescription(AddPrescriptionDTO dto, CancellationToken cancellationToken);

}
