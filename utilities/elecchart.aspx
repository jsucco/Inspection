<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="elecchart.aspx.vb" Inherits="core.UTILITIES_elecchart" %>

<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Slider"
    TagPrefix="c1" %>

<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Chart" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1ProgressBar" TagPrefix="c1" %>
<%@ Register Assembly="C1.Web.Wijmo.Controls.4" Namespace="C1.Web.Wijmo.Controls.C1Input" TagPrefix="c1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="chart">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <input type="button" onclick="AJAXSendIntArray()" value="Send Int array" />
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
                
                
                <c1:C1LineChart ID="C1WebChart1" runat="server" 
                    Height="500px" Width="1257px" ShowChartLabels="False" Culture="en-US" Type=Area BackColor="#262626">
                    
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
                    <Header Compass="North" Text="Main Electric Power - Instantaneous Use (kW)">
                        <TextStyle FontSize="24" FontFamily="Arial rounded MT Bold" Fill-Color="#DBDBDB"></TextStyle>
                    </Header>
                    <Footer Compass="South" Visible="False">
                    </Footer>
                    <Legend Text="LEGEND">
                        <TextStyle FontSize="18" FontFamily="Arial rounded MT Bold" Fill-Color="#DBDBDB"></TextStyle>
                    </Legend>
                    <Axis X-AutoMajor="True" X-Min="0" X-Max="0" X-UnitMajor="0">
                        <X  AutoMax=true AutoMin=true TickMajor-Factor=2 AutoMajor=true text="Test" >
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
                        <Y Compass="West" Visible="False" Text="DeltaT">
                            <TextStyle FontSize="18" FontFamily="Arial rounded MT Bold" Fill-Color="#DBDBDB"></TextStyle>
                            <Labels TextAlign=Center>
                                <Style FontSize="14" Fill-Color="#DBDBDB">
                                    
                                </Style>
                            </Labels>
                            <GridMajor Visible="False">
                            </GridMajor>
                            <GridMinor Visible="False">
                            </GridMinor>
                        </Y>
                    </Axis>
                    
                </c1:C1LineChart>
            <div id="scrollbar">
                <c1:C1Slider ID="C1Slider1" runat="server" Width="1257px" 
                    onclientbuttonclick="C1Slider1_OnClientButtonClick" Value="50" Height="25px" />
            </div>
            <div id="scrolltag1">
            <input id="Text1" type="text" onchange="Text1_OnChange()"/> 
            </div>
            <div id="scrolltag2">
            <input id="Text2" type="text" /> 
            </div>
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
    var mydata = new Array();
    var time = new Array();
    var mydata1 = new Array();
    var time1 = new Array();

    var names = [];
    var prices = [];
    
   
    var minDate = new Date("<%= minDate %>");
    var maxDate = new Date("<%= maxDate %>");
    var minTimestamp = minDate.getTime();
    var maxTimestamp = maxDate.getTime();
    var selectedMax;
    var selectedMin;

    var minDate = new Date(mydata[1]);
    function AJAXSendIntArray() {

        var values = $("#<%=C1Slider1.ClientID%>").c1slider("values");

        
        $.ajax({
            url: 'Handler2.ashx',
            type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
            data: { method: 'getredraw', args: { date1: values[0], date2: values[1], field: 'time' }},
            success: function (data) {time1 = $.parseJSON(data);},
            error: function (a, b, c) {
                alert('error');
            } 
        });
        $.ajax({
            url: 'Handler2.ashx',
            type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
            data: { method: 'getredraw', args: { date1: values[0], date2: values[1], field: '1'} },
            success: callredraw,
            error: function (a, b, c) {
                alert('error');
            }
        });

        function callredraw(data) {
            mydata1 = $.parseJSON(data);
            //alert('callback' + ' :' + mydata1[1] + ":" + time1[1]);

            var myarray = new Array();
            var arraylength = time1.length;

            for (var i = 0; i < arraylength; i++) {

                var obj2f = new Date(time1[i]);

                myarray.push(obj2f);


            }



            $("#<%= C1WebChart1.ClientID %>").c1linechart("option", "seriesList", [
                    {
                        label: "DeltaT",
                        legendEntry: true,
                        fitType: "spline",
                        data: {
                            x: myarray,
                            y: mydata1
                        },
                        markers: {
                            visible: true,
                            type: "circle"
                        }
                    }
                ]);
             $("#<%= C1WebChart1.ClientID %>").c1linechart("redraw", "true");

        }

    }

    function Button1_Click() {
        alert(mydata[1])
    }

    
           
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
//        var prm = Sys.WebForms.PageRequestManager.getInstance();
//        prm.add_initializeRequest(initializeRequest);
//        prm.add_beginRequest(beginRequest);
//        prm.add_endRequest(endRequest);
        var mydata1 = new Array(6.38, 6.54, 6.44, 6.4, 6.36, 6.43, 6.43, 6.07, 6.33, 6.36, 6.65, 6.28, 6.15, 6.14, 5.99, 6.01, 6.12, 6.47, 5.91, 6.14, 6.16, 6.34, 6.36, 6.13, 6.06, 5.88, 5.9, 5.79, 5.61, 5.63, 5.66, 5.58, 5.6, 5.69, 5.6, 5.63, 5.56, 5.5, 5.35, 5.21, 5.27, 5.18, 5.13, 5.19, 5.14, 5.16, 5.15, 5.1, 5.12, 5.06, 5.07, 5.07, 5.11, 5.04, 5.14, 5.29, 5.17, 5.19, 5.25, 5.16, 5.14, 5.11, 5.04, 5, 4.91, 5.03, 4.94, 4.93, 4.82, 4.73, 4.69, 4.69, 4.61, 4.54, 4.67, 4.63, 4.66, 4.58, 4.58, 4.52, 4.57, 4.49, 4.37, 4.29, 4.28, 4.26, 4.25, 4.18, 4.13, 4.12, 4.12, 4.14, 4.07, 4.05, 4.07, 4.07, 4.08, 3.99, 4.05, 4.01, 3.98, 4, 3.93, 3.97, 3.95, 3.93, 3.93, 3.85, 3.81, 3.81, 3.84, 3.82, 3.79, 3.82, 3.82, 3.84, 3.8, 3.8, 3.78, 3.72);
        var time1 = new Array(new Date(2008, (11 - 1), 30), new Date(2008, (12 - 1), 9), new Date(2008, (12 - 1), 19), new Date(2008, (12 - 1), 29), new Date(2009, (1 - 1), 9), new Date(2009, (1 - 1), 19), new Date(2009, (1 - 1), 30), new Date(2009, (2 - 1), 9), new Date(2009, (2 - 1), 20), new Date(2009, (3 - 1), 2), new Date(2009, (3 - 1), 9), new Date(2009, (3 - 1), 18), new Date(2009, (3 - 1), 30), new Date(2009, (4 - 1), 15), new Date(2009, (5 - 1), 1), new Date(2009, (5 - 1), 16), new Date(2009, (6 - 1), 1), new Date(2009, (6 - 1), 15), new Date(2009, (7 - 1), 2), new Date(2009, (7 - 1), 14), new Date(2009, (8 - 1), 3), new Date(2009, (8 - 1), 16), new Date(2009, (8 - 1), 31), new Date(2009, (9 - 1), 14), new Date(2009, (10 - 1), 2), new Date(2009, (10 - 1), 16), new Date(2009, (11 - 1), 1), new Date(2009, (11 - 1), 15), new Date(2009, (12 - 1), 1), new Date(2009, (12 - 1), 15), new Date(2010, (1 - 1), 3), new Date(2010, (1 - 1), 15), new Date(2010, (2 - 1), 2), new Date(2010, (2 - 1), 15), new Date(2010, (3 - 1), 1), new Date(2010, (3 - 1), 15), new Date(2010, (4 - 1), 1), new Date(2010, (4 - 1), 15), new Date(2010, (4 - 1), 20), new Date(2010, (4 - 1), 27), new Date(2010, (5 - 1), 4), new Date(2010, (5 - 1), 11), new Date(2010, (5 - 1), 18), new Date(2010, (5 - 1), 26), new Date(2010, (6 - 1), 1), new Date(2010, (6 - 1), 9), new Date(2010, (6 - 1), 15), new Date(2010, (6 - 1), 22), new Date(2010, (6 - 1), 29), new Date(2010, (7 - 1), 6), new Date(2010, (7 - 1), 13), new Date(2010, (7 - 1), 20), new Date(2010, (7 - 1), 27), new Date(2010, (8 - 1), 3), new Date(2010, (8 - 1), 10), new Date(2010, (8 - 1), 17), new Date(2010, (8 - 1), 24), new Date(2010, (8 - 1), 31), new Date(2010, (9 - 1), 1), new Date(2010, (9 - 1), 7), new Date(2010, (9 - 1), 14), new Date(2010, (9 - 1), 21), new Date(2010, (9 - 1), 28), new Date(2010, (10 - 1), 5), new Date(2010, (10 - 1), 12), new Date(2010, (10 - 1), 19), new Date(2010, (10 - 1), 26), new Date(2010, (11 - 1), 1), new Date(2010, (11 - 1), 9), new Date(2010, (11 - 1), 16), new Date(2010, (11 - 1), 23), new Date(2010, (11 - 1), 30), new Date(2010, (12 - 1), 9), new Date(2010, (12 - 1), 14), new Date(2010, (12 - 1), 22), new Date(2010, (12 - 1), 29), new Date(2011, (1 - 1), 4), new Date(2011, (1 - 1), 12), new Date(2011, (1 - 1), 20), new Date(2011, (1 - 1), 27), new Date(2011, (1 - 1), 30), new Date(2011, (2 - 1), 1), new Date(2011, (2 - 1), 9), new Date(2011, (2 - 1), 15), new Date(2011, (2 - 1), 22), new Date(2011, (3 - 1), 3), new Date(2011, (3 - 1), 8), new Date(2011, (3 - 1), 15), new Date(2011, (3 - 1), 21), new Date(2011, (3 - 1), 28), new Date(2011, (4 - 1), 4), new Date(2011, (4 - 1), 11), new Date(2011, (4 - 1), 18), new Date(2011, (4 - 1), 25), new Date(2011, (5 - 1), 3), new Date(2011, (5 - 1), 9), new Date(2011, (5 - 1), 16), new Date(2011, (5 - 1), 23), new Date(2011, (5 - 1), 30), new Date(2011, (6 - 1), 6), new Date(2011, (6 - 1), 13), new Date(2011, (6 - 1), 21), new Date(2011, (6 - 1), 27), new Date(2011, (7 - 1), 4), new Date(2011, (7 - 1), 11), new Date(2011, (7 - 1), 18), new Date(2011, (7 - 1), 25), new Date(2011, (8 - 1), 1), new Date(2011, (8 - 1), 8), new Date(2011, (8 - 1), 15), new Date(2011, (8 - 1), 17), new Date(2011, (8 - 1), 22), new Date(2011, (8 - 1), 29), new Date(2011, (9 - 1), 5), new Date(2011, (9 - 1), 12), new Date(2011, (9 - 1), 19), new Date(2011, (9 - 1), 27), new Date(2011, (10 - 1), 3), new Date(2011, (10 - 1), 10), new Date(2011, (10 - 1), 17));

        $.ajax({
            url: 'Handler2.ashx',
            type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
            data: { method: 'gettime'
            },
            success: function (data) {
                time = $.parseJSON(data);



            },
            error: function (a, b, c) {
                alert('error');
            }
        });

        $.ajax({
            //            url: 'Handler2.ashx',
            url: 'handlers/ut_charts_handler.ashx',

            type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
            data: { method: 'chartcallback'
            },
            success: callback

            ,
            error: function (a, b, c) {
                alert('error');
            }
        });
        function callback(data) {
            mydata = $.parseJSON(data);
            alert(mydata[1] + "::" + time[1]);

            var myarray = new Array();
            var arraylength = time.length;

            for (var i = 0; i < arraylength; i++) {

                var obj2f = new Date(time[i]);

                myarray.push(obj2f);


            }

            $("#<%= C1WebChart1.ClientID %>").c1linechart({

                // setup chart visualization as area 
                type: "area",
                // disabling animation 
                animation: { enabled: false },
                // disablning chart points labels 
                showChartLabels: false,
                hint: {
                    content: function () {
                        return this.y + '%';
                    }
                },
                seriesList: [
                        {
                            label: "Top 10,000",
                            legendEntry: true,
                            fitType: "spline",
                            data: {
                                x: myarray,
                                y: mydata
                            }
                        }]
                        ,
                axis: {
                    y: {

                    },
                    x: {

                        annoFormatString: "MM/dd/yyyy hh:mm"
                    }
                }

            });


//            $("#<%= C1WebChart1.ClientID %>").c1linechart("option", "seriesList", [
//                    {
//                        label: "DeltaT",
//                        legendEntry: true,
//                        fitType: "spline",
//                        data: {
//                            x: time,
//                            y: mydata
//                        },
//                        markers: {
//                            visible: true,
//                            type: "circle"
//                        }
//                    }
//                ]);


        }
    });

    function C1Slider1_OnClientButtonClick(sender, eventargs) {
        alert('Hello');
        document.getElementById("Text1").value = $("#<%=C1Slider1.ClientID%>").c1slider("value");
    };
    function Text1_OnChange() {
        var val = parseInt(document.getElementById("Text1").value);
        $("#<%=C1Slider1.ClientID%>").c1slider("value", val);
    };

    $(function () {
        // Set slide event handler function
        $("#<%=C1Slider1.ClientID%>").c1slider({
            range: true,
            min: minTimestamp,
            max: maxTimestamp,
            step: 100000,
            slide: function (e) {
                var values = $("#<%=C1Slider1.ClientID%>").c1slider("values");
                selectedMin = values[0];
                selectedMax = values[1];
                alert('hello');
                var temp = new Date(selectedMin);
                var temp2 = new Date(selectedMax);
                document.getElementById("Text1").value = (temp.getMonth() + 1) + "/" + temp.getDate() + "/" + temp.getFullYear() + " " + temp.getHours() + ":00:00";
                document.getElementById("Text2").value = (temp2.getMonth() + 1) + "/" + temp2.getDate() + "/" + temp2.getFullYear() + " " + temp2.getHours() + ":00:00";
            },
            stop: AJAXSendIntArray
        });
    });


    </script>

  


</asp:Content>


