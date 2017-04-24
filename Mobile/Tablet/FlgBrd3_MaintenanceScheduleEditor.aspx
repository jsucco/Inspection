<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Mobile.master" AutoEventWireup="false" CodeFile="FlgBrd3_MaintenanceScheduleEditor.aspx.vb" Inherits="core.APP_Presentation_FlgBrd2_MaintenanceScheduleEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="hidden">
            <input type="hidden" value ="False" id="Authenticated_hidden" runat="server" />
           </div> 
                <div id="loginfrm" style="Z-INDEX: 110; height: 300px; width: 425px; position: absolute; top:25px; left:38%; display:none" >
				    <div id="LocationSelection">
                    <ASP:LABEL id="lblUserID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 30px"
					runat="server" font-bold="True" Font-Size="Large">CORPORATE NAME:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: relative; TOP: 30px">
                    <div style="Z-INDEX: 102; LEFT: 100px; POSITION: relative; TOP: -5px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
                        <select id="selectNames" name="CorporateName" style="width: 162px; height: 35px; " class="inputelement"></select>
               
                                </div>
                            </div>
                        </div>
                    <div id="FlagBoardSelection">
                    <ASP:LABEL id="lblfbID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 70px"
					runat="server" font-bold="True" Font-Size="Large">Flag Board:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: relative; TOP: 70px">
                    <div style="Z-INDEX: 102; LEFT: 100px; POSITION: relative; TOP: -5px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
                        <select id="FlagBoardNames" name="FlagBoardName" style="width: 162px; height: 35px; " class="inputelement"></select>
               
                                </div>
                            </div>
                        </div>
                    &nbsp;&nbsp;
                  </div>
    <input id="screendata" type="hidden" value="data:" name="screendata" runat="server" />
<div style="position:relative;top: 0px;" class="tabletback">
    <ASP:TEXTBOX id="PageTitle" style="Z-INDEX: 101; LEFT: 30px; font-size:medium; font-family:Times New Roman; color:#6E90A6; font-weight:bold; border:0px; background:transparent; POSITION: absolute; TOP: 53px"
			tabIndex="18" runat="server" width="220px" height="50px"></ASP:TEXTBOX>
    
    

<div id="ButtonHolder1" class="ButtonHolder" style="position:relative;top: 100px;">
    <input id="BtnUp1" type="button" style="left: 5px; top: 0px; position: absolute; height: 30px; width:80px; margin-top: .2%; margin-bottom: auto; background:rgba(236, 173, 58, 0.819608)" class="scaledbutton" value="UnScheduled" />
    <input id="BtnUp2" type="button" style="left: 110px; top: 0px; position: absolute; height: 30px; width:80px; margin-top: .2%; background:rgb(110, 149, 215)" class=scaledbutton value="Scheduled" />
    <ASP:TEXTBOX id="ShiftDate" style="Z-INDEX: 104; LEFT: 195px; font-size:small; font-weight:bold; border:0px; background:transparent; POSITION: absolute; TOP: 15px"
			tabIndex="18" runat="server" width="160px" height="30px"></ASP:TEXTBOX>
    <ASP:TEXTBOX id="ShiftInfo" style="Z-INDEX: 104; LEFT: 195px; font-size:small; font-weight:bold; border:0px; background:transparent; POSITION: absolute; TOP: 40px"
			tabIndex="18" runat="server" width="165px" height="30px"></ASP:TEXTBOX>
</div>
<div id="tablehold" style="position:relative; Z-INDEX: 104; top: 110px; margin:auto;" >
    <table id="wijgrid" style=" font-size:medium; Z-INDEX: 104; font-weight:800; ">
        
        </table>
    <div id="gridpager" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
    <div style="position:relative; top: 20px; ">
    <input id="Button1" type="button" style="left: 10px; top: 20px; position:relative; height: 30px; width:80px" class=scaledbutton value="Edit" />
    <input id="Button2" type="button" style="left: 110px; top: 20px; position:absolute; height: 30px; width:80px" class=scaledbutton value="New" />
    
</div>
</div>

<div id="ButtonHolder2" class="ButtonHolder" style="position:relative; height:40px; top: 180px;">
    <input id="FBChange" type="button" style="float:left; top: 5px; position: relative; height: 25px; width:120px;" class=change value="Change fb" />
<%--    <input id="Button1" type="button" style="left: 10px; top: 20px; position:relative; height: 40px; width:110px" class=export value="Edit" />
    <input id="Button2" type="button" style="left: 150px; top: 20px; position:absolute; height: 40px; width:110px" class=export value="New" />--%>
