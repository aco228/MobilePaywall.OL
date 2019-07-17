using Cashflow.Client;
using Cashflow.Message;
using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Helpers
{

  public class TransactionResult
  {
    public Cashflow.Message.Data.Transaction Transaction;
    public string Username;
    public string Password;
  }

  public class CashflowProvider
  {
    public static TransactionResult GetTransaction(string transactionGuidInput, string paymentGuidInput, string transactioGroupInput)
    {
      Guid transactionGuid = Guid.Empty;
      Guid paymentGuid = Guid.Empty;
      Guid transactionGroupGuid = Guid.Empty;

      if (!Guid.TryParse(transactionGuidInput, out transactionGuid) || !Guid.TryParse(paymentGuidInput, out paymentGuid) || !Guid.TryParse(transactioGroupInput, out transactionGroupGuid))
        return null;

      Payment payment = Payment.CreateManager().Load(paymentGuid, GuidType.Internal);
      if (payment == null)
        return null;

      ServiceLookupMethodMap slmm = ServiceLookupMethodMap.CreateManager().Load(payment.ServiceOffer.Service, payment.PaymentRequest.Customer.Country, LookupMethod.Wap);

      TransactionClient transactionClient = new TransactionClient();
      transactionClient.AttachLogWriter(new CashflowLog(payment.ServiceOffer.Service));

      GetTransactionsRequest getTransactionRequest = new GetTransactionsRequest(RequestMode.Default,
                                                                                payment.ExternalPaymentGuid.ToString(),
                                                                                slmm.PaymentConfiguration.PaymentCredentials.Username,
                                                                                slmm.PaymentConfiguration.PaymentCredentials.Password,
                                                                                transactionGroupGuid,
                                                                                null);

      GetTransactionsResponse getTransactionResponse = transactionClient.GetTransactions(getTransactionRequest, null);

      foreach (Cashflow.Message.Data.Transaction transaction in getTransactionResponse.Transactions)
        if (transaction.TransactionID == transactionGuid)
          return new TransactionResult()
          {
            Transaction = transaction,
            Username = slmm.PaymentConfiguration.PaymentCredentials.Username,
            Password = slmm.PaymentConfiguration.PaymentCredentials.Password
          };

      return null;
    }

    public static bool Refund(TransactionResult transaction)
    {
      return Refund(transaction.Transaction, transaction.Username, transaction.Password);
    }

    public static bool Refund(Cashflow.Message.Data.Transaction transaction, string username, string password)
    {
      if (transaction == null)
        return false;

      TransactionClient transactionClient = new TransactionClient();

      RefundTransactionRequest refundTransactionRequest =
        new RefundTransactionRequest(RequestMode.Synchronous,
                                     transaction.TransactionID.ToString(),
                                     username,
                                     password,
                                     transaction.TransactionID,
                                     RefundReason.Custom,
                                     "MP: Refunding our payments!",
                                     null);

      RefundTransactionResponse refundTransactionResponse = transactionClient.RefundTransaction(refundTransactionRequest, null);
      return refundTransactionResponse.Status.Code == MessageStatusCode.Success;
    }


    public static bool Cancel(string paymentGuidString)
    {
      Guid paymentGuid = Guid.Empty;
      if (!Guid.TryParse(paymentGuidString, out paymentGuid))
      {
        return false;
      }

      Payment payment = Payment.CreateManager().Load(paymentGuid, GuidType.Internal);
      if (payment == null)
        return false;

      ServiceLookupMethodMap slmm = ServiceLookupMethodMap.CreateManager(2).Load(payment.ServiceOffer.Service, payment.PaymentRequest.Customer.Country, LookupMethod.Wap);

      string referenceId = payment.ExternalPaymentGuid.Value.ToString();
      string username = slmm.PaymentConfiguration.PaymentCredentials.Username;
      string password = slmm.PaymentConfiguration.PaymentCredentials.Password;
      CancelSubscriptionRequest request = new CancelSubscriptionRequest(RequestMode.Synchronous, referenceId, username, password, payment.ExternalPaymentGuid.Value, SubscriptionCancellationMode.Interactive, null);
      SubscriptionClient client = new SubscriptionClient();
      client.AttachLogWriter(new CashflowLog(payment.ServiceOffer.Service));
      CancelSubscriptionResponse response = client.CancelSubscription(request);
      if (response == null || response.Status == null || response.Status.Code != MessageStatusCode.Success)
      {
        // TODO: logging error
        return false;
      }
      // TODO: logging subscription canceled
      return true;
    }

  }
}
