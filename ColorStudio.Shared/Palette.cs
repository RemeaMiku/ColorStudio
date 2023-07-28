namespace ColorStudio.Shared;

public class Palette
{
    private const int _maxCount = 16;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsEmpty => Colors.Count == 0;
    public DateTimeOffset CreationTime { get; } = DateTimeOffset.Now;
    public List<Color> Colors { get; } = new(_maxCount);
    public readonly static Palette Empty = new();
    public Palette(string? name = null, string? description = null)
    {
        Name = name;
        Description = description;
    }
    public Palette AddColor(Color color)
    {
        if (Colors.Count == _maxCount)
            throw new InvalidOperationException($"The maximum capacity ({_maxCount}) has been reached");
        Colors.Add(color);
        return this;
    }

    public Palette AddColors(ReadOnlySpan<Color> colors)
    {
        for (int i = 0; i < Colors.Count; i++)
            AddColor(colors[i]);
        return this;
    }
    public Palette RemoveColor(string colorName)
    {
        if (IsEmpty)
            return this;
        var index = Colors.FindIndex(color => color.Name == colorName);
        if (index == -1)
            throw new ArgumentException("The color of this name does not exist", nameof(colorName));
        Colors.RemoveAt(index);
        return this;
    }
    public Palette Clear()
    {
        if (IsEmpty)
            return this;
        Colors.Clear();
        return this;
    }
    public Color? FindColor(string colorName)
    {
        if (IsEmpty)
            return null;
        var index = Colors.FindIndex(color => color.Name == colorName);
        if (index == -1)
            return null;
        return Colors[index];
    }
    public Color? FindSameColor(Color targetColor)
    {
        if (IsEmpty)
            return null;
        foreach (var color in Colors)
            if (color == targetColor)
                return color;
        return null;
    }
}
