#!/bin/bash

if [ $# -lt 4 ]
then
    echo "usage: $0 {root path} {proj path} {export list} {iap export path}"
    exit 1
fi
echo "Start Build Xcode"
ROOT_PATH=$1
PROJ_PATH=$2
EXPORT_LIST=$3
IAP_EXPORT_PATH=$4


ARCHIVE_NAME=ipa
IPA_NAME=IAPFile
cd $PROJ_PATH
xcodebuild archive -scheme "Unity-iPhone" -configuration "Release" -archivePath $PROJ_PATH//$ARCHIVE_NAME.xcarchive
xcodebuild -exportArchive -archivePath $PROJ_PATH/$ARCHIVE_NAME.xcarchive -exportPath $PROJ_PATH/$IPA_NAME -exportOptionsPlist $EXPORT_LIST
