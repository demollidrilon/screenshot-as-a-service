using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Viewports;
public class MobileViewportStrategy : IViewportStrategy
{
    public ViewportType Type => ViewportType.Mobile;
    public Task ApplyAsync(IPage page)
    {
        return page.SetViewportAsync(new ViewPortOptions
        {
            Width = 375,
            Height = 812,
            IsMobile = true,
            HasTouch = true,
            IsLandscape = false
        });
    }
}