using System.Runtime.InteropServices;
using System.Windows;

namespace DubiousDubiUniverse.InkCanvasForClass.Helpers;

public static class DpiUtilities {
    private const int MONITOR_DEFAULTTONEAREST = 2;

    // you should always use this one and it will fallback if necessary
    // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow
    public static int GetDpiForWindow(IntPtr hwnd) {
        var h = LoadLibrary("user32.dll");
        var ptr = GetProcAddress(h, "GetDpiForWindow"); // Windows 10 1607
        if (ptr == IntPtr.Zero)
            return GetDpiForNearestMonitor(hwnd);

        return Marshal.GetDelegateForFunctionPointer<GetDpiForWindowFn>(ptr)(hwnd);
    }

    public static int GetDpiForNearestMonitor(IntPtr hwnd) {
        return GetDpiForMonitor(GetNearestMonitorFromWindow(hwnd));
    }

    public static int GetDpiForNearestMonitor(int x, int y) {
        return GetDpiForMonitor(GetNearestMonitorFromPoint(x, y));
    }

    public static int GetDpiForMonitor(IntPtr monitor, MonitorDpiType type = MonitorDpiType.Effective) {
        var h = LoadLibrary("shcore.dll");
        var ptr = GetProcAddress(h, "GetDpiForMonitor"); // Windows 8.1
        if (ptr == IntPtr.Zero)
            return GetDpiForDesktop();

        var hr = Marshal.GetDelegateForFunctionPointer<GetDpiForMonitorFn>(ptr)(monitor, type, out var x, out var y);
        if (hr < 0)
            return GetDpiForDesktop();

        return x;
    }

    public static int GetDpiForDesktop() {
        var hr = D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED, typeof(ID2D1Factory).GUID,
            IntPtr.Zero, out var factory);
        if (hr < 0)
            return 96; // we really hit the ground, don't know what to do next!

        factory.GetDesktopDpi(out var x, out var y); // Windows 7
        Marshal.ReleaseComObject(factory);
        return (int)x;
    }

    public static IntPtr GetDesktopMonitor() {
        return GetNearestMonitorFromWindow(GetDesktopWindow());
    }

    public static IntPtr GetShellMonitor() {
        return GetNearestMonitorFromWindow(GetShellWindow());
    }

    public static IntPtr GetNearestMonitorFromWindow(IntPtr hwnd) {
        return MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
    }

    public static IntPtr GetNearestMonitorFromPoint(int x, int y) {
        return MonitorFromPoint(new POINT { x = x, y = y }, MONITOR_DEFAULTTONEAREST);
    }

    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpLibFileName);

    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("user32")]
    private static extern IntPtr MonitorFromPoint(POINT pt, int flags);

    [DllImport("user32")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int flags);

    [DllImport("user32")]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("user32")]
    private static extern IntPtr GetShellWindow();

    [DllImport("d2d1")]
    private static extern int D2D1CreateFactory(D2D1_FACTORY_TYPE factoryType,
        [MarshalAs(UnmanagedType.LPStruct)] Guid riid, IntPtr pFactoryOptions, out ID2D1Factory ppIFactory);

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    public static double GetWPFDPIScaling() {
        var resHeight = GetSystemMetrics(1); // 1440
        var actualHeight = SystemParameters.PrimaryScreenHeight; // 960
        var ratio = actualHeight / resHeight;
        var dpi = resHeight / actualHeight;
        return dpi;
    }

    private delegate int GetDpiForWindowFn(IntPtr hwnd);

    private delegate int GetDpiForMonitorFn(IntPtr hmonitor, MonitorDpiType dpiType, out int dpiX, out int dpiY);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT {
        public int x;
        public int y;
    }

    private enum D2D1_FACTORY_TYPE {
        D2D1_FACTORY_TYPE_SINGLE_THREADED = 0,
        D2D1_FACTORY_TYPE_MULTI_THREADED = 1
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("06152247-6f50-465a-9245-118bfd3b6007")]
    private interface ID2D1Factory {
        int ReloadSystemMetrics();

        [PreserveSig]
        void GetDesktopDpi(out float dpiX, out float dpiY);

        // the rest is not implemented as we don't need it
    }
}

public enum MonitorDpiType {
    Effective = 0,
    Angular = 1,
    Raw = 2
}