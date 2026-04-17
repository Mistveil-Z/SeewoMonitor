<div align="center">

# SeewoMonitor

[![Stars](https://img.shields.io/github/stars/Mistveil-Z/SeewoMonitor?style=flat-square)](https://github.com/Mistveil-Z/SeewoMonitor)
[![正式版 Release](https://img.shields.io/github/v/release/Mistveil-Z/SeewoMonitor?style=flat-square&color=green&label=正式版)](https://github.com/Mistveil-Z/SeewoMonitor/releases)
[![.NET 版本](https://img.shields.io/badge/.NET-8.0-blue?style=flat-square)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

SeewoMonitor 是一款专为希沃集控设计的监控软件，当教师使用希沃集控发送通知 ~~（进行视奸）~~ 时，SeewoMonitor 可以发送 ClassIsland Uri 或发出系统通知，以便学生及时发现并采取措施。

---

</div>

## 功能

> [ !CAUTION]
>
> 不同版本的希沃集控可能会有不同的行为，SeewoMonitor 可能无法在所有版本中正常工作。
>
> 不同版本的 SeewoMonitor 可能会有不同的功能和使用方法，请注意查看对应版本的说明。

### 希沃集控监控检测

- [x] 当教师使用希沃集控进行监控时，SeewoMonitor 可以检测到这一行为，并发出相应的通知。
- [x] 通过 `--check-interval=1000` 或 `--check-interval 1000` (单位为毫秒) 参数设置检测间隔时间，默认为 1000 毫秒。

### 通知提醒

- [x] 通过 `--notify` 或 `-n` 参数启用 SeewoMonitor 的启动通知提醒（之后会将这个参数放到“是否启用‘系统通知’”功能）。
- [x] 通过发送 ClassIsland Uri （需要配合 ClassIsland 使用，之后会添加额外参数）。
- [ ] 通过发出系统通知。

### 日志

- [x] 通过 `--console` 或 `-c` 参数将监控行为打印到控制台。
- [ ] 记录监控行为的日志。

### AppMonitor

- [ ] 监控其他应用程序的行为，并发出相应的通知（将更改项目名称）。

### 图形化界面 

- [ ] 提供图形化界面，方便用户操作 ~~（大概率不会做，但是有可能做成 ClassIsland 插件）~~。

### 配置文件

- [ ] 提供配置文件，无需通过启动参数传参。

---

## 开始使用

> [ !IMPORTANT]
>
> 当使用“通过发送 ClassIsland Uri”时，确保已经安装并正确配置了 ClassIsland，以便能够接收 SeewoMonitor 发出的 Uri 通知。

> [ !TIP]
>
> 有关 ClassIsland 自动化功能的详细信息，请参阅 [ClassIsland 文档-自动化](https://docs.classisland.tech/app/automation.html)。 

### 启动 SeewoMonitor

- **1. 通过 ClassIsland 自动化启动（推荐）。**

  - 触发器：应用启动时

  - 行动：运行-应用程序
	> 绝对路径 `SeewoMonitor.exe`
	>
	> 启动参数 `-启动参数`

- **2. 通过命令行启动。**

  ```bash
  # 显示控制台窗口，设置检测间隔时间为 1000 毫秒，启用启动通知提醒。
  SeewoMonitor.exe -启动参数
  ```

- **3. 通过快捷方式启动 ~~（有必要在这里写这个吗）~~。**

  > 创建 SeewoMonitor 的快捷方式，放置于自启动文件夹内，并在快捷方式的属性中添加启动参数。

### 启动参数说明

- **启动参数1-显示控制台窗口：** `--console` 或 `-c`。

- **启动参数2-设置检测间隔时间：** `--check-interval=1000` 或 `--check-interval 1000` (单位为毫秒)，默认为 1000 毫秒。

- **启动参数3-启用启动通知提醒：** `--notify` 或 `-n`。

### 提醒方式说明

- **1. 通过发送 ClassIsland Uri。**

  - 触发器：收到Uri时
	> SeewoMonitor 启动： `sm_run`

	> SeewoMonitor 终止： `sm_stop` （可能存在延迟）

  - 行动：显示提醒

- **2. 通过发出系统通知。**

  > 呃，这个功能还没有实现，所以先不写了。

---

## 免责声明

### 小心点！

- 被发现了作者不担责。

---

## 其它

### 由来

- 见下。

  >希沃集控的监控功能虽然方便了教师，但也引起了学生的担忧和不安。为了帮助学生及时发现监控行为并采取措施，SeewoMonitor 应运而生 ~~（AI写的，你别管）~~。

### 实现方式

- rtcRemoteDesktop.exe。

  >SeewoMonitor 通过监控 `rtcRemoteDesktop.exe` 进程的行为来检测希沃集控的监控行为。当检测到监控行为时，SeewoMonitor 可以通过发送 ClassIsland Uri 或发出系统通知来提醒学生。`

### 鸣谢

- FeltSquirrel727。

  > 本项目一开始由 FeltSquirrel727 由 Python 构建，Mistveil-Z 进行重构和维护。