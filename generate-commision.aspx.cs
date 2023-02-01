using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class generate_commision : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGenerateCommision_Click(object sender, EventArgs e)
    {
        try
        {
            String query = String.Empty;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            query = "SELECT * FROM tblGenerateCommission WHERE inYears='" + ddYear.SelectedValue + "' AND inMonths='" + ddMonth.SelectedValue + "'";
            DataSet ds = Database.get_DataSet(query);
            if (ds.Tables[0].Rows.Count == 0)
            {
                query = "SELECT CP.inReferalId,CP.inVendorId,(O.SUserid)AS inShopId,CP.flgIsGST," +
                    " SUM(O.PATABLEAMOUNT)AS dcRevenue, SUM(O.GST) AS dcGST,SUM((O.PATABLEAMOUNT) - (O.GST))AS dcComissionedRevenue," +
                    " (CP.inCommisionPercent) AS dcComissionPercentage, SUM(((O.PATABLEAMOUNT) - (O.GST)) * (CP.inCommisionPercent) / 100) AS dcComissionAmount " +
                    " FROM tbl.CompanyProfile CP JOIN tbl.Orders O ON O.SUserid = CP.userId WHERE CP.inROLE = 2 AND MONTH(o.orderDate)= " + ddMonth.SelectedValue + " " +
                    " AND YEAR(O.orderDate)= " + ddYear.SelectedValue + " GROUP BY O.SUserid,CP.inVendorId,CP.flgIsGST,CP.inCommisionPercent,CP.inReferalId";
                DataSet loDsShopUser = Database.get_DataSet(query);
                if (loDsShopUser.Tables[0].Rows.Count > 0)
                {
                    query = "INSERT INTO tblGenerateCommission(inYears, inMonths, dtCreatedDate)" +
                             "VALUES('" + ddYear.SelectedValue
                             + "', '" + ddMonth.SelectedValue
                             + "', '" + DateTime.Now.ToString() + "')";

                    query += " declare @inGenerateCommissionId bigint select @inGenerateCommissionId=IDENT_CURRENT('tblGenerateCommission')";


                    foreach (DataRow dr in loDsShopUser.Tables[0].Rows)
                    {
                        query += "INSERT INTO tblUserWiseCommissionReport(inGenerateCommissionId,inYears,inMonths,inReferral,inVendorId,inShopId," +
                            "dcRevenue,dcGST,dcComissionedRevenue,dcComissionPercentage,dcComissionAmount,inPaymentStatus,stRemark,dtCreatedDate)" +
                            "VALUES(@inGenerateCommissionId, '" + ddYear.SelectedValue
                            + "', '" + ddMonth.SelectedValue
                            + "', '" + dr["inReferalId"]
                            + "', '" + dr["inVendorId"]
                            + "', '" + dr["inShopId"]
                            + "', '" + dr["dcRevenue"]
                            + "', '" + dr["dcGST"]
                            + "', '" + dr["dcComissionedRevenue"]
                            + "', '" + dr["dcComissionPercentage"]
                            + "', '" + dr["dcComissionAmount"]
                            + "', '1', 'Commission Calculation', '" + dateTime.ToString() + "')";

                        // inPaymentStatus =  1 is Pending
                    }
                    string result = Database.Execute(query);
                    if (result == "1")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Commision Genearted Successfully!')", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already generated commission for the selected year and month.')", true);
            }

        }
        catch (Exception ex)
        {

        }
    }
}