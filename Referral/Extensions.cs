using Referral.Dtos.ReferralDto;
using Referral.Model;

namespace Referral;

public static class Extensions
{
    public static ClientDto AsDto(this Client client)
    {
        return new ClientDto
        {
            Id = client.Id, FirstName = client.FirstName, LastName = client.LastName,
            TelephoneNumber = client.TelephoneNumber, 
            ReferralCode = client.ReferralCode, DateCreated = client.DateCreated, 
            CreatedUsingReferralCode = client.CreatedUsingReferralCode,
            NumberOfTimeReferralHasBeenUsed = client.NumberOfTimeReferralHasBeenUsed,
            Role = client.Role, AmountPaid = client.AmountPaid, IsBusiness = client.IsBusiness,
            StripeAccountId = client.StripeAccountId,
            StripeAccountLink = client.StripeAccountLink
        };
    }
}

public static class SD
{
    public const string Role_Client = "Client";
    public const string Role_Admin = "Admin";
    public const string Role_Business = "Business";
}