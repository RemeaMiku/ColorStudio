using System.Linq;
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
            var pixels = Enumerable.Range(0, image.Width)
               .SelectMany(x => Enumerable.Range(0, image.Height)
               .Select(y => new Color((uint)image.GetPixel(x, y))));
            var extractor = new ThemeColorExtractor();
            return extractor.Extract(pixels, maxSize);
        });
}
