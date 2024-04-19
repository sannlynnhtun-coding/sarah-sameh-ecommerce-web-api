using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICartRepository _cartRepository;

        public PaymentController(UserManager<ApplicationUser> userManager, IPaymentRepository paymentRepository, ICartRepository cartRepository)
        {
            this._userManager = userManager;
            this._paymentRepository = paymentRepository;
            this._cartRepository = cartRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAllPayment()
        {
            var paymentsWithUserNames = _paymentRepository.GetAll()
              .Select(payment => new
              {
                  payment.Id,
                  payment.Date,
                  payment.Method,
                  payment.Amount,
                  CustomerName = payment.Customer.UserName,
                  CustomerEmail = payment.Customer.Email
              })
              .ToList();

            //return Ok(paymentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = paymentsWithUserNames
            };
            return response;
        }

        [HttpGet("{username}")]
        [Authorize]
        public ActionResult<GeneralResponse> GetAllByUserName(string username)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
            {
                return NotFound($"User '{username}' not found.");
            }
            var paymentsWithUserNames = _paymentRepository.GetAll()
            .Where(payment => payment.Customer.UserName == username)
            .Select(payment => new
            {
                payment.Id,
                payment.Date,
                payment.Method,
                payment.Amount,

                CustomerName = payment.Customer.UserName,
                CustomerEmail = payment.Customer.Email
            })
            .ToList();

            // return Ok(paymentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = paymentsWithUserNames
            };
            return response;
        }



        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralResponse>> AddPayment(PaymentDto paymentDto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (ModelState.IsValid)
            {
                // var currentUser = await userManager.GetUserAsync(User);
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var payment = new Payment
                {
                    Date = paymentDto.Date,
                    Method = paymentDto.Method,
                    Amount = _cartRepository.GetTotalPrice(id),
                    Customer = currentUser,
                    CustomerId = currentUser.Id
                };

                _paymentRepository.Insert(payment);
                _paymentRepository.Save();
                // return Ok();
                return new GeneralResponse
                {
                    IsPass = true,
                    Message = new
                    {
                        payment.Id,
                        payment.Method,
                        payment.Amount

                    }
                };
            }
            else
            {
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Cant Add Payment"
                };
                return localresponse;
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, PaymentDto updatedPayment)
        {
            Payment oldPayment = _paymentRepository.GetById(id);
            if (oldPayment == null)
            {
                GeneralResponse localResponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Not Found"
                };
                return localResponse;
            }
            oldPayment.Date = updatedPayment.Date;
            oldPayment.Method = updatedPayment.Method;
            oldPayment.Amount = updatedPayment.Amount;


            _paymentRepository.Update(oldPayment);
            _paymentRepository.Save();
            // return NoContent();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = new
                {
                    oldPayment.Id,
                    oldPayment.Date,
                    oldPayment.Method,
                    oldPayment.Amount

                }
            };
            return response;

        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public ActionResult<GeneralResponse> Remove(int id)
        {
            try
            {
                _paymentRepository.Delete(id);
                _paymentRepository.Save();
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = true,
                    Message = "Done"
                };
                return Ok(localresponse);
            }
            catch (Exception ex)
            {
                GeneralResponse response = new GeneralResponse()
                {
                    IsPass = false,
                    Message = ex.Message

                };
                return StatusCode(500, response);
            }
        }

    }
}
