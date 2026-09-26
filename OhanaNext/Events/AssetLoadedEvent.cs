using Ohana.Core.Assets;

namespace OhanaNext.Events;

public class AssetLoadedEvent
{
    public IAsset Asset { get; }

    public AssetLoadedEvent(IAsset asset)
    {
        Asset = asset;
    }
}