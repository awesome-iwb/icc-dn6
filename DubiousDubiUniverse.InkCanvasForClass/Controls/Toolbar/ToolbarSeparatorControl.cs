using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DubiousDubiUniverse.InkCanvasForClass.Controls.Toolbar
{
    public class ToolbarSeparatorControl : Control {
        public ToolbarSeparatorControl() {
            Style = new ResourceDictionary() {
                Source = new Uri(
                    "/DubiousDubiUniverse.InkCanvasForClass;component/Resources/Dictionaries/ToolbarSeparatorControlStyleDictionary.xaml",
                    UriKind.Relative)
            }["tb_default_separator"] as Style;
        }

        public double SeparatorThickness {
            get => (double)GetValue(SeparatorThicknessProperty);
            set => SetValue(SeparatorThicknessProperty, value);
        }

        public static readonly DependencyProperty SeparatorThicknessProperty =
            DependencyProperty.Register(nameof(SeparatorThickness), typeof(double), typeof(ToolbarSeparatorControl),
                new PropertyMetadata(2.0));

        public double CornerRadius {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(ToolbarSeparatorControl),
                new PropertyMetadata(2.0));

        public Color SeparatorColor {
            get => (Color)GetValue(SeparatorColorProperty);
            set => SetValue(SeparatorColorProperty, value);
        }

        public static readonly DependencyProperty SeparatorColorProperty =
            DependencyProperty.Register(nameof(SeparatorColor), typeof(Color), typeof(ToolbarSeparatorControl),
                new PropertyMetadata(Colors.Black));

        public double SeparatorOpacity {
            get => (double)GetValue(SeparatorOpacityProperty);
            set => SetValue(SeparatorOpacityProperty, value);
        }

        public static readonly DependencyProperty SeparatorOpacityProperty =
            DependencyProperty.Register(nameof(SeparatorOpacity), typeof(double), typeof(ToolbarSeparatorControl),
                new PropertyMetadata(1.0));

        public double SeparatorHeight {
            get => (double)GetValue(SeparatorHeightProperty);
            set => SetValue(SeparatorHeightProperty, value);
        }

        public static readonly DependencyProperty SeparatorHeightProperty =
            DependencyProperty.Register(nameof(SeparatorHeight), typeof(double), typeof(ToolbarSeparatorControl),
                new PropertyMetadata(48.0));
    }
}
