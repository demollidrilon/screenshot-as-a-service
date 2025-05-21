using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Viewports;
public interface IViewportStrategy
{
    ViewportType Type { get; }
    Task ApplyAsync(IPage page);
}