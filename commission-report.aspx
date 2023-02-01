<%@ Page Language="C#" AutoEventWireup="true" CodeFile="commission-report.aspx.cs" Inherits="commission_report" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Commission Report</title>
    <link href="assets/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="assets/plugins/bower_components/datatables/media/css/dataTables.bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/style.css" rel="stylesheet">

    <style type="text/css">
        body {
            margin: 0px;
        }

        ul {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            background-color: #333;
        }

        li {
            float: left;
        }

            li a {
                display: block;
                color: white;
                text-align: center;
                padding: 14px 16px;
                text-decoration: none;
            }

                /* Change the link color to #111 (black) on hover */
                li a:hover {
                    background-color: #111;
                     color:greenyellow;
                }

        .active {
            background-color: #111;
             color:greenyellow;
             font-weight:bold;
        }
    </style>
    <link href="https://cdn.datatables.net/1.10.22/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.6.4/css/buttons.dataTables.min.css" rel="stylesheet" />
</head>
<body style="background-color: white !important;">
    <form id="form1" runat="server">
        <ul>
            <li><a href="sales-report.aspx">Sales Report</a></li>
            <li><a href="service-wise-sales-report.aspx">Service Wise Sales Report</a></li>
            <li><a href="gst-report.aspx">GST Report</a></li>
            <li><a class="active" href="commission-report.aspx">Commission Report</a></li>

            <%--<li style="float: right"><a href="dashboard.html" style="font-weight:bold; color:darkorange;">Back</a></li>--%>
        </ul>
        <div class="container-fluid">
            <div class="row bg-title">
                <div class="col-md-12">
                    <h2 class="page-title text-center" style="font-weight:bold; color:darkgreen; text-decoration:underline;">Commission Report</h2>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class="white-box" style="height: 100px;">
                        <div class="col-lg-12">
                            <div class="col-lg-3">
                                <label>Select Service</label>
                                <select id="ddServiceName" class="form-control">
                                </select>
                            </div>
                            <div class="col-lg-2">
                                <label>From Date</label>
                                <input type="date" id="txtFromDate" class="form-control" />
                            </div>
                            <div class="col-lg-2">
                                <label>To Date</label>
                                <input type="date" id="txtToDate" class="form-control" />
                            </div>
                            <div class="col-lg-2">
                                <label>Paymnet Status</label>
                                <select id="ddPaymentStatus" class="form-control">
                                    <option value="0">--All--</option>
                                    <option value="paid">Paid</option>
                                    <option value="unpaid">Unpaid</option>
                                </select>
                            </div>
                            <div class="col-lg-2">
                                <label style="color: transparent;">Search</label>
                                <br />
                                <a class="btn btn-info" href="#" onclick="BindOrder();">Submit</a>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-12">
                        <div class="white-box">
                            <div class="table-responsive" style="overflow: auto;">
                                <table id="myShopOrderList" class="display nowrap table table-hover table-striped table-bordered" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th style="display: none;">#</th>
                                            <th>Order #</th>
                                            <th>Invoice-No</th>
                                            <th>Shop-Name</th>
                                            <th>Customer-Name</th>
                                            <th>Order-Date</th>
                                            <th>Payment-Status</th>
                                            <th>Payment-Mode</th>
                                            <th>Amount</th>
                                            <th>Discount</th>
                                            <th>Payable-Amt</th>
                                        </tr>
                                    </thead>
                                    <tbody id="listMyShopOrder">
                                    </tbody>
                                </table>

                                <table id="myTotal" class="display nowrap table table-hover table-striped table-bordered" cellspacing="0" width="100%">
                                    <tbody id="listmyTotal">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /.row -->
            </div>
        </div>
        <script src="assets/plugins/bower_components/jquery/dist/jquery.min.js"></script>
        <!-- Bootstrap Core JavaScript -->
        <script src="assets/bootstrap/dist/js/bootstrap.min.js"></script>


        <script>

            $(document).ready(function () {
                BindShop();
            });

            //Bind shop list code
            function BindShop() {
                //alert(id);
                $('#ddVendorName').html('<h3><i class="fa fa-spiner fa-spin"></i></h3>');
                var option = "<option value='0'>Select Shop Name</option>";
                var url = "https://api.kwickwash.com/api/ShopList";
                //alert(url);
                $.ajax({
                    url: url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: 'GET',
                    success: function (data) {
                        _details = data;
                        if (typeof (_details) !== 'undefined') {
                            $.each(_details, function (i, object) {
                                option += "<option value='" + object.userId + "'>" + object.companyName + "</option>";
                            });
                        }
                        $('#ddShopName').html(option);
                    },
                    error: function (errorThrown) {
                        console.log(errorThrown);
                    }
                });
            }
            // end binding


            function BindOrder() {
                $('#listMyShopOrder').html('<td colspan="6" style="text-align: center;"><span><i class="fa fa-spinner fa-spin" style="color:darkslateblue; font-size:40px; text-align:center;"></i></span></td>');
                var id = $('#ddShopName').val();
                var fdate = $('#txtFromDate').val();
                var tdate = $('#txtToDate').val();
                var pstatus = $('#ddPaymentStatus').val();
                var url = "https://api.kwickwash.com/api/orderlist/GetOrderReport?id=" + id + "&fdate=" + fdate + "&tdate=" + tdate + "&pstatus=" + pstatus;
                var ctr = 0;
                var totalPayment = 0;
                //alert(url);
                $.ajax({
                    url: url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: 'GET',
                    success: function (data) {
                        _details = data;
                        var _htmlTable = "";
                        if (typeof (_details) !== 'undefined') {
                            $.each(_details, function (i, object) {
                                ctr++;
                                var payment = 0;
                                _htmlTable += '<tr>';
                                _htmlTable += '<td style="display:none;">' + ctr + '</td>';
                                _htmlTable += '<td>' + object.orderId + '</td>';
                                _htmlTable += '<td>' + object.invoiceNo + '</td>';
                                _htmlTable += '<td>' + object.companyName + '</td>';
                                _htmlTable += '<td>' + object.name + '</td>';
                                _htmlTable += ' <td>' + object.orderDate + '</td>';
                                _htmlTable += ' <td>' + object.status + '</td>';
                                _htmlTable += ' <td>' + object.paymentMode + '</td>';
                                _htmlTable += ' <td>' + Number(object.ttlAmount).toFixed(2) + '</td>';
                                _htmlTable += ' <td>' + Number(object.ttlDiscount).toFixed(2) + '</td>';
                                _htmlTable += ' <td>' + Number(object.ttlPayableAmount).toFixed(2) + '</td>';
                                _htmlTable += '</tr>';
                                payment = object.ttlPayableAmount;
                                totalPayment += parseFloat(payment);
                                //alert(totalPayment);
                            });
                        }
                        $('#listMyShopOrder').html(_htmlTable);



                        var htmlTable1 = "";
                        htmlTable1 += '<tr>';
                        htmlTable1 += '<td colspan="6">Total Amount</td>';
                        htmlTable1 += ' <td>' + Number(totalPayment).toFixed(2) + '</td>';
                        htmlTable1 += '</tr>';
                        $('#listmyTotal').html(htmlTable1);

                    },
                    error: function (errorThrown) {
                        console.log(errorThrown);
                    }
                });


            }


        </script>
        <script src="assets/js/cbpFWTabs.js"></script>


        <script src="assets/plugins/bower_components/toast-master/js/jquery.toast.js"></script>
        <script src="assets/plugins/bower_components/datatables/datatables.min.js"></script>
        <!-- start - This is for export functionality only -->
        <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.flash.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
        <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
        <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.print.min.js"></script>
        <!-- end - This is for export functionality only -->
        <script type="text/javascript" src="assets/js/ajaxupload.js"></script>

    </form>
</body>
</html>