using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using XtbDataRetriever.Dbs;
using XtbDataRetriever.Jobs.Symbols;
using XtbDataRetriever.Jobs.Bids;
using XtbDataRetriever.Errors;
using XtbDataRetriever.Configurations;
using XtbDataRetriever.Tools;
using xAPI.Sync;
using xAPI.Commands;
using xAPI.Responses;
using xAPI.Records;

namespace XtbDataRetriever.Jobs.XtbConnector
{
    class XtbConnector
    {
        protected Logger log { get; set; }

        protected Error err { get; set; }

        protected string Login { get; set; }

        protected string Pwd { get; set; }

        protected Server Server { get; set; }

        protected List<Symbol> Symbols { get; set; }

        protected SyncAPIConnector APIConnector { get; set; }

        protected Credentials Credentials { get; set; }

        protected Mysql MyDBConnector { get; set; }

        ///////////////////////////////////
        // Séparation entre les variables et les fonctions
        ///////////////////////////////////

        /// <summary>
        /// Initialisation du connector xtb
        /// </summary>
        /// <returns></returns>
        public Error FeedConnector()
        {
            string _login = "";
            string _pwd = "";
            string _server = "";
            List<string> _currencies = new List<string>();

            //////////////////////////////////////////////////
            // Load de la configuration pour l'utilisateur
            //////////////////////////////////////////////////

            err = Configuration.LoadConnectorSettings(ref _login, ref _pwd, ref _server);
            if (err.IsAnError)
            {
                return err;
            }

            switch (_server)
            {
                case "demo":
                    this.Server = Servers.DEMO;
                    break;
                case "real":
                    this.Server = Servers.REAL;
                    break;
            }

            this.Login = _login;
            this.Pwd = _pwd;

            return new Error(false, "Connector Configuration downloaded !");
        }

        /// <summary>
        /// Fonction de vérification des symbols en base par rapport à celles disponibles sur le serveur xtb
        /// </summary>
        /// <returns></returns>
        protected Error CheckSymbols()
        {
            this.MyDBConnector = new Mysql();

            if (this.MyDBConnector.Connect().IsAnError)
            {
                return err;
            }

            List<Symbol> ss = new List<Symbol>();

            err = this.MyDBConnector.Load_symbols(ref ss);
            if (err.IsAnError)
            {
                return err;
            }
            this.Symbols = ss;

            /*
            AllSymbolsResponse symbols = APICommandFactory.ExecuteAllSymbolsCommand(APIConnector, true);

            List<Symbol> temporary_currencies = new List<Symbol>();
            
            ////////////////
            // for csv print
            // var csv = new StringBuilder();
            ////////////////


            foreach (SymbolRecord symbol in symbols.SymbolRecords)
            {
                /////////////
                // for csv print
                // var newLine = string.Format("{0};{1};{2};{3};{4}", symbol.Symbol, symbol.Description, symbol.Currency, symbol.GroupName, symbol.CategoryName);
                // csv.AppendLine(newLine);
                //
                /////////////

                Symbol sc = this.Symbols.Find(s => s.Name == symbol.Symbol);

                if (sc != null)
                    temporary_currencies.Add(sc);
            }
            
            ////////////////
            // for csv print
            // File.AppendAllText("C:\\Users\\valen\\Desktop\\market\\test.csv", csv.ToString());
            ////////////////

            foreach (Symbol s in this.Symbols)
            {
                Symbol sc = temporary_currencies.Find(s2 => s2.Name == s.Name);

                if (sc == null)
                    log.Warn("This currency doesn't exist -> " + s.Name);
            }

            this.Symbols = temporary_currencies;
            */
            if (this.Symbols.Count > 0)
            {
                return new Error(false, this.Symbols.Count.ToString() + " symbol(s) valid(s) !");
            }

            return new Error(true, "No symbol valid !");
        }

        /// <summary>
        /// Fonction de connexion au serveur xtb et de check de la configuration du logiciel
        /// </summary>
        /// <param name="_log"></param>
        /// <returns></returns>
        public Error ConnectAndCheck(Logger _log)
        {

            this.log = _log;

            ////////////////
            // Load de la configuration pour l'utilisateur
            ////////////////
            err = this.FeedConnector();
            if (err.IsAnError)
            {
                return err;
            }

            log.Info("Configuration loaded...");

            ////////////////
            // Test de connexion aux serveurs xtb
            ////////////////

            try
            {
                this.APIConnector = new SyncAPIConnector(Server);
            }
            catch
            {
                return new Error(true, "Unable to connect to server !");
            }

            log.Info("Server online...");

            ////////////////
            // Feed des Credentials    
            ////////////////

            this.Credentials = new Credentials(this.Login, this.Pwd);

            ////////////////
            // Tentative d'authentification
            ////////////////

            try
            {
                APICommandFactory.ExecuteLoginCommand(this.APIConnector, this.Credentials);
            }
            catch
            {
                return new Error(true, "Bad credentials !!");
            }

            log.Info("Server connected...");

            ////////////////
            // Vérification de la configuration (les devises)
            ////////////////

            log.Info("Configuration checking...");

            err = this.CheckSymbols();
            if (err.IsAnError)
            {
                return err;
            }

            log.Info("");
            log.Info("");

            log.Info("I will retrieve " + this.Symbols.Count.ToString() + " symbols :");

            foreach (Symbol s in this.Symbols)
            {
                log.Info("  > " + s.Name + " ( " + s.Id.ToString() + " )");
            }

            log.Info("");
            log.Info("");

            return new Error(false, "The system is ready !");
        }

