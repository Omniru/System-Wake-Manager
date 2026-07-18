@echo off
REM ---------------------------------------------------------------------------
REM  Builds System Wake Manager in Release and copies the result to the repo
REM  root as System.Wake.Manager.exe (the distributed artifact).
REM
REM  Just double-click this file, or run it from a command prompt.
REM
REM  The actual compile lives in build.ps1 so there is only one copy of the
REM  build logic. Administrator rights are NOT needed to build - only to run
REM  the finished application.
REM ---------------------------------------------------------------------------

setlocal

set "REPO_ROOT=%~dp0"
set "BUILD_SCRIPT=%REPO_ROOT%build.ps1"
set "BUILT_EXE=%REPO_ROOT%systemWakeManager\bin\Release\systemWakeManager.exe"
set "PUBLISHED_EXE=%REPO_ROOT%System.Wake.Manager.exe"

if not exist "%BUILD_SCRIPT%" (
    echo ERROR: build.ps1 not found next to this batch file.
    goto :failed
)

echo Building Release...
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%BUILD_SCRIPT%" -Configuration Release
if errorlevel 1 (
    echo ERROR: Build failed.
    goto :failed
)

if not exist "%BUILT_EXE%" (
    echo ERROR: Build reported success but %BUILT_EXE% is missing.
    goto :failed
)

echo Publishing to "%PUBLISHED_EXE%"...
copy /y "%BUILT_EXE%" "%PUBLISHED_EXE%" >nul
if errorlevel 1 (
    echo ERROR: Could not copy the built exe to the repository root.
    echo        Close System.Wake.Manager.exe if it is currently running.
    goto :failed
)

echo.
echo Done. Published: %PUBLISHED_EXE%
echo.
echo Note: the application requires administrator rights at run time,
echo       because powercfg needs elevation to change wake settings.
endlocal
exit /b 0

:failed
echo.
endlocal
exit /b 1
