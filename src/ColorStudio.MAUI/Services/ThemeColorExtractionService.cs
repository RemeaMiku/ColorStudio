namespace ColorStudio.MAUI.Services;

public partial class ThemeColorExtractionService
{
    private readonly ThemeColorExtractor _extractor = new();
    private const int _maxPixelCount = 1000000;
    public partial Task<IEnumerable<Color>> GetThemeColorsAsync(string filePath, int maxSize);

    private (HashSet<int> WidthIndexes, HashSet<int> HeightIndexes) GetIndexes(int width, int height)
    {
        var random = new Random();
        var widthIndexes = new HashSet<int>();
        var heightIndexes = new HashSet<int>();
        while (widthIndexes.Count * heightIndexes.Count <= _maxPixelCount)
        {
            widthIndexes.Add(random.Next(0, width - 1));
            heightIndexes.Add(random.Next(0, height - 1));
        }
        return (widthIndexes, heightIndexes);
    }
}
