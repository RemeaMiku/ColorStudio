namespace ColorStudio.Shared;

public readonly struct Color : IEquatable<Color>
{
    public string Name
    {
        get
        {
            if (_name is null || string.IsNullOrEmpty(_name))
                return ColorTranslator.ToHtml(_value);
            return _name;
        }
        init => _name = value;
    }
    public byte A
    {
        get => _value.A;
        init
        {
            if (_value.A != value)
                _value = System.Drawing.Color.FromArgb(value, _value);
        }
    }
    public byte R
    {
        get => _value.R;
        init
        {
            if (_value.R != value)
                _value = System.Drawing.Color.FromArgb(_value.A, _value.R, _value.G, _value.B);
        }
    }
    public byte G
    {
        get => _value.G;
        init
        {
            if (_value.G != value)
                _value = System.Drawing.Color.FromArgb(_value.A, _value.R, _value.G, _value.B);
        }
    }
    public byte B
    {
        get => _value.B;
        init
        {
            if (_value.B != value)
                _value = System.Drawing.Color.FromArgb(_value.A, _value.R, _value.G, _value.B);
        }
    }
    public string ArgbAsHex
    {
        get => ColorTranslator.ToHtml(_value);
        init
        {
            _value = ColorTranslator.FromHtml(value);
        }
    }
    //public string Argb => string.Join(',', A, R, G, B);
    public readonly static Color Empty = new();
    private readonly string? _name;
    public Color()
    {
        _name = null;
        _value = System.Drawing.Color.Empty;
    }
    public Color(string argb, string? name = null)
    {
        _name = name;
        _value = ColorTranslator.FromHtml(argb);
    }

    public Color(uint argb, string? name = null)
    {
        _name = name;
        _value = System.Drawing.Color.FromArgb(unchecked((int)argb));
    }

    public Color(int r, int g, int b, int a = 255, string? name = null)
    {
        _name = name;
        _value = System.Drawing.Color.FromArgb(a, r, g, b);
    }
    public Color(System.Drawing.Color value, string? name = null)
    {
        _name = name;
        _value = value;
    }
    public static implicit operator Color(System.Drawing.Color value)
    => new(value, null);
    private readonly System.Drawing.Color _value;

    public float Brightness
    {
        get => _value.GetBrightness();
    }

    public float Hue
    {
        get => _value.GetHue();
    }

    public float Saturation
    {
        get => _value.GetSaturation();
    }

    public bool Equals(Color other)
    {
        return (A == other.A) && (R == other.R) && (other.G == other.G) && (B == other.B);
    }
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Color other && Equals(other);

    public static bool operator ==(Color left, Color right) => left.Equals(right);

    public static bool operator !=(Color left, Color right) => !(left == right);

    public override string ToString() => Name;
    public bool IsEmpty => _value.IsEmpty;
    public override int GetHashCode() => _value.GetHashCode();
    public float DistanceFrom(Color other, ColorDistanceMode mode = ColorDistanceMode.RedMean)
        => DistanceBetween(this, other, mode);
    public static float DistanceBetween(Color left, Color right, ColorDistanceMode mode = ColorDistanceMode.RedMean) => mode switch
    {
        ColorDistanceMode.RedMean => DistanceBetweenCommon(left, right),
        ColorDistanceMode.CIEDE2000 => throw new NotImplementedException(),
        _ => DistanceBetweenCommon(left, right),
    };
    /// <summary>
    /// https://zh.wikipedia.org/wiki/%E9%A2%9C%E8%89%B2%E5%B7%AE%E5%BC%82
    /// </summary>
    /// <param name="color1"></param>
    /// <param name="color2"></param>
    /// <returns></returns>
    private static float DistanceBetweenCommon(Color color1, Color color2)
    {
        var meanR = (color1.R + color2.R) / 2f;
        var dR = color1.R - color2.R;
        var dG = color1.G - color2.G;
        var dB = color1.B - color2.B;
        var dC = Sqrt((2 + meanR / 256) * dR * dR + 4 * dG * dG + (2 + (255 - meanR) / 256) * dB * dB);
        return (float)dC;
    }

    public static Color Average(Color color1, Color color2)
        => new((int)Round((color1.R + color2.R) / 2f), (int)Round((color1.G + color2.G) / 2f), (int)Round((color1.B + color2.B) / 2f));

    public static Color Average(IEnumerable<Color> colors)
    {
        var r = (int)Round(colors.Average(c => c.R));
        var g = (int)Round(colors.Average(c => c.G));
        var b = (int)Round(colors.Average(c => c.B));
        return new(r, g, b);
    }
}