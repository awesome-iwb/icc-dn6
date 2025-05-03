using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DubiousDubiUniverse.InkCanvasForClass.Converters;

public enum ToolbarReverseColorConverterRequiredColor {
    Foreground,
    Background
}

public class ToolbarReverseColorConverter : IMultiValueConverter {
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
        var isReverse = (bool)values[0];
        var foregroundColor = (Color)values[1];
        var backgroundColor = (Color)values[2];
        var requiredColor = (ToolbarReverseColorConverterRequiredColor)values[3];
        var isBrushNeeded = (bool)values[4];

        if (isReverse)
            return requiredColor switch {
                ToolbarReverseColorConverterRequiredColor.Foreground => isBrushNeeded
                    ? new SolidColorBrush(backgroundColor)
                    : backgroundColor,
                ToolbarReverseColorConverterRequiredColor.Background => isBrushNeeded
                    ? new SolidColorBrush(foregroundColor)
                    : foregroundColor,
                _ => throw new ArgumentOutOfRangeException(nameof(requiredColor), requiredColor, null)
            };
        return requiredColor switch {
            ToolbarReverseColorConverterRequiredColor.Foreground => isBrushNeeded
                ? new SolidColorBrush(foregroundColor)
                : foregroundColor,
            ToolbarReverseColorConverterRequiredColor.Background => isBrushNeeded
                ? new SolidColorBrush(Color.FromArgb(140, 159, 159, 159))
                : Color.FromArgb(140, 159, 159, 159),
            _ => throw new ArgumentOutOfRangeException(nameof(requiredColor), requiredColor, null)
        };
    }


    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }

    private static double GetGrayLevel(Color color) {
        return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
    }
}