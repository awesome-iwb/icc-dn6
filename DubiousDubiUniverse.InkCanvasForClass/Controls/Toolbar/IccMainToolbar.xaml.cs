using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DubiousDubiUniverse.InkCanvasForClass.Helpers;
using DubiousDubiUniverse.InkCanvasForClass.Services;
using DubiousDubiUniverse.InkCanvasForClass.Windows;
using Image = System.Windows.Controls.Image;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace DubiousDubiUniverse.InkCanvasForClass.Controls.Toolbar;

public enum ToolbarVisibilityStatus {
    Visible,
    Invisible,
    ShowingWithAnimation,
    HidingWithAnimation
}

public partial class IccMainToolbar {
    private readonly MainToolbarWindow _toolbarWindow;
    private readonly SettingsService _settingsService;
    private bool isInitialized = false;

    public IccMainToolbar(MainToolbarWindow tbWin, SettingsService settingsService) {
        _toolbarWindow = tbWin;
        _settingsService = settingsService;
        InitializeComponent();

        Initialize();
    }

    private void Initialize() {
        // --- 事件添加 ---
        EmojiHeadIcon.MouseDown += EmojiButtonMouseDown;
        EmojiHeadIcon.MouseMove += EmojiButtonMouseMove;
        EmojiHeadIcon.MouseUp += EmojiButtonMouseUp;
        Loaded += (_, __) => {
            if (isInitialized) return;

            // --- 读取设置并应用 ---
            var settings = _settingsService.Settings;
            SetZoomFactor(settings.ToolbarZoomFactor);
            _toolbarWindow.RelocateToolbarToDefaultPlacement();
        };
        // --- 事件添加 ---
    }

    #region General

    public void SetZoomFactor(double zoomFactor) {
        if (zoomFactor < 0.25 || zoomFactor > 3.5) return;
        tbScale.ScaleX = zoomFactor + 0.15;
        tbScale.ScaleY = zoomFactor + 0.15;
    }

    #endregion

    #region SpecificButtons

    /// <summary>
    ///     这个是清屏按钮的事件，同步修改按下后Body边框的颜色
    /// </summary>
    /// <param name="o"></param>
    /// <param name="b"></param>
    private void ClearButton_PressedChanged(object o, bool b) {
        AppleRCornerBorderControlBody.BorderColor = b
            ? (Color)ColorConverter.ConvertFromString("#e7000b")
            : (Color)ColorConverter.ConvertFromString("#9f9fa9");
    }

    #endregion

    #region SmileHeadIcon

    private Point _mouseDownPoint;
    private Point _mouseDownDeltaPoint;
    private Point _mouseMovePoint;
    private bool _isEmojiBtnMouseDown; // 判断鼠标是否被按下
    private readonly double _dpiVal = DpiUtilities.GetWPFDPIScaling();
    private bool _isEmojiIconTriggerMoving; // 是否触发了工具栏的移动

    private ToolbarVisibilityStatus _toolbarBodyVisibilityStatus = ToolbarVisibilityStatus.Visible;

    public void ShowToolbarBody(bool isShow, bool isAnimated) {
        bool isCurrentlyVisible = ToolbarBody.Visibility == Visibility.Visible ||
                                  _toolbarBodyVisibilityStatus == ToolbarVisibilityStatus.ShowingWithAnimation;

        if (isCurrentlyVisible == isShow)
            return;

        double from = isShow ? 0 : 1;
        double to = isShow ? 1 : 0;
        var duration = TimeSpan.FromMilliseconds(isAnimated ? 100 : 0);

        if (ToolbarBody.RenderTransform is not ScaleTransform)
            ToolbarBody.RenderTransform = new ScaleTransform(1, 1);

        if (isShow)
            ToolbarBody.Visibility = Visibility.Visible;

        var scaleAnimation = new DoubleAnimation(from, to, duration) {
            EasingFunction = new PowerEase { Power = 4, EasingMode = EasingMode.EaseOut }
        };
        scaleAnimation.Completed += (s, e) => {
            if (!isShow)
                ToolbarBody.Visibility = Visibility.Collapsed;

            _toolbarBodyVisibilityStatus = isShow
                ? ToolbarVisibilityStatus.Visible
                : ToolbarVisibilityStatus.Invisible;
        };

        ToolbarBody.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);

        var opacityAnimation = new DoubleAnimation(from, to, duration);
        ToolbarBody.BeginAnimation(OpacityProperty, opacityAnimation);

        _toolbarBodyVisibilityStatus = isShow
            ? (isAnimated ? ToolbarVisibilityStatus.ShowingWithAnimation : ToolbarVisibilityStatus.Visible)
            : (isAnimated ? ToolbarVisibilityStatus.HidingWithAnimation : ToolbarVisibilityStatus.Invisible);
    }

    private void EmojiButtonMouseDown(object o, MouseButtonEventArgs e) {
        if (_isEmojiBtnMouseDown) return;
        _mouseDownPoint = ((Visual)e.Source).PointToScreen(e.GetPosition((Image)e.Source));
        var imageLtPoint = ((Image)e.Source).PointToScreen(new Point(0, 0));
        _mouseDownDeltaPoint = new Point(_mouseDownPoint.X - imageLtPoint.X, _mouseDownPoint.Y - imageLtPoint.Y);
        var image = e.Source as Image;
        image.CaptureMouse();
        _isEmojiBtnMouseDown = true;
        _isEmojiIconTriggerMoving = false;
    }

    private void EmojiButtonMouseMove(object o, MouseEventArgs e) {
        if (!_isEmojiBtnMouseDown) return;
        _mouseMovePoint = ((Visual)e.Source).PointToScreen(e.GetPosition((Image)e.Source));
        if (Math.Sqrt(Math.Pow(Math.Abs(_mouseDownPoint.X - _mouseMovePoint.X), 2) +
                      Math.Pow(Math.Abs(_mouseDownPoint.Y - _mouseMovePoint.Y), 2)) >= 16)
            _isEmojiIconTriggerMoving = true;
        if (_isEmojiIconTriggerMoving)
            _toolbarWindow.RelocateToolbarWindow(_mouseMovePoint.X / _dpiVal - _mouseDownDeltaPoint.X / _dpiVal,
                _mouseMovePoint.Y / _dpiVal - _mouseDownDeltaPoint.Y / _dpiVal);
    }

    private void EmojiButtonMouseUp(object o, MouseButtonEventArgs e) {
        if (!_isEmojiBtnMouseDown) return;
        var image = e.Source as Image;
        image.ReleaseMouseCapture();
        _isEmojiBtnMouseDown = false;
        if (_isEmojiIconTriggerMoving == false &&
            _toolbarBodyVisibilityStatus != ToolbarVisibilityStatus.HidingWithAnimation &&
            _toolbarBodyVisibilityStatus != ToolbarVisibilityStatus.ShowingWithAnimation)
            ShowToolbarBody(_toolbarBodyVisibilityStatus != ToolbarVisibilityStatus.Visible, true);
        _isEmojiIconTriggerMoving = false;
    }

    #endregion
}