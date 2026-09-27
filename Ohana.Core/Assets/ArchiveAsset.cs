namespace Ohana.Core.Assets;

public class ArchiveAsset : IAsset
{
    private readonly Dictionary<Type, IAsset> _assets = new Dictionary<Type, IAsset>();

    public string Name { get; set; }

    public void AddChildAssets(IReadOnlyList<IAsset> assets)
    {
        foreach (var asset in assets)
        {
            _assets.Add(asset.GetType(), asset);
        }
    }
}