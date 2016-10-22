<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="TrainingVideo.aspx.vb" Inherits="core.Training_TrainingVideo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <head>
<!--jQuery References--> 
<script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
<script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>

<!--Theme-->
<link href="http://cdn.wijmo.com/themes/aristo/jquery-wijmo.css" rel="stylesheet" type="text/css" />

<!--Wijmo Widgets CSS-->
<link href="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.css" rel="stylesheet" type="text/css" />

<!--Wijmo Widgets JavaScript-->
<script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.js" type="text/javascript"></script>
<script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20141.34.js" type="text/javascript"></script>
 
<!--Knockout JS Library-->
<script src="http://cdn.wijmo.com/wijmo/external/knockout-2.2.0.js" type="text/javascript"></script>
  
<!--Wijmo Knockout Integration Library-->
<script src="http://cdn.wijmo.com/interop/knockout.wijmo.3.20141.34.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $('#vid1').wijvideo(
            {
                fullScreenButtonVisible: true,
                showControlsOnHover: true
            });
    });
</script>




</head>
<body>
<video controls="controls" id="vid1" width="400" height="275">
        <source src="http://coreroute_test.standardtextile.com/Training/VID-20130220-00001.ogv" type='video/ogg; codecs="theora, vorbis"'>
        <source src="http://coreroute_test.standardtextile.com/Training/VID-20130220-00001.m4v" type='video/mp4; codecs="avc1.42E01E, mp4a.40.2"'>
<%--        <source src="~/Training/VID-20130220-00001.ogv" type='video/ogg; codecs="theora, vorbis"'>
        <source src="~/Training/VID-20130220-00001.m4v" type='video/mp4; codecs="avc1.42E01E, mp4a.40.2"'>--%>
<%--        <source src="http://cdn.wijmo.com/movies/wijmo.theora.ogv" type='video/ogg; codecs="theora, vorbis"'>
        <source src="http://cdn.wijmo.com/movies/wijmo.mp4video.mp4" type='video/mp4; codecs="avc1.42E01E, mp4a.40.2"'>--%>


        HTML5 is required to see this video.
</video>



</body>





  
   





</asp:Content>


