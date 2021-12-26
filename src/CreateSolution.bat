IF NOT DEFINED PROCESSOR_ARCHITEW6432 (
    set PowerShellExecutable="%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe"
) ELSE (
    set PowerShellExecutable="%SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe"
)

set Directory=%~dp0

:: "CreateSolution.ps1" must be run by a powershell of the current system architecture
%PowerShellExecutable% -ExecutionPolicy Bypass -NoLogo -NonInteractive -NoProfile -File "%Directory%CreateSolution.ps1" %User%

pause