namespace Infrastructure.Configs;

public class ImageGeneratorConfig
{
    public const string Position = "ImageGeneratorConfig";

    public int PixelsInWidth { get; set; } = 8;
    public int PixelsInHeight { get; set; } = 8;
    public int CountColor { get; set; } = 3;
    public int WhiteFrequency { get; set; } = 2;
}