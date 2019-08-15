. $1
root=`pwd`
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
        say "$build_type $server_name 版本制作失败"
fi

