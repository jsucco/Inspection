<%@ Page Title="APR" Language="VB" AutoEventWireup="false" CodeFile="MobileMaintSched_Editor.aspx.vb" Inherits="core.Mobile_Tablet_MobileMaintSched_Editor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<div data-role="header" style="z-index:1; height:45px;">
    <h1 style="display:none;">Board Schedule Editor</h1>
      <div style="position:absolute; top: 9px; border:1px solid; width:230px; border-radius:15px; height: 25px;">
          <ASP:LABEL id="lblWorkOrderNumber" style="Z-INDEX: 105; font-size:small; LEFT: 5px; top:4px; POSITION: absolute;"
					runat="server" font-bold="True" width="176px">WorkOrder Number:</ASP:LABEL>
          <h3 id="MS_Workorder" style="position: absolute; top: -18px; left: 150px;">1111</h3>
      
      </div>
  </div>
<body>
<div data-role="content" style="padding:1px; height:500px;">
    <div class="ui-field-contain" style="overflow:auto;height:93%;">
      <div class="ui-field-contain" style="width:100%; top: 0px; float:right;">
          <label for="select-custom-1">Machine Name:</label>
            <select name="select-custom-2" id="MM_Name" class="inputelement" data-native-menu="false"></select>
          
          </div>
      <div class="ui-field-contain" style="width:100%; top: 40px; float:right;">
          <label for="select-custom-5">Board ID:</label>
            <select name="select-custom-5" id="MFBID_Sel" class="inputelement" data-native-menu="false"></select>
          
          </div>
      <div class="ui-field-contain" style="width:100%; top: 80px; float:right;">
          <label for="select-custom-1">Maintenace Type:</label>
            <select name="select-custom-3" id="MT_Name" class="inputelement" data-native-menu="false"></select>
           
          </div>
      <div class="ui-field-contain" style="width:100%; top: 120px; float:right;">
           <label for="select-custom-1">Next Maintenace Date:</label>
            <input type="text" id="MS_Next_Main_Date" data-inline="false" data-role="date" class="inputelement" readonly="readonly">
          </div>
      <%--<div class="ui-field-contain" style="width:100%; top: 120px; float:right;">
           <label for="CD-date-input">Completed Date:</label>
            <input type="text" id="MS_Main_Comp_Date" data-inline="false" data-role="date" class="inputelement" readonly="readonly">
          </div>--%>
      <div class="ui-field-contain" style="width:100%; top: 160px; float:right;">
           <label for="CD-date-input">Notes:</label>
            <input type="text" id="MS_Notes" data-inline="false" data-role="text" class="inputelement">
          </div>
      <div class="ui-field-contain" style="width:100%; top: 200px; float:right;">
           <label for="UR-input">UnScheduled Reason:</label>
            <input type="text" id="MS_Unscheduled_Reason" data-role="text" class="inputelement">
          </div>
      <div class="ui-field-contain" style="width:100%; top: 240px; float:right;">
           <label for="select-custom-4">Completed By:</label>
            <select name="EMPFirstName" id="EMP_First_Name" data-native-menu="false" class="inputelement"></select>
          </div>
      <div class="ui-field-contain" style="width:100%; top: 260px; float:right;">
           <label for="ID-number">Interval Days:</label>
            <input type="number" data-clear-btn="true" name="ID-number" id="MS_Frequency" class="inputelement" value="">
          </div>
    </div>
  </div><!-- /content -->
  
   <div data-role="footer">
    <div id="buttonholder" style="Position:absolute; left: 1%; top: 0px;">       
        <div style="Z-INDEX: 119; LEFT: 125px; POSITION: absolute; TOP: 0px; width: 70px; left: 0px;" tabIndex="19">
					
        <button id="UpdateButton" class=export style="width: 100px;" class="sendbackclick">Update</button>
        </div> 

        <div style="Z-INDEX: 119; LEFT: 200px; POSITION: absolute; TOP: 0px; width: 70px; left: 120px; margin: 0px" tabIndex="19">
					
        <button id="DeleteButton" class=export style="width: 100px;" class="sendbackclick">Delete</button>
        </div> 

        <div style="Z-INDEX: 119; LEFT: 275px; POSITION: absolute; TOP: 0px; width: 105px; left: 240px;" tabIndex="19">
					
        <button id="ExitButton" class=export style="width: 100px;" class="sendbackclick">Exit</button>
        </div>
       </div>
    <div class="ui-field-contain" style="top: 51px; position:absolute; left: 1%;">
          <input id="MS_Main_Comp_Date" type="checkbox" name="check-1" style="font-weight:bold; width:32px; height:32px;" class="chkbox inputelement" value="checked" />
          <label style="font-weight:bold; width:200px; position:absolute; left:45px; top:50%;">WorkOrder Completed</label>

      </div>
  </div><!-- /footer -->
