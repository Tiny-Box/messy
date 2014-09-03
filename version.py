#-*- coding: utf-8
import os
import os.path
import md5
import hashlib
import sys
import zipfile
import shutil

# 设置全局的文件名列表
game = 'gcld/'
update = sys.argv[2]
gamelist = []
updatelist = []

# 将要解压的apk包复制然后改名
def Rename(filepath):
	shutil.copyfile(filepath, "gcld.zip")

# 解压zip包
def zipextract(filename):
	f = zipfile.ZipFile(filename,'r')
	for file in f.namelist():
		f.extract(file, "gcld/")

# 获得版本号
def getversion(filepath):
	f = open(filepath + '/version.lua')
	version = f.readlines(3)
	version = version[1:3]
	return version

# 删除文件夹及内部文件
def delete(src):
    if os.path.isfile(src):
        try:
            os.remove(src)
        except:
            pass
    elif os.path.isdir(src):
        for item in os.listdir(src):
            itemsrc=os.path.join(src,item)
            delete(itemsrc)
        try:
            os.rmdir(src)
        except:
            pass

# 对比两个文件的MD5是否相等
def MD5(path):
	f1 = open(game + 'assets/' + path, 'r')
	f2 = open(update+ '/' + path, 'r')
	return md5.new(f1.read()).hexdigest() == md5.new(f2.read()).hexdigest()

# 统计文件的文件名列表
Add_file = []
Mod_file = []
Del_file = []

# 主程
def main():

	Rename(sys.argv[1])
	zipextract('gcld.zip')

	gamelist = os.listdir('gcld/assets/')
	updatelist = os.listdir(sys.argv[2])

	# 对比原游戏包和动更包里面的文件，并统计
	for i in gamelist:
		try:
			updatelist.index(i)
			if MD5(i) != True:
				Mod_file.append(i)
			updatelist.remove(i)
		except ValueError:
			Del_file.append(i)
	Add_file = updatelist

	# 获得底包和动更包的版本号
	updateversion = getversion(sys.argv[2])
	bottomversion = getversion('gcld/assets')

	# 将统计结果写入文件
	result = open("version.txt", "w+")

	result.write("底包版本： " + '\n')
	for i in bottomversion:
		result.write(i)

	result.write("动更包版本: " + '\n')
	for i in updateversion:
		result.write(i)
	result.write('\n\n\n\n')

	result.write("新增文件: " + str(len(Add_file)) + '\n')
	for i in Add_file:
		result.write(i)
		result.write('\n')
	result.write('\n\n\n\n')

	result.write("更改文件: " + str(len(Mod_file)) + '\n')
	for i in Mod_file:
		result.write(i)
		result.write('\n')
	result.write('\n\n\n\n')

	result.write("删除文件: " + str(len(Del_file)) + '\n')
	for i in Del_file:
		result.write(i)
		result.write('\n')
	result.close()

	# 删除掉改名的zip包与解压的文件
	delete('gcld')
	os.remove('gcld.zip')
		

main()
