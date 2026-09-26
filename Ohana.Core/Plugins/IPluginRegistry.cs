namespace Ohana.Core.Plugins;

public interface IPluginRegistry
{
    void Register<TCapability>(TCapability capability) where TCapability : class;
    IReadOnlyList<TCapability> GetAll<TCapability>();
}

public class PluginRegistry : IPluginRegistry
{
    private readonly HashSet<object> _capabilities = [];

    public void Register<TCapability>(TCapability capability) where TCapability : class
    {
        _capabilities.Add(capability);
    }

    public IReadOnlyList<TCapability> GetAll<TCapability>()
    {
        return _capabilities.OfType<TCapability>().ToList();
    }
}