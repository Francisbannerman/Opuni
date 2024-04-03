using Referral.Dtos.ReferralDto;

namespace Referral.EndPoints;

public interface IEndPoints
{
    public ClientDto CreateClientEndPoint(CreateClientDto clientDto);
}