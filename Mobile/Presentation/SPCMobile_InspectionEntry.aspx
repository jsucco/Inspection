<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SPCMobile_InspectionEntry.aspx.vb" Inherits="core.Mobile_Presentation_SPCMobile_InspectionReporter" MasterPageFile="~/APP/MasterPage/Mobile.master" %>
<%@ OutputCache Location="Server" Duration="60" VaryByParam="*" %>

<asp:Content ID="htmlContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="DefectTemplate-class">
  <div><input id="button1" type="button" value="one"  style="width:200px; height:200px; font-size:1.2em;" ></input></div>
  <div><input id="button2" type="button" value="two"  style="width:200px; height:200px; font-size:1.2em;" ></input></div>
  <div><input id="button3" type="button" value="three"  style="width:200px; height:200px; font-size:1.2em;" ></input></div>
  <div><input id="button4" type="button" value="four"  style="width:200px; height:200px; font-size:1.2em;" ></input></div>
</div>

</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="ControlOptions" runat="server">
<link rel="stylesheet" type="text/css" href="../../Styles/slick.css" />
<link rel="stylesheet" type="text/css" href="../../Styles/slick-theme.css" />
<style>
    .buttontemplate {
          color: #fff;
          background-color: #6496c8;
          text-shadow: -1px 1px #417cb8;
          border: solid;
          border-color:white;
          display: inline-block;
          margin: 0 10px 0 0;
          padding: 15px 45px;
          font-size: 48px;
          font-family: "Bitter",serif;
          line-height: 1.8;
          appearance: none;
          box-shadow: none;
          border-radius: 0;
    }
        .buttontemplate:active {
            background-color:#A3C6EA;
        }
</style>

<script type="text/javascript" src="//code.jquery.com/jquery-1.11.0.min.js"></script>
<script type="text/javascript" src="//code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
<script type="text/javascript" src="../../Scripts/slick.min.js"></script>

<script type="text/javascript">
    var TemplateNames = <%=TemplateNames%>
    console.log(TemplateNames);
    $(document).ready(function () {
        $.each(TemplateNames, function (index, value) {
            console.log(index);
            console.log(value);

        });
        $('.DefectTemplate-class').slick({
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 3
        });

    });
    


</script>
</asp:Content>