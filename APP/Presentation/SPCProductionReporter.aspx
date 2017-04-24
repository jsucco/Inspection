<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Site_2.master" AutoEventWireup="false" CodeFile="SPCProductionReporter.aspx.vb" Inherits="core.APP_DataEntry_TemplateManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="loading" style="width:66%; top:150px; height:580px; left:29.7%; position:absolute; background-color:white; z-index:200;">
        <div style="width:97%; top:7px; height:100%; position:absolute;z-index:0; background-color:white; opacity:.4; left: 40px;"></div>
        <%--<input type="image" src="../../Images/load-indicator.gif" style="z-index:200; margin-left: 41%; margin-top:20%; position:absolute;" />--%>
    </div>
    <div style="Z-INDEX: 102; LEFT: 30px; POSITION: relative; TOP: -10px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
        <select id="selectNames"  name="TemplateId" style="width: 162px; height: 35px; position:relative; top:12px; " class="inputelement"></select>
               
            </div>
    <div style="position:relative; left: 30px; top: 25px;">
        <asp:Label ID="LblDateFrom" runat="server" CssClass="labelleft1" >Begin Date</asp:Label>
        <input type="text" id="TxtDateFrom" name="datefrom" style="float:left; width: 110px" class=labelleft1 />
        <input type="hidden" id="DateFrom_Hidden" runat="server" />
        </div>
    <div style="position:relative; left: 30px; top: 60px;">
        <asp:Label ID="LblDateTo" runat="server" CssClass="labelleft1" >End Date  </asp:Label>
        <div style="position:absolute; left:79px; top:-2px;">
            <input type="text" id="TxtDateTo" name="dateto" style="float:left;  width: 110px; left:14px;" class=labelleft1 />
            <input type="hidden" id="DateTo_Hidden" runat="server"/>
        </div>
        
        </div>
    <div id="DateDiv" style="position: relative; width: 12%; height: 250px; top: 80px; left: 30px;" class="statspad">
        <label id="TotalYards" style="top: 10px; left: 22px;" class="statspad_label">Total Yards: </label>
        <label id="TotalYardsval" style="position: relative; float: right; top: -5px;" class="statspad_label">0_</label>
        <label id="RollCount" style="top: 90px; left: 22px; " class="statspad_label">Roll Count: </label>
        <label id="RollCountval" style="top: 90px; left: 70%;" class="statspad_label">0_</label>
        <label id="WorkOrder" style="top: 170px; left: 22px;" class="statspad_label">Work Order Count: </label>
        <label id="WorkOrderval" style="top: 170px; left: 70%" class="statspad_label">0_</label>
        <div id = "Chk1" style="position:absolute; top: 220px; left: 30px; display:none; padding: 0px;" class="statspad_label">
                                        <input id="Filter" name="ChkBx1" style="display:none;" type="checkbox" class=chkbox2 />Filter</div>
        <input id="GetReports" type="button" class="export divflip" style="position:absolute; font-size:small; left: 0px; width: 75px; top: 235px; height: 40px;" value="REPORTS" />
        <asp:Button ID="GoButton" runat="server" Text="Export" style="position:relative; font-size:small; left:70%; width: 75px; top: 220px; height: 40px;" class=export/>
    </div>
    <div id="ReportsDiv" style="position:relative; width:12%; height:250px; top:80px; left:30px; display:none;" class="statspad">
        <div id="ReportHolder1" style=""><input type="hidden" id="reportname1_hidden" runat="server" value="0" /><input type="hidden" id="reportstatus1_hidden" runat="server" value="false" />
         <input type="checkbox" runat="server" style="position:relative; top:10px;transform: scale(1.5);" id="check1">
         <label runat="server" id="ReportLabel1" class="statspad_label" style="left: 40%; top:66px;">            WorkOrder Summary</label></div>

        <asp:Button ID="GetReport1" runat="server" Text="Export" style="position:absolute; font-size:xx-small; top: 219px; left:60%; font-size:small; width: 75px; height: 40px; color:white; background-color:transparent; border-color:white; border-width:3px; right: 10px;"/>
        <input id="GetDate" type="button" class="export divflip" style="position:absolute; font-size:xx-small; left: 20px; width: 75px; top: 220px; height: 40px;" value="GO BACK" />
    </div>
    <asp:Button ID="GODate" runat="server" Text="GO" style="position:relative; font-size:small; left: 30px; width: 75px; top: 90px; height: 40px;" class=export/>
    <div style="position:relative;"  >
        <label id="NoData" style="position: absolute; top: -130px; display: none; left: 62%; font-size: large; font-weight: 700; text-decoration: underline;">NO RECORDS FOUND</label>
    </div>
    <div id="linegraph" style="position:absolute; left:28%; top: 130px; width: 400px;"></div>
    <%--<div id="linchartborder" style="position:absolute; display:none; left:28%; top: 140px; width: 68%;height: 590px;border-style: solid;border-color: rgb(162, 153, 153);">
        <div style="position:relative; width:99%;" div=""></div></div>--%>
    <div id="example" style="position:absolute; left:21.5%; top: 130px; width: 77%; z-index:300;">
        <svg id="visualisation" width="100%" height="600"></svg>
    </div>
    <div id="PieChart"style="position:absolute; left: 36%; top: 415px;">
    </div>
    <div id="EmployeeChart" style="Position:absolute; left: 65%; top: 415px;"></div>

    <label id="Atitle" style="position:absolute; width: 100px; left:45.5%; top:670px; font-family:sans-serif; font-size:medium">Defect Type</label>
        
    <label id="Atitle" style="position:absolute; width: 300px; left:55%; font-family:sans-serif; font-size:large; top: 700px;">Defect Count BreakDown</label>

    <label id="Atitle" style="position:absolute; width:150px; top:670px; left:69%; font-family:sans-serif; font-size:medium">Product Name</label>
    <div id="PieLegend" style="position: absolute; top: 445px; left: 31%; width: 50%;"></div>
    <div id="tabs" style="position:absolute; Z-INDEX: 104; top: 760px; margin:auto; width: 91%; height: 550px; left:5%;" >
        <ul>
            <li><a href="#tabs-1">Work Orders</a></li>
            <li><a href="#tabs-2">Rolls</a></li>
            <li><a href="#tabs-3">Operator</a></li>
            <li><a href="#tabs-4">Interval(Hours)</a></li>
        </ul>
        <div id="tabs-1">
            <table id="wogrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; width:80%; "></table>
                <div id="gridpager1" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
        </div>
        <div id="tabs-2">
            <table id="rlgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; width:80%; "></table>
                <div id="gridpager2" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
        </div>
        <div id="tabs-3">
            <table id="opgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; width:80%; "></table>
                <div id="gridpager3" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
        </div>
        <div id="tabs-4">
            <table id="hrgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; width:80%; "></table>
                <div id="gridpager4" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
        </div>
    
    </div>
