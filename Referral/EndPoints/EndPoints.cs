using Referral.Dtos.ReferralDto;
using Referral.Model;
using Referral.Repositories;
using Referral.Services;
using Stripe;

namespace Referral.EndPoints;

public class EndPoints : IEndPoints
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ReferralCodeService _referralCodeService;
    public EndPoints(IUnitOfWork unitOfWork,
        ReferralCodeService referralCodeService)
    {
        _unitOfWork = unitOfWork;
        _referralCodeService = referralCodeService;
    }
    
    public ClientDto CreateClientEndPoint(CreateClientDto clientDto)
    {
        if (!_unitOfWork.Client.Exists(r => r.ReferralCode == clientDto.CreatedUsingReferralCode))
        {
            throw new Exception("You used a non-existent referral code. Re-look at the referral code " +
                              "and try again with the appropriate referral code");
        }
        var id = Guid.NewGuid();
        Client client = new()
        {
            Id = id, FirstName = clientDto.FirstName, LastName = clientDto.FirstName,
            TelephoneNumber = clientDto.TelephoneNumber, 
            ReferralCode = _referralCodeService.GenerateUserReferralCode(clientDto.FirstName, clientDto.LastName, id),
            DateCreated = DateTimeOffset.Now, CreatedUsingReferralCode = clientDto.CreatedUsingReferralCode,
            NumberOfTimeReferralHasBeenUsed = 0, Role = SD.Role_Client, IsBusiness = false
        };
        _unitOfWork.Client.Add(client);
        _referralCodeService.IncreaseReferrersCountOfReferree(clientDto.CreatedUsingReferralCode);

        _unitOfWork.Save();
        return client.AsDto();
    }
}