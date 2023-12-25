using EasyReport.Domain;
using EasyReport.WebApi.Services;

namespace EasyReport.WebApi.Controllers;

public class ResourceController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    : ResourceControllerBase<Resource>(unitOfWork, webHostEnvironment);