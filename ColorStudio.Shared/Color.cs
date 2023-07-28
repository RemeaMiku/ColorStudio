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
    public string Html
    {
        get => ColorTranslator.ToHtml(_value);
        init
        {
            _value = ColorTranslator.FromHtml(value);
        }
    }
    public string Argb => string.Join(',', A, R, G, B);
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
}