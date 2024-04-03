namespace Front.Models;

public class ClientCreatedResponse
{
    public HttpResponseMessage Response { get; set; }
    public Guid? CreatedClientId { get; set; }
}