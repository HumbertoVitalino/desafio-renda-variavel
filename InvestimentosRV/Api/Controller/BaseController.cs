using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controller;

public abstract class BaseController : ControllerBase
{
    protected int UserId
    {
        get
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var userId))
                return userId;           

            throw new InvalidOperationException("User ID not found or invalid in token.");
        }
    }
}