<meta charset=utf-8 />
  <meta name="viewport" content="width=device-width, initial-scale=.9">
<input type="hidden" value="0" id="rolldata_Hidden" runat="server"/>
<input type="hidden" value="0" id="workorderdata_Hidden" runat="server"/>
<input type="hidden" value="0" id="hourlydata_Hidden" runat="server"/>
<input type="hidden" value="NONE" id="DataNo_Hidden" runat="server"/>
<input type="hidden" value="-1" id="InspectionId_Hidden" runat="server"/>
<input type="hidden" value="-1" id="HasReports_Hidden" runat="server"/>
<input type="hidden" value="-1" id="SelectId_Hidden" runat="server"/>
<input type="hidden" value="-1" id="SelectName_Hidden" runat="server"/>
<input type="hidden" value="-1" id="WOTableData_Hidden" runat="server"/>
<input type="hidden" value="-1" id="RLTableData_Hidden" runat="server"/>
<input type="hidden" value="-1" id="HRTableData_Hidden" runat="server"/>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">

<style>
    #linebar {
    font-size: 0.5em;
    font-family: sans-serif;
    color: white;
    background-color: steelblue;
    text-align: right;
    padding: 0.5em !important;
    margin: 0.2em !important;
}
    #sopt_menu {
        font-size: 11px;
        left: 586px;
        top: 770px;
        z-index: 130;
    }
    .ui-jqgrid tr.jqgrow td {
      font-weight: normal;
      overflow: hidden;
      white-space: pre;
      height: 22px;
      padding: 0 2px 0 2px;
      border-bottom-width: 1px;
      border-bottom-color: inherit;
      border-bottom-style: solid;
      color: black;
      font-size: medium;
    }
