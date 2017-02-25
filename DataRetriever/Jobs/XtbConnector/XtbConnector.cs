using System;
using System.Collections.Generic;
using DataRetriever.Dbs;
using DataRetriever.Jobs.Symbols;
using DataRetriever.Jobs.Bids;
using DataRetriever.Errors;
using DataRetriever.Configurations;
using DataRetriever.Tools;
using xAPI.Sync;
using xAPI.Commands;
using xAPI.Responses;
using xAPI.Records;
using DataRetriever.Logs;
using System.Timers;
using DataRetriever.Jobs.Calculations;

namespace DataRetriever.Jobs.XtbConnector
{
    class XtbConnector
    {
        protected Error err { get; set; }

        protected string Login { get; set; }

        protected string Pwd { get; set; }

        protected Server Server { get; set; }

        protected string MySQL_Server { get; set; }

        protected string MySQL_Database { get; set; }

        protected string MySQL_Login { get; set; }

        protected string MySQL_Password { get; set; }

        protected List<Symbol> Symbols { get; set; }

        protected SyncAPIConnector API_Connector { get; set; }

        protected Credentials Credentials { get; set; }

        protected Mysql MyDB_Connector { get; set; }

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

            string _mysql_server = "";
            string _mysql_database = "";
            string _mysql_login = "";
            string _mysql_password = "";

            List<string> _currencies = new List<string>();

            //////////////////////////////////////////////////
            // Load de la configuration pour l'utilisateur
            //////////////////////////////////////////////////

            err = Configuration.LoadConnectorSettings(ref _login, ref _pwd, ref _server, ref _mysql_server, ref _mysql_database, ref _mysql_login, ref _mysql_password);
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

            this.MySQL_Server = _mysql_server;
            this.MySQL_Database = _mysql_database;
            this.MySQL_Login = _mysql_login;
            this.MySQL_Password = _mysql_password;

            return new Error(false, "Connector Configuration downloaded !");
        }

        /// <summary>
        /// Fonction de vérification des symbols en base par rapport à celles disponibles sur le serveur xtb
        /// </summary>
        /// <returns></returns>
        protected Error CheckSymbols()
        {
            this.MyDB_Connector = new Mysql(this.MySQL_Server, this.MySQL_Database, this.MySQL_Login, this.MySQL_Password);

            if (this.MyDB_Connector.Connect().IsAnError)
            {
                return err;
            }

            List<Symbol> ss = new List<Symbol>();

            err = this.MyDB_Connector.Load_symbols(ref ss);
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
                    Log.Warn("This currency doesn't exist -> " + s.Name);
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
        /// Fonction de connexion au serveur xtb et de check de la configuration du Logiciel
        /// </summary>
        /// <returns></returns>
        public Error ConnectAndCheck()
        {
            ////////////////
            // Load de la configuration pour l'utilisateur
            ////////////////
            err = this.FeedConnector();
            if (err.IsAnError)
            {
                return err;
            }

            Log.Info("Configuration loaded...");

            ////////////////
            // Test de connexion aux serveurs xtb
            ////////////////

            try
            {
                this.API_Connector = new SyncAPIConnector(Server);
            }
            catch
            {
                return new Error(true, "Unable to connect to server !");
            }

            Log.Info("Server online...");

            ////////////////
            // Feed des Credentials    
            ////////////////

            this.Credentials = new Credentials(this.Login, this.Pwd);

            ////////////////
            // Tentative d'authentification
            ////////////////

            try
            {
                APICommandFactory.ExecuteLoginCommand(this.API_Connector, this.Credentials);
            }
            catch
            {
                return new Error(true, "Pbs with the credentials or the connection !");
            }

            Log.Info("Server connected...");

            ////////////////
            // Vérification de la configuration (les devises)
            ////////////////

            Log.Info("Configuration checking...");

            err = this.CheckSymbols();
            if (err.IsAnError)
            {
                return err;
            }

            Log.Info("");
            Log.Info("");

            Log.CyanInfo("Retrieve " + this.Symbols.Count.ToString() + " symbols :");

            foreach (Symbol s in this.Symbols)
            {
                Log.WhiteInfo("  > " + s.Name + " ( " + s.Id.ToString() + " )");
            }

            Log.Info("");
            Log.Info("");

            return new Error(false, "The system is ready !");
        }

        /// <summary>
        /// Fonction pour récupérer l'heure du serveur xtb
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return Tool.LongUnixTimeStampToDateTime(APICommandFactory.ExecuteServerTimeCommand(API_Connector, true).Time);
        }

