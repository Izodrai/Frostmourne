using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using XtbDataRetriever.Errors;
using XtbDataRetriever.Jobs.Symbols;
using XtbDataRetriever.Jobs.Bids;

namespace XtbDataRetriever.Dbs
{
    class Mysql
    {
        protected MySqlConnection MysqlConnector { get; set; }

        /// <summary>
        /// Init la connexion à la base de données mysql
        /// </summary>
        /// <returns></returns>
        public Error Connect(string _server, string _database, string _login, string _password)
        {
            try
            {
                this.MysqlConnector = new MySqlConnection("server=" + _server + ";database = " + _database + "; user id = " + _login + "; password = " + _password);
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

        /// <summary>
        /// Récupère la dernière date de bid pour un symbol
        /// </summary>
        /// <param name="_d">Ref sur la date de bid max</param>
        /// <param name="_symbol_id">Id du symbol en base à récupérer</param>
        /// <returns></returns>
        public Error Search_last_insert_for_this_value(ref DateTime _d, int _symbol_id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("", this.MysqlConnector);

                cmd.CommandText = "SELECT MAX(bid_at) FROM stock_values WHERE symbol_id = @symbol_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", _symbol_id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        if (Convert.ToString(values[0]) == "")
                        {
                            _d = DateTime.Now.AddDays(-30);
                            return new Error(false, "no rows for this currency");
                        }

                        _d = DateTime.Parse(Convert.ToString(values[0]));
                    }
                }

                return new Error(false, "last insert loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }

        /// <summary>
        /// Récupère les bid pour un symbol depuis une date
        /// </summary>
        /// <param name="_bids">Ref sur les bids à alimenter</param>
        /// <param name="_from_d">Date from à récupérer</param>
        /// <param name="_symbol_id">Symbol Id à récupérer</param>
        /// <param name="_symbol_name">Symbol Name à récupérer</param>
        /// <returns></returns>
        public Error Load_bid_values_for_one_symbol(ref List<Bid> _bids, DateTime _from_d, int _symbol_id, string _symbol_name)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("", this.MysqlConnector);

                cmd.CommandText = "SELECT id, bid_at, start_bid_value, last_bid_value FROM stock_values WHERE symbol_id = @symbol_id AND bid_at > @from";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", _symbol_id);
                cmd.Parameters.AddWithValue("from", _from_d.ToString("yyyy-MM-dd HH:mm:ss"));

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        _bids.Add(new Bid(Convert.ToInt32(values[0]), _symbol_id, _symbol_name, DateTime.Parse(Convert.ToString(values[1])), Convert.ToDouble(values[2]), Convert.ToDouble(values[3])));
                    }

                }
                return new Error(false, "last rows loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }

        /// <summary>
        /// Update les bid en base par rapport à ce qui a été récupéré chez xtb
        /// </summary>
        /// <param name="bids_in_db_to_update">List des bids à update en base</param>
        /// <returns></returns>
        public Error Update_bid_values(List<Bid> bids_in_db_to_update)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("", this.MysqlConnector);

                cmd.CommandText = "UPDATE stock_values SET start_bid_value = @new_start_bid_value, last_bid_value = @new_last_bid_value, updated_at = CURRENT_TIMESTAMP WHERE id = @bid_id";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@new_start_bid_value", 1);
                cmd.Parameters.AddWithValue("@new_last_bid_value", 1);
                cmd.Parameters.AddWithValue("@bid_id", 1);

                foreach (Bid b in bids_in_db_to_update)
                {
                    cmd.Parameters["@new_start_bid_value"].Value = b.Start_bid_value;
                    cmd.Parameters["@new_last_bid_value"].Value = b.Last_bid_value;
                    cmd.Parameters["@bid_id"].Value = b.Id;
                    cmd.ExecuteNonQuery();
                }
                return new Error(false, "bids updated");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }

        public Error Add_bid_values(List<Bid> bids_to_add)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("", this.MysqlConnector);

                cmd.CommandText = "INSERT INTO stock_values (symbol_id, bid_at, start_bid_value, last_bid_value, json_calculation) VALUES (@symbol_id, @bid_at, @start_bid_value, @last_bid_value, @json)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@symbol_id", 1);
                cmd.Parameters.AddWithValue("@start_bid_value", 1);
                cmd.Parameters.AddWithValue("@last_bid_value", 1);
                cmd.Parameters.AddWithValue("@bid_at", "One");
                cmd.Parameters.AddWithValue("@json", "{}");

                foreach (Bid b in bids_to_add)
                {
                    cmd.Parameters["@symbol_id"].Value = b.Symbol_id;
                    cmd.Parameters["@start_bid_value"].Value = b.Start_bid_value;
                    cmd.Parameters["@last_bid_value"].Value = b.Last_bid_value;
                    cmd.Parameters["@bid_at"].Value = b.Bid_at.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters["@json"].Value = b.GetCalculationString();
                    cmd.ExecuteNonQuery();
                }
                return new Error(false, "bids added");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }
    }
}

