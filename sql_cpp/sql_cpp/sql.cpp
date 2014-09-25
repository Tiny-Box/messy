#include <winsock2.h>
#include <iostream>
#pragma comment(lib , "libmysql.lib")
#include "mysql.h"
#include <string>
#include <regex>
#include <stdio.h>
#include <sstream>
#include <stdlib.h>



using namespace std;

MYSQL mysql;
MYSQL_RES *result;
MYSQL_ROW sql_row;
string Database;
string Table;
string Key;
string sqlAlt;

// 数字转字符串函数
string num2str(double i)
{
	stringstream ss;
	ss << i;
	return ss.str();
}

// 从数据库要插入的表中获得长度不够的字段的设定长度
int getLen(string table, string key)
{
	int length;
	int res;

	string sql_getLen = "select CHARACTER_MAXIMUM_LENGTH from information_schema.columns where table_schema = DATABASE() AND table_name = '"+ table +"' AND COLUMN_NAME = '"+ key +"' ";
	res = mysql_query(&mysql, sql_getLen.c_str());
	result = mysql_store_result(&mysql);
	
	sql_row = mysql_fetch_row(result);
	length = atoi(sql_row[0]);
	
	if (length == 1)
	{
		return length = 20;
	}
	else
	{
		return length *= 5;
	}

}

// 获得修改长度的SQL语句
string getAlter(string table, string key, int len)
{
	string str;
	return "alter table " + table + " modify " + key + " VARCHAR(" + num2str(len) + ");";
}

// 每次执行一条SQL的函数主体
void Connect(string sql)
{
	// 数据库状态值
	int res;
	// 数据库初始化
	mysql_init(&mysql);
	
	// 连接MySQL数据库
	if (mysql_real_connect(&mysql, "localhost", "root", "1234", Database.c_str(), 3306, 0, 0))
	{
		// 执行一次sql，获得状态值
		res = mysql_query(&mysql, sql.c_str());
		// 获得错误信息并将其转换为字符串
		const char* s = mysql_error(&mysql);
		string error = string(s);
		// 如果执行不成功就一直循环
		while (1 == res)
		{
			// 获得报错的字段名
			regex sKey("\'(.*)\'");
			smatch rKey;
			regex_search(error, rKey, sKey);
			Key = rKey[1];

			// 获得报错的数据库里的表名
			regex sTable("update\\s`(.*)`\\sset");
			smatch rTable;
			regex_search(sql, rTable, sTable);
			Table = rTable[1];

			// 获得字段长度
			int len = getLen(Table, Key);
			
			// 生成修改字段长度的SQL并执行
			sqlAlt = getAlter(Table, Key, len);
			res = mysql_query(&mysql, sqlAlt.c_str());

			// 在执行一次原SQL
			res = mysql_query(&mysql, sql.c_str());
		}


	}
	else
	{
		cout << "error" << endl;
		
	}

}
int main()
{
	char ch;
	string sql;
	char fileName[10000];
	//string fileName = "D:\sql_test.sql";
	cin >> fileName;
	cin >> Database;
	//scanf_s("\s", fileName);

	FILE *pFile;
	fopen_s(&pFile, fileName, "r");
	
	while ((ch = getc(pFile)) != EOF)
	{
		// 遇到换行符时说明一条SQL已经读取完
		if (ch == '\n' || ch == '\r')
		{
			// 执行一次SQL并清空sql
			Connect(sql);
			sql = "";
		}
		else
		{
			sql += ch;
		}
	}

	Connect(sql);

	cout << "It's over~" << endl;
	system("pause");
	return 0;
}

