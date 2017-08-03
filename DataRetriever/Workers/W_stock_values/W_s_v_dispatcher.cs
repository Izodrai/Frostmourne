using Frostmourne_basics;
using Frostmourne_basics.Dbs;
using System.Threading;
using xAPI.Sync;

namespace DataRetriever.Workers.W_stock_values
{
    public partial class Stock_values
    {
        public static void Display_choice()
        {
            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();
            Log.MagentaInfo("Stock Values Menu ");
            Log.YellowInfo("What do you want to do ?");
            Log.WhiteInfo("(1) -> Get from DB - last insert (SV) for a symbol");
            Log.CyanInfo("(2) -> Get from DB - last insert (SV) for each symbol");
            
            Log.WhiteInfo("(3) -> Get from DB - nb of inserts by symbol");
            Log.CyanInfo("(4) -> Get from DB - nb of insert by day for a symbol between two date");
            
            Log.WhiteInfo("(5) -> Get from DB - Stock Values for a symbol between two date");
            Log.CyanInfo("(6) -> Get from XTB and setup DB - Stock Values for a symbol from last insert");

            Log.WhiteInfo("(7) -> Set DB - Update stocks value calculation");


            Log.Info("(0) -> Return To Main Menu");
        }

        public static Error Dispatch_choice(string _choice, ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Log.Info("You chose : (" + _choice + ")");

            Error err = new Error();

            switch (_choice)
            {
                case "1":
                    err = Get_from_db_last_insert_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "2":
                    err = Get_from_db_last_insert_for_each_symbol(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "3":
                    err = Get_from_db_nb_insert_by_symbol(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "4":
                    err = Get_from_db_nb_insert_by_day_between_two_date_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "5":
                    err = Get_from_db_stock_values_between_two_date_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "6":
                    err = Get_from_xtb_stock_values_from_last_insert_for_symbol(ref Xtb_api_connector, ref configuration, ref MyDB);
                    if (err.IsAnError)
                        return err;
                    break;
                case "7":
                    err = Set_db_stock_values_calculations(ref Xtb_api_connector, ref configuration, ref MyDB);
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
