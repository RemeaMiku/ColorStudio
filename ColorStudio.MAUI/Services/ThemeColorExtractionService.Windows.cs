using System.Drawing;

namespace ColorStudio.MAUI.Services;

partial class ThemeColorExtractionService
{
    public partial async Task<IEnumerable<Color>> GetThemeColorsAsync(string filePath, int maxSize)
        => await Task.Run(() =>
        {
            using var image = new Bitmap(filePath);
            var pixels = Enumerable.Range(0, image.Width)
            .SelectMany(x => Enumerable.Range(0, image.Height)
            .Select(y => new Color(image.GetPixel(x, y))));
            var extractor = new ThemeColorExtractor();
            return extractor.Extract(pixels, maxSize);
        });
}