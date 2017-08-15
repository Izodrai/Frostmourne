using Frostmourne_basics;
using System.Threading;
using DataRetriever.Workers.W_sym_status;
using DataRetriever.Workers.W_trades;
using DataRetriever.Workers.W_stock_values;
using System;
using Frostmourne_basics.Dbs;
using xAPI.Sync;

namespace DataRetriever.Workers
{
    class Dispatcher
    {
        public static Error Dispatch_choice(string _choice, ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Log.Info("You chose : ("+ _choice+")");

            Error err = new Error();
            string choice = "";

            switch (_choice)
            {
                case "1":
                    while (choice != "0")
                    {
                        Symbol_status.Display_choice();
                        choice = Console.ReadLine();
                        if (choice == "0")
                        {
                            Log.JumpLine();
                            Thread.Sleep(500);
                            continue;
                        }

                        err = Symbol_status.Dispatch_choice(choice, ref Xtb_api_connector, ref configuration, ref MyDB);
                        if (err.IsAnError)
                            return err;
                    }

                    break;
                case "2":
                    while (choice != "0")
                    {
                        Stock_values.Display_choice();
                        choice = Console.ReadLine();
                        if (choice == "0")
                        {
                            Log.JumpLine();
                            Thread.Sleep(500);
                            continue;
                        }

                        err = Stock_values.Dispatch_choice(choice, ref Xtb_api_connector, ref configuration, ref MyDB);
                        if (err.IsAnError)
                            return err;
                    }
                    break;
                case "3":
                    while (choice != "0")
                    {
                        Trades.Display_choice();
                        choice = Console.ReadLine();
                        if (choice == "0")
                        {
                            Log.JumpLine();
                            Thread.Sleep(500);
                            continue;
                        }

                        err = Trades.Dispatch_choice(choice, ref Xtb_api_connector, ref configuration, ref MyDB);
                        if (err.IsAnError)
                            return err;
                    }
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
