namespace ColorStudio.Shared;

public class ThemeColorExtractor
{
    private readonly List<ColorBox> _colorBoxes = new(Palette.Capacity);
    public IEnumerable<Color> Extract(IEnumerable<Color> colors, int maxSize)
    {
        if (maxSize <= 0 || colors.Count() <= maxSize)
            return colors;
        var box = new ColorBox(colors.ToList());
        while (_colorBoxes.Count <= maxSize - 1)
        {
            _colorBoxes.AddRange(box.Split());
            box = _colorBoxes.MaxBy(c => c._weight);
            _colorBoxes.Remove(box);
        }
        var count = _colorBoxes.Count;
        for (; ; )
        {
            MergeColorBoxes();
            if (count == _colorBoxes.Count)
                break;
            count = _colorBoxes.Count;
        }
        return _colorBoxes
            .OrderByDescending(c => c._colors.Count)
            .Select(c => c._averageColor)
            .Take(maxSize);
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
                if (Color.DistanceBetween(color1, color2) == 0)
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