using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DubiousDubiUniverse.InkCanvasForClass.Converters;

public class IconKeyToToolbarIconConverter : IMultiValueConverter {
    private readonly ResourceDictionary _iconDictionary = new() {
        Source = new Uri(
            "/DubiousDubiUniverse.InkCanvasForClass;component/Resources/Dictionaries/ToolbarIconsDictionary.xaml",
            UriKind.Relative)
    };

    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
        var key = values[0] as string;
        var color = (Color)values[1];
        var filled = values[2] as string == "t";
        var isReverse = (bool)values[3];
        var backgroundColor = (Color)values[4];
        var isPressed = (bool)values[5];
        var di = !string.IsNullOrEmpty(key) &&
                 _iconDictionary.Contains("tb_icon_" + key + (filled ? "_filled" : ""))
            ? _iconDictionary["tb_icon_" + key + (filled ? "_filled" : "")] as DrawingImage
            : null;
        if (di is { Drawing: DrawingGroup group }) {
            var clone = group.Clone();
            ReplacePenColors(clone,
                isReverse && isPressed ? new SolidColorBrush(backgroundColor) : new SolidColorBrush(color));
            di = new DrawingImage(clone);
        }

        return di;
    }


    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }

    private static void ReplacePenColors(Drawing drawing, Brush newBrush) {
        if (drawing is DrawingGroup group) {
            foreach (var child in group.Children) ReplacePenColors(child, newBrush);
        } else if (drawing is GeometryDrawing geometryDrawing) {
            // 替换 Pen.Brush
            if (geometryDrawing.Pen != null) {
                var clonedPen = geometryDrawing.Pen.Clone();
                clonedPen.Brush = newBrush;
                geometryDrawing.Pen = clonedPen;
            }

            // 替换填充 Brush（仅限 SolidColorBrush，其他类型如渐变或图像填充可按需拓展）
            if (geometryDrawing.Brush is SolidColorBrush) geometryDrawing.Brush = newBrush;
        }
    }
}