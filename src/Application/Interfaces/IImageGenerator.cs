namespace Application.Interfaces;

public interface IImageGenerator
{
    byte[] GenerateImage(
        int pixelsInWidth,
        int pixelsInHeight,
        int countColor,
        int whiteFrequency
    );
}