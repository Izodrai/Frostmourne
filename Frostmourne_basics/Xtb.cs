using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Xtb
    {
        public static Error Retrieve_and_update_data_for_symbols(Mysql MyDB, SyncAPIConnector Xtb_api_connector, int _days_to_retrieve, int _months_to_retrieve)
        {
            Error err;
            List<Symbol> symbols = new List<Symbol>();

            err = MyDB.Load_data_retrieve_symbols(ref symbols);
            if (err.IsAnError)
            {
                return err;
            }

            DateTime tNow = DateTime.Now;
            DateTime tFrom = tNow.Date.AddMonths(-_months_to_retrieve);
            tFrom = tFrom.AddDays(-_days_to_retrieve);

            Log.Info("Time Now -> " + tNow.ToString("yyyy-MM-dd HH:mm:ss") + " ||| retrieve data from -> " + tFrom.ToString("yyyy-MM-dd HH:mm:ss"));

            foreach (Symbol symbol in symbols)
            {
                List<Bid> bids = new List<Bid>();

                err = Retrieve_bids_of_symbol_from_xtb(Xtb_api_connector, symbol, xAPI.Codes.PERIOD_CODE.PERIOD_M5, tNow, tFrom, ref bids);
                if (err.IsAnError)
                {
                    return err;
                }

                err = MyDB.Insert_or_update_bid_values(bids);
                if (err.IsAnError)
                {
                    return err;
                }
            }

            return new Error(false, "");
        }
        
        public static Error Retrieve_bids_of_symbol_from_xtb(SyncAPIConnector _api_connector, Symbol _symbol, xAPI.Codes.PERIOD_CODE _period, DateTime tNow, DateTime tFrom, ref List<Bid> bids)
        {
            Log.Info("Retrieve data for -> " + _symbol.Name);
            
            ////////////////
            // Récupération des données de xtb sur la période
            ////////////////

            long? timeTStart = Tool.LongDateTimeToUnixTimeStamp(tFrom);

            ChartLastResponse resp;

            try
            {
                resp = APICommandFactory.ExecuteChartLastCommand(_api_connector, _symbol.Name, _period, timeTStart);
            }
            catch (Exception e)
            {
                return new Error(true, "Error during ExecuteChartLastCommand : " + e.Message);
            }

            RateInfoRecord[] infos = new RateInfoRecord[resp.RateInfos.Count];

            resp.RateInfos.CopyTo(infos, 0);

            if (infos.Length == 0)
                return new Error(true, "No data to retrieve in this range");

            foreach (RateInfoRecord v in infos)
                bids.Add(new Bid(_symbol.Id, _symbol.Name, Tool.LongUnixTimeStampToDateTime(v.Ctm), Convert.ToDouble(v.Open) + Convert.ToDouble(v.Close)));
            
            return new Error(false, "Data symbol retrieved !");
        }

        public static Error Retrieve_list_of_symbols_from_xtb_to_csv(SyncAPIConnector API_Connector)
        {
            Log.Info("Retrieve all forex symbols");

            if (File.Exists("C:\\Users\\valen\\Documents\\Visual Studio 2017\\Projects\\ConsoleAppTestsXTB\\ConsoleAppTestsXTB\\All_symbols.csv"))
            {
                try
                {
                    File.Delete("C:\\Users\\valen\\Documents\\Visual Studio 2017\\Projects\\ConsoleAppTestsXTB\\ConsoleAppTestsXTB\\All_symbols.csv");
                }
                catch (Exception e)
                {
                    return new Error(true, "Cannot delete file : " + e.Message);
                }
            }

            AllSymbolsResponse symbols;

            try
            {
                symbols = APICommandFactory.ExecuteAllSymbolsCommand(API_Connector, true);
            }
            catch (Exception e)
            {
                return new Error(true, "Cannot retrieve symbols : " + e.Message);
            }

            var csv = new StringBuilder();

            foreach (SymbolRecord symbol in symbols.SymbolRecords)
            {
                //if (symbol.CategoryName != "Forex")
                //    continue;

                var newLine = string.Format("{0};{1};{2};{3};{4}", symbol.Symbol, symbol.Description, symbol.Currency, symbol.GroupName, symbol.CategoryName);
                csv.AppendLine(newLine);
            }

            try
            {
                File.AppendAllText("C:\\Users\\valen\\Documents\\Visual Studio 2017\\Projects\\ConsoleAppTestsXTB\\ConsoleAppTestsXTB\\All_symbols.csv", csv.ToString());
            }
            catch (Exception e)
            {
                return new Error(true, "Error during file writing : " + e.Message);
            }

            Log.Info("Forex symbols retrieved");

            return new Error(false, "Symbols retrieved !");
        }
    }
}
