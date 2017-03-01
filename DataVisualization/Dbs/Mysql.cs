using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataVisualization.Errors;
using DataVisualization.Jobs.Symbols;

namespace DataVisualization.Dbs
{
    public class Mysql
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

        /// <summary>
        /// Retourne tous les symbols actif en base
        /// </summary>
        /// <param name="_ss">Ref List de Symbols qui sera alimenté par ceux actif en base</param>
        /// <returns></returns>
        public Error Load_symbols(ref List<Symbol> _ss)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("", this.MysqlConnector);

                cmd.CommandText = "SELECT id, reference, description FROM symbols WHERE active";
                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _ss.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1]), Convert.ToString(values[2])));
                    }
                }

                return new Error(false, "Symbols loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }
    }
}
