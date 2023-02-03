namespace Application.Common.Interfaces;

public interface IImageGeneratorService
{
    static byte[] GenerateImage(
        int pixelsInWidth,
        int pixelsInHeight,
        int countColor,
        int whiteFrequency
    ) => throw new NotImplementedException();
}
