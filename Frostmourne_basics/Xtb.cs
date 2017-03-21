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
        public static Error Load_bids_values_symbol()
        {

            return new Error(false, "data loaded");
        }

        public static Error Retrieve_and_update_data_for_symbol(Mysql MyDB, SyncAPIConnector Xtb_api_connector, Symbol symbol, int _days_to_retrieve, int _months_to_retrieve)
        {
            Error err;
            List<Symbol> not_inactiv_symbols = new List<Symbol>();

            err = MyDB.Load_data_retrieve_symbols(ref not_inactiv_symbols);
            if (err.IsAnError)
            {
                MyDB.Close();
                return err;
            }
            MyDB.Close();

            foreach (Symbol not_inactiv_s in not_inactiv_symbols)
            {
                if (symbol.Name == not_inactiv_s.Name)
                {
                    symbol = not_inactiv_s;
                    break;
                }
            }
            
            if (symbol.Id == 0)
                return new Error(true, "this symbols are not inactive or doesn't exist");

            DateTime tNow = DateTime.Now;
            DateTime tFrom = tNow.Date.AddMonths(-_months_to_retrieve);
            tFrom = tFrom.AddDays(-_days_to_retrieve);

            Log.Info("Time Now -> " + tNow.ToString("yyyy-MM-dd HH:mm:ss") + " ||| retrieve data from -> " + tFrom.ToString("yyyy-MM-dd HH:mm:ss"));

            List<Bid> mysql_bids = new List<Bid>();

            try
            {
                MyDB.Load_bids_values_symbol(ref mysql_bids, tFrom, tNow, symbol);
            }
            catch (Exception e)
            {
                return new Error(true, "Error during Load_bids_values_symbol : " + e.Message);
            }

            List<Bid> xtb_bids = new List<Bid>();

            err = Retrieve_bids_of_symbol_from_xtb(Xtb_api_connector, symbol, xAPI.Codes.PERIOD_CODE.PERIOD_M5, tNow, tFrom, ref xtb_bids);
            if (err.IsAnError)
            {
                return err;
            }

            List<Bid> bids_to_insert_or_update = new List<Bid>();

            foreach (Bid xtb_bid in xtb_bids)
            {
                bool exist = false;
                foreach (Bid mysql_bid in mysql_bids)
                {
                    if (xtb_bid.Symbol_id != mysql_bid.Symbol_id)
                        continue;
                    if (xtb_bid.Bid_at != mysql_bid.Bid_at)
                        continue;
                    if (xtb_bid.Last_bid != mysql_bid.Last_bid)
                        bids_to_insert_or_update.Add(xtb_bid);
                    exist = true;
                }
                if (!exist)
                    bids_to_insert_or_update.Add(xtb_bid);
            }

            err = MyDB.Insert_or_update_bids_values(bids_to_insert_or_update);
            if (err.IsAnError)
            {
                MyDB.Close();
                return err;
            }
            MyDB.Close();

            return new Error(false, "data retrieved");
        }

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
                List<Bid> mysql_bids = new List<Bid>();

                try
                {
                    MyDB.Load_bids_values_symbol(ref mysql_bids, tFrom, tNow, symbol);
                }
                catch (Exception e)
                {
                    return new Error(true, "Error during Load_bids_values_symbol : " + e.Message);
                }

                List<Bid> xtb_bids = new List<Bid>();

                err = Retrieve_bids_of_symbol_from_xtb(Xtb_api_connector, symbol, xAPI.Codes.PERIOD_CODE.PERIOD_M5, tNow, tFrom, ref xtb_bids);
                if (err.IsAnError)
                {
                    return err;
                }

                List<Bid> bids_to_insert_or_update = new List<Bid>();

                foreach (Bid xtb_bid in xtb_bids)
                {
                    bool exist = false;
                    foreach (Bid mysql_bid in mysql_bids)
                    {
                        if (xtb_bid.Symbol_id != mysql_bid.Symbol_id)
                            continue;
                        if (xtb_bid.Bid_at != mysql_bid.Bid_at)
                            continue;
                        if (xtb_bid.Last_bid != mysql_bid.Last_bid)
                            bids_to_insert_or_update.Add(xtb_bid);
                        exist = true;
                    }
                    if (!exist)
                        bids_to_insert_or_update.Add(xtb_bid);
                }

                err = MyDB.Insert_or_update_bids_values(bids_to_insert_or_update);
                if (err.IsAnError)
                {
                    return err;
                }
            }

            return new Error(false, "data retrieved");
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
