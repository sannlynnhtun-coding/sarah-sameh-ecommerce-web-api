using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentController(UserManager<ApplicationUser> userManager, IShipmentRepository shipmentRepository)
        {
            this._userManager = userManager;
            this._shipmentRepository = shipmentRepository;
        }
        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAll()
        {
            var shipmentsWithUserNames = _shipmentRepository.GetAll()
                .Select(shipment => new
                {
                    shipment.Id,
                    shipment.Date,
                    shipment.Address,
                    shipment.State,
                    shipment.City,
                    Zip_Code = shipment.ZipCode,
                    shipment.Country,
                    CustomerName = shipment.Customer.UserName,
                    CustomerEmail = shipment.Customer.Email
                })
                .ToList();

            // return Ok(shipmentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = shipmentsWithUserNames
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
            var shipmentsWithUserNames = _shipmentRepository.GetAll()
            .Where(shipment => shipment.Customer.UserName == username)
            .Select(shipment => new
            {
                shipment.Id,
                shipment.Date,
                shipment.Address,
                shipment.State,
                shipment.City,
                Zip_Code = shipment.ZipCode,
                shipment.Country,
                CustomerName = shipment.Customer.UserName,
                CustomerEmail = shipment.Customer.Email
            })
            .ToList();

            //return Ok(shipmentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = shipmentsWithUserNames
            };
            return response;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralResponse>> AddShipment(ShipmentDto shipmentDto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (ModelState.IsValid)
            {
                //var currentUser = await userManager.GetUserAsync(User);

                var shipment = new Shipment
                {
                    Date = shipmentDto.Date,
                    Address = shipmentDto.Address,
                    State = shipmentDto.State,
                    City = shipmentDto.City,
                    ZipCode = shipmentDto.ZipCode,
                    Country = shipmentDto.Country,
                    Customer = currentUser,
                    CustomerId = currentUser.Id


                };

                _shipmentRepository.Insert(shipment);
                _shipmentRepository.Save();
                //   return Ok();
                return new GeneralResponse
                {
                    IsPass = true,
                    Message = new
                    {
                        shipment.Id,
                        shipment.Date,
                        shipment.Address,
                        shipment.City,
                        shipment.Country,
                        shipment.State,
                        Zip_Code = shipment.ZipCode

                    }
                };
            }
            else
            {
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Cant Add Shipment"
                };
                return localresponse;
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, ShipmentDto updatedShipment)
        {
            Shipment oldShipment = _shipmentRepository.GetById(id);
            if (oldShipment == null)
            {
                // return NotFound();
                GeneralResponse localResponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Not Found"
                };
                return localResponse;
            }
            oldShipment.Address = updatedShipment.Address;
            oldShipment.Date = updatedShipment.Date;
            oldShipment.Country = updatedShipment.Country;
            oldShipment.City = updatedShipment.City;
            oldShipment.State = updatedShipment.State;
            oldShipment.ZipCode = updatedShipment.ZipCode;



            _shipmentRepository.Update(oldShipment);
            _shipmentRepository.Save();
            // return NoContent();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = new
                {
                    oldShipment.Id,
                    oldShipment.Date,
                    oldShipment.Address,
                    oldShipment.City,
                    oldShipment.Country,
                    oldShipment.State,
                    Zip_Code = oldShipment.ZipCode

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
                _shipmentRepository.Delete(id);
                _shipmentRepository.Save();
                // return NoContent();
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = true,
                    Message = "Done"
                };
                return Ok(localresponse);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
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
