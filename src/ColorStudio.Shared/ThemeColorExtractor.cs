namespace ColorStudio.Shared;

public class ThemeColorExtractor
{
    private readonly List<ColorBox> _colorBoxes = new(Palette.Capacity);

    private const float _threshold = 50f;

    public IEnumerable<Color> Extract(IEnumerable<Color> colors, int maxSize)
    {
        if (maxSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxSize), "The maximum size should be greater than 0.");
        if (colors is null)
            throw new ArgumentNullException(nameof(colors));
        var colorList = colors.ToList();
        if (colorList.Count <= maxSize)
            foreach (var color in colorList.Distinct())
                yield return color;
        _colorBoxes.Clear();
        var box = new ColorBox(colorList);
        while (_colorBoxes.Count <= maxSize - 1)
        {
            _colorBoxes.AddRange(box.Split());
            box = _colorBoxes.MaxBy(c => c._weight);
            _colorBoxes.Remove(box);
        }
        int count;
        do
        {
            count = _colorBoxes.Count;
            MergeColorBoxes();
        } while (_colorBoxes.Count != count);
        foreach (var color in (from colorBox in _colorBoxes
                               orderby colorBox.Count descending
                               select colorBox._averageColor).Take(maxSize))
            yield return color;
    }

    private void MergeColorBoxes()
    {
        for (int i = 0; i < _colorBoxes.Count; i++)
        {
            var color1 = _colorBoxes[i]._averageColor;
            for (int j = 1; j < _colorBoxes.Count; j++)
            {
                if (i == j)
                    continue;
                var color2 = _colorBoxes[j]._averageColor;
                if (Color.DistanceBetween(color1, color2) <= _threshold)
                {
                    _colorBoxes.Add(new(_colorBoxes[i]._colors.Concat(_colorBoxes[j]._colors).ToList()));
                    _colorBoxes.Remove(_colorBoxes[i]);
                    _colorBoxes.Remove(_colorBoxes[j]);
                    return;
                }
            }
        }
    }
}