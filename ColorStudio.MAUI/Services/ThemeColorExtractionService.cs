namespace ColorStudio.MAUI.Services;

public partial class ThemeColorExtractionService
{
    public partial Task<IEnumerable<Color>> GetThemeColorsAsync(string filePath, int maxSize);

}
