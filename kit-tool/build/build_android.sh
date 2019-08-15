#!/bin/bash
ProjName=SuperTobyRun
root=`pwd`
v_time=`date "+%Y%m%d%H%M%S"`
logFileroot=~/${ProjName}_Android/$v_time/
ABLog=${logFileroot}BuildAssetBundles.log
PreBuildLog=${logFileroot}PreBuild.log
BuildLog=${logFileroot}Build.log
APK_FILE=${ProjName}

mkdir -p $logFileroot
mkdir ~/APKFile
echo "Remove ${ProjName}"
rm -rf ../Build/*.apk
date
. $1
echo "Start Build Android"
TMP=$root/..
APK_FILE=${APK_FILE}${Suffix}
echo Start Build ${APK_FILE} -bundle${bundle} -verNum${versionName} -verCode${versionCode}
/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -buildTarget android -projectPath $TMP -executeMethod CommandBuild.PreBuild $debugParam -android -quit -logFile $PreBuildLog -name${APK_FILE} -pkg$package -bundle${bundle} -verNum${versionName} -verCode${versionCode}
/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -buildTarget android -projectPath $TMP -executeMethod CommandBuild.Build $debugParam -android -quit -logFile $BuildLog -name${APK_FILE} -pkg$package -bundle${bundle} -verNum${versionName} -verCode${versionCode}

cd $root/../Build
IPA=`find . -name "*${Suffix}"`
echo IPA=$IPA
if [ -f "$IPA" ]; then
	echo "$build_type $server_name 版本制作成功"
    rm ~/${APK_FILE}
    cp $IPA ~/${APK_FILE}
    cp $IPA ${logFileroot}
	echo "$build_type $server_name 版本开始上传"
	fir p $IPA
else
	echo "$build_type $server_name 版本制作失败"
fi
git checkout "${root}/../"
echo "$(date) End Build"
open $logFileroot
