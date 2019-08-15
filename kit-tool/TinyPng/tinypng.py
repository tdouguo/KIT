##coding:utf-8
#/usr/bin/python

import os
import sys
import os.path
import shutil
import tinify

# https://blog.csdn.net/langzxz/article/details/77895178 文章
temPath = os.getcwd()+'/'+'tmp'						# 临时目录,注意该脚本最后是要删掉这个临时目录的
tinify.key = "3RVcNT4ZBNUGlXeDxtWeRGVTCaPinTA0"		# 刚刚申请的API KEY
version = "0.0.1"				# 版本

# 压缩的核心
def compress_core(inputFile, outputFile, img_width):
	source = tinify.from_file(inputFile)
	if img_width is not -1:
		resized = source.resize(method = "scale", width  = img_width)
		resized.to_file(outputFile)
	else:
		source.to_file(outputFile)

# 压缩一个文件夹下的图片
def compress_path(path, width):
    print "compress_path-------------------------------------"
    fromFilePath = path 			# 源路径
    print "fromFilePath=%s" %fromFilePath

    for root, dirs, files in os.walk(fromFilePath):
        print "root = %s" %root
        print "dirs = %s" %dirs
        print "files= %s" %files
        for name in files:
            fileName, fileSuffix = os.path.splitext(name)
            if fileSuffix == '.png' or fileSuffix == '.jpg' or fileSuffix == '.jpeg':
                fromfile =  os.path.join(root,name)
                tofile = os.path.join(temPath,name)
                print fromfile
                print  tofile
                compress_core(fromfile, tofile, width)
                shutil.copy2(tofile, fromfile)# 将压缩后的文件覆盖原文件



if __name__ == "__main__":
    if not os.path.exists(temPath):
        os.mkdir(temPath)
    if len(sys.argv)==2:
        compress_path(sys.argv[1],-1)
    if len(sys.argv)==3:
        compress_path(sys.argv[1],sys.argv[2])
    shutil.rmtree(temPath)