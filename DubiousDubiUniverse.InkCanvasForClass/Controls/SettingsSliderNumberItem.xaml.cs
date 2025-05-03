using System.Windows;

namespace DubiousDubiUniverse.InkCanvasForClass.Controls;

/// <summary>
///     SettingsSliderNumberItem.xaml 的交互逻辑
/// </summary>
public partial class SettingsSliderNumberItem {
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(""));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(""));

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(100.0));

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(0.0));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(0.0));

    public static readonly DependencyProperty RecommendedValueProperty =
        DependencyProperty.Register(nameof(RecommendedValue), typeof(double), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(0.0));

    public static readonly DependencyProperty TickFrequencyProperty =
        DependencyProperty.Register(nameof(TickFrequency), typeof(double), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(1.0));

    public static readonly DependencyProperty IsSnapToTickProperty =
        DependencyProperty.Register(nameof(IsSnapToTick), typeof(bool), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(true));

    public static readonly DependencyProperty IsDisabledProperty =
        DependencyProperty.Register(nameof(IsDisabled), typeof(bool), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(false));

    public static readonly DependencyProperty IsExperimentalProperty =
        DependencyProperty.Register(nameof(IsExperimental), typeof(bool), typeof(SettingsSliderNumberItem),
            new PropertyMetadata(false));

    public SettingsSliderNumberItem() {
        InitializeComponent();
    }

    public string Title {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public double Maximum {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public double Minimum {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Value {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double RecommendedValue {
        get => (double)GetValue(RecommendedValueProperty);
        set => SetValue(RecommendedValueProperty, value);
    }

    public double TickFrequency {
        get => (double)GetValue(TickFrequencyProperty);
        set => SetValue(TickFrequencyProperty, value);
    }

    public bool IsSnapToTick {
        get => (bool)GetValue(IsSnapToTickProperty);
        set => SetValue(IsSnapToTickProperty, value);
    }

    public bool IsDisabled {
        get => (bool)GetValue(IsDisabledProperty);
        set => SetValue(IsDisabledProperty, value);
    }

    public bool IsExperimental {
        get => (bool)GetValue(IsExperimentalProperty);
        set => SetValue(IsExperimentalProperty, value);
    }
}