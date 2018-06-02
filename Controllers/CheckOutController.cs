using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyStore.Migrations;
using MyStore.Models;
using SmartyStreets.USAutocompleteApi;

namespace MyStore.Controllers
{
    public class CheckOutController : Controller
    {
        private BoatChartersDbContext _oContext;
        private EmailService _emailService;
        private SignInManager<BoatChartesUser> _signInManager;
        private BraintreeGateway _braintreeGateway;
        private SmartyStreets.USStreetApi.Client _usStreetApiClient;

        public CheckOutController(BoatChartersDbContext oContext,
            EmailService emailService,
            SignInManager<BoatChartesUser> signInManager,
            Braintree.BraintreeGateway braintreeGateway, SmartyStreets.USStreetApi.Client usStreetApiClient)
        {
            _oContext = oContext;
            _emailService = emailService;
            _signInManager = signInManager;
            _braintreeGateway = braintreeGateway;
            _usStreetApiClient = usStreetApiClient;
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

                // load cart
                if (Request.Cookies.ContainsKey("cartId"))
                {
                    if (Guid.TryParse(Request.Cookies["cartId"], out var cartId))
                    {
                        model.Cart = await _oContext.Carts
                            .Include(carts => carts.CartItems)
                            .ThenInclude(cartitems => cartitems.Yacht)
                            .FirstOrDefaultAsync(x => x.CookieIdentifier == cartId);
                    }


                   

                    

                }
                
                Order order = new Order
                {
                    TrackingNumber = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.Now,
                    OrderItems = model.Cart.CartItems.Select(x => new OrderItem
                    {
                        
                        Yacht = x.Yacht,

                        DatesFrom = x.DatesFrom,
                        DatesTo = x.DatesTo
                    }).ToArray(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Address = model.Address,
                    Country = model.Country,
                    Zip = model.Zip,
                    PhoneNumber = model.PhoneNumber

                };
              

                Braintree.Customer customer = null;
                Braintree.CustomerSearchRequest search = new Braintree.CustomerSearchRequest();
                search.Email.Is(model.Email);
                var searchResult = await _braintreeGateway.Customer.SearchAsync(search);
                if (searchResult.Ids.Count == 0)
                {
                    //Create  a new Braintree Customer
                    Braintree.Result<Customer> creationResult =
                        await _braintreeGateway.Customer.CreateAsync(new Braintree.CustomerRequest
                        {
                            Email = model.Email,
                            Phone = model.PhoneNumber
                        });
                    customer = creationResult.Target;
                }
                else
                {
                    customer = searchResult.FirstItem;
                }


                //More on card testing: https://developers.braintreepayments.com/reference/general/testing/dotnet
                var transaction = new TransactionRequest
                {

                    Amount = model.Cart.CartItems.Sum(x => (x.Yacht.PriceHighSeason ?? 0)),

                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = model.CCnumber,
                        CardholderName = model.NameOnCard,
                        CVV = model.CVV,
                        ExpirationMonth = model.ExpirationMonth?.PadLeft(2, '0'),
                        ExpirationYear = model.ExpirationYear
                    },
                    CustomerId = customer.Id,
                    LineItems = model.Cart.CartItems.Select(x => new TransactionLineItemRequest
                    {
                        Name = x.Yacht.Name,
                       // Description = x.Yacht.Description,
                        ProductCode = x.Yacht.ID.ToString(),
                        Quantity = '1',
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = x.Yacht.PriceHighSeason, //* x.Quantity,
                        TotalAmount = x.Yacht.PriceHighSeason// * x.Quantity
                    }).ToArray()

                };
                var transactionResult = await _braintreeGateway.Transaction.SaleAsync(transaction);

                if (transactionResult.IsSuccess())
                {
                    _oContext.Orders.Add(order);
                    _oContext.CartItems.RemoveRange(model.Cart.CartItems);
                    _oContext.Carts.Remove(model.Cart);
                    await _oContext.SaveChangesAsync();
                    Response.Cookies.Delete("cartId");
                    return RedirectToAction("Receipt", new { id = order.ID });
                }

            }

            return View(model);
        }

        public IActionResult Receipt(int id)
        {
            var order = _oContext.Orders.FirstOrDefault(x => x.ID == id);


            return View(order);
        }
    }
}