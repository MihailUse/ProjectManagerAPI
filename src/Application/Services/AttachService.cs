using System.Reflection;
using Application.Configs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class AttachService : IAttachService
{
    private readonly string _attachPath;
    private readonly IAttachRepository _repository;
    private readonly IImageGenerator _imageGenerator;
    private readonly ImageGeneratorConfig _imageConfig;

    public AttachService(
        IAttachRepository repository,
        IImageGenerator imageGenerator,
        IOptions<ImageGeneratorConfig> imageConfig
    )
    {
        _repository = repository;
        _imageGenerator = imageGenerator;
        _imageConfig = imageConfig.Value;

        _attachPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Attaches");
        Directory.CreateDirectory(_attachPath);
    }

    public async Task<Guid> GenerateImage()
    {
        using var imageStream = _imageGenerator.GenerateImage(
            _imageConfig.PixelsInWidth,
            _imageConfig.PixelsInHeight,
            _imageConfig.CountColor,
            _imageConfig.WhiteFrequency
        );

        var attachId = Guid.NewGuid();
        var attach = new Attach()
        {
            Id = attachId,
            Name = attachId.ToString(),
            MimeType = "image/jpeg",
            Size = imageStream.Length
        };

        var filePath = Path.Combine(_attachPath, attach.Id.ToString());
        using (var stream = File.Create(filePath))
        {
            imageStream.Seek(0, SeekOrigin.Begin);
            await imageStream.CopyToAsync(stream);
        }

        await _repository.Add(attach);
        return attach.Id;
    }

    public async Task<Guid> SaveAttach(Attach attach, Stream fileStream)
    {
        attach.Id = Guid.NewGuid();
        var filePath = Path.Combine(_attachPath, attach.Id.ToString());
        using (var stream = File.Create(filePath))
            await fileStream.CopyToAsync(stream);

        await _repository.Add(attach);
        return attach.Id;
    }

    public async Task<Attach> GetById(Guid id)
    {
        var attach = await _repository.FindById(id);
        if (attach == default)
            throw new NotFoundException("Attach not found");

        return attach;
    }

    public FileStream GetStream(Guid id)
    {
        var filePath = Path.Combine(_attachPath, id.ToString());

        if (!File.Exists(filePath))
            throw new NotFoundException("File file not found");

        return new FileStream(filePath, FileMode.Open);
    }

    public async Task CheckAttachExists(Guid id)
    {
        var attachExists = await _repository.CheckExists(id);
        if (!attachExists)
            throw new NotFoundException("Attach not found");
    }
}