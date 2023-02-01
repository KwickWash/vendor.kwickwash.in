using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Globalization;

public partial class commission_rpt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getVendorList();
        }
        if (Request.QueryString["m"] != null && Request.QueryString["m"] != "0" && Request.QueryString["y"] != null && Request.QueryString["y"] != "0")
        {
            int month = DateTime.ParseExact(Request.QueryString["m"].ToString(), "MMMM", CultureInfo.CurrentCulture).Month;
            getCommission(month, Convert.ToInt32(Request.QueryString["y"]));
        }
    }

    public void getVendorList()
    {
        try
        {
            ddVendor.Items.Clear();
            ddVendor.Items.Add(new ListItem("---Select Vendor---", "0"));
            string query = "select userId,companyName from tbl.CompanyProfile where inRole=0";
            DataSet ds = Database.get_DataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    ddVendor.Items.Add(new ListItem(dr["companyName"].ToString(), dr["userId"].ToString()));
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    public void getShopList()
    {
        try
        {
            ddShopName.Items.Clear();
            ddShopName.Items.Add(new ListItem("---Select Shop---", "0"));
            string query = "select userId,companyName from tbl.CompanyProfile where inVendorId='" + ddVendor.SelectedValue + "'";
            DataSet ds = Database.get_DataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ddShopName.Items.Add(new ListItem(dr["companyName"].ToString(), dr["userId"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_Commission_Click(object sender, EventArgs e)
    {
        getCommission(Convert.ToInt32(ddMonth.SelectedValue), Convert.ToInt32(ddYear.SelectedValue));
    }

    public void getCommission(int inMonth, int inYear)
    {
        try
        {
            
            int inRowCount = 0;

            string lstFilter = string.Empty;

            if(ddVendor.SelectedValue != "0")
            {
                lstFilter += " AND UWC.inVendorId = '" + ddVendor.SelectedValue + "' ";
            }

            if (ddShopName.SelectedValue != "0" && ddShopName.SelectedValue != "") 
            {
                lstFilter += " AND UWC.inShopId = '" + ddShopName.SelectedValue + "' ";
            }

            if(inMonth !=0)
            {
                lstFilter += " AND inMonths = '" + inMonth + "'";
            }

            if (inYear != 0)
            {
                lstFilter += " AND inYears = '" + inYear + "' ";
            }

            string query = "SELECT inYears, inMonths, (cp.companyName) AS vendor, (cp1.companyName) shop, (dcRevenue)AS Revenue, (dcComissionPercentage)AS ComissionPercentage," +
                " CAST(dcComissionWithOutAmount AS DECIMAL(7, 2)) AS Commission " +
                " FROM tblUserWiseCommissionReport UWC" +
                " JOIN tbl.CompanyProfile CP ON CP.userid = UWC.inVendorId " +
                " JOIN tbl.CompanyProfile CP1 ON CP1.userId = UWC.inShopId" +
                " WHERE 1=1  " + lstFilter + " " +
                " ORDER BY inGenerateCommissionId DESC ";
            DataSet ds = Database.get_DataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    inRowCount++;
                    sb.Append("<tr>");
                    sb.Append("<td>" + inRowCount.ToString() + "</td>");
                    DateTime date = new DateTime(Convert.ToInt32(dr["inYears"]), Convert.ToInt32(dr["inMonths"]), 1);
                    sb.Append("<td>" + date.ToString("MMMM") + " / " + dr["inYears"] + "</td>");
                    sb.Append("<td>" + dr["vendor"].ToString() + "</td>");
                    sb.Append("<td>" + dr["shop"].ToString() + "</td>");
                    sb.Append("<td>" + dr["Revenue"].ToString() + "</td>");
                    sb.Append("<td>-</td>");
                    sb.Append("<td>" + dr["ComissionPercentage"].ToString() + "</td>");
                    sb.Append("<td>" + dr["Commission"].ToString() + "</td>");

                    
                    sb.Append("<td>-</td>");
                    sb.Append("<td>-</td>");
                    sb.Append("</tr>");
                }
                trCommisionList.InnerHtml = sb.ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        getShopList();
    }
}