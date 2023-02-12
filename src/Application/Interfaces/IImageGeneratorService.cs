namespace Application.Interfaces;

public interface IImageGeneratorService
{
    byte[] GenerateImage(
        int pixelsInWidth,
        int pixelsInHeight,
        int countColor,
        int whiteFrequency
    );
}
