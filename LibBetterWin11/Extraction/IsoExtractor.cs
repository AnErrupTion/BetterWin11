using DiscUtils;
using DiscUtils.Udf;

namespace BetterWin11_Builder.Extraction;

// Thank you https://social.msdn.microsoft.com/Forums/en-US/d50e93ac-5907-4cf0-ba3e-c8c325563fc9/extract-iso-file-programatically-using-c-code?forum=csharpgeneral!!!!
public static class IsoExtractor
{
    public static void ExtractTo(string? file, string dir)
    {
        using var stream = File.Open(file, FileMode.Open);
        using var reader = new UdfReader(stream);
        ExtractDirectory(reader.Root, dir, string.Empty);
    }

    private static void ExtractDirectory(DiscDirectoryInfo info, string rootPath, string pathInImage)
    {
        if (!string.IsNullOrWhiteSpace(pathInImage))
            pathInImage += Path.AltDirectorySeparatorChar + info.Name;

        rootPath += Path.AltDirectorySeparatorChar + info.Name;
        AppendDirectory(rootPath);

        foreach (var directory in info.GetDirectories())
            ExtractDirectory(directory, rootPath, pathInImage);
        
        foreach (var file in info.GetFiles())
        {
            using var stream = file.OpenRead();
            using var fs = File.Create(rootPath + Path.AltDirectorySeparatorChar + file.Name); // Here you can Set the BufferSize Also e.g. File.Create(RootPath + "\\" + finfo.Name, 4 * 1024)
            stream.CopyTo(fs, 4 * 1024); // Buffer Size is 4 * 1024 but you can modify it in your code as per your need
        }
    }

    private static void AppendDirectory(string? path)
    {
        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        catch (DirectoryNotFoundException)
        {
            AppendDirectory(Path.GetDirectoryName(path));
        }
        catch (PathTooLongException)
        {
            AppendDirectory(Path.GetDirectoryName(path));
        }
    }
}