namespace ItTrAp.InventoryService.Interfaces.Services;

public interface IFileService
{
    Task<(string cypheredName, string uri)> UploadTmpFileAsync(string fileName, Stream file, CancellationToken ct = default);
    Task<(string cypheredName, string uri)> MoveFileFromTmpToUploadsAsync(string uri, CancellationToken ct = default);
    Task DeleteAsync(string uri, CancellationToken ct = default);
    Task DeleteTmpAsync(string fileName, CancellationToken ct = default);
}