using ScreenshotService.Models;

namespace ScreenshotService.Helpers.Viewports;
public class ViewportStrategyRegistry
{
    private readonly Dictionary<ViewportType, IViewportStrategy> _map;

    public ViewportStrategyRegistry(IEnumerable<IViewportStrategy> strategies)
    {
        _map = strategies.ToDictionary(s => s.Type);
    }

    public IViewportStrategy Get(ViewportType type)
    {
        if (_map.TryGetValue(type, out var strategy))
            return strategy;

        throw new NotSupportedException($"Viewport type '{type}' is not supported.");
    }
}