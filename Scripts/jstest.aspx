<%@ Page Title="" Language="VB" MasterPageFile="~/Scripts/corev2.master" AutoEventWireup="false" CodeFile="jstest.aspx.vb" Inherits="core.UTILITIES_chartmaster" %>

<%@ Register assembly="C1.Web.Wijmo.Controls.4" namespace="C1.Web.Wijmo.Controls.C1Chart" tagprefix="wijmo" %>

<%@ Register assembly="C1.Web.iPhone.4" namespace="C1.Web.iPhone.C1SwitchButton" tagprefix="C1SwitchButton" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server" class="container">
    
   
    <div id="MenuButtonContainer" style="position:relative">
    <input id="MenuButton1" type="button" value="" class="showmore"/>
    <div id="dropdown" class="redirect">
        <%--<li>option 1</li>
        <li>option 2</li>
        <li>option 3</li>
        <li>option 4</li>--%>
        
        <input id="CONTROLTEX" type="text" value="CONTROLTEX" class="redirect" />
        <input id="WSI" type="text" value="W.S.I." />
    </div>
    </div>
    <div id="MenuButtonContainer2">
    <input id="MenuButton2" type="button" value="" class="showmore2"/>
    <div id="dropdown2" class="redirect2">
        <%--<li>option 1</li>
        <li>option 2</li>
        <li>option 3</li>
        <li>option 4</li>--%>
        
       
    </div>
    </div>
    <div id="MenuButtonContainer3">
    <input id="MenuButton3" type="button" value="" class="showmore3"/>
    <div id="dropdown3" class="redirect3">
        <input id="WATER" type="text" value="WATER" class="redirect3" />
        <input id="ELECTRIC" type="text" value="ELECTRIC" />
        <input id="GAS" type="text" value="GAS" />
        <input id="EFFLUENT" type="text"/>
    </div>
    </div>
    <div id="MenuButtonContainer4">
    <input id="MenuButton4" type="button" value="" class="showmore4"/>
    <div id="dropdown4" class="redirect4">
        <%--<li>option 1</li>
        <li>option 2</li>
        <li>option 3</li>
        <li>option 4</li>--%>

    </div>
    </div>



    <script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20133.29.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20133.29.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20133.29.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/wijmo/external/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/interop/knockout.wijmo.3.20133.29.js" type="text/javascript"></script>


    <script type="text/javascript" language="javascript">
        var machnames = new Array("<%= machinenames %>");
        var testname = new String("<%= teststring %>");
        var equipmentArray = [<%= equipmentArray %>];
        alert(equipmentArray[4]);
//        var minDate = new Date("<%= minDate %>");
//        var maxDate = new Date("<%= maxDate %>");
//        var minTimestamp = minDate.getTime();
//        var maxTimestamp = maxDate.getTime();
//        var selectedMax;
//        var selectedMin;

//        function ajaxCall() {
//            var json;
//            var timestamp = new Date().getTime();
//            $.get("/handlers/chartdata.ashx", { timestamp: timestamp, startDate: "", endDate: "" }, function (retVal) {
//                if (retVal != "error") {
//                    json = $.parseJSON(retVal);
//                }
//            });
//        }
     
//        $('#MenuButtonContainer').hide().fadeIn(2000);
          $(this.document).ready(function () {

              // not let it make post back
              event.preventDefault();

              $('#MenuButtonContainer').hide().fadeIn(1400);
              $('#MenuButtonContainer2').hide().fadeIn(2000);
              $('#MenuButtonContainer3').hide().fadeIn(2500);
              $('#MenuButtonContainer4').hide().fadeIn(3000);
       
          });


        
//        Create javascript handler names chartdata in the handlers folder.  Use the slider function below to get selected values and pass to start and end date. use the json array and set as datasource for chart.  set hanlders equal to click event.
        $('.showmore').on('click', function () {
            if ($("#dropdown").is(":visible")) {
                $('#dropdown').slideUp("slow");
            } else {
                $('#dropdown').slideDown("slow");
            }
        });
        $('.showmore3').on('click', function () {
           // $('#EFFLUENT').val(machnames[1].toString());
            $('#EFFLUENT').val(equipmentArray[4]);
            if ($("#dropdown3").is(":visible")) {
                $('#dropdown3').slideUp("slow");
            } else {
                $('#dropdown3').slideDown("slow");
            }
        });
        $('.redirect').on('click', function () {
            $("#MenuButtonContainer").fadeOut('slow');
            $("#MenuButtonContainer2").fadeOut('slow');
            $("#MenuButtonContainer3").fadeOut('slow');
            $("#MenuButtonContainer4").fadeOut('slow');
            window.location.replace("SYSTEMS/systems.aspx")
        });
        $('.redirect3').on('click', function () {
            $("#MenuButtonContainer").fadeOut('slow');
            $("#MenuButtonContainer2").fadeOut('slow');
            $("#MenuButtonContainer3").fadeOut('slow');
            $("#MenuButtonContainer4").fadeOut('slow');
            window.location.replace("UTILITIES/Charts.aspx")
        });



//        $("#slider").wijslider({
//            range: true,
//            min: minTimestamp,
//            max: maxTimestamp,
//            step: 100000,
//            stop: function (e) {
//                var values = $("#slider").wijslider("values");
//                selectedMin = values[0];
//                selectedMax = values[1];

//                var temp = new Date(selectedMin);
//                var temp2 = new Date(selectedMax);

//                //alert("min: " + (temp.getMonth() + 1) + "/" + temp.getDate() + "/" + temp.getFullYear() + "\nmax: " + (temp2.getMonth() + 1) + "/" + temp2.getDate() + "/" + temp2.getFullYear())
//                $("#Text1").val((temp.getMonth() + 1) + "/" + temp.getDate() + "/" + temp.getFullYear() + " " + temp.getHours() + ":00:00");

//            },
//            values: [(minTimestamp + 10000), (maxTimestamp - 10000)]
//        });

//        function C1Slider1_OnClientButtonClick(sender, eventargs) {
//            

//            var slider = document.getElementsByClassName("c1Slider")[0];

//            slider.setAttribute("", minTimestamp);
//            slider.setAttribute("", maxTimestamp);

//            var date = new Date(dates[0][2]);
//            var timestamp = date.getTime();

            //alert(dates[0][2].length);
//            alert("min: " + minTimestamp + "\nmax: " + maxTimestamp);
//            document.getElementById("Text1").value = "min: " + minTimestamp + ", max: " + maxTimestamp;
//        
//        };
//        function Text1_OnChange() {
//            var val = parseInt(document.getElementById("Text1").value);
//        };
//        function DoClick(sender, eventargs) {
//            document.getElementById("Text1").value = "test"
//        };

</script> 
 





</asp:Content>

