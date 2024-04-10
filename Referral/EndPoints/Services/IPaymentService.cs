using Referral.Model;

namespace Referral.Services;

public interface IPaymentService
{
    string CreateConnectedAccount();
    string GenerateAccountLink(string id);
    //string StripePayment(decimal amountPaid, string paidTo, Guid transactionId);
    string StripePayment(Guid transactionId, string amountPaid, string paidTo);
    string CreatePrice(decimal amountPaid);
}