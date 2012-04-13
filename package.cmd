@echo off
setlocal

set MSBUILD_EXE=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild
set NUGET_EXE=src\.nuget\nuget.exe
set NUSPEC=src\.nuget\crossroads-net.nuspec
set LIBXS_NUSPEC=src\.nuget\libxs.nuspec
set VERSION_INFO_CS=src\Shared\VersionInfo.cs
set LIBXSVER_TXT=src\libxs-version.txt

:version
set /p VERSION=Enter version (e.g. 1.0): 
set /p BUILD=Enter a build (e.g. 11234): 
set /p REVISION=Enter a revision (e.g. 7): 
set /p MATURITY=Enter maturity (e.g. alpha1, rc1, or blank for Release): 
set /p LIBXSVER=Enter libxs version (e.g. 1.0.0): 
set /p LIBXSCOMMIT=Enter libxs commit (e.g. 21571cf, blank if N/A): 

if not defined MATURITY (
  set MATURITY=Release
  set PRERELEASE=
) else (
  set PRERELEASE=-%MATURITY%
)

:: Shared version info
move %VERSION_INFO_CS% %VERSION_INFO_CS%.bak
echo using System.Reflection; > %VERSION_INFO_CS%
echo. >> %VERSION_INFO_CS%
echo [assembly: AssemblyVersion("%VERSION%.0.0")] >> %VERSION_INFO_CS%
echo [assembly: AssemblyFileVersion("%VERSION%.%BUILD%.%REVISION%")] >> %VERSION_INFO_CS%
echo [assembly: AssemblyInformationalVersion("%VERSION%.%BUILD%.%REVISION% %MATURITY%")] >> %VERSION_INFO_CS%
echo [assembly: AssemblyConfiguration("%MATURITY%")] >> %VERSION_INFO_CS%

:: libxs version info
echo %LIBXSVER% > %LIBXSVER_TXT%
if defined LIBXSCOMMIT (echo Git: %LIBXSCOMMIT% >> %LIBXSVER_TXT%)

%MSBUILD_EXE% src\build.proj /target:Package /Property:Configuration=Release /Property:SignAssembly=true

%NUGET_EXE% Pack %NUSPEC% -Version %VERSION%.%REVISION%%PRERELEASE% -OutputDirectory publish -BasePath .
%NUGET_EXE% Pack %LIBXS_NUSPEC% -Version %LIBXSVER% -OutputDirectory publish -BasePath .

copy LICENSE publish

:: Clean up
move %VERSION_INFO_CS%.bak %VERSION_INFO_CS%
del %LIBXSVER_TXT%

endlocal
if %ERRORLEVEL% GTR 0 pause else exit
