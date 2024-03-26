namespace Referral.Dtos.ReferralDto;

public class CreateClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TelephoneNumber { get; set; }
    public string CreatedUsingReferralCode { get; set; }
}