namespace Application.Interfaces;

public interface IImageGenerator
{
    Stream GenerateImage(
        int pixelsInWidth,
        int pixelsInHeight,
        int countColor,
        int whiteFrequency
    );
}