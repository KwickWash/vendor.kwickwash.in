<%@ Page Language="C#" AutoEventWireup="true" CodeFile="generate-commision.aspx.cs" Inherits="generate_commision" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" type="image/png" sizes="5x5" href="assets/img/favicon.png">
    <title>KwickWash</title>
    <template id="ctrheadercss">
    </template>
    <style type="text/css">
        .classDashboard {
            background-color: #6ab215;
        }

        .row-in-br {
            border-right: 1px solid rgba(120, 130, 140, 0.13);
            background: white;
            margin: 5px;
            border-radius: 3px;
            box-shadow: 0px 1px 1px 0px #ccc;
        }

        .white-box {
            padding: 0px !important;
        }

        .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

        .col-in h3 {
            background: #f74d4d;
            color: white;
            font-size: 11px;
            border-radius: 2px;
            position: absolute;
            top: -22px;
            right: -7px;
            width: auto;
            text-align: center;
            height: 30px;
            min-width: 25px;
        }

        .col-in li.col-middle {
            width: 65% !important;
        }

        .circle-md {
            width: 40px;
            padding-top: 3px;
            height: 40px;
            font-size: 24px !important;
        }

        .row-in i {
            font-size: 21px;
        }

        h4 {
            line-height: 22px;
            font-size: 13px;
        }

        @media (min-width: 1200px) {
            .col-lg-3 {
                width: 24%;
                padding: 13px
            }

            .col-lg-2 {
                width: 19%;
                padding: 5px;
            }
        }

        .userIcon {
            color: darkblue !important;
            background-color: white !important;
        }

        .row-in i {
            font-size: 28px !important;
            padding: 11px !important;
            color: #6ab215;
        }

        .row-in-br {
            background: white !important;
            margin: 11px !important;
            box-shadow: 0 1px 6px 0 #000 !important;
            border-radius: 4px;
        }

        h2 {
            font-size: 14px !important;
            margin-left: 11px;
            font-weight: 500;
        }

        .col-in li {
            display: inline-block;
            vertical-align: middle;
            text-align: center;
            padding: 0px !important;
        }

        h4 {
            color: #6ab215;
            font-weight: 400;
        }

        .card {
            /* Add shadows to create the "card" effect */
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
        }

            /* On mouse-over, add a deeper shadow */
            .card:hover {
                box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
            }

        /* Add some padding inside the card container */
        .container {
            padding: 2px 16px;
        }

        .card {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            border-radius: 5px; /* 5px rounded corners */
            background: black;
        }

        /* Add rounded corners to the top left and the top right corner of the image */
        img {
            border-radius: 5px 5px 0 0;
        }

        b {
            font-size: 20px;
            color: white;
            font-family: system-ui;
            padding: 4px;
            border-radius: 2px;
        }

        .txth1 {
            width: 200px;
            background: white;
            color: rebeccapurple;
        }

        h1, h2, h3, h4, h5, h6 {
            color: wheat;
        }

        .booked {
            background-color: #0876e0 !important;
            color: white;
            font-weight: 400;
        }

        .delivered {
            background-color: #6bb841 !important;
            color: white;
            font-weight: 400;
        }

        .paid {
            background-color: #e89a0a !important;
            color: white;
            font-weight: 400;
        }

        .orderNo {
            color: yellow !important;
            font-style: italic !important;
        }

        .ReadyForDelivery {
            background-color: violet !important;
            color: white;
            font-weight: 400;
        }

        .InProcess {
            background-color: #dcb62d !important;
            color: white;
            font-weight: 400;
        }

        .UnPaid {
            background: #c20b0b !important;
            color: white;
            font-weight: 400;
        }

        .AmountReceived {
            background-color: darkgreen !important;
            color: white;
            font-weight: 400;
        }

        .AmountDue {
            background-color: red !important;
            color: white;
            font-weight: 400;
        }

        .WalletBalace {
            background-color: blueviolet !important;
            color: white;
            font-weight: 400;
        }
    </style>
</head>

