using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Configuration
    {
        public string Xtb_login { get; set; }
        public string Xtb_pwd { get; set; }
        public Server Xtb_server { get; set; }

        public string Mysql_host { get; set; }
        public string Mysql_login { get; set; }
        public string Mysql_pwd { get; set; }
        public string Mysql_database { get; set; }

        public Error LoadConfigurationSettings()
        {
            //////////////////////////////////////////////////
            // Load de l'id de l'utilisateur
            //////////////////////////////////////////////////

            try
            {
                this.Xtb_login = WebConfigurationManager.AppSettings["Xtb_login"];
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
                Xtb_pwd = WebConfigurationManager.AppSettings["xtb_pwd"];
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
                switch (WebConfigurationManager.AppSettings["Xtb_Server"])
                {
                    case "demo":
                        Xtb_server = Servers.DEMO;
                        break;
                    case "real":
                        Xtb_server = Servers.REAL;
                        break;
                    default:
                        return new Error(true, "Error reading app settings Server, not real or demo...");
                }
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Server : " + e.Message);
            }

            try
            {
                Mysql_host = WebConfigurationManager.AppSettings["Mysql_host"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_host : " + e.Message);
            }

            try
            {
                Mysql_login = WebConfigurationManager.AppSettings["Mysql_login"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_login : " + e.Message);
            }

            try
            {
                Mysql_pwd = WebConfigurationManager.AppSettings["Mysql_pwd"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_pwd : " + e.Message);
            }

            try
            {
                Mysql_database = WebConfigurationManager.AppSettings["Mysql_database"];
            }
            catch (ConfigurationErrorsException e)
            {
                return new Error(true, "Error reading app settings Mysql_database : " + e.Message);
            }

            return new Error(false, "Web setting loaded!");
        }
    }
}
