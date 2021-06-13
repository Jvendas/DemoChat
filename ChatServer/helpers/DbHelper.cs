using System;
using System.Data.SqlClient;

namespace ChatServer.helpers
{
    public class DbHelper
    {
        private static string connectionString = Properties.Settings.Default.DemoChatBDConnectionString;
        private SqlConnection conn;

        public DbHelper()
        {
            try
            {
                // configurar ligação à Base de Dados
                conn = new SqlConnection();
                conn.ConnectionString = connectionString;

                // abrir ligação à Base de Dados
                conn.Open();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void CloseConnection()
        {
            conn.Close();
        }

        public int ExecutarComandoSQL(SqlCommand sqlCommand)
        {
            try
            {
                LogHelper.logToFile("SQL Executing: '" + sqlCommand.CommandText + "'");

                // adicionar a connection ao comando sql
                sqlCommand.Connection = conn;

                // executar comando SQL
                int lines = sqlCommand.ExecuteNonQuery();

                return lines;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
