using EasyReport.Domain;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyReport.WebApi.Controllers;

public class ResourceControllerBase<TEntity> : ApiControllerBase where TEntity : Resource, new()
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ResourceControllerBase(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("upload")]
    public virtual async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var extension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var resourceUri = $"/uploads/{fileName}";

        var resource = new TEntity()
        {
            Name = file.FileName,
            Extension = extension,
            Uri = resourceUri,
            Size = file.Length,
            MimeType = file.ContentType,
            Path = filePath
        };

        await _unitOfWork.AddAsync(resource);
        await _unitOfWork.CommitAsync();

        return Ok(resource);
    }
}