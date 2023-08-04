namespace ColorStudio.Shared;

internal readonly struct ColorBox
{
    internal ColorBox(List<Color> colors)
    {
        _colors = colors;
        var minR = byte.MaxValue;
        var maxR = byte.MinValue;
        var minG = byte.MaxValue;
        var maxG = byte.MinValue;
        var minB = byte.MaxValue;
        var maxB = byte.MinValue;
        Parallel.ForEach(colors, color =>
        {
            if (color.R < minR)
                minR = color.R;
            if (color.R > maxR)
                maxR = color.R;
            if (color.G < minG)
                minG = color.G;
            if (color.G > maxG)
                maxG = color.G;
            if (color.B < minB)
                minB = color.B;
            if (color.B > maxB)
                maxB = color.B;
        });
        var rangeR = maxR - minR;
        var rangeG = maxG - minG;
        var rangeB = maxB - minB;
        var maxRange = Max(Max(rangeR, rangeG), rangeB);
        if (maxRange == rangeR)
            _maxRangeChannel = ColorChannel.Red;
        if (maxRange == rangeG)
            _maxRangeChannel = ColorChannel.Green;
        if (maxRange == rangeB)
            _maxRangeChannel = ColorChannel.Blue;
        _weight = (ulong)rangeR * (ulong)rangeG * (ulong)rangeB * (ulong)_colors.Count;
        _averageColor = Color.Average(_colors);
    }
    internal IEnumerable<ColorBox> Split()
    {
        var orderedColors = _maxRangeChannel switch
        {
            ColorChannel.Red => _colors.OrderBy(c => c.R),
            ColorChannel.Green => _colors.OrderBy(c => c.G),
            ColorChannel.Blue => _colors.OrderBy(c => c.B),
            _ => throw new NotImplementedException(),
        };
        var mid = orderedColors.Count() / 2;
        yield return new(orderedColors.Take(mid).ToList());
        yield return new(orderedColors.Skip(mid).ToList());
    }
    internal int Count => _colors.Count;
    internal readonly ulong _weight;
    internal readonly Color _averageColor;
    internal readonly ColorChannel _maxRangeChannel;
    internal readonly List<Color> _colors;
}
