using EasyReport.Domain;
using EasyReport.WebApi.Services;

namespace EasyReport.WebApi.Controllers;

public class AvatarController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    : ResourceControllerBase<Avatar>(unitOfWork, webHostEnvironment);