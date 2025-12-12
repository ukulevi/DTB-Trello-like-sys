using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TrelloSys
{
    public class DatabaseHelper
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["TrelloTaskManagementDB"].ConnectionString;

        public DataTable ExecuteQuery(string query, SqlParameter[] p = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = query.Trim().Contains(" ") ? CommandType.Text : CommandType.StoredProcedure;
                if (p != null) cmd.Parameters.AddRange(p);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool ExecuteNonQuery(string query, SqlParameter[] p = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = query.Trim().Contains(" ") ? CommandType.Text : CommandType.StoredProcedure;
                if (p != null) cmd.Parameters.AddRange(p);
                cmd.ExecuteNonQuery();
                return true;
            }
        }
    }
}