#!/bin/bash
security unlock-keychain -p "0405"  ~/Library/Keychains/login.keychain-db
ProjName=SuperTobyRun
root=`pwd`
v_time=`date "+%Y%m%d%H%M%S"`
logFileroot=~/${ProjName}_IPhone/$v_time/
ABLog=${logFileroot}BuildAssetBundles.log
PreBuildLog=${logFileroot}PreBuild.log
BuildLog=${logFileroot}Build.log
mkdir -p $logFileroot
mkdir ~/IAPFile
echo "Remove ${ProjName}"
rm -rf ../proj.ios
date
. $1
echo "Start Build iOS"
TMP=$root/..
echo Start Build ${APK_FILE} -pkg$package -bundle${bundle} -verNum${versionName} -verCode${versionCode}
/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -buildTarget ios -projectPath $TMP -executeMethod CommandBuild.PreBuild $debugParam -ios -quit -logFile $PreBuildLog -name${APK_FILE} -pkg$package -bundle${bundle} -verNum${versionName} -verCode${versionCode}
/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -buildTarget ios -projectPath $TMP -executeMethod CommandBuild.Build $debugParam -ios -quit -logFile $BuildLog -pkg$package

cd $root
echo "$(date) build app"

./update_version.sh  "$root/../proj.ios" "${versionName}" "${versionCode}"
rm -rf $root/../proj.ios/IAPFile
./CreateIapFile.sh $root $root/../proj.ios $root/ExportOptionsADHoc$debugParam.plist ~/Documents/${ProjName}_${server}_$versionName.$versionCode.ipa

cd $root/../proj.ios
IPA=`find . -name "*.ipa"`
echo IPA=$IPA
if [ -f "$IPA" ]; then
	say "$build_type $server_name 版本制作成功"
    rm ~/${ProjName}.ipa
    cp $IPA ~/${ProjName}.ipa
    cp $IPA ${logFileroot}
    say "$build_type $server_name 版本开始上传"
    if [ "${uploadstore}" == "true" ];then
    	$root/UploadToStore.sh $IPA $appid $apppwd
    else
	fir p $IPA
    	#$root/upload_fir.sh $root/$1 ios $IPA "$2"
    fi
else
	cat $BuildLog
	say "$build_type $server_name 版本制作失败"
fi
git checkout "${root}/../"
echo "$(date) End Build"
open $logFileroot
