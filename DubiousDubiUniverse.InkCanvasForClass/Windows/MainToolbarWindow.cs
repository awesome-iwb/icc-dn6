using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using DubiousDubiUniverse.InkCanvasForClass.Controls.Toolbar;
using DubiousDubiUniverse.InkCanvasForClass.Models;
using DubiousDubiUniverse.InkCanvasForClass.Services;

namespace DubiousDubiUniverse.InkCanvasForClass.Windows {
    public class MainToolbarWindow : Window {
        private readonly IccMainToolbar toolbar;
        private readonly SettingsService settingsService;

        // 注入 IServiceProvider 以便通过 DI 创建 IccMainToolbar
        public MainToolbarWindow(IServiceProvider serviceProvider, SettingsService settingsService) {
            // —— 窗口属性 ——
            Title = "MainToolbarWindow";
            Background = Brushes.Transparent;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            AllowsTransparency = true;
            SizeToContent = SizeToContent.WidthAndHeight;
            // —— 窗口属性 ——

            this.settingsService = settingsService;

            // 通过 ActivatorUtilities 使用 DI 容器创建 Toolbar，并传入 this 窗口实例
            toolbar = ActivatorUtilities.CreateInstance<IccMainToolbar>(serviceProvider, this);
            Content = toolbar;
        }

        public void RelocateToolbarWindow(double x, double y) {
            Left = x;
            Top = y;
        }

        public void RelocateToolbarWindowWithDelta(double deltaX, double deltaY) {
            Left += deltaX;
            Top += deltaY;
        }

        public void RelocateToolbarToDefaultPlacement() {
            var settings = settingsService.Settings;
            Rect workArea = SystemParameters.WorkArea;
            var placement = settings.ToolbarDefaultPlacement;
            var anchor = settings.ToolbarAnchorPoint;

            // Margin sides based on anchor (0:left/top, 1:right/bottom)
            bool marginRight = anchor.X == 1;
            bool marginBottom = anchor.Y == 1;
            double xMargin = settings.ToolbarDefaultPlacementXMargin;
            double yMargin = settings.ToolbarDefaultPlacementYMargin;

            double x = Left;
            double y = Top;

            // 此时还没有获取到
            if (ActualWidth == 0 || ActualHeight == 0) return;

            // 计算水平位置
            switch (placement) {
                case ToolbarPlacement.LeftTop:
                case ToolbarPlacement.LeftBottom:
                    x = workArea.Left + (marginRight ? -xMargin : xMargin);
                    break;
                case ToolbarPlacement.RightTop:
                case ToolbarPlacement.RightBottom:
                    x = workArea.Right - ActualWidth + (marginRight ? -xMargin : xMargin);
                    break;
                case ToolbarPlacement.Top:
                case ToolbarPlacement.Bottom:
                case ToolbarPlacement.Center:
                    x = workArea.Left + (workArea.Width - ActualWidth) / 2 + (marginRight ? -xMargin : xMargin);
                    break;
            }

            // 计算垂直位置
            switch (placement) {
                case ToolbarPlacement.Top:
                case ToolbarPlacement.LeftTop:
                case ToolbarPlacement.RightTop:
                    y = workArea.Top + (marginBottom ? -yMargin : yMargin);
                    break;
                case ToolbarPlacement.Bottom:
                case ToolbarPlacement.LeftBottom:
                case ToolbarPlacement.RightBottom:
                    y = workArea.Bottom - ActualHeight + (marginBottom ? -yMargin : yMargin);
                    break;
                case ToolbarPlacement.Center:
                    y = workArea.Top + (workArea.Height - ActualHeight) / 2 + (marginBottom ? -yMargin : yMargin);
                    break;
            }

            RelocateToolbarWindow(x, y);
        }
    }
}