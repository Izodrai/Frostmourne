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
        public static Error Get_from_xtb_stock_values_from_last_insert_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            
            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();
            
            List<Symbol> symbol_list = new List<Symbol>();

            Log.MagentaInfo("Get from XTB and setup DB Stock Value for a Symbol Menu");
            err = Commands.Load_all_symbols_status(ref configuration, ref MyDB, ref symbol_list);
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

            Log.JumpLine();

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
            
            List<Bid> bids = new List<Bid>();

            err = Commands.Get_from_xtb_stock_values_from_last_insert_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB, s_to_check, ref bids);
            if (err.IsAnError)
                return err;

            Display_stock_values(ref bids);
           
            return new Error(false, "");
        }
        
        public static Error Set_db_stock_values_calculations(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();

            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();

            Log.MagentaInfo("Set db, update stock value calculation Menu");
            Log.JumpLine();

            Bid bid = new Bid();
            Symbol symbol = new Symbol(1, "", "");

            err = Commands.Get_from_db_last_insert_for_symbol(ref configuration, ref MyDB, symbol, ref bid);
            if (err.IsAnError)
                return err;
            
            List<Bid> bids = new List<Bid>();

            err = Commands.Get_from_db_stock_values_between_two_date_for_symbol(ref configuration, ref MyDB, symbol, bid.Bid_at.AddHours(-1), bid.Bid_at, ref bids);
            if (err.IsAnError)
                return err;

            Display_stock_values(ref bids);

            Log.WhiteInfo("Which stock value do you want update ? (Write the ID)");

            string choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Update aborted");
                return new Error(false, "");
            }

            Log.JumpLine();

            Bid bid_to_update = new Bid();

            foreach (Bid b in bids)
            {
                if (b.Id.ToString() == choice)
                {
                    bid_to_update = b;
                    break;
                }
            }

            if (bid_to_update.Id == 0)
            {
                Log.Error("This ID doesn't exist : " + choice);
                return new Error(false, "");
            }

            Log.WhiteInfo("Which calculation do you want update ? (Write the Json valid data)");

            choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Update aborted");
                return new Error(false, "");
            }

            bid_to_update.Calculations = choice;

            Log.JumpLine();

            List<Bid> bids_to_update = new List<Bid>();

            bids_to_update.Add(bid_to_update);

            err = MyDB.Update_bid_calculations(bids_to_update);
            if (err.IsAnError)
                return err;

            List<Bid> bids_to_updated = new List<Bid>();

            err = Commands.Get_from_db_stock_values_between_two_date_for_symbol(ref configuration, ref MyDB, symbol, bid.Bid_at.AddHours(-1), bid.Bid_at, ref bids_to_updated);
            if (err.IsAnError)
                return err;

            Display_stock_values(ref bids_to_updated);
            
            return new Error(false, "");
        }
    }
}
