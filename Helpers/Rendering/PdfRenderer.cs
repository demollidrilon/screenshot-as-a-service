using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Rendering;
public class PdfRenderer : IScreenshotRenderer
{
    public FormatType Type => FormatType.Pdf;
    public Task<byte[]> RenderAsync(IPage page)
    {
        return page.PdfDataAsync();
    }
}