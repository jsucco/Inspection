<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DefectImageEntry.aspx.vb" Inherits="core.Mobile_DataEntry_OrderEntry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<div data-role="header" style="z-index:1; height:45px;">
    <h1 style="display:none;">Defect Image Entry</h1>
      <div style="position:absolute; top: 9px; border:1px solid; width:350px; border-radius:15px; height: 25px;">
          <ASP:LABEL id="lblWorkOrderNumber" style="Z-INDEX: 105; font-size:small; LEFT: 5px; top:4px; POSITION: absolute;"
					runat="server" font-bold="True" width="176px">Defect WorkOrder: </ASP:LABEL>
          <label id="InsertFlag" style="color:darkgreen; font-size:medium; font-weight:800; display:none;position:absolute; left: 200px;">UPDATE SUCCESSFUL</label>
      
      </div>
  </div>
<body>
<div data-role="content" style="padding:1px; height:500px;">
    <div class="hidden">
        <div id="hiddenfrm" style="Z-INDEX: 110; height: 55%; width: 85%; position: absolute; top:25px; left:5%; display:none" >
		    <div id="InspectionSelection" style="display:none;">
                    <ASP:LABEL id="lblUserID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 30px"
					runat="server" font-bold="True" Font-Size="Large">Inspection Selection:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: relative; width: 80%; TOP: 30px" class="ui-field-contain">
                    <div style="Z-INDEX: 102; LEFT: 0px; width: 80%; POSITION: absolute; TOP: 45px" class="ui-field-contain">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
                        <select id="Inspection_pop" name="Inspection_pop" style="width: 70%; height: 55px; " data-native-menu="false" ></select>
                                    
                                </div>
                            </div>
                        <div style="z-index:102; left:85%; position: relative; width:11%; top:220px" class="ui-field-contain">
                            <Button id="closeout1" class="export closebox" style="position:relative; float:right;TOP: 0px; width:95%;  height:45px;">Close</Button>
                        </div>
                        </div>
            <div id="DefectSelection" style="display:none;">
                
                    <ASP:LABEL id="lblDefectID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 30px"
					runat="server" font-bold="True" Font-Size="Large">Defect Selection:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: relative; width: 98%; TOP: 30px" class="ui-field-contain">
                    <div style="Z-INDEX: 102; LEFT: -10px; width: 90%; POSITION: absolute; TOP: 45px" class="ui-field-contain">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
                        <select id="Defect_pop" name="Defect_pop" style="width: 95%; height: 55px; " data-native-menu="false" ></select>
                                    
                                </div>
                            </div>
                <div style="z-index:102; left:85%; position: relative; width:11%; top:220px" class="ui-field-contain">
                        <Button id="closeout2" class="export closebox" style="position:relative; float:right;TOP: 0px; width:95%;  height:45px;">Close</Button>
                    </div>
                        </div>
            </div>>
    </div>
    <div class="ui-field-contain" style="overflow:auto;height:93%;">
      <div class="ui-field-contain" style="width:100%; top: 0px; float:right;">
          <label for="select-custom-1">Defect Description:</label>
            <input name="DefectDesc" id="DefectDesc" type="text" data-inline="false" runat="server" class="inputelement" data-role="text" style="border:none; background-color:transparent;"></input>
          
          </div>
      
      <div class="ui-field-contain" style="width:100%; top: 40px; float:right;">
          <label for="select-custom-1">Location/Time:</label>
            <input name="Location" id="Location" type="text" data-inline="false"  runat="server" class="inputelement" data-role="text"></input>
           
          </div>
      <div class="ui-field-contain" style="width:100%; top: 80px; float:right;">
         
       
        <div class="ui-field-contain" style="width:100%; top: 0px; float:right;">
           <span class="fileUpload btn btn-primary">
               <span class="uploadButton">Add Document</span>
               <form action="" method="post" enctype="multipart/form-data" id="myImageForm"><input type="file" accept="image/*" capture  class="upload" id="flDoc" name="flDoc" />

               </form>

           </span>
          </div>
          <input id="JobNumberHidden" runat="server" type="hidden" value="" />
          <input id="InspectionIDHidden" runat="server" type="hidden" value="" />
    </div>
  </div><!-- /content -->
  
   <div data-role="footer">
    <div id="buttonholder" style="Position:absolute; left: 1%; top: 0px;">       
        <div style="Z-INDEX: 119; LEFT: 125px; POSITION: absolute; TOP: -90px; width: 70px; left: 0px;" tabIndex="19">
					
        
        <button id="UpdateButton" class=export style="width: 100px;" class="sendbackclick">Update</button>
        </div> 

        <div style="Z-INDEX: 119; LEFT: 275px; POSITION: absolute; TOP: -90px; width: 105px; left: 240px;" tabIndex="19">
					
        <button id="ExitButton" class=export style="width: 100px;" runat="server" >Exit</button>

        </div>
        <div style="Z-INDEX: 119; LEFT: 275px; POSITION: absolute; TOP: -90px; width: 105px; left: 480px;" tabIndex="19">
					
        <button id="SelectDefect" class=export style="width: 100px;" runat="server" >SELECT DEFECT</button>

        </div>
       </div>

  </div><!-- /footer -->
