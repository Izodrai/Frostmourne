using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtbDataRetriever.Errors;

namespace XtbDataRetriever.Configurations
{
    class Configuration
    {

        public static Error LoadConnectorSettings(
            ref string _login, 
            ref string _pwd, 
            ref string _server, 
            ref string _mysql_server, 
            ref string _mysql_database,
            ref string _mysql_login,
            ref string _mysql_password)
        {
            //////////////////////////////////////////////////
            // Load de l'id de l'utilisateur
            //////////////////////////////////////////////////

            var appSettings = ConfigurationManager.AppSettings;

            try
            {
                _login = appSettings["XtbUserId"];
            }
            catch (ConfigurationErrorsException)
            {
                return new Error(true, "Error reading app settings UserId");
            }

            //////////////////////////////////////////////////
            // Load du mot de passe de l'utilisateur
            //////////////////////////////////////////////////
            try
            {
                _pwd = appSettings["XtbUserPwd"];
            }
            catch (ConfigurationErrorsException)
            {
                return new Error(true, "Error reading app settings UserPwd");
            }


            //////////////////////////////////////////////////
            // Load du type de connexion aux serveurs (demo ou real uniquement)
            //////////////////////////////////////////////////
            try
            {
                switch (appSettings["XtbServer"])
                {
                    case "demo":
                        _server = "demo";
                        break;
                    case "real":
                        _server = "real";
                        break;
                    default:
                        return new Error(true, "Error reading app settings Server, not real or demo...");
                }
            }
            catch (ConfigurationErrorsException)
            {
                return new Error(true, "Error reading app settings Server");
            }

            try
            {
                _mysql_server = appSettings["MysqlServer"];
            }
            catch (ConfigurationErrorsException)
            {
                return new Error(true, "Error reading app settings MysqlServer");
            }

            try
            {
                _mysql_database = appSettings["MysqlDatabase"];
            }
            catch (ConfigurationErrorsException)
            {
                return new Error(true, "Error reading app settings MysqlDatabase");
            }

            try
            {
                _mysql_login = appSettings["MysqlLogin"];
            }
            catch (ConfigurationErrorsException)
            {
                return new Error(true, "Error reading app settings MysqlLogin");
            }

            try
            {
                _mysql_password = appSettings["MysqlPassword"];
            }
            catch (ConfigurationErrorsException)
            {
                return new Error(true, "Error reading app settings MysqlPassword");
            }

            return new Error(false, "App setting loaded!");
        }
    }
}
