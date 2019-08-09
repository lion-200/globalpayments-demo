using GlobalPayments.Api;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace globalpayments.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Pay()
        {
            var card = new CreditCardData();

            // create the card object
            //var card = new CreditCardData
            //{
            //    Number = "4263970000005262",
            //    ExpMonth = 12,
            //    ExpYear = 2025,
            //    Cvn = "131",
            //    CardHolderName = "James Mason"
            //};

            //ProcessPayment(card);

            return View(card);
        }

        public void ProcessPayment(CreditCardData model)
        {
            // configure client & request settings
            ServicesContainer.ConfigureService(new GatewayConfig
            {
                MerchantId = "nutextileslondon",
                //MerchantId = "realexsandbox",
                AccountId = "internet",
                SharedSecret = "secret",
                //SharedSecret = "Po8lRRT67a",
                ServiceUrl = "https://api.sandbox.realexpayments.com/epage-remote.cgi"
                //ServiceUrl = "https://test.realexpayments.com/epage-remote.cgi"
            });

            try
            {
                // process an auto-settle authorization
                var response = model.Charge(129.99m)
                               .WithCurrency("EUR")
                               .Execute();

                var result = response.ResponseCode; // 00 == Success
                var message = response.ResponseMessage; // [ test system ] AUTHORISED

                // get the reponse details to save to the DB for future transaction management requests
                var orderId = response.OrderId;
                var authCode = response.AuthorizationCode;
                var paymentsReference = response.TransactionId; // pasref
            }

            catch (ApiException exce)
            {
                // TODO: add your error handling here
            }
        }

        [HttpPost]
        public ActionResult Pay(CreditCardData model)
        {
            ProcessPayment(model);           

            return View(model);
        }
    }
}