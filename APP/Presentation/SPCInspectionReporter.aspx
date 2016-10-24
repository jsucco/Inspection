<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Site_2.master" AutoEventWireup="false" CodeFile="SPCInspectionReporter.aspx.vb" Inherits="core.APP_DataEntry_TemplateManager" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ OutputCache Location="Server" VaryByParam="*" Duration="2000" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
  <%--<link rel="stylesheet" type="text/css" href="../../Styles/slick.css"/>
  <link rel="stylesheet" type="text/css" href="../../Styles/slick-theme.css"/>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="loading" style="width:75%; top:150px; height:520px; left:310px; position:absolute; background-color:white; z-index:1000;">
        <div style="width:100%; top:7px; height:100%; position:absolute;z-index:0; background-color:lightgray; opacity:.4;"></div>
        <input type="image" src="../../Images/load-indicator.gif" style="z-index:200; margin-left: 41%; margin-top:20%; position:absolute;" />
    </div>
    <div style="Z-INDEX: 102; LEFT: 30px; POSITION: relative; TOP: -10px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
        <select id="selectNames"  name="LocationId" style="width: 162px; height: 35px; position:relative; top:12px; " class="inputelement"></select>
               
            </div>
    <div style="position:relative; left: 30px; top: 25px;">
        <asp:Label ID="LblDateFrom" runat="server" CssClass="labelleft1" >Begin Date</asp:Label>
        <input type="text" id="TxtDateFrom" name="datefrom" style="float:left;" class=labelleft1 />
        <input type="hidden" id="DateFrom_Hidden" runat="server" />
        </div>
    <div style="position:relative; left: 30px; top: 60px;">
        <asp:Label ID="LblDateTo" runat="server" CssClass="labelleft1" >End Date  </asp:Label>
        <div style="position:absolute; left:79px; top:-2px;">
            <input type="text" id="TxtDateTo" name="dateto" style="float:left; left:14px;" class=labelleft1 />
            <input type="hidden" id="DateTo_Hidden" runat="server"/>
        </div>
        
        </div>
    <div id="DateDiv" style="position: relative; width: 12%; height: 250px; top: 80px; left: 30px; display:none;" class="statspad">
        <label id="TotalDefects" style="top: 10px; left: 30px;" class="statspad_label">Total Defects: </label>
        <label id="TotalDefectsval" style="top: 10px; left: 80%" class="statspad_label">0_</label>
        <label id="WorkOrder" style="top: 50px; left: 30px;" class="statspad_label"># WorkOrders: </label>
        <label id="WorkOrderval" style="top: 50px; left: 80%" class="statspad_label">0_</label>
        <label id="DefPerWorkOrder" style="top: 90px; left: 30px;" class="statspad_label">Defects Per WorkOrder Avg: </label>
        <label id="DefPerWorkOrderval" style="top: 90px; left: 80%" class="statspad_label">0_</label>
        <div id = "Chk1" style="position:absolute; top: 220px; left: 30px; display:none; padding: 0px;" class="statspad_label">
                                        <input id="Filter" name="ChkBx1" style="display:none;" type="checkbox" class=chkbox2 />Filter</div>
        <%--<input id="GetReports" type="button" class="export divflip" style="position:absolute; font-size:xx-small; left: 0px; width: 75px; top: 235px; height: 40px;" value="REPORTS" />--%>
        
    </div>
    <div id="ReportsDiv" style="position:relative; width: 12%; height:250px; top:80px; left:30px;" class="statspad">
        <div id="ReportHolder1" style=" height: 50px; position: relative; top: 0px"><input type="hidden" id="reportname1_hidden" value="Ins_GriegeBatch" runat="server" /><input type="hidden" id="reportstatus1_hidden" runat="server" value="false" />
         <input type="checkbox" runat="server" style="position:relative; top:-13px; left: -10%; transform: scale(1.5);" id="check1">
         <label runat="server" id="ReportLabel1" class="statspad_label" style="position:relative; font-size:13px;  top:-16px;">            GRIEGE BATCH</label></div>
        <div id="ReportHolder2" style="position:relative; height: 50px;">
        <input type="checkbox"  runat="server" style="position:relative; top:-20px; left: -10%;transform: scale(1.5);" id="check2">
        <label runat="server"  id="ReportLabel2" class="statspad_label" style=" top:-23px; font-size:13px;  position:relative;">            JOB SUMMARY</label>
        <input type="hidden" id="reportname2_hidden" value="Ins_WorkOrderInspection" runat="server" /><input type="hidden" id="reportstatus2_hidden" runat="server" value="false" />
        </div>
        <div id="ReportHolder3" style="position:relative; height:50px;">
        <input type="checkbox"  runat="server" style="position:relative; top:-20px; left: -10%;transform: scale(1.5);" id="check3">
        <label runat="server"  id="ReportLabel3" class="statspad_label" style="font-size:13px;  position:relative; top: -20px;">            COMPLIANCE</label>
        <input type="hidden" id="reportname3_hidden" value="Ins_WorkOrderCompliance" runat="server" /><input type="hidden" id="reportstatus3_hidden" runat="server" value="false" />
        </div>
        <div id="ReportHolder4" style="position:relative; height:50px;">
        <input type="checkbox"  runat="server" style="position:relative; top:-20px; left: -10%;transform: scale(1.5);" id="check4">
        <label runat="server"  id="ReportLabel4" class="statspad_label" style="font-size:13px;  position:relative; top: -20px;">            DEFECTS GRID</label>
        <input type="hidden" id="reportname4_hidden" value="Ins_DefectsGrid" runat="server" /><input type="hidden" id="reportstatus4_hidden" runat="server" value="false" />
        </div>
         <div id="ReportHolder5" style="position:relative; height:50px;" >
        <input type="checkbox"  runat="server" style="position:relative; top:-20px; left: -10%;transform: scale(1.5);" id="check5">
        <label runat="server"  id="ReportLabel5" class="statspad_label" style="  font-size:13px;  position:relative;  top: -20px;">            SPECS GRID</label>
        <input id="reportname5_hidden" type="hidden" value="Ins_SpecsGrid" runat="server" /><input type="hidden" id="reportstatus5_hidden" runat="server" value="false" />
         </div>
        <asp:Button ID="GetReport1" runat="server" OnClientClick="FlagExport()" Text="Export" style="position:relative; font-size:xx-small; top: -15px; left:118px; font-size:small; width: 75px; height: 40px; border-color:white; border-width:3px; right: 10px;"/>
        <%--<input id="GetDate" type="button" class="export divflip" style="position:absolute; font-size:xx-small; left: 20px; width: 75px; top: 220px; height: 40px;" value="GO BACK" />--%>
        <%--<asp:Button ID="GoButton" runat="server" Text="Export" style="position:relative; font-size:xx-small; left:79%; width: 75px; top: 220px; height: 40px;" class=export/>--%>
    </div>
    <asp:Button ID="GODate" runat="server" Text="GO" style="position:relative; font-size:small; left: 30px; width: 75px; top: 40px; height: 40px;" class=export/>
    <div style="position:relative; width: 250px; height:50px; top:130px; left: 30px;">
        <%--<asp:Button ID="GetImage"  runat="server" Text="Get Image" style="position:absolute; font-size:x-small; left: 12px; width: 65px; top: 0px; height: 30px;" class=export />--%>
        <%--<input Id="GetProductSpecs" type="button" value="View Specs" style="position:absolute; font-size:x-small; left: 110px; width: 95px; top: 0px; height: 30px;" class=export />--%>
        <input id="DefectId_Value_Hidden" runat="server" type="hidden" value="0" />
    </div>
    <div style="position:relative;"  >
        <label id="NoData" style="position: absolute; top: -130px; display: none; left: 62%; font-size: large; font-weight: 700; text-decoration: underline;">NO RECORDS FOUND</label>
    </div>
    <div id="linegraph" style="position:absolute; left:28%; top: 130px; width: 400px;"></div>
    <%--<div id="linchartborder" style="position:absolute; left:29.5%; top: 140px; width: 66%;height: 590px;border-style: solid;border-color: rgb(162, 153, 153);">
        <div style="position:relative; width:99%;" div=""></div></div>--%>
    <div id="example" style="position:absolute; left:305px; top: 130px; width:500px; z-index:300;">
        <label id="chart_label" style="font-family:Arial; font-size:13px; font-weight:bold; position:relative; left: 20%; top:65px; z-index:1000; color:black; display:none;" >DEFECT BREAKDOWN</label>
        <%--<svg id="visualisation" width="100%" height="550"></svg>--%>
        <div id="chart_div"></div>
    </div>
    
    <div id="Graph2Div" style="position:absolute; left:54%; top: 150px; width: 805px; z-index:300;">
        <%--<svg id="visualisation" width="100%" height="550"></svg>--%>
        <div id="scatter_dual_y"></div>
    </div>
    <div id="PieLegend" style="position: absolute; top: 445px; left: 31%; width: 50%;"></div>

    </div>--%>
    <input id="DefectID" type="text" style="display:none;" value="testing1232" />

    <div id="tabs" style="position:absolute; Z-INDEX: 104; top: 700px; margin:auto; width: 91%; height: 550px; left:5%;" >
        <ul>
            <li><a href="#tabs-1">JobSummary</a></li>
            <li><a href="#tabs-2">Defect Master</a></li>
            <li><a href="#tabs-3">Spec Master</a></li>
            <li><a href="#tabs-4">Photos</a></li>
        </ul>
        <div id="tabs-1">
            <table id="ijsgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800;  ">
                </table>
                <div id="ijsgridpager1" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
        </div>
        <div id="tabs-2">
            <table id="wijgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800;  ">
                </table>
                <div id="gridpager1" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
        </div>
        <div id="tabs-3">
            <table id="Specgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800 ">
                </table>
                <div id="gridpager2" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
        </div>
        <div id="tabs-4">
            <div id="wijcarousel0" style="height:410px"> 
                </div> 
        </div>
    
    </div>

