# Safe Exam Browser (Refactoring Study)

> A security-research fork of [Safe Exam Browser](https://github.com/SafeExamBrowser/seb-win-refactoring) (SEB) for Windows, used to study how the lockdown browser enforces its exam environment and how those enforcement mechanisms are implemented in code.

![C#](https://img.shields.io/badge/C%23-.NET%20Framework%204.7.2-512BD4?logo=csharp&logoColor=white)
![Platform](https://img.shields.io/badge/platform-Windows-0078D6?logo=windows&logoColor=white)
![Chromium](https://img.shields.io/badge/engine-Chromium%20(WebView2)-4285F4?logo=googlechrome&logoColor=white)

## Overview

Safe Exam Browser is an open-source kiosk application that locks down a Windows workstation into a controlled environment for running online exams. This repository is a **reverse-engineering and code-analysis study** of the official SEB Windows client: the goal was to understand the layered architecture behind the browser's lockdown, monitoring, and configuration-integrity features, and to document what each subsystem does by reading and selectively modifying its source.

The work was a solo learning exercise. It uses standard reverse-engineering tooling (traffic inspection with proxy/packet-capture utilities and testing inside virtual machines) to trace how the runtime, service, and client processes cooperate to establish and verify the exam environment. This README describes the project neutrally as a study of the software's internals; it is not a guide for defeating exam proctoring.

## What was analyzed

The SEB codebase is organized as a large multi-project .NET solution. The study focused on the subsystems that create and guard the locked-down session:

- **Runtime / Client / Service split** — SEB runs as cooperating processes (a privileged Windows service, a runtime coordinator, and the user-facing client) that communicate over a local IPC channel. The analysis followed how a session is bootstrapped and torn down through these components.
- **Kiosk mode & operations pipeline** (`SafeExamBrowser.Runtime/Operations`) — the ordered "operations" that set up a session: display configuration, virtual-machine and remote-session detection, kiosk-mode activation, and application startup.
- **Monitoring** (`SafeExamBrowser.Monitoring`) — keyboard/mouse interception, active-window and display-change detection, and the process/application monitor used to watch the environment.
- **Configuration & cryptography** (`SafeExamBrowser.Configuration`) — how `.seb` configuration data is parsed and mapped, and how the browser exam key / config key hashes are generated and checked to prove configuration integrity to an exam server.
- **Browser** (`SafeExamBrowser.Browser`) — the integrated Chromium engine (via Microsoft Edge WebView2 / CefSharp) that renders the exam page.

The commit history documents targeted modifications to these subsystems (e.g. adjusting monitoring behavior, kiosk defaults, VM/remote-session checks, and configuration-key handling) that were made to observe how each guard affects the running environment. A small companion project, `TestExamKeyHash`, was added to experiment with SEB's exam-key hashing logic in isolation.

## Tech stack

- **Language:** C# on .NET Framework 4.7.2 (WPF for the desktop/mobile UIs)
- **Browser engine:** Chromium via Microsoft Edge WebView2 Runtime
- **Build/CI:** Visual Studio solution (`SafeExamBrowser.sln`), MSI/setup bundle projects, AppVeyor + Codecov config (upstream)
- **Analysis tooling:** proxy-based HTTPS inspection, packet capture, and virtual machines for isolated testing

## Requirements

SEB 3.x requires the following to run (installed automatically by the setup bundle; only needed manually with the MSI packages):

- .NET Framework 4.7.2 Runtime — https://dotnet.microsoft.com/download/dotnet-framework/net472
- Microsoft Edge WebView2 Runtime — https://go.microsoft.com/fwlink/p/?LinkId=2124703
- Visual C++ 2015-2019 Redistributable — https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads

## Project structure

The solution follows a contracts-based, dependency-injected layout. Each functional area ships as a pair of `*.Contracts` (interfaces/DTOs) and implementation assemblies, with matching `*.UnitTests` projects:

```
SafeExamBrowser.Runtime        Session lifecycle & operations pipeline
SafeExamBrowser.Client         User-facing exam client
SafeExamBrowser.Service        Privileged Windows service
SafeExamBrowser.Browser        Chromium integration
SafeExamBrowser.Monitoring     Keyboard/mouse/window/display/process monitoring
SafeExamBrowser.Configuration  .seb parsing, data mapping & cryptography
SafeExamBrowser.Communication  Inter-process communication
SafeExamBrowser.Lockdown       Feature lockdown
SafeExamBrowser.WindowsApi     Native Windows API wrappers
SafeExamBrowser.UserInterface.*  WPF UI (Desktop / Mobile / Shared)
TestExamKeyHash                Standalone exam-key hashing experiment
```

## Building

Open `SafeExamBrowser.sln` in Visual Studio with the .NET Framework 4.7.2 developer pack and the WebView2 SDK installed, then build the solution. Build artifacts are for study/testing only and should not be used in any production or examination setting.

## Credits & license

This is a fork of the official [SafeExamBrowser/seb-win-refactoring](https://github.com/SafeExamBrowser/seb-win-refactoring) project, developed at ETH Zürich. All upstream copyright and the original license (see [`LICENSE.txt`](LICENSE.txt), MPL 2.0) apply. This repository exists for educational security research only.
