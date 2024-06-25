using Api.ExternalServices.Interfaces;
using Newtonsoft.Json;

namespace Api.ExternalServices;

internal class ExchangeRateResponse
{
    public Rate[] Rates { get; set; }
}

internal class Rate
{
    public decimal Mid { get; set; }
}

public class NBPService : INBPService
{
    private readonly HttpClient httpClient;

    public NBPService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<decimal?> PriceInPlnToCurrencyAsync(decimal priceInPln, string targetCurrency,
        CancellationToken cancellationToken)
    {
        try
        {
            string response =
                await this.httpClient.GetStringAsync("https://api.nbp.pl/api/exchangerates/rates/a/" + targetCurrency,
                    cancellationToken);
            ExchangeRateResponse? exchangeRate = JsonConvert.DeserializeObject<ExchangeRateResponse>(response);

            if (exchangeRate == null || exchangeRate.Rates.Length == 0)
                return null;

            decimal exchangeRateValue = exchangeRate.Rates[0].Mid;

            return priceInPln / exchangeRateValue;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
