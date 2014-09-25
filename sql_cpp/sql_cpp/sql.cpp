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

// ����ת�ַ�������
string num2str(double i)
{
	stringstream ss;
	ss << i;
	return ss.str();
}

// �����ݿ�Ҫ����ı��л�ó��Ȳ������ֶε��趨����
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

// ����޸ĳ��ȵ�SQL���
string getAlter(string table, string key, int len)
{
	string str;
	return "alter table " + table + " modify " + key + " VARCHAR(" + num2str(len) + ");";
}

// ÿ��ִ��һ��SQL�ĺ�������
void Connect(string sql)
{
	// ���ݿ�״ֵ̬
	int res;
	// ���ݿ��ʼ��
	mysql_init(&mysql);
	
	// ����MySQL���ݿ�
	if (mysql_real_connect(&mysql, "localhost", "root", "1234", Database.c_str(), 3306, 0, 0))
	{
		// ִ��һ��sql�����״ֵ̬
		res = mysql_query(&mysql, sql.c_str());
		// ��ô�����Ϣ������ת��Ϊ�ַ���
		const char* s = mysql_error(&mysql);
		string error = string(s);
		// ���ִ�в��ɹ���һֱѭ��
		while (1 == res)
		{
			// ��ñ�����ֶ���
			regex sKey("\'(.*)\'");
			smatch rKey;
			regex_search(error, rKey, sKey);
			Key = rKey[1];

			// ��ñ�������ݿ���ı���
			regex sTable("update\\s`(.*)`\\sset");
			smatch rTable;
			regex_search(sql, rTable, sTable);
			Table = rTable[1];

			// ����ֶγ���
			int len = getLen(Table, Key);
			
			// �����޸��ֶγ��ȵ�SQL��ִ��
			sqlAlt = getAlter(Table, Key, len);
			res = mysql_query(&mysql, sqlAlt.c_str());

			// ��ִ��һ��ԭSQL
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
		// �������з�ʱ˵��һ��SQL�Ѿ���ȡ��
		if (ch == '\n' || ch == '\r')
		{
			// ִ��һ��SQL�����sql
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