</body>
</div><!-- /page -->
<meta charset=utf-8 />
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="../../Styles/jquery.mobile-git.css" />
  <script src="../../Scripts/jquery-1.9.1.js"></script>
  <script src="../../Scripts/jquery.ui.datepicker.js"></script>
  <script src="../../Scripts/jquery.mobile-git.js"></script>
  <script src="../../Scripts/jquery.mobile.datepicker.js"></script>
  <link rel="stylesheet" href="../../Scripts/jquery.mobile.datepicker.css" />
  <script type="text/javascript">
      var BaseURI;
      var MSID_Select;
      var MS_Workorder;
      var MS_Maint_Code;
      var MM_Id;
      var MT_Id;
      var MS_Frequency;
      var MS_Next_Main_Date;
      var MS_Main_Comp_Date;
      var EMP_ID; 
      var MS_Unscheduled_Reason;
      var MS_Notes;
      var MethodType;
      var MFBID = new Number("<%= MFBID %>");
      var MFBNAME = '<%=MFBNAME%>';
      var FBSelList;
      var selectedCID = 0; 
      var MFBID_selected;
        $(document).ready(function () {
            $.support.touchOverflow = true;
            $.mobile.touchOverflowEnabled = true;
            var pageheight = screen.height;
            var pagewidth = screen.width - 7;
            var completedbyarray = <%=CompletedByStringArray%>
            var maintenancenamesarray = <%=MaintenanceStringArray%>
            var machinenamesarray = <%=MachineStringArray%>
            MS_Workorder = <%=WorkOrder%>
            MS_Maint_Code = <%=MS_Maint_Code%>
            MSID_Select = <%=MS_ID%>
            MM_Id = <%=MM_Id%>
            MT_Id = <%=MT_Id%>
            MS_Frequency = <%=MS_Frequency%>
            MS_Next_Main_Date = "<%=MS_Next_Main_Date%>"
            MS_Main_Comp_Date = "<%=MS_Main_Comp_Date%>"
            EMP_ID = <%=EMP_ID%>
            MS_Unscheduled_Reason = "<%=MS_Unscheduled_Reason%>"
            MS_Notes = "<%=MS_Notes%>"
            MethodType = "<%=MethodType%>"
            BaseURI = "<%=Session("BaseUri")%>";
            MFBID_selected = <%=MFBID%>;
            selectedCID = <%=CID%>;
            FBSelList = <%=FBSelList%>;
            console.log(FBSelList);


            $('.mobilebody').css({ "height": pageheight.toString() + "px" });
            $('.ui-content').css({ "height": (pageheight - 270).toString() + "px" });
            $('#scrollable').css({ "height": (pageheight - 210).toString() + "px" });
            $('#MS_Workorder').text(MS_Workorder);
            $('#MS_Next_Main_Date').val(MS_Next_Main_Date)
            //$('#MS_Main_Comp_Date').val(MS_Main_Comp_Date)
            $('#MS_Unscheduled_Reason').val(MS_Unscheduled_Reason)
            $('#MS_Notes').val(MS_Notes)
            $('#MS_Frequency').val(MS_Frequency)
            $('#HeadLoginView_HyperLink1').css({"display": 'none'});
            if (MS_Main_Comp_Date != "" ) { 
                document.getElementById('MS_Main_Comp_Date').checked = true;
            } else { 
                document.getElementById('MS_Main_Comp_Date').checked = false;
            }

            console.log($('#HeadLoginView_HyperLink1'))
            controlshandler.addArrayDrop("MM_Name", machinenamesarray, MM_Id)
            controlshandler.addArrayDrop("MFBID_Sel", FBSelList, MFBID_selected)
            controlshandler.addArrayDrop("MT_Name", maintenancenamesarray, MT_Id)
            controlshandler.addArrayDrop("EMP_First_Name", completedbyarray, EMP_ID)
        });

        var controlshandler = {
            addArrayDrop: function (selectid, array, selectedval) {
                var arraylength = array.length
                console.log(array);
                var selector = document.getElementById(selectid);
                for (i = 0; i < arraylength; i++) {
                    var option = document.createElement("option");
                    option.text = array[i].text;
                    option.value = array[i].id;
                    selector.add(option, selector[0]);
                }
                
                $("#" + selectid).val(selectedval);
                $("#" + selectid).selectmenu();
                $("#" + selectid).selectmenu('refresh', true);

            }
        };

        var DataControl = {

            SendInputElements: function (inputarray) {
                var JsonString = JSON.stringify(inputarray);
                console.log(MethodType)
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/SPC_MaintFlag_Handler.ashx',
                    type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                    data: { method: 'UpdateMobile', args: { Json: JsonString, methodname: MethodType, PassCID: selectedCID } },
                    success: function (data) {
                        if(data == "False") {alert("error inserting data - update")}
                        if(data == "True") {window.location = BaseURI + "/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx?MFBID=" + MFBID.toString() + "&MFBNAME=" + MFBNAME }
                        
                        console.log(data);
                    },
                    error: function(a, b, c) {
                        alert("error sending data - update failed");
                    }

                })
            },
            DeleteRecord: function (MSID) { 
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/SPC_MaintFlag_Handler.ashx',
                    type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                    data: { method: 'DeleteMaintInputData', args: { MSID: MSID}},
                    success: function (data) {
                        if(data == "Row Deleted") {
                            window.location = BaseURI + "/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx"
                        } else {
                            alert("error deleting data - delete failed")
                        }
                        
                        console.log(data);
                    },
                    error: function(a, b, c) {
                        alert("error sending data - update failed");
                    }

                })
            }, 
            SetCookie: function (cname, cvalue, exdays) { 
                var d = new Date();
                d.setTime(d.getTime() + (exdays*24*60*60*1000));
                var expires = "expires="+d.toUTCString();
                document.cookie = cname + "=" + cvalue + "; " + expires;

            }
        };

        $("#UpdateButton").click(function (e) {
            e.preventDefault();
            var elementarrayval = [];

            $(".inputelement").each(function () {
                var elementid = $(this).attr('id');
                var elementval = $(this).val();
                if (elementid != undefined || elementid != null) {
                    switch (elementid) { 
                        case "MS_Main_Comp_Date": 
                            elementarrayval.push({value:$(this).prop('checked'),key:elementid});
                            break;
                        case "EMP_First_Name": 
                            DataControl.SetCookie("FBUserID", elementval, 365);
                            elementarrayval.push({value:elementval,key:elementid});
                            break;
                        default: 
                            elementarrayval.push({value:elementval,key:elementid});
                    }
                    
                }
            });
            elementarrayval.push({value: MSID_Select ,key: 'MS_Id'});
            elementarrayval.push({value: MS_Workorder ,key: 'MS_Workorder'});
            elementarrayval.push({value: MS_Maint_Code ,key: 'MS_Maint_Code'});
            var selectBoardID = $('#MFBID_Sel').val();
            if (selectBoardID && selectBoardID.toString().length > 0) {
                elementarrayval.push({value: selectBoardID, key: 'MFB_Id'});
            } else { 
                alert('Please Select a Board ID'); 
                return false;
            }
         
            DataControl.SendInputElements(elementarrayval);
        });
        
        $("#DeleteButton").click(function (e) {
            e.preventDefault();
            var r=confirm("SURE? Click the OK To Delete!");
            if (r==true)
            {
                DataControl.DeleteRecord(MSID_Select);
            }
            else
            {
          
            }

            
        });
        
        $("#ExitButton").click(function (e) {
            e.preventDefault();
            window.location = BaseURI + "/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx"
        });

</script>
</html>
