namespace ColorStudio.MAUI.ViewModels;

public partial class ColorViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AsBrush))]
    [NotifyPropertyChangedFor(nameof(AsMauiColor))]
    Color _data;
    public SolidColorBrush AsBrush
        => new(AsMauiColor);
    public Microsoft.Maui.Graphics.Color AsMauiColor
        => new(Data.R, Data.G, Data.B, Data.A);
    public ColorViewModel(Color data)
    {
        Data = data;
    }
}
