using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Front.Models;
using Newtonsoft.Json;
using Referral.Dtos.ReferralDto;

namespace Front.Proxys;

public class Proxy
{
    private readonly HttpClient _httpClient;
    public Proxy()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7144/"); // Update with your Web API URL
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<ClientCreatedResponse> CreateClientProxy
        (CreateClientDto clientDto)
    {
        var jsonContent = JsonConvert.SerializeObject(clientDto);
        var response = await _httpClient.PostAsync("api/referral/create",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));

        Guid? createdClientId = null;
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdClient = JsonConvert.DeserializeObject<ClientDto>(responseBody);
            createdClientId = createdClient?.Id;
        }
        return new ClientCreatedResponse
        {
            Response = response, CreatedClientId = createdClientId
        };
    }
    
    public async Task<string> MakePaymentProxy
        (decimal amountPaid, string clientId)
    {
        string requestUrl = $"api/referral/makepayment?AmountPaid={amountPaid}&ClientId={clientId}";
        var requestData = new
        {
            AmountPaid = amountPaid.ToString(), ClientId = clientId
        };
        var jsonContent = JsonConvert.SerializeObject(requestData);
        var response = await _httpClient.PostAsync(requestUrl, new 
            StringContent(jsonContent, Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            return null;
        }
    }
}