using Frostmourne_basics;
using Frostmourne_basics.Dbs;
using Frostmourne_basics.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using xAPI.Sync;

namespace DataRetriever.Workers.W_sym_status
{
    public partial class Symbol_status
    {
        public static Error Update_symbol_status(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err = new Error();

            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();

            List<Symbol> symbol_list = new List<Symbol>();

            Log.MagentaInfo("Update Symbol Status Menu");
            err = Commands.Load_all_symbols_status(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbol_list);
            if (err.IsAnError)
                return err;

            Display_symbols("all status", ref symbol_list);
            Log.Info("| 0 | Exit | Abort |");
            Log.JumpLine();

            Log.WhiteInfo("Which status symbol do you want update ? (Write the ID)");
            
            string choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Update aborted");
                return new Error(false, "");
            }
            
            Symbol s_to_update = new Symbol();

            foreach (Symbol s in symbol_list)
            {
                if (s.Id.ToString() == choice)
                {
                    s_to_update = s;
                    break;
                }
            }

            if (s_to_update.Id == 0)
            {
                Log.Error("This ID doesn't exist : " + choice);
                return new Error(false, "");
            }

            Log.WhiteInfo("Which status  do you want apply ? (1 -> inactive, 2 -> standby, 3-> simulation, 4-> active, 0 -> abort)");

            choice = "";
            choice = Console.ReadLine();
            if (choice == "0")
            {
                Log.WhiteInfo("Update aborted");
                return new Error(false, "");
            }

            string new_status;

            switch (choice)
            {
                case "1":
                    new_status = "inactive";
                    break;
                case "2":
                    new_status = "standby";
                    break;
                case "3":
                    new_status = "simulation";
                    break;
                case "4":
                    new_status = "active";
                    break;
                default:
                    Log.JumpLine();
                    Log.Error("Is not a valid choice...");
                    Thread.Sleep(500);
                    return new Error(false, "");
            }
            
            Log.Info("You will update : " + s_to_update.Name + " with " + s_to_update.State + " status to " + new_status + " status are you sure ? (y/n)");

            choice = "";
            choice = Console.ReadLine();
            if (choice != "y")
            {
                Log.WhiteInfo("Update aborted");
                return new Error(false, "");
            }

            s_to_update.State = new_status;

            err = Commands.Update_symbol_status(ref Xtb_api_connector, ref configuration, ref MyDB, s_to_update);
            if (err.IsAnError)
                return err;

            Symbol s_updated = new Symbol();
            s_updated.Id = s_to_update.Id;
            err = Commands.Load_symbol_status(ref Xtb_api_connector, ref configuration, ref MyDB, ref s_updated);
            if (err.IsAnError)
                return err;
            
            if (s_updated.State != s_to_update.State)
                return new Error(true, "Error during state update");
            
            Log.JumpLine();
            Log.GreenInfo("Symbol status updated !");
            Log.JumpLine();
            Log.Info("| " + s_updated.Id + " | " + s_updated.Name + " | " + s_updated.State + " |");
            Log.JumpLine();
            Log.JumpLine();
            
            return new Error(false, "");
        }
    }
}
