<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="BarCharts.aspx.vb" Inherits="core.UTILITIES_Charts" %>

<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Input" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Calendar" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1GridView" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Chart" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1ProgressBar" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Extenders.4" Namespace="C1.Web.Wijmo.Extenders.C1FormDecorator" TagPrefix="c1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="chart">
   
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <fieldset>
                        <legend>Date Range</legend>
                        <div class="contain">
                            <div class="third">
                                <asp:Label ID="LblDateFrom" runat="server" CssClass="left" AssociatedControlID="TxtDateFrom">Begin Date</asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDateTo" runat="server" AssociatedControlID="TxtDateTo" 
                                    CssClass="left">End Date</asp:Label>
                                <div class="left">
                                    <c1:C1InputDate ID="TxtDateFrom" runat="server" ShowTrigger="true" 
                                        Date="01/23/2014 21:28:00" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <c1:C1InputDate ID="TxtDateTo" runat="server" CssClass="left" 
                                        ShowTrigger="true" Date="01/29/2014 12:52:00" />
                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="Button2" runat="server" Height="30px" 
                                        Text="LineChart Usage Usage" Width="180px" />

                                </div>
                            </div>
                            <div class="third">
                                <div class="left">
                                </div>
                            </div>
                            <%--<div class="third">
                                <asp:Button ID="BtnUpdateRange" runat="server" Text="Update" />
                                <c1:C1ButtonExtender ID="BtnUpdateRange_C1ButtonExtender" runat="server" TargetControlID="BtnUpdateRange">
                                </c1:C1ButtonExtender>
                            </div>--%>
                        </div>
                        <div class="clear">
                        </div>
                        <%--<p>
                            <em>Data will only be displayed for a maximum range of 6 months.</em></p>--%>
                    </fieldset>
                </div>
                
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="statusbar">
        <div class="progress" style="display: none">
            <c1:C1ProgressBar ID="C1ProgressBar1" runat="server" UseEmbeddedjQuery="false" AnimationDelay="0" AnimationOptions-Duration="800" Height="20px" Width="920px" LabelAlign="Center" />
        </div>
        <c1:C1BarChart ID="C1BarChart1" runat="server" Culture="en-US" 
            DataSourceID="ObjectDataSource1" Height="500px" Width="1100px">
            <Hint>
			    <Content Function="hintContent" />
		    </Hint>
            <Animation Enabled=true Duration=1000  Easing=EaseOutElastic />
            <Footer Compass="South" Visible="False"></Footer>

            <Axis>
                    <X Text="Total Gallons">
                        <Labels>
                            <Style FontSize="14">
                            </Style>

            <AxisLabelStyle FontSize="14"></AxisLabelStyle>
                        </Labels>

            <GridMajor Visible="True"></GridMajor>

            <GridMinor Visible="False"></GridMinor>
                    </X>
                    
            <Y Visible="False" Compass="West">
            <Labels TextAlign="Center"></Labels>

            <GridMajor Visible="True"></GridMajor>

            <GridMinor Visible="False"></GridMinor>
            </Y>
                    
            </Axis>
            
            <SeriesStyles>
                <c1:ChartStyle>
                    <Fill Color="#3399FF" ColorBegin="#0099CC" ColorEnd="#3399FF">
                    </Fill>
                </c1:ChartStyle>
                
            </SeriesStyles>
            
            <Header Text="Total Equipment Water Usage (GAL)"></Header>
            <ChartLabelStyle FontSize="17"  />
            <DataBindings>
                <c1:C1ChartBinding XField="category" XFieldType="String" YField="gallons" YFieldType="Number"  />
            </DataBindings>
        </c1:C1BarChart>
    </div>
    <script type="text/javascript">

        
        function hintContent() {
            return Globalize.format(this.y, "c");
        }


        
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(initializeRequest);
            prm.add_beginRequest(beginRequest);
            prm.add_endRequest(endRequest);


        });
        function initializeRequest(sender, args) {
            $(".progress").show();
            $("#<%=C1ProgressBar1.ClientID %>").c1progressbar("option", "value", 25);
        }
        function beginRequest(sender, args) {
            $("#<%=C1ProgressBar1.ClientID %>").c1progressbar("option", "animationOptions", { duration: 2500 });
            $("#<%=C1ProgressBar1.ClientID %>").c1progressbar("option", "value", 80);
        }
        function endRequest(sender, args) {
            $("#<%=C1ProgressBar1.ClientID %>").c1progressbar("option", "animationOptions", { duration: 400 });
            $("#<%=C1ProgressBar1.ClientID %>").c1progressbar("option", "value", 100);
            window.setTimeout(resetIt, 500);
        }
        function resetIt() {
            $("#<%=C1ProgressBar1.ClientID %>").c1progressbar("option", "value", 0);
            $(".progress").hide();
        }
    </script>
    
    
    
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="getsummedwater" TypeName="core.dtaker">
        <SelectParameters>
            <asp:ControlParameter ControlID="TxtDateFrom" Name="fromdate" PropertyName="Date" 
                Type="DateTime" DefaultValue="2014-01-23 21:28:00" />
            <asp:ControlParameter ControlID="TxtDateTo" DefaultValue="2014-01-29 12:52:00" 
                Name="ToDate" PropertyName="Date" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Button ID="Button1" runat="server" Height="30px" Text="DRAW CHART" 
                                   Width="130px" />
    
    
   
    
</asp:Content>

