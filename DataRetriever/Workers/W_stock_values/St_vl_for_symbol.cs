using Frostmourne_basics;
using Frostmourne_basics.Dbs;
using System.Collections.Generic;
using xAPI.Sync;

namespace DataRetriever.Workers.W_stock_values
{
    public partial class Stock_values
    {
        public static Error Get_from_db_st_vl_between_two_date_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {

            return new Error(false, "");
        }
        public static Error Get_from_db_st_vl_from_date_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {

            return new Error(false, "");
        }

        public static void Display_stock_values(ref List<Bid> _bids)
        {
            Log.JumpLine();

            if (_bids.Count == 0)
            {
                Log.YellowInfo("No stock value in db");
                return;
            }
            Log.YellowInfo("List of stock value in db: ");
            Log.WhiteInfo("| Id | Symbol_id (Name) | Bid_at | Last_bid | calculations |");

            foreach (Bid b in _bids)
            {
                Log.Info("| " + b.Id + " | " + b.Symbol.Id.ToString() + " ( " + b.Symbol.Name+ ") | " + b.Bid_at + " | " + b.Last_bid + " | " + b.Calculations + " |");
            }
        }
    }
}
