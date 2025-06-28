using Abstractions;
using Application.Files.Interfaces;

namespace Infrastructure.Services;

public class FileService : IFileService
{
    public static readonly string RootUrl = "api";
    public static readonly string RootFolder = "files";
    public static readonly string UploadsFolder = "uploads";
    public static readonly string Tempfolder = "tmp";

    public static string RootFolderUrl => Path.Combine("/", RootUrl, RootFolder);
    public static string TempFolderUrl => Path.Combine("/", RootUrl, RootFolder, Tempfolder);
    public static string UploadsFolderUrl => Path.Combine("/", RootUrl, RootFolder, UploadsFolder);

    public static string CurrentDirectory => System.AppDomain.CurrentDomain.BaseDirectory;
    public static string RootFolderPhysicalPath => Path.Combine(CurrentDirectory, RootFolder);
    public static string TempFolderPhysicalPath => Path.Combine(CurrentDirectory, RootFolder, Tempfolder);
    public static string UploadsFolderPhysicalPath => Path.Combine(CurrentDirectory, RootFolder, UploadsFolder);

    public async Task<(string cypheredName, string uri)> UploadTmpFileAsync(string fileName, Stream file, CancellationToken ct = default)
    {
        if (!Directory.Exists(TempFolderPhysicalPath))
        {
            Directory.CreateDirectory(TempFolderPhysicalPath);
        }

        var extension = Path.GetExtension(fileName) ?? string.Empty;

        var generatedFileName = Guid.NewGuid().ToString("N") + extension;
        var filePath = Path.Combine(TempFolderPhysicalPath, generatedFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await file.CopyToAsync(fileStream, ct);
        }

        var uri = Path.Combine(TempFolderUrl, generatedFileName);

        return (generatedFileName, uri);
    }

    public Task<(string cypheredName, string uri)> MoveFileFromTmpToUploadsAsync(string uri, CancellationToken ct = default)
    {
        if (!uri.StartsWith(TempFolderUrl))
        {
            throw new ArgumentException("Image must be temporary.");
        }

        var src = uri.Replace(TempFolderUrl, TempFolderPhysicalPath);
        var dst = uri.Replace(TempFolderUrl, UploadsFolderPhysicalPath);

        if (!Directory.Exists(Path.Combine(UploadsFolderPhysicalPath)))
        {
            Directory.CreateDirectory(Path.Combine(UploadsFolderPhysicalPath));
        }

        if (File.Exists(src))
        {
            File.Move(src, dst);
        }

        var fileName = uri.Substring(TempFolderUrl.Length).TrimStart('/');
        var newUri = uri.Replace(TempFolderUrl, UploadsFolderUrl);

        return Task.FromResult((fileName, newUri));
    }

    public Task DeleteAsync(string uri, CancellationToken ct = default)
    {
        if (uri.StartsWith(TempFolderUrl))
        {
            var filePath = uri.Replace(TempFolderUrl, TempFolderPhysicalPath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        else if (uri.StartsWith(UploadsFolderUrl))
        {
            var filePath = uri.Replace(UploadsFolderUrl, UploadsFolderPhysicalPath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        else
        {
            throw new ArgumentException("Invalid file path.");
        }

        return Task.CompletedTask;
    }

    public Task DeleteTmpAsync(string fileName, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        var filePath = Path.Combine(TempFolderPhysicalPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}