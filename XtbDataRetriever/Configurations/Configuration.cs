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

        public static Error LoadConnectorSettings(ref string _login, ref string _pwd, ref string _server)
        {
            //////////////////////////////////////////////////
            // Load de l'id de l'utilisateur
            //////////////////////////////////////////////////

            var appSettings = ConfigurationManager.AppSettings;

            try
            {
                _login = appSettings["UserId"];
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
                _pwd = appSettings["UserPwd"];
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
                switch (appSettings["Server"])
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

            return new Error(false, "App setting loaded!");
        }
    }
}
