using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DubiousDubiUniverse.InkCanvasForClass.Controls.Toolbar;

public class ToolbarButton : Button {
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ToolbarButton), new PropertyMetadata(""));

    public static readonly DependencyProperty IconNameProperty =
        DependencyProperty.Register(nameof(IconName), typeof(string), typeof(ToolbarButton),
            new PropertyMetadata(""));

    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(ToolbarButton),
            new PropertyMetadata(24.0));

    public static readonly DependencyProperty IsIconFilledProperty =
        DependencyProperty.Register(nameof(IsIconFilled), typeof(bool), typeof(ToolbarButton),
            new PropertyMetadata(false));

    public static readonly DependencyProperty IsPressedReverseColorProperty =
        DependencyProperty.Register(nameof(IsPressedReverseColor), typeof(bool), typeof(ToolbarButton),
            new PropertyMetadata(false));

    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register(nameof(IconColor), typeof(Color), typeof(ToolbarButton),
            new PropertyMetadata(Colors.Black));

    public static readonly DependencyProperty PointerOverBackgroundProperty =
        DependencyProperty.Register(nameof(PointerOverBackground), typeof(Color), typeof(ToolbarButton),
            new PropertyMetadata(Color.FromArgb(20, 0, 0, 0)));

    public static readonly DependencyProperty PressedBackgroundProperty =
        DependencyProperty.Register(nameof(PressedBackground), typeof(Color), typeof(ToolbarButton),
            new PropertyMetadata(Color.FromArgb(140, 159, 159, 159)));

    public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register(nameof(FontColor),
        typeof(Color), typeof(ToolbarButton),
        new PropertyMetadata(Colors.Black));

    public static readonly DependencyProperty FontWeightProperty =
        DependencyProperty.Register(nameof(FontWeight), typeof(FontWeight), typeof(ToolbarButton),
            new PropertyMetadata(FontWeights.Normal));

    public ToolbarButton() {
        Style = new ResourceDictionary {
            Source = new Uri(
                "/DubiousDubiUniverse.InkCanvasForClass;component/Resources/Dictionaries/MainToolbarButtonStylesDictionary.xaml",
                UriKind.Relative)
        }["default_tb_button_style"] as Style;

        Loaded += (_, __) => {
            var dpd = DependencyPropertyDescriptor.FromProperty(IsPressedProperty, typeof(Button));
            dpd?.AddValueChanged(this, OnIsPressedChanged);
        };
    }

    public string Title {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string IconName {
        get => (string)GetValue(IconNameProperty);
        set => SetValue(IconNameProperty, value);
    }

    public double IconSize {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public bool IsIconFilled {
        get => (bool)GetValue(IsIconFilledProperty);
        set => SetValue(IsIconFilledProperty, value);
    }

    public Color IconColor {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public Color PointerOverBackground {
        get => (Color)GetValue(PointerOverBackgroundProperty);
        set => SetValue(PointerOverBackgroundProperty, value);
    }

    public Color PressedBackground {
        get => (Color)GetValue(PressedBackgroundProperty);
        set => SetValue(PressedBackgroundProperty, value);
    }

    public bool IsPressedReverseColor {
        get => (bool)GetValue(IsPressedReverseColorProperty);
        set => SetValue(IsPressedReverseColorProperty, value);
    }

    public Color FontColor {
        get => (Color)GetValue(FontColorProperty);
        set => SetValue(FontColorProperty, value);
    }

    public new FontWeight FontWeight {
        get => (FontWeight)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    public event EventHandler<bool>? IsPressedChanged;

    private void OnIsPressedChanged(object? sender, EventArgs e) {
        IsPressedChanged?.Invoke(this, IsPressed);
    }
}