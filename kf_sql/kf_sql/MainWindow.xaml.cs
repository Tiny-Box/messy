using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;

namespace kf_sql
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 打开Excel
        public void OpenExcel(object obj, RoutedEventArgs e)
        {
            conExcel("a");
            //Stream myStream = null;
            //Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "Excel files (*.xls, *xlsx)|*.xls; *xlsx";
            //openFileDialog1.FilterIndex = 1;
            //openFileDialog1.RestoreDirectory = true;

            //bool? userClickedOK = openFileDialog1.ShowDialog();

            //if (userClickedOK == true)
            //{
            //    try
            //    {
            //        if ((myStream = openFileDialog1.OpenFile()) != null)
            //        {
            //            using (myStream)
            //            {
            //                //string ConnectionStr = "";
            //                ////ConnectionStr = this.judgeVersion(openFileDialog1.FileName);

            //                ////MessageBox.Show(ConnectionStr);
            //                ////this.conExcel(ConnectionStr);
            //                ////MessageBox.Show(PubCom.Ylabel);
            //                //MessageBox.Show(openFileDialog1.FileName);
            //                conExcel(openFileDialog1.FileName);

            //            }

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            //    }
            //}
        }
        #endregion

        #region 给Excel连接字符串
        private string conExcel_str(string filename)
        {
            return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";" +
                                "Extended Properties='Excel 12.0 Xml;HDR=NO" + ";'";
        }
        #endregion

        // 生成连接SQL
        public string ConnectionStr(string Database, string DataSource, string User, string Password)
        {
            return "Database='" + Database + "';Data Source='" + DataSource + "';User Id='" + User + "';Password='" + Password + "';charset='utf8';pooling=true";
        }

        public void conExcel(string filename)
        {
           

            DataSet dtTemp = new DataSet();

            // 利用com组件打开Excel
            object oMissing = System.Reflection.Missing.Value;
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
            Excel.Worksheet ws = (Excel.Worksheet)xlWorkbook.Worksheets[1];

            Excel.Range rRng = xlApp.ActiveCell;

            // 获取当前表中实际数据行数
            int num = ws.UsedRange.CurrentRegion.Rows.Count;

            //服务器数量

            string cell = "A" + num.ToString();
            rRng = ws.get_Range(cell, cell);
            double Num = rRng.Value;


            //最低级别

            cell = "E" + num.ToString();
            rRng = ws.get_Range("E2", cell);
            object[,] rank = rRng.Value;

            //开服时间
            cell = "G" + num.ToString();
            rRng = ws.get_Range("G2", cell);
            object[,] time = rRng.Value;
            
            // 关闭xlApp
            xlApp.Visible = false;
            xlApp.Quit();


            string conn = "Database='" + Database.Text + "';Data Source='" + Datasource.Text + "';User Id='" + user.Text + "';Password='" + password.Text + "';charset='utf8';pooling=true";

            string drop = "DROP TABLE IF EXISTS `kfwd_rule`;";

            string addTable = "CREATE TABLE `kfwd_rule` (`pk` int(11) NOT NULL AUTO_INCREMENT, `server_end_time` datetime DEFAULT NULL, `server_start_time` datetime DEFAULT NULL, `rule_id` int(11) NOT NULL, `match_limit` int(11) DEFAULT NULL, `level_range_list` varchar(255) DEFAULT NULL, `reward_rule_group_type` varchar(255) DEFAULT NULL, `round_god_list` varchar(255) DEFAULT NULL, `level_range_type` int(11) NOT NULL, PRIMARY KEY(`pk`), KEY `idx1` (`rule_id`, `server_start_time`)) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;";

            // 搜索kfwd_rule表与重建
            int b = ExecuteNonQuery(conn, CommandType.Text, drop, null);
            b = ExecuteNonQuery(conn, CommandType.Text, addTable, null);
            
            // 批量导入kfwd_rule的数据
            kfwd_rule_sql(num, rank, time, conn);





            

        }


        public void kfwd_season_info_sql(object obj, RoutedEventArgs e)
        {
            string conn = "Database='"+Database.Text+"';Data Source='"+Datasource.Text+"';User Id='"+user.Text+"';Password='"+password.Text+"';charset='utf8';pooling=true";
            
            string count_sql = "select count(*) from `kfwd_season_info`";
            object a = ExecuteScalar(conn, CommandType.Text, count_sql, null);
            int count = Convert.ToInt32(a);
            count += 1;

            string season_id_sql = "select season_id from `kfwd_season_info` where `pk` = " + count.ToString() + ";";
            a = ExecuteScalar(conn, CommandType.Text, season_id_sql, null);
            int season_id = Convert.ToInt32(a);
            season_id += 1;

            DateTime sign_up_d = DateTime.Parse(sign_up.Text);
            DateTime begin_d = DateTime.Parse(begin.Text);
            DateTime end_d = DateTime.Parse(end.Text);

            DateTime active_time = sign_up_d;
            DateTime battle_time = begin_d;
            DateTime end_time = end_d.AddHours(27);
            DateTime next_day_begion_time = begin_d.AddDays(1);
            DateTime schedule_time = begin_d.AddSeconds(-595);
            DateTime show_battle_time = schedule_time.AddSeconds(5);
            DateTime sign_up_finish_time = begin_d.AddMinutes(-10);
            DateTime sign_up_time = sign_up_d;
            DateTime third_day_begion_time = end_d;

            // 做添加进kfwd_season_info的SQL
            string sql = "insert into `kfwd_season_info` VALUES (" + count.ToString() + ",'"+active_time.ToString()+"','"+battle_time.ToString()+"','"+end_time.ToString()+"',1,'"+next_day_begion_time.ToString()+"',5,540,'"+schedule_time.ToString()+"',"+season_id.ToString()+",'"+show_battle_time.ToString()+"','"+sign_up_finish_time.ToString()+"','"+sign_up_time.ToString()+"',15,,1,1,1,,,,0,0,360,'"+third_day_begion_time.ToString()+"',1);";
            int b = ExecuteNonQuery(conn, CommandType.Text, sql, null);
        }

        public void kfwd_rule_sql(int num, object[,] rank, object[,] time, string conn)
        {
            string[] sql = new string[num];

            sql[0] = "insert into kfwd_rule values('1', '"+ time[1,1].ToString() +"', '2014-02-25 00:00:01', '1', '1', '"+ rank[1,1] +"-999', '1', '13', '1');";
            int b = ExecuteNonQuery(conn, CommandType.Text, sql[0], null); 

            MessageBox.Show(sql[0]);
            for (int i = 2; i < num; i++)
            {
                DateTime endtime = DateTime.Parse(time[i, 1].ToString());
                DateTime starttime = DateTime.Parse(time[i-1, 1].ToString()).AddSeconds(1);
                sql[i] = "insert into kfwd_rule values('"+i.ToString()+"','"+endtime.ToString()+"','"+starttime.ToString()+"','1','1','"+rank[i,1].ToString()+"-999','1','13','1');";
                b = ExecuteNonQuery(conn, CommandType.Text, sql[i], null); 
            }

       
        }

        #region SQL 操作
        // 用于缓存参数的HASH表
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///  给定连接的数据库用假设参数执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {

            MySqlCommand cmd = new MySqlCommand();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        
        /// <summary>
        /// 用现有的数据库连接执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="connection">一个现有的数据库连接</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {

            MySqlCommand cmd = new MySqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///使用现有的SQL事务执行一个sql命令（不返回数据集）
        /// </summary>
        /// <remarks>
        ///举例:
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个现有的事务</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 用执行的数据库连接执行一个返回数据集的sql命令
        /// </summary>
        /// <remarks>
        /// 举例:
        ///  MySqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>包含结果的读取器</returns>
        public static MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            //创建一个MySqlCommand对象
            MySqlCommand cmd = new MySqlCommand();
            //创建一个MySqlConnection对象
            MySqlConnection conn = new MySqlConnection(connectionString);

            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
            //因此commandBehaviour.CloseConnection 就不会执行
            try
            {
                //调用 PrepareCommand 方法，对 MySqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 MySqlCommand  的 ExecuteReader 方法
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //清除参数
                cmd.Parameters.Clear();
                return reader;
            }
            catch
            {
                //关闭连接，抛出异常
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            //创建一个MySqlCommand对象
            MySqlCommand cmd = new MySqlCommand();
            //创建一个MySqlConnection对象
            MySqlConnection conn = new MySqlConnection(connectionString);

            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，

            try
            {
                //调用 PrepareCommand 方法，对 MySqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 MySqlCommand  的 ExecuteReader 方法
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();

                adapter.Fill(ds);
                //清除参数
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        ///例如:
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        ///<param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 用指定的数据库连接执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        /// 例如:
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个存在的数据库连接</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public static object ExecuteScalar(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {

            MySqlCommand cmd = new MySqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 将参数集合添加到缓存
        /// </summary>
        /// <param name="cacheKey">添加到缓存的变量</param>
        /// <param name="commandParameters">一个将要添加到缓存的sql参数集合</param>
        public static void CacheParameters(string cacheKey, params MySqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 找回缓存参数集合
        /// </summary>
        /// <param name="cacheKey">用于找回参数的关键字</param>
        /// <returns>缓存的参数集合</returns>
        public static MySqlParameter[] GetCachedParameters(string cacheKey)
        {
            MySqlParameter[] cachedParms = (MySqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            MySqlParameter[] clonedParms = new MySqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (MySqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 准备执行一个命令
        /// </summary>
        /// <param name="cmd">sql命令</param>
        /// <param name="conn">OleDb连接</param>
        /// <param name="trans">OleDb事务</param>
        /// <param name="cmdType">命令类型例如 存储过程或者文本</param>
        /// <param name="cmdText">命令文本,例如:Select * from Products</param>
        /// <param name="cmdParms">执行命令的参数</param>
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (MySqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion


    }
}
