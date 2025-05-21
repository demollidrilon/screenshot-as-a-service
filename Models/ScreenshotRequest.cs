namespace ScreenshotService.Models;
public class ScreenshotRequest
{
    public string Url { get; set; } = string.Empty;
    public FormatType Format { get; set; } = FormatType.Png;
    public ViewportType Viewport { get; set; } = ViewportType.Desktop;
}