<meta charset=utf-8 />
  <meta name="viewport" content="width=device-width, initial-scale=.9">
<input type="hidden" value="0" id="TableData_Hidden" runat="server"/>
    <input type="hidden" value="0" id="SpecgridData_Hidden" runat="server"/>
<input type="hidden" value="NONE" id="DataNo_Hidden" runat="server"/>
<input type="hidden" value="-1" id="InspectionId_Hidden" runat="server"/>
<input type="hidden" value="-1" id="HasReports_Hidden" runat="server"/>
<input type="hidden" value="-1" id="SpecgridInspectionId_Hidden" runat="server"/>
<input type="hidden" value="-1" id="TemplateId_Hidden" runat="server"/>
<input type="hidden" value="0" id="ExportFlag_Hidden" runat="server"/>
    <input type="hidden" value="0" id="CID_Hidden" runat="server"/>
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

<%--<script src="../../Scripts/jquery.layout.js" type="text/javascript"></script>
<script src="../../Scripts/grid.locale-en.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.jqGrid.js" type="text/javascript"></script>--%>
<asp:PlaceHolder runat="server">
    <%: Scripts.Render("~/bundles/SPCInspectReporter") %>
</asp:PlaceHolder>
<%--<script src="../../Scripts/slick.min.js" type="text/javascript"></script>--%>
<link href="../../Styles/ui.jqgrid.css" rel="stylesheet" type="text/css" />
<%--<script src="http://d3js.org/d3.v3.min.js" charset="utf-8"></script>
<script src="http://d3js.org/d3.v3.js"></script>
<script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20141.34.js" type="text/javascript"></script>
<link href="http://cdn.wijmo.com/themes/arctic/jquery-wijmo.css" rel="stylesheet" type="text/css" />
<link href="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.css" rel="stylesheet" type="text/css" />--%>

