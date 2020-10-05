using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Pra.Bieren.CORE.Services
{
    public class DBConnector
    {
        public static DataTable ExecuteSelect(string sqlInstructie)
        {
            string constring = Helper.GetConnectionString();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sqlInstructie, constring);
            try
            {
                da.Fill(ds);
            }
            catch (Exception fout)
            {
                string foutmelding = fout.Message;
                return null;
            }
            return ds.Tables[0];
        }
        public static bool ExecuteCommand(string sqlInstructie)
        {
            string constring = Helper.GetConnectionString();
            SqlConnection mijnVerbinding = new SqlConnection(constring);
            SqlCommand mijnOpdracht = new SqlCommand(sqlInstructie, mijnVerbinding);
            try
            {
                mijnOpdracht.Connection.Open();
                mijnOpdracht.ExecuteNonQuery();
                mijnVerbinding.Close();
                return true;
            }
            catch (Exception fout)
            {
                string foutmelding = fout.Message;
                return false;
            }
        }
        public static string ExecuteScalaire(string sqlScalaireInstructie)
        {
            string constring = Helper.GetConnectionString();
            SqlConnection mijnVerbinding = new SqlConnection(constring);
            SqlCommand mijnOpdracht = new SqlCommand(sqlScalaireInstructie, mijnVerbinding);
            mijnVerbinding.Open();
            string retour = mijnOpdracht.ExecuteScalar().ToString();
            mijnVerbinding.Close();
            return retour;
        }
    }
}
