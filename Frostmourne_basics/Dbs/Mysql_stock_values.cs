using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        public Error Insert_or_update_bid_values(List<Bid> _bids)
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

                return new Error(false, "bids inserted or updated");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }
    }
}
