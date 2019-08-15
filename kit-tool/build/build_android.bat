set root=%cd%
set APK_FILE=SuperTobyRun.apk
set PACKAGE=com.tiptop.supertobyrun
set bundle=true
set versionName=1.0.1
set versionCode=2
set debugParam=%1
if debugParam=="" (
	set debugParam ="debug"
)
echo debugParam is %debugParam%
echo "Start Build Android"
REM "C:\Program Files\Unity\Editor\Unity" -batchmode -buildTarget android -projectPath "%root%/.." -executeMethod CommandBuild.PreBuild -%debugParam% -quit -logFile .\PreBuild.log -bundle%bundle% -verNum%versionName% -verCode%versionCode%
"C:\Program Files\Unity\Editor\Unity" -batchmode -buildTarget android -projectPath "%root%/.." -executeMethod CommandBuild.Build -%debugParam% -android -quit -logFile .\build_android.log -name%APK_FILE% -pkg%PACKAGE% -bundle%bundle% -verNum%versionName% -verCode%versionCode%
echo "End Build,please see log PreBuild.log and build_android.log"