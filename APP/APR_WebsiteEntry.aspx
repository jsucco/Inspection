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
        <div id="Container00" style="position: absolute; display: none;">
            <input id="MenuButton00" type="button" value="" class="Container00 pointer" />
        </div>
        <div id="Container01" style="position: absolute; left: 320px; display: none;">
            <input id="MenuButton01" type="button" value="" class="Container01 pointer" />
        </div>
        <div id="Container02" style="position: absolute; left: 630px; display: none;">
            <input id="MenuButton02" type="button" value="" class="Container02 pointer" />
        </div>
        <div id="Container03" style="position: absolute; left: 940px; display: none;">
            <input id="MenuButton03" type="button" value="" class="Container03 pointer" />
        </div>
        <div id="Container04" style="position: absolute; left: 1250px; display: none;">
            <input id="MenuButton04" type="button" value="" class="Container04 pointer" />
        </div>
        <div id="Container05" style="position: absolute; left: 1560px; display: none;">
            <input id="MenuButton05" type="button" value="" class="Container05 pointer" />
        </div>
        <div id="Container10" style="position: absolute; top: 110px; display: none;">
            <input id="MenuButton10" type="button" value="" class="Container10 pointer" />
        </div>
        <div id="Container11" style="position: absolute; left: 320px; top: 110px; display: none;">
            <input id="MenuButton11" type="button" value="" class="Container11 pointer" />
        </div>
        <div id="Container12" style="position: absolute; left: 630px; top: 110px; display: none;">
            <input id="MenuButton12" type="button" value="" class="Container12 pointer" />
        </div>
        <div id="Container13" style="position: absolute; left: 940px; top: 110px; display: none;">
            <input id="MenuButton13" type="button" value="" class="Container13 pointer" />
        </div>
        <div id="Container14" style="position: absolute; left: 1250px; top: 110px; display: none;">
            <input id="MenuButton14" type="button" value="" class="Container14 pointer" />
        </div>
        <div id="Container15" style="position: absolute; left: 1560px; top: 110px; display: none;">
            <input id="MenuButton15" type="button" value="" class="Container15 pointer" />
        </div>
        <div id="Container20" style="position: absolute; top: 220px; display: none;">
            <input id="MenuButton20" type="button" value="" class="Container20 pointer" />
        </div>
        <div id="Container21" style="position: absolute; left: 320px; top: 220px; display: none;">
            <input id="MenuButton21" type="button" value="" class="Container21 pointer" />
        </div>
        <div id="Container22" style="position: absolute; left: 630px; top: 220px; display: none;">
            <input id="MenuButton22" type="button" value="" class="Container22 pointer" />
        </div>
        <div id="Container23" style="position: absolute; left: 940px; top: 220px; display: none;">
            <input id="MenuButton23" type="button" value="" class="Container23 pointer" />
        </div>
        <div id="Container24" style="position: absolute; left: 1250px; top: 220px; display: none;">
            <input id="MenuButton24" type="button" value="" class="Container24 pointer" />
        </div>
        <div id="Container25" style="position: absolute; left: 1560px; top: 220px; display: none;">
            <input id="MenuButton25" type="button" value="" class="Container25 pointer" />
        </div>
    </div>



    <script src="../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>

    <style type="text/css">
        .Container00 {
            background-image: url('../Images/Panel00.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container01 {
            background-image: url('../Images/Panel01.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container02 {
            background-image: url('../Images/Panel02.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container03 {
            background-image: url('../Images/Panel03.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container04 {
            background-image: url('../Images/Panel04.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container05 {
            background-image: url('../Images/Panel05.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container10 {
            background-image: url('../Images/Panel10.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container11 {
            background-image: url('../Images/Panel11.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container12 {
            background-image: url('../Images/Panel12.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container13 {
            background-image: url('../Images/Panel13.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container14 {
            background-image: url('../Images/Panel14.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container15 {
            background-image: url('../Images/Panel15.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container20 {
            background-image: url('../Images/Panel20.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container21 {
            background-image: url('../Images/Panel21.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container22 {
            background-image: url('../Images/Panel22.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container23 {
            background-image: url('../Images/Panel23.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container24 {
            background-image: url('../Images/Panel24.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
        }
        .Container25 {
            background-image: url('../Images/Panel25.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 300px 100px;
            border-radius: 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -webkit-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            -moz-box-shadow: 5px 15px 15px rgba(0,0,0,.7);
            width: 300px;
            height: 100px;
            border: none;
            outline: none;
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
            $('#Container00').fadeIn(1400);
            $('#Container01').fadeIn(1600);
            $('#Container02').fadeIn(1800);
            $('#Container03').fadeIn(2000);
            $('#Container04').fadeIn(2200);
            $('#Container05').fadeIn(2400);
            $('#Container10').fadeIn(1400);
            $('#Container11').fadeIn(1600);
            $('#Container12').fadeIn(1800);
            $('#Container13').fadeIn(2000);
            $('#Container14').fadeIn(2200);
            $('#Container15').fadeIn(2400);
            $('#Container20').fadeIn(1400);
            $('#Container21').fadeIn(1600);
            $('#Container22').fadeIn(1800);
            $('#Container23').fadeIn(2000);
            $('#Container24').fadeIn(2200);
            $('#Container25').fadeIn(2400);
            event.preventDefault();
            BaseURI = "<%=Session("BaseUri")%>";
            Authenticated = $("#MainContent_Authenticated_hidden").val();
            var hiddenSection = $('div.hidden');
            //if (Authenticated == "True") { 
            //    $(hiddenSection).fadeOut();
            //    $("#loginfrm").css('display', 'none');
            //} else { 
            //    $("#loginfrm").fadeIn();
            //    hiddenSection.fadeIn()
            //        .css({ 'display':'block' })
            //        // set to full screen
            //        .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
            //        .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
            //            left:($(window).width() - hiddenSection.width())/2 + 'px' })
            //        // greyed out background
            //        .css({ 'background-color': 'rgba(0,0,0,0.5)' });
            //}
        });






        $(window).resize(function () {

            Authenticated = $("#MainContent_Authenticated_hidden").val();
            var hiddenSection = $('div.hidden');
            if (Authenticated == "True") {
                $(hiddenSection).fadeOut();
                $("#loginfrm").css('display', 'none');
            } else {
                $("#loginfrm").fadeIn();
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

        });



    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" runat="Server">
</asp:Content>

