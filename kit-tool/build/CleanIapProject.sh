#!/bin/bash

if [ $# -lt 2 ]
then
    echo "usage: $0 {root path} {proj path}"
    exit 1
fi
echo "Start Clean Xcode"
ROOT_PATH=$1
PROJ_PATH=$2

cd $PROJ_PATH
xcodebuild clean
cd $ROOT_PATH