<body class="fix-header">
    <form id="frm1" runat="server">
        <!-- Preloader -->
        <div class="preloader">
            <svg class="circular" viewBox="25 25 50 50">
                <circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10" />
            </svg>
        </div>

        <!-- Wrapper -->
        <div id="wrapper">

            <!-- Start Topbar header-->
            <div id="ctrheader"></div>
            <!-- End Topbar header-->
            <!-- Start Left Sidebar -->
            <div class="navbar-default sidebar" role="navigation" id="ctrLeftMenu">
            </div>
            <!-- End Left Sidebar -->
            <!-- Page Content -->
            <div id="page-wrapper">
                <div class="container-fluid">
                    <div class="row bg-title">
                        <div class="col-lg-3 col-md-4 col-sm-4 col-xs-12">
                            <h4 class="page-title" style="color: black;">Generate Commision Report</h4>
                        </div>

                    </div>

                    <!-- Different data widgets -->
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="white-box" style="height: 700px;">
                                <div class="col-lg-2">
                                    <label>Year</label>
                                    <asp:DropDownList ID="ddYear" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="---Select Year---" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                        <asp:ListItem Text="2021" Value="2021"></asp:ListItem>
                                        <asp:ListItem Text="2022" Value="2022"></asp:ListItem>
                                        <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>--%>
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
                                    <%-- <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>--%>
                                </div>

                                <div class="col-lg-3">
                                    <br>
                                    <asp:Button ID="btnGenerateCommision" runat="server" CssClass="btn btn-info" Text="Generate Commision" OnClick="btnGenerateCommision_Click" />
                                    <div id="loader" style="display: none;">
                                        <span><i class="fa fa-spinner fa-spin" style="color: #9FCB39; font-size: 40px; text-align: center;"></i></span>
                                    </div>
                                </div>
                                <hr />
                                <div class="col-lg-12">
                                    <div class="table-responsive" style="overflow: auto;" id="divCommissionList">
                                    </div>
                                    <div class="dataTables_paginate paging_simple_numbers">
                                        <ul class="pagination" id="divPaging">
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <!-- End Page Content -->

                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div id="ctrFooter"></div>
        <!-- End Wrapper -->
        <!-- All Jquery -->
        <script src="assets/plugins/bower_components/jquery/dist/jquery.min.js"></script>
        <!-- Bootstrap Core JavaScript -->
        <script src="assets/bootstrap/dist/js/bootstrap.min.js"></script>
        <!-- Menu Plugin JavaScript -->
        <script src="assets/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.js"></script>
        <!--slimscroll JavaScript -->
        <script src="assets/js/jquery.slimscroll.js"></script>
        <!--Wave Effects -->
        <script src="assets/js/waves.js"></script>
        <!--Counter js -->
        <!--Morris JavaScript -->
        <script src="assets/plugins/bower_components/raphael/raphael-min.js"></script>
        <script src="assets/plugins/bower_components/morrisjs/morris.js"></script>
        <!-- chartist chart -->
        <script src="assets/plugins/bower_components/chartist-js/dist/chartist.min.js"></script>
        <script src="assets/plugins/bower_components/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.min.js"></script>
        <!-- Calendar JavaScript -->
        <script src="assets/plugins/bower_components/moment/moment.js"></script>
        <script src='assets/plugins/bower_components/calendar/dist/fullcalendar.min.js'></script>
        <script src="assets/plugins/bower_components/calendar/dist/cal-init.js"></script>
        <script src="assets/plugins/bower_components/Minimal-Gauge-chart/js/cmGauge.js"></script>
        <!-- Date Picker Plugin JavaScript -->
        <script src="assets/plugins/bower_components/bootstrap-datepicker/bootstrap-datepicker.min.js"></script>
        <!-- Date range Plugin JavaScript -->
        <script src="assets/plugins/bower_components/timepicker/bootstrap-timepicker.min.js"></script>
        <script src="assets/plugins/bower_components/bootstrap-daterangepicker/daterangepicker.js"></script>
        <script src="assets/plugins/bower_components/sweetalert/sweetalert.min.js"></script>
        <!-- Magnific popup JavaScript -->
        <script src="assets/plugins/bower_components/Magnific-Popup-master/dist/jquery.magnific-popup.min.js"></script>
        <script src="assets/plugins/bower_components/Magnific-Popup-master/dist/jquery.magnific-popup-init.js"></script>
        <!-- Custom Theme JavaScript -->
        <script src="assets/js/validator.js"></script>
        <script src="assets/js/custom.min.js"></script>
        <script src="assets/js/dashboard2.js"></script>


        <script src="assets/plugins/bower_components/toast-master/js/jquery.toast.js"></script>
        <script src="assets/plugins/bower_components/datatables/datatables.min.js"></script>
        <script type="text/javascript">

            $(document).ready(function () {
                BindOrder(1)
            });
            function bgLoad() {
                BindOrder(1);
            }

            function BindOrder(pageno) {
                //alert(id);
                $('#divCommissionList').html('<span><i class="fa fa-spinner fa-spin" style="color:#9FCB39; font-size:40px; text-align:center;"></i></span>');
                var shopUserid = $('#hd_Userid').val();
                var url = "apiService.asmx/GetCommissionList";
                var data = "{pageno:'" + pageno + "'}";
                var pageList = '';
                var slno = 0;
                $.ajax({
                    url: url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: 'POST',
                    data: data,
                    success: function (data) {
                        _details = data.d;
                        var _htmlTable = '<table id="myTable2" class="display nowrap table table-bordered" cellspacing="0" width="100%">';
                        _htmlTable += '<thead><tr><th>S.No.</th><th>Year/Month</th><th>View</th></tr>';
                        _htmlTable += '</thead><tbody id="listCommissionList"';

                        if (typeof (_details) !== 'undefined') {
                            $.each(_details, function (i, object) {
                                _htmlTable += '<tr>';
                                if (i == 0) {
                                    slno = Number(10) * Number(pageno);
                                    slno = slno - (10);
                                }
                                slno++;
                                _htmlTable += '<tr>';
                                _htmlTable += '<td>' + slno + '</td>';
                                _htmlTable += ' <td>' + object.stMonths + ' / ' + object.inYears + '</td>';
                                _htmlTable += ' <td><a target="_blank" href="commission-rpt.aspx?m=' + object.stMonths + '&y=' + object.inYears + '">View</a></td>';
                                pageList = object.paging;
                            });
                        }
                        _htmlTable += '</tbody></table>'
                        $('#divCommissionList').html(_htmlTable);
                        $('#divPaging').html(pageList);

                    },
                    error: function (errorThrown) {
                        console.log(errorThrown);
                    }
                });
            }
        </script>
    </form>
</body>
</html>