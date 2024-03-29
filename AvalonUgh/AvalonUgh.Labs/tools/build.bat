@echo off
set TargetFileName=%2
set ConfigurationName=%3

if %ConfigurationName%==Debug (
  goto :eof
)

:: Dll name
@call :jsc %1 

if '%ERRORLEVEL%' == '-1' (
    echo jsc failed.
    goto :eof
)
:: Namespace name, type name
if %ConfigurationName%==Release (
  @call :mxmlc AvalonUgh/Labs/ActionScript LabsFlash
)

goto :eof

:jsc
pushd ..\bin\%ConfigurationName%

::call c:\util\jsc\bin\jsc.exe %TargetFileName%  -as -js

if %ConfigurationName%==Release (
  call c:\util\jsc\bin\jsc.exe %TargetFileName%  -as
)

if %ConfigurationName%==ReleaseWithJavaScript (
  call c:\util\jsc\bin\jsc.exe %TargetFileName%  -js
)

::call c:\util\jsc\bin\jsc.exe %TargetFileName%  -as 


popd
goto :eof

:mxmlc
@echo off
pushd ..\bin\%ConfigurationName%\web



call :build %1 %2
:: to upload to nonoba and to reference in tests
echo coping to public...
copy "*.swf" "../../../../Public/"

popd
goto :eof

:build
echo - %2
:: http://www.adobe.com/products/flex/sdk/
:: -compiler.verbose-stacktraces 
:: call C:\util\flex2\bin\mxmlc.exe -keep-as3-metadata -incremental=true -output=%2.swf -strict -sp=. %1/%2.as
:: call C:\util\flex\bin\mxmlc.exe -debug -keep-as3-metadata -incremental=true -output=%2.swf -strict -sp=. %1/%2.as
call C:\util\flex_sdk_4.6\bin\mxmlc.exe -static-link-runtime-shared-libraries=true   --target-player=11.1.0 -optimize -keep-as3-metadata -incremental=true -output=%2.swf -strict -sp=. %1/%2.as
goto :eof