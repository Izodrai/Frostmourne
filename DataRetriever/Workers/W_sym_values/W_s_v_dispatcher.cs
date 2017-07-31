using Frostmourne_basics;
using Frostmourne_basics.Dbs;
using xAPI.Sync;

namespace DataRetriever.Workers.W_sym_values
{
    public partial class Symbol_values
    {
        public static void Display_choice()
        {
            Log.JumpLine();
            Log.JumpLine();
            Log.Info("########################");
            Log.JumpLine();
            Log.JumpLine();
            Log.CyanInfo("Symbol Values Menu ");
            Log.WhiteInfo("What do you want to do ?");
            Log.Info("Work In Progress");
            Log.Info("(0) -> Return To Main Menu");
        }

        public static Error Dispatch_choice(string _choice, ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {

            return new Error(false, "");
        }
    }
}
