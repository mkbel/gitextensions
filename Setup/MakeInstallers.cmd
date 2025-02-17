@echo off

rem
rem Update this version number with every release
rem
setlocal
set version=2.51.05
set numericVersion=2.51.05
if not "%APPVEYOR_BUILD_VERSION%"=="" (
    set version=%APPVEYOR_BUILD_VERSION%
    set numericVersion=%APPVEYOR_BUILD_VERSION%
)

SET Configuration=%1
IF "%Configuration%"=="" SET Configuration=Release

SET BuildType=%2
IF "%BuildType%"=="" SET BuildType=Rebuild

set normal=GitExtensions-%Version%.msi

set msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
set output=bin\Release\GitExtensions.msi
set project=Setup.wixproj

set build=%msbuild% %project% /t:%BuildType% /p:Version=%version% /p:NumericVersion=%numericVersion% /p:Configuration=%Configuration% /nologo /v:m

echo Creating installers for Git Extensions %version%
echo.

echo Removing %normal%
del %normal% 2> nul

echo.

echo Building %normal%
%build%
IF ERRORLEVEL 1 EXIT /B 1
copy %output% %normal%
IF ERRORLEVEL 1 EXIT /B 1
