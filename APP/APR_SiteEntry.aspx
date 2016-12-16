<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="APR_SiteEntry.aspx.vb" Inherits="core.APP_Menu_APR_SiteEntry" %>
<%@ OutputCache Location="Server" VaryByParam="*" Duration="60" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <div class = "main" style="position:relative; margin: auto; width:98%;">
        <div class="hidden">
            <input type="hidden" value ="False" id="Authenticated_hidden" runat="server" />
           </div> 
                <div id="loginfrm" style="Z-INDEX: 110; height: 138px; width: 317px; position: absolute; top:25px; left:38%; display:none" >
                    <asp:Login ID="Login1" runat="server"
                        OnAuthenticate="OnAuthenticate" Width="200">
                    </asp:Login>
                    &nbsp;&nbsp;
                  </div>
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
            <input id="Dashboard" name="Dashboard" type="text" class="dropdown2-sub" value="Dashboard" style="background-color:#B80030; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
            <input id="Results" name="Results" type="text" class="dropdown2-sub" value="Results" style="background-color:#B80030; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
        <div id="sub-results" class="sub-redirect2">
            <input id="sub-Results-1" name="sub-Results-1" type="text" class="dropdown2-Results-sub" value="Inspection" style="background-color:#A5989B; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
            <input id="sub-Results-2" name="sub-Results-2" type="text" class="dropdown2-Results-sub" value="Production" style="background-color:#A5989B; height: 45px; width: 300px; font-size: 100%; text-align:center; color:White; font-Family:Arial Rounded MT Bold; border: none;" />
        </div>
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

        </div>
        </div>
    </div>
    <script src="../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>

<STYLE type="text/css">
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
        z-index:105;
    }
.sub-redirect2 {
    display: none;
    position:absolute;
    top: 235px;
    width:300px;
    background-color:#A5989B;
    color:White;
    font-Family:Arial Rounded MT Bold;
    font-size: 95%;
    text-align:center;
}
    #MainContent_Login1_Password {
        width:149px;
    
        -ms-box-sizing:content-box;
        -moz-box-sizing:content-box;
        box-sizing:content-box;
        -webkit-box-sizing:content-box; 
    }
    #MainContent_Login1_UserName {
        width:149px;
        -ms-box-sizing:content-box;
        -moz-box-sizing:content-box;
        box-sizing:content-box;
        -webkit-box-sizing:content-box; 
    }
    label[for*="MainContent_Login1"], label[htmlfor*="MainContent_Login1"]
    {
        position: relative; 
        top: -8px; 
 
    }
    #MainContent_Login1_RememberMe {
        display: none; 
    }
    label[for*="MainContent_Login1_RememberMe"], label[htmlfor*="MainContent_Login1_RememberMe"]
    {
        display:none; 
 
    }
</STYLE>
<script type="text/javascript" language="javascript">
        var ctxlinks = ["http://STCSTT.controltex.com", "http://STCCAR.controltex.com/", "http://STCCORETEST.controltex.com"];
        var CorpID = "<%=CID%>";
        var UserID = "<%=UserID%>";
        var NavPerms = <%=NavPerms%>;
        var Authenticated;
        var cookieuserdata = "<%=CookieUserData%>"
        var IsTablet = new Boolean();
        var IsPC = new Boolean();
        var BaseURI;

        $(this.document).ready(function () {

            event.preventDefault();
            $('#MenuButtonContainer').fadeIn(1400);
            $('#MenuButtonContainer2').fadeIn(2000);
            $('#MenuButtonContainer3').fadeIn(2500);
            $('#MenuButtonContainer4').fadeIn(3000);
            BaseURI = "<%=Session("BaseUri")%>";
            console.log(BaseURI);
            Authenticated = $("#MainContent_Authenticated_hidden").val();
            var hiddenSection = $('div.hidden');
            if (Authenticated == "True") { 
                $(hiddenSection).fadeOut();
                $("#loginfrm").css('display', 'none');
            } else { 
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
        });

        $('#CONTROLTEX').on('click', function () {
            fadeoutContainers();

            var link = GetCtxLinks();
            window.location.assign(link);
        });
        
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
                $('#sub-results').css('display', 'none');
            }
        });
        $('.dropdown2-Results-sub').on('click', function () {
            var elementname = $(this).attr('name');
            var Destination;
            var url;
            var permission = false;

            switch (elementname) { 
                case "sub-Results-1":
                    if (Authenticated == 'True') { 
                        url = BaseURI + '/APP/Presentation/InspectionVisualizer.aspx';
                        permission = true;   
                    }
                    break;
                case "sub-Results-2":
                    if (Authenticated == 'True') { 
                        url = BaseURI + '/APP/Presentation/SPCProductionReporter.aspx';
                        permission = true;
                    }
                    break;
            }
            console.log(permission);
            console.log(url);
            console.log(Authenticated);
            if (permission == true) {
                fadeoutContainers();
                window.location.assign(url)
            }
            else if (Destination = "NONE") { 

            }
            else {
                $('#entrydenied').fadeIn(700);
                setTimeout(function () { $('#entrydenied').fadeOut(700); }, 3000);
            }
        });
        $('.dropdown2-sub').on('click', function () {
            var elementname = $(this).attr('name');
            var Destination;
            var url;
            var permission = false;
            
            console.log(BaseURI);
            switch (elementname) { 
                case "loom":
                    if (Authenticated == 'True') { 
                        permission = true;
                        url = BaseURI + '/APP/Presentation/FlgBrd_STTLoomPickCount.aspx';
                        Destination = "APRLoom";
                    }
                    break;
                case "flagboard":
                    if (Authenticated == 'True') { 
                        permission = true;
                        url = BaseURI + '/APRMaintenance/Default.aspx?CID=' + CorpID;
                        Destination = "APRFlagboard";
                    }
                    break;
                case "utilities":
                    if (Authenticated == 'True') { 
                        permission = true;
                        url = BaseURI + '/UTILITIES/chartpage.aspx';
                        Destination = "APRUtilities";
                    }
                    break;
                case "inspect":
                    if (Authenticated = 'True') { 
                        permission = true;
                        url = BaseURI + '/APP/Mob/SPCInspectionInput.aspx';
                        Destination = "APRInspect";
                    }
                    break;
                case "Results":
                   
                    permission = false;
                    if ($("#sub-results").is(":visible")) {
                        $('#sub-results').slideUp("slow");
                        $("#dropdown2").css('height', 'auto');
                    } else {
                        $("#dropdown2").css('height', '273px');
                        $('#sub-results').slideDown("slow");
                    }
                    Destination = "NONE";
                    break;
                case "Dashboard":
                    if (Authenticated = 'True') { 
                        permission = true;
                        url = BaseURI + '/APRDashboard/Default.aspx';
                        Destination = "APRInspect";
                    }
                    break;
            }

            if (permission == true) {
                fadeoutContainers();
                window.location.assign(url)
            }
            else if (Destination = "NONE") { 

            }
            else {
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

        $( window ).resize(function() {

            Authenticated = $("#MainContent_Authenticated_hidden").val();
            var hiddenSection = $('div.hidden');
            if (Authenticated == "True") { 
                $(hiddenSection).fadeOut();
                $("#loginfrm").css('display', 'none');
            } else { 
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

        });



</script> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
</asp:Content>

