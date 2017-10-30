<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site_2.master" AutoEventWireup="false" CodeFile="FlgBrd_STTLoomPickCount.aspx.vb" Inherits="core.APP_Presentation_FlgBrd_STTLoomPickCount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div style="position:relative; top: 0px;">
<div id="DivImage1" class = "GroovedMachinePanel" style="position: absolute; display: none; top: 50px; left: 220px;">
    <input id="MachineTitle1" class="admintext" type=text value="Dornier 2" style="font-size:300%;" />
    <input id="PicksLabel1" class="admintext" type="text" style="font-size:250%; Position: absolute; top: 50px; left: 50px;">
    <input id="StopsLabel1" class="stopslabel" type="text" style="font-size:200%; Position: absolute; top: 130px; left: 50px; width: 400px; color: Red;">
</div>
<div style="position: absolute; top: 50px; left: -10px;">
    <img id="MachineImage1" style="height: 119px; width: 225px;" src="" alt="" />
    <ASP:TEXTBOX id="UpdateDate" style="Z-INDEX: 104; font-size:100%; font-weight:bold; border:0px; background:transparent; POSITION: absolute; TOP: 120px; left: 8px;"
		tabIndex="18" runat="server" width="450px" height="30px"></ASP:TEXTBOX>
    <ASP:TEXTBOX id="ShiftDate" style="Z-INDEX: 104; display:none; LEFT: 300px; font-size:large; font-weight:bold; border:0px; background:transparent; POSITION: relative; TOP: -41px"
			tabIndex="18" runat="server" width="450px" height="30px"></ASP:TEXTBOX>
    <ASP:TEXTBOX id="ShiftInfo" style="Z-INDEX: 104; LEFT: -150px; font-size:large; font-weight:bold; display:none; border:0px; background:transparent; POSITION: relative; TOP: 0px"
			tabIndex="18" runat="server" width="450px" height="30px"></ASP:TEXTBOX>
    <asp:Button ID="Exportxl" runat="server" Text="EXPORT XLSX" style="position:absolute; top: 210px; left: 8px; width: 115px; height: 35px;" class=export/>
    <input id="exportdates" name="exportdates" type="hidden" runat="Server" />
    <div id="exportpopup" style="width: 225px; height: 275px; display: none; z-index:120; position: absolute; top: 280px; margin:none; left: 15px;border: 2px solid; border-width: medium; box-shadow: 10px 10px 5px #888888; background: white;" > 
        <div style="position:absolute; top: 3px;">
        <asp:Label ID="LblDateFrom" runat="server" CssClass="labelleft1" >Begin Date</asp:Label>
        <input type="text" id="TxtDateFrom" name="datefrom" style="float:left;" class=labelleft1 /> 
        </div>
        <div style="position:absolute; top: 63px; height: 102px;">
        <asp:Label ID="LblDateTo" runat="server" CssClass="labelleft1" >End Date</asp:Label>
        </div>
        <div style="float:left; position:absolute; top: 83px;">
        <input type="text" id="TxtDateTo" name="dateto" style="float:left;" class=labelleft1 />
        </div>
        <div id = "Chk1" style="position:absolute; top: 120px; left: 5px; padding: 0px;" class=SheetClass1>
                                        <input id="Mins" name="ChkBx1" type="checkbox" class=chkbox2 runat=server />Minutes</div>
        <div id = "Chk2" style="position:absolute; top: 140px; left: 5px; padding: 0px;" class=SheetClass1>
                                        <input id="Hrs" name="ChkBx2" type="checkbox" class=chkbox2 runat=server />Hours</div>
        <div id = "Chk3" style="position:absolute; top: 160px; left: 5px; padding: 0px;" class=SheetClass1>
                                        <input id="Dys" name="ChkBx3" type="checkbox" class=chkbox2 runat=server />Days</div>
        <div id = "Chk4" style="position:absolute; top: 180px; left: 5px; padding: 0px;" class=SheetClass1>
                                        <input id="Mnths" name="ChkBx4" type="checkbox" class=chkbox2 runat=server />Months</div>
        <div id ="GoDiv" style="position:absolute; top: 215px; left: 5px;" >

            <asp:Button ID="GoButton" runat="server" Text="GO" style="position:absolute; width: 85px; height: 30px;" class=export/>
        </div>
    </div>
