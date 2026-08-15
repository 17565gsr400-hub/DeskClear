using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace DeskClear.Core
{
    public static class WindowCloser
    {
        private const uint WM_CLOSE = 0x0010;
        private const int GWL_EXSTYLE = -20;
        private const long WS_EX_TOOLWINDOW = 0x00000080L;

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongW")]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        public static void CloseAll()
        {
            foreach (IntPtr hWnd in GetTargetWindows())
            {
                PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        // Dry Run用。本番と同じ対象判定を使うが、WM_CLOSEは送らない。
        public static IReadOnlyList<WindowTargetInfo> InspectTargets()
        {
            List<WindowTargetInfo> result = new();

            foreach (IntPtr hWnd in GetTargetWindows())
            {
                GetWindowThreadProcessId(hWnd, out uint processId);

                result.Add(new WindowTargetInfo
                {
                    Handle = hWnd,
                    ProcessId = processId,
                    ProcessName = GetProcessName(processId),
                    Title = GetTitle(hWnd),
                    ClassName = GetClassNameText(hWnd),
                    IsToolWindow = IsToolWindow(hWnd)
                });
            }

            return result;
        }

        // CloseAll() と InspectTargets() が同じ判定を共有する。
        private static List<IntPtr> GetTargetWindows()
        {
            int myProcessId = Environment.ProcessId;
            IntPtr shellWindow = GetShellWindow();
            IntPtr desktopWindow = GetDesktopWindow();

            List<IntPtr> targetWindows = new();

            EnumWindows((hWnd, lParam) =>
            {
                // Windows Shellのデスクトップ本体を保護
                if (hWnd == shellWindow || hWnd == desktopWindow)
                    return true;

                // 非表示ウィンドウは対象外
                if (!IsWindowVisible(hWnd))
                    return true;

                // タスクバーなどWindows Shellの操作基盤を明示保護
                string className = GetClassNameText(hWnd);

                if (className.Equals("Shell_TrayWnd", StringComparison.OrdinalIgnoreCase) ||
                    className.Equals("Shell_SecondaryTrayWnd", StringComparison.OrdinalIgnoreCase))
                    return true;

                // 通常アプリ窓ではないToolWindowを対象外
                if (IsToolWindow(hWnd))
                    return true;

                // タイトルなしウィンドウは対象外
                if (GetWindowTextLength(hWnd) == 0)
                    return true;

                GetWindowThreadProcessId(hWnd, out uint processId);

                // DeskClear自身を保護
                if (processId == myProcessId)
                    return true;

                // Windowsの入力基盤UIを保護
                string processName = GetProcessName(processId);

                if (processName.Equals("TextInputHost", StringComparison.OrdinalIgnoreCase))
                    return true;

                targetWindows.Add(hWnd);
                return true;

            }, IntPtr.Zero);

            return targetWindows;
        }

        private static string GetTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);

            if (length <= 0)
                return "";

            StringBuilder buffer = new(length + 1);
            GetWindowText(hWnd, buffer, buffer.Capacity);
            return buffer.ToString();
        }

        private static string GetClassNameText(IntPtr hWnd)
        {
            StringBuilder buffer = new(256);
            GetClassName(hWnd, buffer, buffer.Capacity);
            return buffer.ToString();
        }

        private static string GetProcessName(uint processId)
        {
            try
            {
                using Process process = Process.GetProcessById((int)processId);
                return process.ProcessName;
            }
            catch
            {
                return "(取得失敗)";
            }
        }

        private static bool IsToolWindow(IntPtr hWnd)
        {
            long exStyle = GetWindowLongPtr(hWnd, GWL_EXSTYLE).ToInt64();
            return (exStyle & WS_EX_TOOLWINDOW) != 0;
        }

        private static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return IntPtr.Size == 8
                ? GetWindowLongPtr64(hWnd, nIndex)
                : new IntPtr(GetWindowLong32(hWnd, nIndex));
        }
    }
}
