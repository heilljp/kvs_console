// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Reflection.Metadata;
using MySql.Data.MySqlClient;
using log4net;
using log4net.Config;
using System.Reflection;


internal class Program
{

     //private static ILog log = LogManager.GetLogger(typeof(Program));

    private static void Main(string[] args)
    {
        MySqlConnection myConnection;
        string? inputIP = string.Empty;
        string myConnectionString;
        string query = string.Empty;


        //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        var _log4net = log4net.LogManager.GetLogger(typeof(Program));

        if(args.Length > 0 ){

            inputIP = args[0].ToString();
        }

        while(true)
        {
            while(true)
            {
                //Console.WriteLine("Hello, World!");
                Console.WriteLine("Please Input Controller IP : ");

                if(string.IsNullOrWhiteSpace(inputIP)){
                    inputIP = Console.ReadLine();
                }

                if (string.IsNullOrEmpty(inputIP))
                {

                    Console.WriteLine("Please Input Controller IP ==============..");
                    //return;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("database checking....");

                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(300);
                    Console.Write("...");
                }

                Console.Clear();

                //set the correct values for your server, user, password and database name
                myConnectionString = string.Format("server={0};uid=sa;pwd=imtsoft;database=kvs", inputIP);
                //myConnectionString = string.Format("server={0};uid=root;pwd=kimjongpeel76&^;database=kvs", inputIP);

//Console.WriteLine(myConnectionString);

                try
                {
                    query = "SELECT  TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,ORDINAL_POSITION,COLUMN_DEFAULT  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_SCHEMA = 'kvs'  AND TABLE_NAME   = 'KVS_H'  and ORDINAL_POSITION > 21 ";
                    CheckSelectColums(query, "1", "DATA COLUMNS");


                    query = "SELECT  TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,ORDINAL_POSITION,COLUMN_DEFAULT  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_SCHEMA = 'kvs'  AND TABLE_NAME   = 'kvs_log'  ";
                    CheckSelectColums(query, "2", "LOG COLUMNS");


                    query = "select LOG_DATE , QUERY_KEY, QUERY_BODY, INS_DT from kvs_log   order by ID desc limit 5 ; ";
                    SelectLogdata(query, "3", "LOG DATA");
                    

                    query = "select POS_NO, BILL_NO, CALL_NO, POSSVR_TARGET_YN, POSSVR_IP, POSSVR_PORT, COOKFINISH_SEND_DATA, COOKFINISH_SEND_YN from KVS_H     order by ORDER_TM desc limit 5 ; ";
                    SelectCookCompleteSendData(query, "4", "SEND DATA_INFO ");


                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    //MessageBox.Show(ex.Message);
                    Console.WriteLine("=============================================");
                    Console.WriteLine("=== Please Input Collect Controller IP !!!===");
                    Console.WriteLine("=============================================");
                    Console.WriteLine("");
                    Console.WriteLine(ex.ToString());
                }

                inputIP =  string.Empty;
        }
            
        void CheckSelectColums(string sql, string num, string showTxt)
        {

            _log4net.Info("======================");
            _log4net.Info(showTxt);
            _log4net.Info("======================");


            using (myConnection = new MySqlConnection(myConnectionString))
            {
                //open a connection
                myConnection.Open();

                // create a MySQL command and set the SQL statement with parameters
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = sql;

                // execute the command and read the results
                using (var myReader = myCommand.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(myReader);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string? name = row["TABLE_NAME"].ToString();
                            string? id = row["COLUMN_NAME"].ToString();
                            string strMerge = string.Empty;

                            // ...
                            //Console.WriteLine("{0} / {1} ", name, id);
                            strMerge = string.Format("{0} / {1} ", name, id);
                            _log4net.Info(strMerge);
                        }
                    }

                    PrintWriteLine(showTxt, dt.Rows.Count.ToString());
                }
            }
        }


        void SelectCookCompleteSendData(string sql, string num, string showTxt)
        {
            
            _log4net.Info("======================");
            _log4net.Info("SEND DATA_INFO SELECT");
            _log4net.Info("======================");
            //_log4net.Info("\n");

            using (myConnection = new MySqlConnection(myConnectionString))
            {
                //open a connection
                myConnection.Open();

                // create a MySQL command and set the SQL statement with parameters
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = sql;

                // execute the command and read the results
                using (var myReader = myCommand.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(myReader);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string? POS_NO = row["POS_NO"].ToString();
                            string? BILL_NO = row["BILL_NO"].ToString();
                            string? CALL_NO = row["CALL_NO"].ToString();

                            string? POSSVR_TARGET_YN = row["POSSVR_TARGET_YN"].ToString();
                            string? POSSVR_IP = row["POSSVR_IP"].ToString();
                            string? POSSVR_PORT = row["POSSVR_PORT"].ToString();
                            string? COOKFINISH_SEND_DATA = row["COOKFINISH_SEND_DATA"].ToString();
                            string? COOKFINISH_SEND_YN = row["COOKFINISH_SEND_YN"].ToString();

                            string strMerge = string.Empty;
                            strMerge = string.Format("\n POS_NO\t{0}\n BILL_NO\t{1}\n CALL_NO\t{2}\n POSSVR_TARGET_YN\t{3}\n POSSVR_IP\t{4}\n POSSVR_PORT\t{5}\n COOKFINISH_SEND_DATA\t{6}\n COOKFINISH_SEND_YN\t{7}\n "
                                                        , POS_NO, BILL_NO, CALL_NO, POSSVR_TARGET_YN, POSSVR_IP, POSSVR_PORT, COOKFINISH_SEND_DATA, COOKFINISH_SEND_YN );
                            // ...
                            //Console.WriteLine("");
                            //Console.WriteLine(strMerge);
                            //Console.WriteLine("");


                            _log4net.Info(strMerge);
                        }
                    }

                    PrintWriteLine(showTxt, dt.Rows.Count.ToString());
                }
            }
        }

        void SelectLogdata(string sql, string num, string showTxt)
        {
            
            _log4net.Info("===================");
            _log4net.Info("LOG_TABLE SELECT");
            _log4net.Info("===================");
            //_log4net.Info("\n");

            
            using (myConnection = new MySqlConnection(myConnectionString))
            {
                //open a connection
                myConnection.Open();

                // create a MySQL command and set the SQL statement with parameters
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = sql;


                // execute the command and read the results
                using (var myReader = myCommand.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(myReader);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
 
                            string? logdate = row["LOG_DATE"].ToString();
                            string? querykey = row["QUERY_KEY"].ToString();
                            string? querybody = row["QUERY_BODY"].ToString();
                            string? insdt = row["INS_DT"].ToString();
                            string strMerge = string.Empty;
                            strMerge = string.Format("\n LOG_DATE\t{0}\n QUERY_KEY\t{1}\n QUERY_BODY\t{2}\n INS_DT\t\t{3} ", logdate, querykey, querybody, insdt);
                            // ...
                            //Console.WriteLine("");
                            //Console.WriteLine("\n LOG_DATE\t{0}\n QUERY_KEY\t{1}\n QUERY_BODY\t{2}\n INS_DT\t\t{3} ", logdate, querykey, querybody, insdt);
                            //Console.WriteLine("");
                            
                            _log4net.Info(strMerge);
                        }
                    }

                    PrintWriteLine(showTxt, dt.Rows.Count.ToString());
                }
            }
        }


        void PrintWriteLine(string p1, string p2)
        {
            //Console.WriteLine("");
            Console.WriteLine(string.Format("{0}===================  Count {1} ..", p1, p2));
            //Console.WriteLine("");

        }
    }
}