using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class AttachController : ControllerBase
{
    private readonly IAttachService _attachService;

    public AttachController(IAttachService attachService)
    {
        _attachService = attachService;
    }

    [HttpPost]
    public async Task<Guid> Create(IFormFile file)
    {
        var attach = new Attach()
        {
            Name = file.Name,
            Size = file.Length,
            MimeType = file.ContentType,
        };

        using var fileStream = file.OpenReadStream();
        return await _attachService.SaveAttach(attach, fileStream);
    }

    [HttpGet]
    public async Task<FileResult> Get(Guid attachId, bool download = false)
    {
        var attach = await _attachService.GetById(attachId);
        var fs = _attachService.GetStream(attach.Id);

        if (download)
            return File(fs, attach.MimeType, attach.Name);

        return File(fs, attach.MimeType);
    }
}