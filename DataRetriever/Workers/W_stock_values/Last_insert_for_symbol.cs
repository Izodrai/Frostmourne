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
        public static Error Get_from_db_last_insert_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();

            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();

            List<Symbol> symbol_list = new List<Symbol>();

            Log.MagentaInfo("Get Last Stock Value Insert for a Symbol Menu");
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

            Bid bid = new Bid();

            err = Commands.Get_from_db_last_insert_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB, s_to_check, ref bid);
            if (err.IsAnError)
                return err;

            List<Bid> bid_list = new List<Bid>();
            bid_list.Add(bid);

            Display_stock_values(ref bid_list);

            return new Error(false, "");
        }

        public static Error Get_from_db_last_insert_for_each_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();

            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();

            List<Symbol> symbol_list = new List<Symbol>();

            Log.MagentaInfo("Get Last Stock Value Insert for each Symbol Menu");
            err = Commands.Load_all_symbols_status(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            List<Bid> bid_list = new List<Bid>();

            foreach (Symbol s in symbol_list)
            {
                Bid bid = new Bid();

                err = Commands.Get_from_db_last_insert_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB, s, ref bid);
                if (err.IsAnError)
                    return err;
                bid_list.Add(bid);
            }

            Display_stock_values(ref bid_list);

            return new Error(false, "");
        }
    }
}
