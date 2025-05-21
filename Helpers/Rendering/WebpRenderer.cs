using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Rendering;
public class WebpRenderer : IScreenshotRenderer
{
    public FormatType Type => FormatType.Webp;
    public Task<byte[]> RenderAsync(IPage page)
    {
        return page.ScreenshotDataAsync(new ScreenshotOptions
        {
            FullPage = true,
            Type = ScreenshotType.Webp
        });
    }
}