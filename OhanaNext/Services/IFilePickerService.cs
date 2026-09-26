using Microsoft.Win32;
using Ohana.Core.FileFormats;

namespace OhanaNext.Services;

public interface IFilePickerService
{
    string? PickFile(IReadOnlyList<FileFormatDescriptor> formats);
}

public class FIlePickerService : IFilePickerService
{
    public string? PickFile(IReadOnlyList<FileFormatDescriptor> formats)
    {
        var dialog = new OpenFileDialog
        {
            Filter = BuildFilter(formats)
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    private static string BuildFilter(IReadOnlyList<FileFormatDescriptor> formats)
    {
        var allExtensions = formats.SelectMany(x => x.Extensions).Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(ToPattern);

        var filters = new List<string>();
        var allPattern = string.Join(';', allExtensions);

        if (!string.IsNullOrEmpty(allPattern))
        {
            filters.Add($"All Supported Files ({allPattern})|{allPattern}");
        }

        foreach (var format in formats)
        {
            var pattern = string.Join(';', format.Extensions.Select(ToPattern));

            filters.Add($"{format.Name} ({pattern})|{pattern}");
        }

        filters.Add("All Files (*.*)|*.*");

        return string.Join('|', filters);
    }

    private static string ToPattern(string extension)
    {
        return $"*{extension}";
    }
}