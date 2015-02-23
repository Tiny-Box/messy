import os

from os.path import join, getsize

def getDirSize( dir ):
    size = 0L
    for root, dirs, files in os.walk(dir):

        size += sum( [getsize(join(root, name).decode("gbk2313").encode("utf-8")) for name in files] )
    return size

def main():
    dir = "D:\\"
    # address = "C:/Users/Box/AppData/Local"
    # filesize = getDirSize( address )
    # print ( "there are %0.3f" %(filesize/1024/1024), "Mb in /home/wangzr" )
    for root, dirs, files in os.walk(dir):
    	print [join(root, name) for name in files]

    return 0

if __name__ == '__main__': main()
