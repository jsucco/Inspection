<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/MobileLogin.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="core.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="frmFacilitySelect" ContentPlaceHolderID="MainContent" Runat="Server">
    <STYLE type="text/css">

#loginfrm {
	BORDER-RIGHT: 0px; BORDER-TOP: #95A0A9 1px solid; BACKGROUND: #87A7AA; LEFT: expression(document.body.clientWidth / 2 - this.offsetWidth / 2); BORDER-LEFT: 0px; WIDTH: 100%px; BORDER-BOTTOM: #95A0A9 1px solid; POSITION: absolute; TOP: 150px; HEIGHT: 326px;
     box-shadow: 5px 15px 15px rgba(0,0,0,.7);
     -webkit-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
     -moz-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
     border-radius: 4px;
}
.logininput {
    background: #87A7AA;
    border: 0 none;
    font: 800 18px Arial,Helvetica,Sans-serif;
    color: #d7d7d7;
    width:150px;
    line-height: 30px;
    padding: 6px 15px 6px 35px;
    -webkit-border-radius: 20px;
    -moz-border-radius: 20px;
    border-radius: 20px;
    text-shadow: 0 2px 2px rgba(0, 0, 0, 0.3); 
    -webkit-box-shadow: 0 1px 0 rgba(255, 255, 255, 0.1), 0 1px 3px rgba(0, 0, 0, 0.2) inset;
    -moz-box-shadow: 0 1px 0 rgba(255, 255, 255, 0.1), 0 1px 3px rgba(0, 0, 0, 0.2) inset;
    box-shadow: 0 1px 0 rgba(255, 255, 255, 0.1), 0 1px 3px rgba(0, 0, 0, 0.2) inset;
    -webkit-transition: all 0.7s ease 0s;
    -moz-transition: all 0.7s ease 0s;
    -o-transition: all 0.7s ease 0s;
    transition: all 0.7s ease 0s;
    outline: none;
    }

.logininput :focus {
    background: #fcfcfc;
    color: #6a6f75;
    width: 200px;
    -webkit-box-shadow: 0 1px 0 rgba(255, 255, 255, 0.1), 0 1px 0 rgba(0, 0, 0, 0.9) inset;
    -moz-box-shadow: 0 1px 0 rgba(255, 255, 255, 0.1), 0 1px 0 rgba(0, 0, 0, 0.9) inset;
    box-shadow: 0 1px 0 rgba(255, 255, 255, 0.1), 0 1px 0 rgba(0, 0, 0, 0.9) inset;
    text-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);
    }
</STYLE>

		<DIV id="loginfrm" style="Z-INDEX: 101; height: 398px; width: 340px;">
            <ASP:LABEL id="lblUserID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 130px"
					runat="server" font-bold="True" Font-Size="Large">User ID:</ASP:LABEL>
                <input type="text" id="userid" name="username" class="logininput" runat="server" style="Z-INDEX: 105; LEFT: 100px; POSITION: absolute; TOP: 120px"
					tabIndex="2" runat="server">
                <ASP:LABEL id="lblPassword" style="Z-INDEX: 106; LEFT: 10px; POSITION: absolute; TOP: 195px"
					runat="server" font-bold="True" Font-Size="Large">Password:</ASP:LABEL>
                <input type="text" id="password" name="password" class="logininput" runat="server" style="Z-INDEX: 107; LEFT: 100px; POSITION: absolute; TOP: 190px"
					tabIndex="2">
                    
				<ASP:LABEL id="lblCpyR1" 
					style="Z-INDEX: 108; LEFT: 50px; POSITION: absolute; TOP: 361px" runat="server"
					font-bold="True" font-size="12pt" font-names="Symbol">ã</ASP:LABEL>
				<ASP:LABEL id="lblCpyR2" style="Z-INDEX: 109; LEFT: 65px; POSITION: absolute; TOP: 363px"
					runat="server" font-bold="True" font-size="10pt">2014 Standard Textile Company, Inc.</ASP:LABEL>
				
                <ASP:BUTTON id="btnLogin" 
                    style="Z-INDEX: 120; LEFT: 130px; POSITION: absolute; TOP: 260px" tabIndex="4"
					runat="server" CssClass="export" text="Login"></ASP:BUTTON>
                <ASP:LABEL id="lblError" style="Z-INDEX: 114; LEFT: 64px; POSITION: absolute; TOP: 156px" runat="server"
					font-bold="True" width="308px" height="14px" forecolor="Red"></ASP:LABEL><ASP:LABEL id="SetFocus1" runat="server"></ASP:LABEL>
                &nbsp;&nbsp;
                
                <input id="RememberChk" runat="server" type="checkbox" class="scaledRadioButtons" style="Z-INDEX: 103; LEFT: 130px; POSITION: absolute; TOP: 320px; font-size:medium" />
                <input type="text" value="REMEMBER ME" style ="position:absolute; top: 317px; left: 155px;" class="standardtext" />
        </DIV>
    <script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>

    <script src="http://code.jquery.com/jquery-migrate-1.2.1.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#loginView').hide();

            var userelement = document.getElementById('MainContent_userid');
            console.log($('#MainContent_userid'));
            
            hideKeyboard();
            
        });

        //function hideKeyboard(element) {
           
        //    element.attr('readonly', 'readonly'); // Force keyboard to hide on input field.
        //    element.attr('disabled', 'true'); // Force keyboard to hide on textarea field.
        //    console.log(element[0].getAttribute("readonly"));
        //    setTimeout(function () {
        //        element.blur();  //actually close the keyboard
        //        // Remove readonly attribute after keyboard is hidden.
        //        element.removeAttr('readonly');
        //        element.removeAttr('disabled');
        //        console.log('after');
        //        console.log(element[0].getAttribute("readonly"));
        //    }, 1);
        //}
        function hideKeyboard() {
            //this set timeout needed for case when hideKeyborad
            //is called inside of 'onfocus' event handler
            setTimeout(function () {

                //creating temp field
                var field = document.createElement('input');
                field.setAttribute('type', 'text');
                //hiding temp field from peoples eyes
                //-webkit-user-modify is nessesary for Android 4.x
                field.setAttribute('style', 'position:absolute; top: 0px; opacity: 0; -webkit-user-modify: read-write-plaintext-only; left:0px;');
                document.body.appendChild(field);

                //adding onfocus event handler for out temp field
                field.onfocus = function () {
                    //this timeout of 200ms is nessasary for Android 2.3.x
                    setTimeout(function () {

                        field.setAttribute('style', 'display:none;');
                        setTimeout(function () {
                            document.body.removeChild(field);
                            document.body.focus();
                        }, 14);

                    }, 200);
                };
                //focusing it
                field.focus();

            }, 50);

        }
        $("#MainContent_userid").click(function () {
            

        });
</script>

        
</script>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
</asp:Content>

