using FaturasHandler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// ✅ Controllers
[ApiController]
[Route("api/users")]
public class UserDataController : ControllerBase
{
    private readonly IUserDataService _userService;

    public UserDataController(IUserDataService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.RemoveAsync(userId, cancellationToken);
        if (!result)
            return NotFound();

        return Ok();
    }
}