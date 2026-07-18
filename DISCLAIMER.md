# Disclaimer and Conditions of Use

## No warranty and no liability

System Wake Manager is provided free of charge, "as is", under the MIT License
(see [LICENSE.md](LICENSE.md)). The warranty disclaimer and limitation of
liability in that license apply in full and are incorporated here by reference.
Nothing in this document grants any warranty that the license disclaims.

To the maximum extent permitted by applicable law, the authors and copyright
holders are not liable for any direct, indirect, incidental, special,
consequential, or exemplary damages arising from the use of this software,
including but not limited to lost data, lost profits, missed backups, missed
scheduled tasks, loss of remote access, hardware inoperability, or business
interruption.

Some jurisdictions do not allow the exclusion of certain warranties or the
limitation of liability for personal injury, gross negligence, or willful
misconduct. Where such exclusions are not permitted, the disclaimers above apply
only to the extent the law allows, and the remaining provisions stay in effect.

## What this software actually does

This is a graphical front end for the Microsoft Windows `powercfg` command. It
requires administrator privileges and it **modifies your system's power
configuration**. Specifically it runs:

- `powercfg -devicequery wake_from_any` / `wake_armed` (read only)
- `powercfg -lastwake` (read only)
- `powercfg -devicedisablewake "<device>"` (**modifies system state**)
- `powercfg -deviceenablewake "<device>"` (**modifies system state**)

The software does not transmit data anywhere, does not phone home, and makes no
changes beyond invoking the `powercfg` commands listed above.

## Specific risks you accept by using this software

Read these before running it. They are the realistic failure modes, not
hypotheticals.

1. **Your computer may stop waking up.** Disabling wake on your keyboard and
   mouse can leave a machine that cannot be woken from sleep except by the
   physical power button. On some hardware this is the normal result of
   "Disable All Devices".
2. **Scheduled work may silently fail.** Disabling wake devices can prevent the
   system from waking for scheduled backups, updates, maintenance windows, media
   recording, or any other timed task.
3. **Remote access may be lost.** Disabling wake on a network adapter breaks
   Wake-on-LAN. If the machine is unattended or remote, you may be unable to
   reach it and may need physical access to recover it.
4. **Actions apply immediately and without confirmation.** By design, this
   application does not ask "are you sure?" and does not show a completion
   dialog. A click on "Disable Wake Device", or on "Disable All Devices", takes
   effect at once. There is no undo button; reversal means selecting the device
   and choosing "Enable Wake Device".
5. **"Disable All Devices" is a bulk operation.** It disables wake on every
   device in the currently loaded list, which may include your keyboard, mouse,
   and network adapter simultaneously. See risks 1 and 3.
6. **Administrator privileges are required.** The application requests elevation
   on launch. Elevated software can affect system stability. Only run builds you
   obtained from a source you trust, and prefer building from source yourself.
7. **Results depend on your hardware, drivers, and Windows version.** `powercfg`
   behavior varies across systems. A device may reappear as armed after a driver
   update, a Windows update, or a BIOS/UEFI change.

## Recovery

If you disable wake on a device and want it back:

1. Launch the application and click **All Wake Devices**.
2. Select the device — devices with wake disabled are shown greyed out and
   struck through.
3. Click **Enable Wake Device** (or double-click the device to toggle it).

If the machine will no longer wake at all, power it on with the physical power
button, then follow the steps above. Wake settings can also be restored outside
this application with `powercfg -deviceenablewake "<device name>"` from an
elevated Command Prompt, or by reinstalling the affected device driver from
Device Manager.

## Your responsibility

You are responsible for determining whether this software is appropriate for
your systems, for testing it outside production first, and for maintaining the
ability to recover a machine that will not wake. Do not deploy it across
machines you do not administer or cannot physically reach.

## Trademarks

Windows, Microsoft, and `powercfg` are trademarks or products of Microsoft
Corporation. This project is not affiliated with, endorsed by, or sponsored by
Microsoft Corporation.
