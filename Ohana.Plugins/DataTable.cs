namespace Ohana.Plugins.CGFX;

public struct DictionaryDataEntry
{
    public uint NameOffset { get; }
    public uint DataOffset { get; }

    public DictionaryDataEntry(uint nameOffset, uint dataOffset)
    {
        NameOffset = nameOffset;
        DataOffset = dataOffset;
    }
}

public class DataTable
{
    public int Length { get; }
    public IReadOnlyList<DictionaryDataEntry> Models { get; }
    public IReadOnlyList<DictionaryDataEntry> Textures { get; }
    public IReadOnlyList<DictionaryDataEntry> LookUpTables { get; }
    public IReadOnlyList<DictionaryDataEntry> Materials { get; }
    public IReadOnlyList<DictionaryDataEntry> Shaders { get; }
    public IReadOnlyList<DictionaryDataEntry> Cameras { get; }
    public IReadOnlyList<DictionaryDataEntry> Lights { get; }
    public IReadOnlyList<DictionaryDataEntry> Fogs { get; }
    public IReadOnlyList<DictionaryDataEntry> Scenes { get; }
    public IReadOnlyList<DictionaryDataEntry> SkeletalAnimations { get; }
    public IReadOnlyList<DictionaryDataEntry> MaterialAnimations { get; }
    public IReadOnlyList<DictionaryDataEntry> VisibilityAnimations { get; }
    public IReadOnlyList<DictionaryDataEntry> CameraAnimations { get; }
    public IReadOnlyList<DictionaryDataEntry> LightAnimations { get; }
    public IReadOnlyList<DictionaryDataEntry> Emitters { get; }

    public DataTable(int length, IReadOnlyList<DictionaryDataEntry> models, IReadOnlyList<DictionaryDataEntry> textures,
        IReadOnlyList<DictionaryDataEntry> lookUpTables, IReadOnlyList<DictionaryDataEntry> materials,
        IReadOnlyList<DictionaryDataEntry> shaders, IReadOnlyList<DictionaryDataEntry> cameras,
        IReadOnlyList<DictionaryDataEntry> lights, IReadOnlyList<DictionaryDataEntry> fogs,
        IReadOnlyList<DictionaryDataEntry> scenes, IReadOnlyList<DictionaryDataEntry> skeletalAnimations,
        IReadOnlyList<DictionaryDataEntry> materialAnimations, IReadOnlyList<DictionaryDataEntry> visibilityAnimations,
        IReadOnlyList<DictionaryDataEntry> cameraAnimations, IReadOnlyList<DictionaryDataEntry> lightAnimations,
        IReadOnlyList<DictionaryDataEntry> emitters)
    {
        Length = length;
        Models = models;
        Textures = textures;
        LookUpTables = lookUpTables;
        Materials = materials;
        Shaders = shaders;
        Cameras = cameras;
        Lights = lights;
        Fogs = fogs;
        Scenes = scenes;
        SkeletalAnimations = skeletalAnimations;
        MaterialAnimations = materialAnimations;
        VisibilityAnimations = visibilityAnimations;
        CameraAnimations = cameraAnimations;
        LightAnimations = lightAnimations;
        Emitters = emitters;
    }
}