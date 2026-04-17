using System.Diagnostics;
using System.Runtime.InteropServices;

class SeewoMonitor
{
    // 配置
    static string TARGET_PROCESS = "rtcRemoteDesktop";
    static int CHECK_INTERVAL = 1000; // 毫秒
    static string TARGET_URL_ON_START = "classisland://app/api/automation/run/sm_run";
    static string TARGET_URL_ON_STOP = "classisland://app/api/automation/run/sm_stop";

    static bool lastRunning = false;
    static Mutex? appMutex;

    // WinAPI
    [DllImport("kernel32.dll")]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    private static extern bool FreeConsole();

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [STAThread]
    static void Main(string[] args)
    {
        bool notifyOnStart = false;

        // 单实例校验
        const string mutexName = "Global\\SeewoMonitor_Mutex_v1";
        bool createdNew;
        try
        {
            appMutex = new Mutex(true, mutexName, out createdNew);
        }
        catch
        {
            createdNew = false;
        }

        if (!createdNew)
        {
            // 第二个实例：显示通知并退出
            ShowNotification("SeewoMonitor", "程序已在运行，第二个实例已退出。");
            return;
        }

        // 解析参数（包含 console、check-interval、notify）
        ProcessStartupArgs(args, ref notifyOnStart);

        if (notifyOnStart)
        {
            ShowNotification("SeewoMonitor", "程序已启动。");
        }

        Log("-----------------------------");
        Log("SeewoMonitor");
        Log("Version 1.1.0.1");
        Log("Powered by Mistveil-Z and FeltSquirrel727");
        Log("-----------------------------");
        Log($"当前检测程序: {TARGET_PROCESS}.exe");
        Log($"当前检测间隔: {CHECK_INTERVAL} ms");
        Log($"启动调用URL: {TARGET_URL_ON_START}");
        Log($"停止调用URL: {TARGET_URL_ON_STOP}");

        try
        {
            while (true)
            {
                bool running = Process.GetProcessesByName(TARGET_PROCESS).Any();

                Log($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 状态: {running}");

                if (running && !lastRunning)
                {
                    OpenUrl(TARGET_URL_ON_START);
                    Log($"检测到启动，已调用 {TARGET_URL_ON_START}");
                }
                else if (!running && lastRunning)
                {
                    OpenUrl(TARGET_URL_ON_STOP);
                    Log($"检测到终止，已调用 {TARGET_URL_ON_STOP}");
                }

                lastRunning = running;
                Thread.Sleep(CHECK_INTERVAL);
            }
        }
        finally
        {
            // 释放互斥体（进程结束时）
            try { appMutex?.ReleaseMutex(); } catch { }
            appMutex?.Dispose();
        }
    }

    static void ProcessStartupArgs(string[] args, ref bool notifyOnStart)
    {
        if (args == null || args.Length == 0) return;

        // 查找 --console 参数
        bool wantConsole = args.Any(a => string.Equals(a, "--console", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(a, "-c", StringComparison.OrdinalIgnoreCase));

        if (wantConsole && GetConsoleWindow() == IntPtr.Zero)
        {
            AllocConsole();
        }

        // 处理 --check-interval=<ms> 或 --check-interval <ms>
        for (int i = 0; i < args.Length; i++)
        {
            string a = args[i];
            if (a.StartsWith("--check-interval", StringComparison.OrdinalIgnoreCase))
            {
                string? value = null;
                int eq = a.IndexOf('=');
                if (eq >= 0 && eq + 1 < a.Length)
                {
                    value = a.Substring(eq + 1);
                }
                else if (i + 1 < args.Length)
                {
                    value = args[i + 1];
                }

                if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int ms) && ms > 0)
                {
                    CHECK_INTERVAL = ms;
                    Log($"已设置检测间隔为 {CHECK_INTERVAL} ms");
                }
                else
                {
                    Log("无效的 --check-interval 值，保持默认。");
                }
            }
            else if (string.Equals(a, "--notify", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(a, "-n", StringComparison.OrdinalIgnoreCase))
            {
                notifyOnStart = true;
            }
        }
    }

    static bool IsConsoleAttached()
    {
        return GetConsoleWindow() != IntPtr.Zero;
    }

    static void Log(string message)
    {
        if (IsConsoleAttached())
        {
            Console.WriteLine(message);
        }
    }

    static void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception e)
        {
            Log("打开URL失败: " + e.Message);
        }
    }

    static void ShowNotification(string title, string text, int timeoutMs = 4000)
    {
        // 使用 NotifyIcon 显示托盘气泡通知
        try
        {
            using NotifyIcon ni = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                BalloonTipTitle = title,
                BalloonTipText = text
            };

            // ShowBalloonTip 的 timeout 参数单位为毫秒
            ni.ShowBalloonTip(timeoutMs);

            // 显示通知，随后退出或继续运行
            Thread.Sleep(Math.Min(timeoutMs + 500, 5000));
            ni.Visible = false;
        }
        catch
        {
            // 无法显示通知时，不抛出异常
        }
    }
}
