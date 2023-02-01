using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class download_gst_report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["fd"] != "" && Request.QueryString["fd"] != null)
        {
            string query_where = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                if (Request.QueryString["fd"] != "" || Request.QueryString["td"] != "")
                {
                    if (Request.QueryString["ps"] == "0")
                    {
                        query_where = " where convert(date,o.orderDate) between '" + Request.QueryString["fd"] + "' and '" + Request.QueryString["td"] + "'";
                    }
                    else
                    {
                        query_where = " where convert(date,o.orderDate) between '" + Request.QueryString["fd"]
                            + "' and '" + Request.QueryString["td"] + "' and o.status='" + Request.QueryString["ps"] + "'";
                    }
                }
                using (SqlCommand cmd = new SqlCommand("SELECT (o.orderId) AS ORDERID,(o.invoiceNo) AS INVOICENO,(c.companyName)AS SHOPNAME," +
                    "(o.orderDate) AS ORDERDATE,(o.Status) AS PAYMENTSTATUS,(o.paymentMode)AS PAYMENTMODE,(o.AMOUNT) AS AMOUNT,(o.GST)AS GST," +
                    "(o.DISCOUNTAMOUNT)AS DISCOUNT,(o.SUBTOTAL)AS PAYABLEAMOUNT FROM tbl.Orders o join tbl.profile p " +
                    "on p.userid = o.cuserid " +
                    "join tbl.CompanyProfile c on o.SUserid = c.Userid " + query_where + " and o.SUserid ='" + Request.QueryString["id"] + "' order by o.orderId asc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "GST Report");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=GST-Report.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}