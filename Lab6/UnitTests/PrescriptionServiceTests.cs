using Lab6.DTOs;
using Lab6.Services;
using Shouldly;
using UnitTests.Fakes;
using Xunit.Abstractions;

namespace UnitTests;

public class PrescriptionServiceTests
{
    private readonly ITestOutputHelper testOutputHelper;
    private IPrescriptionService prescriptionService;

    public PrescriptionServiceTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.prescriptionService = new PrescriptionService(
            new FakePrescriptionRepository(),
            new FakeMedicamentRepository(),
            new FakePatientRepository());
    }

    [Fact]
    public async void Should_ThrowException_WhenDateIsMoreThatDateDue()
    {
        var command = new AddPrescriptionDTO
        {
            Patient = new PatientDTO
            {
                IdPatient = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateOnly(1990, 1, 1)
            },
            Medicaments = new List<MedicamentDTO>
            {
                new MedicamentDTO
                {
                    IdMedicament = 1,
                    Details = "Details",
                    Dose = 1
                }
            },
            IdDoctor = 1,
            Date = new DateOnly(2022, 1, 1),
            DueDate = new DateOnly(2021, 1, 1)
        };

        await Should.ThrowAsync<InvalidOperationException>(this.prescriptionService.AddPrescription(command, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenThereAreMoreThan10Medicaments()
    {
        var command = new AddPrescriptionDTO
        {
            Patient = new PatientDTO
            {
                IdPatient = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateOnly(1990, 1, 1)
            },
            Medicaments = Enumerable.Repeat(new MedicamentDTO
                {
                    IdMedicament = 1,
                    Details = "Details",
                    Dose = 1
                }, 11).ToList(),
            IdDoctor = 1,
            Date = new DateOnly(2022, 1, 1),
            DueDate = new DateOnly(2023, 1, 1)
        };

        await Should.ThrowAsync<InvalidOperationException>(this.prescriptionService.AddPrescription(command, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenMedicamentDoesNotExist()
    {
        var command = new AddPrescriptionDTO
        {
            Patient = new PatientDTO
            {
                IdPatient = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateOnly(1990, 1, 1)
            },
            Medicaments = new List<MedicamentDTO>
            {
                new MedicamentDTO
                {
                    IdMedicament = 2,
                    Details = "Details",
                    Dose = 1
                }
            },
            IdDoctor = 1,
            Date = new DateOnly(2022, 1, 1),
            DueDate = new DateOnly(2023, 1, 1)
        };

        await Should.ThrowAsync<InvalidOperationException>(this.prescriptionService.AddPrescription(command, CancellationToken.None));
    }

    [Fact]
    public async void Should_NotThrowException_WhenCorrectDataIsPassed()
    {
        var command = new AddPrescriptionDTO
        {
            Patient = new PatientDTO
            {
                IdPatient = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateOnly(1990, 1, 1)
            },
            Medicaments = new List<MedicamentDTO>
            {
                new MedicamentDTO
                {
                    IdMedicament = 1,
                    Details = "Details",
                    Dose = 1
                }
            },
            IdDoctor = 1,
            Date = new DateOnly(2022, 1, 1),
            DueDate = new DateOnly(2023, 1, 1)
        };

        await Should.NotThrowAsync(this.prescriptionService.AddPrescription(command, CancellationToken.None));
    }
}
