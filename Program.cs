using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class SeewoMonitor
{
    // 配置
    static string TARGET_PROCESS = "rtcRemoteDesktop";
    static int CHECK_INTERVAL = 1000; // 毫秒
    static string TARGET_URL_ON_START = "classisland://app/api/automation/run/sm_run";
    static string TARGET_URL_ON_STOP = "classisland://app/api/automation/run/sm_stop";

    static bool lastRunning = false;

    static void Main(string[] args)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine("SeewoMonitor");
        Console.WriteLine("Version 1.0.0");
        Console.WriteLine("Powered by FeltSquirrel727");
        Console.WriteLine("Improved by Mistveil-Z");
        Console.WriteLine("-----------------------------");
        Console.WriteLine($"当前检测程序: {TARGET_PROCESS}.exe");
        Console.WriteLine($"当前检测间隔: {CHECK_INTERVAL} ms");
        Console.WriteLine($"启动调用URL: {TARGET_URL_ON_START}");
        Console.WriteLine($"停止调用URL: {TARGET_URL_ON_STOP}");

        while (true)
        {
            bool running = Process.GetProcessesByName(TARGET_PROCESS).Any();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 状态: {running}");

            if (running && !lastRunning)
            {
                OpenUrl(TARGET_URL_ON_START);
                Console.WriteLine($"检测到启动，已调用 {TARGET_URL_ON_START}");
            }
            else if (!running && lastRunning)
            {
                OpenUrl(TARGET_URL_ON_STOP);
                Console.WriteLine($"检测到终止，已调用 {TARGET_URL_ON_STOP}");
            }

            lastRunning = running;
            Thread.Sleep(CHECK_INTERVAL);
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
            Console.WriteLine("打开URL失败: " + e.Message);
        }
    }
}
