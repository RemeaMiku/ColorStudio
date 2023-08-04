﻿using System.Linq;
using Android.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace ColorStudio.MAUI.Services;

partial class ThemeColorExtractionService
{
    public partial async Task<IEnumerable<Color>> GetThemeColorsAsync(string filePath, int maxSize)
    => await Task.Run(() =>
        {
            using var stream = File.OpenRead(filePath);
            using var image = PlatformImage.FromStream(stream).AsBitmap();
            var pixels = image.Height * image.Width <= _maxPixelCount ? GetPixelsCommon(image) : GetPixelsOversized(image);
            return _extractor.Extract(pixels, maxSize);
        });

    private List<Color> GetPixelsOversized(Bitmap image)
    {
        var pixels = new List<Color>(_maxPixelCount);
        (var widthIndexes, var heightIndexes) = GetIndexes(image.Width, image.Height);
        foreach (var i in widthIndexes)
            foreach (var j in heightIndexes)
                pixels.Add(new(image.GetPixel(i, j)));
        return pixels;
    }

    private List<Color> GetPixelsCommon(Bitmap image)
    {
        var pixels = new List<Color>(image.Width * image.Height);
        for (var i = 0; i < image.Width; i++)
            for (int j = 0; j < image.Height; j++)
                pixels.Add(new(image.GetPixel(i, j)));
        return pixels;
    }
}
