using System.Drawing;
using ColorStudio.Shared;
using static System.Console;

#pragma warning disable CA1416

var start = DateTimeOffset.Now;
var image = new Bitmap("D:\\onedrive\\Desktop\\resources\\2023春日祭-术力口问答互动背景.png");
var pixels = Enumerable.Range(0, image.Width).SelectMany(x => Enumerable.Range(0, image.Height).Select(y => new ColorStudio.Shared.Color(image.GetPixel(x, y))));
var extractor = new ThemeColorExtractor();
var themeColors = extractor.Extract(pixels, 16);
foreach (var themeColor in themeColors)
    WriteLine(themeColor);
WriteLine((DateTimeOffset.Now - start).TotalMilliseconds);