        /// <summary>
        /// Fonction pour récupérer les dernières valeurs d'un symbol chez xtb
        /// et update ou ajouter en base si c'est différent ou inexistant
        /// </summary>
        /// <returns></returns>
        public Error RetrieveSymbolsAndAddOrUpdate()
        {
            foreach (Symbol symbol in this.Symbols)
            {
                Log.Info("Retrieve data for this symbols : " + symbol.Name);

                DateTime tLastInsert = new DateTime();
                DateTime tStart = new DateTime();

                ////////////////
                // Récupération de la date du dernier insert (ou de l'absence en cas de setup du symbol)
                ////////////////

                err = this.MyDB_Connector.Search_last_insert_for_this_value(ref tLastInsert, symbol.Id);
                if (err.IsAnError)
                    return err;
                
                ////////////////
                // Set de la date de récupération à time du dernier insert moins une heure histoire d'être sur des dernières valeurs
                ////////////////

                tStart = tLastInsert.AddHours(-1);

                Log.Info("Last Insert -> " + tLastInsert.ToString("yyyy-MM-dd HH:mm:ss") + " ||| retrieve data from -> " + tStart.ToString("yyyy-MM-dd HH:mm:ss"));

                ////////////////
                // Récupération des dernières données en base pour ce symbol
                ////////////////

                List<Bid> bids_in_db = new List<Bid>();

                err = this.MyDB_Connector.Load_bid_values_for_one_symbol(ref bids_in_db, tStart, symbol.Id, symbol.Name);
                if (err.IsAnError)
                    return err;

                ////////////////
                // Récupération des données de xtb sur la période sans données en base (ou sur un mois lors de l'init d'un symbol);
                ////////////////

                long? timeTStart = Tool.LongDateTimeToUnixTimeStamp(tStart);

                ChartLastResponse resp = APICommandFactory.ExecuteChartLastCommand(API_Connector, symbol.Name, xAPI.Codes.PERIOD_CODE.PERIOD_M5, timeTStart);

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

                        double open = Convert.ToDouble(v.Open);
                        double close = Convert.ToDouble(v.Open) + Convert.ToDouble(v.Close);

                        if (open != b.Start_bid || close != b.Last_bid)
                        {
                            b.Start_bid = open;
                            b.Last_bid = close;
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
                        double open = Convert.ToDouble(v.Open);
                        double close = Convert.ToDouble(v.Open) + Convert.ToDouble(v.Close);

                        bids_to_add.Add(new Bid(symbol.Id, Tool.LongUnixTimeStampToDateTime(v.Ctm), open, close));
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
                    {
                        double open = Convert.ToDouble(v.Open);
                        double close = Convert.ToDouble(v.Open) + Convert.ToDouble(v.Close);

                        bids_to_add.Add(new Bid(symbol_id, Tool.LongUnixTimeStampToDateTime(v.Ctm), open, close));
                    }
                }

                ////////////////
                //Update des bids
                ////////////////
                
                err = this.MyDB_Connector.Update_bid_values(bids_in_db_to_update);
                if (err.IsAnError)
                    return err;

                ////////////////
                // Ajout des bids
                ////////////////
                
                err = this.MyDB_Connector.Add_bid_values(bids_to_add);
                if (err.IsAnError)
                    return err;

                Log.GreenInfo("Data retrieved for this symbols : " + symbol.Name);
                Log.Info("");
            }
            return new Error(false, "data updated");
        }

