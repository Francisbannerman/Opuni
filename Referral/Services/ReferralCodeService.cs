using System.Security.Cryptography;
using System.Text;
using Referral.Repositories;

namespace Referral.Services;

public class ReferralCodeService
{
    private readonly IUnitOfWork _unitOfWork;
    public ReferralCodeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    private string GenerateReferralCode(Guid userId)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId.ToString()));
            string referralCode = Convert.ToBase64String(hashBytes)
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .Substring(0, 5); 
            return referralCode;
        }
    }

    public string GenerateUserReferralCode(string firstName, string lastName, Guid id)
    {
        string userAcronym = $"{firstName[0].ToString().ToUpper()}{lastName[0].ToString().ToUpper()}";
        string referralCode;
        do
        {
            referralCode = userAcronym + GenerateReferralCode(id);
        } 
        while (_unitOfWork.Client.Exists(r => r.ReferralCode == referralCode));
        return referralCode;
    }

    public void IncreaseReferrersCountOfReferree(string clientDto)
    {
        var referrer =
            _unitOfWork.Client.GetSpecial(u => u.ReferralCode == clientDto);

        if (referrer.NumberOfTimeReferralHasBeenUsed == null)
        {
            referrer.NumberOfTimeReferralHasBeenUsed = 1;
        }
        referrer.NumberOfTimeReferralHasBeenUsed++;
        _unitOfWork.Client.Update(referrer);
    }
}