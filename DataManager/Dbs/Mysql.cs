using System;
using MySql.Data.MySqlClient;
using DataManager.Errors;

namespace DataManager.Dbs
{
    class Mysql
    {
        protected MySqlConnection MysqlConnector { get; set; }

        protected string Server { get; set; }

        protected string Database { get; set; }

        protected string Login { get; set; }

        protected string Pwd { get; set; }

        public Mysql(string _server, string _database, string _login, string _password)
        {
            this.Server = _server;
            this.Database = _database;
            this.Login = _login;
            this.Pwd = _password;

            Console.WriteLine("server=" + this.Server + ";database = " + this.Database + "; user id = " + this.Login + "; password = " + this.Pwd);
        }
        /// <summary>
        /// Init la connexion à la base de données mysql
        /// </summary>
        /// <returns></returns>
        public Error Connect()
        {
            try
            {
                this.MysqlConnector = new MySqlConnection("server=" + this.Server + ";database = " + this.Database + "; user id = " + this.Login + "; password = " + this.Pwd);
                this.MysqlConnector.Open();

                return new Error(false, "Mysql Database connected");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }
    }
}
