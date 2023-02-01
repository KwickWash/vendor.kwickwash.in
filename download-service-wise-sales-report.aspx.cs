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

public partial class download_service_wise_sales_report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null && Request.QueryString["sn"] != null && Request.QueryString["fd"] != "" && Request.QueryString["td"] != null)
        {
            string query_where = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select (oi.ServiceName+' - '+oi.ProductName) AS Description,(cast(cast(oi.totalQty as float) as nvarchar(50))+' '+'PSC')AS Counts," +
                    "(oi.price) As UnitPrice,(oi.orderQty) As Qty,(oi.Unit) as Unit, (cast(oi.orderQty as float) * cast(oi.price as float)) As TotalPrice" +
                    " from tbl.OrderItems oi join tbl.product p on oi.proId = p.proId join tbl.service s" +
                    " on oi.SrId = s.SrId join tbl.Orders o  on o.orderId = oi.orderId where oi.suserid = '" + Request.QueryString["id"].ToString() + "' and oi.srId = '" + Request.QueryString["sn"] + "'" +
                    " and convert(date, o.orderDate) between '" + Request.QueryString["fd"] + "' and '" + Request.QueryString["td"] + "'";
                using (SqlCommand cmd = new SqlCommand(query))
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
                                wb.Worksheets.Add(dt, "Service Wise Sales Report");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=Service-Wise-Sales-Report.xlsx");
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