# -*- coding:utf-8
import os
import os.path
import re
import argparse


# 获得绝对路径
# _localDir=os.path.dirname(__file__)
# _curpath=os.path.normpath(os.path.join(os.getcwd(),_localDir))
# curpath=_curpath

# 参数模块
parser = argparse.ArgumentParser(description='A script for dealing the crash log')
parser.add_argument('-i', type=str, required=True, metavar='InfoAddress', dest='infoAddress', help='The info file\'s address')
parser.add_argument('-c', type=str, required=True, metavar='CrashAddress', dest='crashAddress', help='The crash file\'s address')
parser.add_argument('-d', type=str, required=True, metavar='DateTime', dest='dateTime', help='The date of log')
args = parser.parse_args()


# 输入要处理的log的日期
#address = raw_input()
# 生成两套路径获得奔溃文件与intro的文件名
crashlist = os.listdir(args.crashAddress)
infolist = os.listdir(args.infoAddress)

# 错误列表哈希表
error = {}

# 返回错误类型
def readInfo(fileName):
	errorKind = "error: \n"
	file = open(args.infoAddress + '\\' + fileName)
	lines = file.readlines()
	for i in range (0, len(lines)):
		if re.findall(r'lua stack begin', lines[i]) != []:
			i += 1
			while re.findall(r'lua stack end', lines[i]) == []:
				errorKind += lines[i]
				i += 1
			break
	file.close()
	return errorKind

# 主程 从两个文件夹中找到对应的文件 然后取出错误类型写入哈希
for i in range	(0, len(crashlist)):
	for j in range (0, len(infolist)):
		if re.findall(r'crash(.*)\.' , crashlist[i]) == re.findall(r'info(.*)', infolist[j]):
			try:
				error[readInfo(infolist[j])][0] += 1
			except Exception, e:
				error[readInfo(infolist[j])] = []
				error[readInfo(infolist[j])].append(1)
				error[readInfo(infolist[j])].append(infolist[j])

# 将结果排序
Error = sorted(error.iteritems(), key = lambda asd:asd[1], reverse = True)

# print Error

# 写入结果
result = open("error_log_statistics"+ args.dateTime +".txt", "w+")
for k,v in Error:
        result.write("Number: " + str(v[0]) +'\n')
        result.write("Example: " + str(v[1]) + '\n')
        result.write(k + '\n')
        result.write('\n\n\n')

result.close()





