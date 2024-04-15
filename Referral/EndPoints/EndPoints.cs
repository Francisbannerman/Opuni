using System.Collections;
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
    private readonly IPaymentService _paymentService;
    private readonly ReportService _report;
    public EndPoints(IUnitOfWork unitOfWork,
        ReferralCodeService referralCodeService, IPaymentService paymentService,
        ReportService report)
    {
        _unitOfWork = unitOfWork;
        _referralCodeService = referralCodeService;
        _paymentService = paymentService;
        _report = report;
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
            Id = id, FirstName = clientDto.FirstName, LastName = clientDto.LastName,
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

    public void DeleteClientEndPoint(string referralCode)
    {
        var existingClient = _unitOfWork.Client.GetSpecial(u => u.ReferralCode == referralCode);
        if (existingClient == null)
        {
            throw new Exception("client not found");
        }
        _unitOfWork.Client.Remove(existingClient);
        _unitOfWork.Save();
    }

    public string MakeClientAdminEndPoint(string referralCode)
    {
        var existingClient = _unitOfWork.Client.GetSpecial(u => u.ReferralCode == referralCode);
        if (existingClient == null)
        {
            throw new Exception("client could not be found");
        }
        var stripeAccNum = _paymentService.CreateConnectedAccount();
        existingClient.IsBusiness = true;
        existingClient.StripeAccountId = stripeAccNum;
        existingClient.StripeAccountLink = _paymentService.GenerateAccountLink(stripeAccNum);
        existingClient.Role = SD.Role_Admin;
        _unitOfWork.Client.Update(existingClient);
        _unitOfWork.Save();
        return existingClient.StripeAccountLink;
    }

    public string MakeClientBusinessEndPoint(string referralCode)
    {
        var existingClient = _unitOfWork.Client.GetSpecial(u => u.ReferralCode == referralCode);
        if (existingClient == null)
        {
            throw new Exception("client could not be found");
        }
        var stripeAccNum = _paymentService.CreateConnectedAccount();
        existingClient.IsBusiness = true;
        existingClient.StripeAccountId = stripeAccNum;
        existingClient.StripeAccountLink = _paymentService.GenerateAccountLink(stripeAccNum);
        existingClient.Role = SD.Role_Business;
        _unitOfWork.Client.Update(existingClient);
        _unitOfWork.Save();
        return existingClient.StripeAccountLink;
    }

    public ClientDto GetClientEndPoint(string referralCode)
    {
        var client = _unitOfWork.Client.GetSpecial(u => u.ReferralCode == referralCode);
        if (client == null)
        {
            throw new Exception("client could not be found");
        }
        return client.AsDto();
    }

    public IEnumerable<ClientDto> GetAllClientsEndPoint()
    {
        var clients = _unitOfWork.Client.GetAll().Select(client => client.AsDto());
        if (clients == null)
        {
            throw new Exception("client could not be found");
        }
        return clients.ToList();
    }

    public string MakePaymentEndPoint(AmountPaid amountPaid)
    {
        var client = _unitOfWork.Client.GetSpecial(u => u.Id == Guid.Parse(amountPaid._ClientId));
        var clientName = $"{client.FirstName} {client.LastName}";
        var paidTo = _unitOfWork.Client.GetSpecial(u => u.ReferralCode == client.CreatedUsingReferralCode);
        
        if (paidTo.StripeAccountId == null && paidTo.IsBusiness == false)
        {
            throw new Exception(
                "This business is not a business. You can't send money to someone who is not a business on this platform");
        }
        var sessionUrl = _paymentService.StripePayment
            (client.Id, _paymentService.CreatePrice(amountPaid._AmountPaid),
                paidTo.StripeAccountId);
        
        client.AmountPaid = amountPaid._AmountPaid;
        _unitOfWork.Client.Update(client);
        _unitOfWork.Save();
        return sessionUrl;
    }

    public bool HasAdminAccess(string id)
    {
        var client = _unitOfWork.Client.GetSpecial(u => u.Id == Guid.Parse(id));
        var referrer = _unitOfWork.Client.GetSpecial(u => u.ReferralCode == client.CreatedUsingReferralCode);
        if (referrer.Role == SD.Role_Admin)
        {
            return true;
        }
        return false;
    }

    public byte[] DownloadReport()
    {
        return _report.ExcelReport();
    }
}