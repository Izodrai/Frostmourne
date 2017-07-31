using Frostmourne_basics;
using System.Threading;

namespace DataRetriever.Workers
{
    class Dispatcher
    {
        public static Error Dispatch_choice(string _choice)
        {
            Log.Info("You choose : ("+ _choice+")");

            switch (_choice)
            {
                case "1":
                    //CaseOne();
                    break;
                case "2":
                    //CaseOne();
                    break;
                case "3":
                    //CaseOne();
                    break;
                case "4":
                    //CaseOne();
                    break;
                default:
                    Log.Info("Is not a valid choice...");
                    Thread.Sleep(1000);
                    break;
            }

            return new Error(false, "");
        }
    }
}
