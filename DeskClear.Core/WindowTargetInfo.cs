using System;

namespace DeskClear.Core
{
    public sealed class WindowTargetInfo
    {
        public IntPtr Handle { get; init; }
        public uint ProcessId { get; init; }
        public string ProcessName { get; init; } = "";
        public string Title { get; init; } = "";
        public string ClassName { get; init; } = "";
        public bool IsToolWindow { get; init; }
    }
}
