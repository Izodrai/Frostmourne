﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        public Error Load_bids_values_symbol(List<Bid> _bids, DateTime _from_d, Symbol symbol)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, bid_at, last_bid FROM stock_values WHERE symbol_id = @symbol_id AND bid_at > @from", this.Mysql_connector);
                
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", symbol.Id);
                cmd.Parameters.AddWithValue("from", _from_d.ToString("yyyy-MM-dd HH:mm:ss"));

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        _bids.Add(new Bid(Convert.ToInt32(values[0]), symbol.Id, symbol.Name, DateTime.Parse(Convert.ToString(values[1])), Convert.ToDouble(values[2])));
                    }

                }

                this.Close();
                return new Error(false, "last rows loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }

        public Error Insert_or_update_bids_values(List<Bid> _bids)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO stock_values (`symbol_id`, `bid_at`, `last_bid`) VALUES (@symbol_id, @bid_at, @last_bid) ON DUPLICATE KEY UPDATE `last_bid`= @last_bid", this.Mysql_connector);
                
                cmd.Parameters.Clear();
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@symbol_id", 1);
                cmd.Parameters.AddWithValue("@last_bid", 1);
                cmd.Parameters.AddWithValue("@bid_at", "One");

                foreach (Bid b in _bids)
                {
                    cmd.Parameters["@symbol_id"].Value = b.Symbol_id;
                    cmd.Parameters["@last_bid"].Value = b.Last_bid;
                    cmd.Parameters["@bid_at"].Value = b.Bid_at.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.ExecuteNonQuery();
                }

                this.Close();
                return new Error(false, "bids inserted or updated");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
    }
}
