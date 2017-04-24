<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="MachineStatus.aspx.vb" Inherits="core.EQUIPMENT_COMMERCIAL_WASHING_commwash" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<div style="height:75px; width: 1157px;" ></div>
<div style="float:left; height:400px; width: 662px;"> 
<asp:Label ID="machinetitle_label" runat="server" ForeColor="#717073" 
                    Font-Bold="True" Font-Names="Segoe UI Symbol" 
        Font-Size="X-Large"></asp:Label>
<p style="height:15px;"></p>
<asp:Image ID="Image1" runat="server" Height="426px" Width="423px" />
</div>
<div style="float:left; height:600px; width: 400px;">
        <br />
        <br />
        <br />
        <asp:Button ID="commwash_details_button" runat="server" Font-Bold="True" 
            Font-Names="Segoe UI Symbol" Font-Size="Medium" ForeColor="White" 
            Height="45px" Text="DETAILS" Width="385px" BorderStyle="Outset" CssClass="firstlongbutton" />
        &nbsp;&nbsp;
        <p style="height:20px;"></p>
        <asp:Button ID="commwash_safety_button" runat="server" Font-Bold="True" 
            Font-Names="Segoe UI Symbol" Font-Size="Medium" ForeColor="White" 
            Height="45px" Text="SAFETY" Width="385px" BorderStyle="Outset" CssClass="second" />
        &nbsp;&nbsp;
        <p style="height:20px;"></p>
        <asp:Button ID="commwash_current_button" runat="server" Font-Bold="True" 
            Font-Names="Segoe UI Symbol" Font-Size="Medium" ForeColor="White" 
            Height="45px" Text="CURRENT USES" Width="385px" BorderStyle="Outset" CssClass="thirdmenu" />
        &nbsp;&nbsp;
        <p style="height:20px;"></p>
        <asp:Button ID="commwash_hitory_button" runat="server" Font-Bold="True" 
            Font-Names="Segoe UI Symbol" Font-Size="Medium" ForeColor="White" 
            Height="45px" Text="HISTORY" Width="385px" BorderStyle="Outset" CssClass="fourth" />
        &nbsp;&nbsp;
        <p style="height:20px;"></p>
        <asp:Button ID="commwash_training" runat="server" Font-Bold="True" 
            Font-Names="Segoe UI Symbol" Font-Size="Medium" ForeColor="White" 
            Height="45px" Text="TRAINING" Width="385px" BorderStyle="Outset" CssClass="firstlongbutton" />
        &nbsp;&nbsp;
        <p></p>
</div>
<div style="height:135px;"></div>
</asp:Content>

