using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationPeriodController : ControllerBase
{
    private readonly RegistrationPeriodService _registrationPeriodService;

    public RegistrationPeriodController(RegistrationPeriodService registrationPeriodService)
    {
        _registrationPeriodService = registrationPeriodService;
    }

    /// <summary>
    /// The registration window currently in effect. Returns a null body when no
    /// window has been configured, which the UI treats as closed.
    /// </summary>
    [HttpGet("current")]
    public async Task<ActionResult<RegistrationPeriod?>> GetCurrent()
    {
        var period = await _registrationPeriodService.GetCurrentAsync();
        return Ok(period);
    }
}
