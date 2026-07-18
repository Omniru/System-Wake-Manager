# System Wake Manager

A small Windows utility for seeing which devices are allowed to wake your
computer, and turning that permission on or off. It is a graphical front end for
the built-in `powercfg` command, so you do not have to remember device names or
type them into an elevated prompt.

Useful when a machine keeps waking itself up at night and you want to find the
culprit — typically a network adapter, a mouse, or a "wake timer" capable device.

> **Read [DISCLAIMER.md](DISCLAIMER.md) before running this.** The application
> changes your system's power configuration, requires administrator rights, and
> applies changes immediately without a confirmation prompt. Disabling wake on
> your keyboard, mouse, or network adapter can leave a machine you cannot wake
> remotely, or at all except by the physical power button.

## Using it

Run `System.Wake.Manager.exe`. It will request administrator rights, which
`powercfg` needs in order to change wake settings.

| Button | What it does |
| --- | --- |
| **All Wake Devices** | Lists every device capable of waking the system. |
| **Armed Wake Devices** | Lists only devices currently allowed to wake it. |
| **Last Used Wake Device** | Shows what woke the machine most recently. |
| **Enable Wake Device** | Allows the selected device to wake the system. |
| **Disable Wake Device** | Stops the selected device from waking it. |
| **Disable All Devices** | Stops *every* device in the current list. See the disclaimer. |

Select a device in the list, then use the Enable/Disable buttons — or just
double-click a device to toggle it.

In the **All Wake Devices** view, devices whose wake permission is disabled are
shown greyed out and struck through, so you can see at a glance what is on and
what is off. A device you just disabled stays visible in the list, restyled
rather than removed, until you reload with one of the list buttons.

## Building from source

Requires the .NET Framework 4.0 runtime (present on any modern Windows) and, for
the recommended path, the
[.NET Framework Developer Pack](https://aka.ms/msbuild/developerpacks).

Double-click **`publish.bat`**. It builds in Release and copies the result to
`System.Wake.Manager.exe` in the repository root.

Alternatively:

```powershell
# Build only, no publish step
.\build.ps1 -Configuration Release
```

```powershell
# If you have the developer pack installed, the standard toolchain works too
MSBuild systemWakeManager.sln /t:Rebuild /p:Configuration=Release
```

`build.ps1` exists because MSBuild fails with `MSB3644` when no targeting pack
is installed; it drives `csc.exe` from the .NET Framework runtime directory
instead, mirroring the settings in `systemWakeManager.csproj`. Building does not
require administrator rights — only running the finished application does.

## Recovering a machine that will not wake

If you disabled wake on the wrong device, you do not need this application to
undo it. From an elevated Command Prompt:

```
powercfg -devicequery wake_from_any
powercfg -deviceenablewake "Device Name Here"
```

See [DISCLAIMER.md](DISCLAIMER.md) for the full recovery notes.

## License

MIT — see [LICENSE.md](LICENSE.md).
