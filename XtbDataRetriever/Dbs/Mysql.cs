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

        /// <summary>
        /// Ajoute les bid que l'on a pas en base par rapport a été récupéré chez xtb
        /// </summary>
        /// <param name="bids_to_add"></param>
        /// <returns></returns>
        public Error Add_bid_values(List<Bid> bids_to_add)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("", this.MysqlConnector);

                cmd.CommandText = "INSERT INTO stock_values (symbol_id, bid_at, start_bid_value, last_bid_value, created_at, updated_at) VALUES (@symbol_id, @bid_at, @start_bid_value, @last_bid_value, @created_at, @updated_at)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@symbol_id", 1);
                cmd.Parameters.AddWithValue("@start_bid_value", 1);
                cmd.Parameters.AddWithValue("@last_bid_value", 1);
                cmd.Parameters.AddWithValue("@bid_at", "One");
                cmd.Parameters.AddWithValue("@created_at", "One");
                cmd.Parameters.AddWithValue("@updated_at", "One");
                
                foreach (Bid b in bids_to_add)
                {
                    cmd.Parameters["@symbol_id"].Value = b.Symbol_id;
                    cmd.Parameters["@start_bid_value"].Value = b.Start_bid_value;
                    cmd.Parameters["@last_bid_value"].Value = b.Last_bid_value;
                    cmd.Parameters["@bid_at"].Value = b.Bid_at.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters["@created_at"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters["@updated_at"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.ExecuteNonQuery();
                }
                return new Error(false, "bids added");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }

        /// <summary>
        /// Load les valeurs des bids sur les deux derniers jours pour les analyser
        /// </summary>
        /// <param name="_bids"></param>
        /// <param name="_symbol_id"></param>
        /// <param name="_symbol_name"></param>
        /// <returns></returns>
        public Error Load_last_2_days_bid_values_for_one_symbol(ref List<Bid> _bids, int _symbol_id, string _symbol_name)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("", this.MysqlConnector);

                cmd.CommandText = "SELECT sv_id, bid_at, start_bid_value, last_bid_value, sma_c, sma_l, ema_c, ema_l, sa_id FROM `v_last_2_days_stock_values` WHERE s_id = @symbol_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", _symbol_id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        int sv_id = Convert.ToInt32(values[0]);
                        DateTime bid_at = DateTime.Parse(Convert.ToString(values[1]));
                        double start_value = Convert.ToDouble(values[2]);
                        double last_value = Convert.ToDouble(values[3]);
                        int sa_id = 0;
                        double sma_c = 0;
                        double sma_l = 0;
                        double ema_c = 0;
                        double ema_l = 0;

                        if (Convert.ToString(values[4]) != "")
                            sma_c = Convert.ToDouble(values[4]);

                        if (Convert.ToString(values[4]) != "")
                            sma_c = Convert.ToDouble(values[4]);

                        if (Convert.ToString(values[5]) != "")
                            sma_l = (Convert.ToDouble(values[5]));
                        
                        if (Convert.ToString(values[4]) != "")
                            ema_c = Convert.ToDouble(values[4]);

                        if (Convert.ToString(values[5]) != "")
                            ema_l = (Convert.ToDouble(values[5]));

                        if (Convert.ToString(values[6]) != "")
                            sa_id = (Convert.ToInt32(values[6]));


                        Bid b = new Bid(sv_id, _symbol_id, _symbol_name, bid_at, start_value, last_value, sma_c, sma_l, ema_c, ema_l, sa_id);

                        _bids.Add(b);
                    }

                }
                return new Error(false, "last rows loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }
    }
}

