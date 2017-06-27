<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="LoginTest" %>

<!DOCTYPE html>

<html lang="en"><head>
        <meta charset="utf-8">
        <title>Login - Standard Textile</title>
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0;">
        <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700" rel="stylesheet" type="text/css">
        <style type="text/css">
                        /*********************************************************************************/
            /* Basic                                                                         */
            /*********************************************************************************/

	            *
	            {
		            -moz-box-sizing: border-box;
		            -webkit-box-sizing: border-box;
		            -o-box-sizing: border-box;
		            -ms-box-sizing: border-box;
		            box-sizing: border-box;
	            }

	            html, body {
	                margin: 0;
	                padding: 0;
	                width: 100%;
	                height: 100%;
	                display: table;
	            }

	            body {
	                font-family: 'Source Sans', Arial, sans-serif;
		            font-weight: 400;
	                color: #717073;
	                background-color: #919195;
	            }

	            h1
	            {
		            font-family: 'Open Sans', Arial, sans-serif;
		            font-weight: 300;
		            font-size: 1.5em;
                    margin: 0 0 15px 0;
                    padding: 0; 
	            }

                p 
                {
                    font-family: 'Open Sans', Arial, sans-serif;
                    font-weight: 400;
                    font-size: 0.8em;
                    margin: 0 0 5px 0;
                    padding: 0; 
                }

            /*********************************************************************************/
            /* Wrapper                                                                       */
            /*********************************************************************************/

	            #wrapper
	            {
		            display: table-cell;
	                text-align: center;
	                vertical-align: middle;
	            }

            /*********************************************************************************/
            /* Content                                                                       */
            /*********************************************************************************/
		
	            #content
	            {
	                display: inline-block;
	                text-align: left;
	            }
	

            /*********************************************************************************/
            /* Form                                                                        */
            /*********************************************************************************/
            #login {
	            background-color: #FFF;
	            width: 350px;
	            height: 450px;
	            padding: 70px 50px;
	            text-align: center;
	            margin: 0 auto;
            }

            .tile-login {
	            width: 200px;
	            margin: 0 auto;
            }
            input, input:active, input:focus, button {
	            height: 50px;
	            position: relative;
                margin-bottom: 10px;
            }	
	            .text {
		            width: 148px;
		            font-size: 0.875em;
		            color: #717073;
		            background-color: #FFF;
		            border: solid 1px rgba(233,227,220,1);
                    border-left: none;
		            padding-left: 0.5em;
	            }

	            .button {
		            width: 200px;
		            background-color: #5D87A1;
		            color: #FFF;
		            font-size: 1.125em;
		            cursor: pointer;
		            border: none;
	            }

	            #login a {
		            font-size: 0.875em;
		            text-decoration: none;
		            color: #717073;
	            }

                .checkbox {
                    margin: 10px 0;
                    text-align:left;
                    margin-left: 25px;
                }

                label {
                    display: inline-block;
                    cursor: pointer;
                    position: relative;
                    padding-left: 25px;
                    font-family: 'Open Sans', Arial, sans-serif;
                    font-weight: 400;
                    font-size: 0.875em;
                }

                input[type=checkbox] {
                    display: none;
                }
                    label:before {
                        content: "";
                        display: inline-block;
                        width: 16px;
                        height: 16px;
                        margin-right: 10px;
                        position: absolute;
                        left: 0;
                        bottom: 1px;
                        background-color: #eee;
                        box-shadow: inset 0px 2px 3px 0px rgba(0, 0, 0, .3), 0px 1px 0px 0px rgba(255, 255, 255, .8); 
                    }
                    .checkbox label:before {
                        border-radius: 3px;
                    }
                    input[type=checkbox]:checked + label:before {
                        content:"\2713";
                        text-shadow: 1px 1px 1px rgba(0, 0, 0, .2);
                        font-size: 15px;
                        color: #717073;
                        text-align: center;
                        line-height: 15px;
                    }

            .icon-login {
	            width: 50px;
	            height: 50px;
	            overflow: hidden;
	            float: left;
            }

	            .tile-username {
		            background-color: #73AFB6;
	  	            color: white;
	            }
	            .tile-password {
		            background-color: #FFCC4D;
	  	            color: white;
	            }
	

            .validation-summary-errors, .validation-reqd-errors {
	            width: 200px;
	            text-align: left;
	            margin: 0 auto;
                color: #D31245; 
            }
            h4 {
                font-family: 'Open Sans', Arial, sans-serif;
	            font-weight: 300;
	            font-size: 1em;
                padding: 0;
                margin: 0;
            }
            .error {
                font-family: 'Open Sans', Arial, sans-serif;
	            font-weight: 400;
	            font-size: 0.675em;
                padding: 0;
                margin: 0;
            }


        </style>
        
        <link href="Styles/style-login.css" rel="stylesheet">

        <!-- jQuery -->
        <script type="text/javascript" charset="utf8" src="//code.jquery.com/jquery-1.10.2.min.js"></script>

         <script type="text/javascript">
             
             var login_server = 'http://sso.standardtextile.com';
           
         </script>
    </head>
