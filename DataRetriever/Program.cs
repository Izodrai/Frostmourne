using System;
using Frostmourne_basics;
using DataRetriever.Workers;
using xAPI.Sync;
using Frostmourne_basics.Dbs;
using System.Threading;

namespace DataRetriever
{
    class Program
    {        
        static void Main(string[] args)
        {
            Error err = new Error();
            Mysql MyDB = new Mysql();
            SyncAPIConnector Xtb_api_connector = null;
            Configuration configuration = new Configuration();
            
            Log.JumpLine();
            Log.MagentaInfo("Frostmourne system started at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            Log.JumpLine();

            err = Data_retriever_configuration.LoadAPIConfigurationSettings(ref configuration);
            if (err.IsAnError)
            {
                Log.Error(err.MessageError);
                return;
            }

            Data_retriever_configuration.PrintConfiguration(ref configuration);
            
            err = Tool.InitAll(ref Xtb_api_connector, ref configuration, ref MyDB);
            if (err.IsAnError)
            {
                Log.Error(err.MessageError);
                return;
            }

            string choice = "";

            while (choice != "0")
            {
                Log.JumpLine();
                Log.JumpLine();
                Log.Info("########################");
                Log.JumpLine();
                Log.JumpLine();

                Log.WhiteInfo("What do you want to do ?");
                Log.Info("(1) -> Check and Manage Symbol Status");
                Log.Info("(2) -> Check and Manage Symbol Values");
                Log.Info("(0) -> Exit");

                choice = Console.ReadLine();
                if (choice == "0")
                {
                    Log.JumpLine();
                    Log.MagentaInfo("Frostmourne system stoped at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    Thread.Sleep(500);
                    continue;
                }

                err = Dispatcher.Dispatch_choice(choice, ref Xtb_api_connector, ref configuration, ref MyDB);
                if (err.IsAnError)
                {
                    Log.Error(err.MessageError);
                    return;
                }
            }
        }
        
    }
}
