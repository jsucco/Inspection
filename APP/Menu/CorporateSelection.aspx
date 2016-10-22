<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/CorpSelect.master" AutoEventWireup="false" CodeFile="CorporateSelection.aspx.vb" Inherits="core.login" %>

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

		<DIV id="loginfrm" style="Z-INDEX: 101; height: 398px; width: 397px; position: absolute; top:200px;">
            <ASP:LABEL id="lblUserID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 130px"
					runat="server" font-bold="True" Font-Size="Large">CORPORATE NAME:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: absolute; TOP: 120px">
                    <div style="Z-INDEX: 102; LEFT: 100px; POSITION: relative; TOP: 5px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
                        <select id="selectNames" name="CorporateName" style="width: 162px; height: 35px; " class="inputelement"></select>
               
                            </div>
                </div>

               <input type="button" style="Z-INDEX: 120; LEFT: 130px; POSITION: absolute; TOP: 260px" value="GO" class="export" />
            
        </DIV>

    <script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            console.log('test loginpage');
            var width = $(window).width();
            var height = $(window).height();
            var CorpList = <%=CorpList%>;
            var html = [];
            var name;
            console.log(CorpList);
            if (CorpList[0] != 0) { 
                for(var i = 0; i < CorpList.length; i++){
                    name = CorpList[i];
                    html.push('<option value="'+name.id+'">'+name.text+'</option>');
                }
            }

            $("#selectNames").html(html.join('')).bind("change", function(){

                var selectedid = $(this).val();
                if (selectedid) { 
                    window.location.assign("<%=Session("BaseUri")%>" + '?UC=000' + selectedid)
                    }
            });
           
        });

</script>
<script type="text/javascript" src="Scripts/ClientScreenData.js">
        
</script>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
</asp:Content>

