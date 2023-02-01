<%@ Page Language="C#" AutoEventWireup="true" CodeFile="commission-rpt.aspx.cs" Inherits="commission_rpt" %>

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
                    color: greenyellow;
                }

        .active {
            background-color: #111;
            color: greenyellow;
            font-weight: bold;
        }
    </style>
    <link href="https://cdn.datatables.net/1.10.22/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.6.4/css/buttons.dataTables.min.css" rel="stylesheet" />
</head>
<body style="background-color: white !important;">
    <form id="form1" runat="server">
        <ul>
           <%-- <li><a href="sales-report.aspx">Sales Report</a></li>
            <li><a href="service-wise-sales-report.aspx">Service Wise Sales Report</a></li>
            <li><a href="gst-report.aspx">GST Report</a></li>--%>
            <li><a class="active" href="commission-rpt.aspx">Commission Report</a></li>

            <%--<li style="float: right"><a href="dashboard.html" style="font-weight:bold; color:darkorange;">Back</a></li>--%>
        </ul>
        <div class="container-fluid">
            <div class="row bg-title">
                <div class="col-md-12">
                    <h2 class="page-title text-center" style="font-weight: bold; color: darkgreen; text-decoration: underline;">Commission Report</h2>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class="white-box" style="height: 100px;">
                        <div class="col-lg-12">

                            <div class="col-lg-2">
                                <label>Vendor</label>
                                <asp:DropDownList ID="ddVendor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddVendor_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-lg-3">
                                <label>Shop Name</label>
                                  <asp:DropDownList ID="ddShopName" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <label>Month</label>
                                <asp:DropDownList ID="ddMonth" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="---Select Month---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <label>Year</label>
                                <asp:DropDownList ID="ddYear" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="---Select Year---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                    <asp:ListItem Text="2021" Value="2021"></asp:ListItem>
                                    <asp:ListItem Text="2022" Value="2022"></asp:ListItem>
                                    <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <label style="color: transparent;">Search</label>
                                <br />
                                <asp:Button ID="btn_Commission" Text="Submit" runat="server" CssClass="btn btn-info" OnClick="btn_Commission_Click" />
                               <%-- <a class="btn btn-info" href="#" onclick="BindCommsion();">Submit</a>--%>
                                <a class="btn btn-info" href="#" onclick="ExportExcel();">Export To Excel</a>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-12">
                        <div class="white-box">
                            <div class="table-responsive" style="overflow: auto;">
                                <table id="CommisionList" class="display nowrap table table-hover table-striped table-bordered" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th>Sl.No</th>
                                            <th>Month/Year</th>
                                            <th>Vendor Name</th>
                                            <th>Shop Name</th>
                                            <th>Revenue</th>
                                            <th>GST</th>
                                            <th>Comission Percentage</th>
                                            <th>Commission</th>
                                            
                                            <th>Payment Status</th>
                                            <th>Remark</th>
                                        </tr>
                                    </thead>
                                    <tbody id="trCommisionList" runat="server">
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

          
            function ExportExcel() {
                window.location = "download-gst-report.aspx?id=" + $('#ddShopName').val() + "&fd=" + $('#txtFromDate').val() + "&td=" + $('#txtToDate').val() + "&ps=" + $('#ddPaymentStatus').val();
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

