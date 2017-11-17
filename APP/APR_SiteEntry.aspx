<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="APR_SiteEntry.aspx.vb" Inherits="core.APP_Menu_APR_SiteEntry" %>

<%@ OutputCache Location="Server" VaryByParam="*" Duration="60" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="main" style="position: relative; margin: auto; width: 98%;">
        <div class="hidden" style="display: none;">
            <input type="hidden" value="False" id="Authenticated_hidden" runat="server" />
        </div>
        <div id="loginfrm" style="z-index: 110; height: 138px; width: 317px; position: absolute; top: 25px; left: 38%; display: none">
            <asp:Login ID="Login1" runat="server"
                OnAuthenticate="OnAuthenticate" Width="200">
            </asp:Login>
            &nbsp;&nbsp;
        </div>
        <div id="MenuButtonContainer" style="position: absolute; display: none;">
            <input id="MenuButton1" type="button" value="" class="systemslongbutton pointer" />
            <div id="dropdown" class="redirect">
                <%--<li>option 1</li>
            <li>option 2</li>
            <li>option 3</li>
            <li>option 4</li>--%>

                <input id="CONTROLTEX" type="text" value="CTX" class="redirect pointer" />
                <input id="ADMIN" type="text" class="pointer" value="ADMIN" />
            </div>
        </div>
        <div id="MenuButtonContainer2" style="position: absolute; left: 26.5%; display: none;">
            <input id="MenuButton2" type="button" value="" class="secondlongbutton pointer" />
            <div id="dropdown2" class="redirect2 pointer">
                <input id="LoomPres" name="loom" type="text" class="dropdown2-sub pointer" value="Looms FlagBoard" class="redirect3" style="background-color: #B80030; height: 45px; width: 300px; font-size: 100%; text-align: center; color: White; font-family: Arial Rounded MT Bold; border: none;" />
                <input id="MaintFB" name="flagboard" type="text" class="dropdown2-sub pointer" value="Maintenance FlagBoard" style="background-color: #B80030; height: 45px; width: 300px; font-size: 100%; text-align: center; color: White; font-family: Arial Rounded MT Bold; border: none;" />
                <input id="Inspect" name="inspect" type="text" class="dropdown2-sub pointer" value="Inspection Utility" style="background-color: #B80030; height: 45px; width: 300px; font-size: 100%; text-align: center; color: White; font-family: Arial Rounded MT Bold; border: none;" />
                <input id="Dashboard" name="Dashboard" type="text" class="dropdown2-sub pointer" value="Dashboard" style="background-color: #B80030; height: 45px; width: 300px; font-size: 100%; text-align: center; color: White; font-family: Arial Rounded MT Bold; border: none;" />
                <input id="Results" name="Results" type="text" class="dropdown2-sub pointer" value="Results" style="background-color: #B80030; height: 45px; width: 300px; font-size: 100%; text-align: center; color: White; font-family: Arial Rounded MT Bold; border: none;" />
                <div id="sub-results" class="sub-redirect2">
                    <input id="sub-Results-1" name="sub-Results-1" type="text" class="dropdown2-Results-sub pointer" value="Inspection" style="background-color: #A5989B; height: 45px; width: 300px; font-size: 100%; text-align: center; color: White; font-family: Arial Rounded MT Bold; border: none;" />
                    <input id="sub-Results-2" name="sub-Results-2" type="text" class="dropdown2-Results-sub pointer" value="Production" style="background-color: #A5989B; height: 45px; width: 300px; font-size: 100%; text-align: center; color: White; font-family: Arial Rounded MT Bold; border: none;" />
                </div>
            </div>

        </div>
        <div id="MenuButtonContainer3" style="position: absolute; left: 51.5%; display: none;">
            <div id="MenuButtonCtx" class="showmore dropdown2-sub pointer">
                <div style="padding-top: 25%;">
                    <img style="display: inherit; margin: auto;" src="../Images/icons8-Microsoft Excel Filled-100.png" class="pointer" />
                    <div class="menu-button-label-wrapper">
                        <label class="menu-button-label pointer">DATA AUTOMATIONS</label>
                    </div>

                </div>
            </div>
            <div id="dropdown3" class="redirect3">
                <input id="WATER" type="text" value="WATER" class="redirect3" />
                <input id="ELECTRIC" type="text" value="ELECTRIC" />
                <input id="GAS" type="text" value="GAS" />
                <input id="EFFLUENT" type="text" />
            </div>
        </div>
        <div id="MenuButtonContainer4" style="position: absolute; left: 76.5%; display: none;">
            <input id="MenuButton4" type="button" value="" class="showmore4" />
            <div id="dropdown4" class="redirect4">
            </div>
        </div>
    </div>
    <script src="../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>

    <style type="text/css">
        .pointer {
            cursor: pointer;
        }

        .menu-button-label {
            top: 10px;
            position: relative;
            color: white;
            font-size: 20px;
            font-weight: 700;
            font-family: Verdana, Arial, Helvetica, sans-serif, impact;
        }

        .menu-button-label-wrapper {
            display: inherit;
            margin: auto;
            width: 240px;
            vertical-align: middle;
            top: 10px;
            position: relative;
        }

        #loginfrm {
            BORDER-RIGHT: 0px;
            BORDER-TOP: #95A0A9 1px solid;
            BACKGROUND: #87A7AA;
            LEFT: expression(document.body.clientWidth / 2 - this.offsetWidth / 2);
            BORDER-LEFT: 0px;
            WIDTH: 100%px;
            BORDER-BOTTOM: #95A0A9 1px solid;
            POSITION: absolute;
            TOP: 150px;
            HEIGHT: 326px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
            border-radius: 4px;
        }

        #ADMIN {
            border: NONE;
            background-color: #9FA1A4;
            font-Family: Arial Rounded MT Bold;
            font-size: 100%;
            text-align: center;
            color: White;
            width: 150px;
            word-wrap: break-word;
            float: left;
            height: 45px;
            outline: none;
            width: 100%;
        }

        #CONTROLTEX {
            border: NONE;
            background-color: #9FA1A4;
            font-Family: Arial Rounded MT Bold;
            font-size: 100%;
            text-align: center;
            color: White;
            width: 100%;
            float: none;
            height: 45px;
            outline: none;
        }

        div.hidden {
            display: none;
            position: fixed;
            z-index: 105;
        }

        .sub-redirect2 {
            display: none;
            position: absolute;
            top: 235px;
            width: 300px;
            background-color: #A5989B;
            color: White;
            font-Family: Arial Rounded MT Bold;
            font-size: 95%;
            text-align: center;
        }

        #MainContent_Login1_Password {
            width: 149px;
            -ms-box-sizing: content-box;
            -moz-box-sizing: content-box;
            box-sizing: content-box;
            -webkit-box-sizing: content-box;
        }

        #MainContent_Login1_UserName {
            width: 149px;
            -ms-box-sizing: content-box;
            -moz-box-sizing: content-box;
            box-sizing: content-box;
            -webkit-box-sizing: content-box;
        }

        label[for*="MainContent_Login1"], label[htmlfor*="MainContent_Login1"] {
            position: relative;
            top: -8px;
        }

        #MainContent_Login1_RememberMe {
            display: none;
        }

        label[for*="MainContent_Login1_RememberMe"], label[htmlfor*="MainContent_Login1_RememberMe"] {
            display: none;
        }
    </style>
    <script type="text/javascript">
        var ctxlinks = ["http://STCSTT.controltex.com", "http://STCCAR.controltex.com/", "http://STCCORETEST.controltex.com"];
        var CorpID = "<%=CID%>";
        var CtxCID = "<%=CtxCID%>"
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
            console.log("LocationObject", CtxCID);
            Authenticated = $("#MainContent_Authenticated_hidden").val();
            var hiddenSection = $('div.hidden');
        });

        $('#CONTROLTEX').on('click', function () {

            if (!CtxCID && CtxCID.length != 6) {
                alert("invalid controltex link")
                return;
            }

            fadeoutContainers();

            window.location.assign("http://CTXWEB.CONTROLTEX.COM/CTX?UC=" + CtxCID);
        });

        $("#MenuButtonCtx").on('click', function () {
            window.location.assign("http://m.standardtextile.com/dataautomations/launch.aspx?ReportType=A");
        });

        $("#ADMIN").on('click', function () {
            fadeoutContainers();
            window.location.assign("<%=MenuUrl%>/Manage/Index?cid=" + CorpID);
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

            switch (elementname) {
                case "loom":

                    permission = true;
                    url = BaseURI + '/APP/Presentation/FlgBrd_STTLoomPickCount.aspx';
                    Destination = "APRLoom";

                    break;
                case "flagboard":

                    permission = true;
                    window.location.href = 'http://maintenance.standardtextile.com?CID=' + CtxCID;
                    return;
                    Destination = "APRFlagboard";

                    break;
                case "utilities":

                    permission = true;
                    url = BaseURI + '/UTILITIES/chartpage.aspx';
                    Destination = "APRUtilities";

                    break;
                case "inspect":

                    permission = true;
                    url = BaseURI + '/APP/Mob/SPCInspectionInput.aspx';
                    Destination = "APRInspect";
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

                    permission = true;
                    window.location.href = 'http://dashboard.standardtextile.com';
                    return;
                    Destination = "APRInspect";

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

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" runat="Server">
</asp:Content>

