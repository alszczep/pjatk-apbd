using api.Models;

namespace Tests.Fakes;

public static class FakesConsts
{
    public static readonly Guid NotExistingId = Guid.Parse("00000000-0000-0000-0000-000000000000");

    public static readonly string ExistingCurrency = "USD";
    public static readonly decimal ExistingCurrencyMultiplier = 0.5m;

    public static readonly Discount Discount1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Description = "ActiveLower",
        DiscountPercentage = 5,
        StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
        EndDate = DateTime.Now.Add(TimeSpan.FromDays(30))
    };

    public static readonly Discount Discount2 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Description = "ActiveHigher",
        DiscountPercentage = 10,
        StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
        EndDate = DateTime.Now.Add(TimeSpan.FromDays(30))
    };

    public static readonly Discount Discount3 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
        Description = "Inactive",
        DiscountPercentage = 15,
        StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
        EndDate = DateTime.Now.Subtract(TimeSpan.FromDays(20))
    };

    public static readonly SoftwareProduct SoftwareProduct1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = "Name1",
        Version = "Version1",
        Category = "Category1",
        Description = "Description1",
        UpfrontYearlyPriceInPln = 12000
    };

    public static readonly SoftwareProduct SoftwareProduct2 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Name = "Name2",
        Version = "Version2",
        Category = "Category2",
        Description = "Description2",
        UpfrontYearlyPriceInPln = 24000,
        Discounts = new List<Discount>
        {
            Discount1,
            Discount2,
            Discount3
        }
    };

    public static readonly ContractPayment ContractPayment1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        PaymentAmountInPln = 1000
    };

    public static readonly Contract ContractUnsignedForSoftwareProduct1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        SoftwareProduct = SoftwareProduct1,
        StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
        EndDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
        PriceInPlnAfterDiscounts = 5000,
        IsSigned = false,
        YearsOfExtendedSupport = 1
    };

    public static readonly Contract ContractUnsignedForSoftwareProduct2WithPayment = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        SoftwareProduct = SoftwareProduct2,
        StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
        EndDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
        PriceInPlnAfterDiscounts = 5000,
        IsSigned = false,
        YearsOfExtendedSupport = 1,
        Payments = new List<ContractPayment>
        {
            ContractPayment1
        }
    };

    public static readonly Contract ContractSignedAndActiveForSoftwareProduct1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
        SoftwareProduct = SoftwareProduct1,
        StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
        EndDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
        PriceInPlnAfterDiscounts = 5000,
        IsSigned = true,
        SignedDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)),
        YearsOfExtendedSupport = 1
    };

    public static readonly Contract ContractSignedAndInactiveForSoftwareProduct1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
        SoftwareProduct = SoftwareProduct1,
        StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(1000)),
        EndDate = DateTime.Now.Subtract(TimeSpan.FromDays(990)),
        PriceInPlnAfterDiscounts = 5000,
        IsSigned = true,
        SignedDate = DateTime.Now.Subtract(TimeSpan.FromDays(995)),
        YearsOfExtendedSupport = 1
    };

    public static readonly Subscription SubscriptionActiveForSoftwareProduct1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        SoftwareProduct = SoftwareProduct1,
        AddedDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(10))),
        BasePriceForRenewalPeriod = 10000,
        RenewalPeriodInMonths = 12,
        Payments = new List<SubscriptionPayment>
        {
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                PeriodLastDay = DateOnly.FromDateTime(DateTime.Now.AddMonths(12).Subtract(TimeSpan.FromDays(11))),
                AmountPaid = 10000
            }
        }
    };

    public static readonly ClientCompany ClientCompany1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Krs = "Krs1",
        CompanyName = "CompanyName1",
        Address = "CompanyAddress1",
        PhoneNumber = "CompanyPhoneNumber1",
        Email = "CompanyEmail1",
        Type = ClientType.Company,
        Contracts = new List<Contract>
        {
            ContractSignedAndInactiveForSoftwareProduct1
        }
    };

    public static readonly ClientIndividual ClientIndividual1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Pesel = "Pesel1",
        FirstName = "FirstName1",
        LastName = "LastName1",
        Address = "IndividualAddress1",
        PhoneNumber = "IndividualPhoneNumber1",
        Email = "IndividualEmail1",
        Type = ClientType.Individual,
        Contracts = new List<Contract>
        {
            ContractUnsignedForSoftwareProduct1
        }
    };

    public static readonly ClientIndividual ClientIndividual2 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
        Pesel = "Pesel2",
        FirstName = "FirstName2",
        LastName = "LastName2",
        Address = "IndividualAddress2",
        PhoneNumber = "IndividualPhoneNumber2",
        Email = "IndividualEmail2",
        Type = ClientType.Individual,
        Contracts = new List<Contract>
        {
            ContractSignedAndActiveForSoftwareProduct1
        }
    };

    public static readonly ClientIndividual ClientIndividual3 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
        Pesel = "Pesel3",
        FirstName = "FirstName3",
        LastName = "LastName3",
        Address = "IndividualAddress3",
        PhoneNumber = "IndividualPhoneNumber3",
        Email = "IndividualEmail3",
        Type = ClientType.Individual,
        Subscriptions = new List<Subscription>
        {
            SubscriptionActiveForSoftwareProduct1
        }
    };

    public static readonly ClientIndividual ClientIndividual4 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
        Pesel = "Pesel4",
        FirstName = "FirstName4",
        LastName = "LastName4",
        Address = "IndividualAddress4",
        PhoneNumber = "IndividualPhoneNumber4",
        Email = "IndividualEmail4",
        Type = ClientType.Individual
    };
}
