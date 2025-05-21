using PuppeteerSharp;
using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Rendering;
public interface IScreenshotRenderer
{
    FormatType Type { get; }
    Task<byte[]> RenderAsync(IPage page);
}