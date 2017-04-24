<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master"AutoEventWireup="false" CodeFile="utilities.aspx.vb" Inherits="core.UTILITIES_utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        &nbsp;</h2>
     
     <div style="float:left; width:60px; height: 165px">
     </div>

     <div style="float:left; width:1100px; height: 365px">   
        
        <asp:Button ID="water_but" runat="server" CssClass="water" 
              Height="200px" Width="200px"  />
              &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="elec_but" runat="server" CssClass="electric" 
              Height="200px" Width="200px" />
              &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="gas_BUT" runat="server" CssClass="gas" 
              Height="200px" Width="200px" />
              &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="eff_but" runat="server" CssClass="effluent" 
              Height="200px" Width="200px" />
              &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="spec_chart" runat="server" CssClass="spec_chart" 
              Height="200px" Width="200px" />
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </div>

   
    
    
    </div>


</asp:Content>

