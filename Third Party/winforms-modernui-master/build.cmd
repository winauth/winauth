@echo off

set config=Release
set outputdir=%cwd%\build
set cwd=%CD%
set commonflags=/p:Configuration=%config% /p:CLSCompliant=False 

set fdir=%WINDIR%\Microsoft.NET\Framework
set msbuild=%fdir%\v4.0.30319\msbuild.exe

:build
echo ---------------------------------------------------------------------
echo Building AnyCpu release...
%msbuild% MetroFramework\MetroFramework.csproj %commonflags% /tv:2.0 /p:TargetFrameworkVersion=v2.0 /p:Platform="Any Cpu" /p:OutputPath="%outputdir%\AnyCpu\NET20"
if errorlevel 1 goto build-error
%msbuild% MetroFramework\MetroFramework.csproj %commonflags% /tv:3.5 /p:TargetFrameworkVersion=v3.5 /p:Platform="Any Cpu" /p:OutputPath="%outputdir%\AnyCpu\NET35"
if errorlevel 1 goto build-error
%msbuild% MetroFramework\MetroFramework.csproj %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.0 /p:Platform="Any Cpu" /p:OutputPath="%outputdir%\AnyCpu\NET40"
if errorlevel 1 goto build-error
%msbuild% MetroFramework\MetroFramework.csproj %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.5 /p:Platform="Any Cpu" /p:OutputPath="%outputdir%\AnyCpu\NET45"
if errorlevel 1 goto build-error

:build-error
echo Failed to compile...

:done
echo.
echo ---------------------------------------------------------------------
echo Compile finished....
cd %cwd%
goto exit

:exit