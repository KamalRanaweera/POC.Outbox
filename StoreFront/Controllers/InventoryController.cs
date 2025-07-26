using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Dtos;
using StoreFront.Interfaces;
using StoreFront.Models;
using System.Text.Json;

namespace StoreFront.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<List<InventoryRecord>>> GetInventory()
        {
            return Ok(await _inventoryService.List());
        }

        [HttpPost("reset-inventory")]
        public async Task ResetQuantity()
        {
            await _inventoryService.ResetQuantity(10, 10);
        }


        // DELETE: api/Inventory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryRecord(int id)
        {
            return await _inventoryService.RemoveItem(id) ? Ok() : NoContent();
        }
    }
}
