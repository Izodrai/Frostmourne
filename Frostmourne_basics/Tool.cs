using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Tool
    {
        public static DateTime LongUnixTimeStampToDateTime(long? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(unixTimeStamp / 1000)).ToUniversalTime();
            return dtDateTime;
        }

        public static long LongDateTimeToUnixTimeStamp(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double timeStamp = (date.ToUniversalTime() - epoch).TotalSeconds;
            return Convert.ToInt64(timeStamp) * 1000;
        }

        public static DateTime DoubleUnixTimeStampToDateTime(double? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds((double)unixTimeStamp).ToUniversalTime();
        }

        public static double DoubleDateTimeToUnixTimeStamp(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 1, 0, 0, DateTimeKind.Utc);
            return (date.ToUniversalTime() - epoch).TotalSeconds;
        }

        public static Error InitMyDb(ref Configuration configuration, ref Mysql MyDB)
        {
            Error err;

            /*
            err = configuration.LoadAPIConfigurationSettings();

            if (err.IsAnError)
            {
                return err;
            }
            */
            //////////////////////////////////////////////
            //
            // Tentative d'authentification au serveur Atiesh
            //
            //////////////////////////////////////////////

            MyDB = new Mysql(configuration.Mysql_host, configuration.Mysql_port, configuration.Mysql_database, configuration.Mysql_login, configuration.Mysql_pwd);

            err = MyDB.Connect();
            if (err.IsAnError)
                return err;
            MyDB.Close();

            return new Error(false, "Init success");
        }

        public static Error InitAll(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err;
            Credentials Credentials;

            err = configuration.LoadAPIConfigurationSettings();

            if (err.IsAnError)
            {
                return err;
            }

            //////////////////////////////////////////////
            //
            // Test de connexion aux serveurs xtb
            //
            //////////////////////////////////////////////
            
            Xtb_api_connector = new SyncAPIConnector(configuration.Xtb_server);


            //////////////////////////////////////////////
            //
            // Renseignement des Credentials 
            //   
            //////////////////////////////////////////////

            Credentials = new Credentials(configuration.Xtb_login, configuration.Xtb_pwd);

            //////////////////////////////////////////////
            //
            // Tentative d'authentification au serveur XTB
            //
            //////////////////////////////////////////////

            try
            {
                APICommandFactory.ExecuteLoginCommand(Xtb_api_connector, Credentials);
            }
            catch
            {
                return err;
            }

            //////////////////////////////////////////////
            //
            // Tentative d'authentification au serveur Atiesh
            //
            //////////////////////////////////////////////

            MyDB = new Mysql(configuration.Mysql_host, configuration.Mysql_port, configuration.Mysql_database, configuration.Mysql_login, configuration.Mysql_pwd);

            err = MyDB.Connect();
            if (err.IsAnError)
            {
                return err;
            }
            MyDB.Close();

            return new Error(false, "Init success");
        }
    }
}
