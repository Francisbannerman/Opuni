namespace Referral.Model;

public class Client
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TelephoneNumber { get; set; }
    public string ReferralCode { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public string CreatedUsingReferralCode { get; set; }
    public int NumberOfTimeReferralHasBeenUsed { get; set; }
    public string Role { get; set; }
}