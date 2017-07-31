using Frostmourne_basics;
using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Threading;
using xAPI.Sync;

namespace DataRetriever.Workers.W_sym_status
{
    public partial class Symbol_status
    {
        protected static void Display_symbols(string symbol_type, ref List<Symbol> _sl)
        {
            Log.JumpLine();

            if (_sl.Count == 0)
            {
                Log.YellowInfo("No " + symbol_type + " symbols in db");
                return;
            }
            Log.YellowInfo("List of " + symbol_type + " symbols in db: ");
            Log.WhiteInfo("| Id | Name | Status |");

            foreach (Symbol s in _sl)
            {
                Log.Info("| " + s.Id + " | " + s.Name + " | " + s.State + " |");
            }
        }

        public static Error Return_active_symbols(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_active_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Display_symbols("active", ref symbol_list);

            return err;
        }

        public static Error Return_simulation_symbols(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_simulation_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Display_symbols("simulation", ref symbol_list);

            return err;
        }

        public static Error Return_standby_symbols(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_standby_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Display_symbols("standby", ref symbol_list);

            return err;
        }

        public static Error Return_not_inactive_symbols(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_not_inactive_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Display_symbols("not inactive", ref symbol_list);

            return err;
        }

        public static Error Return_inactive_symbols(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_inactive_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Display_symbols("inactive", ref symbol_list);

            return err;
        }

        public static Error Return_all_symbols_status(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            List<Symbol> symbol_list = new List<Symbol>();

            err = Commands.Load_all_symbols_status(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Display_symbols("all status", ref symbol_list);

            return err;
        }

        public static Error Return_symbol_status(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            Symbol symbol = new Symbol();

            Log.WhiteInfo("Which status symbol do you want select ? (Write the ID)");

            string choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Select aborted");
                return new Error(false, "");
            }

            ///////////////
            // Je ne suis pas obligé de faire ce load de tous les symbols mais c'est pour tester la fonction Load_symbol_status
            //

            List<Symbol> symbols_list = new List<Symbol>();

            err = Commands.Load_all_symbols_status(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbols_list);
            if (err.IsAnError)
                return err;

            foreach (Symbol s in symbols_list)
            {
                if (s.Id.ToString() == choice)
                {
                    symbol.Id = Convert.ToInt32(choice);
                    break;
                }
            }

            if (symbol.Id == 0)
            {
                Log.JumpLine();
                Log.Error("This ID doesn't exist : " + choice);
                return new Error(false, "");
            }

            err = Commands.Load_symbol_status(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol);
            if (err.IsAnError)
                return err;

            List<Symbol> symbol_list = new List<Symbol>();
            symbol_list.Add(symbol);

            Display_symbols("status", ref symbol_list);

            return err;
        }
    }
}