</body>

  <meta charset=utf-8 />
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="../../Styles/jquery.mobile-git.css" />
  <script src="../../Scripts/jquery-1.9.1.js"></script>
  <script src="../../Scripts/jquery.ui.datepicker.js"></script>
  <script src="../../scripts/jquery.mobile-git.js"></script>
  <script src="../../Scripts/jquery.mobile.datepicker.js"></script>
<%--  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.js"></script> --%>
  <script src="http://malsup.github.com/jquery.form.js"></script>
    <style>
         #hiddenfrm {
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
  <script type="text/javascript">


      var CartDirection;
      var DefectID = new Number(<%=DefectID%>);
      var HasInspectionID = '<%=HasInspectionID%>';
      var HasDefectID = '<%=HasDefectID%>';
      var CID = '<%=CID%>';
      var WorkOrderSelection = <%=WorkOrderSelection%>;
      var DefectSelection = <%=DefectSelection%>;
      var InspectionStarted = '<%=InspectionStarted%>';
      var InspectionID = '<%=InspectionID%>';
      var hiddenSection = $('div.hidden');
      $(function () {
          var hiddenSection = $('div.hidden');
          if (HasInspectionID == 'false') {

              
              console.log(WorkOrderSelection); 
              var html3 = [];
              var htmlar = [];
              var name;
              if (WorkOrderSelection) { 
                  for(var i = 0; i < WorkOrderSelection.length; i++){
                      name = WorkOrderSelection[i];
                      var TemplateName = name.Name;
                      if (!TemplateName) { 
                          TemplateName = "NoName"; 
                      }
                      // var date = new Date(JSON.parse(name.Inspection_Started));
                      console.log(name.Inspection_StartedString);
                      // var str = (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear() + " " +  date.getHours() + ":" + date.getMinutes();

                      if (name.JobNumber == "SELECT OPTION") { 
                          html3.push('<option value="' + name.id + '">' + name.JobNumber + '</option>');
                          htmlar.push({label: name.JobNumber, value: name.id});
                      } else { 
                          var label = 'ID: ' + name.id + ' WO: ' + name.JobNumber + ' -> ' + TemplateName + ' Started: ' + name.Inspection_StartedString;
                          htmlar.push({label: label, value: name.id});
                          html3.push('<option value="' + name.id + '">ID: ' + name.id + ' WO: ' + name.JobNumber + ' -> ' + TemplateName + ' Started: ' + name.Inspection_StartedString +'</option>');
                      }
                  }
              }
              console.log(htmlar);
              controlshandler.addArrayDrop("Inspection_pop", htmlar, "1000")
              $("#UpdateButton").prop("disabled", true);
              $("#ExitButton").prop("disabled", true);
              $("#SelectDefect").prop("disabled", true);

              $("#hiddenfrm").fadeIn();
              $("#InspectionSelection").fadeIn();
              hiddenSection.fadeIn()
                            .css({ 'display': 'block' })
                            // set to full screen
                            .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
                            .css({
                                top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
                                left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
                            })
                            // greyed out background
                            .css({ 'background-color': 'rgba(0,0,0,0.5)' });
          } else if (HasDefectID == 'false') { 
              var htmlar = [];
              var name;
              if (DefectSelection) { 
                  for(var i = 0; i < DefectSelection.length; i++){
                      name = DefectSelection[i];
                      console.log(name.DefectTime.toString()); 
                      var label = "";
                      if (name.DefectId == -1) { 
                          label = name.DefectDesc.toString()
                      } else { 
                          label = "DefectID: " + name.DefectId.toString() + " DefectTime: " + name.DefectTime.toString() + " Desc: " + name.DefectDesc.toString()
                      }
                      
                      htmlar.push({label: label, value: name.DefectId.toString()});
                  }
                  controlshandler.addArrayDrop("Defect_pop", htmlar, "-1")
                  $("#UpdateButton").prop("disabled", true);
                  $("#ExitButton").prop("disabled", true);
                  $("#hiddenfrm").fadeIn();
                  $("#SelectDefect").prop("disabled", true);
                  $("#DefectSelection").fadeIn();
                  hiddenSection.fadeIn()
                                .css({ 'display': 'block' })
                                // set to full screen
                                .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
                                .css({
                                    top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
                                    left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
                                })
                                // greyed out background
                                .css({ 'background-color': 'rgba(0,0,0,0.5)' });
              }
          }

          //$('.chkbox').on('click', function () {
          //    console.log($(this).attr('id'));
          //});
          //$("#CartNumber").on("mouseleave focusout", function () {
          //    var result = DataControl.isNumeric($(this).val());
          //    var valuestring = new String($(this).val())
          //    DataControl.CartNumberFlag = result;
          //    console.log(result.length);
          //    console.log(result);
          //    if (result == false && valuestring.length > 0) {
          //        console.log("****")
          //        $('#CartNumberFlag').toggle();

          //        var delay = setTimeout(function() { $('#CartNumberFlag').toggle();},3000)
          //    }
          //});
      });

      $(".closebox").click(function (e) {

          console.log('xxx');
          $("#hiddenfrm").fadeOut();
          $("#InspectionSelection").fadeOut();
          $("#DefectSelection").fadeOut();
         
          hiddenSection.fadeOut()
          $("#ExitButton").prop("disabled", false);
          $("#SelectDefect").prop("disabled", false);
      });
      $("#SelectDefect").click(function (e) {
          e.preventDefault();

          $("#hiddenfrm").fadeIn();
          $("#InspectionSelection").fadeIn();
          hiddenSection.fadeIn()
                        .css({ 'display': 'block' })
                        // set to full screen
                        .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
                        .css({
                            top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
                            left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
                        })
                        // greyed out background
                        .css({ 'background-color': 'rgba(0,0,0,0.5)' });
          
      });
      $("#UpdateButton").click(function (e) {
          e.preventDefault();

          if ( $.isNumeric(DefectID) == true) {
              $("#myImageForm").ajaxSubmit({ url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/DefectImage_handler.ashx?DefectID=' + DefectID.toString(), type: 'post', success: function (data) { alert(data); } })
          } else { 
            alert("DefectId passed was not numeric");
          }

          
          console.log("<%=Session("BaseUri")%>" + '/handlers/DataEntry/DefectImage_handler.ashx');
      });
      $("#Inspection_pop").change(function() { 
          var selectedid = $(this).val(); 
          var selectedIndex = 0;
          if (selectedid != -1){
              var querystring = "";
              for (i = 0;i < WorkOrderSelection.length;i++) { 
                  if(WorkOrderSelection[i].id == selectedid) { 
                      selectedIndex = i; 
                  }
              }
              querystring = "CID=" + CID.toString() + "&DefectID=0&InspectionID=" + selectedid.toString() + "&JobNumber=" + WorkOrderSelection[selectedIndex].JobNumber.toString() + "&InspectionStarted=" + InspectionStarted.toString();
              window.location.assign("<%=Session("BaseUri")%>" + "/Mobile/DataEntry/DefectImageEntry.aspx?" + querystring)
          }
          console.log($(this).val());
      });
      $("#Defect_pop").change(function() { 
          console.log('xxx');
          var selectedid = $(this).val(); 
          var selectedIndex = 0;
          if (selectedid != -1){
              var querystring = "";
              //for (i = 0;i < DefectSelection.length;i++) { 
              //    if(WorkOrderSelection[i].id == selectedid) { 
              //        selectedIndex = i; 
              //    }
              //}
              var JobNumberHidden = $("#JobNumberHidden").val().toString()
              var InspectionIDHidden = $("#InspectionIDHidden").val().toString()
              if (JobNumberHidden.length > 0 && InspectionIDHidden.length > 0) { 
                  querystring = "CID=" + CID.toString() + "&DefectID="  + selectedid.toString() + "&InspectionID=" + InspectionIDHidden + "&JobNumber=" + JobNumberHidden + "&InspectionStarted=" + InspectionStarted.toString();
                  window.location.assign("<%=Session("BaseUri")%>" + "/Mobile/DataEntry/DefectImageEntry.aspx?" + querystring)
              } else { 
                  alert('Job Number or InspectionId not set'); 
              }
          }
          console.log($(this).val());
      });
      $("#ExitButton").click(function (e) {

          e.preventDefault();
          $.ajax({
              url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
              type: 'GET',
              data: { method: 'GetRedirect', args: { InspectionId: InspectionID, InspectionStarted: InspectionStarted}},
              success: function (data) {
                  console.log(data);
                  if (data) { 
                      window.location.assign("<%=Session("BaseUri")%>" + data);
                  } else { 
                      window.location.assign("<%=Session("BaseUri")%>" + '/APP/Mob/SPCInspectionInput.aspx');
                  }
                    },
              error: function (a, b, c) {
                  window.location.assign("<%=Session("BaseUri")%>" + '/APP/Mob/SPCInspectionInput.aspx');
              }
          });
       //   window.location.assign("<%=Session("BaseUri")%>" + '/APP/DataEntry/SPCInspectionInput.aspx');
      });
      var controlshandler = {
          addArrayDrop: function (selectid, array, selectedval) {
              var arraylength = array.length
              var selector = document.getElementById(selectid);
              for (i = 0; i < arraylength; i++) {
                  var option = document.createElement("option");
                  option.text = array[i].label;
                  option.value = array[i].value;
                  selector.add(option, selector[0]);
              }
                
              $("#" + selectid).val(selectedval);
              $("#" + selectid).selectmenu();
              $("#" + selectid).selectmenu('refresh', true);

          }
      };
  </script>

</html>
