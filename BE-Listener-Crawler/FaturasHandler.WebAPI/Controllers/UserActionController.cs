using FaturasHandler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FaturasHandler.WebAPI.Controllers
{

    [ApiController]
    [Route("api/user-actions")]
    public class UserActionsController : ControllerBase
    {
        private readonly IUserActionService _userActionService;
        public UserActionsController(IUserActionService userActionService) => _userActionService = userActionService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserActions(Guid userId, CancellationToken cancellationToken) =>
            Ok(await _userActionService.GetUserActionsByUserAsync(userId, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> CreateUserAction([FromBody] Guid userId, CancellationToken cancellationToken)
        {
            var result = await _userActionService.CreateUserActionAsync(userId, cancellationToken);
            return CreatedAtAction(nameof(GetUserActions), new { userId = userId }, result);
        }
    }
}
