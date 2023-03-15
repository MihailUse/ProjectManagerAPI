using System.Reflection;
using Application.Configs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class AttachService : IAttachService
{
    private readonly string _attachPath;
    private readonly IDatabaseContext _database;
    private readonly IImageGenerator _imageGenerator;
    private readonly ImageGeneratorConfig _imageConfig;

    public AttachService(
        IDatabaseContext database,
        IImageGenerator imageGenerator,
        IOptions<ImageGeneratorConfig> imageConfig
    )
    {
        _database = database;
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

        await _database.Attaches.AddAsync(attach);
        await _database.SaveChangesAsync();
        return attach.Id;
    }

    public async Task<Guid> SaveAttach(Attach attach, Stream fileStream)
    {
        await _database.Attaches.AddAsync(attach);

        var filePath = Path.Combine(_attachPath, attach.Id.ToString());
        using (var stream = File.Create(filePath))
            await fileStream.CopyToAsync(stream);

        await _database.SaveChangesAsync();
        return attach.Id;
    }

    public async Task<Attach> GetById(Guid attachId)
    {
        var attach = await _database.Attaches.FindAsync(attachId);
        if (attach == default)
            throw new NotFoundException("Attach not found");

        return attach;
    }

    public FileStream GetStream(Guid attachId)
    {
        var filePath = Path.Combine(_attachPath, attachId.ToString());

        if (!File.Exists(filePath))
            throw new NotFoundException("File file not found");

        return new FileStream(filePath, FileMode.Open);
    }
}