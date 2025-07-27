using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Dtos;
using StoreFront.Interfaces;
using StoreFront.Models;
using StoreFront.Services;
using System.Text.Json;

namespace StoreFront.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly StoreFrontDbContext _db;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService, StoreFrontDbContext db)
        {
            _mapper = mapper;
            _orderService = orderService;
            _db = db;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] OrderDto orderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                return await _orderService.PlaceOrder(order) ? Ok() : BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

        }
    }
}
