using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Viewports;
public class DesktopViewportStrategy : IViewportStrategy
{
    public ViewportType Type => ViewportType.Desktop;
    public Task ApplyAsync(IPage page)
    {
        return Task.CompletedTask;
    }
}