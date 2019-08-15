del info.txt
aapt dump badging "%1" >>info.txt
echo off
set package=""
For /f "tokens=1* delims=:" %%i in ('Type info.txt^|Findstr /n ".*"') do (
	If "%%i"=="1" (
		set package=%%j
	)
)
for /f "tokens=1,2,3* delims=:'" %%a in ("%package%") do (
	set packageName=%%c
)

Echo ÕıÔÚĞ¶ÔØ£º%packageName%
adb uninstall %packageName%
pause