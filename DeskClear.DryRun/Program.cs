using DeskClear.Core;

Console.WriteLine("==========================================");
Console.WriteLine(" DeskClear サンダーボルト照準確認モード");
Console.WriteLine(" ※ WM_CLOSE は送信しません");
Console.WriteLine("==========================================");
Console.WriteLine();

IReadOnlyList<WindowTargetInfo> targets = WindowCloser.InspectTargets();

if (targets.Count == 0)
{
    Console.WriteLine("対象ウィンドウはありません。");
}
else
{
    int number = 1;

    foreach (WindowTargetInfo target in targets)
    {
        bool shellLike =
            target.ClassName.Equals("Shell_TrayWnd", StringComparison.OrdinalIgnoreCase) ||
            target.ClassName.Equals("Shell_SecondaryTrayWnd", StringComparison.OrdinalIgnoreCase);

        string warning = shellLike ? "  <<< 要確認: Shell系UI" : "";

        Console.WriteLine($"[{number}]");
        Console.WriteLine($"Process : {target.ProcessName}");
        Console.WriteLine($"PID     : {target.ProcessId}");
        Console.WriteLine($"Class   : {target.ClassName}{warning}");
        Console.WriteLine($"ToolWnd : {(target.IsToolWindow ? "YES <<< 要確認" : "NO")}");
        Console.WriteLine($"Title   : {target.Title}");
        Console.WriteLine();

        number++;
    }
}

Console.WriteLine("------------------------------------------");
Console.WriteLine($"照準対象: {targets.Count} 個");
Console.WriteLine("このモードではウィンドウを閉じていません。");
Console.WriteLine();
Console.WriteLine("Enterキーで終了します。");
Console.ReadLine();