using PuppeteerSharp;
using ScreenshotService.Models;
using ScreenshotService.Helpers.Rendering;
using ScreenshotService.Helpers.Viewports;

namespace ScreenshotService.Helpers;
public class PuppeteerHelper
{
    private readonly ScreenshotRendererRegistry _rendererRegistry;
    private readonly ViewportStrategyRegistry _viewportRegistry;

    public PuppeteerHelper(ScreenshotRendererRegistry rendererRegistry, ViewportStrategyRegistry viewportRegistry)
    {
        _rendererRegistry = rendererRegistry;
        _viewportRegistry = viewportRegistry;
    }

    public async Task<byte[]> CaptureScreenshotAsync(string url, string format, string viewport)
    {
        if (!Enum.TryParse<FormatType>(format, true, out var formatType))
            throw new ArgumentException($"Invalid format type: {formatType}");

        if (!Enum.TryParse<ViewportType>(viewport, true, out var viewportType))
            throw new ArgumentException($"Invalid viewport type: {viewport}");
        
        await new BrowserFetcher().DownloadAsync();

        var launchOptions = new LaunchOptions { Headless = true };
        await using var browser = await Puppeteer.LaunchAsync(launchOptions);
        await using var page = await browser.NewPageAsync();

        var strategy = _viewportRegistry.Get(viewportType);
        await strategy.ApplyAsync(page);

        await page.GoToAsync(url, new NavigationOptions
        {
            Timeout = 30000,
            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 }
        });

        var renderer = _rendererRegistry.Get(formatType);
        return await renderer.RenderAsync(page);
    }
}