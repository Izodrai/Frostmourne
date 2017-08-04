using Frostmourne_basics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DataAPI
{
    public class Data_api_configuration
    {
        public static Error LoadAPIConfigurationSettings(ref Frostmourne_basics.Configuration Config)
        {
            //////////////////////////////////////////////////
            // Load de l'environnement
            //////////////////////////////////////////////////

            try
            {
                Config.Environnement = WebConfigurationManager.AppSettings["Environnement"];
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
                Config.Xtb_login = WebConfigurationManager.AppSettings["Xtb_login"];
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
                Config.Xtb_pwd = WebConfigurationManager.AppSettings["xtb_pwd"];
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
                Config.Set_server(WebConfigurationManager.AppSettings["Xtb_Server"]);
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
                Config.Mysql_host = WebConfigurationManager.AppSettings["Mysql_host"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_host : " + e.Message);
            }
            try
            {
                Config.Mysql_pwd = WebConfigurationManager.AppSettings["Mysql_pwd"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_pwd : " + e.Message);
            }
            try
            {
                Config.Mysql_port = WebConfigurationManager.AppSettings["Mysql_port"];
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
                Config.Mysql_login = WebConfigurationManager.AppSettings["Mysql_login"];
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
                Config.Mysql_database = WebConfigurationManager.AppSettings["Mysql_database"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_database : " + e.Message);
            }

            Log.GreenInfo("Configuration loaded");
            Log.JumpLine();

            return new Error(false, "Web setting loaded!");
        }
    }
}