        /// <summary>
        /// Fonction pour récupérer l'heure du serveur xtb
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return Tool.LongUnixTimeStampToDateTime(APICommandFactory.ExecuteServerTimeCommand(APIConnector, true).Time);
        }


        public Error RetrieveSymbolsAndAddOrUpdate()
        {
            foreach (Symbol symbol in this.Symbols)
            {
                log.Info("Retrieve data for this symbols : " + symbol.Name);

                DateTime tLastInsert = new DateTime();
                DateTime tStart = new DateTime();

                ////////////////
                // Récupération de la date du dernier insert (ou de l'absence en cas de setup du symbol)
                ////////////////

                err = this.MyDBConnector.Search_last_insert_for_this_value(ref tLastInsert, symbol.Id);
                if (err.IsAnError)
                    return err;
                
                ////////////////
                // Set de la date de récupération à time du dernier insert moins une heure histoire d'être sur des dernières valeurs
                ////////////////

                tStart = tLastInsert.AddHours(-1);

                log.Info("Last Insert -> " + tLastInsert.ToString("yyyy-MM-dd HH:mm:ss") + " ||| retrieve data from -> " + tStart.ToString("yyyy-MM-dd HH:mm:ss"));

                ////////////////
                // Récupération des dernières données en base pour ce symbol
                ////////////////

                List<Bid> bids_in_db = new List<Bid>();

                err = this.MyDBConnector.Load_bid_values_for_one_symbol(ref bids_in_db, tStart, symbol.Id, symbol.Name);
                if (err.IsAnError)
                    return err;

                ////////////////
                // Récupération des données de xtb sur la période sans données en base (ou sur un mois lors de l'init d'un symbol);
                ////////////////

                long? timeTStart = Tool.LongDateTimeToUnixTimeStamp(tStart);

                ChartLastResponse resp = APICommandFactory.ExecuteChartLastCommand(APIConnector, symbol.Name, xAPI.Codes.PERIOD_CODE.PERIOD_M5, timeTStart);

                RateInfoRecord[] infos = new RateInfoRecord[resp.RateInfos.Count];

                resp.RateInfos.CopyTo(infos, 0);
                
                ////////////////
                // Déclaration bids to update and to add in the database;
                ////////////////

                List<Bid> bids_in_db_to_update = new List<Bid>();
                List<Bid> bids_to_add = new List<Bid>();

                ////////////////
                // Comparaison des données en bases par rapport à celles récupérées chez xtb
                ////////////////
                // Vérification des bids à update
                ////////////////

                foreach (Bid b in bids_in_db)
                {
                    foreach (RateInfoRecord v in infos)
                    {
                        if (Tool.LongUnixTimeStampToDateTime(v.Ctm).CompareTo(b.Bid_at) != 0)
                        {
                            continue;
                        }

                        if (Convert.ToDouble(v.Open) != b.Bid_value)
                        {
                            b.Bid_value = Convert.ToDouble(v.Open);
                            bids_in_db_to_update.Add(b);
                        }
                    }
                }

                ////////////////
                // Vérification des bids à ajouter
                ////////////////
                
                foreach (RateInfoRecord v in infos)
                {
                    if (bids_in_db.Count == 0)
                    {
                        bids_to_add.Add(new Bid(symbol.Id, Tool.LongUnixTimeStampToDateTime(v.Ctm), Convert.ToDouble(v.Open)));
                        continue;
                    }
                    
                    int symbol_id = 0;

                    foreach (Bid b in bids_in_db)
                    {
                        if (Tool.LongUnixTimeStampToDateTime(v.Ctm).CompareTo(b.Bid_at) == 0)
                        {
                            symbol_id = 0;
                            break;
                        }
                        symbol_id = symbol.Id;
                    }

                    if (symbol_id != 0)
                        bids_to_add.Add(new Bid(symbol_id, Tool.LongUnixTimeStampToDateTime(v.Ctm), Convert.ToDouble(v.Open)));
                }

                ////////////////
                //Update des bids
                ////////////////
                
                err = this.MyDBConnector.Update_bid_values(bids_in_db_to_update);
                if (err.IsAnError)
                    return err;

                ////////////////
                // Ajout des bids
                ////////////////
                
                err = this.MyDBConnector.Add_bid_values(bids_to_add);
                if (err.IsAnError)
                    return err;

                log.Info("Data retrieved for this symbols : " + symbol.Name);
            }
            return new Error(false, "data updated");
        }
    }
}