</style>
<link href="../../Styles/ui.jqgrid.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/jquery.layout.js" type="text/javascript"></script>
<script src="../../Scripts/grid.locale-en.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.jqGrid.js" type="text/javascript"></script>
<script src="http://d3js.org/d3.v3.min.js" charset="utf-8"></script>
<script src="http://d3js.org/d3.v3.js"></script>
<script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20141.34.js" type="text/javascript"></script>
<link href="http://cdn.wijmo.com/themes/arctic/jquery-wijmo.css" rel="stylesheet" type="text/css" />
<link href="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">

    var showfilter = false;
    var fromdatestring;
    var todatestring;
    var loadcount = 0;
    var PageFlag = false;
    var ReportList;
    var HasReports;
    var HasNoWorkOrders;
    var workorderdata; 
    var rolldata;
    var hourlydata;
    var Operatordata;
    var CID;
    var HP_ChartRange;
    var LocationNames;
    var selectedId = <%=SelectedId%>;
    var ChartMedType;
    $(document).ready(function () {
        fromdatestring = "<%=fromdatestring%>"
        todatestring = "<%=todatestring%>"
        var InspectStats = <%=InspectStats%>;
        var fromdate = new Date(fromdatestring);
        var todate = new Date(todatestring);
        HasNoWorkOrders = <%=HasNoWorkOrders%>;
        HasReports = '<%=HasReports%>';
        ReportList = <%=ReportList%>;
        workorderdata = <%=WorkOrderData%>;
        rolldata = <%=RollData%>;
        Operatordata = <%=OpertorData%>;
        hourlydata = <%=HourlyData%>;
        HP_ChartRange = <%=HP_ChartRange%>;
        LocationNames = <%=LocationNames%>;
        ChartMedType = '<%=ChartMedType%>';

        //document.body.style.zoom="90%"
        $("#MainContent_DateFrom_Hidden").val(fromdatestring);
        $("#MainContent_DateTo_Hidden").val(todatestring);

        if (HP_ChartRange[0] != 0) { 
            //charthandler.Create(HP_ChartRange); 
            charthandler.CreateGraph2();
        }
        if (HasReports == 'False') {
            $("#GetReports").css("display", "none");} else { 
             
            for (var i = 0; i < ReportList.length; i++) { 
                
                if (ReportList[i].value == false) { 
                    var repn = i + 1;
                    
                    $("#ReportHolder" + repn.toString()).toggle();
                }
               
            }
          
        } 
        if (HasReports == true) { 
            $('#MainContent_HasReports_Hidden').val('True');
        }

        if (HasNoWorkOrders != true) { 
            //charthandler.CreateExample(DefectMasterHistogram);
            //charthandler.CreateDefectDescPie(DefectByType);
            //charthandler.CreatePie(DefectByEmployeeNo);
        } 
        
        $("article").css("height","1300px");
        if (InspectStats.length == 3 ) { 
            $('#RollCountval').text(InspectStats[2].value);
            $('#WorkOrderval').text(InspectStats[1].value);
            $('#TotalYardsval').text(InspectStats[0].value);
        }
     
        var WorkOrdergridInit = JSON.stringify(workorderdata);
        var RollInit = JSON.stringify(rolldata);
        var HourlyInit = JSON.stringify(hourlydata);
        var OperatorInit = JSON.stringify(Operatordata);
        
        $("#MainContent_workorder_Hidden").val(WorkOrdergridInit);
        $("#MainContent_rolldata_Hidden").val(RollInit);
        $("#MainContent_hourlydata_Hidden").val(HourlyInit);
        $("#MainContent_Operatordata_Hidden").val(OperatorInit);
        //date.setDate(date.getDate() - 4);

        $("#TxtDateFrom").wijinputdate({
            dateFormat: 'd',
            dateChanged: function (e, data) { 
                var formatted_Fromdate = (data.date.getMonth() + 1) + "/" + data.date.getDate()  + "/" + data.date.getFullYear();
               
                $("#MainContent_DateFrom_Hidden").val(formatted_Fromdate);
            
            },
            date: fromdate
        });
        //date.setDate(date.getDate() + 4)
        $("#TxtDateTo").wijinputdate({
            dateFormat: 'd',
            dateChanged: function (e, data) { 
                var formatted_Todate = (data.date.getMonth() + 1) + "/" + data.date.getDate()  + "/" + data.date.getFullYear();
               
                $("#MainContent_DateTo_Hidden").val(formatted_Todate);
            },
            date: todate
        });
        var html = [];
        var name;
        for(var i = 0; i < LocationNames.length; i++){
            name = LocationNames[i];
            html.push('<option value="'+name.id+'">'+name.text+'</option>');
        }
        console.log(html)
        $("#selectNames").html(html.join('')).bind("change", function(){

            var selectedid = $(this).val();
            var fromdateval = $("#TxtDateFrom").val();
            var todateval = $("#TxtDateTo").val();
            
                if (selectedid) {
                $("#SelectId_Hidden").val(selectedid); 
                var querystring = "LocationId=" + selectedid.toString() + "&datefrom=" + fromdateval.toString() + "&dateto=" + todateval.toString() + "&selectChange=1"
                var hash;
                
                window.location.assign("<%=Session("BaseUri")%>" + "/APP/Presentation/SPCProductionReporter.aspx?" + querystring)
            }
    
        });
        $("#selectNames").val(selectedId.toString());
           
        
        $("#tabs").wijtabs();
        gridhandler.RenderWorkOrderData();
        $("#tabs").wijtabs("select", 1);
        gridhandler.RenderRollData(); 
        $("#tabs").wijtabs("select", 2);
        gridhandler.RenderOppData(); 
        $("#tabs").wijtabs("select", 3);
        gridhandler.RenderHourlyGrid();
        $("#tabs").wijtabs("select", 0);
        var gridstate = 0; 
        var clickcount = 0;
        
        $(".divflip").click(function () { 
            if (HasReports == "True") { 
                $("#DateDiv").toggle(); 
                $("#ReportsDiv").toggle();
            }
        });
       
    });
    var MngrTemplateId;
    var wocolnames = ["ID", "Machine", "WorkOrder", "Operator", "StartTime", "FinishTime", "DataNo", "GreigeNo", "CutLengthSpec", "JobYds", "JobSheets", "JobOverLengthInches", "ScheduledTime", "DownTime", "RunTime", "AvgSheetsPerHour", "JDECOMP", "JDESCRAP", "JDETOTREC", "DIFF_PERC"];
    var wocol_stt = [{ name: 'ID', index: 'ID', hidden:true},
                     { name: 'Machine', index: 'Machine', sortable: false, width: 130}, 
                     { name: 'WorkOrder', index: 'WorkOrder', width: 125},
                     { name: 'OperatorNo', index: 'OperatorNo', width: 90},
                     { name: 'StartTime', index: 'StartTime', width: 185, sorttype: 'date', formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}},
                     { name: 'FinishTime', index: 'FinishTime', width: 185, sorttype: 'date', formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}},
                     { name: 'DataNo', index: 'DataNo', width: 90},
                     { name: 'GreigeNo', index: 'GreigeNo', width: 145, hidden:true},
                     { name: 'CutLengthSpec', index: 'CutLengthSpec', width: 100}, 
                     { name: 'JobYds', index: 'JobYds', width: 120, searchoptins:true, sorttype:'float', searchoptions:{sopt:['eq','ne','le','lt','gt','ge']} }, 
                     { name: 'JobSheets', index: 'JobSheets', width: 90, searchoptins:true, sorttype:'integer', searchoptions:{sopt:['eq','ne','le','lt','gt','ge']} }, 
                     { name: 'JobOverLengthInches', index: 'JobOverLengthInches', width: 120, searchoptins:true, sorttype:'float', searchoptions:{sopt:['eq','ne','le','lt','gt','ge']} }, 
                     { name: 'ScheduledTime', index: 'ScheduledTime', width: 85, hidden: true}, 
                     { name: 'DownTime', index: 'Downtime', width: 75 }, 
                     { name: 'RunTime', index: 'RunTime', width: 75 }, 
                     { name: 'AvgSheetsPerHour', index: 'AvgSheetsPerHour', width: 90}, 
                     { name: 'JDECOMP', index: 'JDECOMP', width: 75}, 
                     { name: 'JDESCRAP', index: 'JDESCRAP', width: 75},
                     { name: 'JDETOTREC', index: 'JDETOTREC', width: 75},
                     { name: 'DIFF_PERC', index: 'DIFF_PERC', width: 75,  searchoptins:true, sorttype:'float', searchoptions:{sopt:['eq','ne','le','lt','gt','ge']}}
    ];

    var rlcolnames = ['RollProductionID', 'Machine', 'OperatorNo', 'StartTime', 'EndTime', 'TotalYds', 'TotalSheets', 'TicketYds', 'TicketOverYds', 'RollNo', 'JobNo', 'DataNo', 'GreigeNo', 'TimeStamp_Trans'];
    var rlcol_stt = [{ name: 'RollProductionID', index: 'RollProductionID', width: 125, hidden:true},
                     { name: 'Machine', index: 'Machine', sortable: false, width: 120}, 
                     { name: 'OperatorNo', index: 'OperatorNo', width: 90},
                     { name: 'StartTime', index: 'StartTime', width: 185, sorttype: 'date', formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}},
                     { name: 'EndTime', index: 'FinishTime', width: 185, sorttype: 'date', formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}},
                     { name: 'TotalYds', index: 'TotalYds', width: 90},
                     { name: 'TotalSheets', index: 'TotalSheets', width: 90},
                     { name: 'TicketYds', index: 'TicketYds', width: 90}, 
                     { name: 'TicketOverYds', index: 'TicketOverYds', width: 90}, 
                     { name: 'RollNo', index: 'RollNo', width: 125}, 
                     { name: 'JobNo', index: 'JobNo', width: 125}, 
                     { name: 'DataNo', index: 'DataNo', width: 125}, 
                     { name: 'GreigeNo', index: 'GreigeNo', width: 145},
                     { name: 'TimeStamp_Trans', index: 'TimeStamp_Trans', width: 185, formatter: 'date', hidden: true}
    ];
    var opcolnames = ['OperatorID', 'Machine', 'OperatorNo', 'StartTime', 'EndTime', 'TotalYds', 'TotalSheets', 'Efficiency', 'AvgSheetsPerMin', 'AvgYdsPerMin', 'OverLengthInches'];
    var opcol_stt = [{ name: 'OperatorID', index: 'OperatorID', width: 125, hidden:true},
                     { name: 'Machine', index: 'Machine', sortable: false, width: 160}, 
                     { name: 'OperatorNo', index: 'OperatorNo', width: 120},
                     { name: 'StartTime', index: 'StartTime', width: 195, sorttype: 'date', formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}},
                     { name: 'EndTime', index: 'FinishTime', width: 195, sorttype: 'date', formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}},
                     { name: 'TotalYds', index: 'TotalYds', width: 110},
                     { name: 'TotalSheets', index: 'TotalSheets', width: 110},
                     { name: 'Efficiency', index: 'TicketYds', width: 110}, 
                     { name: 'AvgSheetsPerMin', index: 'TicketOverYds', width: 120}, 
                     { name: 'AvgYdsPerMin', index: 'RollNo', width: 120}, 
                     { name: 'OverLengthInches', index: 'JobNo', width: 125}, 
    ];
    var hrcolnames = ['HourID', 'Machine', 'HourBegin', 'WorkOrderID', 'ProductCount','HourlyYds', 'OverLengthInches', 'CutLengthSpec', 'RunTime', 'DownTime'];
    var hrcol_stt = [{ name: 'HourID', index: 'HourID', width: 125},
                     { name: 'Machine', index: 'Machine', sortable: false, width: 120}, 
                     { name: 'HourBegin', index: 'HourBegin', sorttype: 'date', width: 145, formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}},
                     { name: 'WorkOrderID', index: 'WorkOrderID', width: 125},
                     { name: 'ProductCount', index: 'ProductCount', width: 90},
                     { name: 'HourlyYds', index: 'HourlyYds', width: 90},
                     { name: 'OverLengthInches', index: 'OverLengthInches', width: 90},
                     { name: 'CutLengthSpec', index: 'CutLengthSpec', width: 120}, 
                     { name: 'RunTime', index: 'RunTime', width: 185, sorttype: 'date' }, 
                     { name: 'DownTime', index: 'Downtime', width: 185, sorttype: 'date' }
        ];
    var gridhandler = {
        RenderWorkOrderData: function () {
            var colModelObj;
            var colNameObj; 

            colModelObj = wocol_stt;
            colNameObj = wocolnames;

            $("#wogrid").jqGrid({
                datatype: "local",
                colNames: colNameObj,
                colModel: colModelObj,
                pager: '#gridpager1',
                caption: "Work Order Summary",
                autowidth: false,
                shrinkToFit: false, 
                width: Number($('#tabs').width() - 20),
                multiselect: false,
                loadonce: false,
                gridview: true,
                scroll: true,
                viewrecords: true,
                data: workorderdata,
                height: "390",
                onSelectRow: function (id) {
                           
                },
                gridComplete: function () {

                    if (loadcount > 1 && PageFlag == false ) { 

                        $('#loading').toggle();
                                
                    }
                    var mydata = $("#wogrid").jqGrid('getGridParam','data');
                    var stringarray = JSON.stringify(mydata);
                                
                    $("#MainContent_WOTableData_Hidden").val(stringarray);      
                    if (PageFlag == true) { PageFlag = false}
                    loadcount++;
                }
            });
            $("#wogrid").jqGrid('navGrid', '#gridpager1',
                {
                    edit: false,
                    add: false,
                    del: false,
                    search: false,
                    edittext: "Delete"
                }
                    );
            $("#wogrid").setGridParam({
                onPaging: function() {
                           
                    PageFlag = true;
                }
            });
            jQuery("#wogrid").jqGrid('filterToolbar',{
                searchOperators : true, 
                afterSearch: function () { 
                        
                    var mydata = $("#wogrid").jqGrid('getGridParam','data');
                    var stringarray = JSON.stringify(mydata);
                                
                    $("#MainContent_WOTableData_Hidden").val(stringarray);
                               
                }
            }).trigger('reloadGrid');
            //jQuery("#wijgrid").jqGrid('filterToolbar',{
            //    searchOperators : true, 
            //    afterSearch: function () { 
                               
            //    }
            //}).trigger('reloadGrid');
                
        },
        RenderRollData: function () { 

            var colModelObj;
            var colNameObj; 
           
            colModelObj = rlcol_stt;
            colNameObj = rlcolnames;
                    
            $("#rlgrid").jqGrid({
                datatype: "local",
                colNames: colNameObj,
                colModel: colModelObj,
                pager: '#gridpager2',
                caption: "Roll Production Summary",
                autowidth: false,
                shrinkToFit: false, 
                width: Number($('#tabs').width() - 20),
                multiselect: false,
                loadonce: false,
                gridview: true,
                rowNum: 13,
                viewrecords: true,
                data: rolldata,
                height: "100%",
                gridComplete: function () {
                    var mydata = $("#rlgrid").jqGrid('getGridParam','data');
                    var stringarray = JSON.stringify(mydata);
                                
                    $("#MainContent_RLTableData_Hidden").val(stringarray);   

                }
            });
            jQuery("#rlgrid").jqGrid('filterToolbar',{
                searchOperators : true, 
                afterSearch: function () { 
                               
                }
            }).trigger('reloadGrid');
        },
        RenderOppData: function () { 

            var colModelObj;
            var colNameObj; 
           
            colModelObj = opcol_stt;
            colNameObj = opcolnames;
                    
            $("#opgrid").jqGrid({
                datatype: "local",
                colNames: colNameObj,
                colModel: colModelObj,
                pager: '#gridpager3',
                caption: "Operator Production Summary",
                autowidth: true,
                shrinkToFit: true, 
                //width: Number($('#tabs').width() - 20),
                multiselect: false,
                loadonce: false,
                gridview: true,
                rowNum: 13,
                viewrecords: true,
                data: Operatordata,
                height: "100%",
                gridComplete: function () {
                    var mydata = $("#opgrid").jqGrid('getGridParam','data');
                    var stringarray = JSON.stringify(mydata);
                                
                    $("#MainContent_RLTableData_Hidden").val(stringarray);   

                }
            });
            jQuery("#opgrid").jqGrid('filterToolbar',{
                searchOperators : true, 
                afterSearch: function () { 
                               
                }
            }).trigger('reloadGrid');
        },
        RenderHourlyGrid: function (CID) { 
            var colModelObj;
            var colNameObj; 

            colModelObj = hrcol_stt;
            colNameObj = hrcolnames;

            $("#hrgrid").jqGrid({
                datatype: "local",
                colNames: colNameObj,
                colModel: colModelObj,
                pager: '#gridpager4',
                caption: "Hourly Production Summary",
                autowidth: true,
                multiselect: false,
                loadonce: false,
                gridview: true,
                rowNum: 13,
                viewrecords: true,
                data: hourlydata,
                height: "100%",
                shrinkToFit:true,
                gridComplete: function () {
                    var mydata = $("#hrgrid").jqGrid('getGridParam','data');
                    var stringarray = JSON.stringify(mydata);
                                
                    $("#MainContent_HRTableData_Hidden").val(stringarray);   

                }
            });
            jQuery("#hrgrid").jqGrid('filterToolbar',{
                searchOperators : true, 
                afterSearch: function () { 
                               
                }
            }).trigger('reloadGrid');
        },
        formatSpecGrid: function (cellvalue, options, rowobject) { 

            var specdelta = new Number(rowobject.SpecDelta);
            var Specvaluelower = new Number(rowobject.Spec_Value_Lower);
            var Specvalueupper = new Number(rowobject.Spec_Value_Upper);
            if (specdelta < 0) { 
                console.log('hit-specneg');
                if (specdelta < Specvaluelower) {   
                    return "<span style='color:red; font-weight:bolder'>" + cellvalue + "</span>";
                } else { 
                    return "<span style='color: green; font-weight: normal;'>" + cellvalue + "</span>";
                }
            } else { 
                if (rowobject.SpecDelta > rowobject.Spec_Value_Upper) { 
                    return "<span style='color:red; font-weight:bolder'>" + cellvalue + "</span>";
                } else { 
                    return "<span style='color: green; font-weight: normal;'>" + cellvalue + "</span>";
                }
            }
        },
        GetUniqueNames: function(columnName) { 
            var texts = $('#wijgrid').jqGrid('getCol',columnName), uniqueTexts = [],
            textsLength = texts.length, text, textsMap = {}, i;
            var mydata = $('#wijgrid').jqGrid('getGridParam','data');
            var mydata1 = $('#wijgrid').jqGrid("getGridParam", "data"),
               DataNoList = $.map(mydata1, function (item) { 
                   if (columnName == 'DataNo') {
                       return item.DataNo; 
                   } else if (columnName == 'InspectionId') { 
                       return item.InspectionId
                   }
               });


            for (i=0;i<DataNoList.length;i++) {
                text = DataNoList[i];
                if (text !== undefined && textsMap[text] === undefined) {
                    // to test whether the texts is unique we place it in the map.
                    textsMap[text] = true;
                    uniqueTexts.push(text);
                }
            }
            return uniqueTexts;
        },
        buildSearchSelect : function(uniqueNames) {
            var values=":All";
            $.each (uniqueNames, function() {
                values += ";" + this + ":" + this;
            });
            return values;
        },
        setSearchSelect: function(columnName) {
            $('#wijgrid').jqGrid('setColProp', columnName,
                        {
                            stype: 'select',
                            searchoptions: {
                                value:gridhandler.buildSearchSelect(gridhandler.GetUniqueNames(columnName)),
                                sopt:['eq']
                            }
                        }
            );
        }
    };
    var charthandler = { 
        CreateGraph2 : function () { 
            var parseDate = d3.time.format("%x %I:%M:%S %p").parse;
            //var borderwidth = document.getElementById('linchartborder').clientWidth;
            var borderwidth = $('body').width();
            HP_ChartRange.forEach(function(d) {
                d.HOURBEGIN = parseDate(d.HOURBEGIN);
            });

            var margin = {top: 20, right: 55, bottom: 30, left: 40},
                  width  = borderwidth * .67,
                  height = 600  - margin.top  - margin.bottom;
            //var x = d3.scale.ordinal()
            //    .rangeRoundBands([0, width], 1);
            var y = d3.scale.linear()
                .rangeRound([height, 0]);
            var x = d3.time.scale()
                .domain(d3.extent(HP_ChartRange, function(d) { return d.HOURBEGIN; }))
                .range([0, width])
                .nice(d3.time.hour);
            //var y = d3.scale.linear()
            //    .range([height, 0]);
            var xAxis = d3.svg.axis()
                .scale(x)
                .orient("bottom")
                .ticks(d3.time.hour,12)
                .tickFormat(d3.time.format('%b %d'));
            //.ticks(d3.time.hour, 24)
            //.tickFormat(d3.format("%b %d"))
            //.tickSubdivide(true)
            
            var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left");
            var stack = d3.layout.stack()
                    .offset("zero")
                    .values(function (d) { return d.values; })
                    .x(function (d) { return x(d.date); })
                    .y(function (d) { return d.value; });
            var area = d3.svg.area()
                    .interpolate("cardinal")
                    .x(function (d) { return x(d.date); })
                    .y0(function (d) { return y(d.y0); })
                    .y1(function (d) { return y(d.y0 + d.y); });
            var color = d3.scale.ordinal()
                    .range(["rgb(0, 77, 156)","rgb(119, 148, 43)","rgb(134, 138, 123)","rgb(183, 100, 88)","#d3c47c", "rgb(226, 230, 49)", "rgb(37, 37, 37)", "rgb(134, 166, 200)", "rgb(192, 219, 123)", "rgb(163, 21, 0)"]);
            var svg = d3.select('#visualisation').append("svg")
                    .attr("width",  width  + margin.left + margin.right)
                    .attr("height", height + margin.top  + margin.bottom)
                  .append("g")
                    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
            //d3.csv("data/crunchbase-quarters.csv", function (error, data) {
            $(function () {
                console.log(HP_ChartRange);
                
                var labelVar = 'HOURBEGIN';
                var varNames = d3.keys(HP_ChartRange[0])
                    .filter(function (key) { return key !== labelVar;});
                color.domain(varNames);
                var seriesArr = [], series = {};
                varNames.forEach(function (name) {
                    series[name] = {name: name, values:[]};
                    seriesArr.push(series[name]);
                });
                HP_ChartRange.forEach(function (d) {
                    varNames.map(function (name) {
                        series[name].values.push({date: d[labelVar], value: +d[name]});
                    });
                });
                //x.domain(HP_ChartRange.map(function (d) { return d.HOURBEGIN; }));
          
                stack(seriesArr);
                y.domain([0, d3.max(seriesArr, function (c) { 
                    return d3.max(c.values, function (d) { return d.y0 + d.y; });
                })]);
                
                var selection = svg.selectAll(".series")
                  .data(seriesArr)
                  .enter().append("g")
                  .attr("class", "series");
                selection.append("path")
                  .attr("class", "streamPath")
                  .attr("d", function (d) { return area(d.values); })
                  .style("fill", function (d) { return color(d.name); })
                  .attr("transform", "translate(120,0)")
                  .style("stroke", "grey");
                svg.append("g")
                   .attr("class", "x axis")
                   .attr("transform", "translate(120," + height + ")")
                   .call(xAxis);
                svg.append("g")
                    .attr("class", "y axis")
                    .attr("transform", "translate(120,0)")
                    .call(yAxis)
                  .append("text")
                    .attr("transform", "rotate(0)")
                    .attr("x", 146)
                    .attr("y", 6)
                    .attr("dy", ".71em")
                    .style("text-anchor", "end")
                    .style("font-size", "medium")
                    .text("Number of " + ChartMedType + "");
                //var points = svg.selectAll(".seriesPoints")
                //  .data(seriesArr)
                //  .enter().append("g")
                //  .attr("class", "seriesPoints");

                //points.selectAll(".point")
                //  .data(function (d) { return d.values; })
                //  .enter().append("circle")
                //   .attr("class", "point")
                //   .attr("cx", function (d) { return x(d.label) + x.rangeBand() / 2; })
                //   .attr("cy", function (d) { return y(d.y0 + d.y); })
                //   .attr("r", "10px")
                //   .style("fill",function (d) { return color(d.name); })
                //   .on("mouseover", function (d) { showPopover.call(this, d); })
                //   .on("mouseout",  function (d) { removePopovers(); })
                var legend = svg.selectAll(".legend")
                    .data(varNames.slice().reverse())
                  .enter().append("g")
                    .attr("class", "legend")
                    .attr("transform", function (d, i) { return "translate(55," + i * 20 + ")"; });
                legend.append("rect")
                    .attr("x", -16)
                    .attr("width", 10)
                    .attr("height", 10)
                    .style("fill", color)
                    .style("stroke", "grey");
                legend.append("text")
                    .attr("x", - 19)
                    .attr("y", 6)
                    .attr("dy", ".35em")
                    .style("text-anchor", "end")
                    .text(function (d) { return d; });

                
            });
        }


    };

    function removePopovers () {
        $('.popover').each(function() {
            $(this).remove();
        }); 
    }
    function showPopover (d) {
        $(this).popover({
            title: d.name,
            placement: 'auto top',
            container: 'body',
            trigger: 'manual',
            html : true,
            content: function() { 
                return "Quarter: " + d.label + 
                       "<br/>Rounds: " + d3.format(",")(d.value ? d.value: d.y1 - d.y0); }
        });
        $(this).popover('show')
    }
