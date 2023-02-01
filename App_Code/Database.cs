using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Xml;

/// <summary>
/// Summary description for Database
/// </summary>

public class Database
{
    public Database()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string ConnectionString
    {
        get
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to get Database Connection string from Web Config File");
            }
        }
    }
    public static SqlDataReader get_DataRead(string query)
    {
        SqlDataReader dr = null;
        try
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            return dr;
        }
        catch (Exception ex)
        {
            //return ex.Message.ToString();
        }
        return dr;
    }

    public static DataTable get_DataTable(string query)
    {
        try
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error occoured while retriving data from database");
        }
    }





    public static string Execute(string query)
    {
        try
        {
            string ret = "0";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
            return "1";
            // return ret.ToString();
        }

        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public static int ExecuteScaler(string query)
    {
        try
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);

            ret = (int)cmd.ExecuteScalar();
            con.Close();

            return ret;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public static string Execute_Transaction(string query1, string query2)
    {
        try
        {

            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(query1, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, con, trans);
                cmd.ExecuteNonQuery();
                trans.Commit();
                return "1";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message.ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public static string Execute_Transaction(string query1, string query2, string query3)
    {
        try
        {

            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(query1, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query3, con, trans);
                cmd.ExecuteNonQuery();
                trans.Commit();
                return "1";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message.ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public static string Execute_Transaction(string query1, string query2, string query3, string query4)
    {
        try
        {

            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(query1, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query3, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query4, con, trans);
                cmd.ExecuteNonQuery();
                trans.Commit();
                return "1";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message.ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public static string Execute_Transaction(string query1, string query2, string query3, string query4, string query5)
    {
        try
        {

            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(query1, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query3, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query4, con, trans);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query5, con, trans);
                cmd.ExecuteNonQuery();
                trans.Commit();
                return "1";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message.ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public static DataSet get_DataSet(string query)
    {
        try
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error occoured while retriving data from database" + ex.Message.ToString());
        }
    }

    public static DataSet get_DataSet(string query, string tablename, int curr, int size)
    {
        SqlConnection con = new SqlConnection(ConnectionString);
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(query, con);
        DataSet ds = new DataSet();
        da.Fill(ds, (curr - 1) * size, size, tablename);
        //dr.Close();
        con.Close();
        return ds;
    }
    public static int Dashboard(string columns, string colvalue, string userid)
    {
        try
        {
            string query = "update tbl.ttlUserDashboard set " + columns + "+='" + colvalue + "' where Userid='" + userid + "'";
            string query1 = "update tbl.ttlUserDashboard set " + columns + "+='" + colvalue + "'where Userid='1'";
            string ret = Database.Execute_Transaction(query, query1);
            if (ret == "1")
            {
                return 1;
            }
            return 1;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

}
