using Ohana.Core.FileFormats;
using Ohana.Core.Plugins;

namespace Ohana.Plugins.CGFX
{
    public class CgfxPlugin : IPlugin
    {
        public PluginDescriptor Descriptor => new PluginDescriptor(
            "CGFX",
            "CGFX File Importer",
            [
                "Denys \"terraKote\" Kushnirenko",
                "gdkchan"
            ],
            new Version(0, 1));

        public void Register(IPluginRegistry registry)
        {
            registry.Register<IFileImporter>(new CgfxFileImporter());
        }
    }
}