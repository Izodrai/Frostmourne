using Frostmourne_basics;
using Frostmourne_basics.Dbs;
using System.Collections.Generic;
using System.Threading;
using xAPI.Sync;

namespace DataRetriever.Workers.W_sym_status
{
    public partial class Symbol_status
    {
        public static void Display_choice()
        {
            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();
            Log.CyanInfo("Symbol Status Menu ");
            Log.WhiteInfo("What do you want to do ?");
            Log.Info("(1) -> Check All Symbols status");
            Log.Info("(2) -> Check Not Inactive Symbols");
            Log.Info("(3) -> Check Inactive Symbols");
            Log.Info("(4) -> Check Active Symbols");
            Log.Info("(5) -> Check Standby Symbols");
            Log.Info("(6) -> Check Simulation Symbols");
            Log.Info("(7) -> Check Symbol status");
            Log.Info("(8) -> Update Symbol status");
            Log.Info("(0) -> Return To Main Menu");
        }

        public static Error Dispatch_choice(string _choice, ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Log.Info("You chose : (" + _choice + ")");

            Error err = new Error();

            switch (_choice)
            {
                case "1":
                    err = Return_all_symbols_status(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "2":
                    err = Return_not_inactive_symbols(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "3":
                    err = Return_inactive_symbols(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "4":
                    err = Return_active_symbols(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "5":
                    err = Return_standby_symbols(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "6":
                    err = Return_simulation_symbols(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "7":
                    err = Return_symbol_status(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "8":
                    err = Update_symbol_status(ref Xtb_api_connector, ref configuration, ref MyDB);
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
        
    }
}
