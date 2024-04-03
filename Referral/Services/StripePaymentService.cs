using Front.Model;
using Referral.Model;
using Stripe;
using Stripe.Checkout;

namespace Referral.Services;

public class StripePaymentService : IPaymentService
{
    private readonly IConfiguration _config;

    public StripePaymentService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateConnectedAccount()
    {
        // Create a connected account
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        var options = new AccountCreateOptions { Type = "express" };
        var service = new AccountService();
        var account = service.Create(options);
        return account.Id;
    }

    public string GenerateAccountLink(string accountId)
    {
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        // Generate an Account Link
        var options = new AccountLinkCreateOptions
        {
            Account = accountId,
            RefreshUrl = "https://niimantse.wordpress.com/",
            ReturnUrl = "https://niimants3.wordpress.com/",
            Type = "account_onboarding"
        };
        var service = new AccountLinkService();
        var accountLink = service.Create(options);

        return accountLink.Url;
    }
    
    public string StripePayment(decimal amountPaid, string paidTo, Guid transactionId)
    {
        var domain = "https://localhost:7010/";
        var successURL = $"https://localhost:7195/MakePayment/SuccessfulPayment?id={transactionId}";
        var cancelURL = $"https://localhost:7195/MakePayment/FailedPayment";
        
        var options = new SessionCreateOptions
        {
            SuccessUrl = successURL,
            CancelUrl = cancelURL,
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };
        var sessionLineItem = new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(Convert.ToDecimal(amountPaid) * 100),
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = paidTo
                }
            },
            Quantity = 1
        };
        options.LineItems.Add(sessionLineItem);
          
        var service = new SessionService();
        Session session = service.Create(options);

        return session.Url;
    }

    public string StripePayment(Guid transactionId, string amountPaid, string paidTo)
    {
        var domain = "https://localhost:7010/";
        var successURL = $"https://localhost:7195/MakePayment/SuccessfulPayment?id={transactionId}";
        var cancelURL = $"https://localhost:7195/MakePayment/FailedPayment";

        var options = new SessionCreateOptions
        {
            SuccessUrl = successURL,
            CancelUrl = cancelURL,
            Mode = "payment",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = amountPaid,
                    Quantity = 1,
                },
            },
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                ApplicationFeeAmount = 123,
                TransferData = new SessionPaymentIntentDataTransferDataOptions
                {
                    Destination = paidTo
                },
            },
        };
        var service = new SessionService();
        Session session = service.Create(options);
        return session.Url;
    }

    public void TopUpAccount()
    {
        // Set your secret key. Remember to switch to your live secret key in production.
        // See your keys here: https://dashboard.stripe.com/apikeys
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

        var options = new TopupCreateOptions
        {
            Amount = 2000,
            Currency = "usd",
            Description = "Top-up for week of May 31",
            StatementDescriptor = "Weekly top-up",
        };
        var service = new TopupService();
        service.Create(options);
    }

    public void PayBusiness()
    {
        // Set your secret key. Remember to switch to your live secret key in production.
        // See your keys here: https://dashboard.stripe.com/apikeys
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

        var options = new TransferCreateOptions
        {
            Amount = 1000,
            Currency = "usd",
            Destination = "{{CONNECTED_STRIPE_ACCOUNT_ID}}"
        };
        var service = new TransferService();
        var Transfer = service.Create(options);
    }

    public string CreatePrice(decimal amountPaid)
    {
        var options = new PriceCreateOptions
        {
            Currency = "usd",
            UnitAmount = (long)(amountPaid * 100),
            Recurring = null,
            ProductData = new PriceProductDataOptions { Name = "Gold Plan" }
        };
        var service = new PriceService();
        var session = service.Create(options);
        return session.Id;
    }
}