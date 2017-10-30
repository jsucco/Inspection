<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="gaschart.aspx.vb" Inherits="core.UTILITIES_elecchart" %>

<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Chart" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1ProgressBar" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Input" TagPrefix="c1" %>


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
                                        Date="02/04/2014 21:28:00" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <c1:C1InputDate ID="TxtDateTo" runat="server" CssClass="left" 
                                        ShowTrigger="true" Date="02/09/2014 12:52:00" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    
                                </div>
                            </div>
                            <div class="third">
                                <div class="left">
                                </div>
                            </div>
                           
                        </div>
                        <div class="clear">
                        </div>
                        <%--<p>
                            <em>Data will only be displayed for a maximum range of 6 months.</em></p>--%>
                    </fieldset>
                </div>
                
                
                <c1:C1LineChart ID="C1WebChart1" runat="server" DataSourceID="ObjectDataSource1" 
                    Height="500px" Width="1257px" ShowChartLabels="False" Culture="en-US" Type=Area BackColor="#262626">
                    <Animation Direction = "Vertical"/>
                    <SeriesTransition Duration="2000" />
                    
                    <Animation Duration="2000" />
                    <SeriesStyles>
                        <c1:ChartStyle Stroke="#4B6C9E" StrokeWidth="4" Opacity="0.8">
                        </c1:ChartStyle>
                    </SeriesStyles>
                    <SeriesHoverStyles>
                        <c1:ChartStyle Stroke="#4B6C9E" StrokeWidth="6" Opacity="0.9">
                        </c1:ChartStyle>
                    </SeriesHoverStyles>
                    <Header Compass="North" Text="Main Natural Gas - Use (CCF/Hour)">
                        <TextStyle FontSize="24" FontFamily="Arial rounded MT Bold" Fill-Color="#DBDBDB"></TextStyle>
                    </Header>
                    <Footer Compass="South" Visible="False">
                    </Footer>
                    <Legend Text="LEGEND">
                         <TextStyle FontSize="18" FontFamily="Arial rounded MT Bold" Fill-Color="#DBDBDB"></TextStyle>
                    </Legend>
                    <Axis>
                        <X AutoMax=true AutoMin=true Text="DATE" >
                            <TextStyle FontSize="18" FontFamily="Arial rounded MT Bold" Fill-Color="#DBDBDB"></TextStyle>
                            <Labels>
                                <Style FontSize="14" Fill-Color="#DBDBDB">
                                    
                                </Style>
                            </Labels>
                            <GridMajor Visible="True">
                            
                            </GridMajor>
                            <GridMinor Visible="False">
                            </GridMinor>
                        </X>
                        <Y Compass="West" Visible="False" Text="CCF PER HOUR">
                            <TextStyle FontSize="18" FontFamily="Arial rounded MT Bold" Fill-Color="#DBDBDB"></TextStyle>
                            <Labels TextAlign=Center>
                                <Style FontSize="14" Fill-Color="#DBDBDB">
                                    
                                </Style>
                            </Labels>
                            <GridMajor Visible="True">
                            </GridMajor>
                            <GridMinor Visible="False">
                            </GridMinor>
                        </Y>
                    </Axis>
                    <DataBindings>
                        <c1:C1ChartBinding XField="TimeStamp" XFieldType="DateTime" YField="Main_Gas_Usage" YFieldType="Number"  />
                        
                    </DataBindings>
                </c1:C1LineChart>
                <asp:Button ID="Button1" runat="server" Height="30px" Text="DRAW CHART" 
                                   Width="130px" onclick="Button1_Click" />

        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="statusbar">
        <div class="progress" style="display: none">
            <c1:C1ProgressBar ID="C1ProgressBar1" runat="server" UseEmbeddedjQuery="false" AnimationDelay="0" AnimationOptions-Duration="800" Height="20px" Width="920px" LabelAlign="Center" />
        </div>
</div>

<script type="text/javascript">


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


    function hintContent() {
        return Globalize.format(this.y, "c");
    }



    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_beginRequest(beginRequest);
        prm.add_endRequest(endRequest);


    });

    </script>

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="getmgasinterval" TypeName="core.dtaker">
        <SelectParameters>
            <asp:ControlParameter ControlID="TxtDateFrom" Name="fromdate" PropertyName="Date" 
                Type="DateTime" DefaultValue="2014-02-04 21:28:00" />
            <asp:ControlParameter ControlID="TxtDateTo" DefaultValue="2014-02-09 12:52:00" 
                Name="ToDate" PropertyName="Date" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>


</asp:Content>


