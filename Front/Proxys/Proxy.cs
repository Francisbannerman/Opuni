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

        string? createdClientId = null;
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            // var createdClient = JsonConvert.DeserializeObject<ClientDto>(responseBody);
            var createdClient = JsonConvert.DeserializeObject(responseBody);
            createdClientId = createdClient.ToString();
            return new ClientCreatedResponse
            {
                Response = response, CreatedClientId = createdClientId
            };
        }
        return null;
    }
    
    public async Task<string> MakePaymentProxy(AmountPaid amountPaid)
    {
        var jsonContent = JsonConvert.SerializeObject(amountPaid);
        string requestUrl = $"api/referral/makepayment?AmountPaid={amountPaid._AmountPaid}&ClientId={amountPaid._ClientId}";

        var response = await _httpClient.PostAsync(requestUrl,
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var redirectToPaymentPage = JsonConvert.DeserializeObject<string>(responseBody);
            return redirectToPaymentPage;
        }
        return null;
    }

    public async Task<HttpResponseMessage> GetAllClientsProxy()
    {
        string requestUrl = $"api/referral/getall";
        var response = await _httpClient.GetAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
        }
    }

    public async Task<HttpResponseMessage> GetClientProxy(string referralCode)
    {
        string requestUrl = $"api/referral/{referralCode}";
        var response = await _httpClient.GetAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
    }

    public async Task<HttpResponseMessage> DeleteClientProxy(string referralCode)
    {
        string requestUrl = $"api/referral/delete/{referralCode}";
        var response = await _httpClient.DeleteAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
        }
    }

    public async Task<string> MakeClientAdminProxy(string referralCode)
    {
        string requestUrl = $"api/referral/admin/{referralCode}";
        var jsonContent = JsonConvert.SerializeObject(referralCode);

        var response = await _httpClient.PutAsync(requestUrl, new StringContent
            (jsonContent, Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var redirectToPaymentPage = JsonConvert.DeserializeObject<string>(responseBody);
            return redirectToPaymentPage;
        }
        return null;
    }

    public async Task<string> MakeClientBusinessProxy(string referralCode)
    {
        string requestUrl = $"api/referral/business/{referralCode}";
        var jsonContent = JsonConvert.SerializeObject(referralCode);

        var response = await _httpClient.PutAsync(requestUrl, new StringContent
            (jsonContent, Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var redirectToPaymentPage = JsonConvert.DeserializeObject<string>(responseBody);
            return redirectToPaymentPage;
        }
        return null;
    }

    public async Task<HttpResponseMessage> HasAdminAccessProxy(string id)
    {
        string requestUrl = $"api/referral/hasadminaccess/{id}";

        var response = await _httpClient.GetAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
    }

    public async Task<HttpResponseMessage> DownloadGetAllProxy()
    {
        string requestUrl = $"api/referral/downloadreport";
        var response = await _httpClient.GetAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
    }
}