</script>


    <style>

.bar {
  fill: steelblue;
}

.bar:hover {
  fill: brown;
}

.axis {
  font: 10px sans-serif;
}

.axis path,
.axis line {
  fill: none;
  stroke: #000;
  shape-rendering: crispEdges;
}

.x.axis path {
  display: none;
}
.browser text {
  text-anchor: end;
}
.line {
  fill: none;
  stroke: steelblue;
  stroke-width: 1.5px;
}
.statspad {
          color: #fff;
          background-color: #6496c8;
          text-shadow: -1px 1px #417cb8;
          border: solid;
          border-color:white;
          display: inline-block;
          margin: 0 10px 0 0;
          padding: 15px 45px;
          font-size: 48px;
          font-family: "Bitter",serif;
          line-height: 1.8;
          appearance: none;
          box-shadow: none;
          border-radius: 0;
    }
.statspad_label {
        font-family: sans-serif;
        font-size: xx-small;
        color: white;
        position: absolute;
        }
</style>
<script>
    var DefectMasterMonthlyCount;// = [{ "Listdate": "2014-08-01", "Count": 0 }, { "Listdate": "2014-09-01", "Count": 0 }, { "Listdate": "2014-10-01", "Count": 0 }, { "Listdate": "2014-11-01", "Count": 15 }, { "Listdate": "2014-12-01", "Count": 146 }];
    
    var dataDefault = [{ "date": "1900-01-20", "total": 3 }, { "date": "1900-03-21", "total": 0 }, { "date": "1900-03-22", "total": 0 }, { "date": "1900-03-23", "total": 0 }, { "date": "1900-03-24", "total": 0 }];

</script>
<script type="text/javascript" src="../../Scripts/D3/Donut3D.js"></script>
<script type="text/javascript">
    
    var DefectByType;
    var salesData=[
	{label:"Basic", color:"#3366CC"},
	{label:"Plus", color:"#DC3912"},
	{label:"Lite", color:"#FF9900"},
	{label:"Elite", color:"#109618"},
	{label:"Delux", color:"#990099"}
    ];
    $(document).ready(function () {

      
        
        
       

        var piecartheight = 300;
        var ChartWidth = 585;
        var Axcenter = 200;
        var Bxcenter = 460;
        var rxradius = 110;
        var ryradius = 90;
        var hheight = 30;
        var fontsize1 = "medium";

        
    });
</script>

</asp:Content>