</div>
<div id="tablehold1" style="position:relative; Z-INDEX: 100; top: 261px; left: 223px;" >
   <table id="StopGrid1" style="font-size: 92%; width: 447px;">
        </table>
</div>
<div id="Pickhold1" style="position:absolute; Z-INDEX: 100; top: 53px; left: 485px;" >
   <table id="PicksGrid1" style="font-size: 92%; width: 185px;">
        </table>
</div>
<div id="DivImage2" class = "GroovedMachinePanel" style="position: absolute; display: none; width: 450px; height: 280px; top: 50px; left: 700px;">
    <input id="MachineTitle2" class="admintext" type=text value="Dornier 3" style="font-size:300%;" />
    <input id="PicksLabel2" class="admintext" type="text" style="font-size:250%; Position: absolute; top: 50px; left: 50px;">
    <input id="StopsLabel2" class="stopslabel" type="text" style="font-size:200%; Position: absolute; top: 130px; left: 50px; width: 400px; color: Red;">
</div>

<div id="tablehold2" style="position:absolute; Z-INDEX: 100; top: 261px; left: 703px;" >
   <table id="StopGrid2" style="font-size: 92%; width: 447px;">
        </table>
</div>

<div id="Pickhold2" style="position:absolute; Z-INDEX: 100; top: 53px; left: 964px;" >
   <table id="PicksGrid2" style="font-size: 92%; width: 185px;">
        </table>
</div>

<div id="DivImage3" class = "GroovedMachinePanel" style="position: absolute; display: none; width: 450px; height: 280px; top: 360px; left: 220px;">
    <input id="MachineTitle3" class="admintext" type=text value="Dornier 4" style="font-size:300%;" />
    <input id="PicksLabel3" class="admintext" type="text" style="font-size:250%; Position: absolute; top: 50px; left: 50px;">
    <input id="StopsLabel3" class="stopslabel" type="text" style="font-size:200%; Position: absolute; top: 130px; left: 50px; width: 400px; color: Red;">
</div>

<div id="tablehold3" style="position:absolute; Z-INDEX: 100; top: 571px; left: 223px;" >
   <table id="StopGrid3" style="font-size: 92%; width: 447px;">
        </table>
</div>

<div id="Pickhold3" style="position:absolute; Z-INDEX: 100; top: 363px; left: 485px;" >
   <table id="PicksGrid3" style="font-size: 92%; width: 185px;">
        </table>
</div>

<div id="DivImage4" class = "GroovedMachinePanel" style="position: absolute; display: none; width: 450px; height: 280px; top: 360px; left: 700px;">
    <input id="MachineTitle4" class="admintext" type=text value="Dornier 5" style="font-size:300%;" />
    <input id="PicksLabel4" class="admintext" type="text" style="font-size:250%; Position: absolute; top: 50px; left: 50px;">
    <input id="StopsLabel4" class="stopslabel" type="text" style="font-size:200%; Position: absolute; top: 130px; left: 50px; width: 400px; color: Red;">
</div>

<div id="tablehold4" style="position:absolute; Z-INDEX: 100; top: 571px; left: 703px;" >
   <table id="StopGrid4" style="font-size: 92%; width: 447px;">
        </table>
</div>  

<div id="Pickhold4" style="position:absolute; Z-INDEX: 100; top: 363px; left: 964px;" >
   <table id="PicksGrid4" style="font-size: 92%; width: 185px;">
        </table>
</div>

</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
<script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>

<!--Wijmo Widgets CSS-->
<link href="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.css" rel="stylesheet" type="text/css" />

    <!--Theme-->
    <link href="http://cdn.wijmo.com/themes/metro/jquery-wijmo.css" rel="stylesheet" type="text/css" />

<!--Wijmo Widgets JavaScript-->
<script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20141.34.js" type="text/javascript"></script>


<script type="text/javascript">

     var ItemSelect;
     var menulevel = 0;
     var menutext = ["Machine Groups", "Machines"];
     var wijgridName = ["#StopGrid1", "#StopGrid2", "#StopGrid3", "#StopGrid4", "#PicksGrid1", "#PicksGrid2", "#PicksGrid3", "#PicksGrid4"];
     var PicksLabel = ["#PicksLabel1", "#PicksLabel2", "#PicksLabel3", "#PicksLabel4"];
     var Containers = ["#DivImage1", "#DivImage2", "#DivImage3", "#DivImage4"];
     var StopsLabel = ["#StopsLabel1", "#StopsLabel2", "#StopsLabel3", "#StopsLabel4"];
     var date = new Date();
     
