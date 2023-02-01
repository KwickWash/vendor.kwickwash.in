using System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for apiService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class apiService : System.Web.Services.WebService
{
    public apiService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    public string login(string username, string password)
    {
        try
        {
            if (username.ToString() == "" || password.ToString() == "")
            {
                return "Confirm your e-mail ID  and Password and try again.";
            }
            else
            {
                DataTable DT = Users.ValidateUser(username.ToString(), password.ToString());
                if (DT.Rows.Count > 0)
                {
                    DataRow DR = DT.Rows[0];
                    HttpContext.Current.Session["ActiveUser"] = DR["Userid"].ToString();
                    string query_login = "update tbl.login set status=1 where userid='" + DR["Userid"].ToString() + "'";
                    string res = Database.Execute(query_login);
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }

    [WebMethod(EnableSession = true)]
    public int SessionLogout()
    {
        if (HttpContext.Current.Session["ActiveUser"] != null)
        {
            HttpContext.Current.Session.Clear();
        }
        return 0;
    }

    [WebMethod(EnableSession = true)]
    public int SessionTimeout()
    {
        if (HttpContext.Current.Session["ActiveUser"] != null)
        {
            int ret = Convert.ToInt32(HttpContext.Current.Session["ActiveUser"].ToString());
            return ret;
        }
        return 0;
    }

    [WebMethod(EnableSession = true)]
    public string isValid(int id, string tbName, string cName, string cName1)
    {
        string ret = "0";
        string query = "update " + tbName + " set " + cName1 + "=~" + cName1 + " where " + cName + "='" + id.ToString() + "'";
        ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string isHold(int id)
    {
        string ret = "0";
        string query = "update tbl.CompanyProfile set isStatus=~isStatus where userid='" + id.ToString() + "'";
        ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string setService(int id)
    {
        string queryIns = "";
        string queryService = "select srid,serviceName,serviceDescription,serviceImg from  tbl.Service where srid='" + id + "'";
        DataSet dsSer = Database.get_DataSet(queryService);
        if (dsSer.Tables[0].Rows.Count > 0)
        {
            queryIns = "insert into tbl.MyService(srid,serviceName,serviceDescription,serviceImg,userid) values ('" + dsSer.Tables[0].Rows[0]["srid"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["serviceName"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["serviceDescription"].ToString() + "','" + dsSer.Tables[0].Rows[0]["serviceImg"].ToString()
                + "','" + HttpContext.Current.Session["ActiveUser"].ToString() + "')";
        }
        string ret = Database.Execute(queryIns);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string SetProduct(int id)
    {
        string queryIns = "";
        string queryProduct = "select proid,srId,serviceName,productName,unit,price,proImg,status,productCode,dropOffPrice,pickupDropPrice,productQty,minOrder from tbl.product  where proid='" + id + "'";
        DataSet dsSer = Database.get_DataSet(queryProduct);
        if (dsSer.Tables[0].Rows.Count > 0)
        {
            queryIns = "insert into tbl.myproduct(srId,serviceName,productName,unit,price,proImg,userid,productCode,dropOffPrice,pickupDropPrice,productQty,minOrder,proId) values ('" + dsSer.Tables[0].Rows[0]["srId"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["serviceName"].ToString() + "','" + dsSer.Tables[0].Rows[0]["productName"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["unit"].ToString() + "','" + dsSer.Tables[0].Rows[0]["price"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["proImg"].ToString() + "','" + HttpContext.Current.Session["ActiveUser"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["productCode"].ToString() + "','" + dsSer.Tables[0].Rows[0]["dropOffPrice"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["pickupDropPrice"].ToString() + "','" + dsSer.Tables[0].Rows[0]["productQty"].ToString()
                + "','" + dsSer.Tables[0].Rows[0]["minOrder"].ToString() + "','" + dsSer.Tables[0].Rows[0]["proid"].ToString() + "')";
        }
        string ret = Database.Execute(queryIns);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string SetProductPrice(int id, string productPrice, string productPrice1)
    {
        string query = "update tbl.myproduct set myPrice='" + productPrice + "',myPrice1='" + productPrice1 + "' where prodId='" + id
            + "' and UserId='" + HttpContext.Current.Session["ActiveUser"].ToString() + "'";
        string ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string SetPaymentStatus(int id, string PaymentMode)
    {
        string query = "update tbl.orders set paymentMode='" + PaymentMode + "',Status='Paid' where orderId='" + id + "'";
        string ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string CustomerPayment(int CustomerId, string CustomerName, string InvoiceNo, int OrderNo, int ShopId, decimal Balance, string PaymentMode, decimal ReceiveAmount, decimal PayableAmt, string Mobile)
    {
        decimal TotalBalace = Balance + ReceiveAmount;
        decimal AvaialbeBalance = TotalBalace - PayableAmt;

        string query = "update tbl.Profile set Balance='" + AvaialbeBalance + "' where userId='" + CustomerId + "'";
        string queryCustomerLedger = "INSERT INTO tblCustomerLedger (CustomerId,ShopId,OrderNo,EntryDate,Decsription,DebiteAmount,CreditAmount,Balance) VALUES " +
            " ('" + CustomerId + "','" + ShopId + "','" + OrderNo + "','" + DateTime.Now.ToShortDateString() + "','','" + PayableAmt
            + "','" + ReceiveAmount + "','" + AvaialbeBalance + "')";
        string queryOrder = "UPDATE tbl.Orders SET Status='Paid', paymentMode='" + PaymentMode + "' WHERE orderId='" + OrderNo + "'";
        string ret = Database.Execute_Transaction(query, queryCustomerLedger, queryOrder);
        if (ret == "1")
        {
            string query_SMS = "select flgIsSMS from tbl.profile where userid='" + CustomerId + "'";
            DataSet ds_SMS = Database.get_DataSet(query_SMS);
            if (ds_SMS.Tables[0].Rows[0]["flgIsSMS"].ToString() == "1" || ds_SMS.Tables[0].Rows[0]["flgIsSMS"].ToString() == "True")
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                string sendSMSAPI = "http://m1.sarv.com/api/v2.0/sms_campaign.php?token=705915705611ff6c4c5f9d8.90022694&user_id=63515991&route=TR&template_id=6742&sender_id=KWwash&language=EN&template=Dear+Qutub+your+payment+for+Order+No+1023%2C+Invoice+No.+19209%2C+amounting+Rs.+2022.+Is+Successfully+completed+by+Cash.+Thank+you+for+choosing+Kwickwash.+Visit+Again+www.kwickwash.in&contact_numbers=7277527789";
                sendSMSAPI = sendSMSAPI.Replace("1023", OrderNo.ToString());
                sendSMSAPI = sendSMSAPI.Replace("Qutub", CustomerName.ToString());
                sendSMSAPI = sendSMSAPI.Replace("7277527789", Mobile);
                sendSMSAPI = sendSMSAPI.Replace("Cash", PaymentMode);
                sendSMSAPI = sendSMSAPI.Replace("19209", InvoiceNo);
                sendSMSAPI = sendSMSAPI.Replace("2022", PayableAmt.ToString());
                WebRequest request = HttpWebRequest.Create(sendSMSAPI);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();
            }
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string SetDeliveryStatus(int id, string CustomerName, string InvoiceNo, int OrderNo, decimal PayableAmt, string Mobile, string CustomerUserId)
    {
        string query = "update tbl.orders set deliveryStatus='Delivered',deliveryDate='" + DateTime.Now.ToString() + "' where orderId='" + id + "'";
        string ret = Database.Execute(query);
        if (ret == "1")
        {
            string query_SMS = "select flgIsSMS from tbl.profile where userid='" + CustomerUserId + "'";
            DataSet ds_SMS = Database.get_DataSet(query_SMS);
            if (ds_SMS.Tables[0].Rows[0]["flgIsSMS"].ToString() == "1" || ds_SMS.Tables[0].Rows[0]["flgIsSMS"].ToString() == "True")
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                string sendSMSAPI = "http://m1.sarv.com/api/v2.0/sms_campaign.php?token=705915705611ff6c4c5f9d8.90022694&user_id=63515991&route=TR&template_id=6737&sender_id=KWwash&language=EN&template=Dear+Qutub+your+delivery+for+Order+No+1023%2C+Invoice+No.+19209%2C+amounting+Rs.+2022.+Is+Successfully+completed.+Thank+you+for+choosing+Kwickwash.+Visit+Again+www.kwickwash.in&contact_numbers=7277527789";
                sendSMSAPI = sendSMSAPI.Replace("1023", OrderNo.ToString());
                sendSMSAPI = sendSMSAPI.Replace("Qutub", CustomerName.ToString());
                sendSMSAPI = sendSMSAPI.Replace("7277527789", Mobile);
                sendSMSAPI = sendSMSAPI.Replace("19209", InvoiceNo);
                sendSMSAPI = sendSMSAPI.Replace("2022", PayableAmt.ToString());
                WebRequest request = HttpWebRequest.Create(sendSMSAPI);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();
            }
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string SetSMS(string id)
    {
        string query = "update tbl.profile set flgIsSMS=~flgIsSMS where profileId='" + id + "'";
        string ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string SetOrderStatus(int OrderNo, string OrderStatus, string DeliveryDate, string OrderDate, string Mobile, string CustomerName, string CustomerUserId)
    {
        string sendSMSAPI = string.Empty;
        string query = "update tbl.orders set deliveryStatus='" + OrderStatus + "' where orderId='" + OrderNo + "'";
        string ret = Database.Execute(query);
        if (ret == "1")
        {
            string query_SMS = "select flgIsSMS from tbl.profile where userid='" + CustomerUserId + "'";
            DataSet ds_SMS = Database.get_DataSet(query_SMS);
            if (ds_SMS.Tables[0].Rows[0]["flgIsSMS"].ToString() == "1" || ds_SMS.Tables[0].Rows[0]["flgIsSMS"].ToString() == "True")
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                if (OrderStatus == "InProcess")
                {
                    sendSMSAPI = "http://m1.sarv.com/api/v2.0/sms_campaign.php?token=705915705611ff6c4c5f9d8.90022694&user_id=63515991&route=TR&template_id=6743&sender_id=KWwash&language=EN&template=Dear+CCCC+your+Order+No+OOOO+Dated+DDDD+Is+being+processed+and+will+be+ready+by+RRRR+stay+updated+after+login+in+App+or+Website+at+www.kwickwash.in&contact_numbers=7277527789";
                    sendSMSAPI = sendSMSAPI.Replace("OOOO", OrderNo.ToString());
                    sendSMSAPI = sendSMSAPI.Replace("CCCC", CustomerName);
                    sendSMSAPI = sendSMSAPI.Replace("7277527789", Mobile);
                    sendSMSAPI = sendSMSAPI.Replace("RRRR", DeliveryDate);
                    sendSMSAPI = sendSMSAPI.Replace("DDDD", OrderDate);

                }
                else if (OrderStatus == "ReadyForDelivery")
                {
                    sendSMSAPI = "http://m1.sarv.com/api/v2.0/sms_campaign.php?token=705915705611ff6c4c5f9d8.90022694&user_id=63515991&route=TR&template_id=6743&sender_id=KWwash&language=EN&template=Dear+CCCC+your+Order+No+OOOO+Dated+DDDD+Is+being+processed+and+will+be+ready+by+RRRR+stay+updated+after+login+in+App+or+Website+at+www.kwickwash.in&contact_numbers=7277527789";
                    sendSMSAPI = sendSMSAPI.Replace("OOOO", OrderNo.ToString());
                    sendSMSAPI = sendSMSAPI.Replace("CCCC", CustomerName);
                    sendSMSAPI = sendSMSAPI.Replace("7277527789", Mobile);
                    sendSMSAPI = sendSMSAPI.Replace("RRRR", DeliveryDate);
                    sendSMSAPI = sendSMSAPI.Replace("DDDD", OrderDate);
                }

                WebRequest request = HttpWebRequest.Create(sendSMSAPI);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();
            }
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string setDriver(string koId, string did, string dname)
    {
        string ret = "0";
        try
        {
            string query = "update tbl.kwickOrder set did='" + did + "',dname='" + dname + "' where koId='" + koId + "' ";
            ret = Database.Execute(query);
            if (ret == "1")
            {
                ret = "1";
            }
            else
            {
                ret = "0";
            }
        }
        catch (Exception ex)
        {
            ret = "00";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string OrderDlt(int oid)
    {
        string ret = "0";

        string query_get = "select * from tbl.orders where  orderid='" + oid + "'";
        DataSet ds_get = Database.get_DataSet(query_get);
        if (ds_get.Tables[0].Rows.Count > 0)
        {
            string query = "delete tbl.orders where orderid='" + oid + "' delete tbl.OrderItems where orderid = '" + oid 
                + "'  delete tblCustomerLedgers where OrderNo='" + oid + "'";

            string query_get_amt = "select debiteAmount,CustomerId from tblCustomerLedgers where OrderNo='" + oid + "'";
            DataSet ds_amt_get = Database.get_DataSet(query_get_amt);
            if (ds_amt_get.Tables[0].Rows.Count > 0)
            {
                string query_profile = "update tbl.Profile set balance=balance-(" + ds_amt_get.Tables[0].Rows[0]["debiteAmount"].ToString().Replace("-","").ToString() 
                    + ") where userid=" + ds_amt_get.Tables[0].Rows[0]["CustomerId"] + "";
                ret = Database.Execute_Transaction(query, query_profile);
                if (ret == "1")
                {
                    ret = "1";
                }
            }
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string UpdatePassword(int id, string newpassword)
    {
        string ret = "0";
        string query = "update tbl.login set password='" + newpassword + "' where userid='" + id + "'";
        ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string OrderReqDlt(int rid)
    {
        string ret = "0";

        string query = "delete tbl.kwickOrder where koid='" + rid + "'";
        string res = Database.Execute(query);
        ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string PayNow(int orderId, string pmode)
    {
        string ret = "0";

        string query = "update tbl.Orders set Status='Paid',paymentMode='" + pmode + "' where orderId='" + orderId + "'";
        string res = Database.Execute(query);
        if (res == "1")
        {
            //int rest = Database.Dashboard("ttlDeliveryPending", "1", HttpContext.Current.Session["ActiveUser"].ToString());
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string Delivered(int orderId)
    {
        string ret = "0";

        string query = "update tbl.Orders set deliveryStatus='Delivered', deliveryDate='" + DateTime.Now.ToString() + "' where orderId='" + orderId + "'";
        string res = Database.Execute(query);
        if (res == "1")
        {
            query = "update tbl.ttlUserDashboard set ttlProcess-= '1',ttlProcessPending-='1',ttlProcessCompleted-='1',ttlDelivery+='1'" +
                " ,ttlDeliveryPending-='1',ttlDeliveryCompleted+='1' where Userid='" + HttpContext.Current.Session["ActiveUser"].ToString() + "'";

            string query1 = "update tbl.ttlUserDashboard set ttlProcess-= '1',ttlProcessPending-='1',ttlProcessCompleted-='1',ttlDelivery+='1'" +
               " ,ttlDeliveryPending-='1',ttlDeliveryCompleted+='1' where Userid='1'";
            ret = Database.Execute_Transaction(query, query1);
            ret = "1";
        }
        return ret.ToString();
    }

    [WebMethod(EnableSession = true)]
    public string getOrderReq(string id)
    {
        string ret = "0";

        string query = "select count(*)as TotalCtr from tbl.kwickOrder where shopid='" + id + "' and dname is null";
        DataSet ds = Database.get_DataSet(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ret = ds.Tables[0].Rows[0]["TotalCtr"].ToString();
        }
        return ret.ToString();
    }
    public class OrderList
    {
        public string NAME { get; set; }
        public string ORDERID;
        public string INVOICENO;
        public string TTLAMOUNT;
        public string TTLDISCOUNT;
        public string TTLPAYABLEAMOUNT;
        public string STATUS;
        public string ORDERDATE;
        public string DELIVERYDATE;
        public string ORDERTYPE;
        public string DROPREQUEST;
        public string PICKUPSLIP;
        public string PAYMENTMODE;
        public string DELIVERYSTATUS;
    }

    [WebMethod]
    public List<OrderList> GetOrderList1(string mobileOrder, string status, int id)
    {
        List<OrderList> list = new List<OrderList>();
        try
        {
            string query = "";
            if (status == "0")
            {
                query = "SELECT O.SUSERID,U.NAME,O.ORDERID,O.INVOICENO,O.TTLAMOUNT,O.TTLDISCOUNT,O.TTLPAYABLEAMOUNT,O.STATUS,O.ORDERDATE,O.DELIVERYDATE,O.ORDERTYPE," +
                    " O.DROPREQUEST,O.PICKUPSLIP,O.PAYMENTMODE,O.DELIVERYSTATUS FROM tbl.orders O LEFT JOIN tbl.PROFILE U ON O.CUSERID = U.USERID " +
                    " WHERE O.SUSERID='" + id + "' AND CAST(U.NAME AS nvarchar(100)) LIKE'%" + mobileOrder + "%' OR " +
                    " CAST(U.MOBILE AS nvarchar(100))='" + mobileOrder + "' OR CAST(O.ORDERID AS nvarchar(100))='" + mobileOrder + "' OR O.INVOICENO='" + mobileOrder + "' " +
                    " ORDER BY O.ORDERDATE DESC";
            }
            else
            {
                query = "SELECT O.SUSERID,U.NAME,O.ORDERID,O.INVOICENO,O.TTLAMOUNT,O.TTLDISCOUNT,O.TTLPAYABLEAMOUNT,O.STATUS,O.ORDERDATE,O.DELIVERYDATE,O.ORDERTYPE," +
                    " O.DROPREQUEST,O.PICKUPSLIP,O.PAYMENTMODE,O.DELIVERYSTATUS FROM tbl.orders O LEFT JOIN tbl.PROFILE U ON O.CUSERID = U.USERID " +
                    " WHERE O.SUSERID='" + id + "' AND O.STATUS='" + status + "' AND CAST(U.NAME AS nvarchar(100)) LIKE'%" + mobileOrder + "%' OR " +
                    " CAST(U.MOBILE AS nvarchar(100))='" + mobileOrder + "' OR CAST(O.ORDERID AS nvarchar(100))='" + mobileOrder + "' OR O.INVOICENO='" + mobileOrder + "' " +
                    " ORDER BY O.ORDERDATE DESC";

            }
            DataSet ds = Database.get_DataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OrderList data = new OrderList();
                    data.NAME = dr["NAME"].ToString();
                    data.ORDERID = dr["ORDERID"].ToString();
                    data.INVOICENO = dr["INVOICENO"].ToString();
                    data.TTLAMOUNT = dr["TTLAMOUNT"].ToString();
                    data.TTLDISCOUNT = dr["TTLDISCOUNT"].ToString();
                    data.TTLPAYABLEAMOUNT = dr["TTLPAYABLEAMOUNT"].ToString();
                    data.STATUS = dr["STATUS"].ToString();
                    DateTime dtORDERDATE = Convert.ToDateTime(dr["ORDERDATE"]);
                    data.ORDERDATE = dtORDERDATE.ToString("dd-MMM-yyyy").ToString();
                    DateTime dtDELIVERYDATE = Convert.ToDateTime(dr["DELIVERYDATE"]);
                    data.DELIVERYDATE = dtDELIVERYDATE.ToString("dd-MMM-yyyy").ToString();
                    data.ORDERTYPE = dr["ORDERTYPE"].ToString();
                    data.DROPREQUEST = dr["DROPREQUEST"].ToString();
                    data.PICKUPSLIP = dr["PICKUPSLIP"].ToString();
                    data.PAYMENTMODE = dr["PAYMENTMODE"].ToString();
                    data.DELIVERYSTATUS = dr["DELIVERYSTATUS"].ToString();
                    list.Add(data);
                }
            }
        }
        catch { }
        return list;
    }
    public class ShopOrderList
    {
        public string orderId { get; set; }
        public string invoiceNo { get; set; }
        public string ttlQty { get; set; }
        public string ttlPayableAmount { get; set; }
        public string Status { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string orderDate { get; set; }
        public string deliveryDate { get; set; }
        public string paging { get; set; }
        public decimal? payableamt { get; set; }

    }

    [WebMethod]
    public List<ShopOrderList> GetOrderList(int id, int pageno, string filter)
    {
        string query = "";
        int curr = pageno;
        string pageing = "";
        if (filter == "")
        {
            if (id == 2197)
            {
                query = "select o.PATABLEAMOUNT,o.ttlPayableAmount,o.orderId,p.name,p.mobile,o.orderDate,o.deliveryDate,o.Status from tbl.Orders o join tbl.profile p on p.userid = o.cuserid where o.SUserid='" + id + "' order by o.orderId desc";
            }
            else
            {
                query = "select (o.ttlPayableAmount) AS PATABLEAMOUNT,o.ttlPayableAmount,o.orderId,p.name,p.mobile,o.orderDate,o.deliveryDate,o.Status from tbl.Orders o join tbl.profile p on p.userid = o.cuserid where o.SUserid='" + id + "' order by o.orderId desc";
            }
        }
        else
        {
            if (id == 2197)
            {
                query = "select o.PATABLEAMOUNT,o.ttlPayableAmount,o.orderId,p.name,p.mobile,o.orderDate,o.deliveryDate,o.Status from tbl.Orders o join tbl.profile p on p.userid = o.cuserid where o.SUserid='" + id + "' and p.name like '%" + filter + "%'  or CAST(p.mobile AS nvarchar(100))='" + filter + "'  order by o.orderId desc";
            }
            else
            {
                query = "select (o.ttlPayableAmount) AS PATABLEAMOUNT,o.ttlPayableAmount,o.orderId,p.name,p.mobile,o.orderDate,o.deliveryDate,o.Status from tbl.Orders o join tbl.profile p on p.userid = o.cuserid where o.SUserid='" + id + "' and p.name like '%" + filter + "%'  or CAST(p.mobile AS nvarchar(100))='" + filter + "'  order by o.orderId desc";
            }
        }
        DataTable dt = Database.get_DataTable(query);
        DataSet ds = Database.get_DataSet(query, "tbl.Orders", curr, 10);

        List<ShopOrderList> OrderList = new List<ShopOrderList>(ds.Tables[0].Rows.Count);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, dt.Rows.Count, 10, "&");
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ShopOrderList data = new ShopOrderList();
                data.orderId = dr["orderId"].ToString();
                data.Status = dr["Status"].ToString();
                data.name = dr["name"].ToString();
                data.mobile = dr["mobile"].ToString();
                DateTime oDate = Convert.ToDateTime(dr["orderDate"]);
                data.orderDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                oDate = Convert.ToDateTime(dr["deliveryDate"]);
                data.deliveryDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.ttlPayableAmount = dr["ttlPayableAmount"].ToString();
                data.paging = pageing;
                data.payableamt = Convert.ToDecimal(dr["PATABLEAMOUNT"]);
                OrderList.Add(data);
            }
        }
        return OrderList;
    }

    [WebMethod]
    public List<ShopOrderList> GetOrderList2(int id, int pageno, string filter)
    {

        int curr = pageno;
        string pageing = "";
        DataSet loSourceDataDataSet = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getVendorWiseAllOrderList]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = id;
        loCommand.Parameters.Add("@name", SqlDbType.VarChar, 200).Value = filter;
        loCommand.Parameters.Add("@inPageNo", SqlDbType.Int).Value = pageno;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(loSourceDataDataSet, "OutputTable");

        List<ShopOrderList> OrderList = new List<ShopOrderList>(loSourceDataDataSet.Tables[0].Rows.Count);
        if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
        {
            int rowCount = (int)loSourceDataDataSet.Tables[0].Rows[0]["inRecordCount"];
            if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, rowCount, 10, "&");
            }
            foreach (DataRow dr in loSourceDataDataSet.Tables[0].Rows)
            {
                ShopOrderList data = new ShopOrderList();
                data.orderId = dr["orderId"].ToString();
                data.Status = dr["Status"].ToString();
                data.name = dr["name"].ToString();
                data.mobile = dr["mobile"].ToString();
                DateTime oDate = Convert.ToDateTime(dr["orderDate"]);
                data.orderDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                oDate = Convert.ToDateTime(dr["deliveryDate"]);
                data.deliveryDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.ttlPayableAmount = dr["ttlPayableAmount"].ToString();
                data.paging = pageing;
                OrderList.Add(data);
            }
        }
        return OrderList;
    }


    public class ShopOrderStatusList
    {
        public string orderId { get; set; }
        public string orderType { get; set; }
        public string dropRequest { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string orderDate { get; set; }
        public string status { get; set; }
        public string deliveryDate { get; set; }
        public decimal? ttlPayableAmount { get; set; }
        public string paymentMode { get; set; }
        public string deliveryStatus { get; set; }
        public string paging { get; set; }
    }

    [WebMethod]
    public List<ShopOrderStatusList> GetShopOrderStatusList(int id, int pageno, string filter, string status)
    {
        if (status == "null")
            status = "";
        int curr = pageno;
        string pageing = "";
        DataSet loSourceDataDataSet = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getVendorWiseAllOrderStatuList]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = id;
        loCommand.Parameters.Add("@name", SqlDbType.VarChar, 200).Value = filter;
        loCommand.Parameters.Add("@inPageNo", SqlDbType.Int).Value = pageno;
        loCommand.Parameters.Add("@stStatus", SqlDbType.VarChar, 200).Value = status;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(loSourceDataDataSet, "OutputTable");

        List<ShopOrderStatusList> OrderList = new List<ShopOrderStatusList>(loSourceDataDataSet.Tables[0].Rows.Count);
        if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
        {
            int rowCount = (int)loSourceDataDataSet.Tables[0].Rows[0]["inRecordCount"];
            if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, rowCount, 10, "&");
            }
            foreach (DataRow dr in loSourceDataDataSet.Tables[0].Rows)
            {
                ShopOrderStatusList data = new ShopOrderStatusList();
                data.orderId = dr["orderId"].ToString();
                data.orderType = dr["OrderType"].ToString();
                data.dropRequest = dr["dropRequest"].ToString();
                data.name = dr["name"].ToString();
                data.mobile = dr["mobile"].ToString();
                DateTime oDate = Convert.ToDateTime(dr["orderDate"]);
                data.orderDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.status = dr["Status"].ToString();
                oDate = Convert.ToDateTime(dr["deliveryDate"]);
                data.deliveryDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.ttlPayableAmount = Convert.ToDecimal(dr["ttlPayableAmount"]);
                data.paymentMode = dr["paymentMode"].ToString();
                data.deliveryStatus = dr["deliveryStatus"].ToString();
                data.paging = pageing;
                OrderList.Add(data);
            }
        }
        return OrderList;
    }

    [WebMethod]
    public List<ShopOrderStatusList> GetShopOrderHistoryList(int id, int pageno, string filter)
    {

        int curr = pageno;
        string pageing = "";
        DataSet loSourceDataDataSet = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getVendorWiseAllOrderHistoryList]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = null;
        loCommand.Parameters.Add("@name", SqlDbType.VarChar, 200).Value = filter;
        loCommand.Parameters.Add("@inPageNo", SqlDbType.Int).Value = pageno;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(loSourceDataDataSet, "OutputTable");

        List<ShopOrderStatusList> OrderList = new List<ShopOrderStatusList>(loSourceDataDataSet.Tables[0].Rows.Count);
        if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
        {
            int rowCount = (int)loSourceDataDataSet.Tables[0].Rows[0]["inRecordCount"];
            if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, rowCount, 10, "&");
            }
            foreach (DataRow dr in loSourceDataDataSet.Tables[0].Rows)
            {
                ShopOrderStatusList data = new ShopOrderStatusList();
                data.orderId = dr["orderId"].ToString();
                data.orderType = dr["OrderType"].ToString();
                data.dropRequest = dr["dropRequest"].ToString();
                data.name = dr["name"].ToString();
                data.mobile = dr["mobile"].ToString();
                DateTime oDate = Convert.ToDateTime(dr["orderDate"]);
                data.orderDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.status = dr["Status"].ToString();
                oDate = Convert.ToDateTime(dr["deliveryDate"]);
                data.deliveryDate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.ttlPayableAmount = Convert.ToDecimal(dr["ttlPayableAmount"]);
                data.paymentMode = dr["paymentMode"].ToString();
                data.deliveryStatus = dr["deliveryStatus"].ToString();
                data.paging = pageing;
                OrderList.Add(data);
            }
        }
        return OrderList;
    }


    public class CustomerSalesReport
    {
        public int ORDERID { get; set; }
        public string ORDERTYPE { get; set; }
        public string DROPREQUEST { get; set; }
        public string INVOICENO { get; set; }
        public int TTLQTY { get; set; }
        public decimal? PATABLEAMOUNT { get; set; }
        public string ORDERDATE { get; set; }
        public string DELIVERYDATE { get; set; }
        public string STATUS { get; set; }
        public string DELIVERYSTATUS { get; set; }
        public string NAME { get; set; }
        public string EMAILID { get; set; }
        public string ADDRESS { get; set; }
        public string paging { get; set; }
    }

    [WebMethod]
    public List<CustomerSalesReport> GetCustomerSalesReport(int id, int pageno, string filter)
    {

        int curr = pageno;
        string pageing = "";
        DataSet loSourceDataDataSet = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getAdminCustomerWiseSalesList]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@inShopUserId", SqlDbType.Int).Value = id;
        loCommand.Parameters.Add("@stMobile", SqlDbType.VarChar, 200).Value = filter;
        loCommand.Parameters.Add("@inPageNo", SqlDbType.Int).Value = pageno;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(loSourceDataDataSet, "OutputTable");

        List<CustomerSalesReport> OrderList = new List<CustomerSalesReport>(loSourceDataDataSet.Tables[0].Rows.Count);
        if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
        {
            int rowCount = (int)loSourceDataDataSet.Tables[0].Rows[0]["inRecordCount"];
            if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, rowCount, 10, "&");
            }
            foreach (DataRow dr in loSourceDataDataSet.Tables[0].Rows)
            {
                CustomerSalesReport data = new CustomerSalesReport();
                data.ORDERID = Convert.ToInt32(dr["ORDERID"]);
                data.ORDERTYPE = dr["ORDERTYPE"].ToString();
                data.DROPREQUEST = dr["DROPREQUEST"].ToString();
                data.INVOICENO = dr["INVOICENO"].ToString();
                data.TTLQTY = Convert.ToInt32(dr["TTLQTY"]);
                data.PATABLEAMOUNT = Convert.ToDecimal(dr["PATABLEAMOUNT"]);
                DateTime oDate = Convert.ToDateTime(dr["orderDate"]);
                data.ORDERDATE = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                oDate = Convert.ToDateTime(dr["DELIVERYDATE"]);
                data.DELIVERYDATE = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.STATUS = dr["STATUS"].ToString();
                data.DELIVERYSTATUS = dr["DELIVERYSTATUS"].ToString();
                data.NAME = dr["NAME"].ToString();
                data.EMAILID = dr["EMAILID"].ToString();
                data.ADDRESS = dr["ADDRESS"].ToString();
                data.paging = pageing;
                OrderList.Add(data);
            }
        }
        return OrderList;
    }

    public class Profile
    {
        public int profileId { get; set; }
        public string Status { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public string emailId { get; set; }
        public string mobile { get; set; }
        public decimal? balance { get; set; }
        public string flgIsSMS { get; set; }
        public int? ttlorder { get; set; }
        public decimal? ttlorderamt { get; set; }
        public decimal? ttlpaidamt { get; set; }
        public string paging { get; set; }
        public string shopname { get; set; }
    }

    [WebMethod]
    public List<Profile> GetCustomer(int shopUserid, int pageno, string mobile)
    {
        int curr = pageno;
        string pageing = "";
        DataSet loSourceDataDataSet = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getVendorWiseCustomerAllList]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@inShopUserId", SqlDbType.Int).Value = shopUserid;
        loCommand.Parameters.Add("@stMobile", SqlDbType.VarChar, 200).Value = mobile;
        loCommand.Parameters.Add("@inPageNo", SqlDbType.Int).Value = pageno;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(loSourceDataDataSet, "OutputTable");

        List<Profile> ProfileList = new List<Profile>(loSourceDataDataSet.Tables[0].Rows.Count);
        if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
        {
            int rowCount = (int)loSourceDataDataSet.Tables[0].Rows[0]["inRecordCount"];
            if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, rowCount, 10, "&");
            }
            foreach (DataRow dr in loSourceDataDataSet.Tables[0].Rows)
            {
                Profile data = new Profile();
                data.profileId = Convert.ToInt32(dr["profileId"]);
                data.Status = dr["Status"].ToString();
                data.userId = dr["userId"].ToString();
                data.name = dr["name"].ToString();
                data.emailId = dr["emailId"].ToString();
                data.mobile = dr["mobile"].ToString();
                data.balance = Convert.ToDecimal(dr["balance"]);
                data.flgIsSMS = dr["flgIsSMS"].ToString();
                data.shopname = dr["shopname"].ToString();
                if (dr["ttlorder"] != DBNull.Value)
                    data.ttlorder = Convert.ToInt32(dr["ttlorder"]);
                else
                    data.ttlorder = 0;

                if (dr["ttlorderamt"] != DBNull.Value)
                    data.ttlorderamt = Convert.ToDecimal(dr["ttlorderamt"]);
                else
                    data.ttlorderamt = 0;

                if (dr["ttlpaidamt"] != DBNull.Value)
                    data.ttlpaidamt = Convert.ToDecimal(dr["ttlpaidamt"]);
                else
                    data.ttlpaidamt = 0;
                data.paging = pageing;
                ProfileList.Add(data);
            }
        }
        return ProfileList;
    }

    public class CustomerLedgerList
    {
        public string companyName { get; set; }
        public int orderNo { get; set; }
        public int invoiceNo { get; set; }
        public string orderType { get; set; }
        public string dropRequest { get; set; }
        public string entryDate { get; set; }
        public decimal? debiteAmount { get; set; }
        public decimal? creditAmount { get; set; }
        public decimal? balance { get; set; }
        public decimal? ttlorder { get; set; }
        public decimal? ttlorderamt { get; set; }
        public decimal? ttlpaidamt { get; set; }
        public decimal? ttlunpaidamt { get; set; }

    }

    [WebMethod]
    public List<CustomerLedgerList> GetCustomerLedger(int customerId)
    {
        List<CustomerLedgerList> CustomerLedgerList = new List<CustomerLedgerList>();

        DataSet ds = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getCustomerLedgerList]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@inCustomerId", SqlDbType.Int).Value = customerId;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(ds, "OutputTable");

        List<Profile> ProfileList = new List<Profile>(ds.Tables[0].Rows.Count);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CustomerLedgerList CustomerLedger = new CustomerLedgerList();

                CustomerLedger.companyName = dr["companyName"].ToString();
                CustomerLedger.orderNo = Convert.ToInt32(dr["OrderNo"]);
                CustomerLedger.invoiceNo = Convert.ToInt32(dr["invoiceNo"]);
                CustomerLedger.orderType = dr["orderType"].ToString();
                CustomerLedger.dropRequest = dr["dropRequest"].ToString();

                DateTime dtentryDate = Convert.ToDateTime(dr["entryDate"]);
                CustomerLedger.entryDate = dtentryDate.ToString("dd-MMM-yyyy hh:mm").ToString();

                if (dr["debiteAmount"] != DBNull.Value)
                    CustomerLedger.debiteAmount = Convert.ToDecimal(dr["debiteAmount"]);
                else
                    CustomerLedger.debiteAmount = 0;

                if (dr["creditAmount"] != DBNull.Value)
                    CustomerLedger.creditAmount = Convert.ToDecimal(dr["creditAmount"]);
                else
                    CustomerLedger.creditAmount = 0;

                if (dr["balance"] != DBNull.Value)
                    CustomerLedger.balance = Convert.ToDecimal(dr["balance"]);
                else
                    CustomerLedger.balance = 0;

                if (dr["ttlorder"] != DBNull.Value)
                    CustomerLedger.ttlorder = Convert.ToDecimal(dr["ttlorder"]);
                else
                    CustomerLedger.ttlorder = 0;

                if (dr["ttlorderamt"] != DBNull.Value)
                    CustomerLedger.ttlorderamt = Convert.ToDecimal(dr["ttlorderamt"]);
                else
                    CustomerLedger.ttlorderamt = 0;

                if (dr["ttlpaidamt"] != DBNull.Value)
                    CustomerLedger.ttlpaidamt = Convert.ToDecimal(dr["ttlpaidamt"]);
                else
                    CustomerLedger.ttlpaidamt = 0;

                if (dr["ttlunpaidamt"] != DBNull.Value)
                    CustomerLedger.ttlunpaidamt = Convert.ToDecimal(dr["ttlunpaidamt"]);
                else
                    CustomerLedger.ttlunpaidamt = 0;

                CustomerLedgerList.Add(CustomerLedger);
            }
        }
        return CustomerLedgerList;
    }

    public class ShopPaymentReceivedList
    {
        public string clid { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string orderno { get; set; }
        public string entrydate { get; set; }
        public string creditamount { get; set; }
        public string paymentmode { get; set; }
        public string paging { get; set; }
    }

    [WebMethod]
    public List<ShopPaymentReceivedList> GetShopPaymentReceivedLedger(int id, int pageno, string filter)
    {
        int curr = pageno;
        string pageing = "";
        DataSet loSourceDataDataSet = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getAllShopPaymentReceivedLedger]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = null;
        loCommand.Parameters.Add("@name", SqlDbType.VarChar, 200).Value = filter;
        loCommand.Parameters.Add("@inPageNo", SqlDbType.Int).Value = pageno;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(loSourceDataDataSet, "OutputTable");

        List<ShopPaymentReceivedList> OrderList = new List<ShopPaymentReceivedList>(loSourceDataDataSet.Tables[0].Rows.Count);
        if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
        {
            int rowCount = (int)loSourceDataDataSet.Tables[0].Rows[0]["inRecordCount"];
            if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, rowCount, 10, "&");
            }
            foreach (DataRow dr in loSourceDataDataSet.Tables[0].Rows)
            {
                ShopPaymentReceivedList data = new ShopPaymentReceivedList();
                data.clid = dr["clid"].ToString();
                data.name = dr["name"].ToString();
                data.mobile = dr["mobile"].ToString();
                data.orderno = dr["orderno"].ToString();
                DateTime oDate = Convert.ToDateTime(dr["entrydate"]);
                data.entrydate = oDate.ToString("dd-MMM-yyyy hh:mm").ToString();
                data.creditamount = dr["creditamount"].ToString();
                data.paymentmode = dr["paymentmode"].ToString();
                data.paging = pageing;
                OrderList.Add(data);
            }
        }
        return OrderList;
    }

    [WebMethod(EnableSession = true)]
    public string SetArea(int areaId, string shopId)
    {
        string ret = "0";
        string query = "update tbl.area set shopId='" + shopId.ToString() + "', status=~status where areaId='" + areaId.ToString() + "'";
        ret = Database.Execute(query);
        if (ret == "1")
        {
            ret = "1";
        }
        return ret.ToString();
    }


    public class CommissionList
    {
        public int inGenerateCommissionId { get; set; }
        public int inYears { get; set; }
        public string stMonths { get; set; }
        public string paging { get; set; }
    }

    [WebMethod]
    public List<CommissionList> GetCommissionList(int pageno)
    {

        int curr = pageno;
        string pageing = "";
        DataSet loSourceDataDataSet = new DataSet();
        SqlConnection loCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        SqlCommand loCommand = new SqlCommand();
        loCommand.Connection = loCon;
        loCommand.CommandText = "[dbo].[getCommissionList]";
        loCommand.CommandType = CommandType.StoredProcedure;
        loCommand.Parameters.Clear();
        loCommand.Parameters.Add("@inPageNo", SqlDbType.Int).Value = pageno;

        SqlDataAdapter loDataAdapter = new SqlDataAdapter(loCommand);
        loDataAdapter.Fill(loSourceDataDataSet, "OutputTable");

        List<CommissionList> CommissionList = new List<CommissionList>(loSourceDataDataSet.Tables[0].Rows.Count);
        if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
        {
            int rowCount = (int)loSourceDataDataSet.Tables[0].Rows[0]["inRecordCount"];
            if (loSourceDataDataSet.Tables[0].Rows.Count > 0)
            {
                pageing = Function.doPaging(curr, rowCount, 10, "&");
            }
            foreach (DataRow dr in loSourceDataDataSet.Tables[0].Rows)
            {
                CommissionList data = new CommissionList();
                data.inGenerateCommissionId = Convert.ToInt32(dr["inGenerateCommissionId"]);
                data.inYears = Convert.ToInt32(dr["inYears"]);
                DateTime date = new DateTime(Convert.ToInt32(dr["inYears"]),Convert.ToInt32(dr["inMonths"]) , 1);
                data.stMonths = date.ToString("MMMM");
                data.paging = pageing;
                CommissionList.Add(data);
            }
        }
        return CommissionList;
    }
}