        public Error LoopDataRetrieveAndCalculation()
        {

            err = RetrieveSymbolsAndAddOrUpdate();
            if (err.IsAnError)
            {
                return err;
            }

            err = CalculateBids();
            if (err.IsAnError)
            {
                return err;
            }

            return new Error(false, "The application has been terminated !!!");

            /*
            try
            {
                // Création d'un timer de 30s pour la récupération des données
                Timer dataTimer = new Timer(30000);
                // Hook up the Elapsed event for the timer. 
                dataTimer.Elapsed += ProcessDataRetrievingAndCalculation;
                dataTimer.AutoReset = true;
                dataTimer.Enabled = true;

                Log.WhiteInfo("Press the Enter key to exit the application...");
                Log.Info("");

                Console.ReadLine();

                dataTimer.Stop();
                dataTimer.Dispose();

                return new Error(false, "The application has been quit !!!");
            }
            catch (Exception ex)
            {
                return new Error(true, ex.Message);
            }
            */

        }

        private void ProcessDataRetrievingAndCalculation(Object source, ElapsedEventArgs e)
        {
            err = RetrieveSymbolsAndAddOrUpdate();
            if (err.IsAnError)
            {
                Log.Error(err.MessageError);
                return;
            }

            err = CalculateBids();
            if (err.IsAnError)
            {
                Log.Error(err.MessageError);
                return;
            }
        }

        /// <summary>
        /// Fonction pour calculer les divers outils mathématique sur les derniers bids
        /// </summary>
        /// <returns></returns>
        private Error CalculateBids()
        {
            foreach (Symbol symbol in this.Symbols)
            {

                ////////////////
                // Récupération des dernières données en base pour ce symbol
                ////////////////

                List<Bid> bids_to_calculate = new List<Bid>();

                err = this.MyDB_Connector.Load_last_5_days_bid_values_for_one_symbol(ref bids_to_calculate, symbol.Id, symbol.Name);
                if (err.IsAnError)
                    return err;
                
                ////////////////
                // Calcul des moyennes mobiles simple (SMA)
                ////////////////

                err = Calculation.SMA(ref bids_to_calculate);
                if (err.IsAnError)
                    return err;

                ////////////////
                // Calcul des moyennes mobiles exponentielles (SMA)
                ////////////////

                err = Calculation.EMA(ref bids_to_calculate);
                if (err.IsAnError)
                    return err;

                ////////////////
                // Calcul des indicateurs du MACD
                ////////////////

                int trigger = 30;

                err = Calculation.MACD(ref bids_to_calculate, trigger);
                if (err.IsAnError)
                    return err;

                ////////////////
                // Ajout et update des calculs sur les bids
                ////////////////
                
                foreach (Bid b in bids_to_calculate)
                {
                    // Console.WriteLine(b.Bid_at.ToString()+ " | Start_bid -> " + b.Start_bid+ " | Last_bid -> " + b.Last_bid+ " | Macd_signal -> " + b.Calculation.Macd_signal + " | Macd_absol_max_signal -> " + b.Calculation.Macd_absol_max_signal + " | Macd_absol_trigger_signal -> " + b.Calculation.Macd_absol_trigger_signal);
                    
                    if (b.Calculation.Id == 0) {
                        err = this.MyDB_Connector.Add_stock_analyse(b);
                        if (err.IsAnError)
                            return err;
                        continue;
                    }

                    if (b.Calculation.Data_to_update)
                    {
                        err = this.MyDB_Connector.Update_stock_analyse(b);
                        if (err.IsAnError)
                            return err;
                        continue;
                    }
                }
            }
            
            return new Error(false, "Calculations down");
        }
    }
}
