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

/// <summary>
/// Summary description for Users
/// </summary>
public class Users
{
    public Users()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string _UserID;
  
    public string UserID
    {
        get { return _UserID; }
        set { _UserID = value; }
    }
   
    private string _UserName;

    public string UserName
    {
        get { return _UserName; }
        set { _UserName = value; }
    }
    private string _Email;

    public string Email
    {
        get { return _Email; }
        set { _Email = value; }
    }

    private string _ProfileName;

    public string ProfileName
    {
        get { return _ProfileName; }
        set { _ProfileName = value; }
    }
    private string _Role;

    public string Role
    {
        get { return _Role; }
        set { _Role = value; }
    }

    private string _bId;
    public string bId
    {
        get { return _bId; }
        set { _bId = value; }
    }

    private string _ZId;
    public string ZId
    {
        get { return _ZId; }
        set { _ZId = value; }
    }

    private string _DId;
    public string DId
    {
        get { return _DId; }
        set { _DId = value; }
    }
    public static DataTable ValidateUser(string UserName, string Password)
    {
        DataTable DT = Database.get_DataTable("select * from tbl.[Login] where USERNAME='" + UserName + "' and password='" + Password + "' AND ROLE='5'");
        if (DT.Rows.Count > 0)
        {
            DataRow DR = DT.Rows[0];
            if (!(UserName.Equals(DR["USERNAME"].ToString()) && Password.Equals(DR["password"].ToString())))
            {
                DT.Clear();
            }
        }
        return DT;
    }

    public static DataTable ValidateUser1(string UserName, string Password)
    {
        DataTable DT = Database.get_DataTable("select * from tbl.[Login1] where username='" + UserName + "' and password='" + Password + "' and status='1'");
        //DataTable DT = Database.get_DataTable("select * from tbl.[Login1] where username='" + UserName + "' and password='" + Password + "' AND ROLE='1' and status='1'");
        if (DT.Rows.Count > 0)
        {
            DataRow DR = DT.Rows[0];
            if (!(UserName.Equals(DR["username"].ToString()) && Password.Equals(DR["password"].ToString())))
            {
                DT.Clear();
            }
        }
        return DT;
    }
    public static Users ActiveUser
    {
        get { return (Users)HttpContext.Current.Session["ActiveUser"];}
        set { HttpContext.Current.Session["ActiveUser"] = value; }
    }
}
