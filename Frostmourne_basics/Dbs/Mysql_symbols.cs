using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        /// <summary>
        /// Récupère les symbols sur lesquels on doit récupérer des données de XTB 
        /// </summary>
        /// <param name="_ss"></param>
        /// <returns></returns>
        public Error Load_data_retrieve_symbols(ref List<Symbol> _ss)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference, description FROM v_symbols_data_retrieve", this.Mysql_connector);
                
                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _ss.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1]), Convert.ToString(values[2])));
                    }
                }
                this.Close();
                return new Error(false, "Symbols loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }

        /// <summary>
        /// Récupère les symbols actifs
        /// </summary>
        /// <param name="_ss"></param>
        /// <returns></returns>
        public Error Load_active_symbols(ref List<Symbol> _ss)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference, description FROM v_symbols_active", this.Mysql_connector);
                
                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _ss.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1]), Convert.ToString(values[2])));
                    }
                }
                this.Close();
                return new Error(false, "Symbols loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }

        /// <summary>
        /// Récupère les symbols inactifs
        /// </summary>
        /// <param name="_ss"></param>
        /// <returns></returns>
        public Error Load_inactive_symbols(ref List<Symbol> _ss)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference, description FROM v_symbols_inactive", this.Mysql_connector);
                
                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        
                        _ss.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1]), Convert.ToString(values[2])));
                    }
                }
                this.Close();
                return new Error(false, "Symbols loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }

        /// <summary>
        /// Récupère les symbols en mode simulation
        /// </summary>
        /// <param name="_ss"></param>
        /// <returns></returns>
        public Error Load_simulation_symbols(ref List<Symbol> _ss)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference, description FROM v_symbols_simulation", this.Mysql_connector);
                
                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _ss.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1]), Convert.ToString(values[2])));
                    }
                }
                this.Close();
                return new Error(false, "Symbols loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }

        /// <summary>
        /// Récupère les symbols en mode standby
        /// </summary>
        /// <param name="_ss"></param>
        /// <returns></returns>
        public Error Load_standby_symbols(ref List<Symbol> _ss)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference, description FROM v_symbols_standby", this.Mysql_connector);
                
                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _ss.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1]), Convert.ToString(values[2])));
                    }
                }
                this.Close();
                return new Error(false, "Symbols loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
    }
}
