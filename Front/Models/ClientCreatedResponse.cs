namespace Front.Models;

public class ClientCreatedResponse
{
    public HttpResponseMessage Response { get; set; }
    public string? CreatedClientId { get; set; }
}