</div>




 </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
<script src="../../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <!--Theme-->
<link href="http://cdn.wijmo.com/themes/arctic/jquery-wijmo.css" rel="stylesheet" type="text/css" />
<link href="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.css" rel="stylesheet" type="text/css" />
    <style>
        .grid .ui-jqgrid-htable th,
        .grid .ui-jqgrid-btable .jqgrow td {
            height: 5em !important;
        }
                #loginfrm {
	        BORDER-RIGHT: 0px; BORDER-TOP: #95A0A9 1px solid; BACKGROUND: #87A7AA; LEFT: expression(document.body.clientWidth / 2 - this.offsetWidth / 2); BORDER-LEFT: 0px; WIDTH: 100%px; BORDER-BOTTOM: #95A0A9 1px solid; POSITION: absolute; TOP: 150px; HEIGHT: 326px;
             box-shadow: 5px 15px 15px rgba(0,0,0,.7);
             -webkit-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
             -moz-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
             border-radius: 4px;
        }
        div.hidden {
        display: none;
        position: fixed;
        z-index:106;
    }
    </style>
<link href="../../Styles/ui.jqgrid.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
<script src="../../Scripts/grid.locale-en.js" type="text/javascript"></script>
    <script type="text/javascript">
       
        var MFBID = new Number("<%= MFBID %>");
        var BlankEMPID = new Number("<%= BlankEMPID %>");
        var IsTablet = new Boolean();
        var IsPC = new Boolean();
        var BaseURI = new String(); 
        var filterholder = false;
        var MSID = 1;
        var SchId = new String();
        var BoardData;
        var MFBID;
        var HasCID;
        var HasFB;
        var selectedCID;
        var BoardType = '<%=BoardType%>';
        var BoardNumber = '<%=BoardNumber%>';
        var TableJson;
        $(document).ready(function () {

            schedhold = '2';
            var BtnUp2 = document.getElementById('BtnUp2');
            var BtnUp1 = document.getElementById('BtnUp1');
            BtnUp1.style.backgroundColor = "#A9C6D7";

            if (MFBID == -1) { 
                document.getElementById('<%=PageTitle.ClientID %>').value = "ALL Maintenance Entries";
            } else {
                if (BoardType == 'WO') { 
                    document.getElementById('<%=PageTitle.ClientID %>').value = "Warp Out Board " + BoardNumber;
                    } else { 
                        document.getElementById('<%=PageTitle.ClientID %>').value = "Maintenance Flagboard "  + BoardNumber;
                    }
                }
            //IsPC = $.parseJSON("<%=Session("IsPC")%>".toLowerCase());
            //IsTablet = $.parseJSON("<%=Session("IsTablet")%>".toLowerCase());
            BaseURI = "<%=Session("BaseUri")%>"
            SchId = "<%=SchId%>"
            BoardData = <%=BoardData%>
            HasCID = '<%=HasCID%>';
            selectedCID = '<%=CID%>';
            HasFB = '<%=HasFB%>';
            var CorpSelList = <%=CorpSelList%>;
            var FBSelList = <%=FBSelList%>;
            console.log(SchId);
            if (SchId == '2' || SchId == '1') {
                //callgridhandler(SchId);
                gridhandler.CallData(SchId);
                if (SchId == '1') { 
                    $('#BtnUp2').css("background", "rgba(236, 173, 58, 0.82)")
                    $('#BtnUp1').css("background", "rgb(110, 149, 215)")
                } else { 
                    $('#BtnUp1').css("background", "rgba(236, 173, 58, 0.82)")
                    $('#BtnUp2').css("background", "rgb(110, 149, 215)")
                }
            }
            else {
                //callgridhandler('2');
                gridhandler.CallData('2');
                $('#BtnUp1').css("background", "rgba(236, 173, 58, 0.82)")
                $('#BtnUp2').css("background", "rgb(110, 149, 215)")
            }

            var hiddenSection = $('div.hidden');
            if (HasCID == 'False') { 
                var html = [];
                var name;
                $('#FlagBoardSelection').css('display','none');
                if (CorpSelList[0] != 0) { 
                    for(var i = 0; i < CorpSelList.length; i++){
                        name = CorpSelList[i];
                        html.push('<option value="'+name.id+'">'+name.text+'</option>');
                    }
                }

                $("#selectNames").html(html.join('')).bind("change dblclick", function(){

                    var selectedid = $(this).val();
                    if (selectedid) { 
                        console.log("<%=Session("BaseUri")%>");
                        window.location.assign("<%=Session("BaseUri")%>" + '/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx?CID_Info=000' + selectedid)
                }
            });
            $("#selectNames").val(selectedCID);
            $("#loginfrm").fadeIn();
            hiddenSection.fadeIn()
                .css({ 'display':'block' })
                // set to full screen
                .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
                .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
                    left:($(window).width() - hiddenSection.width())/2 + 'px' })
                // greyed out background
                .css({ 'background-color': 'rgba(0,0,0,0.5)' });
        } else { 
            if (HasFB == 'True') { 
                $(hiddenSection).fadeOut();
                $("#loginfrm").css('display', 'none');

                //gridhandler.CallData('2');
            } else { 
                $('#LocationSelection').css('display','none');

                var hiddenSection = $('div.hidden');
                if (HasFB == 'False') { 
                    var html = [];
                    var name;
              
                    if (FBSelList[0] != 0) { 
                        for(var i = 0; i < FBSelList.length; i++){
                            name = FBSelList[i];
                            html.push('<option value="'+name.id+'">'+name.text+'</option>');
                        }
                    }

                    $("#FlagBoardNames").html(html.join('')).bind("change dblclick", function(){

                        var selectedid = $(this).val();
                        if (selectedid) { 
                            var selectedtext = $('#FlagBoardNames option:selected').text(); 
                            window.location.assign("<%=Session("BaseUri")%>" + '/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx?MFBID=' + selectedid + '&MFBNAME=' + selectedtext)
                        }
                    });
                    $("#FlagBoardNames").val(MFBID);
                    $("#loginfrm").fadeIn();
                    hiddenSection.fadeIn()
                        .css({ 'display':'block' })
                        // set to full screen
                        .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
                        .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
                            left:($(window).width() - hiddenSection.width())/2 + 'px' })
                        // greyed out background
                        .css({ 'background-color': 'rgba(0,0,0,0.5)' });
                }
            }
        }
            $('#FBChange').on('click', function (e) {
                $("#loginfrm").fadeIn();
                HasFB = 'False';
                var html = [];
                var name;
                console.log('click');
                if (FBSelList[0] != 0) { 
                    for(var i = 0; i < FBSelList.length; i++){
                        name = FBSelList[i];
                        html.push('<option value="'+name.id+'">'+name.text+'</option>');
                    }
                }

            $("#FlagBoardNames").html(html.join('')).bind("change dblclick", function(){

                var selectedid = $(this).val();
                if (selectedid) { 
                    var selectedtext = $('#FlagBoardNames option:selected').text();
                    window.location.assign("<%=Session("BaseUri")%>" + '/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx?MFBID=' + selectedid + '&MFBNAME=' + selectedtext)
                }
            });
            $("#FlagBoardNames").val(MFBID);
            $('#FlagBoardSelection').css('display','block');
            $('#LocationSelection').css('display','none');
            hiddenSection.fadeIn()
                .css({ 'display':'block' })
                // set to full screen
                .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
                .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
                    left:($(window).width() - hiddenSection.width())/2 + 'px' })
                // greyed out background
                .css({ 'background-color': 'rgba(0,0,0,0.5)' });
            });




            var pageheight = screen.height - 42;
            var pagewidth = screen.width;
            $('.tabletback').css({"height": pageheight.toString() + "px"});
            $('.mobilebody').css({ "height": pageheight.toString() + "px"});
            
            var now = new Date();

            var night = new Date(
                now.getFullYear(),
                now.getMonth(),
                now.getDate() + 1, // the next day, ...
                0, 0, 0 // ...at 00:00:00 hours
            );

            var msTillMidnight = night.getTime() - now.getTime();

            setTimeout(function () { location.reload(); }, msTillMidnight);
        });
        window.addEventListener("orientationchange", function () {
            // Announce the new orientation number
            var pagewidth = screen.width - 5;

            $("#wijgrid").jqGrid('setGridWidth', pagewidth, true)
        }, false);

        $('#Button1').click(function (e) {
            e.preventDefault();

            var gridselrow = $("#wijgrid").jqGrid('getGridParam', 'selrow');
            if (gridselrow) {
                var rows = jQuery("#wijgrid").jqGrid('getRowData');

                if (rows[gridselrow - 1].MS_Id > 0) {
                    var querystring = 'MSID=' + rows[gridselrow - 1].MS_Id.toString() + '&Method=update&SchId=' + SchId

                        window.location.assign(BaseURI + '/Mobile/Tablet/MobileMaintSched_Editor.aspx?MSID=' + rows[gridselrow - 1].MS_Id.toString() + '&Method=update&SchId=' + SchId + '&CID=' + selectedCID.toString() + '&MFBID=' + MFBID.toString() + '&Type=' + BoardType + '&Name=' + BoardType + BoardNumber.toString());
                    
                }
            }
            else {
                alert('NO ROW SELECTED');
            }
      
        });
        $('#Button2').click(function (e) {
            e.preventDefault();

            window.location.assign(BaseURI + '/Mobile/Tablet/MobileMaintSched_Editor.aspx?MSID=new&Method=new&SchId=' + SchId + '&CID=' + selectedCID.toString()  + '&MFBID=' +  MFBID.toString() + '&Type=' + BoardType + '&Name=' + BoardType + BoardNumber.toString());
 
        });


        var gridhandler = {
            CallData: function (parameter1) {
                console.log(MFBID);
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/ut_flagboard_handler.ashx',
                    type: 'POST',
                    data: { method: 'GetMobileInputData', args: { Schedule: parameter1, MFB_Id: MFBID, sCID: selectedCID } },
                    success: function (data) {
                        var json;
                        var $wijgrid = $("#wijgrid")
                        if (BoardData.length > 0) {
                            json = BoardData;
                        } else {
                            json = $.parseJSON(data);
                            TableJson = json;
                        }
                        console.log(BoardData.Length);
                        var width = Number(100);
                        var pagewidth = screen.width - 50;

                        $wijgrid.jqGrid({

                            datatype: "local",
                            height: 250,
                            colNames: ['Name', 'MSID', 'BoardID', 'Reason', 'CLosed', 'Created', 'MaintType', 'WorkOrder'],
                            colModel: [
                                { name: 'MM_Name', index: 'MM_Name', width: 100, sorttype: "text", formatter: gridhandler.format_grid },
                                { name: 'MS_Id', index: 'MS_Id', sorttype: "int", width: pagewidth * .05, hidden: true },
                                { name: 'MFB_Id', index: 'MFB_Id', hidden: false, sortable: false, align: "center", width: pagewidth * .06, formatter: gridhandler.format_grid },
                                { name: 'MS_Unscheduled_Reason', index: 'MS_Unscheduled_Reason', width: pagewidth * .15, sorttype: "text", formatter: gridhandler.format_grid },
                                { name: 'MS_WOClosed_Timestamp', index: 'MS_WOClosed_Timestamp', align: "center", width: pagewidth * .17, sorttype: 'date', formatter: gridhandler.format_grid, formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'} },
                                { name: 'MS_WOCreate_Timestamp', index: 'MS_WOCreate_Timestamp', width: pagewidth * .17, align: "left", sorttype: "float", sorttype: 'date', formatter: gridhandler.format_grid, formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'} },
                                { name: 'MT_Description', index: 'MT_Description', align: "left", width: pagewidth * .17, sorttype: "text", formatter: gridhandler.format_grid },
                                { name: 'MS_WorkOrder', index: 'MS_WorkOrder', sortable: false, align: "left", width: pagewidth * .05, sorttype: "int", formatter: gridhandler.format_grid }
                            ],
                            pager: '#gridpager',
                            caption: "Maintenance FlagBoard",
                            autowidth: true,
                            multiselect: false,
                            loadonce: true,
                            gridview: true,
                            rowNum: 20,
                            viewrecords: true,
                            data: json,
                            height: "100%",
                            gridComplete: function () {
                                $("td", ".jqgrow").height(30);
                            },
                            onSelectRow: function (rowid) { 
                                var MFBIDCell = $("#" + rowid).find("td[aria-describedby='wijgrid_MFB_Id']").html();
                                if (MFBIDCell && TableJson.length > 0) { 
                                     
                                    MFBID = TableJson[rowid - 1].MFB_Id

                                }
 
                            }

                        });  
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });


            },
            format_grid: function (cellvalue, options, rowobject) {

                if (rowobject.MS_WOClosed_Timestamp) {
                    return "<span style='color: green; font-weight:bolder'>" + cellvalue + "</span>";
                } else {

                    return "<span style='color: black; font-weight: normal;'>" + cellvalue + "</span>";
                }

                
            }


        };

       
   
        $(function () {

            $("#BtnUp1").click(function () {

                window.location = BaseURI + "/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx?SchId=2&MFBID=" +  MFBID.toString()

            });

            $("#BtnUp2").click(function () {

                window.location = BaseURI + "/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx?SchId=1&MFBID=" +  MFBID.toString()

            });


        });

        

</script>


</asp:Content>



