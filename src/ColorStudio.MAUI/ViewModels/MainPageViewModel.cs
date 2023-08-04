using System.Collections.ObjectModel;
using ColorStudio.MAUI.Services;
using CommunityToolkit.Mvvm.Input;

namespace ColorStudio.MAUI.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    readonly IMediaPicker _mediaPicker;
    readonly ThemeColorExtractionService _themeColorExtractionService;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ImageSource))]
    string _filePath = string.Empty;
    public ObservableCollection<ColorViewModel> ThemeColors { get; } = new();

    int _maxSize = 8;
    public ImageSource ImageSource => ImageSource.FromFile(FilePath);
    public MainPageViewModel(IMediaPicker mediaPicker, ThemeColorExtractionService themeColorExtractionService)
    {
        _mediaPicker = mediaPicker;
        _themeColorExtractionService = themeColorExtractionService;
    }

    [RelayCommand]
    async Task PickImage()
    {
        var result = await _mediaPicker.PickPhotoAsync(new()
        {
            Title = "选择图片",
        });
        if (result == null)
            return;
        FilePath = result.FullPath;
        ThemeColors.Clear();
        try
        {
            var themeColors = (await _themeColorExtractionService.GetThemeColorsAsync(FilePath, _maxSize)).ToList();
            foreach (var themeColor in themeColors)
            {
                ThemeColors.Add(new(themeColor));
            }
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("错误", $"解析图片时发生了错误：{Environment.NewLine}{e.Message}", "确定");
        }
    }
}
