<#
.SYNOPSIS
    Builds System Wake Manager without requiring a .NET Framework targeting pack.

.DESCRIPTION
    The preferred way to build this project is:

        MSBuild systemWakeManager.sln /t:Rebuild /p:Configuration=Release

    That requires the .NET Framework 4.x Developer Pack (https://aka.ms/msbuild/developerpacks).
    Without it MSBuild fails with MSB3644 ("reference assemblies not found").

    This script is the fallback: it drives csc.exe from the installed .NET Framework
    runtime directory, mirroring the settings in systemWakeManager.csproj
    (WinExe, x86, app.manifest, MainForm.resx). Use it when the developer pack is
    not available; prefer MSBuild when it is.

.PARAMETER Configuration
    Release (default) or Debug.
#>
[CmdletBinding()]
param(
    [ValidateSet('Release', 'Debug')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'

$projectDir = Join-Path $PSScriptRoot 'systemWakeManager'
$outputDir  = Join-Path $projectDir "bin\$Configuration"
$objDir     = Join-Path $projectDir "obj\$Configuration"

$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path $csc)) {
    $csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
}
if (-not (Test-Path $csc)) {
    throw "csc.exe (.NET Framework 4.0) not found. Install the .NET Framework 4.x developer tools."
}

New-Item -ItemType Directory -Force $outputDir | Out-Null
New-Item -ItemType Directory -Force $objDir    | Out-Null

# Compile MainForm.resx to a .resources file. resgen.exe ships with the Windows SDK
# and is often absent, so use the ResX reader from System.Windows.Forms instead.
Add-Type -AssemblyName System.Windows.Forms
$resourcesFile = Join-Path $objDir 'systemWakeManager.MainForm.resources'
$reader = New-Object System.Resources.ResXResourceReader((Join-Path $projectDir 'MainForm.resx'))
$writer = New-Object System.Resources.ResourceWriter($resourcesFile)
try {
    foreach ($entry in $reader) { $writer.AddResource($entry.Key, $entry.Value) }
} finally {
    $reader.Close()
    $writer.Close()
}

$sources = @(
    'MainForm.cs'
    'MainForm.Designer.cs'
    'Program.cs'
    'Properties\AssemblyInfo.cs'
) | ForEach-Object { Join-Path $projectDir $_ }

$references = @(
    'System.dll'
    'System.Core.dll'
    'System.Data.dll'
    'System.Drawing.dll'
    'System.Windows.Forms.dll'
    'System.Xml.dll'
    'Microsoft.CSharp.dll'
) | ForEach-Object { "/r:$_" }

$exePath = Join-Path $outputDir 'systemWakeManager.exe'

# Mirrors systemWakeManager.csproj: WinExe, x86, and the requireAdministrator manifest
# (the app shells out to powercfg, which needs elevation to change wake settings).
$arguments = @(
    '/nologo'
    '/target:winexe'
    '/platform:x86'
    "/win32manifest:$(Join-Path $projectDir 'app.manifest')"
    "/resource:$resourcesFile"
    "/out:$exePath"
)

if ($Configuration -eq 'Release') {
    $arguments += @('/optimize+', '/define:TRACE')
} else {
    $arguments += @('/debug+', '/define:DEBUG;TRACE', '/checked+')
}

& $csc @arguments @references @sources
if ($LASTEXITCODE -ne 0) {
    throw "Compilation failed with exit code $LASTEXITCODE."
}

Copy-Item (Join-Path $projectDir 'app.config') (Join-Path $outputDir 'systemWakeManager.exe.config') -Force

Write-Host "Built $exePath" -ForegroundColor Green
