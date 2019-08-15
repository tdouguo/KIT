del info.txt
aapt dump badging "%1" >>info.txt
echo off
For /f "tokens=1* delims=:" %%i in ('Type info.txt^|Findstr /n ".*"') do (
If "%%i"=="1" Echo %%j
)
pause