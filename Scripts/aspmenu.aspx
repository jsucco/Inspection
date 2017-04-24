<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="aspmenu.aspx.vb" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">



    <asp:Menu ID="SystemButton" runat="server" StaticDisplayLevels="1" Orientation=Horizontal>
        <LevelMenuItemStyles>
            <asp:MenuItemStyle BackColor=#717073 Width=300px Height=300px />
            <asp:MenuItemStyle BackColor=#D31245 Width=300px Height=47px HorizontalPadding=0px />
        </LevelMenuItemStyles>
        <Items>
            <asp:MenuItem Text="SYSTEMS" Value="SYSTEMS">
                <asp:MenuItem Text="CONTROLTEX" Value="CONTROLTEX"></asp:MenuItem>
            </asp:MenuItem>
        </Items>
       
    </asp:Menu>



</asp:Content>

