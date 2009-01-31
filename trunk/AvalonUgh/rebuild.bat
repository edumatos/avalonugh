@echo off
setlocal
echo - svn update
X:\util\TortoiseSVN\bin\TortoiseProc.exe /command:update /path:"." /notempfile /closeonend:1
echo - hint path fix
c:\util\jsc\bin\HintPathFixer.exe


set msbuild=%SystemRoot%\Microsoft.NET\Framework\v3.5\MSBuild.exe
set flags=/nologo /verbosity:q /p:Configuration=Release

:: rebuild framework

call :build AvalonUgh.Labs.sln



popd
endlocal
goto :eof

:buildsln
call :build %1\%2\%2.sln
goto :eof

:build
echo - %1
call %msbuild% %flags% %1
goto :eof