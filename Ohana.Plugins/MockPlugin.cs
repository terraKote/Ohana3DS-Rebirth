using Ohana.Core.Plugins;

namespace Ohana.Plugins
{
    public class MockPlugin : IPlugin
    {
        public string Id => "MockPlugin";
        public string Name => "Mock Plugin";
        public string Description =>  "This is Mock Plugin description.";
    }
}
