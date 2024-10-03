// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Reflection.Metadata;
using MySql.Data.MySqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        MySqlConnection myConnection;
        string? inputIP = string.Empty;
        string myConnectionString;
        string query = string.Empty;
            
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
                myConnectionString = string.Format("server={0};uid=root;pwd=kimjongpeel76&^;database=kvs", inputIP);

                try
                {
                    query = "SELECT  TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,ORDINAL_POSITION,COLUMN_DEFAULT  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_SCHEMA = 'kvs'  AND TABLE_NAME   = 'kvs_h'  and ORDINAL_POSITION > 21 ";
                    CheckSelectColums(query, "1", "DATA COLUMNS");


                    query = "SELECT  TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,ORDINAL_POSITION,COLUMN_DEFAULT  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_SCHEMA = 'kvs'  AND TABLE_NAME   = 'kvs_log'  ";
                    CheckSelectColums(query, "2", "LOG COLUMNS");

                }
                catch //(MySql.Data.MySqlClient.MySqlException ex)
                {
                    //MessageBox.Show(ex.Message);
                    Console.WriteLine("=============================================");
                    Console.WriteLine("=== Please Input Collect Controller IP !!!===");
                    Console.WriteLine("=============================================");
                    Console.WriteLine("");
                }

                inputIP =  string.Empty;
        }
            
        void CheckSelectColums(string sql, string num, string showTxt)
        {
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

                            // ...
                            Console.WriteLine("{0} / {1} ", name, id);
                        }
                    }

                    PrintWriteLine(showTxt, dt.Rows.Count.ToString());
                }
            }
        }

        void PrintWriteLine(string p1, string p2)
        {
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0}=================== Column Count {1} ..", p1, p2));
            Console.WriteLine("");

        }
    }
}