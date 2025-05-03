using System.Windows;

namespace DubiousDubiUniverse.InkCanvasForClass.Controls;

public partial class SettingsToggleSwitchItem {
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(SettingsToggleSwitchItem),
            new PropertyMetadata("默认标题"));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(SettingsToggleSwitchItem),
            new PropertyMetadata("默认描述"));

    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register("IsChecked", typeof(bool), typeof(SettingsToggleSwitchItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty IsExperimentalProperty =
        DependencyProperty.Register("IsExperimental", typeof(bool), typeof(SettingsToggleSwitchItem),
            new PropertyMetadata(false));

    public SettingsToggleSwitchItem() {
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

    public bool IsChecked {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public bool IsExperimental {
        get => (bool)GetValue(IsExperimentalProperty);
        set => SetValue(IsExperimentalProperty, value);
    }
}