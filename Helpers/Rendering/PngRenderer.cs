using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Rendering;
public class PngRenderer : IScreenshotRenderer
{
    public FormatType Type => FormatType.Png;
    public Task<byte[]> RenderAsync(IPage page)
    {
        return page.ScreenshotDataAsync(new ScreenshotOptions
        {
            FullPage = true,
            Type = ScreenshotType.Png
        });
    }
}