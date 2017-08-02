using DataRetriever.Workers.W_sym_status;
using Frostmourne_basics;
using Frostmourne_basics.Commands;
using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using xAPI.Sync;

namespace DataRetriever.Workers.W_stock_values
{
    public partial class Stock_values
    {
        public static Error Get_from_db_stock_values_between_two_date_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();

            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();

            List<Symbol> symbol_list = new List<Symbol>();

            Log.MagentaInfo("Get Stock Value Insert for a Symbol Menu");
            err = Commands.Load_all_symbols_status(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Symbol_status.Display_symbols("all status", ref symbol_list);
            Log.Info("| 0 | Exit | Abort |");
            Log.JumpLine();

            Log.WhiteInfo("Which status symbol do you want check ? (Write the ID)");

            string choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            Symbol s_to_check = new Symbol();

            foreach (Symbol s in symbol_list)
            {
                if (s.Id.ToString() == choice)
                {
                    s_to_check = s;
                    break;
                }
            }

            if (s_to_check.Id == 0)
            {
                Log.Error("This ID doesn't exist : " + choice);
                return new Error(false, "");
            }

            Log.JumpLine();
            Log.WhiteInfo("From witch date do you want check ? (Write the date with YYYY-MM-DD)");
            choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            DateTime from = new DateTime();

            try
            {
                from = DateTime.Parse(choice);
            }
            catch (Exception e)
            {
                Log.Error("Bad date format -> " + e);
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            if (from == new DateTime())
            {
                Log.Error("Bad date format");
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            Log.JumpLine();
            Log.WhiteInfo("To witch date do you want check ? (Write the date with YYYY-MM-DD)");
            choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            DateTime to = new DateTime();

            try
            {
                to = DateTime.Parse(choice);
            }
            catch (Exception e)
            {
                Log.Error("Bad date format -> " + e);
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            if (to == new DateTime())
            {
                Log.Error("Bad date format");
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            if (from.Date >= to.Date)
            {
                Log.Error("from.Date >= to.Date");
                Log.WhiteInfo(from.Date + " | " + to.Date);
            }

            Log.JumpLine();
            Log.Info("You will check " + s_to_check.Id.ToString() + " (" + s_to_check.Name + ") inserted between " + from.ToString() + " to " + to.ToString() + " status are you sure ? (y/n)");

            choice = Console.ReadLine();
            if (choice != "y")
            {
                Log.WhiteInfo("Check aborted");
                return new Error(false, "");
            }

            List<Bid> bids = new List<Bid>();

            err = Commands.Get_from_db_stock_values_between_two_date_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB, s_to_check, from, to, ref bids);
            if (err.IsAnError)
                return err;

            Display_stock_values(ref bids);

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
