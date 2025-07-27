using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Outbox.Shared.Dtos;
using ShipmentProcessor.Interfaces;
using ShipmentProcessor.Models;

namespace ShipmentProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly ShipmentDbContext _db;
        private readonly IMapper _mapper;
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IMapper mapper, IShipmentService shipmentService, ShipmentDbContext db)
        {
            _mapper = mapper;
            _shipmentService = shipmentService;
            _db = db;
        }

        [HttpPost("list-pending-shipments")]
        public async Task<IActionResult> ListPendingShipments()
        {
            try
            {
                return Ok(await _shipmentService.ListPendingShipments());
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("update-shipment")]
        public async Task<IActionResult> UpdateShipment(Guid shipmentId, ShipmentStatus status)
        {
            try
            {
                if (!(status == ShipmentStatus.InProgress || status == ShipmentStatus.Complete))
                    return BadRequest("status must be either InProgress or Complete");

                return Ok(await _shipmentService.UpdateShipment(shipmentId, status));
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

    }
}
