<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Site_2.master" AutoEventWireup="false" CodeFile="InspectionVisualizer.aspx.vb" Inherits="core.APP_Presentation_InspectionVisualizer" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%--<%@ OutputCache Location="Server" VaryByParam="*" Duration="2000" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .ui-widget-content td {
            font-size: 1.2em;
        }

        .l1 {
            padding-left: 1em;
        }

        .loading {
            position: fixed;
            z-index: 999;
            height: 2em;
            width: 2em;
            overflow: show;
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
    <div style="position: absolute; width: 130px; left: 85%; height: 150px;">
        <div id="actionbuttons" style="width: 110px; margin: 0px; position: relative; float: right; top: -2px;">
            <a href="#" style="color: white !important; font-size: 19px; width: 65px; height: 60px;" class="actionButton">Clear
            <br />
                Search
            </a>
        </div>
    </div>
    <div id="PageFilters">
        <div style="position: absolute; z-index: 100; left: 95.2%;" id="filterdiv">
            <a id="hideFilters" style="position: absolute; border-style: outset;"></a>
            <a id="showFilters" style="position: absolute; display: none; border-style: outset;"></a>
        </div>
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
            <div data-role="fieldcontain" style="position: absolute; top: 30px; left: 90%; z-index: 100;">
                <label for="select-based-flipswitch" class="filterlabel">Prp Code:</label>
                <div style="position: relative;">
                    <select id="select-prp" style="position: absolute; left: -2px; width: 150px;" data-role="flipswitch" class="selector" multiple="multiple">
                    </select>
                </div>
            </div>
        </div>
        <div id="GridDiv" style="position: relative; left: -10px; top: 170px; width: 100%; height: 100px;">
            <table id="MainGrid" style="width: 100%;">
            </table>
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
        var $AuditType = "ALL";
        var $DataNo = "ALL";
        var $WorkOrder = "ALL";
        var ActiveFilter = "ALL";
        var DefectPictureArray = [];
        var DefectPictureArrayF = [];
        var selectFiltervalues = [{ col: "id", val: "ALL" }, { col: "JobNumber", val: "ALL" }, { col: "UnitDesc", val: "ALL" }, { col: "Name", val: "ALL" }, { col: "Technical_PassFail", val: "ALL" }];
        var selectSpecFiltervalues = [{ col: "id", val: "ALL" }, { col: "JobNumber", val: "ALL" }, { col: "UnitDesc", val: "ALL" }, { col: "DataNo", val: "ALL" }];
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
            var selList = ["661", "662", "482", "486", "485", "578", "113", "112", "111", "1001", "115", "114", "627", "590", "643", "488"];
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

            $("#MainGrid").jqGrid({
                datatype: 'local',
                colNames: ['Facility', 'Time_Period', 'No. of Defects', 'No. of Rejects', 'No. of Inspections', 'No. of Rejected Lots', 'DHU', 'Reject Rate', 'Lot Acceptance'],
                colModel: [
                    { name: 'Facility', width: 200, align: 'center', cellattr: arrtSetting },
                    { name: 'Time_Period', width: 200 },
                    { name: 'No_of_Defects', width: 200 },
                    { name: 'No_of_Rejects', width: 200 },
                    { name: 'No_of_Inspections', width: 200 },
                    { name: 'No_of_Rejected_Lots', width: 200 },
                    { name: 'DHU', width: 200 },
                    { name: 'Reject_Rate', width: 200 },
                    { name: 'Lot_Acceptance', width: 200 }
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
                    var colName = colNames[iCol];
                    var colVal = $(this).jqGrid("getCell", rowid, iCol);
                    alert('the value of this cell is ' + colVal);
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
                else if (value.text.trim() === 'Augusta' || value.text.trim() === 'Carolina' || value.text.trim() === 'Thomaston' || value.Abreviation.trim() === 'PCE' || value.Abreviation.trim() === 'STM' || value.Abreviation.trim() === 'SAT') {
                    dchtml.push('<option value = "' + value.CID.trim() + '" class="l1">' + value.text.trim() + ' (' + value.Abreviation.trim() + ')</option>');
                    dcid.push(value.CID.trim());
                }
                else if (value.Abreviation.trim() === 'FNL' || value.Abreviation.trim() === 'FPC' || value.Abreviation.trim() === 'FSK') {
                    ihtml.push('<option value = "' + value.CID.trim() + '" class="l1">' + value.text.trim() + ' (' + value.Abreviation.trim() + ')</option>');
                    iid.push(value.CID.trim());
                }
                else if (value.Abreviation.trim() !== 'ALL') {
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
            datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo);
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
                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo);
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
                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo);
                } else {
                    $('#MainGrid').jqGrid("clearGridData")
                    $('#MainGrid').trigger('reloadGrid');
                }
                
            });
            $("#select-DataNo").change(function () {
                $DataNo = $("#select-DataNo :selected").text();
                console.log($DataNo);
                document.getElementById("loading").style.display = "block";
                datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo);
            });
            //$("#Locations").html(html.join(''));
            var html = [];

            datahandler.Get_Prpselect();
            datahandler.GetDefectImageDescList();
            //add items to html
            $("#Lcustomers").html(html.join(''));

            //animate the items to view
            carousel.animateLocationsOn();

            $("#tabs").css({ height: $(window).height() + 200 });
            var ithtml = [];


            if (InspectionTypesArray) {
                ithtml.push('<option value="ALL">ALL</option>');
                $.each(InspectionTypesArray, function (k, value) {

                    ithtml.push('<option value="' + value.Abreviation + '">' + value.Name + '</option>');
                });
                $("#select-AuditType").html(ithtml.join(''));
            }
            $("#filter-dialog").wijdialog({
                autoOpen: true,
                height: 430,
                width: 520,
                autoOpen: false,
                modal: false,
                //Set "OK" button for wijdialog. 
                buttons: {
                    Close: function () {
                        $('#ovsgrid').hideCol('F_ytd');
                        $('#ovsgrid').hideCol('F_mtd');
                        $('#ovsgrid').setGridWidth(800);
                        $(this).wijdialog("close");
                    }
                },
                //Set visibility of caption buttons. 
                captionButtons: {
                    pin: { visible: false },
                    refresh: { visible: false },
                    toggle: { visible: false },
                    minimize: { visible: false },
                    maximize: { visible: false }
                },
                close: function () {

                    $('#ovsgrid').hideCol('F_ytd');
                    $('#ovsgrid').hideCol('F_mtd');
                    $('#ovsgrid').setGridWidth($('#tabs').width() * .35)
                    datahandler.GetDHULine();
                    var grapwidt = $('#tabs').width() - $('#ovsholder').width() - 100;

                    $('#GrapBorder').css("width", grapwidt.toString() + 'px');
                    ;

                },
                open: function () {
                    //$("#ytdslider").wijslider({ orientation: "horizontal", range: true, dragFill: false, min: 0, max: 500, step: 2, values: [100, 400] });
                }
            });
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

            $('.PageFilter').on('change', function (event) {
                LineGraphldcnt = 0;
                BreakOutldcnt = 0;
                FilterSource = 'PageFilter';

                //console.log(event.delegateTarget.id, event.currentTaret.value);
                if (event.delegateTarget.id == "select-AuditType") {
                    FilterCnt++;
                    $AuditType = event.currentTarget.value;
                    ActiveFilter = "AuditType";
                    FilterColumnName = "pf_AuditType";
                    Filterqvalue = $AuditType;

                    var ExistsFlag = false;
                    //ActiveFilterArray = $.each(ActiveFilterArray, function(index, value) { 
                    //    if (value.Name == "pf_AuditType") { 
                    //        value.value = $AuditType;
                    //        value.id = FilterCnt;
                    //        ExistsFlag = true;
                    //    } 
                    //});
                    for (var i = ActiveFilterArray.length - 1; i >= 0; i--) {
                        if (ActiveFilterArray[i].Name === "pf_AuditType" && $AuditType == "ALL") {
                            ActiveFilterArray.splice(i, 1);
                            ExistsFlag = true;
                        } else if (ActiveFilterArray[i].Name === "pf_AuditType") {
                            ActiveFilterArray[i].value = $AuditType
                            ExistsFlag = true;
                        }
                    }
                    if (ExistsFlag == false) {
                        ActiveFilterArray.push({ id: FilterCnt, Name: "pf_AuditType", value: $AuditType });
                    }

                }
                if (event.delegateTarget.id == "select-WorkOrder") {
                    FilterCnt++;
                    $WorkOrder = event.currentTarget.value;
                    ActiveFilter = "WorkOrder";
                    FilterColumnName = "pf_WorkOrder";
                    Filterqvalue = $WorkOrder;
                    var ExistsFlag = false;
                    for (var i = ActiveFilterArray.length - 1; i >= 0; i--) {
                        if (ActiveFilterArray[i].Name === "pf_WorkOrder" && $WorkOrder == "ALL") {
                            ActiveFilterArray.splice(i, 1);
                            ExistsFlag = true;
                        } else if (ActiveFilterArray[i].Name === "pf_WorkOrder") {
                            ActiveFilterArray[i].value = $WorkOrder
                            ExistsFlag = true;
                        }
                    }

                    if (ExistsFlag == false) {
                        ActiveFilterArray.push({ id: FilterCnt, Name: "pf_WorkOrder", value: $WorkOrder });
                    }

                }
                if (event.delegateTarget.id == "select-DataNo") {
                    FilterCnt++;
                    $DataNo = event.currentTarget.value;
                    ActiveFilter = "DataNumber";
                    FilterColumnName = "pf_DataNumber";
                    Filterqvalue = $DataNo;
                    var ExistsFlag = false;
                    for (var i = ActiveFilterArray.length - 1; i >= 0; i--) {
                        if (ActiveFilterArray[i].Name === "pf_DataNumber" && $DataNo == "ALL") {
                            ActiveFilterArray.splice(i, 1);
                            ExistsFlag = true;
                        } else if (ActiveFilterArray[i].Name === "pf_DataNumber") {
                            ActiveFilterArray[i].value = $DataNo
                            ExistsFlag = true;
                        }
                    }

                    if (ExistsFlag == false) {
                        ActiveFilterArray.push({ id: FilterCnt, Name: "pf_DataNumber", value: $DataNo });
                    }

                }

                ActiveFilterString = JSON.stringify(ActiveFilterArray);
                $('#MainContent_FilterListTag').val(ActiveFilterString);

                datahandler.FilterEvent(event.currentTarget.value, event.delegateTarget.id);

            });

            $("#showcolumns-span").click(function () {

                FilterSource = "ColumnFilters";

                datahandler.GetDHULine();
                var grapwidt = $('#tabs').width() - $('#ovsholder').width() - 290;
                $("#ovsgrid").jqGrid('setGridParam',
                    { datatype: 'json' }).trigger('reloadGrid');
                $('#ovsgrid').showCol('F_ytd');
                $('#ovsgrid').showCol('F_mtd');
                $('#GrapBorder').css("width", grapwidt.toString() + 'px');
                // $('#ovsgrid').jqGrid('setColProp','F_ytd',{width:20});
                //$('#ovsgrid .ui-jqgrid-labels > th:eq(3)').css('width','120px')
                $('#ovsholder').find("th:eq(4)").each(function () { $(this).css('width', '120px') })
                $('#ovsgrid').find("td:eq(4)").each(function () { $(this).css('width', '120px') })
                $('#ovsholder').find("th:eq(3)").each(function () { $(this).css('width', '120px') })
                $('#ovsgrid').find("td:eq(3)").each(function () { $(this).css('width', '120px') })
                $('#ovsholder').find("th:eq(2)").each(function () { $(this).css('width', '120px') })
                $('#ovsgrid').find("td:eq(2)").each(function () { $(this).css('width', '120px') })
                $('#ovsholder').find("th:eq(1)").each(function () { $(this).css('width', '120px') })
                $('#ovsgrid').find("td:eq(1)").each(function () { $(this).css('width', '120px') })
                $('#ovsholder').find("th:eq(0)").each(function () { $(this).css('width', '220px') })
                $('#ovsgrid').find("td:eq(0)").each(function () { $(this).css('width', '220px') })
                $('#ovsgrid').setGridWidth(950);

                $('#filter-dialog').wijdialog('open')
            });
            $("#select-Location_fc").on('change', function (event) {
                var selecteval = event.currentTarget.value;

                SelectedCID = selecteval;
                FilterSource = 'ColumnFilters';
                select_Location_fc = selecteval;
                $("#ovsgrid").jqGrid('setGridParam',
                    { datatype: 'json' }).trigger('reloadGrid');

            });
            $('#select-graph').on('change', function (event) {

                var selecteval = event.currentTarget.value;
                switch (selecteval) {
                    case 'LineGraph':
                        $('#Graph2Holder').css("display", "none");
                        $('#Graph1Holder').css("display", "Block");
                        if (LineGraphldcnt == 0) {
                            //$('#loading').toggle();
                            $('#linegraph1').empty();
                            datahandler.GetDHULine();
                        }
                        LineGraphldcnt++;
                        break;
                    case 'BreakOut':
                        $('#Graph1Holder').css("display", "none");
                        $('#Graph2Holder').css("display", "Block");
                        if (BreakOutldcnt == 0) {
                            //$('#loading').toggle();
                            $('#linegraph2').empty();
                            datahandler.GetDefectCountBreakdown();
                        }
                        BreakOutldcnt++;
                        break;
                }
            });

            $('.actionButton').on('click', function (event) {
                $locSelect.val([]).trigger("change");
                $('#MainGrid').jqGrid("clearGridData")
                $('#MainGrid').trigger('reloadGrid');
            });
            $('#backdiv').on('click', function (event) {

                window.location.assign('../../APP/APR_SiteEntry.aspx')
            });
            $('#hideFilters').on('click', function (event) {
                $('#FilterDiv').animate({ height: '-=95px' });
                //$("#tabs").animate({height: '-=90px'});
                $('#hideFilters').toggle();
                $('#showFilters').toggle();

            });
            $("#ExpandedImageCloser").on('click', function (event) {
                $("#ExpandedImageHolder").toggle();
            });
            $('#showFilters').on('click', function (event) {
                $('#FilterDiv').animate({ height: '+=95px' });
                //$("#tabs").animate({height: '+=90px'});
                $('#hideFilters').toggle();
                $('#showFilters').toggle();
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
                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo);
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
                    datahandler.LocationChangeEvent(selList, fromdate, todate, $DataNo);
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
            $("#tabs").wijtabs({
                select: function (e, args) {
                    SelectedTab = args.tab.innerText;
                    switch (SelectedTab) {
                        case 'Overview':
                            TabAccCnt[0].Overview++;
                            //grids.RenderOvsgrid();
                            break;
                        case 'JobSummary':
                            TabAccCnt[0].JobSummary++;

                            if (TabAccCnt[0].JobSummary == 1) {
                                grids.RenderSummaryGrid()

                            } else if (TabAccCnt[0].JobSummary > 1) {
                                $("#ijsgrid").jqGrid('setGridParam',
                                    { datatype: 'json' }).trigger('reloadGrid');
                            }
                            break;
                        case 'SpecSummary':
                            TabAccCnt[0].SpecSummary++;
                            if (TabAccCnt[0].SpecSummary == 1) {
                                grids.RenderSpecDisplayGrid();
                            } else if (TabAccCnt[0].SpecSummary > 1) {
                                $("#Specgrid").jqGrid('setGridParam',
                                    { datatype: 'json' }).trigger('reloadGrid');
                            }
                            break;
                        case 'Photos':
                            TabAccCnt[0].Photos++;
                            $("#ImageHolder").css("width", $("#tabs").width() - 100);
                            // $("#loading2").css({"width": ($("#tabs").width() - 200), "height": ($("#tabs").height() - 80)});
                            $("#loading2").toggle();
                            datahandler.GetDefectImages_2();
                            //if (TabAccCnt[0].Photos ==1) { 
                            //    datahandler.GetDefectImages("999");

                            //} else { 
                            //    datahandler.FilterEvent("485", "Photos");
                            //}
                            break;
                    }

                }
            });
            $("#tabs").wijtabs("select", 0);
            $("#Graphs .owl-item").css("height", ($('#tabs').height() - 125).toString() + "px")
            //datahandler.GetDefectImages("999");
            datahandler.GetDHULine();
            datahandler.GetDataNos();
            datahandler.GetWorkOrders();
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
            RenderSummaryGrid: function () {
                var dataarray = [];
                var SubGridEditFlag = false;
                var rowcnt = $("#tabs").height() / 22.5225;

                var EditPermissions = new Boolean(ADMINFLAG);
                var hiddenVal = false;
                if (EditPermissions == false) { hiddenVal = true; }
                if (ADMINFLAG != 'true') { hiddenVal = true; EditPermissions = false; }
                $("#ijsgrid").jqGrid({
                    datatype: "json",
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/JobSumary_Load.ashx',
                    editurl: "<%=Session("BaseUri")%>" + '/handlers/Presentation/JobSummary_Crud.ashx',
                    colNames: ['ac', 'ijsid', 'JobType', 'JobNumber', 'DataNo', 'PRP_Code', 'UnitDesc', 'Location', 'TemplateId', 'TemplateName', 'LineType', 'TotalInspectedItems', 'ItemPassCount', 'ItemFailCount', 'WOQuantity', 'WorkOrderPieces', 'AQL_Level', 'SampleSize', 'RejectLimiter', 'CID', 'Pass/Fail', 'Started', 'Finished', 'DHU', 'RejectionRate', 'UnitCost', 'Comments'],
                    colModel: [
                        {
                            name: 'delete', index: 'delete', hidden: hiddenVal, width: 90, formatter: 'actions', formatoptions: {
                                onError: function (rowid, jqXHR, textStatus) {
                                    alert('There was some problem, wit row ' + rowid.toString() + ' :' + textStatus);
                                }, actions: { delbutton: EditPermissions, editbutton: EditPermissions, keys: false }
                            }
                        },
                        { name: 'ijsid', index: 'ijsid', hidden: false, editable: true, formatter: grids.formatijsGrid, width: 76, editoptions: { dataInit: function (e) { $(e).prop('readonly', true) } } },
                        { name: 'JobType', index: 'JobType', hidden: true },
                        { name: 'JobNumber', index: 'JobNumber', editable: true, formatter: grids.formatijsGrid },
                        { name: 'DataNo', index: 'DataNo', hidden: true },
                        { name: 'PRP_Code', index: 'PRP_Code', editable: true, hidden: false },
                        { name: 'UnitDesc', index: 'UnitDesc', width: 255, editable: false, formatter: grids.formatijsGrid },
                        { name: 'Location', index: 'Location', width: 90, editable: true, edittype: 'select', editoptions: { value: LocationNamesDrop }, search: false, formatter: grids.formatijsGrid },
                        { name: 'TemplateId', index: 'TemplateId', hidden: true },
                        { name: 'Name', index: 'Name', editable: false, formatter: grids.formatijsGrid },
                        {
                            name: 'LineType', index: 'LineType', editable: true, edittype: 'select', editoptions: { value: 'MFG:MFG; FINAL AUDIT:FINAL AUDIT; IN LINE:IN LINE; ROLL:ROLL; UPGRADES:UPGRADES; PASS/FAIL:PASS/FAIL; CHECK:CHECK; KAUMO:KAUMO; SHIPPING:SHIPPING' },
                            formatter: grids.formatijsGrid
                        },
                        { name: 'TotalInspectedItems', index: 'TotalInspectedItems', editable: true, formatter: 'number', search: false, formatoptions: { actions: { afterSave: function (x) { } }, decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 0, defaultValue: '0' } },
                        { name: 'ItemPassCount', index: 'ItemPassCount', editable: true, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 0, defaultValue: '0' } },
                        { name: 'ItemFailCount', index: 'ItemFailCount', editable: true, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 0, defaultValue: '0' } },
                        { name: 'WOQuantity', index: 'WOQuantity', editable: true, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 0, defaultValue: '0' } },
                        { name: 'WorkOrderPieces', index: 'WOQuantity', editable: true, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 0, defaultValue: '0' } },
                        { name: 'AQL_Level', index: 'AQL_Level', editable: true, edittype: 'select', editoptions: { value: '1:1; 1.5:1.5; 2.5:2.5; 4:4; 100:100' }, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 1, defaultValue: '0' } },
                        { name: 'SampleSize', index: 'WOQuantity', editable: true, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 0, defaultValue: '0' } },
                        { name: 'RejectLimiter', index: 'WOQuantity', editable: true, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 0, defaultValue: '0' } },
                        { name: 'CID', index: 'CID', hidden: true, width: 85, formatter: grids.formatijsGrid },
                        { name: 'Technical_PassFail', index: 'Technical_PassFail', editable: true, edittype: 'select', editoptions: { value: 'PASS:PASS; FAIL:FAIL; : ' }, formatter: grids.formatijsGrid },
                        { name: 'STARTED', index: 'STARTED', editable: false, search: false, formatter: grids.formatijsGrid, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A' } },
                        { name: 'FINISHED', index: 'FINISHED', editable: false, search: false, formatter: grids.formatijsGrid, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A' } },
                        { name: 'DHU', index: 'DHU', editable: false, formatter: 'number', search: false, formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'RejectionRate', index: 'RejectionRate', editable: false, search: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'UnitCost', index: 'UnitCost', editable: true, search: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'Comments', index: 'Comments', hidden: false, width: 120, formatter: grids.formatijsGrid, editable: true }
                    ],
                    jsonReader: { id: "0", root: "rows", total: "total", page: "page", records: "records", subgrid: { root: "rows", repeatitems: "repeatitems", cell: "cell", id: "id" }, userdata: "userdata" },
                    autowidth: false,
                    pager: '#ijsgridpager1',
                    caption: "JobSummary Manager",
                    shrinkToFit: false,
                    width: Number($('#tabs').width() - 55),
                    height: $('#tabs').height() - 215,
                    multiselect: false,
                    viewrecords: true,
                    gridview: true,
                    scroll: false,
                    loadonce: false,
                    paging: false,
                    rowNum: Math.round(rowcnt),
                    postData: {
                        CID_Posted: function () {
                            return SelectedCID;
                        },
                        fromdate: function () {
                            return $("#TxtDateFrom").val();
                        },
                        todate: function () {
                            return $("#TxtDateTo").val();
                        },
                        rowNum: function () {
                            return Math.round(rowcnt);
                        },
                        FlagCnt: function () {
                            return ijsnextcnt;
                        },
                        SessionId: function () {
                            return SessionId;
                        },
                        NextFlag: function () {
                            return NextFlag;
                        },
                        FilterFlag: function () {
                            return ijsFilterFlag;
                        },
                        FilterColumnName: function () {
                            return FilterColumnName;
                        },
                        Filterqvalue: function () {
                            return Filterqvalue;
                        },
                        SelectFilterValues: function () {
                            return JSON.stringify(selectFiltervalues);
                        },
                        LocationArrayString: function () {
                            return LocationsStringArray;
                        },
                        FilterListstring: function () {
                            return ActiveFilterString;
                        }

                    },
                    gridComplete: function () {


                        var listdata = $("#ijsgrid").jqGrid('getGridParam', 'userData');

                        if (listdata) {
                            var parsedata = $.parseJSON(listdata);

                            grids.setSearchSelect2("ijsid", "ijsgrid", parsedata.Col1);
                            grids.setSearchSelect2("JobNumber", "ijsgrid", parsedata.Col2);
                            grids.setSearchSelect2("UnitDesc", "ijsgrid", parsedata.Col3);
                            grids.setSearchSelect2("Name", "ijsgrid", parsedata.Col5);
                            grids.setSearchSelect2("Technical_PassFail", "ijsgrid", parsedata.Col7);
                            $('.ijsDropDown').change(function (e) {

                                FilterColumnName = e.currentTarget.id;
                                Filterqvalue = e.currentTarget.value;
                                var arr = $.map(selectFiltervalues, function (k, v) {

                                    if ('gs_ijsgrid_' + k.col == FilterColumnName) {
                                        return { col: k.col, val: e.currentTarget.value };
                                    } else {
                                        return { col: k.col, val: k.val };
                                    }
                                });
                                selectFiltervalues = arr;

                                FilterCnt++;
                                $AuditType = event.currentTarget.value;
                                ActiveFilter = FilterColumnName;

                                var ExistsFlag = false;

                                for (var i = ActiveFilterArray.length - 1; i >= 0; i--) {
                                    if (ActiveFilterArray[i].Name === FilterColumnName && event.currentTarget.value == "ALL") {
                                        ExistsFlag = true;
                                        ActiveFilterArray.splice(i, 1);


                                    } else if (ActiveFilterArray[i].Name === FilterColumnName) {
                                        ActiveFilterArray[i].value = e.currentTarget.value
                                        ExistsFlag = true;
                                    }
                                }

                                if (ExistsFlag == false) {
                                    ActiveFilterArray.push({ id: FilterCnt, Name: FilterColumnName, value: e.currentTarget.value });
                                }

                                ActiveFilterString = JSON.stringify(ActiveFilterArray);

                                ijsFilterFlag = true;

                                $("#ijsgrid").jqGrid('setGridParam',
                                    { datatype: 'json', page: ijsnextcnt }).trigger('reloadGrid');
                            });
                            $("#gs_ijsgrid_Name").val(parsedata.selectedVal5);
                            $("#gs_ijsgrid_Technical_PassFail").val(parsedata.selectedVal7);
                            $("#gs_ijsgrid_UnitDesc").val(parsedata.selectedVal3);
                            $("#gs_ijsgrid_JobNumber").val(parsedata.selectedVal2);
                            $("#gs_ijsgrid_id").val(parsedata.selectedVal1);

                        }

                        NextFlag = false;
                        if (SelectedTab == "JobSummary") {
                            ijsFilterFlag = false;
                        }
                    },
                    onPaging: function (e) {
                        NextFlag = true;
                        if (e == "next_ijsgridpager1") {
                            ijsnextcnt++;

                            $("#ijsgrid").jqGrid('setGridParam',
                                { datatype: 'json', page: ijsnextcnt }).trigger('reloadGrid');

                        }
                        if (e == "prev_ijsgridpager1" && ijsnextcnt > 0) { ijsnextcnt--; }

                    },
                    onSelectRow: function (id) {

                        var rowdata = $("#ijsgrid").find("td[aria-describedby='ijsgrid_ijsid']");
                        if (rowdata) {

                            subgridquerystr = "ijsid=" + rowdata[id - 1].innerText;
                            grids.Selected_ijs_id = rowdata[id - 1].innerText;

                        }
                    },
                    subGrid: true,
                    subGridRowColapsed: function (pID, id) {
                        if (SubGridEditFlag == true) {
                            $("#ijsgrid").jqGrid('setGridParam',
                                { datatype: 'json' }).trigger('reloadGrid');
                            SubGridEditFlag = false;
                        }
                    },
                    subGridRowExpanded: function (subgrid_id, row_id) {

                        var grid = $("#ijsgrid");
                        prase = new DOMParser();
                        var rowdata = $("#ijsgrid #" + row_id).find("td[aria-describedby='ijsgrid_ijsid']").html();

                        if (rowdata) {
                            if (rowdata.length > 1) {
                                subgridquerystr = "ijsid=" + rowdata
                            }
                        }
                        var subgrid_table_id, pager_id;
                        subgrid_table_id = subgrid_id + "_t";
                        pager_id = "p_" + subgrid_table_id;
                        jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
                        jQuery("#" + subgrid_table_id).jqGrid({
                            url:"<%=Session("BaseUri")%>" + '/handlers/Presentation/DefectMasterSubgrid_Load.ashx?' + subgridquerystr,
                            editurl: "<%=Session("BaseUri")%>" + '/handlers/Presentation/DefectMaster_Crud.ashx',
                            datatype: "json",
                            colNames: ['ac', "DefectID", "DefectTime", "EmployeeNo", "InspectionId", "DefectDesc", "DefectType", "Product", "WorkRoom"],
                            colModel: [
                                {
                                    name: 'delete', index: 'delete', hidden: hiddenVal, width: 90, formatter: 'actions', formatoptions: {
                                        onSuccess: function (jqXHR) {
                                            SubGridEditFlag = true
                                        },
                                        actions: { delbutton: EditPermissions, editbutton: EditPermissions, keys: false }
                                    }
                                },
                                { name: "DefectID", index: "DefectID", width: 85, key: true },
                                { name: "DefectTime", index: "DefectTime", width: 180 },
                                { name: "EmployeeNo", index: "EmployeeNo", editable: true, edittype: 'select', editoptions: { value: EmployeeNames }, width: 220, align: "right" },
                                { name: "InspectionId", index: "InspectionId", width: 85, align: "right" },
                                { name: "DefectDesc", index: "DefectDesc", editable: true, edittype: 'select', editoptions: { value: DefectDescs }, width: 370, align: "right", sortable: false },
                                { name: "DefectClass", index: "DefectClass", editable: true, edittype: 'select', editoptions: { value: DefectTypes }, width: 370, align: "right", sortable: false },
                                { name: "Product", index: "Product", editable: true, width: 230, align: "right", sortable: false },
                                { name: "WorkRoom", index: "WorkRoom", editable: true, width: 230, align: "right", sortable: false },
                            ],
                            height: '100%',
                            rowNum: 20,
                            pager: pager_id,
                            loadonce: true,
                            postData: {
                                SessionId: function () {
                                    return SessionId;
                                }
                            },
                            onSelectRow: function (id) {
                                var rowdata = $("#" + subgrid_table_id).find("td[aria-describedby='" + subgrid_table_id + "_DefectID']");

                                if (rowdata) {
                                    if (rowdata.length > 0 && id > 0) {
                                        subgridquerystr = "dmId=" + id;
                                    }
                                }
                            },
                            gridComplete: function () {
                                var tableht = $("#" + subgrid_table_id).height() + 46;

                                if (tableht > 46) {
                                    $("#" + subgrid_id).css('height', tableht.toString() + "px")
                                }

                            }
                        });
                        jQuery("#" + subgrid_table_id).jqGrid('navGrid', "#" + pager_id, { edit: false, add: false, del: false })
                    },
                    loadComplete: function () {
                        var iCol = grids.getColumnIndexByName($("#ijsgrid"), 'delete');

                        $(this).find(">tbody>tr.jqgrow>td:nth-child(" + (iCol + 1) + ")")
                            .each(function () {
                                $("<div>", {
                                    title: "Custom",
                                    mouseover: function () {
                                        $(this).addClass('ui-state-hover');
                                    },
                                    mouseout: function () {
                                        $(this).removeClass('ui-state-hover');
                                    },
                                    click: function (e) {
                                        var editObject = e.currentTarget.nextSibling.id;

                                        if (editObject == null)
                                            return;

                                        var rowindex = editObject.split("_")[1];

                                        if (rowindex == null || rowindex.length == 0)
                                            return;

                                        var rowObject = $("#ijsgrid").find("tr")[rowindex].children[2].innerHTML;
                                        var CID = $("#ijsgrid").find("tr")[rowindex].children[20].innerHTML;
                                        var optionHtml = [];

                                        optionHtml.push('<option value="' + rowObject + '">' + rowObject + '</option>');
                                        grids.RSSelectedRowCID = "000" + CID.toString();

                                        $("#Merge1").html(optionHtml.join(''));
                                        $('#Merge1').val(rowObject).trigger('change');
                                        $("#dialog-confirm").dialog({
                                            resizable: false,
                                            height: "auto",
                                            width: 400,
                                            height: 440,
                                            modal: true,
                                            buttons: {
                                                "MERGE": function () {
                                                    if ($("#Merge1-JobId-Label").text() == "0" || $("#Merge2-JobId-Label").text() == "0") {
                                                        $("#Merge-result-label").text("*Two Id's must be selected.");
                                                        return;
                                                    }
                                                    var job1 = $("#Merge1").val();
                                                    var job2 = $("#Merge2").val();

                                                    $("#Merge-loading-img").css("display", "inline");
                                                    $.ajax({
                                                        url: '<%=APIBaseUrl%>' + '/api/CombineJobs/' + job1 + '/' + job2,
                                                        method: "GET",
                                                        success: function (data, textStatus, jqXHR) {
                                                            $("#Merge-loading-img").css("display", "none");
                                                            var message = (data.ErrorMessage != null) ? data.ErrorMessage : "";
                                                            if (!!data && !!data.Result) {
                                                                if (data.Result) {
                                                                    $("#Merge-result-label").text("*MERGE SUCCESS!! " + message);
                                                                    $("#ijsgrid").jqGrid('setGridParam',
                                                                        { datatype: 'json' }).trigger('reloadGrid');
                                                                    return;
                                                                }
                                                            }
                                                            $("#Merge-result-label").text("*MERGE FAILED. " + message);
                                                        },
                                                        error: function (jqXHR, textStatus, errorThrown) {
                                                            console.log(errorThrown);
                                                        }
                                                    })
                                                },
                                                Cancel: function () {
                                                    $(this).dialog("close");
                                                }
                                            },
                                            open: function (event, ui) {

                                                if ($.ui && $.ui.dialog && !$.ui.dialog.prototype._allowInteractionRemapped && $(this).closest(".ui-dialog").length) {
                                                    if ($.ui.dialog.prototype._allowInteraction) {
                                                        $.ui.dialog.prototype._allowInteraction = function (e) {
                                                            if ($(e.target).closest('.select2-drop').length) return true;
                                                            return ui_dialog_interaction.apply(this, arguments);
                                                        };
                                                        $.ui.dialog.prototype._allowInteractionRemapped = true;
                                                    }
                                                    else {
                                                        $.error("You must upgrade jQuery UI or else.");
                                                    }
                                                }
                                            },
                                            close: function (event, ui) {
                                                $("#Merge-result-label").text('');
                                            }
                                        });
                                    }
                                }
                                ).css({ "margin-right": "5px", float: "left", cursor: "pointer" })
                                    .addClass("ui-pg-div ui-inline-custom")
                                    .append('<span class="ui-icon ui-icon-merge"></span>')
                                    .prependTo($(this).children("div"));
                            });
                        $("#Merge1").select2({
                            placeholder: "Select a Table Id",
                            data: null,
                            ajax: {
                                url: '<%=APIBaseUrl%>' + '/api/SuggestJob',
                                type: 'GET',
                                delay: 250,
                                dataType: 'json',
                                data: function (params) {
                                    return {
                                        q: (params.term != null ? params.term : ""),
                                        cid: grids.RSSelectedRowCID
                                    };
                                },
                                processResults: function (data, params) {
                                    return {
                                        results: data
                                    };
                                }
                            }
                        });

                        $("#Merge2").select2({
                            placeholder: "Select a Table Id",
                            data: null,
                            ajax: {
                                url: '<%=APIBaseUrl%>' + '/api/SuggestJob',
                                type: 'GET',
                                delay: 250,
                                dataType: 'json',
                                data: function (params) {

                                    return {
                                        q: (params.term != null ? params.term : ""),
                                        cid: grids.RSSelectedRowCID
                                    };
                                },
                                processResults: function (data, params) {

                                    return {
                                        results: data
                                    };
                                }
                            }
                        });
                    }
                });
                jQuery("#ijsgrid").jqGrid('navGrid', '#ijsgridpager1', { edit: false, add: false, del: false });
                jQuery("#ijsgrid").jqGrid('inlineNav', '#ijsgrid', {
                    editParams: {
                        successfunc: function (response) {

                        }
                    }
                });
                if (EditPermissions == true) {
                    jQuery("#ijsgrid").jqGrid('inlineNav', '#ijsgridpager1', {
                        edit: EditPermissions,
                        edittext: 'edit',
                        add: false,
                        save: EditPermissions,
                        savetext: 'save',
                        delete: EditPermissions,
                        deletetext: 'delete',
                        editParams: {
                            keys: true,
                            extraparam: {
                                IjsId: function () {
                                    return grids.Selected_ijs_id;
                                },
                                SessionId: function () {
                                    return SessionId;
                                },
                                UserActivityLogId: function () {
                                    return PageUserActivityLogId;
                                }
                            },
                            successfunc: function (response) {
                                console.log('success save!')
                            }
                        }


                    });
                }
                $('#ijsgrid').jqGrid('navGrid', '#gridpager',
                    {
                        edit: false,
                        add: false,
                        del: false,
                        search: false
                    });
                jQuery("#ijsgrid").jqGrid('filterToolbar', {
                    searchOperators: true,
                    afterSearch: function (v) {

                    }
                }).trigger('reloadGrid');
            },
            Selected_ijs_id: 0,
            RenderSpecDisplayGrid: function () {
                var rowcnt = $("#tabs").height() / 22.5225;

                $("#Specgrid").jqGrid({
                    datatype: "json",
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SpecSumaryDisplay_Load.ashx',
                    colNames: ['id', 'JobNumber', 'UnitDesc', 'LineType', 'DataNo', 'ProductType', 'Location', 'InspectionJobSummaryId', 'Started', 'Finished', 'totcount', 'SpecsMet', 'SpecsFailed'],
                    colModel: [
                        { name: 'id', index: 'id', hidden: false, editable: false, formatter: grids.formatijsGrid, width: 56 },
                        { name: 'JobNumber', index: 'JobNumber', editable: false, formatter: grids.formatijsGrid },
                        { name: 'UnitDesc', index: 'UnitDesc', width: 300, editable: false, formatter: grids.formatijsGrid },
                        { name: 'LineType', index: 'LineType', editable: false, search: false, formatter: grids.formatijsGrid },
                        { name: 'DataNo', index: 'DataNo', editable: false, formatter: grids.formatijsGrid },
                        { name: 'ProductType', index: 'ProductType', width: 255, search: false, hidden: true, editable: false, formatter: grids.formatijsGrid },
                        { name: 'Location', index: 'Location', width: 120, editable: false, search: false, formatter: grids.formatijsGrid },
                        { name: 'CID', index: 'CID', hidden: true, width: 85, search: false, formatter: grids.formatijsGrid },
                        { name: 'Inspection_Started', index: 'Inspection_Started', search: false, width: 200, editable: false, formatter: grids.formatijsGrid },
                        { name: 'Inspection_Finished', index: 'Inspection_Finished', search: false, width: 200, editable: false, formatter: grids.formatijsGrid },
                        { name: 'totcount', index: 'totcount', editable: false, search: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'SpecsMet', index: 'SpecMet', editable: false, search: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } },
                        { name: 'SpecsFailed', index: 'SpecsFailed', editable: false, search: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' } }
                    ],
                    jsonReader: { id: "0", root: "rows", total: "total", page: "page", records: "records", subgrid: { root: "rows", repeatitems: "repeatitems", cell: "cell", id: "id" } },
                    autowidth: false,
                    pager: '#Specgridpager1',
                    caption: "SpecSummary Manager",
                    shrinkToFit: true,
                    width: Number($('#tabs').width() - 55),
                    height: $('#tabs').height() - 215,
                    multiselect: false,
                    viewrecords: true,
                    gridview: true,
                    scroll: false,
                    loadonce: false,
                    paging: false,
                    rowNum: Math.round(rowcnt),
                    datainit: function () {

                    },
                    postData: {
                        CID_Posted: function () {
                            return SelectedCID;
                        },
                        fromdate: function () {
                            return $("#TxtDateFrom").val();
                        },
                        todate: function () {
                            return $("#TxtDateTo").val();
                        },
                        rowNum: function () {
                            return Math.round(rowcnt);
                        },
                        FlagCnt: function () {
                            return ijsnextcnt;
                        },
                        SessionId: function () {
                            return SessionId;
                        },
                        NextFlag: function () {
                            return NextFlag;
                        },
                        FilterFlag: function () {
                            return SgFilterFlag;
                        },
                        FilterColumnName: function () {
                            return FilterColumnName;
                        },
                        Filterqvalue: function () {
                            return Filterqvalue;
                        },
                        SelectFilterValues: function () {
                            return JSON.stringify(selectSpecFiltervalues);
                        },
                        LocationArrayString: function () {
                            return LocationsStringArray;
                        },
                        FilterListstring: function () {
                            return ActiveFilterString;
                        }
                    },
                    gridComplete: function () {

                        var listdata = $("#Specgrid").jqGrid('getGridParam', 'userData');


                        if (listdata && listdata.length > 0) {
                            var parsedata = $.parseJSON(listdata);

                            grids.setSearchSelect2("JobNumber", "Specgrid", parsedata.Col2);
                            grids.setSearchSelect2("DataNo", "Specgrid", parsedata.Col4);
                            grids.setSearchSelect2("id", "Specgrid", parsedata.Col1);
                            grids.setSearchSelect2("UnitDesc", "Specgrid", parsedata.Col3);

                            $('.SpecDropDown').change(function (e) {

                                FilterColumnName = e.currentTarget.id;

                                var arr = $.map(selectSpecFiltervalues, function (k, v) {

                                    if ('gs_Specgrid_' + k.col == FilterColumnName) {
                                        return { col: k.col, val: e.currentTarget.value };
                                    } else {
                                        return { col: k.col, val: k.val };
                                    }
                                });
                                selectSpecFiltervalues = arr;
                                Filterqvalue = e.currentTarget.value;
                                var ExistsFlag = false;
                                ActiveFilterArray = $.each(ActiveFilterArray, function (index, value) {
                                    if (value.Name == FilterColumnName) {
                                        FilterCnt++;
                                        value.value = e.currentTarget.value;
                                        value.id = FilterCnt;
                                        ExistsFlag = true;
                                    }
                                });
                                if (ExistsFlag == false) {
                                    FilterCnt++;
                                    ActiveFilterArray.push({ id: FilterCnt, Name: FilterColumnName, value: e.currentTarget.value });
                                }
                                if (ActiveFilterArray.length > 0) {
                                    ActiveFilterString = JSON.stringify(ActiveFilterArray);
                                }

                                SgFilterFlag = true;
                                $("#Specgrid").jqGrid('setGridParam',
                                    { datatype: 'json', page: ijsnextcnt }).trigger('reloadGrid');
                            });
                            $("#gs_Specgrid_DataNo").val(parsedata.selectedVal4);
                            $("#gs_Specgrid_JobNumber").val(parsedata.selectedVal2);
                        }

                        NextFlag = false;
                        if (SelectedTab == "SpecSummary") {
                            SgFilterFlag = false;
                        }
                    },
                    onPaging: function (e) {
                        NextFlag = true;
                        if (e == "next_Specgridpager1") {
                            ijsnextcnt++;

                            $("#Specgrid").jqGrid('setGridParam',
                                { datatype: 'json', page: ijsnextcnt }).trigger('reloadGrid');

                        }
                        if (e == "prev_Specgridpager1" && ijsnextcnt > 0) { ijsnextcnt--; }

                    },
                    onSelectRow: function (id) {
                        var grid = $("#Specgrid");

                        var rowdata = $("#Specgrid").find("td[aria-describedby='Specgrid_id']");

                        if (rowdata) {

                            Specsubgridquerystr = "ijsid=" + rowdata[id - 1].innerText;

                        }


                    },
                    subGrid: true,
                    subGridModel: [{
                        name: ["SMid", "Timestamp", "ProductType", "Spec_Description", "value", "Upper_Spec_Value", "Lower_Spec_Value", "MeasureValue", "SpecDelta", "SpecSource"],
                        width: [85, 180, 180, 275, 100, 120, 120, 120, 120, 200]

                    }],
                    subgridtype: function (rowidprm) {
                        jQuery.ajax({
                            url:"<%=Session("BaseUri")%>" + '/handlers/Presentation/SpecsSubgrid_Load.ashx?' + Specsubgridquerystr,
                            data: rowidprm,
                            dataType: "json",
                            complete: function (jsondata, stat) {
                                if (stat == "success") {
                                    var thegrid = jQuery("#Specgrid")[0];
                                    thegrid.subGridJson(eval("(" + jsondata.responseText + ")"), rowidprm.id);
                                }
                            }
                        });
                    },
                    subGridBeforeExpand: function (pID, id) {

                        var grid = $("#Specgrid");
                        prase = new DOMParser();
                        var rowdata = $("#Specgrid").find("td[aria-describedby='Specgrid_id']");

                        if (rowdata) {

                            Specsubgridquerystr = "ijsid=" + rowdata[id - 1].innerText;
                        }

                    }

                });
                jQuery("#Specgrid").jqGrid('navGrid', '#Specgridpager1', { edit: false, add: false, del: false });
                $('#Specgrid').jqGrid('navGrid', '#gridpager',
                    {
                        edit: false,
                        add: false,
                        del: false,
                        search: false
                    }
                );
                jQuery("#Specgrid").jqGrid('filterToolbar', {
                    searchOperators: true,
                    afterSearch: function (v) {




                    }
                }).trigger('reloadGrid');

            },
            RenderSpecGridData: function () {
                var rowcnt = $("#tabs").height() / 22.5225;

                $("#Specgrid").jqGrid({
                    datatype: "json",
                    url:     "<%=Session("BaseUri")%>" + '/handlers/Presentation/SpecSumary_Load.ashx',
                    colNames: ['SpecId', 'id', 'Location', 'InspectionJobSummaryId', 'DefectId', 'JobNumber', 'DataNo', 'ItemNumber', 'ProductType', 'POM_Row', 'Spec_Description', 'Lower_Spec_Value', 'Upper_Spec_Value', 'InspectionId', 'value', 'MeasureValue', 'OffSpec', 'Timestamp', 'Inspection_Started'],
                    colModel: [
                        { name: 'SpecId', index: 'SpecId', hidden: true, editable: false },
                        { name: 'id', index: 'id', hidden: false, editable: false, search: false },
                        { name: 'Location', index: 'Location', hidden: false, editable: false, search: false },
                        { name: 'InspectionJobSummaryId', index: 'InspectionJobSummaryId', sortable: false, width: 165, formatter: grids.formatSpecGrid },
                        { name: 'DefectId', index: 'DefectId', sortable: false, width: 90, formatter: grids.formatSpecGrid },
                        { name: 'JobNumber', index: 'JobNumber', sortable: false, width: 155, formatter: grids.formatSpecGrid },
                        { name: 'DataNo', index: 'DataNo', sortable: false, width: 155, formatter: grids.formatSpecGrid },
                        { name: 'ItemNumber', index: 'ItemNumber', sortable: false, width: 90, formatter: grids.formatSpecGrid },
                        { name: 'ProductType', index: 'ProductType', sortable: false, width: 165, formatter: grids.formatSpecGrid },
                        { name: 'POM_Row', index: 'POM_Row', sortable: false, width: 90, formatter: grids.formatSpecGrid },
                        { name: 'Spec_Description', index: 'Spec_Description', width: 145, formatter: grids.formatSpecGrid },
                        { name: 'Lower_Spec_Value', index: 'Lower_Spec_Value', width: 145, search: false, formatter: grids.formatSpecGrid },
                        { name: 'Upper_Spec_Value', index: 'Upper_Spec_Value', width: 145, search: false, formatter: grids.formatSpecGrid },
                        { name: 'InspectionId', index: 'InspectionId', formatter: grids.formatSpecGrid },
                        { name: 'value', index: 'value', width: 120, formatter: grids.formatSpecGrid },
                        { name: 'MeasureValue', index: 'MeasureValue', width: 120, searchoptins: true, sorttype: 'integer', searchoptions: { sopt: ['eq', 'ne', 'le', 'lt', 'gt', 'ge'] }, formatter: grids.formatSpecGrid },
                        { name: 'SpecDelta', index: 'SpecDelta', width: 120, searchoptins: true, sorttype: 'integer', searchoptions: { sopt: ['eq', 'ne', 'le', 'lt', 'gt', 'ge'] }, formatter: grids.formatSpecGrid },
                        { name: 'Timestamp', index: 'Timestamp', width: 185, sorttype: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A' }, formatter: 'date' },
                        { name: 'Inspection_Started', index: 'Inspection_Started', width: 185, sorttype: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A' }, formatter: 'date' }
                    ],
                    pager: '#gridpager2',
                    caption: "Product Specs & Measurements",
                    autowidth: false,
                    shrinkToFit: true,
                    width: Number($('#tabs').width() - 55),
                    height: $('#tabs').height() - 215,
                    multiselect: false,
                    viewrecords: true,
                    gridview: true,
                    scroll: false,
                    loadonce: false,
                    paging: false,
                    rowNum: Math.round(rowcnt),
                    postData: {
                        CID: function () {
                            return SelectedCID;
                        },
                        fromdate: function () {
                            return $("#TxtDateFrom").val();
                        },
                        todate: function () {
                            return $("#TxtDateTo").val();
                        },
                        rowNum: function () {
                            return Math.round(rowcnt);
                        },
                        FlagCnt: function () {
                            return ijsnextcnt;
                        },
                        SessionId: function () {
                            return SessionId;
                        },
                        NextFlag: function () {
                            return NextFlag;
                        }
                    },
                    height: "390",
                    gridComplete: function () {
                        //gridhandler.setSearchSelect("InspectionJobSummaryId", "Specgrid") 

                        //gridhandler.setSearchSelect("Spec_Description", "Specgrid")
                    }
                });
                jQuery("#Specgrid").jqGrid('filterToolbar', {
                    searchOperators: true,
                    afterSearch: function () {



                    }
                }).trigger('reloadGrid');
            },
            formatijsGrid: function (cellvalue, options, rowobject) {


                if (rowobject.Started == "01/01/ 00:00 AM") {
                    return "<span style='color:red; font-weight:bolder; font-size: 11px;'></span>"
                } else {
                    if (rowobject.Technical_PassFail == 'FAIL') {

                        //return "<span style='color:red; font-weight:bolder; font-size: 13px;'>" + cellvalue + "</span>";
                        return cellvalue;

                    } else {
                        //return "<span style='color: #717073; font-weight:800; font-size: 13px;'>" + cellvalue + "</span>";
                        return cellvalue;
                    }
                }
            },
            formatovsGrid: function (cellvalue, options, rowobject) {

                return "<span style='color: #717073; font-weight:800; font-size: 21px;'>" + cellvalue + "</span>";

            },
            formatSpecGrid: function (cellvalue, options, rowobject) {

                var specdelta = new Number(rowobject.SpecDelta);
                var Specvaluelower = new Number(rowobject.Lower_Spec_Value);
                var Specvalueupper = new Number(rowobject.Upper_Spec_Value);
                if (specdelta < 0) {

                    if (specdelta < Specvaluelower) {
                        return "<span style='color:red; font-weight:bolder'>" + cellvalue + "</span>";
                    } else {
                        return "<span style='color: green; font-weight: normal;'>" + cellvalue + "</span>";
                    }
                } else {

                    if (specdelta > Specvalueupper) {
                        return "<span style='color:red; font-weight:bolder'>" + cellvalue + "</span>";
                    } else {
                        return "<span style='color: green; font-weight: normal;'>" + cellvalue + "</span>";
                    }
                }
            },
            setSearchSelect2: function (columnName, tableid, dataarray) {

                var selectstr = '<select id="gs_' + tableid + '_' + columnName + '" class="ijsDropDown" style = "width:100%; padding:0px; height: 40px"><option value="ALL">ALL</option>'

                if (tableid == 'Specgrid') {
                    selectstr = '<select id="gs_' + tableid + '_' + columnName + '" class="SpecDropDown" style = "width:100%; padding:0px; height: 40px"><option value="ALL">ALL</option>'
                }
                $.each(dataarray, function (k, v) {
                    //if (columnName == "UnitDesc") { console.log(v);}
                    selectstr = selectstr + '<option value="' + v + '">' + v + '</option>'
                });

                selectstr = selectstr + '</select>'

                $('#gs_' + columnName).replaceWith(selectstr);
            },
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
            GetUniqueNames: function (columnName, tableName) {

                var texts = $('#' + tableName).jqGrid('getCol', columnName), uniqueTexts = [],
                    textsLength = texts.length, text, textsMap = {}, i;
                var mydata = $('#' + tableName).jqGrid('getCol', columnName, false);
                var mydata1 = $('#' + tableName).jqGrid("getGridParam", "data");//,
                var DataNoList = [];

                $.each(mydata, function (k, v) {
                    var html = v;

                    var div = document.createElement("div");
                    div.innerHTML = html;
                    var text = div.textContent || div.innerText || "";

                    DataNoList.push(text);

                });
                //$.each(mydata1, function(k,v) { 
                //    $.each(v, function(k2,v2) { 
                //        $("#gs_" + k2).css("height", "40px");
                //        $("#gs_" + k2).css("font-size", "medium");
                //        $("#gs_" + k2).css("font-weight", "700");
                //        if (k2 == columnName) { 
                //            DataNoList.push(v2);
                //        }

                //    });

                //});

                for (i = 0; i < DataNoList.length; i++) {
                    text = DataNoList[i];
                    if (text !== undefined && textsMap[text] === undefined) {
                        // to test whether the texts is unique we place it in the map.
                        textsMap[text] = true;
                        uniqueTexts.push(text);
                    }
                }

                return uniqueTexts;
            },
            buildSearchSelect: function (uniqueNames) {
                //var values=":All";
                var values = ["All"];
                $.each(uniqueNames, function () {
                    //values += ";" + this + ":" + this;
                    values.push(this);
                });
                return values;
            },
            getColumnIndexByName: function (grid, columnName) {
                var cm = grid.jqGrid('getGridParam', 'colModel'), i, l = cm.length;
                for (i = 0; i < l; i++) {
                    if (cm[i].name === columnName) {
                        return i; // return the index
                    }
                }
                return -1;
            }
        };
        var graphs = {
            RenderDHULine: function (data, titledata) {
                var newarray = [,];
                var fieldlength = titledata.length;
                var rowcnt = 0;
                // Create the data table. 
                console.log(titledata);
                if (data == null || data.length == 0) {
                    var zeroRow = [];
                    var today = new Date();
                    var datestr = (today.getMonth() + 1).toString() + "/" + (today.getDay() + 1).toString();
                    zeroRow.push(datestr);

                    for (var i = 0; i < titledata.length; i++) {
                        zeroRow.push(0);
                    }
                    newarray[0] = zeroRow;
                } else {
                    $.each(data, function (index, value) {
                        var innarray = [];
                        var propCnt = 0;
                        $.each(value, function (innerIndex, innerValue) {
                            innarray.push(innerValue);
                            propCnt++;
                            if (propCnt > fieldlength)
                                return false;
                        });
                        newarray[rowcnt] = innarray;
                        rowcnt++;
                    });
                }

                console.log("newarray", newarray);
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'DATEVAL');
                $.each(titledata, function (index, value) {

                    data.addColumn('number', value.Object1);
                });

                data.addRows(newarray);

                var grapwidt = $('#tabs').width() - $('#ovsholder').width() + 150;

                var options = {
                    title: 'DHU BY DAY AND LOCATION (Completed Inspections Only)',
                    subtitle: '(Includes All Types for Completed Inspections)',
                    titleTextStyle: {
                        color: 'black',
                        fontName: 'Arial',
                        fontSize: 26,
                        bold: true
                    },
                    width: $("#GrapBorder").width() - 20,
                    height: $("#GrapBorder").height() - 40,
                    colors: ['#6496c8', '#B0B579', '#FBB040', '#D31245'],
                    backgroundColor: 'transparent',
                    titlePosition: 'out'
                };


                //var chart = new google.charts.Scatter(document.getElementById('scatter_dual_y'));

                $('#linegraph1').empty();
                var chart = new google.visualization.LineChart(document.getElementById('linegraph1'));
                chart.draw(data, options);
                $('#loading').css({ display: 'none' });
            },
            RenderREJLine: function (data, titledata) {
                var newarray = [];
                var fieldlength = titledata.length;  //titledata.length; 

                // Create the data table.
                $.each(data, function (index, value) {
                    switch (fieldlength) {
                        case 1:
                            newarray.push([value.DATEVAL, value.LOC_1])
                            break;
                        case 2:
                            newarray.push([value.DATEVAL, value.LOC_1, value.LOC_2])
                            break;
                        case 3:
                            newarray.push([value.DATEVAL, value.LOC_1, value.LOC_2, value.LOC_3])
                            break;
                        case 4:
                            newarray.push([value.DATEVAL, value.LOC_1, value.LOC_2, value.LOC_3, value.LOC_4])
                            break;
                    }
                });

                var data = new google.visualization.DataTable();
                data.addColumn('string', 'DATEVAL');
                //data.addColumn('number', "LOC_1");
                //data.addColumn('number', "LOC_2");
                //data.addColumn('number', "LOC_3");
                //data.addColumn('number', "LOC_4");

                $.each(titledata, function (index, value) {

                    data.addColumn('number', value.Object1);
                });

                data.addRows(newarray);

                var options = {
                    title: 'REJ% BY DAY AND LOCATION',
                    subtitle: '(Includes All Types)',
                    titleTextStyle: {
                        color: 'black',
                        fontName: 'Arial',
                        fontSize: 13,
                        bold: true
                    },
                    titlePosition: 'in',
                    width: $('#tabs').width() * .33,
                    height: $('#tabs').height() - 10,
                    colors: ['#6496c8', '#B0B579', '#FBB040', '#D31245'],
                    backgroundColor: 'transparent',
                    titlePosition: 'out'
                };


                //var chart = new google.charts.Scatter(document.getElementById('scatter_dual_y'));
                var chart = new google.visualization.LineChart(document.getElementById('linegraph2'));
                chart.draw(data, options);


            },
            GetStackedBreakdown: function (jsondata, titleArr) {
                var newarray = [];
                var data = new google.visualization.DataTable();
                // Create the data table.
                $.each(jsondata, function (index, value) {
                    var RowObject = new Object();
                    var Rowarray = [];
                    $.each(value, function (index1, value1) {

                        var newobj = new Object();
                        newobj = value1;

                        if (newobj.toString().length > 0) {
                            if (index1 != "RowID") {

                                RowObject[index1] = value1
                                Rowarray.push(value1);
                            }
                        }
                    });
                    //newarray.push(RowObject)
                    newarray.push(Rowarray)
                });


                data.addColumn('string', 'DefectDesc');
                $.each(titleArr, function (index, value) {
                    data.addColumn('number', value.Object3);
                });

                data.addRows(newarray);

                var options = {
                    title: 'DEFECTCOUNT BREAKDOWN',
                    titleTextStyle: {
                        color: 'black',
                        fontName: 'Arial',
                        fontSize: 26,
                        bold: true
                    },
                    width: $("#GrapBorder").width() - 20,
                    height: $("#GrapBorder").height() - 40,
                    bar: { groupWidth: '75%' },
                    isStacked: true,
                    colors: ['#6496c8', '#FBB040', '#E0E033', '#B0B579', 'green', '#385B38', '#333F6D', '#D42A38', '#CA8B90', '#FB8B94'],
                    titlePosition: 'out',
                    legend: { position: 'top' },
                };

                // Instantiate and draw our chart, passing in some options.
                $('#linegraph2').empty();
                var chart = new google.visualization.BarChart(document.getElementById('linegraph2'));
                chart.draw(data, options);
                //$('#loading').toggle();
            }
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
            GetDefectImages: function (CID) {

                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDashBoardImageArray', args: { Locationid: CID, todate: $Todateval, fromdate: $Fromdateval, ActiveLocationStr: LocationsStringArray, ActiveFilterStr: ActiveFilterString } },
                    success: function (data) {
                        if (data) {
                            var newPicArray = [];
                            DefectPictureArray = $.parseJSON(data);

                            if (ActiveFilterArray.length == 0) {
                                $("#customers").empty();
                                newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                    return index2;
                                });
                                datahandler.InjectPictureArray(newPicArray);
                            } else {
                                $.each(ActiveFilterArray.sort(), function (index, value) {

                                    switch (value.Name) {
                                        case 'pf_DataNumber':
                                            newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                                if ($DataNo == 'ALL' || $DataNo == index2.DataNo) {
                                                    return index2;
                                                }
                                            });
                                            datahandler.InjectPictureArray(newPicArray);
                                            break;
                                        case 'pf_AuditType':

                                            newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                                if ($AuditType == 'ALL' || $AuditType == index2.AuditType) {
                                                    return index2;
                                                }
                                            });
                                            datahandler.InjectPictureArray(newPicArray);
                                            break;
                                        default:

                                    }
                                });
                            }
                            //if (DefectPictureArray && DefectPictureArray.length > 0 ) { 
                            //    var numi = document.getElementById('defectCarousel');
                            //    var html1 = [];
                            //    $.each(DefectPictureArray, function (index, value) { 
                            //        html1.push('<div class="item"><img src="' + value.linkUrl + '" style="width: 80%; height: 250px; display: block;" alt="Defect Image" ><h4 style="font-size: 20px; height: 49px;">' + value.caption + '</h4></div>');
                            //    });
                            //    $("#defectCarousel").html(html1.join(''));
                            //}

                            //var html = [];
                            //var json = [{ text: "1", id: 1 }, { text: "1", id: 1 }];
                            ////first get a list of items from db
                            //$.each(DefectPictureArray, function (k, value) {
                            //    html.push('<li class="customerItem animated">');
                            //    html.push('<a onclick="click()" title="" href="#">');
                            //    html.push('<h2><span class="custNumber"><img src="' + value.linkUrl + '" style="width: 80%; height: 250px; display: block;" alt="Defect Image" ><h4 style="font-size: 20px; height: 49px;">' + value.caption + '</h4></h2>');
                            //    //html.push('<h2><span class="custNumber">' + value.caption + '</span></h2>');
                            //    html.push('</a></li>');
                            //});

                            ////add items to html
                            //$("#customers").html(html.join(''));

                            ////animate the items to view
                            //carousel.animateCustomersOn('customers');
                            //$("#defectCarousel").owlCarousel({
                            //    slideSpeed : 950,
                            //    autoplay: false,
                            //    loop:true,
                            //    items : 5,
                            //    lazyLoad : true,
                            //    autoHeight: false,
                            //    itemsDesktop : [1199,3],
                            //    itemsDesktopSmall : [979,3]

                            //}).trigger('owl.play',6000);;


                        }
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });


            },
            GetDefectImages_2: function () {

                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetInspectionImageBase64', args: { fromdate: $Fromdateval, todate: $Todateval, ActiveLocationStr: LocationsStringArray, ActiveFilterStr: ActiveFilterString } },
                    success: function (data) {
                        if (data) {
                            var newPicArray = [];
                            DefectPictureArray = $.parseJSON(data);
                            newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                return index2;
                            });
                            $("#loading2").toggle();
                            datahandler.InjectPictureArray(newPicArray);

                        }
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });


            },
            GetDefectImageDescList: function () {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDefectImageDescList', args: { fromdate: $Fromdateval, todate: $Todateval } },
                    success: function (data) {

                        var DefectDescList = $.parseJSON(data);
                        var DefectDescListF = [];
                        if (DefectDescList != null) {
                            if (DefectDescList.length > 0) {
                                var fchtml = [];

                                fchtml.push('<option value = "ALL">ALL</option>');
                                $.each(DefectDescList, function (index, value) {
                                    if (value != null) {
                                        fchtml.push('<option value = "' + value.DefectDesc_.trim() + '">' + value.DefectDesc_.trim() + '</option>');
                                    }

                                });

                                $("#select-DefectType-Photos").html(fchtml.join(''));
                                $("#select-DefectType-Photos").val('ALL');

                                $('#select-DefectType-Photos').on('change', function (event) {

                                    var ExistsFlag = false;
                                    for (var i = ActiveFilterArray.length - 1; i >= 0; i--) {
                                        if (ActiveFilterArray[i].Name === "tf_DefectType-Photos" && $AuditType == "ALL") {
                                            ActiveFilterArray.splice(i, 1);
                                            ExistsFlag = true;
                                        } else if (ActiveFilterArray[i].Name === "tf_DefectType-Photos") {
                                            ActiveFilterArray[i].value = event.currentTarget.value
                                            ExistsFlag = true;
                                        }
                                    }
                                    if (ExistsFlag == false) {
                                        ActiveFilterArray.push({ id: FilterCnt, Name: "tf_DefectType-Photos", value: event.currentTarget.value });
                                    }
                                    if (event.currentTarget.value == 'ALL') {
                                        DefectPictureArrayF = [];
                                    }
                                    $("#customers").empty();

                                    ActiveFilterString = JSON.stringify(ActiveFilterArray);
                                    datahandler.SelectFilterPictureArray("tf_DefectType-Photos")
                                });

                                $('#select-prp').on('change', function (evt) {

                                    var evtobj = evt.currentTarget.selectedOptions;
                                    var DefectDescList_new = [];

                                    if (DefectDescListF.length > 0) {
                                        DefectDescList_new = jQuery.extend(true, [], DefectDescListF);
                                        DefectDescListF.length = 0;
                                    } else {
                                        DefectDescList_new = jQuery.extend(true, [], DefectDescList);
                                    }
                                    if (evtobj.length > 0) {

                                        for (var i = DefectDescList_new.length - 1; i >= 0; i--) {
                                            if (DefectDescList_new[i].Prp_Code != null) {
                                                for (var j = evtobj.length - 1; j >= 0; j--) {
                                                    if (DefectDescList_new[i].Prp_Code.trim() == evtobj[j].value && evtobj[j].value.length > 0) {
                                                        DefectDescListF.push(DefectDescList_new[i])
                                                    }
                                                }

                                            }

                                        }

                                    } else {
                                        DefectDescListF = jQuery.extend(true, [], DefectDescList_new);
                                    }
                                    var ddfhtml = [];
                                    var curval = $("#select-DefectType-Photos").val();
                                    ddfhtml.push('<option value = "ALL">ALL</option>');
                                    $.each(DefectDescListF, function (index, value) {
                                        if (value != null) {
                                            ddfhtml.push('<option value = "' + value.DefectDesc_.trim() + '">' + value.DefectDesc_.trim() + '</option>');
                                        }

                                    });

                                    $("#select-DefectType-Photos").empty();
                                    $("#select-DefectType-Photos").html(ddfhtml.join(''));
                                    $("#select-DefectType-Photos").val(curval);
                                });
                                $('.PageFilter').on('change', function (event) {
                                    var FilterId = event.delegateTarget.id;
                                    var FilterVal = event.delegateTarget.value;
                                    var curval = $("#select-DefectType-Photos").val();
                                    if (FilterId == 'select-AuditType' && FilterVal != null) {
                                        DefectDescListF = jQuery.extend(true, [], DefectDescList);
                                        for (var i = DefectDescListF.length - 1; i >= 0; i--) {
                                            if (DefectDescListF[i].AuditType != null) {
                                                if (DefectDescListF[i].AuditType.trim() != FilterVal.trim() && FilterVal != "ALL") {
                                                    DefectDescListF.splice(i, 1);
                                                }
                                            }

                                        }

                                    }
                                    if (FilterId == 'select-DataNo' && FilterVal != null) {
                                        DefectDescListF = jQuery.extend(true, [], DefectDescList);
                                        for (var i = DefectDescListF.length - 1; i >= 0; i--) {
                                            if (DefectDescListF[i].DataNo_ != null) {
                                                if (DefectDescListF[i].DataNo_.trim() != FilterVal.trim() && FilterVal != "ALL") {
                                                    DefectDescListF.splice(i, 1);
                                                }
                                            }

                                        }

                                    }
                                    if (FilterId == 'select-WorkOrder' && FilterVal != null) {
                                        DefectDescListF = jQuery.extend(true, [], DefectDescList);
                                        for (var i = DefectDescListF.length - 1; i >= 0; i--) {
                                            if (DefectDescListF[i].WorkOrder_ != null) {
                                                if (DefectDescListF[i].WorkOrder_.trim() != FilterVal.trim() && FilterVal != "ALL") {
                                                    DefectDescListF.splice(i, 1);
                                                }
                                            }

                                        }

                                    }
                                    var ddfhtml = [];
                                    ddfhtml.push('<option value = "ALL">ALL</option>');
                                    $.each(DefectDescListF, function (index, value) {
                                        if (value != null) {
                                            ddfhtml.push('<option value = "' + value.DefectDesc_.trim() + '">' + value.DefectDesc_.trim() + '</option>');
                                        }

                                    });

                                    $("#select-DefectType-Photos").empty();
                                    $("#select-DefectType-Photos").html(ddfhtml.join(''));
                                    $("#select-DefectType-Photos").val(curval);
                                })
                            }
                        }
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });

            },
            SetImageTest: function () {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetInspectionImageBase64' },
                    success: function (data) {

                        $("#SampleBase64").attr("src", 'data:image/png;base64,' + data)
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });

            },
            FilterEvent: function (value, field) {
                //console.log(field + ' : ' + value); 
                datahandler.GetDataNos();
                datahandler.GetWorkOrders();
                switch (SelectedTab) {
                    case 'Overview':
                        OwFilterFlag = true;
                        var selectgr = $('#select-graph').val();
                        //$('#loading').toggle();
                        if (selectgr == 'LineGraph') {
                            $('#linegraph1').empty();
                            datahandler.GetDHULine();
                            $("#ovsgrid").jqGrid('setGridParam',
                                { datatype: 'json' }).trigger('reloadGrid');
                        } else {
                            $('#linegraph2').empty();
                            datahandler.GetDefectCountBreakdown();
                        }

                        break;
                    case 'JobSummary':
                        ijsFilterFlag = true;
                        $("#ijsgrid").jqGrid('setGridParam',
                            { datatype: 'json', page: ijsnextcnt }).trigger('reloadGrid');
                        break;
                    case 'SpecSummary':
                        SgFilterFlag = true;
                        $("#Specgrid").jqGrid('setGridParam',
                            { datatype: 'json', page: ijsnextcnt }).trigger('reloadGrid');
                        break;
                    case 'Photos':
                        //console.log(DefectPictureArray);
                        PtFilterFlag = true;
                        var owl = $("#defectCarousel");
                        var pos = 0;
                        //console.log($AuditType);
                        var FilteredArray = [];
                        $("#customers").empty();

                        datahandler.SelectFilterPictureArray(field)
                        break;

                };
                //$("#defectCarousel").empty();

            },
            LocationChangeEvent: function (cidArray, fromdate, todate, DataNo) {
                console.log('cidArray:' + cidArray);
                //alert('Location Changed!');
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDataArray', args: { array: cidArray, from: fromdate, toDate: todate, DN: DataNo} },
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
                        document.getElementById("loading").style.display = "none";
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            },
            SelectFilterPictureArray: function (field) {
                var NewDefectPictureArray = DefectDescListF = jQuery.extend(true, [], DefectPictureArray);
                var newPicArray = [];
                if (field == "Date") {
                    $("#customers").empty();
                    datahandler.GetDefectImages('999')
                } else {
                    if (ActiveFilterArray.length > 0) {
                        $.each(ActiveFilterArray.sort(), function (index, value) {

                            switch (value.Name) {
                                case 'pf_DataNumber':
                                    newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                        if ($DataNo == 'ALL' || $DataNo == index2.DataNo) {
                                            return index2;
                                        }
                                    });
                                    datahandler.InjectPictureArray(newPicArray);
                                    break;
                                case 'pf_AuditType':
                                    newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                        if ($AuditType == 'ALL' || $AuditType == index2.AuditType) {
                                            return index2;
                                        }
                                    });
                                    datahandler.InjectPictureArray(newPicArray);
                                    break;
                                case 'pf_prp':
                                    newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                        if (value.value == index2.prpcode) {
                                            return index2;
                                        }
                                    });
                                    datahandler.InjectPictureArray(newPicArray);
                                    break;
                                case 'tf_DefectType-Photos':

                                    newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                                        if (value.value == index2.DefectDesc || $("#select-DefectType-Photos").val() == "ALL") {

                                            return index2;
                                        }
                                    });

                                    datahandler.InjectPictureArray(newPicArray);
                                    break;
                                default:

                            }

                        });
                        DefectPictureArrayF = $.extend(true, [], newPicArray);
                    } else {
                        newPicArray = $.map(DefectPictureArray, function (index2, value2) {
                            return index2;
                        });
                        DefectDescListF.length = 0;
                        datahandler.InjectPictureArray(newPicArray);
                    }

                }


            },
            InjectPictureArray: function (picarray) {
                if (picarray && picarray.length > 0) {
                    $("#customers").empty();
                    var html = [];
                    var json = [{ text: "1", id: 1 }, { text: "1", id: 1 }];
                    var aid = 1;
                    //first get a list of items from db

                    $.each(picarray, function (k, value) {
                        html.push('<li class="customerItem animated">');
                        html.push('<a  onclick="return false;" ondblclick="carousel.EnlargeMe(' + aid.toString() + ');return false;" title="" href="#"><img id="ExpandButton" src="../../Images/148-Expand-button-symbol-of-four-arrows.png" onclick="carousel.EnlargeMe(' + aid.toString() + ');return false;" style="position:relative; width: 33px; float:right;" />');
                        html.push('<h2><span class="custNumber"><img id="Image_' + aid.toString() + '" src="' + value.linkUrl + '" style="width: 80%; height: 250px; display: block;" alt="Defect Image" ><h4 style="font-size: 19px; height: 49px;">' + value.caption + '</h4></h2>');
                        html.push('<h5 style="position:relative; color:white; font-size:15px; top: -45px;">Audit Type: ' + value.AuditType.toString() + ' </h5>')
                        html.push('<h5 style="position:relative; color:white; font-size:15px; top: -60px;">DataNo: ' + value.DataNo.toString() + ' </h5>')
                        //html.push('<h2><span class="custNumber">' + value.caption + '</span></h2>');
                        html.push('</a></li>');
                        aid++;
                    });

                    //add items to html
                    $("#customers").html(html.join(''));

                    //animate the items to view
                    carousel.animateCustomersOn();
                } else {
                    var html = [];
                    $("#customers").empty();
                    html.push('<li class="customerItem animated">');
                    html.push('<a onclick="click()return false;" title="" href="#">');
                    html.push('<h2><span class="custNumber">NO PICTURES</span></h2>');
                    html.push('</a></li>');
                    $("#customers").html(html.join(''));
                    carousel.animateCustomersOn();
                }
            },
            FilterJsonTable: function (s, l, key) {
                var returnarray = [];
                $.each(l, function (index, value) {
                    $.each(value, function (index1, value1) {
                        if (index1 == key) {
                            if (value.Abreviation == s) {
                                returnarray.push(value);
                            }
                        }
                    });
                });
                return returnarray;
            },
            GetDHULine: function () {
                console.log("GETDHULINE", LocationsStringArray)
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDHULineChart', args: { todate: $Todateval, fromdate: $Fromdateval, DataNo: $DataNo, AuditType: $AuditType, LocArray: LocationsStringArray } },
                    success: function (data) {
                        if (data) {
                            var dataarr = data.split('%%%');
                            if (dataarr.length == 2) {

                                var parseArr = $.parseJSON(dataarr[0]);
                                var titleArr = $.parseJSON(dataarr[1]);
                                graphs.RenderDHULine(parseArr, titleArr);
                            }
                        }
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });

            },
            GetREJLine: function () {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetREJLineChart', args: { todate: $Todateval, fromdate: $Fromdateval } },
                    success: function (data) {
                        if (data) {


                            var dataarr = data.split('%%%');
                            if (dataarr.length == 2) {

                                var parseArr = $.parseJSON(dataarr[0]);
                                var titleArr = $.parseJSON(dataarr[1]);
                                graphs.RenderREJLine(parseArr, titleArr);
                            }
                        }
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });

            },
            GetDefectCountBreakdown: function () {

                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetStackedDefectLineType', args: { todate: $Todateval, fromdate: $Fromdateval, DataNo: $DataNo, AuditType: $AuditType, LocArray: LocationsStringArray } },
                    success: function (data) {

                        var dataarr = data.split('%%%');
                        if (dataarr.length == 2) {

                            var parseArr = $.parseJSON(dataarr[0]);
                            var titleArr = $.parseJSON(dataarr[1]);
                            graphs.GetStackedBreakdown(parseArr, titleArr);
                        }

                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });


            },
            GetWorkOrders: function () {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetWorkOrders', args: { fromdate: $Fromdateval, todate: $Todateval, LocArray: LocationsStringArray, AuditType: $AuditType } },
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
            GetDataNos: function () {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDataNos', args: { fromdate: $Fromdateval, todate: $Todateval, LocArray: LocationsStringArray, AuditType: $AuditType } },
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
            Get_Prpselect: function () {

                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'GET',
                    data: { method: 'Get_prp_select2', args: { fromdate: $Fromdateval, todate: $Todateval } },
                    success: function (data) {
                        if (data) {
                            var parseArr = $.parseJSON(data);
                            $("#select-prp").select2({
                                data: parseArr
                            });

                            $('#select-prp').on('change', function (evt) {

                                var evtobj = evt.currentTarget.selectedOptions;

                                FilterCnt++;
                                if (evtobj) {
                                    var row = 0;
                                    var data = $.grep(ActiveFilterArray, function (e) {
                                        return e.Name != "pf_prp";
                                    });
                                    ActiveFilterArray = data;

                                    $.each(evtobj, function (index, value) {

                                        var eleval = value;
                                        ActiveFilterArray.push({ id: FilterCnt, Name: "pf_prp", value: eleval.innerHTML });

                                    });

                                    ActiveFilterString = JSON.stringify(ActiveFilterArray);
                                    datahandler.FilterEvent("val", "prp");

                                }

                            });
                        }
                    },
                    error: function (a, b, c) {
                        alert(c);

                    }
                });


            }

        };
        function callajax() {
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                type: 'GET',
                data: { method: 'GetDHU_Scat' },
                success: function (data) {
                    if (data) {
                        var parseArr = $.parseJSON(data);
                        $.each(parseArr, function (index, value) {

                        });
                    }
                },
                error: function (a, b, c) {
                    alert(c);

                }
            });
        }

    </script>

</asp:Content>

