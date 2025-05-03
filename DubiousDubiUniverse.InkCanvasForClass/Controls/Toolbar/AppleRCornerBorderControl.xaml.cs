using System.Windows;
using System.Windows.Media;

namespace DubiousDubiUniverse.InkCanvasForClass.Controls.Toolbar;

public enum BorderThicknessLevel {
    None,
    Thin,
    Medium,
    Thick
}

public partial class AppleRCornerBorderControl {
    public static readonly DependencyProperty BorderThicknessLevelProperty =
        DependencyProperty.Register(nameof(BorderThicknessLevel), typeof(BorderThicknessLevel),
            typeof(AppleRCornerBorderControl),
            new PropertyMetadata(BorderThicknessLevel.Medium, OnVisualPropertyChanged));

    public static readonly DependencyProperty BorderColorProperty =
        DependencyProperty.Register(nameof(BorderColor), typeof(Color),
            typeof(AppleRCornerBorderControl),
            new PropertyMetadata(Colors.Black, OnVisualPropertyChanged));

    public AppleRCornerBorderControl() {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    public BorderThicknessLevel BorderThicknessLevel {
        get => (BorderThicknessLevel)GetValue(BorderThicknessLevelProperty);
        set => SetValue(BorderThicknessLevelProperty, value);
    }

    public Color BorderColor {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    private static void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is AppleRCornerBorderControl control && control.IsLoaded) control.ApplyImagesAndColors();
    }

    private void OnLoaded(object sender, RoutedEventArgs e) {
        ApplyImagesAndColors();
    }

    private string BorderThicknessLevelToString(BorderThicknessLevel level) {
        return level switch {
            BorderThicknessLevel.None => "0",
            BorderThicknessLevel.Thin => "1",
            BorderThicknessLevel.Medium => "1.5",
            BorderThicknessLevel.Thick => "2",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    private double BorderThicknessLevelToDouble(BorderThicknessLevel level) {
        return level switch {
            BorderThicknessLevel.None => 0,
            BorderThicknessLevel.Thin => 1,
            BorderThicknessLevel.Medium => 1.5,
            BorderThicknessLevel.Thick => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    private void ApplyImagesAndColors() {
        var prefix = "toolbar_head_body_border_";
        var suffixL = "_l";
        var suffixR = "_r";

        var thicknessStr = BorderThicknessLevelToString(BorderThicknessLevel);
        var keyL = $"{prefix}{thicknessStr}{suffixL}";
        var keyR = $"{prefix}{thicknessStr}{suffixR}";

        var dict = Resources.MergedDictionaries.FirstOrDefault();
        if (dict != null)
            if (dict[keyL] is DrawingImage leftImg &&
                dict[keyR] is DrawingImage rightImg) {
                imgLeft.Source = CloneAndColorDrawingImage(leftImg, BorderColor);
                imgRight.Source = CloneAndColorDrawingImage(rightImg, BorderColor);
            }

        borderLine.BorderBrush = new SolidColorBrush(BorderColor);
        borderLine.BorderThickness = new Thickness(0, BorderThicknessLevelToDouble(BorderThicknessLevel), 0,
            BorderThicknessLevelToDouble(BorderThicknessLevel));
    }

    private DrawingImage CloneAndColorDrawingImage(DrawingImage original, Color newColor) {
        var cloned = original.Clone();
        if (cloned.Drawing is DrawingGroup group) {
            var newGroup = group.Clone();
            ReplaceColorInDrawing(newGroup, newColor);
            return new DrawingImage(newGroup);
        }

        return cloned;
    }

    private void ReplaceColorInDrawing(Drawing drawing, Color color) {
        if (drawing is DrawingGroup group) {
            foreach (var child in group.Children) ReplaceColorInDrawing(child, color);
        } else if (drawing is GeometryDrawing geo) {
            if (geo.Pen != null) {
                geo.Pen = geo.Pen.Clone();
                geo.Pen.Brush = new SolidColorBrush(color);
            }

            if (geo.Brush != null && geo.Brush is SolidColorBrush) geo.Brush = new SolidColorBrush(color);
        }
    }
}