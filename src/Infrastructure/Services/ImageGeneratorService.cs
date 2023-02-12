﻿using Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Domain.Services;

public class ImageGeneratorService : IImageGeneratorService
{
    private readonly Random _rand = new Random();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pixelsInWidth"></param>
    /// <param name="pixelsInHeight"></param>
    /// <param name="countColor"></param>
    /// <param name="whiteFrequency"></param>
    /// <returns></returns>
    public byte[] GenerateImage(
        int pixelsInWidth,
        int pixelsInHeight,
        int countColor,
        int whiteFrequency
    )
    {
        int halfPixelsInWidth = pixelsInWidth / 2;
        using var image = new Image<Rgb24>(pixelsInWidth, pixelsInHeight);
        var colors = GenerateColors(countColor);

        for (int y = 0; y < pixelsInHeight; y++)
            for (int x = 0; x < halfPixelsInWidth; x++)
            {
                int horizontalIndex = pixelsInWidth - 1; // width-1 because SetPixel range [0, width-1]
                int randomColor = _rand.Next(countColor);
                var currentColor = _rand.Next(whiteFrequency) == 1 ? colors[randomColor] : Color.White;

                image[x, y] = currentColor;
                image[horizontalIndex - x, y] = currentColor;
            }

        using var ms = new MemoryStream();
        image.SaveAsBmp(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Generate array with random colors 
    /// </summary>
    /// <param name="countColor"></param>
    /// <returns>array with random colors</returns>
    private Color[] GenerateColors(int countColor)
    {
        var colors = new Color[countColor];
        byte[] bytes = new byte[3];

        for (int i = 0; i < countColor; i++)
        {
            _rand.NextBytes(bytes);
            colors[i] = new Color(new Rgb24(bytes[0], bytes[1], bytes[2]));
        }

        return colors;
    }
}
