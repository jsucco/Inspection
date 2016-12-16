﻿<%@ Page Title="APR" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="APR_SiteEntry.aspx.vb" Inherits="core.APP_Menu_APR_SiteEntry" %>
<%@ OutputCache Location="Server" VaryByParam="*" Duration="60" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <div class = "main" style="position:relative; margin: auto; width:98%;">
        <div id="MenuButtonContainer" style="position:absolute; display: none;">
        <input id="MenuButton1" type="button" value="" class="systemslongbutton"/>
        <div id="dropdown" class="redirect">
            <%--<li>option 1</li>
            <li>option 2</li>
            <li>option 3</li>
            <li>option 4</li>--%>
        
            <input id="CONTROLTEX" type="text" value="CTX" class="redirect" />
            <input id="WSI" type="text" value="W.S.I." />
        </div>
        </div>
        <div id="MenuButtonContainer2" style="position:absolute; left: 26.5%; display: none;">
        <input id="MenuButton2" type="button" value="" class="secondlongbutton" />
        <div id="dropdown2" class="redirect2">
            <input id="LoomPres" name="loom" type="text" class="dropdown2-sub" value="Looms FlagBoard" class="redirect3" style="background-color:#B80030; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
            <input id="MaintFB" name="flagboard" type="text" class="dropdown2-sub" value="Maintenance FlagBoard" style="background-color:#B80030; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
            <input id="Inspect" name="inspect" type="text" class="dropdown2-sub" value="Inspection Utility" style="background-color:#B80030; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
            <input id="Results" name="Results" type="text" class="dropdown2-sub" value="Results" style="background-color:#B80030; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
        </div>
        </div>
        <div id="MenuButtonContainer3" style="position:absolute; left: 51.5%; display: none;">
        <input id="MenuButton3" name="utilities" type="button" value="" class="showmore dropdown2-sub"/>
        <div id="dropdown3" class="redirect3">
            <input id="WATER" type="text" value="WATER" class="redirect3" />
            <input id="ELECTRIC" type="text" value="ELECTRIC" />
            <input id="GAS" type="text" value="GAS" />
            <input id="EFFLUENT" type="text"/>
        </div>
        </div>
        <div id="MenuButtonContainer4" style="position:absolute; left: 76.5%; display: none;">
        <input id="MenuButton4" type="button" value="" class="showmore4"/>
        <div id="dropdown4" class="redirect4">
            <%--<li>option 1</li>
            <li>option 2</li>
            <li>option 3</li>
            <li>option 4</li>--%>

        </div>
        </div>
    </div>
<%--    <script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>
<%--    <script src="../../Scripts/jquery.scrollTo.js" type="text/javascript"></script>--%>

        
</script>

<script type="text/javascript" language="javascript">
        var ctxlinks = ["http://STCSTT.controltex.com", "http://STCCAR.controltex.com/", "http://STCCORETEST.controltex.com"];
        var CorpID = "<%=CID%>";
        var UserID = "<%=UserID%>";
        var NavPerms = <%=NavPerms%>;
        var IsTablet = new Boolean();
        var IsPC = new Boolean();
        var BaseURI;
        $(this.document).ready(function () {

            // not let it make post back
            event.preventDefault();
            $('#MenuButtonContainer').fadeIn(1400);
            $('#MenuButtonContainer2').fadeIn(2000);
            $('#MenuButtonContainer3').fadeIn(2500);
            $('#MenuButtonContainer4').fadeIn(3000);
            BaseURI = "<%=Session("BaseUri")%>"
            console.log(BaseURI);

        });



        //        Create javascript handler names chartdata in the handlers folder.  Use the slider function below to get selected values and pass to start and end date. use the json array and set as datasource for chart.  set hanlders equal to click event.
        $('#MenuButton1').on('click', function () {
            if ($("#dropdown").is(":visible")) {
                $('#dropdown').slideUp("slow");
            } else {
                $('#dropdown').slideDown("slow");
            }
        });
        $('#MenuButton2').on('click', function () {
            if ($("#dropdown2").is(":visible")) {
                $('#dropdown2').slideUp("slow");
            } else {
                $('#dropdown2').slideDown("slow");
            }
        });

        $('#CONTROLTEX').on('click', function () {
            fadeoutContainers();

            var link = GetCtxLinks();
            window.location.assign(link);
            console.log(link);
        });

        $('.dropdown2-sub').on('click', function () {
            var elementname = $(this).attr('name');
            var Destination;
            var url;
            var permission = false;
            
            console.log(BaseURI);
            switch (elementname) { 
                case "loom":
                    if (NavPerms[0].APRLoom_Enabled == true) { 
                        url = BaseURI + '/APP/Presentation/FlgBrd_STTLoomPickCount.aspx';
                        Destination = "APRLoom";
                    }
                    break;
                case "flagboard":
                    if (NavPerms[0].APRPM_Enabled == true) { 
                        permission = true;
                        url = BaseURI + '/APRMaintenance/Default.aspx?CID=' + CorpID + '&UserID=' + UserID;
                        Destination = "APRFlagboard";
                    }
                    break;
                case "utilities":
                    if (NavPerms[0].APRUtility_Enabled == true) { 
                        permission = true;
                        url = BaseURI + '/UTILITIES/chartpage.aspx';
                        Destination = "APRUtilities";
                    }
                    break;
                case "inspect":
                    if (NavPerms[0].APRInspection_Enabled == true) { 
                        permission = true;
                        url = BaseURI + '/APP/Mob/SPCInspectionInput.aspx';
                        Destination = "APRInspect";
                    }
                    break;
                case "Results":
                    if (NavPerms[0].APRInspection_Enabled == true) { 
                        permission = true;
                        url = BaseURI + '/APP/Presentation/SPCInspectionReporter.aspx';
                        Destination = "APRInspect";
                    } 
                    if (NavPerms[0].APRSPC_Enabled == true) { 
                        permission = true;
                        url = BaseURI + '/APP/Presentation/SPCProductionReporter.aspx';
                        Destination = "APRSPC";
                    }
                    break;
            }

            if (permission == true) {
                fadeoutContainers();
                window.location.assign(url)
            } else {
                $('#entrydenied').fadeIn(700);
                setTimeout(function () { $('#entrydenied').fadeOut(700); }, 3000);
            }
        });

        function fadeoutContainers() {
            $("#MenuButtonContainer").fadeOut('slow');
            $("#MenuButtonContainer2").fadeOut('slow');
            $("#MenuButtonContainer3").fadeOut('slow');
            $("#MenuButtonContainer4").fadeOut('slow');
        }

        function GetCtxLinks() {

            switch (CorpID.toString()) {
                case '00597':
                    return ctxlinks[0];
                    break;
                case '00587':
                    return ctxlinks[1];
                    break;
                case '00577': 
                    return ctxlinks[2];
                    break;
            }
        }



</script> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
</asp:Content>

