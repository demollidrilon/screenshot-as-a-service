using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Viewports;
public class TabletViewportStrategy : IViewportStrategy
{
    public ViewportType Type => ViewportType.Tablet;

    public Task ApplyAsync(IPage page)
    {
        return page.SetViewportAsync(new ViewPortOptions
        {
            Width = 768,
            Height = 1024,
            IsMobile = true,
            HasTouch = true,
            IsLandscape = false
        });
    }
}