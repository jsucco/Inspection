<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Site_2.master" AutoEventWireup="false" CodeFile="InspectionVisualizer.aspx.vb" Inherits="core.APP_Presentation_InspectionVisualizer" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%--<%@ OutputCache Location="Server" VaryByParam="*" Duration="2000" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .ui-widget-content td {
            font-size: 1em;
        }

        .l1 {
            padding-left: 1em;
        }

        .ui-jqgrid tr.jqgrow td {
            white-space: normal !important;
        }

        .loading {
            position: fixed;
            z-index: 20000;
            height: 2em;
            width: 2em;
            overflow: visible;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            .loading:before {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            .loading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>
    <div style="position: absolute; top: 69px; z-index: 1000" id="backdiv">
        <a href="<%=Session("BaseUri")%>/APP/APR_SiteEntry.aspx" title="Back to MENU" id="menuLnkBack">M</a>
    </div>
    <div class="loading" id="loading" style="display: none;">Loading&#8230;</div>

    <div style="position: absolute; left: 50px; width: 75%; height: 150px;">
        <div id="LcustomersSlider" style="top: 10px; left: 100px; display: none; position: relative; height: 150px;">
            <div id="LcustomersWrapper" style="width: 1690px;" class="">
                <ul id="Lcustomers"></ul>
            </div>

        </div>
        <div id="select2div" style="top: 10px; left: 100px; position: relative; height: 150px;">
            <select id="select-location" style="width: 100%;">
                <optgroup label="Foreign" id="ForeignGroup"></optgroup>
                <optgroup label="Domestic" id="DomesticGroup"></optgroup>
            </select>
        </div>
    </div>
    <div style="position: absolute; width: 180px; left: 85%; height: 70px;">
        <div id="actionbuttons" style="width: 180px; margin: 0px; position: relative; float: right; top: -2px;">


            <a href="#" id="ab" style="color: white !important; font-size: 19px; width: 65px; height: 60px; display: block; float: left;" class="actionButton">Clear Search
            </a>




            <a href="#" id="ab2" style="color: white  !important; font-size: 16px; width: 65px; height: 60px; display: block; margin-left: 90px;" class="actionButton">How To Use
            </a>





        </div>
    </div>
    <div id="PageFilters">
        <%-- <div style="position: absolute; z-index: 100; left: 95.2%;" id="filterdiv">
            <a id="hideFilters" style="position: absolute; border-style: outset;"></a>
            <a id="showFilters" style="position: absolute; display: none; border-style: outset;"></a>
        </div>--%>
        <div id="FilterDiv" style="position: relative; left: -10px; top: 70px; width: 100%; height: 100px; background-color: rgba(240,240,240,1); border: 1px solid #ccc; overflow: hidden;">
            <h2 style="position: absolute; top: -15px;">PAGE FILTERS</h2>

            <div id="fromdatecontainer" style="position: absolute; top: 30px; left: 1%">
                <asp:Label ID="LblDateFrom" runat="server" CssClass="filterlabel">Begin Date</asp:Label>
                <div style="position: absolute; left: 29px;">
                    <input type="text" id="TxtDateFrom" name="datefrom" style="float: left; height: 36px;" class="labelleft1" />
                    <input type="hidden" id="DateFrom_Hidden" class="PageFilter" runat="server" />
                </div>
            </div>
            <div id="todatecontainer" style="position: absolute; top: 30px; left: 25%">
                <asp:Label ID="LblDateTo" runat="server" CssClass="filterlabel">End Date  </asp:Label>
                <div style="position: absolute; left: 29px;">
                    <input type="text" id="TxtDateTo" name="dateto" style="float: left; left: 14px; height: 36px;" class="labelleft1" />
                    <input type="hidden" id="DateTo_Hidden" runat="server" class="PageFilter" />
                </div>
            </div>
            <div data-role="fieldcontain" style="position: absolute; top: 30px; left: 50%; z-index: 100;">
                <label for="select-based-flipswitch" class="filterlabel">DataNumber:</label>
                <select id="select-DataNo" style="position: absolute; left: -2px;" data-role="flipswitch" class="PageFilter selector">
                    <option value="ALL">ALL</option>
                </select>
            </div>
            <div data-role="fieldcontain" style="position: absolute; top: 30px; left: 60%; z-index: 100;">
                <label for="select-based-flipswitch" class="filterlabel">Work Order:</label>
                <select id="select-WorkOrder" style="position: absolute; left: -2px;" data-role="flipswitch" class="PageFilter selector">
                    <option value="ALL">ALL</option>
                </select>
            </div>

            <div data-role="fieldcontain" style="position: absolute; top: 30px; left: 70%; z-index: 100;">
                <label for="select-based-flipswitch" class="filterlabel">Audit Type:</label>
                <select id="select-AuditType" data-role="flipswitch" class="PageFilter selector">
                </select>
            </div>
            <%-- <div data-role="fieldcontain" style="position: absolute; top: 30px; left: 90%; z-index: 100;">
                <label for="select-based-flipswitch" class="filterlabel">Prp Code:</label>
                <div style="position: relative;">
                    <select id="select-prp" style="position: absolute; left: -2px; width: 150px;" data-role="flipswitch" class="selector" multiple="multiple">
                    </select>
                </div>
            </div>--%>
        </div>
        <div id="OverallDiv" style="position: relative; left: 15px; top: 110px; width: 100%; height: 100px;">
            <table id="OverallGrid" style="width: 100%;">
            </table>
        </div>
        <div id="GlobalDiv" style="position: relative; left: 15px; top: 190px; width: 100%; height: 100px;">
            <table id="GlobalGrid" style="width: 100%;">
            </table>
        </div>
        <div id="DomesticDiv" style="position: relative; left: 15px; top: 250px; width: 100%; height: 100px;">
            <table id="DomesticGrid" style="width: 100%;">
            </table>
        </div>
        <div id="InteriorsDiv" style="position: relative; left: 15px; top: 310px; width: 100%; height: 100px;">
            <table id="InteriorsGrid" style="width: 100%;">
            </table>
        </div>
        <div id="GridDiv" style="position: relative; left: -10px; top: 370px; width: 100%; height: 100px;">
            <table id="MainGrid" style="width: 100%;">
            </table>
        </div>
        <div id="InstructionDialog" style="display: none; z-index: 30000;">

            <p>Welcome to the Inspection Visualizer!</p>
            <p>To get started, select the locations that you want to view from the dropdown at the top of the page.  Clicking on a category will add all locations under that category.</p>
            <p>If you want to see data for individual workrooms, click on one of the '+' icons on the far left of the data grid.  This will cause a sub-graph of all available workrooms to appear.</p>
            <p>You can view line graphs for all 7 data categories.  To do so, double-click on the cell that has the desired category and time period.  The Custom field is controlled with the Page filters, located just under the location dropdown. </p>
            <p>Once the linegraph is loaded, you can drill down further into the data if you choose.  To do so, just click on one of the nodes on the line graph.  Note that this will only work for 'No. of Defects', 'No. of Rejects','No. of Inspections', and 'No. of Rejected Lots'</p>
            <p>To clear out all data, and wipe the grid clean, click 'Clear Search' in the top right corner of the page.</p>
            <p>Any questions or bug reports should be directed to Kris Bredwell (kbredwell@standardtextile.com)</p>

        </div>
        <div id="GraphDialog" class="GDIV" style="position: relative; display: block; z-index: 1000;">
            <section>
                <div id="chart_div" style="position: absolute; left: 0px; top: 0px; width: 90%; height: 90%; z-index: 10000;"></div>
            </section>


        </div>


        <div id="TableDialog" style="position: relative; display: block; z-index: 1000;">
            <div id="table_div" style="position: absolute; left: 0px; top: 0px; width: 90%; height: 90%; z-index: 10000;"></div>

        </div>
    </div>

    <div id="reportOptions" style="left: 50%; display: none; z-index: 1000;">
        <div id="options">
            <div style="position: absolute; bottom: 3px; right: 3px"><span id="lblReport" style="font-size: 0.8em; font-weight: bold"></span></div>
            <h1>Report Options</h1>
            <div>
                <label>Select Document Type</label>
                <div class="tile-options" id="divCustomerReport1" onclick="">
                    <a id="InsSummary" class="lnk_doc_type">
                        <img class="img-doctype" src="../../Images/excel-2013-icon.png">
                        <label>Inspection Summary Report</label>
                    </a>
                </div>
                <div class="tile-options" id="divCustomerReport2" onclick="">
                    <a id="InsDefects" class="lnk_doc_type">
                        <img class="img-doctype" src="../../Images/excel-2013-icon.png">
                        <label>DefectMaster Report</label>
                    </a>
                </div>
                <div class="tile-options" id="divExcel1">
                    <a id="InsComp" class="lnk_doc_type">
                        <img class="img-doctype" src="../../Images/excel-2013-icon.png">
                        <label>Inspection Compliance</label>
                    </a>
                </div>
                <div class="tile-options" id="divExcel2">
                    <a id="InsSpec" class="lnk_doc_type">
                        <img class="img-doctype" src="../../Images/excel-2013-icon.png">
                        <label>Spec Summary Report</label>
                    </a>
                </div>
                <div class="tile-options" id="divExcel3">
                    <a id="InsDump" class="lnk_doc_type">
                        <img class="img-doctype" src="../../Images/excel-2013-icon.png">
                        <label>Dump Everything Report</label>
                    </a>
                </div>
                <div class="tile-options" id="divExcel4">
                    <a id="InsTimerReport" class="lnk_doc_type">
                        <img class="img-doctype" src="../../Images/excel-2013-icon.png">
                        <label>Timer Report</label>
                    </a>
                </div>

            </div>
        </div>
        <div style="display: none;">
            <asp:Button ID="ReportCallBack" runat="server" Text="none" />
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" runat="Server">
    <link href="../../Styles/InspectionVisualizer.css" rel="stylesheet" type="text/css" />
    <link href="http://cdn.wijmo.com/themes/aristo/jquery-wijmo.css" rel="stylesheet" type="text/css" />
    <script src="http://underscorejs.org/underscore-min.js"></script>
    <script src="http://code.jquery.com/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.11.0/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <link href="../../Styles/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20153.83.min.css" rel="stylesheet" type="text/css" />
    <%--<script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20153.83.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20153.83.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20153.83.js" type="text/javascript"></script>--%>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/InspectionVisualizer") %>
    </asp:PlaceHolder>
    <%--<script src="../../Scripts/grid.locale-en.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>--%>
    <%--<script src="../../Scripts/select2/select2.min.js" type="text/javascript" ></script>--%>
    <link href="../../Styles/select2/select2.css" rel="stylesheet" />
    <style type="text/css">
        .ui-widget-overlay {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 1350px;
        }
    </style>
    <script src="../../Scripts/Gcharts/jsapi.js"></script>

    <script type="text/javascript">
        var URIString;
        var GraphData = [];
        var NodeGraphData = [];
        var ScatterPlotJson;
        var todate;
        var fromdate;
        var DefectMasterHistogram;
        var $Todateval;
        var $Fromdateval;
        var $ytdfromdate;
        var exytd_DateFrom_fc;
        var exmtd_DateFrom_fc;
        var exmtd_DateTo_fc;
        var exytd_DateTo_fc;
        var selectedId;
        var LocationNames = <%=LocationNames%>;
        var SelectedCID = "999";
        var SelectedTab = 'Overview';
        var JobSummaryList;
        var ijsnextcnt = 1;
        var NextFlag = false;
        var OwFilterFlag = false;
        var ijsFilterFlag = false;
        var SgFilterFlag = false;
        var PtFilterFlag = false;
        var TabAccCnt = [{ Overview: 0, JobSummary: 0, SpecSummary: 0, Photos: 0 }];
        var SessionId = '<%=SessionId%>';
        var subgridquerystr = '';
        var Specsubgridquerystr = '';
        var FilterColumnName = '';
        var Filterqvalue = "ALL";
        var BreakOutldcnt = 0;
        var LineGraphldcnt = 0;
        var $AuditType = "FINAL AUDIT";
        $("#select-AuditType option:selected").val("FINAL AUDIT");
        var $DataNo = "ALL";
        var $WorkOrder = "ALL";
        var ActiveFilter = "ALL";
        var DefectPictureArray = [];
        var DefectPictureArrayF = [];
        var selectFiltervalues = [{ col: "id", val: "ALL" }, { col: "JobNumber", val: "ALL" }, { col: "UnitDesc", val: "ALL" }, { col: "Name", val: "ALL" }, { col: "Technical_PassFail", val: "ALL" }];
        var selectSpecFiltervalues = [{ col: "id", val: "ALL" }, { col: "JobNumber", val: "ALL" }, { col: "UnitDesc", val: "ALL" }, { col: "DataNo", val: "ALL" }];
        var selList = ["1001", "115", "114", "627", "590", "482", "485", "578", "112", "111"];
        var startX;
        var initialMouseX;
        var draggedObject;
        var maxLeft;
        var dragging;
        var canScrollClients;
        var LstartX;
        var LinitialMouseX;
        var LdraggedObject;
        var LmaxLeft;
        var Ldragging;
        var LcanScrollClients;
        var InspectionTypesArray = <%=InspectionTypesArray%>;
        var FilterSource = 'PageLoad';
        var FullLocationsArray = [];
        var ActiveLocationsArray = [];
        var ActiveFilterArray = [];
        var ActiveFilterString = "";
        var FilterCnt = 0;
        var LocationsStringArray;
        var PageUserActivityLogId = 0;
        var ADMINFLAG = '';
        var EmployeeNames;
        var DefectDescs;
        var LocationNamesDrop;
        var DefectTypes;
        select_Location_fc = "ALL";
        google.load('visualization', '1.0', { 'packages': ['corechart'] });

        $(function () {
            document.body.style.zoom = "85%"

            $("#PageType").text("Inspection")
                .attr('href', '../Mob/SPCInspectionInput.aspx');
            todate = '<%=todate%>';
            $Todateval = '<%=todate%>';
            fromdate = '<%=fromdate%>';
            $Fromdateval = '<%=fromdate%>';
            exytd_DateFrom_fc = '<%=ytdfromdate%>';
            exmtd_DateFrom_fc = '<%=mtdfromdate%>';
            exytd_DateTo_fc = '<%=CurrentDate%>';
            exmtd_DateTo_fc = '<%=CurrentDate%>';
            PageUserActivityLogId = '<%=UserActivityLogId%>';
            ADMINFLAG = '<%=ADMINFLAG%>';
            EmployeeNames = '<%=EmployeeNames%>';
            DefectDescs = '<%=DefectDescs%>';
            LocationNamesDrop = '<%=LocationNamesDrop%>';
            DefectTypes = '<%=DefectTypes%>';
            $("article").css("height", (2 * screen.availHeight).toString() + "px");
            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.load('current', { 'packages': ['table'] });
            function drawWRChart(Facility, GridType, TimePeriod, WorkRoom) {
                document.getElementById("loading").style.display = "block";
                datahandler.DrawWRChart(Facility, GridType, TimePeriod, WorkRoom, fromdate, todate, $DataNo, $WorkOrder, $AuditType);

            }
            function drawChart(Facility, GridType, TimePeriod) {
                document.getElementById("loading").style.display = "block";
                datahandler.DrawChart(Facility, GridType, TimePeriod, fromdate, todate, $DataNo, $WorkOrder, $AuditType);

            }
            var numi = document.getElementById('Locations');
            var html = [];
            var fchtml = [];
            var dchtml = [];
            var ihtml = [];
            var dhtml = [];
            var dcid = [];
            var fcid = [];
            var iid = [];
            var did = [];

            //var mydata = [
            //    { id: "1", Facility: "Thomaston", Time_Period: "Past 30 Days", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { rowspan: "3" } } },
            //    { id: "2", Facility: "Thomaston", Time_Period: "Past 12 Months", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { display: "none" } } },
            //    { id: "3", Facility: "Thomaston", Time_Period: "Custom", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { display: "none" } } },
            //    { id: "4", Facility: "Carolina", Time_Period: "Past 30 Days", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { rowspan: "3" } } },
            //    { id: "5", Facility: "Carolina", Time_Period: "Past 12 Months", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { display: "none" } } },
            //    { id: "6", Facility: "Carolina", Time_Period: "Custom", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { display: "none" } } },
            //];

            arrtSetting = function (rowId, val, rawObject, cm) {
                var attr = rawObject.attr[cm.name], result;
                if (attr.rowspan) {
                    result = ' rowspan=' + '"' + attr.rowspan + '"';
                } else if (attr.display) {
                    result = ' style="display:' + attr.display + '"';
                }
                return result;
            };
            var rowsToColor = [];
            
            $("#InstructionDialog").wijdialog({
                buttons: {

                    Close: function () {
                        $(this).wijdialog("close");
                    }
                },
                open: function () {

                },
                close: function (event, ui) {

                },
                captionButtons: {
                    pin: { visible: false },
                    refresh: { visible: false },
                    toggle: { visible: false },
                    minimize: { visible: false },
                    maximize: { visible: false }
                },

                resizable: false,
                width: 1200,
                height: 700,
                autoOpen: false,
                position: 'fixed',
                modal: true

            });
            $(".GDIV").wijdialog({
                buttons: {

                    Close: function () {
                        $(this).wijdialog("close");
                    },
                    Get_Printer_Friendly_Version: function () {
                        var iframe = "<iframe width='100%' height='100%' src='" + URIString + "'></iframe>";
                        var x = window.open();
                        x.document.open();
                        x.document.write(iframe);
                        x.document.close();
                    }
                },
                open: function () {

                },
                close: function (event, ui) {

                },
                captionButtons: {
                    pin: { visible: false },
                    refresh: { visible: false },
                    toggle: { visible: false },
                    minimize: { visible: false },
                    maximize: { visible: false }
                },

                resizable: false,
                width: 1200,
                height: 700,
                autoOpen: false,
                position: 'fixed',
                modal: true

            });
            $("#TableDialog").wijdialog({
                buttons: {

                    Close: function () {
                        $(this).wijdialog("close");
                    }
                },
                open: function () {

                },
                close: function (event, ui) {

                },
                captionButtons: {
                    pin: { visible: false },
                    refresh: { visible: false },
                    toggle: { visible: false },
                    minimize: { visible: false },
                    maximize: { visible: false }
                },

                resizable: false,
                width: 1200,
                height: 700,
                autoOpen: false,
                position: 'fixed',
                modal: true
            });
            function rowColorFormatter(cellValue, options, rowObject) {
                if (cellValue === 'Custom')
                    rowsToColor[rowsToColor.length] = options.rowId;
                return cellValue;
            };
            $("#OverallGrid").jqGrid({
                datatype: 'local',
                colNames: ['Facility', 'Time_Period', 'No. of Defects', 'No. of Rejects', 'No. of Inspections', 'No. of Rejected Lots', 'DHU', 'Reject Rate', 'Lot Acceptance', 'Compliance Ratio'],
                colModel: [
                    { name: 'Facility', width: 180, align: 'center', cellattr: arrtSetting },
                    { name: 'Time_Period', width: 180},
                    { name: 'No_of_Defects', width: 180 },
                    { name: 'No_of_Rejects', width: 180 },
                    { name: 'No_of_Inspections', width: 180 },
                    { name: 'No_of_Rejected_Lots', width: 180 },
                    { name: 'DHU', width: 180 },
                    { name: 'Reject_Rate', width: 180 },
                    { name: 'Lot_Acceptance', width: 180},
                    { name: 'Compliance_Ratio', width: 180}
                ],
                cmTemplate: { sortable: false },
                rowNum: 100,
                //rowList: [5, 10, 20],
                //pager: '#pager',
                gridview: true,
                hoverrows: false,

                ignoreCase: true,
                viewrecords: true,
                height: '100%',
                width: '100%',
                shrinkToFit: true,
                beforeSelectRow: function () {
                    return false;
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {

                    var colNames = $(this).jqGrid("getGridParam", "colNames");
                    var GridType = colNames[iCol];
                    var rowNames = $(this).jqGrid("getRowData", iRow);
                    var TP = rowNames.Time_Period;
                    var Fac = rowNames.Facility;
                    var colVal = $(this).jqGrid("getCell", rowid, iCol);
                    drawChart(Fac, GridType, TP);

                }
            });
            $("#InteriorsGrid").jqGrid({
                datatype: 'local',
                colNames: ['Facility', 'Time_Period', 'No. of Defects', 'No. of Rejects', 'No. of Inspections', 'No. of Rejected Lots', 'DHU', 'Reject Rate', 'Lot Acceptance', 'Compliance Ratio'],
                colModel: [
                    { name: 'Facility', width: 180, align: 'center', cellattr: arrtSetting },
                    { name: 'Time_Period', width: 180 },
                    { name: 'No_of_Defects', width: 180 },
                    { name: 'No_of_Rejects', width: 180 },
                    { name: 'No_of_Inspections', width: 180 },
                    { name: 'No_of_Rejected_Lots', width: 180 },
                    { name: 'DHU', width: 180 },
                    { name: 'Reject_Rate', width: 180 },
                    { name: 'Lot_Acceptance', width: 180 },
                    { name: 'Compliance_Ratio', width: 180 }
                ],
                cmTemplate: { sortable: false },
                rowNum: 100,
                //rowList: [5, 10, 20],
                //pager: '#pager',
                gridview: true,
                hoverrows: false,

                ignoreCase: true,
                viewrecords: true,
                height: '100%',
                width: '100%',
                shrinkToFit: true,
                beforeSelectRow: function () {
                    return false;
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {

                    var colNames = $(this).jqGrid("getGridParam", "colNames");
                    var GridType = colNames[iCol];
                    var rowNames = $(this).jqGrid("getRowData", iRow);
                    var TP = rowNames.Time_Period;
                    var Fac = rowNames.Facility;
                    var colVal = $(this).jqGrid("getCell", rowid, iCol);
                    drawChart(Fac, GridType, TP);

                }
            });
            var interiorsgrid = $("#InteriorsGrid");
            var interiorsgview = interiorsgrid.parents("div.ui-jqgrid-view");
            interiorsgview.children("div.ui-jqgrid-hdiv").hide();
            $("#DomesticGrid").jqGrid({
                datatype: 'local',
                colNames: ['Facility', 'Time_Period', 'No. of Defects', 'No. of Rejects', 'No. of Inspections', 'No. of Rejected Lots', 'DHU', 'Reject Rate', 'Lot Acceptance', 'Compliance Ratio'],
                colModel: [
                    { name: 'Facility', width: 180, align: 'center', cellattr: arrtSetting },
                    { name: 'Time_Period', width: 180 },
                    { name: 'No_of_Defects', width: 180 },
                    { name: 'No_of_Rejects', width: 180 },
                    { name: 'No_of_Inspections', width: 180 },
                    { name: 'No_of_Rejected_Lots', width: 180 },
                    { name: 'DHU', width: 180 },
                    { name: 'Reject_Rate', width: 180 },
                    { name: 'Lot_Acceptance', width: 180 },
                    { name: 'Compliance_Ratio', width: 180 }
                ],
                cmTemplate: { sortable: false },
                rowNum: 100,
                //rowList: [5, 10, 20],
                //pager: '#pager',
                gridview: true,
                hoverrows: false,

                ignoreCase: true,
                viewrecords: true,
                height: '100%',
                width: '100%',
                shrinkToFit: true,
                beforeSelectRow: function () {
                    return false;
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {

                    var colNames = $(this).jqGrid("getGridParam", "colNames");
                    var GridType = colNames[iCol];
                    var rowNames = $(this).jqGrid("getRowData", iRow);
                    var TP = rowNames.Time_Period;
                    var Fac = rowNames.Facility;
                    var colVal = $(this).jqGrid("getCell", rowid, iCol);
                    drawChart(Fac, GridType, TP);

                }

            });
            var domesticgrid = $("#DomesticGrid");
            var domesticgview = domesticgrid.parents("div.ui-jqgrid-view");
            domesticgview.children("div.ui-jqgrid-hdiv").hide();
            $("#GlobalGrid").jqGrid({
                datatype: 'local',
                colNames: ['Facility', 'Time_Period', 'No. of Defects', 'No. of Rejects', 'No. of Inspections', 'No. of Rejected Lots', 'DHU', 'Reject Rate', 'Lot Acceptance', 'Compliance Ratio'],
                colModel: [
                    { name: 'Facility', width: 180, align: 'center', cellattr: arrtSetting },
                    { name: 'Time_Period', width: 180 },
                    { name: 'No_of_Defects', width: 180 },
                    { name: 'No_of_Rejects', width: 180 },
                    { name: 'No_of_Inspections', width: 180 },
                    { name: 'No_of_Rejected_Lots', width: 180 },
                    { name: 'DHU', width: 180 },
                    { name: 'Reject_Rate', width: 180 },
                    { name: 'Lot_Acceptance', width: 180 },
                    { name: 'Compliance_Ratio', width: 180 }
                ],
                cmTemplate: { sortable: false },
                rowNum: 100,
                //rowList: [5, 10, 20],
                //pager: '#pager',
                gridview: true,
                hoverrows: false,
                
                ignoreCase: true,
                viewrecords: true,
                height: '100%',
                width: '100%',
                shrinkToFit: true,
                beforeSelectRow: function () {
                    return false;
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {

                    var colNames = $(this).jqGrid("getGridParam", "colNames");
                    var GridType = colNames[iCol];
                    var rowNames = $(this).jqGrid("getRowData", iRow);
                    var TP = rowNames.Time_Period;
                    var Fac = rowNames.Facility;
                    var colVal = $(this).jqGrid("getCell", rowid, iCol);
                    drawChart(Fac, GridType, TP);

                }
                

            });
            var globalgrid = $("#GlobalGrid");
            var globalgview = globalgrid.parents("div.ui-jqgrid-view");
            globalgview.children("div.ui-jqgrid-hdiv").hide();
            $("#MainGrid").jqGrid({
                datatype: 'local',
                colNames: ['Facility', 'Time_Period', 'No. of Defects', 'No. of Rejects', 'No. of Inspections', 'No. of Rejected Lots', 'DHU', 'Reject Rate', 'Lot Acceptance', 'Compliance Ratio'],
                colModel: [
                    { name: 'Facility', width: 180, align: 'center', cellattr: arrtSetting },
                    { name: 'Time_Period', width: 180, formatter: rowColorFormatter },
                    { name: 'No_of_Defects', width: 180 },
                    { name: 'No_of_Rejects', width: 180 },
                    { name: 'No_of_Inspections', width: 180 },
                    { name: 'No_of_Rejected_Lots', width: 180 },
                    { name: 'DHU', width: 180 },
                    { name: 'Reject_Rate', width: 180 },
                    { name: 'Lot_Acceptance', width: 180 },
                    { name: 'Compliance_Ratio', width: 180 }
                ],
                cmTemplate: { sortable: false },
                rowNum: 100,
                //rowList: [5, 10, 20],
                //pager: '#pager',
                gridview: true,
                hoverrows: false,
                subGrid: true,
                subGridRowColapsed: function (pID, id) {

                },
                subGridRowExpanded: function (subgrid_id, row_id) {
                    var grid = $("#MainGrid");
                    prase = new DOMParser();
                    var rowdata = [$("#MainGrid #" + row_id).find("td[aria-describedby='MainGrid_Facility']").html(), $("#MainGrid #" + row_id).find("td[aria-describedby='MainGrid_Time_Period']").html(), $Fromdateval, $Todateval, $("#select-DataNo option:selected").text(), $("#select-WorkOrder option:selected").text(), $("#select-AuditType option:selected").text()];
                    console.log(rowdata);
                    if (rowdata) {
                        if (rowdata.length > 1) {
                            subgridquerystr = "Facility=" + rowdata[0].trim() + "&Time_Period=" + rowdata[1] + "&From_date=" + rowdata[2] + "&To_date=" + rowdata[3] + "&DataNo=" + rowdata[4] + "&WorkOrder=" + rowdata[5] + "&AuditType=" + rowdata[6];
                            console.log(subgridquerystr);
                        }
                    }
                    var subgrid_table_id;
                    subgrid_table_id = subgrid_id + "_t";
                    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
                    jQuery("#" + subgrid_table_id).jqGrid({
                        url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/MainGrid_SubgridLoad.ashx?' + subgridquerystr,
                        datatype: "json",
                        colNames: ['WorkRoom', 'No. of Defects', 'No. of Rejects', 'No. of Inspections', 'No. of Rejected Lots', 'DHU', 'Reject Rate', 'Lot Acceptance', 'Compliance Ratio'],
                        colModel: [
                            { name: 'FacilityWorkroom', width: 180 },
                            { name: 'No_of_Defects', width: 180 },
                            { name: 'No_of_Rejects', width: 180 },
                            { name: 'No_of_Inspections', width: 180 },
                            { name: 'No_of_Rejected_Lots', width: 180 },
                            { name: 'DHU', width: 180 },
                            { name: 'Reject_Rate', width: 180 },
                            { name: 'Lot_Acceptance', width: 180 },
                            { name: 'Compliance_Ratio', width: 180 }
                        ],
                        rowNum: 100,
                        sortname: 'num',
                        sortorder: "asc",
                        height: '100%',
                        onCellSelect: function (row, col, content, event) {
                            var cm = jQuery("#" + subgrid_table_id).jqGrid("getGridParam", "colModel");
                            var GridType = cm[col].name;
                            console.log(GridType);
                            var WorkRoom = jQuery("#" + subgrid_table_id).getLocalRow(row).FacilityWorkroom;
                            console.log(WorkRoom);
                            var TP = rowdata[1];
                            var Fac = rowdata[0].trim();
                            console.log(TP);
                            console.log(Fac);
                            drawWRChart(Fac, GridType, TP, WorkRoom);
                        },
                        loadonce: true,
                        postData: {
                            SessionId: function () {
                                return SessionId;
                            }
                        },
                    });
                    jQuery("#" + subgrid_table_id).jqGrid('navGrid', { edit: false, add: false, del: false })
                },
                ignoreCase: true,
                viewrecords: true,
                height: '100%',
                width: '100%',
                shrinkToFit: true,
                beforeSelectRow: function () {
                    return false;
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {

                    var colNames = $(this).jqGrid("getGridParam", "colNames");
                    var GridType = colNames[iCol];
                    var rowNames = $(this).jqGrid("getRowData", iRow);
                    var TP = rowNames.Time_Period;
                    var Fac = rowNames.Facility;
                    var colVal = $(this).jqGrid("getCell", rowid, iCol);
                    drawChart(Fac, GridType, TP);

                },
                gridComplete: function () {
                    for (var i = 0; i < rowsToColor.length; i++) {

                        $("#" + rowsToColor[i]).find("td").css("background-color", "DarkGrey");

                    }
                }

            });
            
            fchtml.push('<option value="000" >Global Manufacturing</option>');
            dchtml.push('<option value="001" >Domestic Manufacturing</option>');
            dhtml.push('<option value="002" >Distribution</option>');
            ihtml.push('<option value="003" >Interiors</option>');
            $.each(LocationNames, function (index, value) {
                FullLocationsArray.push({ value: value.Abreviation.trim(), status: true, CID: value.CID, ProdAbreviation: value.ProdAbreviation });
                if (value.text.trim() === 'Pakistan' || value.text.trim() === 'El Salvador' || value.text.trim() === 'India' || value.text.trim() === 'China' || value.text.trim() === 'Jordan') {
                    fchtml.push('<option value = "' + value.CID.trim() + '" class="l1">' + value.text.trim() + ' (' + value.Abreviation.trim() + ')</option>');
                    fcid.push(value.CID.trim());
                }
                else if (value.text.trim() === 'Carolina' || value.text.trim() === 'Thomaston' || value.Abreviation.trim() === 'PCE' ) {
                    dchtml.push('<option value = "' + value.CID.trim() + '" class="l1">' + value.text.trim() + ' (' + value.Abreviation.trim() + ')</option>');
                    dcid.push(value.CID.trim());
                }
                else if (value.Abreviation.trim() === 'FNL'  || value.Abreviation.trim() === 'FSK') {
                    ihtml.push('<option value = "' + value.CID.trim() + '" class="l1">' + value.text.trim() + ' (' + value.Abreviation.trim() + ')</option>');
                    iid.push(value.CID.trim());
                }
                else if (value.Abreviation.trim() !== 'ALL' && value.text.trim() !== 'Augusta' && value.Abreviation.trim() !== 'STM' && value.Abreviation.trim() !== 'SAT' && value.Abreviation.trim() !== 'FPC') {
                    dhtml.push('<option value = "' + value.CID.trim() + '" class="l1">' + value.text.trim() + ' (' + value.Abreviation.trim() + ')</option>');
                    did.push(value.CID.trim());
                }
            });
            var cchtml = [];
            cchtml.push('<option value="999" >ALL SITES (ALL)</option>');
            cchtml = cchtml.concat(((fchtml.concat(dchtml)).concat(ihtml)).concat(dhtml));
            LocationsStringArray = JSON.stringify(FullLocationsArray);
            $("#MainContent_SelectedCID").val(LocationsStringArray);
            $("#select-Location_fc").html(cchtml.join(''));
            $("#select-location").html(cchtml.join(''));
            var $locSelect = $("#select-location");
            $locSelect.select2({
                multiple: true,
                templateResult: function (data) {
                    // We only really care if there is an element to pull classes from
                    if (!data.element) {
                        return data.text;
                    }

                    var $element = $(data.element);

                    var $wrapper = $('<span></span>');
                    $wrapper.addClass($element[0].className);

                    $wrapper.text(data.text);

                    return $wrapper;
                }
            });
            document.getElementById("loading").style.display = "block";
            
            datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);
            $locSelect.val(selList).trigger("change");
            $locSelect.on("select2:select", function (e) {
                document.getElementById("loading").style.display = "block";
                var selectedItems = e.currentTarget.selectedOptions;
                selList = [];
                var combined;
                var selval = e.params.data.id;
                $.each(selectedItems, function (key, value) {
                    var optval = $(value).val();
                    var opttext = $(value).text();
                    console.log(optval);
                    console.log(opttext);
                    if (opttext === "Global Manufacturing") {
                        selList = selList.concat(fcid);
                        console.log(fcid);

                    }
                    else if (opttext === "Domestic Manufacturing") {
                        selList = selList.concat(dcid);
                        console.log(dcid);

                    }
                    else if (opttext === "Interiors") {
                        selList = selList.concat(iid);
                        console.log(iid);

                    }
                    else if (opttext === "Distribution") {
                        selList = selList.concat(did);
                        console.log(did);

                    }
                    else if (optval != "999" && selval != "999") {
                        selList.push(optval);
                    }
                    else if (selval == "999" && optval == "999") {
                        selList = ((((selList.concat(dcid))).concat(iid)).concat(fcid)).concat(did);
                    }

                });
                console.log("selList", selList);
                if (selList.length > 0) {
                    $locSelect.val(selList).trigger("change");
                    console.log(fromdate);
                    console.log(todate);

                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);

                } else {
                    $('#MainGrid').jqGrid("clearGridData")
                    $('#MainGrid').trigger('reloadGrid');
                }

            });
            $locSelect.on("select2:unselect", function (e) {
                document.getElementById("loading").style.display = "block";
                var selectedItems = e.currentTarget.selectedOptions;
                selList = [];
                $.each(selectedItems, function (key, value) {
                    var optval = $(value).val();
                    selList.push(optval);
                });
                if (selList.length > 0) {
                    $locSelect.val(selList).trigger("change");
                    console.log(fromdate);
                    console.log(todate);

                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);

                } else {
                    $('#MainGrid').jqGrid("clearGridData")
                    $('#MainGrid').trigger('reloadGrid');
                    document.getElementById("loading").style.display = "none";
                }

            });
            $("#select-DataNo").change(function () {
                $DataNo = $("#select-DataNo :selected").text();
                console.log($DataNo);
                document.getElementById("loading").style.display = "block";
                datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);
            });
            $("#select-WorkOrder").change(function () {
                $WorkOrder = $("#select-WorkOrder :selected").text();
                console.log($WorkOrder);
                document.getElementById("loading").style.display = "block";
                datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);
            });
            $("#select-AuditType").change(function () {
                $AuditType = $("#select-AuditType :selected").text();
                console.log($AuditType);
                document.getElementById("loading").style.display = "block";
                datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);
            });
            //$("#Locations").html(html.join(''));
            var html = [];
            var ithtml = [];


            if (InspectionTypesArray) {
                ithtml.push('<option value="ALL">ALL</option>');
                $.each(InspectionTypesArray, function (k, value) {

                    ithtml.push('<option value="' + value.Abreviation + '">' + value.Name + '</option>');
                });
                $("#select-AuditType").html(ithtml.join(''));
                document.getElementById("select-AuditType").selectedIndex = 1;
            }

            $("#menudiv").toggle();
            $("#loginView").toggle();
            $('#Locations .owl-item').each(function (index) {
                var textsel = $(this).text();

                ActiveFilter = "LOCATION";
                if (textsel == 'ALL') {
                    $(this).css('background', '#7fc242').addClass('clicked')
                    ActiveFilter = "ALL";
                }
            });

            $('#ab').on('click', function (event) {
                $locSelect.val([]).trigger("change");
                $('#MainGrid').jqGrid("clearGridData")
                $('#MainGrid').trigger('reloadGrid');
            });
            $('#ab2').on('click', function (event) {
                $('#InstructionDialog').wijdialog('open')
            });
            $('#backdiv').on('click', function (event) {

                window.location.assign('../../APP/APR_SiteEntry.aspx')
            });

            $("#ExpandedImageCloser").on('click', function (event) {
                $("#ExpandedImageHolder").toggle();
            });

            $('.lnk_doc_type').on('click', function (e) {
                $("#MainContent_ActiveReportId").val(e.currentTarget.id);
                $('#MainContent_ReportCallBack').trigger('click');
            });
            $("#TxtDateFrom").wijinputdate({
                dateFormat: 'd',
                dateChanged: function (e, data) {
                    document.getElementById("loading").style.display = "block";
                    var formatted_Fromdate = (data.date.getMonth() + 1) + "/" + data.date.getDate() + "/" + data.date.getFullYear();
                    FilterSource = 'PageFilter';
                    fromdate = formatted_Fromdate;
                    $("#MainContent_DateFrom_Hidden").val(formatted_Fromdate);
                    $Fromdateval = formatted_Fromdate;
                    LineGraphldcnt = 0;
                    BreakOutldcnt = 0;
                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);
                    datahandler.FilterEvent(formatted_Fromdate, "Date");
                },
                date: $Fromdateval
            });
            $("#MainContent_DateFrom_Hidden").val($Fromdateval);
            $("#TxtDateTo").wijinputdate({
                dateFormat: 'd',
                dateChanged: function (e, data) {
                    document.getElementById("loading").style.display = "block";
                    var formatted_Todate = (data.date.getMonth() + 1) + "/" + data.date.getDate() + "/" + data.date.getFullYear();
                    todate = formatted_Todate;
                    FilterSource = 'PageFilter';
                    $("#MainContent_DateTo_Hidden").val(formatted_Todate);
                    $Todateval = formatted_Todate;
                    LineGraphldcnt = 0;
                    BreakOutldcnt = 0;
                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo, $WorkOrder, $AuditType);
                    datahandler.FilterEvent(formatted_Todate, "Date");
                },
                date: $Todateval
            });
            $("#MainContent_DateTo_Hidden").val($Todateval);
            var ytdsliderfromdate = new Date(exytd_DateFrom_fc);
            var mindate = new Date('1/1/2015');
            var ytdslidertodate = new Date(exytd_DateTo_fc);

            $("#ytdfromdatelbl").text(exytd_DateFrom_fc);
            $("#ytdtodatelbl").text(exytd_DateTo_fc);
            $("#exytd_TxtDateFrom_fc").wijinputdate({
                dateFormat: 'd',
                dateChanged: function (e, data) {
                    FilterSource = 'ColumnFilters';
                    exytd_DateFrom_fc = (data.date.getMonth() + 1) + "/" + data.date.getDate() + "/" + data.date.getFullYear();
                    $("#ovsgrid").jqGrid('setGridParam',
                        { datatype: 'json' }).trigger('reloadGrid');
                },
                date: exytd_DateFrom_fc
            });

            $("#exytd_TxtDateTo_fc").wijinputdate({
                dateFormat: 'd',
                dateChanged: function (e, data) {
                    FilterSource = 'ColumnFilters';
                    exytd_DateTo_fc = (data.date.getMonth() + 1) + "/" + data.date.getDate() + "/" + data.date.getFullYear();
                    $("#ovsgrid").jqGrid('setGridParam',
                        { datatype: 'json' }).trigger('reloadGrid');
                },
                date: exytd_DateTo_fc
            });
            $("#exmtd_TxtDateFrom_fc").wijinputdate({
                dateFormat: 'd',
                dateChanged: function (e, data) {
                    FilterSource = 'ColumnFilters';
                    exmtd_DateFrom_fc = (data.date.getMonth() + 1) + "/" + data.date.getDate() + "/" + data.date.getFullYear();
                    $("#ovsgrid").jqGrid('setGridParam',
                        { datatype: 'json' }).trigger('reloadGrid');
                },
                date: exmtd_DateFrom_fc
            });

            $("#exmtd_TxtDateTo_fc").wijinputdate({
                dateFormat: 'd',
                dateChanged: function (e, data) {
                    FilterSource = 'ColumnFilters';
                    exmtd_DateTo_fc = (data.date.getMonth() + 1) + "/" + data.date.getDate() + "/" + data.date.getFullYear();
                    $("#ovsgrid").jqGrid('setGridParam',
                        { datatype: 'json' }).trigger('reloadGrid');
                },
                date: exmtd_DateTo_fc
            });

            $("#Graphs .owl-item").css("height", ($('#tabs').height() - 125).toString() + "px")
            //datahandler.GetDefectImages("999");
            //datahandler.GetDHULine();
            //datahandler.GetDataNos();
            //datahandler.GetWorkOrders();
            //datahandler.GetREJLine();
            grids.RenderOvsgrid();

            $('#Graph2Holder').css("display", "none");
            $('select-graph').val("LineGraph");
            $('#tabs').fadeIn(230);
            //datahandler.GetDHU_Scat(); 
            //callajax();
        });
        var carousel = {
            animateCustomersOn: function () {
                var container = $('#customers');
                var elements = container.children();
                var totalCount = elements.length;
                var itemWidth = totalCount > 0 ? parseInt($(".customerItem:eq(0)").outerWidth()) : 0;
                var width = totalCount > 0 ? ((totalCount * itemWidth) + (totalCount * 10)) : $(window).width();
                var counter = 1;

                $("#customersWrapper").width(width + "px");

                var standartransitionend = (!!window.webkitURL) ? 'webkitTransitionEnd' : 'transitionend';

                var sliderWidth = $("#customersWrapper").outerWidth();
                var itemGroupCount = width < $(window).width() ? Math.floor(width / itemWidth) : Math.ceil($(window).width() / itemWidth);
                var itemGroupWidth = (itemGroupCount * 10) + (itemWidth * itemGroupCount); //10 = margin added to each item
                maxLeft = -1 * (sliderWidth - (itemGroupWidth - itemWidth));

                $("#customersWrapper").unbind("mousedown");

                //$.unblockUI();
                $("#customersWrapper").bind("mousedown", function (e) {
                    var evt = e || window.event;
                    var offset = $(this).offset();

                    $(this).addClass("draggable");
                    startX = offset.left;
                    initialMouseX = evt.clientX;
                    draggedObject = parent;

                    $(this).bind("mousemove", function (e) {
                        var evt = e || window.event;
                        var dX = evt.clientX - initialMouseX;

                        dragging = Math.abs(dX) > 40;

                        $(this).css({ left: (startX + dX) + 'px' });
                    }).bind("mouseup", function (e) {
                        carousel.setSlider(itemGroupWidth);
                        //  $('html, body').animate({ scrollTop: document.body.scrollTop }, 1); 


                    }).bind("mouseleave", function () {
                        carousel.setSlider(itemGroupWidth);
                    });

                    return false;
                });
                canScrollClients = true;
                if (totalCount == 0) {
                    //$.unblockUI();
                    canScrollClients = false;
                }

            },
            animateLocationsOn: function () {
                var container = $('#Lcustomers');
                var elements = container.children();
                var totalCount = elements.length;
                var itemWidth = totalCount > 0 ? parseInt($(".LcustomerItem:eq(0)").outerWidth()) : 0;
                var width = totalCount > 0 ? ((totalCount * itemWidth) + (totalCount * 10)) : $(window).width();
                var counter = 1;

                $("#LcustomersWrapper").width(width + "px");

                var standartransitionend = (!!window.webkitURL) ? 'webkitTransitionEnd' : 'transitionend';

                var sliderWidth = $("#LcustomersWrapper").outerWidth();
                var itemGroupCount = width < $(window).width() ? Math.floor(width / itemWidth) : Math.ceil($(window).width() / itemWidth);
                var itemGroupWidth = (itemGroupCount * 10) + (itemWidth * itemGroupCount); //10 = margin added to each item
                LmaxLeft = -1 * (sliderWidth - (itemGroupWidth - itemWidth) + 335);

                $("#LcustomersWrapper").unbind("mousedown");

                //$.unblockUI();
                $("#LcustomersWrapper").bind("mousedown", function (e) {
                    var evt = e || window.event;
                    var offset = $(this).offset();

                    $(this).addClass("draggable");
                    LstartX = offset.left - 140;
                    LinitialMouseX = evt.clientX;
                    LdraggedObject = parent;

                    $(this).bind("mousemove", function (e) {
                        var evt = e || window.event;
                        var dX = evt.clientX - LinitialMouseX;

                        Ldragging = Math.abs(dX) > 40;

                        $(this).css({ left: (LstartX + dX) + 'px' });
                    }).bind("mouseup", function (e) {
                        carousel.LocationsetSlider(itemGroupWidth);
                    }).bind("mouseleave", function () {
                        carousel.LocationsetSlider(itemGroupWidth);
                    });

                    return false;
                });
                LcanScrollClients = true;
                if (totalCount == 0) {
                    //$.unblockUI();
                    LcanScrollClients = false;
                }

            },
            setSlider: function (itemGroupWidth) {
                var el = $("#customersWrapper");
                el.unbind("mousemove").unbind("mouseup").removeClass("draggable").unbind("mouseleave");
                var newoffset = el.offset();

                if (newoffset.left > 0 || (newoffset.left < 0 && itemGroupWidth < $(window).width())) {
                    el.animate({
                        left: "0px"
                    }, 800, "swing", function () { dragging = false; });
                } else if (newoffset.left < maxLeft) {
                    el.animate({
                        left: (maxLeft + "px")
                    }, 800, "swing", function () { dragging = false; });
                }
                ;
            },
            LocationsetSlider: function (itemGroupWidth) {
                var el = $("#LcustomersWrapper");
                el.unbind("mousemove").unbind("mouseup").removeClass("draggable").unbind("mouseleave");
                var newoffset = el.offset();

                if (newoffset.left > 0 || (newoffset.left < 0 && itemGroupWidth < $(window).width() * .9)) {
                    el.animate({
                        left: "-0px"
                    }, 800, "swing", function () { Ldragging = false; });
                } else if (newoffset.left < LmaxLeft) {
                    el.animate({
                        left: (LmaxLeft + "px")
                    }, 800, "swing", function () { Ldragging = false; });
                }
                Ldragging = false;
            },
            EnlargeMe: function (ImageId) {
                var Holder = $("#ExpandedImageHolder");
                var Pad = $("#ExpandedImagePad");
                var Image = $("#ExpandedImage");
                var ThmbImage = $("#Image_" + ImageId.toString());
                var AspectRatio = ThmbImage.height() / ThmbImage.width();
                var UsedArray = [];
                if (DefectPictureArrayF.length > 0) {
                    UsedArray = $.extend(true, [], DefectPictureArrayF);
                } else {
                    UsedArray = $.extend(true, [], DefectPictureArray);
                }

                Holder.toggle();
                Holder.css({ "width": screen.width.toString() + "px", "height": screen.height.toString() + "px", "top": -30 + "px" });
                Pad.css({ "width": (screen.width - 80).toString() + "px", "height": (screen.height - 70).toString() + "px" })
                Image.css({ "width": (screen.width - 140).toString() + "px", "height": (screen.height - 140).toString() + "px", "top": 30 + "px" });
                Image.attr("src", UsedArray[ImageId - 1].imageUrl);

                return false;
            }

        };
        var grids = {
            RenderOvsgrid: function () {
                var rowcnt = $("#tabs").height() / 22.5225;

                $("#ovsgrid").jqGrid({
                    datatype: "json",
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/ResultsOverview_Load.ashx',
                    colNames: ['Type', 'YTD', 'MTD', 'F-YTD', 'F-MTD'],
                    colModel: [
                        { name: 'Type', index: 'Type', width: 170 },
                        { name: 'ytd', index: 'ytd', editable: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'mtd', index: 'mtd', editable: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'F_ytd', index: 'F_ytd', editable: false, hidden: true, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'F_mtd', index: 'F_mtd', editable: false, hidden: true, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                    ],
                    autowidth: false,
                    caption: "INSPECTION OVERVIEW",
                    shrinkToFit: true,
                    width: $('#tabs').width() * .35,
                    height: 'auto',
                    multiselect: false,
                    viewrecords: true,
                    gridview: true,
                    scroll: false,
                    loadonce: false,
                    gridComplete: function () {
                        if (SelectedTab == "Overview") {
                            OwFilterFlag = false;
                        }
                    },
                    postData: {
                        CID: function () {
                            return SelectedCID;
                        },
                        todate: function () {
                            return $("#TxtDateTo").val();
                        },
                        DataNumber: function () {
                            return $("#select-DataNo").val();
                        },
                        AuditType: function () {
                            return $("#select-AuditType").val();
                        },
                        ActiveFilterField: function () {
                            return ActiveFilter;
                        },
                        FilterListstring: function () {
                            return ActiveFilterString;
                        },
                        FilterFlag: function () {
                            return OwFilterFlag;
                        },
                        FilterColumnName: function () {
                            return FilterColumnName;
                        },
                        Filterqvalue: function () {
                            return Filterqvalue;
                        },
                        FilterSource: function () {
                            return FilterSource;
                        },
                        ytd_DateFrom_fc: function () {
                            return exytd_DateFrom_fc;
                        },
                        ytd_DateTo_fc: function () {
                            return exytd_DateTo_fc;
                        },
                        mtd_DateFrom_fc: function () {
                            return exmtd_DateFrom_fc;
                        },
                        mtd_DateTo_fc: function () {
                            return exmtd_DateTo_fc
                        },
                        SessionID: function () {
                            return SessionId;
                        },
                        LocationArrayString: function () {
                            return LocationsStringArray;
                        }
                    },
                    paging: false
                });
            },
            RSSelectedRowCID: "",

            Selected_ijs_id: 0,






            setSearchSelect: function (columnName, tableid) {

                var names = grids.GetUniqueNames(columnName, tableid);
                var selectstr = '<select id="gs_' + columnName + '" class="ijsDropDown" style = "width:100%; padding:0px; height: 40px"><option value="ALL">ALL</option>'
                $.each(names, function (k, v) {
                    selectstr = selectstr + '<option value="' + v + '">' + v + '</option>'
                });

                selectstr = selectstr + '</select>'

                $('#gs_' + columnName).replaceWith(selectstr);
                //$('#' + tableid).jqGrid('setColProp', columnName,
                //            {
                //                stype: 'select',
                //                searchoptions: {
                //                    value:grids.buildSearchSelect(grids.GetUniqueNames(columnName,  tableid)),
                //                    sopt:['eq']
                //                }
                //            }
                //);
                //$("#gs_" + columnName).css("height", "40px");
            },


        };




        function FixJson(str) {
            return str
                // wrap keys without quote with valid double quote
                .replace(/([\$\w]+)\s*:/g, function (_, $1) { return '"' + $1 + '":' })
                // replacing single quote wrapped ones to double quote 
                .replace(/'([^']+)'/g, function (_, $1) { return '"' + $1 + '"' })
        };
        var mydata = [];
        var datahandler = {


            DrillDownWR: function (Date, Facility, GridType, TimePeriod, WorkRoom, fromdate, todate, DataNo, WorkOrder, AuditType) {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'DrillDownWR', args: { dt: Date, fac: Facility, gt: GridType, tp: TimePeriod, wr: WorkRoom, from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        var conversion = JSON.parse(data);
                        console.log(conversion);
                        var dataarray = new google.visualization.DataTable();
                        if (GridType === 'No_of_Defects') {
                            dataarray.addColumn('string', 'Type');
                            dataarray.addColumn('number', GridType);
                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(parseFloat(conversion[i][1]));
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");
                        }
                        if (GridType === 'No_of_Rejects') {
                            dataarray.addColumn('string', 'Type');
                            dataarray.addColumn('number', GridType);
                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(parseFloat(conversion[i][1]));
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");
                        }
                        if (GridType === 'No_of_Inspections') {
                            dataarray.addColumn('string', 'Id');
                            dataarray.addColumn('string', 'Job Number');
                            dataarray.addColumn('string', 'Data Number');

                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(conversion[i][1]);
                                dummy.push(conversion[i][2]);
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");
                        }
                        if (GridType === 'No_of_Rejected_Lots') {
                            dataarray.addColumn('string', 'Id');
                            dataarray.addColumn('string', 'Job Number');
                            dataarray.addColumn('string', 'Data Number');

                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(conversion[i][1]);
                                dummy.push(conversion[i][2]);
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");

                        }
                        document.getElementById("loading").style.display = "none";
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            DrawWRChart: function (Facility, GridType, TimePeriod, WorkRoom, fromdate, todate, DataNo, WorkOrder, AuditType) {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'DrawWRChart', args: { fac: Facility, gt: GridType, tp: TimePeriod, wr: WorkRoom, from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        var conversion = JSON.parse(data);
                        console.log(conversion);
                        var dataarray = new google.visualization.DataTable();
                        dataarray.addColumn('date', 'Days');
                        dataarray.addColumn('number', GridType);
                        GraphData = [];
                        var dummy = [];
                        for (i = 0; i < conversion.length; i++) {
                            console.log(conversion[i]);
                            dummy = [];

                            dummy.push(new Date(conversion[i][0]))
                            dummy.push(parseFloat(conversion[i][1]))

                            console.log(dummy);
                            GraphData.push(dummy);
                        }
                        console.log(GraphData);
                        dataarray.addRows(GraphData);

                        var options = {
                            title: 'Graph of ' + Facility + ' and ' + GridType + ' over ' + TimePeriod + ' in ' + WorkRoom,
                            focusTarget: 'datum',
                            legend: 'none',
                            width: 1000,
                            height: 520,
                            forceIFrame: true
                        };

                        var chart = new google.visualization.LineChart(document.getElementById('chart_div'));


                        google.visualization.events.addListener(chart, 'ready', function () {
                            URIString = chart.getImageURI();
                        });

                        chart.draw(dataarray, options);
                        google.visualization.events.addListener(chart, 'select', selectHandler);
                        function selectHandler() {
                            var selection = chart.getSelection();
                            var message = '';

                            var item = selection[0];
                            if (item.row != null) {
                                document.getElementById("loading").style.display = "block";
                                datahandler.DrillDownWR((GraphData[item.row][0].getMonth() + 1) + "/" + GraphData[item.row][0].getDate() + "/" + (GraphData[item.row][0].getYear() + 1900), Facility, GridType, TimePeriod, WorkRoom, fromdate, todate, DataNo, WorkOrder, AuditType);
                            }
                        }
                        $(".GDIV").wijdialog("open");
                        document.getElementById("loading").style.display = "none";
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            DrillDown: function (Date, Facility, GridType, TimePeriod, fromdate, todate, DataNo, WorkOrder, AuditType) {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'DrillDown', args: { dt: Date, fac: Facility, gt: GridType, tp: TimePeriod, from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        var conversion = JSON.parse(data);
                        console.log(conversion);
                        var dataarray = new google.visualization.DataTable();
                        if (GridType === 'No. of Defects') {
                            dataarray.addColumn('string', 'Type');
                            dataarray.addColumn('number', GridType);
                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(parseFloat(conversion[i][1]));
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");
                        }
                        if (GridType === 'No. of Rejects') {
                            dataarray.addColumn('string', 'Type');
                            dataarray.addColumn('number', GridType);
                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(parseFloat(conversion[i][1]));
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");
                        }
                        if (GridType === 'No. of Inspections') {
                            dataarray.addColumn('string', 'Id');
                            dataarray.addColumn('string', 'Job Number');
                            dataarray.addColumn('string', 'Data Number');

                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(conversion[i][1]);
                                dummy.push(conversion[i][2]);
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");

                        }
                        if (GridType === 'No. of Rejected Lots') {
                            dataarray.addColumn('string', 'Id');
                            dataarray.addColumn('string', 'Job Number');
                            dataarray.addColumn('string', 'Data Number');

                            NodeGraphData = [];
                            var dummy = [];

                            for (i = 0; i < conversion.length; i++) {
                                dummy = [];
                                dummy.push(conversion[i][0]);
                                dummy.push(conversion[i][1]);
                                dummy.push(conversion[i][2]);
                                NodeGraphData.push(dummy);
                            }
                            dataarray.addRows(NodeGraphData);
                            var options = {
                                title: 'Defects',
                                width: 1000,
                                height: 520,
                                forceIFrame: true
                            };
                            var chart = new google.visualization.Table(document.getElementById('table_div'));
                            chart.draw(dataarray, options);
                            $("#TableDialog").wijdialog("open");

                        }
                        document.getElementById("loading").style.display = "none";
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            DrawChart: function (Facility, GridType, TimePeriod, fromdate, todate, DataNo, WorkOrder, AuditType) {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'DrawChart', args: { fac: Facility, gt: GridType, tp: TimePeriod, from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        var conversion = JSON.parse(data);
                        console.log(conversion);
                        console.log(Facility);
                        var dataarray = new google.visualization.DataTable();
                        dataarray.addColumn('date', 'Days');
                        dataarray.addColumn('number', GridType);
                        GraphData = [];
                        var dummy = [];
                        for (i = 0; i < conversion.length; i++) {
                            console.log(conversion[i]);
                            dummy = [];
                            //WHAT DATA?

                            dummy.push(new Date(conversion[i][0]))
                            dummy.push(parseFloat(conversion[i][1]))

                            console.log(dummy);
                            GraphData.push(dummy);
                        }
                        console.log(GraphData);
                        dataarray.addRows(GraphData);

                        var options = {
                            title: 'Graph of ' + Facility + ' and ' + GridType + ' over ' + TimePeriod,
                            focusTarget: 'datum',
                            legend: 'none',
                            width: 1000,
                            height: 520,

                            forceIFrame: true
                        };
                        var chart_div = document.getElementById('chart_div');
                        var chart = new google.visualization.LineChart(document.getElementById('chart_div'));


                        google.visualization.events.addListener(chart, 'ready', function () {
                            URIString = chart.getImageURI();
                        });
                        chart.draw(dataarray, options);
                        google.visualization.events.addListener(chart, 'select', selectHandler);
                        function selectHandler() {
                            var selection = chart.getSelection();
                            var message = '';

                            var item = selection[0];
                            if (item.row != null) {
                                //GraphData[item.row][0]
                                document.getElementById("loading").style.display = "block";
                                datahandler.DrillDown((GraphData[item.row][0].getMonth() + 1) + "/" + GraphData[item.row][0].getDate() + "/" + (GraphData[item.row][0].getYear() + 1900), Facility, GridType, TimePeriod, fromdate, todate, DataNo, WorkOrder, AuditType);
                            }
                        }
                        $(".GDIV").wijdialog("open");
                        document.getElementById("loading").style.display = "none";
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            OverallLocationChangeEvent: function (fromdate, todate, DataNo, WorkOrder, AuditType) {

                //alert('Location Changed!');
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetOverallDataArray', args: { from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        mydata = [];
                        console.log(data);
                        var conversion = JSON.parse(data);
                        for (i = 0; i < conversion.length; i++) {
                            mydata.push(JSON.parse(FixJson(conversion[i])));
                        }
                        console.log(mydata);
                        $('#OverallGrid').jqGrid("clearGridData");
                        $('#OverallGrid').jqGrid('setGridParam', { data: mydata });
                        $('#OverallGrid').trigger('reloadGrid');
                        document.getElementById("loading").style.display = "none";
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            InteriorsLocationChangeEvent: function (fromdate, todate, DataNo, WorkOrder, AuditType) {

                //alert('Location Changed!');
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetInteriorsDataArray', args: { from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        mydata = [];
                        console.log(data);
                        var conversion = JSON.parse(data);
                        for (i = 0; i < conversion.length; i++) {
                            mydata.push(JSON.parse(FixJson(conversion[i])));
                        }
                        console.log(mydata);
                        $('#InteriorsGrid').jqGrid("clearGridData");
                        $('#InteriorsGrid').jqGrid('setGridParam', { data: mydata });
                        $('#InteriorsGrid').trigger('reloadGrid');
                        datahandler.OverallLocationChangeEvent(fromdate, todate, DataNo, WorkOrder, AuditType);
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            DomesticLocationChangeEvent: function (fromdate, todate, DataNo, WorkOrder, AuditType) {

                //alert('Location Changed!');
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDomesticDataArray', args: { from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        mydata = [];
                        console.log(data);
                        var conversion = JSON.parse(data);
                        for (i = 0; i < conversion.length; i++) {
                            mydata.push(JSON.parse(FixJson(conversion[i])));
                        }
                        console.log(mydata);
                        $('#DomesticGrid').jqGrid("clearGridData");
                        $('#DomesticGrid').jqGrid('setGridParam', { data: mydata });
                        $('#DomesticGrid').trigger('reloadGrid');
                        datahandler.InteriorsLocationChangeEvent(fromdate, todate, DataNo, WorkOrder, AuditType);
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            GlobalLocationChangeEvent: function (fromdate, todate, DataNo, WorkOrder, AuditType) {
                
                //alert('Location Changed!');
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetGlobalDataArray', args: {from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        mydata = [];
                        console.log(data);
                        var conversion = JSON.parse(data);
                        for (i = 0; i < conversion.length; i++) {
                            mydata.push(JSON.parse(FixJson(conversion[i])));
                        }
                        console.log(mydata);
                        $('#GlobalGrid').jqGrid("clearGridData");
                        $('#GlobalGrid').jqGrid('setGridParam', { data: mydata });
                        $('#GlobalGrid').trigger('reloadGrid');
                        datahandler.DomesticLocationChangeEvent(fromdate, todate, DataNo, WorkOrder, AuditType);
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            LocationChangeEvent: function (cidArray, fromdate, todate, DataNo, WorkOrder, AuditType) {
                console.log('cidArray:' + cidArray);
                //alert('Location Changed!');
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDataArray', args: { array: cidArray, from: fromdate, toDate: todate, DN: DataNo, WO: WorkOrder, AT: AuditType } },
                    success: function (data) {
                        mydata = [];
                        console.log(data);
                        var conversion = JSON.parse(data);
                        for (i = 0; i < conversion.length; i++) {
                            mydata.push(JSON.parse(FixJson(conversion[i])));
                        }
                        console.log(mydata);
                        $('#MainGrid').jqGrid("clearGridData");
                        $('#MainGrid').jqGrid('setGridParam', { data: mydata });
                        $('#MainGrid').trigger('reloadGrid');
                        datahandler.GetDataNos(fromdate, todate, cidArray, AuditType);
                        datahandler.GetWorkOrders(fromdate, todate, cidArray, AuditType);
                        datahandler.GlobalLocationChangeEvent(fromdate, todate, DataNo, WorkOrder, AuditType);
                        
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },

            GetWorkOrders: function (FD, TD, cidArray, AT) {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetWorkOrders', args: { fromdate: FD, todate: TD, LocArray: cidArray, AuditType: AT } },
                    success: function (data) {
                        var json = $.parseJSON(data);

                        //console.log(json);
                        selelm = $("#select-WorkOrder");
                        selelm.empty();
                        var html = [];
                        var name;

                        html.push('<option value="ALL">ALL</option>');

                        for (var i = 0; i < json.length; i++) {
                            name = json[i];
                            html.push('<option value="' + name.id + '">' + name.id + '</option>');
                        }
                        selelm.html(html.join(''));
                        //console.log($DataNo);
                        selelm.val($WorkOrder);
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });

            },
            GetDataNos: function (FD, TD, cidArray, AT) {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDataNos', args: { fromdate: FD, todate: TD, LocArray: cidArray, AuditType: AT } },
                    success: function (data) {
                        var json = $.parseJSON(data);

                        //console.log(json);
                        selelm = $("#select-DataNo");
                        selelm.empty();
                        var html = [];
                        var name;

                        html.push('<option value="ALL">ALL</option>');

                        for (var i = 0; i < json.length; i++) {
                            name = json[i];
                            html.push('<option value="' + name.id + '">' + name.id + '</option>');
                        }
                        selelm.html(html.join(''));
                        //console.log($DataNo);
                        selelm.val($DataNo);
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });

            },


        };


    </script>

</asp:Content>

