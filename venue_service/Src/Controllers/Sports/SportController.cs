using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Interfaces.SportInterfaces;

namespace venue_service.Src.Controllers.Sports;

[Authorize]
[ApiController]
[Route("api/sports")]
public class SportController : ControllerBase
{
    private readonly ISportInterface _sportInterface;

    public SportController(ISportInterface sportInterface)
    {
        _sportInterface = sportInterface;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSports()
    {
        var result = await _sportInterface.GetAllSportsAsync();
        return Ok(result);
    }
}
