using DataRetriever.Errors;
using DataRetriever.Logs;
using DataRetriever.Jobs.XtbConnector;
using System;

namespace DataRetriever
{
    class Program
    {        

        static void Main(string[] args)
        {        
            Error err = new Error();
            XtbConnector conn = new XtbConnector();

            Log.MagentaInfo("The system started at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            //////////////////////////////////////////////////
            // Connexion aux serveurs xtb
            //////////////////////////////////////////////////
            err = conn.ConnectAndCheck();
            if (err.IsAnError)
            {
                Log.Error(err.MessageError);
                return;
            }
            Log.GreenInfo(err.MessageError);
            Log.Info("");

            Log.Info("XTB server time : " + conn.GetServerTime().ToString("yyyy-MM-dd HH:mm:ss"));

            //////////////////////////////////////////////////
            // Opération de récupération des données
            //////////////////////////////////////////////////
            Log.Info("Running data retrieving...");
            
            err = conn.LoopDataRetrieveAndCalculation();
            if (err.IsAnError)
            {
                Log.Error(err.MessageError);
                return;
            }

            Log.Warning(err.MessageError);
        }
        
    }
}
