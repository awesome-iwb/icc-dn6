using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DubiousDubiUniverse.InkCanvasForClass.Converters;

public class ColorToSolidColorBrushValueConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture) {
        if (null == value) return null;

        if (value is Color) {
            var color = (Color)value;
            return new SolidColorBrush(color);
        }

        var type = value.GetType();
        throw new InvalidOperationException("Unsupported type [" + type.Name + "]");
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture) {
        throw new NotImplementedException();
    }
}