<script type="text/javascript">
    var TemplateNames;
    var DefectMaster;
    var SpecGridData;
    var showfilter = false;
    var selectedId = <%=SelectedId%>;
    var fromdatestring;
    var todatestring;
    var loadcount = 0;
    var DefectMasterMonthly;
    //var DefectMasterHistogram = <%=DefectMasterHistogram%>;
    var Chart2Array = <%=Chart2Array%>;
    var ScatterPlotJson;
    var DefectByEmployeeNo;
    var DefectByType;
    var PageFlag = false;
    var ReportList;
    var HasReports;
    var images; 
    var $CID_hidden = $("#MainContent_CID_Hidden");
    var DefectMasterJson;
    var states = { '1': 'Alabama', '2': 'California', '3': 'Florida', '4': 'Hawaii', '5': 'London', '6': 'Oxford' };
     google.load('visualization', '1.0', { 'packages': ['corechart'] });

     //google.setOnLoadCallback(drawChart2);
     //google.load('visualization', '1.1', {packages: ['scatter']});

     //google.setOnLoadCallback(drawChart22);
    $(document).ready(function () {
        fromdatestring = "<%=fromdatestring%>";
        todatestring = "<%=todatestring%>";
        var InspectStats = <%=InspectStats%>;
        var fromdate = new Date(fromdatestring);
        var todate = new Date(todatestring);
        var HasNoDefects = '<%=HasNoDefects%>'
        //document.body.style.zoom="90%"
        $("#MainContent_DateFrom_Hidden").val(fromdatestring);
        $("#MainContent_DateTo_Hidden").val(todatestring);
        LocationNames = <%=LocationNames%>;
        DefectMaster = <%=TemplateDefectMaster%>
        SpecGridData = <%=ProductDisplaySpecCollection%>;
        images = <%=DefectImageDisplayArray%>;
      //  DefectMasterMonthly = <%=DefectMasterMonthlyCount%>
        DefectByEmployeeNo = <%=DefectByTypeChart%>
        DefectByType = <%=DefectByEmployeeNoChart%>;
        ScatterPlotJson = <%=ScatterPlotJson%>;
        HasReports = '<%=HasReports%>'
        ReportList = <%=ReportList%>;

        $('#loading').css('width', $('.main').width() - 270);
        if (HasReports == 'False') {
            $("#GetReports").css("display", "none");} else { 

            //if (ReportList[0] != 0) { 
            for (var i = 0; i < ReportList.length; i++) { 
                 
                if (ReportList[i].value == false) { 
                    var repn = i + 1;

                    $("#ReportHolder" + repn.toString()).toggle();
                }
                //$("#Reportholder" + i.toString()).toggle();
            }
            // }
        } 
        if (HasReports == 'True') { 
            $('#MainContent_HasReports_Hidden').val('True');
        }
         
        var html = [];
        var name; LocationNames; 
        for(var i = 0; i < LocationNames.length; i++){
            name = LocationNames[i];
            html.push('<option value="'+name.CID+'">'+name.text+'</option>');
        }
        $CID_hidden.val(selectedId); 
        $("#selectNames").html(html.join('')).bind("change", function(){
            $('#loading').toggle();
            var selectedid = $(this).val();
            var fromdateval = $("#TxtDateFrom").val();
            var todateval = $("#TxtDateTo").val();
             
            if (selectedid) {
                var querystring = "LocationId=" + selectedid.toString() + "&datefrom=" + fromdateval.toString() + "&dateto=" + todateval.toString() + "&selectChange=1"
                var hash;
                $CID_hidden.val(selectedid); 
                selectedId = selectedid; 
                console.log(gridhandler.FilterJsonTable(selectedid, DefectMasterJson));
                if (selectedId == "999") { 
                    jQuery("#wijgrid").jqGrid('setGridParam', { data: DefectMasterJson, page: 1 }).trigger('reloadGrid');
                } else {
                    jQuery("#wijgrid").jqGrid('setGridParam', { data: gridhandler.FilterJsonTable(selectedid, DefectMasterJson), page: 1 }).trigger('reloadGrid');
                }
                gridhandler.GetDefectCountBreakdown(); 
                gridhandler.GetScatterPlot();
                //$("#wijcarousel0").empty(); 
                gridhandler.GetDefectImages(); 
                //$("#ijsgrid").jqGrid('setGridParam', 
                //         { datatype: 'json' }).trigger('reloadGrid');
                //$("#wijgrid").jqGrid('setGridParam', 
                //         { datatype: 'json' }).trigger('reloadGrid');
                //$("#Specgrid").jqGrid('setGridParam', 
                //         { datatype: 'json' }).trigger('reloadGrid');
                //window.location.assign("<%=Session("BaseUri")%>" + "/APP/Presentation/SPCInspectionReporter.aspx?" + querystring)
            }
            
        });
        $("#selectNames").val(selectedId.toString());

        if (HasNoDefects != 'True') { 
            
            
            //if (DefectByType[0] != 0 ) {
            //    charthandler.CreateDefectDescPie(DefectByType);
            //}
            //if (DefectByEmployeeNo[0] != 0 ) { 
            //    charthandler.CreatePie(DefectByEmployeeNo);
            //} else { 
            //$('#loading').toggle();
            //}
        } else { 
            $('#loading').toggle();
        }
         
        $("article").css("height","1372px");
        //$("article").css("width","92%");
        //if (InspectStats.length > 0 ) { 

        //    $('#TotalDefectsval').text(InspectStats[0].value);
        //    $('#WorkOrderval').text(InspectStats[1].value);
        //    $('#DefPerWorkOrderval').text(InspectStats[2].value);
        //}

        var SpecgridInit = JSON.stringify(SpecGridData);
        var tabledataInit = JSON.stringify(DefectMaster);
        $("#MainContent_GetImage").disabled = true  ;
        $("#MainContent_SpecgridData_Hidden").val(SpecgridInit);
        $("#MainContent_TableData_Hidden").val(tabledataInit);
       // date.setDate(date.getDate() - 4);

        $("#TxtDateFrom").wijinputdate({
            dateFormat: 'd',
            dateChanged: function (e, data) { 
                var formatted_Fromdate = (data.date.getMonth() + 1) + "/" + data.date.getDate()  + "/" + data.date.getFullYear();
               
                $("#MainContent_DateFrom_Hidden").val(formatted_Fromdate);
            
            },
            date: fromdate
        });
        $("#MainContent_DateFrom_Hidden").val(fromdatestring);
        //date.setDate(date.getDate() + 4)
        $("#TxtDateTo").wijinputdate({
            dateFormat: 'd',
            dateChanged: function (e, data) { 
                var formatted_Todate = (data.date.getMonth() + 1) + "/" + data.date.getDate()  + "/" + data.date.getFullYear();
               
                $("#MainContent_DateTo_Hidden").val(formatted_Todate);
            },
            date: todate
        });
        $("#MainContent_DateTo_Hidden").val(todatestring);
        $('#AQLevel').on('select2-selecting', function (e) {
            var selectedid = e.object.id;
            var querystring = "TemplateId=" + selectedid.toString()
            var hash;
            $.when( $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Utility/cypher.ashx',
                    type: 'POST',
                    data: {"querystring": querystring},
                    success: function (data) {
                        
                        hash = data;
                    },
                    error: function (a, b, c) {

                    }
                }) ).done(function ( v1) {
                    
                    window.location.assign("<%=Session("BaseUri")%>" + "/APP/Presentation/SPCInspectionReporter.aspx?" + querystring + "&qtpval=" + hash.toString())
                });

            window.location.assign("<%=Session("BaseUri")%>" + "/APP/Presentation/SPCInspectionReporter.aspx?TemplateId=" + selectedid.toString())
        });
        gridhandler.GetDefectCountBreakdown(); 
        gridhandler.GetScatterPlot();
        $("#tabs").wijtabs();
        //gridhandler.RenderSummaryGrid();
        $("#tabs").wijtabs("select", 1);
        gridhandler.GetGridManagerData()
        $("#tabs").wijtabs("select", 2);
        gridhandler.RenderSpecGridData();
        $("#tabs").wijtabs("select", 3);
        gridhandler.GetDefectImages();
        
        $("#tabs").wijtabs("select", 0);
        

        var gridstate = 0; 
        var clickcount = 0;
        //$("#GetProductSpecs").click(function () { 
        //    gridhandler.RenderSpecGridData();
        //    window.setTimeout(function () { 
        //        $("#Spechold").toggle(); 
        //        if (clickcount < 2) { 
        //            $("#tablehold").toggle();
        //        }
        //    }, 200);
        //    clickcount = clickcount + 1; 
        //    if (clickcount == 1) { 
        //        $('#GetProductSpecs').val('View Defects');
        //    }
        //    if (clickcount > 1) { 
        //        clickcount = 0; 
        //        location.reload();
        //    }
        //});
        $(".divflip").click(function () { 
            if (HasReports == "True") { 
                $("#DateDiv").toggle(); 
                $("#ReportsDiv").toggle();
            }
        });
       
    });
    var MngrTemplateId;
    var gridhandler = {
        GetGridManagerData: function () {
         $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDefectMasterDisplay', args: { fromdate: fromdatestring, todate: todatestring } },
                    success: function (data) {
                        if (data) { 
                            DefectMasterJson = $.parseJSON(data);
                            $("#wijgrid").jqGrid({
                                datatype: "local",
                                //url:     "<%=Session("BaseUri")%>" + '/handlers/Presentation/DefectMaster_Load.ashx',
                                editurl: "<%=Session("BaseUri")%>" + '/handlers/Presentation/DefectMaster_Edit.ashx',
                                colNames: ['DefectId', 'DefectTime', 'WorkOrder', 'ItemNumber', 'RollNo', 'DataNo', 'EmployeeNo', 'Inspector', 'TemplateId', 'Template Name' , 'InspectionId', 'DefectDesc',  'TotalLotPieces', 'Product', 'LoomNo', 'WorkRoom', 'DataType', 'DefectImage',   'InspectionState'],
                                colModel: [
                                        { name: 'DefectID', index: 'DefectID', hidden: true, editable: true },
                                        { name: 'DefectTime', index: 'DefectTime', sortable: false, width: 185, sorttype: 'date',  formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'} },
                                        { name: 'WorkOrder', index: 'WorkOrder', width: 125 },
                                        { name: 'ItemNumber', Index: 'ItemNumber',hidden: true, width: 120},
                                        { name: 'RollNo', index: 'LotNo', width: 145 },
                                        { name: 'DataNo', index: 'DataNo', width: 145, edittype:'select', editoptions: { value: { '1': 'Alabama', '2': 'California', '3': 'Florida', '4': 'Hawaii', '5': 'London', '6': 'Oxford' } },searchoptions:{ sopt:['eq'] }},
                                        { name: 'EmployeeNo', index: 'EmployeeNo', width: 90 },
                                        { name: 'Inspector', Index: 'Inspector',hidden: true, width: 120},
                                        { name: 'TemplateId', index: 'TemplateId', hidden: true, width: 125 },
                                        { name: 'Name', Index: 'Name', width: 130},
                                        { name: 'InspectionId', index: 'InspectionId', width: 90, searchoptins:true, sorttype:'integer', searchoptions:{sopt:['eq','ne','le','lt','gt','ge']} },
                                        { name: 'DefectDesc', index: 'DefectDesc', width: 200, hidden: false },
                                        { name: 'TotalLotPieces', index: 'TotalLotPieces', width: 90 },
                                        { name: 'Product', index: 'Product', width: 145 },
                                        { name: 'LoomNo', index: 'LoomNo',hidden: true, width: 145 },
                                        { name: 'WorkRoom', index: 'WorkRoom', width: 120 },
                                        { name: 'DataType', index: 'DataType', width: 130 },
                                        { name: 'DefectImage_Filename', index: 'DefectImage_Filename', hidden: true, width: 165 },
                                        { name: 'InspectionState', Index: 'InspectionState', hidden: true, editable: true}
                                ],
                                pager: '#gridpager1',
                                caption: "Defect Manager",
                                autowidth: false,
                                multiselect: false,
                                loadonce: false,
                                gridview: true,
                                data: DefectMasterJson,
                                scroll: true,
                                shrinkToFit: false, 
                                width: Number($('#tabs').width() - 20),
                                viewrecords: true,
                                height: "390",
                                postData: { 
                                    CID: function () { 
                                        return selectedId;
                                    }, 
                                    fromdate: function () { 
                                        return $("#TxtDateFrom").val();
                                    }, 
                                    todate: function () { 
                                        return $("#TxtDateTo").val();
                                    }
                                },
                                onSelectRow: function (id) {
                                    var grid = $("#wijgrid");
                                    var rowdata = grid.getRowData(id);
                     
                                    MngrTemplateId = rowdata.TemplateId;
    
                                    $("#MainContent_DefectId_Value_Hidden").val(rowdata.DefectID);

                                    if (rowdata.DefectImage_Filename) { 
                                        $("#MainContent_GetImage").disabled = false
                                        ;
                                    } else { 
                                        $("#MainContent_GetImage").disabled = true
                            
                                    }
                                },
                                gridComplete: function () {


                                    gridhandler.setSearchSelect("WorkOrder", "wijgrid");
                                    gridhandler.setSearchSelect("RollNo", "wijgrid");
                                    gridhandler.setSearchSelect("EmployeeNo", "wijgrid");
                                    gridhandler.setSearchSelect("DefectDesc", "wijgrid");
                                    gridhandler.setSearchSelect("DataNo", "wijgrid");
                                    gridhandler.setSearchSelect("InspectionId", "wijgrid");
                                    gridhandler.setSearchSelect("Name", "wijgrid");
                                    gridhandler.setSearchSelect("WorkRoom", "wijgrid");
                    
                                    //if (loadcount > 1 && PageFlag == false ) { 
                                    //    $("#PieChart").empty();
                                    //    $("#PieLegend").empty();
                                    //    $('#loading').toggle();
                                
                                    //    gridhandler.GetDefectDescPieChart();
                                    //    gridhandler.GetDefectPieChart();
                                    //}
                           
                                    if (PageFlag == true) { PageFlag = false}
                                    loadcount++;
                                }
                            });
                            $('#wijgrid').jqGrid('navGrid', '#gridpager1',
                                {
                                    edit: false,
                                    add: false,
                                    del: false,
                                    search: false,
                                    edittext: "Delete"
                                }
                                    );
                            $('#wijgrid').setGridParam({
                                onPaging: function() {
                           
                                    PageFlag = true;
                                }
                            });
                            jQuery("#wijgrid").jqGrid('filterToolbar',{
                                searchOperators : true, 
                                afterSearch: function () { 
                        
                                    var mydata = $('#wijgrid').jqGrid('getGridParam','data');
                                    //var stringarray = JSON.stringify(mydata);
                                
                                    //$("#MainContent_TableData_Hidden").val(stringarray);
                                    var DataNo = $("#gs_DataNo").val();
                                    var InspectionId = $("#gs_InspectionId").val();
                                    if (DataNo == null) { DataNo = "NONE" } 
                                    if (InspectionId == null || InspectionId.length == 0) {InspectionId = -1 }
                                    $("#MainContent_InspectionId_Hidden").val(InspectionId);
                                    $("#MainContent_DataNo_Hidden").val(DataNo);
                               
                                }
                            }).trigger('reloadGrid');
                        }
                    },
             error: function (a, b, c) {
                 alert(c);

             }
         });
        },
        FilterJsonTable: function ( s, l) { 
            var returnarray = []; 
            console.log(l);

            $.each( l, function( index, value ){
                if (value.Location == s) { 
                    returnarray.push(value); 
                }
            });
            return returnarray;
        },
        RenderSpecGridData: function () { 
             
            $("#Specgrid").jqGrid({
                datatype: "json",
                url:     "<%=Session("BaseUri")%>" + '/handlers/Presentation/SpecMaster_Load.ashx',
                colNames: ['SpecId', 'id', 'Location', 'InspectionJobSummaryId', 'DefectId', 'JobNumber', 'DataNo', 'ItemNumber', 'ProductType', 'POM_Row', 'Spec_Description', 'Lower_Spec_Value', 'Upper_Spec_Value', 'InspectionId', 'value', 'MeasureValue', 'OffSpec',  'Timestamp', 'Inspection_Started'],
                colModel: [
                        { name: 'SpecId', index: 'SpecId', hidden: true, editable: false },
                        { name: 'id', index: 'id', hidden: false, editable: false, search: false },
                        { name: 'Location', index: 'Location', hidden: false, editable: false, search: false },
                        { name: 'InspectionJobSummaryId', index: 'InspectionJobSummaryId', sortable: false, width: 165, formatter: gridhandler.formatSpecGrid },
                        { name: 'DefectId', index: 'DefectId', sortable: false, width: 90, formatter: gridhandler.formatSpecGrid },
                        { name: 'JobNumber', index: 'JobNumber', sortable: false, width: 155, formatter: gridhandler.formatSpecGrid },
                        { name: 'DataNo', index: 'DataNo', sortable: false, width: 155, formatter: gridhandler.formatSpecGrid },
                        { name: 'ItemNumber', index: 'ItemNumber', sortable: false, width: 90, formatter: gridhandler.formatSpecGrid },
                        { name: 'ProductType', index: 'ProductType', sortable: false, width: 165, formatter: gridhandler.formatSpecGrid },
                        { name: 'POM_Row', index: 'POM_Row', sortable: false, width: 90, formatter: gridhandler.formatSpecGrid },
                        { name: 'Spec_Description', index: 'Spec_Description', width: 145, formatter: gridhandler.formatSpecGrid  },
                        { name: 'Lower_Spec_Value', index: 'Lower_Spec_Value', width: 145, search: false, formatter: gridhandler.formatSpecGrid  },
                        { name: 'Upper_Spec_Value', index: 'Upper_Spec_Value', width: 145, search: false, formatter: gridhandler.formatSpecGrid  },
                        { name: 'InspectionId', index: 'InspectionId', formatter: gridhandler.formatSpecGrid },
                        { name: 'value', index: 'value', width: 120, formatter: gridhandler.formatSpecGrid },
                        { name: 'MeasureValue', index: 'MeasureValue', width: 120, searchoptins:true, sorttype:'integer', searchoptions:{sopt:['eq','ne','le','lt','gt','ge']}, formatter: gridhandler.formatSpecGrid },
                        { name: 'SpecDelta', index: 'SpecDelta', width: 120, searchoptins:true, sorttype:'integer', searchoptions:{sopt:['eq','ne','le','lt','gt','ge']}, formatter: gridhandler.formatSpecGrid },
                        { name: 'Timestamp', index: 'Timestamp', width: 185, sorttype: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}, formatter: 'date' },
                        { name: 'Inspection_Started', index: 'Inspection_Started', width: 185, sorttype: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'}, formatter: 'date' }
                ],
                pager: '#gridpager2',
                caption: "Product Specs & Measurements",
                autowidth: false,
                shrinkToFit: false, 
                width: Number($('#tabs').width() - 20),
                multiselect: false,
                loadonce: true,
                gridview: true,
                scroll: true,
                viewrecords: true,
                postData: { 
                    CID: function () { 
                        return selectedId;
                    }, 
                    fromdate: function () { 
                        return $("#TxtDateFrom").val();
                    }, 
                    todate: function () { 
                        return $("#TxtDateTo").val();
                    }
                },
                height: "390",
                gridComplete: function () {
                    //gridhandler.setSearchSelect("InspectionJobSummaryId", "Specgrid") 

                    //gridhandler.setSearchSelect("Spec_Description", "Specgrid")
                }
            });
            jQuery("#Specgrid").jqGrid('filterToolbar',{
                searchOperators : true, 
                afterSearch: function () { 
                    //var mydata = $("#Specgrid").jqGrid('getGridParam','data');
                    //var stringarray = JSON.stringify(mydata);
                                
                    //$("#MainContent_SpecgridData_Hidden").val(stringarray);
                    //var DataNo = $("#gs_DataNo").val();
                    var InspectionId = $("#gs_InspectionId").val();
                    if (InspectionId == null || InspectionId.length == 0) {InspectionId = -1 }
                    $("#MainContent_SpecgridInspectionId_Hidden").val(InspectionId);
                    
                               
                }
            }).trigger('reloadGrid');
        },
        RenderSummaryGrid: function () {
            $("#ijsgrid").jqGrid({
                datatype: "json",
                url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/JobSumary_Load.ashx',
                //url: 'http://apr.standardtextile.com/APRDashboard/handlers/InspectionSummary_Load.ashx',
                colNames: ['id', 'JobType', 'JobNumber', 'UnitDesc', 'Location', 'Pass/Fail', 'Started' , 'Finished', 'DHU', 'RejectionRate', 'UnitCost', 'UpdatedInspectionStarted'],
                colModel: [
                        { name: 'id', index: 'id', hidden: false, editable: false, formatter: charthandler.formatijsGrid, width: 56 },
                        { name: 'JobType', index: 'JobType', hidden: true },
                        { name: 'JobNumber', index: 'JobNumber', editable: false, formatter: charthandler.formatijsGrid },
                        { name: 'UnitDesc', index: 'UnitDesc', width: 255, editable: false, formatter: charthandler.formatijsGrid },
                        { name: 'CID', index: 'CID', hidden: true, width: 85, formatter: charthandler.formatijsGrid },
                        { name: 'Technical_PassFail', index: 'Technical_PassFail',  editable: false, formatter: charthandler.formatijsGrid },
                        { name: 'STARTED', index: 'STARTED', editable: false, formatter: charthandler.formatijsGrid },
                        { name: 'FINISHED', index: 'FINISHED', editable: false, formatter: charthandler.formatijsGrid },
                        { name: 'DHU', index: 'DHU', editable: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' }},
                        { name: 'RejectionRate', index: 'RejectionRate', editable: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' }},
                        { name: 'UnitCost', index: 'UnitCost', editable: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' }},
                        { name: 'UpdatedInspectionStarted', index: 'UpdatedInspectionStarted', hidden: true, editable: false }
                ],
                autowidth: true,
                pager: '#ijsgridpager1',
                caption: "JobSummary Manager",
                shrinkToFit: true, 
                width: Number($('#tabs').width() - 20),
                height: "390",
                multiselect: false,
                viewrecords: true,
                gridview: true,
                loadonce: true,
                datainit: function () {

                },
                postData: {
                    CID: function () { 
                        return selectedId;
                    }, 
                    fromdate: function () { 
                        return $("#TxtDateFrom").val();
                    }, 
                    todate: function () { 
                        return $("#TxtDateTo").val();
                    }
                },
                gridComplete: function () { 
                   

                }

            });
             jQuery("#ijsgrid").jqGrid('navGrid','#ijsgridpager1',{edit:false,add:false,del:false});
            $('#ijsgrid').jqGrid('navGrid', '#gridpager',
            {
                edit: false,
                add: false,
                del: false,
                search: false
            }
                );
            jQuery("#ijsgrid").jqGrid('filterToolbar',{
                searchOperators : true, 
                afterSearch: function () { 
                    var mydata = $("#ijsgrid").jqGrid('getGridParam','data');
                    var stringarray = JSON.stringify(mydata);
                                
                    
                               
                }
            }).trigger('reloadGrid');
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
        GetUniqueNames: function(columnName, tableName) { 
 
            var texts = $('#' + tableName).jqGrid('getCol',columnName), uniqueTexts = [],
            textsLength = texts.length, text, textsMap = {}, i;
            var mydata = $('#' + tableName).jqGrid('getGridParam','data');
            var mydata1 = $('#wijgrid').jqGrid("getGridParam", "data");//,
            var DataNoList = [];
            console.log(mydata1);
            $.each(mydata1, function(k,v) { 
                $.each(v, function(k2,v2) { 
                    if (k2 == columnName) { 
                        DataNoList.push(v2);
                    }
                
                });
                
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
            //var values= ["All"];
            $.each (uniqueNames, function() {
                values += ";" + this + ":" + this;
                //values.push(this);
            });
            return values;
        },
        setSearchSelect: function(columnName, tableid) {

            $('#wijgrid').jqGrid('setColProp', columnName,
                        {
                            stype: 'select',
                            searchoptions: {
                                value:gridhandler.buildSearchSelect(gridhandler.GetUniqueNames(columnName)),
                                sopt:['eq']
                            }
                        }
            );
        }, 
        GetMonthSumFiltered: function () { 
            var DataNo = $("#gs_DataNo").val();
            var InspectionId = $("#gs_InspectionId").val();
            if (DataNo == null) { DataNo = "NONE" } 
            if (InspectionId == null || InspectionId.length == 0) {InspectionId = -1 }

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'POST',
                data: { method: 'GetMonthlySumChart', args: {DataNo: DataNo, InspectionId: InspectionId, fromdatestring: fromdatestring, todatestring: todatestring  } },
                success: function (data) {
                    var json = $.parseJSON(data);
                
                    var matrix = [23];
                    if (loadcount > 1) {
                       
                        var margin = { top: 40, right: 40, bottom: 40, left: 40 },
                              width = 700,
                              height = 275;
                     
                        var data = [4, 8, 15, 16, 23, 42];
                        charthandler.Create(json);
                        
                    }
                    
                },
                error: function (a, b, c) {
                    alert(c);
                }
            });
        },
        GetDefectPieChart: function () { 
            var DataNo = $("#gs_DataNo").val();
            var InspectionId = $("#gs_InspectionId").val();

            if (DataNo == null) { DataNo = "NONE" } 
            if (InspectionId == null || InspectionId.length == 0) {InspectionId = -1 }

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'POST',
                data: { method: 'GetDefectPieChart', args: {DataNo: DataNo, InspectionId: InspectionId, fromdatestring: fromdatestring, todatestring: todatestring, TemplateId: selectedId  } },
                success: function (data) {
                    var json = $.parseJSON(data);

                    if (loadcount > 1) {
                        
                        charthandler.CreatePie(json);
                    }
                },
                error: function (a, b, c) {
                    alert(c);
                }
            });
        },
        GetDefectDescPieChart: function () { 
            var DataNo = $("#gs_DataNo").val();
            var InspectionId = $("#gs_InspectionId").val();
            if (DataNo == null) { DataNo = "NONE" } 
            if (InspectionId == null || InspectionId.length == 0) {InspectionId = -1 }

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'POST',
                data: { method: 'GetDefectDescPieChart', args: {DataNo: DataNo, InspectionId: InspectionId, fromdatestring: fromdatestring, todatestring: todatestring, TemplateId: selectedId  } },
                success: function (data) {
                    var json = $.parseJSON(data);

                    if (loadcount > 1) {
                        charthandler.CreateDefectDescPie(json);
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
                data: { method: 'GetStackedDefectLineType', args: {fromdate: fromdatestring, todate: todatestring, cid: selectedId} },
                success: function (data) {
                    var json = $.parseJSON(data);

                    drawChart2(json);
                },
                error: function (a, b, c) {
                    alert(c);
                }
            });


        }, 
        GetScatterPlot: function () { 

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                type: 'GET',
                data: { method: 'GetScatterPlotData', args: {fromdate: fromdatestring, todate: todatestring, cid: selectedId} },
                success: function (data) {
                    if (data) { 
                        var dataarr = data.split('%%%');
                        if (dataarr.length == 2) {
                            
                            var parseArr = $.parseJSON(dataarr[0]);
                            var titleArr = $.parseJSON(dataarr[1]);

                            drawChart22(parseArr, titleArr);
                        }
                    }
                },
                error: function (a, b, c) {
                    alert(c);
                }
            });


        },
        GetDefectImages: function () { 

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/Presentation/SPC_InspectionVisualizer.ashx',
                type: 'GET',
                data: { method: 'GetImageArray', args: {Locationid: selectedId, fromdate: fromdatestring, todate: todatestring, SessionUri: "<%=Session("BaseUri")%>" } },
                success: function (data) {
                    if (data) { 

                        var parseArr = $.parseJSON(data);
                             
                        console.log(parseArr);
                        $("#wijcarousel0").wijcarousel({ 
                            display: 1, 
                            step: 1, 
                            showTimer: false, 
                            data: parseArr,
                            imageWidth: 700,
                            imageHeight: 500
                        }); 
                        $(".wijmo-wijcarousel-image").css("width","500px");
                    }
                },
                error: function (a, b, c) {
                    alert(c);
                }
            });


        }
    };
    var charthandler = { 
        Draw: function (data) { 
            var scale = d3.scale.linear()
                .domain([0, 50])
                .range([0, 100]);

            var margin = { top: 40, right: 40, bottom: 40, left: 40 },
               width = 700,
               height = 275;

            var x = d3.time.scale()
               .domain([new Date(data[0].Listdate), d3.time.month.offset(new Date(data[data.length - 1].Listdate), 1)])
               .rangeRound([0, width - margin.left - margin.right]);

            var y = d3.scale.linear()
                .domain([0, d3.max(data, function (d) { return d.Count; })])
                .range([height - margin.top - margin.bottom, 0]);

            

            var bars = d3.select("#linegraph").selectAll(".bar")
                .data(data);
     
            // enter selection
            //bars
            //    .enter().append("div");

            // update selection
            //bars
            //    .style("width", function (d) {return scale(d) + "%";})
            //    .text(function (d) {return d;});v
            bars
                .exit().remove();
            bars
                .enter().append("rect")
                .attr('x', function (d) { return x(new Date(d.Listdate)); })
                .attr('y', function (d) { return height - margin.top - margin.bottom - (height - margin.top - margin.bottom - y(d.Count)) })
                .attr('width', 10)
                .attr('height', function (d) { return height - margin.top - margin.bottom - y(d.Count) });
         
            // exit selection

        },
        CreateBarChart: function (Scatdata) { 
             
            //google.load('visualization', '1.0', {'packages':['corechart']});

            //// Set a callback to run when the Google Visualization API is loaded.
            //google.setOnLoadCallback(drawChart);

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Topping');
            data.addColumn('number', 'Slices');
            data.addRows([
                ['Mushrooms', 3],
                ['Onions', 1],
                ['Olives', 1],
                ['Zucchini', 1],
                ['Pepperoni', 2]
            ]);

            // Set chart options
            var options = {'title':'How Much Pizza I Ate Last Night',
                'width':400,
                'height':300};

            // Instantiate and draw our chart, passing in some options.
            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);
            

        },
        CreateExample: function (HistData) { 
            var borderwidth = $('body').width();
             
            var vis = d3.select('#visualisation'),
              WIDTH = borderwidth * .67,
              HEIGHT = 500,
              MARGINS = {
                  top: 60,
                  right: 20,
                  bottom: 60,
                  left: 50
              },
              xRange = d3.scale.ordinal().rangeRoundBands([MARGINS.left, WIDTH - MARGINS.right], 0.1).domain(HistData.map(function (d) {
                  return d.x;
              })),
              yRange = d3.scale.linear().range([HEIGHT - MARGINS.top, MARGINS.bottom]).domain([0,
                d3.max(HistData, function (d) {
                    return d.y;
                })
              ]),

              xAxis = d3.svg.axis()
                .scale(xRange)
                .tickSize(10)
                .tickSubdivide(true),

              yAxis = d3.svg.axis()
                .scale(yRange)
                .tickSize(5)
                .orient("left")
                .tickSubdivide(true);
            vis.append('svg:g')
              .attr('class', 'x axis')
              .attr('transform', 'translate(0,' + (HEIGHT - MARGINS.bottom) + ')')
              .call(xAxis)
                .selectAll("text")  
                .style("text-anchor", "end")
                .style("font-weight","bolder")
                .style("font-size","xx-small")
                .attr("x", -20)
                .attr("dx", "-.4em")
                .attr("dy", ".15em")
                .attr("transform", function(d) {
                    return "rotate(-50)" 
                });
            vis.append('svg:g')
              .attr('class', 'y axis')
              .attr('transform', 'translate(' + (MARGINS.left) + ',0)')
              .call(yAxis)
              .append("text")
                .attr("transform", "rotate(-90)")
                .attr("y", -42)
                .attr("x", -37)
                .attr("dy", "1.71em")
                .style("text-anchor", "end")
                .text("Defect Count");

            vis.selectAll('rect')
              .data(HistData)
              .enter()
              .append('rect')
              .attr('x', function (d) {
                  return xRange(d.x);
              })
              .attr('y', function (d) {
                  return yRange(d.y);
              })
              .attr('width', xRange.rangeBand())
              .attr('height', function (d) {
                  return ((HEIGHT - MARGINS.bottom) - yRange(d.y));
              })
              .attr('fill', 'grey');
        },
        CreatePie: function (PieCartTable) { 

            var piecartheight = 300;
            var ChartWidth = 325;
            var Axcenter = 200;
            var Bxcenter = 120;
            var rxradius = 110;
            var ryradius = 90;
            var hheight = 30;

            var fontsize1 = "medium";
            if (PieCartTable.length > 5) { 
                fontsize1 = "smaller";
            }

            var offsets = document.getElementById('EmployeeChart').getBoundingClientRect();
            var top = offsets.top;
            var left = offsets.left;
            
            for (i = 0; i < PieCartTable.length; i++) {
                var toppx = i * ((piecartheight - 50) / PieCartTable.length) + 10;
             
                $('#PieLegend').append('<span style="position:absolute; left: 101%; top: ' + toppx.toString() + 'px; width: 15px; height:15px; background-color: ' + PieCartTable[i].color + ';box-shadow: 1px 3px 4px rgb(70, 68, 68);" ></span>');
                $('#PieLegend').append('<label style="position:absolute; left: 104%; font-size: ' + fontsize1.toString() + ';  top: ' + toppx.toString() + 'px;  font-size:medium; z-index:100; color:black; width: 150px;font-family: sans-serif;" >' + PieCartTable[i].label + '</label>');
            }
            
            var svg = d3.select("#EmployeeChart").append("svg").attr("width", ChartWidth).attr("height", piecartheight);

            svg.append("g").attr("id","EmployeeNo");

            Donut3D.draw("EmployeeNo", PieCartTable, Bxcenter, 150, rxradius, 70, 30, 0);

            //setTimeout(function(){ $('#loading').toggle(); }, 1000);

        },
        CreateDefectDescPie: function (PieCartTable) { 

            var piecartheight = 300;
            var ChartWidth = 325;
            var Axcenter = 200;
            var Bxcenter = 460;
            var rxradius = 110;
            var ryradius = 90;
            var hheight = 30;

            var fontsize1 = "medium";
            if (PieCartTable.length > 5) { 
                fontsize1 = "smaller";
            }

            for (i = 0; i < PieCartTable.length; i++) {
                var toppx = i * ((piecartheight - 50) / PieCartTable.length) + 10;
             
                $('#PieLegend').append('<span style="position:absolute; left:5px; top: ' + toppx.toString() + 'px; width: 15px; height:15px; background-color: ' +  PieCartTable[i].color + ';box-shadow: 1px 3px 4px rgb(70, 68, 68);" ></span>');
                $('#PieLegend').append('<label style="position:absolute; width:120px; left: 35px; top: ' + toppx.toString() + 'px;   font-size: ' + fontsize1.toString() + '; z-index:100; color:black;font-family: sans-serif;" >' + PieCartTable[i].label + '</label>');
            }
            
            var svg = d3.select("#PieChart").append("svg").attr("width", ChartWidth).attr("height", piecartheight);

            svg.append("g").attr("id","DefectDesc");

            Donut3D.draw("DefectDesc", PieCartTable, Axcenter, 150, rxradius, 70, 30, 0.4);
            
            //setTimeout(function(){ $('#loading').toggle(); }, 1000);

        }
    };
    function drawChart () {

        var data = new google.visualization.DataTable();
        data.addColumn('number', 'Student ID');
        data.addColumn('number', 'Dan');
        data.addColumn('number', 'John');

        data.addRows([
          [0, 0, 1],  [1, 1, 8],   [2, 2, 7],
          [3, 3, 3],  [4, 4, 8],   [5, 5, 9],
          [6, 6, 9],  [7, 7, 7],   [8, 8, 9],
          [9, 9, 4],  [10, 10, 8], [11, 0, 7],
          [12, 5, 6], [13, 3, 9],  [14, 1, 7],
          [15, 5, 1], [16, 6, 6],  [17, 7, 9],
          [18, 3, 8], [19, 9, 9],  [20, 2, 7],
          [21, 2, 9], [22, 2, 8],  [23, 3, 6],
          [24, 4, 6], [25, 2, 8],  [26, 6, 9],
          [27, 2, 8], [28, 8, 7],  [29, 9, 8]
        ]);

        var options = {
            chart: {
                title: 'Students\' Final Grades',
                subtitle: 'based on hours studied'
            },
            width: $(window).width() * .47,
            height: 500,
            axes: {
                y: {
                    'hours studied': {label: 'Hours Studied'}
                }
            }
        };

        var chart = new google.charts.Scatter(document.getElementById('scatter_dual_y'));

        chart.draw(data, options);

    }
    function drawChart22 (jsondata, titledata) {
        console.log('drawchart22');
        var newarray = [];
        var fieldlength = titledata.length; 
        
        // Create the data table.
        $.each(jsondata, function(index, value) {  
            switch (fieldlength) { 
                case 1: 
                    newarray.push([value.DATEVAL, value.DHU_1])
                    break;
                case 2: 
                    newarray.push([value.DATEVAL, value.DHU_1, value.DHU_2])
                    break;
                case 3: 
                    newarray.push([value.DATEVAL, value.DHU_1, value.DHU_2, value.DHU_3])
                    break;
                case 4: 
                    newarray.push([value.DATEVAL, value.DHU_1, value.DHU_2, value.DHU_3, value.DHU_4])
                    break;
            }
        });   

        var data = new google.visualization.DataTable();
        data.addColumn('string', 'DATEVAL');
        $.each(titledata, function(index,value) { 

            data.addColumn('number', value.Object1);
        }); 

        data.addRows(newarray);

        var options = {
            title: 'DHU BY DAY AND TEMPLATE',
            subtitle: '(Includes All Types)',
            titleTextStyle: {
                color:'black',
                fontName: 'Arial', 
                fontSize: 13, 
                bold: true
            }, 
            titlePosition: 'in',
            width: $(window).width() * .44,
            height: 550,
            colors: ['#6496c8','#B0B579','#FBB040', '#D31245'], 
            backgroundColor: 'transparent',
            titlePosition: 'out'
        };


        //var chart = new google.charts.Scatter(document.getElementById('scatter_dual_y'));
        var chart = new google.visualization.ScatterChart(document.getElementById('scatter_dual_y'));
        chart.draw(data, options);
        //chart.draw(data, google.charts.Scatter.convertOptions(options));
        $('#loading').toggle();
    }
    function drawChart2(jsondata) {
        var newarray = [];
        // Create the data table.
        $.each(jsondata, function(index, value) {  
            newarray.push([value.DefectDesc, value.IL, value.EOL, value.ROLL])
        });   

        var data = new google.visualization.DataTable();
        data.addColumn('string', 'DefectDesc');
        data.addColumn('number', 'IL');
        data.addColumn('number', 'EOL');
        data.addColumn('number', 'ROLL');
        data.addRows(newarray);

        var options = {
            width: $(window).width() * .42,
            height: 550, 
            bar: { groupWidth: '75%' },
            isStacked: true,
            colors: ['#6496c8','#B0B579','#FBB040'], 
            titlePosition: 'in',
            legend: { position: 'top'},
        };

        // Instantiate and draw our chart, passing in some options.
        var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
        chart.draw(data, options);
        
        $("#chart_label").toggle(); 
    }

    function FlagExport () { 
        $("#ExportFlag_Hidden").val("1");
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

<script type="text/javascript">
    
    var DefectByType;
    var salesData=[
	{label:"Basic", color:"#3366CC"},
	{label:"Plus", color:"#DC3912"},
	{label:"Lite", color:"#FF9900"},
	{label:"Elite", color:"#109618"},
	{label:"Delux", color:"#990099"}
    ];

</script>

</asp:Content>