<body>
    <!-- Wrapper -->
    <div id="wrapper">
        <!-- Content -->
        <div id="content">
            <header>
                <div class="content-wrapper">
                </div>
            </header>
            <div>
                <section class="content-wrapper main-content clear-fix">
                    
    <input type="hidden" id="returnUrl_hidden" runat="server" value="" />

<script type="text/javascript">

    var returnUrl = '';
    returnUrl = returnUrl.replace(/&amp;/g, '&');
    $("#returnUrl_hidden_MainContent").val(returnUrl); 
    var credsError = "<%=credsError%>"
    $(document).ready(function () {
        if (credsError == "true") {
            $("#errorDiv").css("display", "block");
        }
        setTimeout(function () {
            $("#txtUserName").focus();
        }, 500);
        $('body').css('zoom', '75%');

        //$("#btnSubmit").unbind("click").bind("click", function () {
        //    submitLogin();
        //    return false;
        //});

        $(document).keypress(function (e) {
            if (e.which == "13") {
                SubmitOnce();
            }
        });
    });


    function submitLogin() {

        $(".validation-reqd-errors").hide();
        $(".validation-summary-errors").hide();

        var username = $.trim($("#txtUserName").val());
        var password = $.trim($("#txtPassword").val());
        var rememberme = false;
        if ($("#chkRememberMe").prop("checked")) {
            rememberme = true;
        }

        if (!username || !password) {
            $(".validation-reqd-errors").show();
            return
        }

        $.ajax({
            type: "POST",
            url: login_server + '/Login',
            data: JSON.stringify({ Username: username, Password: password, RememberMe: rememberme, returnUrl: returnUrl }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                alert("success")
            },
            error: function (data) {
                console.log(data);
                var responseText = data["responseText"];
                if (!responseText) {
                    alert("Faulure");
                }
                else {
                    $(".validation-summary-errors").show();
                }
            }
        });
        
    }

    var clicks = 0;
    function SubmitOnce() {
        clicks++;

        if (clicks == 1) {
            return true;
        } else {
            $("#MainContent_btnSubmit").prop('disabled', true);
        }
    }


</script>


        <div id="login">
            <h1>Welcome</h1>
            <p>Please enter your network login</p>
            
            <div>
                <form runat="server">
                    <div class="tile-login">
                        <div class="icon-login tile-username"><img src="http://sso.standardtextile.com/images/icon_username.png"></div>
                        <input class="text" runat="server" id="txtUserName" placeholder="Username" value="textile\">
                    </div>
                    <div class="tile-login">
                        <div class="icon-login tile-password"><img src="http://sso.standardtextile.com/images/icon_password.png"></div>
                        <input class="text" runat="server" id="txtPassword" name="pwdPassword" value="234" type="password" placeholder="Password">
                    </div>
                    <div class="checkbox">
                        <input type="checkbox" id="chkRememberMe">
                        <label for="chkRememberMe">Stay logged in?</label>
                    </div>
                    <asp:Button id="btnSubmit" OnClientClick="SubmitOnce()" runat="server" CssClass="button" Text="Log In" />
                    <%--<button id="btnSubmit" class="button">Log In</button>--%>
               
                    <div id="errorDiv" class="validation-summary-errors" style="display: none">
                        <h4>Login failed</h4>
                        <p class="error">The username or password provided is incorrect</p>
                    </div>
                    <div class="validation-reqd-errors" style="display: none">
                        <h4>Login failed</h4>
                        <p class="error">Username and password are required</p>
                    </div>
                    <div style="display:block; overflow:auto; margin-bottom:20px;"> 
                        <p for="location-select">Select Ctx Location:</p>
                        <select id="location-select"> 
                        </select>
                        <p style="font-size:11px;">*Indiciates which Ctx Location to varify credentials against <br> in addition to Active Directory.</p>
                    </div>
                </form>
                </div> 
        </div>
                </section>
            </div>
            <footer>
                <div class="content-wrapper">
                    <div>
                    </div>
                </div>
            </footer>
        </div>
    </div>
<script type="text/javascript"> 
    $(function () {
        var locations_arr = <%=loc_array%>
        var cid = '<%=selected_cid%>';

        LocationCtxLoc(locations_arr, cid);

    });
    function LocationCtxLoc(Locations, selected_cid) {
        var html = [];
        html.length = 0;
        html.push('<option value="">NO SELELECTION</option>');
        if (Locations != null && Locations.length > 0) {
            var rec;
            for (var i = 0; i < Locations.length; i++) {
                rec = Locations[i];
                var sel_str = ''; 
                if (rec.CID == selected_cid)
                    sel_str = 'selected'; 

                html.push('<option ' + sel_str + ' value="' + rec.CID + '">' + rec.text + '</option>');
            }
        } else {
            html.push('<option value="NA">ERR-NA</option>');
        }
        $("#location-select").html(html.join('')).bind("change", function(){
            var selected = $(this).val();
            window.location.assign('/Login.aspx?UC=' + selected);
        });
    }

</script>
</body>
</html>

