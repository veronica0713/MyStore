using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace MyStore.Controllers
{
    public class AccountController : Controller
    {
        SignInManager<BoatChartesUser> _signInManager;
        EmailService _emailService;
        Braintree.BraintreeGateway _braintreeGateway;

        //using Microsoft.AspNetCore.Identity
        public AccountController(SignInManager<BoatChartesUser> signInManager, EmailService emailService, Braintree.BraintreeGateway braintreeGateway)
        {
            this._signInManager = signInManager;
            this._emailService = emailService;
            this._braintreeGateway = braintreeGateway;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Responds on GET/Account/Register:
        public IActionResult Register()
        {
            return View();
        }

        //Responds on POST/Account/Register:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BoatChartesUser newUser = new BoatChartesUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName =  model.LastName,
                    PhoneNumber = model.PhoneNumber
                };
                IdentityResult creationResult = await this._signInManager.UserManager.CreateAsync(newUser);
                if (creationResult.Succeeded)
                {
                    IdentityResult passwordResult = await this._signInManager.UserManager.AddPasswordAsync(newUser, model.Password);
                    if (passwordResult.Succeeded)
                    {

                        Braintree.CustomerSearchRequest search = new Braintree.CustomerSearchRequest();
                        search.Email.Is(model.Email);
                       var searchResult = await _braintreeGateway.Customer.SearchAsync(search);
                        if (searchResult.Ids.Count == 0)
                        {
                            //create a new Braintree customer
                            await _braintreeGateway.Customer.CreateAsync(new Braintree.CustomerRequest
                            {
                                Email = model.Email,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Phone = model.PhoneNumber
                            });
                        }
                        else
                        {
                            //update the existing Braintree customer
                            Braintree.Customer existingCustomer = searchResult.FirstItem;
                            await _braintreeGateway.Customer.UpdateAsync(existingCustomer.Id, new Braintree.CustomerRequest
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Phone = model.PhoneNumber
                            });
                        }


                        var confirmationToken = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(newUser);
                        
                            confirmationToken = System.Net.WebUtility.UrlEncode(confirmationToken);
                        
                            string currentUrl = Request.GetDisplayUrl();    //This will get me the URL for the current request
                        System.Uri uri = new Uri(currentUrl);   //This will wrap it in a "URI" object so I can split it into parts
                        string confirmationUrl = uri.GetLeftPart(UriPartial.Authority); //This gives me just the scheme + authority of the URI
                        confirmationUrl += "/account/confirm?id=" + confirmationToken + "&userId=" + System.Net.WebUtility.UrlEncode(newUser.Id);


                        await this._signInManager.SignInAsync(newUser, false);

                        var emailResult = await this._emailService.SendEmailAsync(
                            model.Email,
                            "Welcome to My Boat!",
                            "<p>Thanks for signing up, " + model.UserName + "!</p><p><a href=\"" + confirmationUrl + "\">Confirm your account<a></p>",
                            "Thanks for signing up, " + model.UserName + "!"
                                                        );
                        if (!emailResult.Success)
                                                    {
                            throw new Exception(string.Join(',', emailResult.Errors.Select(x => x.Message)));
                                                   }
                                                return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in passwordResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in creationResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }
            return View();
        }
        //Create the Views and any additional Models required for the functionality below:
        //Check the methods on UserManager and SignInManager to figure out how to do this
        //Beware of online examples!  Things might be renamed in code you read, or it might be DotNetFramework4.6
        //Update your Layout to display the correct links depending on whether the user is logged in / out

        
        public async Task<IActionResult> SignOut()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BoatChartesUser existingUser = await this._signInManager.UserManager.FindByNameAsync(model.UserName);

                if (existingUser != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult passwordResult = await this._signInManager.CheckPasswordSignInAsync(existingUser, model.Password, false);
                    if (passwordResult.Succeeded)
                    {
                        this._signInManager.SignInAsync(existingUser, false).Wait();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordIncorrect", "Username or Password is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserDoesNotExist", "Username or Password is incorrect.");

                }
            }
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if ((ModelState.IsValid) && (!string.IsNullOrEmpty(email)))
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(email);
                if(user != null)
                {
                    var resetToken = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

                    resetToken = System.Net.WebUtility.UrlEncode(resetToken);
                    //using Microsoft.AspNetCore.Http.Extensions;
                    string currentUrl = Request.GetDisplayUrl();    //This will get me the URL for the current request
                    System.Uri uri = new Uri(currentUrl);   //This will wrap it in a "URI" object so I can split it into parts
                    string resetUrl = uri.GetLeftPart(UriPartial.Authority); //This gives me just the scheme + authority of the URI
                    resetUrl += "/account/resetpassword?id=" + resetToken + "&userId=" + System.Net.WebUtility.UrlEncode(user.Id);

                    string htmlContent = "<a href=\"" + resetUrl + "\">Reset your password</a>";
                    var emailResult = await _emailService.SendEmailAsync(email, "Reset your password", htmlContent, resetUrl);
                    if (!emailResult.Success)                   
                    {
                        
                        throw new Exception(string.Join(',', emailResult.Errors.Select(x => x.Message)));
                    }
                    return RedirectToAction("ResetSent");
                }
            }
            ModelState.AddModelError("email", "Email is not valid");
            return View();
        }


        public IActionResult ResetSent()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string userId, string password)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _signInManager.UserManager.ResetPasswordAsync(user, id, password);
                return RedirectToAction("LogIn");
            }
            return BadRequest();
        }

        public async Task<IActionResult> Confirm(string id, string userId)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(userId);
            if(user != null)
            {
                await _signInManager.UserManager.ConfirmEmailAsync(user, id);
                return RedirectToAction("Index", "Home");
            }
            return BadRequest();

            
        }

    }
}