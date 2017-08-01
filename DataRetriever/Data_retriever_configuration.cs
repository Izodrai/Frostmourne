using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frostmourne_basics;
using System.Configuration;
using xAPI.Sync;

namespace DataRetriever
{
    class Data_retriever_configuration
    {
        public static Error LoadAPIConfigurationSettings(ref Frostmourne_basics.Configuration Config)
        {
            //////////////////////////////////////////////////
            // Load de l'environnement
            //////////////////////////////////////////////////

            try
            {
                Config.Environnement = ConfigurationManager.AppSettings["Environnement"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings xtb_login : " + e.Message);
            }

            if (Config.Environnement == "prod")
            {
                Config.Prod = true;
            }
            else
            {
                Config.Environnement = "dev";
            }

            //////////////////////////////////////////////////
            // Load de l'id de l'utilisateur
            //////////////////////////////////////////////////

            try
            {
                Config.Xtb_login = ConfigurationManager.AppSettings["Xtb_login"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings xtb_login : " + e.Message);
            }

            //////////////////////////////////////////////////
            // Load du mot de passe de l'utilisateur
            //////////////////////////////////////////////////

            try
            {
                Config.Xtb_pwd = ConfigurationManager.AppSettings["xtb_pwd"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings UserPwd : " + e.Message);
            }

            //////////////////////////////////////////////////
            // Load du type de connexion aux serveurs (demo ou real uniquement)
            //////////////////////////////////////////////////

            try
            {
                switch (ConfigurationManager.AppSettings["Xtb_Server"])
                {
                    case "demo":
                        Config.Xtb_server = Servers.DEMO;
                        break;
                    case "real":
                        Config.Xtb_server = Servers.REAL;
                        break;
                    default:
                        return new Error(true, "Error reading app settings Server, not real or demo...");
                }
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Server : " + e.Message);
            }

            //////////////////////////////////////////////////
            // Load de l'host et mdp mysql suivant l'environnement
            //////////////////////////////////////////////////
            
            try
            {
                Config.Mysql_host = ConfigurationManager.AppSettings["Mysql_host"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_host : " + e.Message);
            }
            try
            {
                Config.Mysql_pwd = ConfigurationManager.AppSettings["Mysql_pwd"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_pwd : " + e.Message);
            }
            try
            {
                Config.Mysql_port = ConfigurationManager.AppSettings["Mysql_port"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_port : " + e.Message);
            }

            //////////////////////////////////////////////////
            // Load du login mysql
            //////////////////////////////////////////////////

            try
            {
                Config.Mysql_login = ConfigurationManager.AppSettings["Mysql_login"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_login : " + e.Message);
            }

            //////////////////////////////////////////////////
            // Load de la base de données
            //////////////////////////////////////////////////

            try
            {
                Config.Mysql_database = ConfigurationManager.AppSettings["Mysql_database"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_database : " + e.Message);
            }

            Log.GreenInfo("Configuration loaded");
            Log.JumpLine();

            return new Error(false, "Web setting loaded!");
        }

        public static void PrintConfiguration(ref Frostmourne_basics.Configuration configuration)
        {
            Log.CyanInfo("\t>Environnement : " + configuration.Environnement);
            Log.JumpLine();
            Log.WhiteInfo("\t>Xtb_login :  " + configuration.Xtb_login);
            Log.CyanInfo("\t>Xtb_pwd :    " + configuration.Xtb_pwd);
            Log.WhiteInfo("\t>Xtb_server : " + configuration.Xtb_server);
            Log.JumpLine();
            Log.CyanInfo("\t>Mysql_host :     " + configuration.Mysql_host);
            Log.WhiteInfo("\t>Mysql_login :    " + configuration.Mysql_login);
            Log.CyanInfo("\t>Mysql_pwd :      " + configuration.Mysql_pwd);
            Log.WhiteInfo("\t>Mysql_database : " + configuration.Mysql_database);
            Log.CyanInfo("\t>Mysql_port :     " + configuration.Mysql_port);
            Log.JumpLine();
        }
    }
}
