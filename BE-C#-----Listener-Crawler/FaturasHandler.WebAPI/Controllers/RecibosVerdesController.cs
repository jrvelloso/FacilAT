using FaturasHandler.Data.Models;
using FaturasHandler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FaturasHandler.WebAPI.Controllers
{
    [ApiController]
    [Route("api/recibos-verdes")]
    public class RecibosVerdesController : ControllerBase
    {
        private readonly IReciboVerdeService _reciboVerdeService;
        public RecibosVerdesController(IReciboVerdeService reciboVerdeService) => _reciboVerdeService = reciboVerdeService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRecibosVerdes(Guid userId, CancellationToken cancellationToken) =>
            Ok(await _reciboVerdeService.GetAllByUserAsync(userId, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> CreateReciboVerde([FromBody] ReciboVerde recibo, Guid userId, CancellationToken cancellationToken)
        {
            var result = await _reciboVerdeService.AddAsync(recibo, userId, cancellationToken);
            return CreatedAtAction(nameof(GetRecibosVerdes), new { userId = recibo.UserId }, result);
        }
    }
}