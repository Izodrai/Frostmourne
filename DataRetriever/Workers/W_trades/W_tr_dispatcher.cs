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
            Log.CyanInfo("(2) -> See opened trades");
            Log.WhiteInfo("(3) -> Close a trade");
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
                case "2":
                    List<Trade> trades = new List<Trade>();
                    err = OpenedTrades(ref Xtb_api_connector, ref configuration, ref MyDB, ref trades, "Opened Trade menu");
                    if (err.IsAnError)
                        return err;
                    break;
                case "3":
                    err = CloseTrade(ref Xtb_api_connector, ref configuration, ref MyDB);
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

            Trade trade = new Trade();

            Log.WhiteInfo("Which CMD choice ? (0 -> buy, 1 -> sell)");

            //trade.Trade_type = Convert.ToInt32(Console.ReadLine());
            trade.Trade_type = 0;

            Log.WhiteInfo("Which volume choice ? (x,y)");

            //trade.Volume = Convert.ToDouble(Console.ReadLine());
            trade.Volume = 0.1;

            trade.Opened_reason = "test_opened";

            err = Commands.Open_trade_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, s_to_check, ref trade);
            if (err.IsAnError)
                return err;

            Log.JumpLine();
            Log.Info("A trade has been opened :");
            Log.Info("Id in db : " + trade.Id);
            Log.Info("Id in xtb : " + trade.Xtb_order_id_2);
            Log.Info("Opened_at : " + trade.Opened_at.ToString("yyyy-MM-dd HH:mm:ss"));
            Log.Info("Opened_price : " + trade.Opened_price.ToString());
            Log.Info("Stop_loss : " + trade.Stop_loss.ToString());
            Log.Info("Volume : " + trade.Volume.ToString());
            Log.Info("Opened_reason : " + trade.Opened_reason);
            if (trade.Trade_type == 0)
                Log.Info("Action : buy");
            else
                Log.Info("Action : sell");
            
            return err;
        }

        public static Error OpenedTrades(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, ref List<Trade> _trades, string msg)
        {
            Error err = new Error();
            
            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();
            
            Log.MagentaInfo(msg);

            err = Commands.Get_open_trades_from_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, ref _trades);
            if (err.IsAnError)
                return err;

            Log.JumpLine();

            if (_trades.Count == 0)
            {
                Log.YellowInfo("no trades to display");
                return new Error(false, "no trades to display");
            }

            Log.YellowInfo("List of opened trades: ");
            Log.WhiteInfo("| Id | Symbol_id (Name) | Xtb_order_id_1 | Xtb_order_id_2 | Opened_at | Opened_value | Stop_loss | Opened_reason | Action | Profit | ");

            foreach (Trade t in _trades)
            {
                string action;
                if (t.Trade_type == 0)
                    action = "buy";
                else
                    action = "sell";

                Log.Info("| " + t.Id + " | " + t.Symbol.Id.ToString() + " ( " + t.Symbol.Name + ") | " + t.Xtb_order_id_1 + " | " + t.Xtb_order_id_2 + " | " + t.Opened_at + " | " + t.Opened_price + " | " + t.Stop_loss + " | " + t.Opened_reason + " | " + action + " | " + t.Profit + " |");
            }

            return err;
        }

        public static Error CloseTrade(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();
            List<Trade> trades = new List<Trade>();

            err = OpenedTrades(ref Xtb_api_connector, ref configuration, ref MyDB, ref trades, "Close Trade Menu");
            if (err.IsAnError)
                return err;
            
            Log.Info("| 0 | Exit | Abort |");
            Log.JumpLine();

            Log.WhiteInfo("On which trade do you want close ? (Write the db ID)");

            string choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Close Trade aborted");
                return new Error(false, "");
            }

            Trade trade_to_close = new Trade();

            foreach (Trade t in trades)
            {
                if (t.Id.ToString() == choice)
                {
                    trade_to_close = t;
                    break;
                }
            }

            if (trade_to_close.Id == 0)
            {
                Log.Error("This ID doesn't exist : " + choice);
                return new Error(false, "");
            }

            trade_to_close.Closed_reason = "test_close";

            err = Commands.Close_trade_xtb(ref Xtb_api_connector, ref configuration, ref MyDB, ref trade_to_close);
            if (err.IsAnError)
                return err;

            Log.JumpLine();
            Log.Info("A trade has been closed :");
            Log.Info("Id in db : " + trade_to_close.Id);
            Log.Info("Id in xtb 1: " + trade_to_close.Xtb_order_id_1);
            Log.Info("Id in xtb 2: " + trade_to_close.Xtb_order_id_2);
            Log.Info("Stop_loss : " + trade_to_close.Stop_loss.ToString());
            Log.Info("Volume : " + trade_to_close.Volume.ToString());
            if (trade_to_close.Trade_type == 0)
                Log.Info("Action : buy");
            else
                Log.Info("Action : sell");
            Log.JumpLine();
            Log.Info("Profit : " + trade_to_close.Profit.ToString());
            Log.JumpLine();
            Log.Info("Opened_at : " + trade_to_close.Opened_at.ToString("yyyy-MM-dd HH:mm:ss"));
            Log.Info("Opened_price : " + trade_to_close.Opened_price.ToString());
            Log.Info("Opened_reason : " + trade_to_close.Opened_reason);
            Log.JumpLine();
            Log.Info("Closed_at : " + trade_to_close.Closed_at.ToString("yyyy-MM-dd HH:mm:ss"));
            Log.Info("Closed_price : " + trade_to_close.Closed_price.ToString());
            Log.Info("Closed_reason : " + trade_to_close.Closed_reason);
            
            return err;
        }
    }
}
