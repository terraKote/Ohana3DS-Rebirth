using System.Text;
using Ohana.Core.Assets;
using Ohana.Core.Extensions;
using Ohana.Core.FileFormats;

namespace Ohana.Plugins.CGFX;

public class CgfxFileImporter : IFileImporter
{
    private const uint BUFFER_SIZE = 4;
    private const string FILE_HEADER_MAGIC = "CGFX";
    private const string DATA_HEADER_MAGIC = "DATA";
    private const string DICTIONARY_HEADER_MAGIC = "DICT";

    public FileFormatDescriptor FormatDescriptor => new("CGFX", [".fs", ".bcres"]);

    public bool CanImport(Stream stream)
    {
        var buffer = new byte[BUFFER_SIZE];
        var fileHeader = ReadFileHeader(stream, buffer);
        return string.Equals(fileHeader.Magic, FILE_HEADER_MAGIC, StringComparison.Ordinal) &&
               fileHeader.Length == stream.Position;
    }

    public IAsset Import(Stream stream)
    {
        var buffer = new byte[BUFFER_SIZE];
        var fileHeader = ReadFileHeader(stream, buffer);

        if (stream.Position != fileHeader.Length)
        {
            throw new Exception("File header size mismatch");
        }

        stream.Position = fileHeader.Length;
        var dataTable = ReadDataTable(stream, buffer);

        return null;
    }

    private static FileHeader ReadFileHeader(Stream stream, byte[] buffer)
    {
        var magicString = stream.ReadString(FILE_HEADER_MAGIC.Length, buffer);
        var endianness = stream.ReadUInt16(buffer);
        var length = stream.ReadUInt16(buffer);
        var revision = stream.ReadUInt32(buffer);
        var fileLength = stream.ReadUInt32(buffer);
        var entryCount = stream.ReadUInt32(buffer);

        return new FileHeader(magicString, endianness, length, revision, fileLength, entryCount);
    }

    private DataTable ReadDataTable(Stream stream, byte[] buffer)
    {
        var magicString = stream.ReadString(DATA_HEADER_MAGIC.Length, buffer);

        if (!string.Equals(magicString, DATA_HEADER_MAGIC, StringComparison.Ordinal))
            throw new Exception("Invalid dictionary header");

        var length = stream.ReadInt32(buffer);

        var modelDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var textureDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var lookUpTablesDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var materialsDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var shadersDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var camerasDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var lightsDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var fogsDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var scenesDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var skeletalAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var materialAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var visibilityAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var cameraAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var lightAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var emittersDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);

        return new DataTable(length,
            modelDictionaryDataEntries,
            textureDictionaryDataEntries,
            lookUpTablesDictionaryDataEntries,
            materialsDictionaryDataEntries,
            shadersDictionaryDataEntries,
            camerasDictionaryDataEntries,
            lightsDictionaryDataEntries,
            fogsDictionaryDataEntries,
            scenesDictionaryDataEntries,
            skeletalAnimationDictionaryDataEntries,
            materialAnimationDictionaryDataEntries,
            visibilityAnimationDictionaryDataEntries,
            cameraAnimationDictionaryDataEntries,
            lightAnimationDictionaryDataEntries,
            emittersDictionaryDataEntries);
    }

    private static IReadOnlyList<DictionaryDataEntry> GetDictionaryDataEntry(Stream stream, byte[] buffer)
    {
        var entryCount = stream.ReadUInt32(buffer);
        var relativeOffset = GetDictionaryDataEntryRelativeOffset(stream, buffer);

        if (entryCount == 0)
            return [];

        var position = stream.Position;
        var dictionaryEntries = GetDictionaryDataEntrySection(stream, buffer, relativeOffset);
        stream.Position = position;

        return dictionaryEntries;
    }

    private static uint GetDictionaryDataEntryRelativeOffset(Stream stream, byte[] buffer)
    {
        var position = (uint)stream.Position;
        var offset = stream.ReadUInt32(buffer);

        if (offset != 0)
        {
            offset += position;
        }

        return offset;
    }

    private static IReadOnlyList<DictionaryDataEntry> GetDictionaryDataEntrySection(Stream stream, byte[] buffer,
        uint relativeOffset)
    {
        stream.Seek(relativeOffset, SeekOrigin.Begin);

        var magic = stream.ReadString(DICTIONARY_HEADER_MAGIC.Length, buffer);
        var length = stream.ReadUInt32(buffer);
        var entryCount = stream.ReadUInt32(buffer);
        var rootNodeReference = stream.ReadInt32(buffer);
        var rootNodeLeft = stream.ReadUInt16(buffer);
        var rootNodeRight = stream.ReadUInt16(buffer);
        var rootNodeNameOffset = stream.ReadUInt32(buffer);
        var rootNodeNameDataOffset = stream.ReadUInt32(buffer);

        var entries = new DictionaryDataEntry[entryCount];

        for (int i = 0; i < entryCount; i++)
        {
            var referenceBit = stream.ReadInt32(buffer);
            var nodeLeft = stream.ReadUInt16(buffer);
            var nodeRight = stream.ReadUInt16(buffer);

            var nameOffset = GetDictionaryDataEntryRelativeOffset(stream, buffer);
            var dataOffset = GetDictionaryDataEntryRelativeOffset(stream, buffer);

            entries[i] = new DictionaryDataEntry(nameOffset, dataOffset);
        }

        return entries;
    }
}