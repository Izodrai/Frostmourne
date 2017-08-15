using DataRetriever.Workers.W_sym_status;
using Frostmourne_basics;
using Frostmourne_basics.Commands;
using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Threading;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace DataRetriever.Workers.W_trades
{
    class Trades
    {
        public static void Display_choice()
        {
            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();
            Log.MagentaInfo("Trade Menu ");
            Log.YellowInfo("What do you want to do ?");
            Log.WhiteInfo("(1) -> Open a trade");
            Log.WhiteInfo("(0) -> Return To Main Menu");
        }
        public static Error Dispatch_choice(string _choice, ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Log.Info("You chose : (" + _choice + ")");

            Error err = new Error();

            switch (_choice)
            {
                case "1":
                    err = OpenTrade(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                default:
                    Log.JumpLine();
                    Log.Error("Is not a valid choice...");
                    Thread.Sleep(500);
                    break;
            }

            return new Error(false, "");
        }

        public static Error OpenTrade(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();

            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();

            List<Symbol> symbol_list = new List<Symbol>();

            Log.MagentaInfo("Open Trade Menu");
            err = Commands.Load_all_symbols_status(ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Symbol_status.Display_symbols("all status", ref symbol_list);
            Log.Info("| 0 | Exit | Abort |");
            Log.JumpLine();

            Log.WhiteInfo("On which symbol do you want open a trade ? (Write the ID)");

            //string choice = Console.ReadLine();
            string choice = "41";
            if (choice == "0")
            {
                Log.WhiteInfo("Open Trade aborted");
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

            Log.WhiteInfo("Which CMD choice ? (0 -> buy, 1 -> sell)");

            //int cmd = Convert.ToInt32(Console.ReadLine());
            int cmd = 0;

            Log.WhiteInfo("Which volume choice ? (x,y)");

            //double volume = Convert.ToDouble(Console.ReadLine());
            double volume = 0.1;

            err = Commands.Open_trade_xtb(Xtb_api_connector, ref configuration, ref MyDB, s_to_check, cmd, volume);
            if (err.IsAnError)
                return err;
            
            return err;
        }
    }
}
