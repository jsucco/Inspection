<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/APP/MasterPage/Mobile.master" CodeFile="APREntry.aspx.vb" Inherits="core.Mobile_APREntry" %>
<%@ OutputCache Duration="120" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <div class = "main" style="position:absolute; width: 360px;">

        <div id="MenuButtonContainer2" style="position:absolute; margin-left:30px; margin-right:30px; top: 100px; display: none;">
        <input id="MenuButton2" type="button" value="" class="secondlongbutton" />
        <div id="dropdown2" class="dropdown2">
            <input id="LoomPres" type="text"  value="Loom PickCounter" class="dropdown2-sub" style="background-color:#9FA1A4; height: 50px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
            <input id="MaintFB" type="text" name="flagboard" value="Maintenance FlagBoard" class="dropdown2-sub" style="background-color:#D31245; height: 50px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
            <input id="Inspection" name="Inspection" type="text" class="dropdown2-sub" value="Inspection" style="background-color:#C9C1B8; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
        </div>
        </div>
        
    </div>
    <script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.scrollTo.js" type="text/javascript"></script>

        
</script>

    <script type="text/javascript" language="javascript">
        var ctxlinks = ["http://STCSTT.controltex.com", "http://STCCAR.controltex.com/", "http://STCCORETEST.controltex.com"];
        var CorpID = new Number("<%= CID%>");
        var CID_Print = "<%=CID_Print%>"
        var IsTablet = new Boolean();
        var IsPC = new Boolean();
        var BaseURI = new String();
        var Istablet = new String();

        $(this.document).ready(function () {

            // not let it make post back
            event.preventDefault();

            $('#MenuButtonContainer2').fadeIn(2000);

            BaseURI = "<%=Session("BaseUri")%>"


        });

        //        Create javascript handler names chartdata in the handlers folder.  Use the slider function below to get selected values and pass to start and end date. use the json array and set as datasource for chart.  set hanlders equal to click event.

        $('#MenuButton2').on('click', function (e) {
            console.log(e);
            if ($("#dropdown2").is(":visible")) {
                $('#dropdown2').slideUp("slow");
            } else {
                $('#dropdown2').slideDown("slow");
            }
        });

        //$('#MaintFB').on('click', function () {
        //    fadeoutContainers();
   
        //    window.location.assign(BaseURI + '/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx');
         
        //});
     
        function fadeoutContainers() {

            $("#MenuButtonContainer2").fadeOut('slow');

        }
        $('.dropdown2-sub').on('click', function () {
            var elementname = $(this).attr('name');
            fadeoutContainers();
            console.log(BaseURI);
            switch (elementname) {
                case "flagboard":
                    window.location.assign(BaseURI + '/Mobile/Tablet/FlgBrd3_MaintenanceScheduleEditor.aspx');
                    break;
                case "Inspection":
                    window.location.assign(BaseURI + '/Mobile/Presentation/SPCMobile_InspectionEntry.aspx?CID=' + CID_Print);
            }

        });
      

</script> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
</asp:Content>
