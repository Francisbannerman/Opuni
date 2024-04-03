using Front.Model;
using Referral.Model;

namespace Referral.Services;

public interface IPaymentService
{
    string CreateConnectedAccount();
    string GenerateAccountLink(string id);
    string StripePayment(decimal amountPaid, string paidTo, Guid transactionId);
    public string StripePayment(Guid transactionId, string amountPaid, string paidTo);
    public string CreatePrice(decimal amountPaid);
}