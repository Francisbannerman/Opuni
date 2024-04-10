using Referral.Dtos.ReferralDto;
using Referral.Model;

namespace Referral.EndPoints;

public interface IEndPoints
{
    public ClientDto CreateClientEndPoint(CreateClientDto clientDto);
    public void DeleteClientEndPoint(string referralCode);
    public string MakeClientAdminEndPoint(string referralCode);
    public string MakeClientBusinessEndPoint(string referralCode);
    public ClientDto GetClientEndPoint(string referralCode);
    public IEnumerable<ClientDto> GetAllClientsEndPoint();
    public string MakePaymentEndPoint(AmountPaid amountPaid);
}