//require(["wijmo.wijgrid"], function () {
     $(document).ready(function () {

         $("#MachineImage1").attr("src", "<%=Session("BaseUri")%>" +'/EQUIPMENT/MACHINE/ImageVB.aspx?ImageID=180');

         $("#DivImage1").fadeIn(1000);
         $("#DivImage2").fadeIn(1000);
         $("#DivImage3").fadeIn(1000);
         $("#DivImage4").fadeIn(1000);

         for (i = 0; i < 4; i++) {
             GridInit(i);
             PicksGridInit(i + 4);
             callPicksgridhandler(i + 2);
         }

         callgridhandler();
         callCurrentPicks()
        
         date.setDate(date.getDate() - 4);

         $("#TxtDateFrom").wijinputdate({
             dateFormat: 'd',
             date: date
         });
         date.setDate(date.getDate() + 4)
         $("#TxtDateTo").wijinputdate({
             dateFormat: 'd',
             date: date
         });
     });
   // });
                function callgridhandler() {

                    $.ajax({
                        url:  "<%=Session("BaseUri")%>" + '/handlers/APR_LoomFlag_Handler.ashx',
                        //url: 'http://coreroute_test.standardtextile.com/handlers/ut_flagboard_handler.ashx',
                        //url: 'http://coredemo.standardtextile.com/handlers/APR_LoomFlag_Handler.ashx',
                        type: 'POST',
                        data: { method: 'GetLoomStatsGrid' },
                        success: function (data) {
                            var json;
                            json = $.parseJSON(data);
//                            console.log(json.length);

                            if (json.length & 4) {
                                for (i = 0; i < json.length; i++) {
                                    AddToGrid(i, json[i]);
//                                    console.log('func');
                                }
                            }

//                            console.log('cont');
                            mytimer = setTimeout(function () {
                                callgridhandler();
                            }, 23000);
                        },
                        error: function (a, b, c) {
                            alert(c);
                        }
                    });
                }
                function callCurrentPicks() {

                    $.ajax({
                        url:   "<%=Session("BaseUri")%>" + '/handlers/APR_LoomFlag_Handler.ashx',
                        //url: 'http://coreroute_test.standardtextile.com/handlers/ut_flagboard_handler.ashx',
                        //url: 'http://coredemo.standardtextile.com/handlers/APR_LoomFlag_Handler.ashx',
                        type: 'POST',
                        data: { method: 'GetCurrPicks' },
                        success: function (data) {
                            var json;
                            json = $.parseJSON(data);


                            //   console.log(json[0].updated);
                            var updatetimestamp = new Date(json[0].updated);

                            document.getElementById("<%=UpdateDate.ClientID %>").value = 'Updated: ' + updatetimestamp.toLocaleDateString() + ' ' + updatetimestamp.toLocaleTimeString();

                            if (json.length & 4) {
                                for (i = 0; i < json.length; i++) {
                                    $(PicksLabel[i]).val(json[i].Picks + " PPM");
                                    if (json[i].Picks < 5) {
                                        GetLastStop(i + 2);
                                        $(Containers[i]).css({ "outline": "none", "border-color": "#CE4747", "box-shadow": "0 0 15px #CE4747" });
                                    }
                                    else {
                                        $(StopsLabel[i]).val('');
                                        $(Containers[i]).css({ "outline": "none", "border-color": "#41BF47", "box-shadow": "0 0 15px #41BF47" });
                                    }

                                }
                            }

                            //  console.log('call last stop');
                            GetShiftInfo();
                            //   console.log(json);
                            mytimer = setTimeout(function () {
                                callCurrentPicks();
                            }, 30000);
                        },
                        error: function (a, b, c) {
                            alert(c);
                        }
                    });
                }

                function GetLastStop(LOOMNO) {
                 //   console.log('getlaststop');
                    $.ajax({
                        url:   "<%=Session("BaseUri")%>" + '/handlers/APR_LoomFlag_Handler.ashx',
                        //url: 'http://coreroute_test.standardtextile.com/handlers/ut_flagboard_handler.ashx',
                        //url: 'http://coredemo.standardtextile.com/handlers/APR_LoomFlag_Handler.ashx',
                        type: 'POST',
                        data: { method: 'GetLastStop', args: { LMN: LOOMNO} },
                        success: function (data) {
                            var json;
                            json = $.parseJSON(data);
                            console.log('GetLastStop');
                            console.log(json);
                            if (typeof json[0].STOPTYPE === 'undefined') { console.log('undefined')}
                            var stopname = json[0].STOPTYPE
                            if (stopname !== null) {
                                //        console.log(stopname.split(LOOMNO.toString())[1]);
                                json[0].STOPTYPE = stopname.split(LOOMNO.toString())[1]
                                $(StopsLabel[LOOMNO - 2]).val(json[0].STOPTYPE + ' ' + json[0].Timestamp);
                            }

                        },
                        error: function (a, b, c) {
                            alert(c);
                        }
                    });
                }
                function callPicksgridhandler(No) {


                    $.ajax({
                        url:   "<%=Session("BaseUri")%>" + '/handlers/APR_LoomFlag_Handler.ashx',
                        //url: 'http://coreroute_test.standardtextile.com/handlers/ut_flagboard_handler.ashx',
                        //url: 'http://coredemo.standardtextile.com/handlers/handlers/APR_LoomFlag_Handler.ashx',
                        type: 'POST',
                        data: { method: 'GetLoomPicksGrid', args: { No: No} },
                        success: function (data) {
                            var json;
                            json = $.parseJSON(data);
//                            console.log('pickgridhandler' + No.toString());
//                            console.log(json);

                            $(wijgridName[No + 2]).wijgrid({
                                highlightCurrentCell: false,
                                data: json

                            });
                            
                        //    console.log('cont');
                            mytimer = setTimeout(function () {
                                callPicksgridhandler(No);
                            }, 15000);
                        },
                        error: function (a, b, c) {
                            alert(c);
                        }
                    });
                }

                function AddToGrid(No, Array) {
                    //var dataArray = new Array();
                   // dataArray = Array;
                    var data = [];
                    

                    data.push(Array);
           
//                    console.log(wijgridName[No]);
//                    console.log(data);
                    $(wijgridName[No])
                        .wijgrid({
                            highlightCurrentCell: false,
                            data: data

                        });
                 

                }

                function GridInit(No) {

                    $(wijgridName[No]).wijgrid({
                        allowSorting: false,
                        allowPaging: false,
                        pageSize: 4,
                        staticRowIndex: -1,
                        showSelectionOnRender: false,
                        columns: [
                                 { dataKey: "Stops_PerShift", headerText: "Total Stops", width: 15, textAlignment: "center",
                                     cellFormatter: formatgridcolumns
                                 },
                                 { dataKey: "Disturbance_PerShift", headerText: "Disturbance", width: 18, textAlignment: "center",
                                     cellFormatter: formatgridcolumns
                                 },
                                 { dataKey: "FillStop_PerShift", headerText: "Fill", width: 12, textAlignment: "center",
                                     cellFormatter: formatgridcolumns
                                 },
                                 { dataKey: "WarpStop_PerShift", headerText: "Warp", width: 12, textAlignment: "center",
                                     cellFormatter: formatgridcolumns
                                 },
                                 { dataKey: "PieceLength_PerShift", headerText: "PieceLength", width: 15,
                                     cellFormatter: formatgridcolumns
                                 },
                                 { dataKey: "Host_PerShift", headerText: "Host", width: 12,
                                     cellFormatter: formatgridcolumns
                                 }
                             ]

                    });


                         }

                         function formatgridcolumns(args) {
                             //console.log('formatgrids');
                             //console.log(args.column._originalDataKey);
                             //console.log(args);
                             if (args.row.type & $.wijmo.wijgrid.rowType.data) { // data row (not group header)                   
                                 args.$container.css("text-align", "center");
                                 args.$container.css("height", "20px");
                                 args.$container.css("vertical-align", "middle");
                             }
                         }

                         function GetShiftInfo() {

                             $.ajax({
                                 url:    "<%=Session("BaseUri")%>" + '/handlers/SPC_MaintFlag_Handler.ashx',
                                 // url: 'http://coreroute_test.standardtextile.com/handlers/SPC_MaintFlag_Handler.ashx',
                                 //     url: 'http://coredemo.standardtextile.com/handlers/SPC_MaintFlag_Handler.ashx',
                                 type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                                 data: { method: 'GetShiftinfo' },
                                 success: function (data) {
                                     var json = $.parseJSON(data);

                                     var startdate = new Date(json[0].StartHour);
                                     var start = updatehour(json[0].StartHour);
                                     var enddate = updatehour(json[0].EndHour);
                                     var str = json[0].StartHour
                                     var end = json[0].EndHour
                                     var starthour = startdate.getHours()

                                     //var enddate = new Date(json[0].EndHour);

                                     if (json.length == 1) {
                                         document.getElementById("<%=ShiftDate.ClientID %>").value = startdate.toDateString() + " - Shift: " + json[0].shiftNum.toString();
                                         document.getElementById("<%=ShiftInfo.ClientID %>").value = start.toString() + " " + str.split(" ")[2] + " TO " + enddate.toString() + " " + end.split(" ")[2];
                                     }
                                     console.log('getshiftinfo');
                                     console.log(startdate.toDateString() + " - Shift: " + json[0].shiftNum.toString());
                                 },
                                 error: function (a, b, c) {
                                     alert(c);
                                 }
                             });
                         }

                         function updatehour(inputhour) {
                             var date = new Date(inputhour);
                             var hour = date.getHours()

                             if (hour > 12) {
                                 hour -= 12;
                             } else if (hour === 0) {
                                 hour = 12
                             }

                             return hour
                         }
                function PicksGridInit(No) {

                    $(wijgridName[No]).wijgrid({
                        allowSorting: false,
                        allowPaging: false,
                        pageSize: 4,
                        staticRowIndex: -1,
                        showSelectionOnRender: false,
                        columns: [
                                 { dataKey: "field", headerText: "field", width: 15, textAlignment: "center",
                                     cellFormatter: function (args) {
                                         if (args.row.type & $.wijmo.wijgrid.rowType.data) { // data row (not group header)                   
                                             args.$container.css("text-align", "center");
                                             args.$container.css("height", "20px");
                                             args.$container.css("vertical-align", "middle");
                                         }

                                     }
                                 },
                                 { dataKey: "value", headerText: "val", width: 20, textAlignment: "center",
                                     cellFormatter: function (args) {
                                         if (args.row.type & $.wijmo.wijgrid.rowType.data) { // data row (not group header)                           
                                             args.$container.css("text-align", "center");
                                             args.$container.css("height", "20px");
                                             args.$container.css("vertical-align", "middle");
                                         }
                                     }
                                 }
                             ]

                    });


                         }

                         $(".wijmo-wijgrid-root").on("change", ".wijmo-wijgrid-innercell", function () {
                             $(this).css({ background: "#ccc" });
                             alert("changed");
                         });

                         $("#PicksGrid1").wijgrid({
                             afterCellUpdate: function (e, args) {
                                 console.log('eventchange');
                                 console.log(e);
                                 console.log(args);
                             }
                         });
                         var chkbutns = ["MainContent_Mins", "MainContent_Hrs", "MainContent_Dys", "MainContent_Mnths"];
                         $('.chkbox2').click(function (e, args) {

                             if (e.delegateTarget.parentNode.id) {
                                 var ParentId = parseInt(e.delegateTarget.parentNode.id.split("k")[1]);                                 
                                 
                                 for (i = 0; i < chkbutns.length; i++) { 
                                     var checkbox = document.getElementById(chkbutns[i]);

                                     if (i == (ParentId - 1)) {
                                         checkbox.checked = true
                                     }
                                     else {
                                         checkbox.checked = false
                                     }
                                 }
                             }
                         });
                         $('#MainContent_Exportxl').click(function (e) {
                             e.preventDefault();
                             $('#exportpopup').fadeToggle();
                             console.log(TxtDateFrom.value);

                         });
                         $('#MainContent_GoButton').click(function (e) {
                          //   e.preventDefault();
                             $('#exportpopup').fadeToggle();
                             exportdates.value = TxtDateFrom.value

                         });
                         
</script>

</asp:Content>

