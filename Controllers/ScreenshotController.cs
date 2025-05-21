using Microsoft.AspNetCore.Mvc;
using ScreenshotService.Models;
using ScreenshotService.Services;

namespace ScreenshotService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ScreenshotController : ControllerBase
{
    private readonly ILogger<ScreenshotController> _logger;
    private readonly ScreenshotGenerator _generator;

    public ScreenshotController(ILogger<ScreenshotController> logger, ScreenshotGenerator generator)
    {
        _logger = logger;
        _generator = generator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateScreenshot([FromBody] ScreenshotRequest request)
    {
        _logger.LogInformation("Received screenshot request: {Url}, Format: {Format}, Viewport: {Viewport}", request.Url, request.Format, request.Viewport);

        var result = await _generator.GenerateAsync(request);

        return Ok(new ApiResponse<ScreenshotResponse>
        {
            Data = result
        });
    }
}