using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Common;

public class ImageHelper
{
    private static readonly Random Rand = new Random();

    public static byte[] GenerateImage(
        int pixelsInWidth = 8,
        int pixelsInHeight = 8,
        int countColor = 3,
        int whiteFrequency = 2
    )
    {
        int halfPixelsInWidth = pixelsInWidth / 2;
        using var image = new Image<Rgb24>(pixelsInWidth, pixelsInHeight);
        var colors = _generateColors(countColor);

        for (int y = 0; y < pixelsInHeight; y++)
        for (int x = 0; x < halfPixelsInWidth; x++)
        {
            int horizontalIndex = pixelsInWidth - 1; // width-1 because SetPixel range [0, width-1]
            int randomColor = Rand.Next(countColor);
            var currentColor = Rand.Next(whiteFrequency) == 1 ? colors[randomColor] : Color.White;

            image[x, y] = currentColor;
            image[horizontalIndex - x, y] = currentColor;
        }

        using var ms = new MemoryStream();
        image.SaveAsBmp(ms);
        return ms.ToArray();
    }

    // generate array with random colors 
    private static Color[] _generateColors(int countColor)
    {
        var colors = new Color[countColor];
        byte[] bytes = new byte[3];

        for (int i = 0; i < countColor; i++)
        {
            Rand.NextBytes(bytes);
            colors[i] = new Color(new Rgb24(bytes[0], bytes[1], bytes[2]));
        }

        return colors;
    }
}