using Referral.Dtos.ReferralDto;
using Referral.Model;

namespace Referral.EndPoints;

public interface IEndPoints
{
    ClientDto CreateClientEndPoint(CreateClientDto clientDto);
    void DeleteClientEndPoint(string referralCode);
    string MakeClientAdminEndPoint(string referralCode);
    string MakeClientBusinessEndPoint(string referralCode);
    ClientDto GetClientEndPoint(string referralCode);
    IEnumerable<ClientDto> GetAllClientsEndPoint();
    string MakePaymentEndPoint(AmountPaid amountPaid);
    bool HasAdminAccess(string id);
    byte[] DownloadReport();
}