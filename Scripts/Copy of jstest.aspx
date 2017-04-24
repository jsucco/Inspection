<%@ Page Title="" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="Copy of jstest.aspx.vb" Inherits="core.UTILITIES_chartmaster" %>

<%@ Register assembly="C1.Web.Wijmo.Controls.4" namespace="C1.Web.Wijmo.Controls.C1Chart" tagprefix="wijmo" %>

<%@ Register assembly="C1.Web.iPhone.4" namespace="C1.Web.iPhone.C1SwitchButton" tagprefix="C1SwitchButton" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        #Text1
        {
            width: 198px;
        }
        #slider
        {
            height: 28px;
            width: 796px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="slider"></div>

    <div style="margin-top: 10px; margin-bottom: 10px;">
        
    </div>

    <input id="Text1" type="text" onchange="Text1_OnChange()" style="display: block; margin: 10px 0px;" />
    <div style="position:relative">
    <input id="MenuButton1" type="button" value="" class="showmore"/>
    <div id="dropdown" class="redirect"><ul>
        <%--<li>option 1</li>
        <li>option 2</li>
        <li>option 3</li>
        <li>option 4</li>--%>
        ControlTexT
    </ul></div>
    </div>
    

    <script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20133.29.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20133.29.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20133.29.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/wijmo/external/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/interop/knockout.wijmo.3.20133.29.js" type="text/javascript"></script>


    <script type="text/javascript" language="javascript">
        var minDate = new Date("<%= minDate %>");
        var maxDate = new Date("<%= maxDate %>");
        var minTimestamp = minDate.getTime();
        var maxTimestamp = maxDate.getTime();
        var selectedMax;
        var selectedMin;

        function ajaxCall() {
            var json;
            var timestamp = new Date().getTime();
            $.get("/handlers/chartdata.ashx", { timestamp: timestamp, startDate: "", endDate: "" }, function (retVal) {
                if (retVal != "error") {
                    json = $.parseJSON(retVal);
                }
            });
        }

//        Create javascript handler names chartdata in the handlers folder.  Use the slider function below to get selected values and pass to start and end date. use the json array and set as datasource for chart.  set hanlders equal to click event.
        $('.showmore').on('click', function () {
            if ($("#dropdown").is(":visible")) {
                $('#dropdown').slideUp("slow");
            } else {
                $('#dropdown').slideDown("slow");
            }
        });
        $('.redirect').on('click', function () {
            window.location.replace("SYSTEMS/systems.aspx")
        });

        $("#slider").wijslider({
            range: true,
            min: minTimestamp,
            max: maxTimestamp,
            step: 100000,
            stop: function (e) {
                var values = $("#slider").wijslider("values");
                selectedMin = values[0];
                selectedMax = values[1];

                var temp = new Date(selectedMin);
                var temp2 = new Date(selectedMax);

                //alert("min: " + (temp.getMonth() + 1) + "/" + temp.getDate() + "/" + temp.getFullYear() + "\nmax: " + (temp2.getMonth() + 1) + "/" + temp2.getDate() + "/" + temp2.getFullYear())
                $("#Text1").val((temp.getMonth() + 1) + "/" + temp.getDate() + "/" + temp.getFullYear() + " " + temp.getHours() + ":00:00");

            },
            values: [(minTimestamp + 10000), (maxTimestamp - 10000)]
        });

        function C1Slider1_OnClientButtonClick(sender, eventargs) {
            

//            var slider = document.getElementsByClassName("c1Slider")[0];

//            slider.setAttribute("", minTimestamp);
//            slider.setAttribute("", maxTimestamp);

//            var date = new Date(dates[0][2]);
//            var timestamp = date.getTime();

            //alert(dates[0][2].length);
            alert("min: " + minTimestamp + "\nmax: " + maxTimestamp);
            document.getElementById("Text1").value = "min: " + minTimestamp + ", max: " + maxTimestamp;
        
        };
        function Text1_OnChange() {
            var val = parseInt(document.getElementById("Text1").value);
        };
        function DoClick(sender, eventargs) {
            document.getElementById("Text1").value = "test"
        };

</script> 
 





</asp:Content>

