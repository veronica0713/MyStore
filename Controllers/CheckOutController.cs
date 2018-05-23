using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyStore.Migrations;
using MyStore.Models;

namespace MyStore.Controllers
{
    public class CheckOutController : Controller
    {
        private BoatChartersDbContext _oContext;
        private EmailService _emailService;
        private SignInManager<BoatChartesUser> _signInManager;
        private BraintreeGateway _braintreeGateway;

        public CheckOutController(BoatChartersDbContext oContext,
            EmailService emailService,
            SignInManager<BoatChartesUser> signInManager,
            Braintree.BraintreeGateway braintreeGateway)
        {
            _oContext = oContext;
            _emailService = emailService;
            _signInManager = signInManager;
            _braintreeGateway = braintreeGateway;
        }



        public IActionResult Index()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {

            if (ModelState.IsValid)
            {
                Order order = new Order
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Address = model.Address,
                    Country = model.Country,
                    Zip = model.Zip,
                    PhoneNumber = model.PhoneNumber

                };
                //More on card testing: https://developers.braintreepayments.com/reference/general/testing/dotnet
                TransactionRequest transaction = new TransactionRequest
                {

                    Amount = model.Cart.CartItems.Sum(x => (x.Yacht.PriceHighSeason ?? 0)),
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = model.CCnumber,
                        CardholderName = model.NameOnCard,
                        CVV = model.CVV,

                        ExpirationYear = model.Expiration.ToString()
                    }

                };
                var transactionResult = await _braintreeGateway.Transaction.SaleAsync(transaction);

                if (transactionResult.IsSuccess())
                {




                    _oContext.Orders.Add(order);
                    _oContext.CartItems.RemoveRange(model.Cart.CartItems);
                    _oContext.Carts.Remove(model.Cart);
                    await _oContext.SaveChangesAsync();
                    Response.Cookies.Delete("cartId");

                }


            }

            return View();
        }
    }
}