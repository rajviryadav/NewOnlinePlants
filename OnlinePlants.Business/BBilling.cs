using OnlinePlants.Data;
using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePlants.Business.BusinessLogicModel;
using Stripe;


namespace OnlinePlants.Business
{
    public class BBilling
    {
        OnlinePlantsContext db = new OnlinePlantsContext();
        public IEnumerable<StripeCard> GetCustomerCardDetails(int RegId)
        {

            StripeConfiguration.SetApiKey("sk_test_TghHPnrj5SeWHFv39KJVg8M5");
            IEnumerable<StripeCard> response = null;
            try
            {
                var chargeService = new StripeChargeService();
                Payments payment = db.tblPayments.Where(a => a.RegID == RegId).FirstOrDefault();
                StripeCharge stripeCharge = chargeService.Get(payment.StripeId);
                var cardService = new StripeCardService();
                response = cardService.List(stripeCharge.CustomerId);

            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public ResponseMessage UpdateCustomerCardDetails(string cardId, string customerId, string ExpirationMonth, string ExpirationYear, string Name)
        {
            StripeConfiguration.SetApiKey("sk_test_TghHPnrj5SeWHFv39KJVg8M5");
            IEnumerable<StripeCard> cardDetails = null;
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var cardOptions = new StripeCardUpdateOptions()
                {
                    ExpirationMonth = ExpirationMonth,
                    ExpirationYear = ExpirationYear,
                    Name = Name
                };

                var cardService = new StripeCardService();
                StripeCard card = cardService.Update(customerId, cardId, cardOptions);
                cardDetails = cardService.List(customerId);
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.classobject = cardDetails;

            }

            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public IEnumerable<StripeCard> DeleteCustomerCard(string cardId, string customerId)
        {
            StripeConfiguration.SetApiKey("sk_test_TghHPnrj5SeWHFv39KJVg8M5");
            IEnumerable<StripeCard> response = null;
            try
            {

                var cardService = new StripeCardService();
                StripeDeleted card = cardService.Delete(customerId, cardId);
                response = cardService.List(customerId);


            }
            catch (Exception ex)
            {

            }
            return response;

        }

        public ResponseMessage AddCustomerCard(string Token, int RegId)
        {
            StripeConfiguration.SetApiKey("sk_test_TghHPnrj5SeWHFv39KJVg8M5");
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var cardOptions = new StripeCardCreateOptions()
                {
                    SourceToken = Token,

                };
                var chargeService = new StripeChargeService();
                Payments payment = db.tblPayments.Where(a => a.RegID == RegId).FirstOrDefault();
                StripeCharge stripeCharge = chargeService.Get(payment.StripeId);
                var cardService = new StripeCardService();
                StripeCard card = cardService.Create(stripeCharge.CustomerId, cardOptions);
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RException = ex.Message;

            }
            return responseMessage;

        }

        public ResponseMessage CustomerPaymentDetails(int RegId)
        {
            StripeConfiguration.SetApiKey("sk_test_TghHPnrj5SeWHFv39KJVg8M5");

            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var chargeService = new StripeChargeService();
                Payments payment = db.tblPayments.Where(a => a.RegID == RegId).FirstOrDefault();
                StripeCharge stripeCharge = chargeService.Get(payment.StripeId);
                string customerId = stripeCharge.CustomerId;
                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(customerId);
                List<CustomerInvoices> invoices = new List<CustomerInvoices>();
                CustomerInvoices invoice = new CustomerInvoices();
                invoice.Description = customer.Description;
                StripeTransfer stripeTransfer = Stripe.Mapper<StripeTransfer>.MapFromJson(payment.StripeObjectJson.ToString());
                invoice.Amount = stripeTransfer.Amount;
                invoice.Email = customer.Email;
                invoice.Paid = payment.Stripepaid.ToString();
                invoice.BalanceTransactionId = payment.BalanceTransactionId;
                invoice.Created = payment.CreatedDate.ToShortDateString();
                invoices.Add(invoice);
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccessAndWait;
                responseMessage.classobject = invoices;
                responseMessage.RURL = "/Packeges/Invoices/";
                
            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                //responseMessage.RMessage = ex.StackTrace;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RException = ex.Message;
            }

            return responseMessage;
        }
    }
}
