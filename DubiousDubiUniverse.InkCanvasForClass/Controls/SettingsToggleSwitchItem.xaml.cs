using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DubiousDubiUniverse.InkCanvasForClass.Controls {
    /// <summary>
    /// SettingsToggleSwitchItem.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsToggleSwitchItem : UserControl {
        public SettingsToggleSwitchItem() {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SettingsToggleSwitchItem),
                new PropertyMetadata("默认标题"));

        public string Title {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(SettingsToggleSwitchItem),
                new PropertyMetadata("默认描述"));

        public string Description {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(SettingsToggleSwitchItem),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsChecked {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty IsExperimentalProperty =
            DependencyProperty.Register("IsExperimental", typeof(bool), typeof(SettingsToggleSwitchItem),
                new PropertyMetadata(false));

        public bool IsExperimental {
            get => (bool)GetValue(IsExperimentalProperty);
            set => SetValue(IsExperimentalProperty, value);
        }
    }
}
