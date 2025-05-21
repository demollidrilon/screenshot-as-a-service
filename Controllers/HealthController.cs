using Microsoft.AspNetCore.Mvc;

namespace ScreenshotService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogDebug("Health check called");
        return Ok("Service is healthy!");
    }
}