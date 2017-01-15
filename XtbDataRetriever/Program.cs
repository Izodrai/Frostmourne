using NLog;
using System.Threading;
using XtbDataRetriever.Errors;
using XtbDataRetriever.Logs;
using XtbDataRetriever.Jobs.XtbConnector;

namespace XtbDataRetriever
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger log = Log.InitLog();
            Error err = new Error();
            XtbConnector conn = new XtbConnector();

            log.Info("Launch System...");

            //////////////////////////////////////////////////
            // Connexion aux serveurs xtb
            //////////////////////////////////////////////////
            err = conn.ConnectAndCheck(log);
            if (err.IsAnError)
            {
                log.Fatal(err.MessageError);
                return;
            }
            log.Info(err.MessageError);

            log.Info("Server time : " + conn.GetServerTime().ToString("yyyy-MM-dd HH:mm:ss"));

            //////////////////////////////////////////////////
            // Opération de récupération des données
            //////////////////////////////////////////////////
            log.Info("Running data retrieving...");

            log.Info("");

            while (true)
            {
                err = conn.RetrieveSymbolsAndAddOrUpdate();
                if (err.IsAnError)
                {
                    log.Fatal(err.MessageError);
                    return;
                }

                log.Info("");
                log.Info("60s remaining before next update");
                Thread.Sleep(15000);
                log.Info("45s remaining before next update");
                Thread.Sleep(15000);
                log.Info("30s remaining before next update");
                Thread.Sleep(15000);
                log.Info("15s remaining before next update");
                Thread.Sleep(15000);
                log.Info("");
            }
        }
    }
}
