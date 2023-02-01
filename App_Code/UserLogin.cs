using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
public class UserLogin
{
    public UserLogin()
    {

    }
    public static string Check(string userid)
    {
        string query = "Select LogTime,LogOut From Admin.Employee Where UserId='" + userid.ToString() + "'";
        DataSet ds = Database.get_DataSet(query);
        DataRow dr = ds.Tables[0].Rows[0];
        string logintime = dr["LogTime"].ToString() + "AM";
        string logouttime = dr["LogOut"].ToString() + "PM";
        DateTime currenttime = DateTime.Now;
        currenttime = Convert.ToDateTime(currenttime.ToShortTimeString());
        string res;
        DateTime logintime1 = Convert.ToDateTime(logintime);
        logintime1 = Convert.ToDateTime(logintime1.ToShortTimeString());
        DateTime logout = Convert.ToDateTime(logouttime);
        logout = Convert.ToDateTime(logout.ToShortTimeString());
        if (currenttime >= logintime1 && logout >= currenttime)
        {
            res="1";
        }
        else
        {
            res="0";
        }
        return res;
    }
}