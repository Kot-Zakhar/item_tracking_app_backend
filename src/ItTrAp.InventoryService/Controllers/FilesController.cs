using ItTrAp.InventoryService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ItTrAp.InventoryService.Controllers;

[Route("api/v1/[controller]")]
public class FilesController(IFileService fileService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        string fileName, uri;

        using (var stream = file.OpenReadStream())
        {
            (fileName, uri) = await fileService.UploadTmpFileAsync(file.FileName, stream);
            stream.Close();
        }

        return Ok(new { FileName = fileName, Uri = uri });
    }

    [HttpDelete("tmp/{fileName}")]
    public async Task<IActionResult> DeleteFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("File name is required.");
        }

        await fileService.DeleteTmpAsync(fileName);
        return NoContent();
    }
}