using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Rendering;
public class ScreenshotRendererRegistry
{
    private readonly Dictionary<FormatType, IScreenshotRenderer> _map;

    public ScreenshotRendererRegistry(IEnumerable<IScreenshotRenderer> renderers)
    {
        _map = renderers.ToDictionary(r => r.Type);
    }

    public IScreenshotRenderer Get(FormatType type)
    {
        if (_map.TryGetValue(type, out var renderer))
            return renderer;

        throw new NotSupportedException($"Format type '{type}' is not supported.");
    }
}