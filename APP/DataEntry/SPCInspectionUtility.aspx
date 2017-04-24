<%@ Page Title="APR" Language="VB" MasterPageFile="~/APP/MasterPage/Site.master" AutoEventWireup="false" CodeFile="SPCInspectionUtility.aspx.vb" Inherits="core.APP_DataEntry_SPCInspectionUtility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="SectionRibbon" style="position:absolute; left:385px; width:68%; height:100%;">
        <ul>
            <li><a href="#ButtonManagerTab">Button Manager</a></li>
            <li><a href="#SpecManagerTab">Spec Manager</a></li>
        </ul>
        <div id="ButtonManagerTab">
            <div id="tabs" style="width:92%; top:150px; height:75%; left:10px; position:absolute;">
                <ul> 
                            <li> 
                                <a href="#tabs-1"></a> 
                                <span class="ui-icon ui-icon-close">Remove Tab</span> 
                            </li> 
                        </ul> 
                        <div id="tabs-1"> 
                            <p> 
                                </p> 
                        </div> 

            </div>
            <div style="left: 30px; top: 80px; position:absolute; border-style: solid; border-bottom-color: #869EB1; width: 66%; border-top-color: white; border-right-color: white; border-left-color: white; height: 60px;">
                <input id="add" type="button" class="export" style="height: 40px; width:110px" value="ADD TAB" />
                <input id="addbutton" type="button" class="export" style="height: 40px; width:110px; position: absolute; top: 0px; left: 140px" value="ADD BUTTON" />
                <div id = "LineTypeDiv" class=SheetClass1 style="position:absolute; top:-9px; left: 270px; width:300px">LineType: 
                                        <select id="LineType_pop" name="LineType_pop" style="width: 162px; height: 35px; ">
                                            <option value="EOL">End Of Line</option>
                                            <option value="IL">InLine</option>
                                            <option value="ROLL">Roll</option>
                                        </select></div>
                <div style="position:relative; float:right; top:-30px; right:200px;">
                <input id="ActiveTemplateTitle" type="text" style="height: 40px; width:200px; position: absolute; top: -10px; border: none; left: 100px; font-family: initial;font-size: large;font-weight: 700;/* border: gray; */" value="">
               <asp:Button ID="TemplateSubmit" CssClass="export" style="position:absolute; left:320px; height: 40px; width:110px" Text ="SAVE" runat="server" />
                     </div>
            </div>
            <div id="loading" style="width:80%; top:170px; height:550px; left:48px; position:absolute;">
                <div style="width:100%; top:7px; height:100%; position:absolute;z-index:0; background-color:lightgray; opacity:.4;"></div>
                <input type="image" src="../../Images/load-indicator.gif" style="z-index:3; margin-left: 41%; margin-top:20%; position:absolute;" />
            </div>
            <div id="UserDirections" style="width:90%; top:170px; display: inline; height:550px; left:48px; position:absolute;">
                <div style="width:100%; top:7px; height:100%; position:absolute;z-index:0; background-color:lightgray; opacity:.4;"></div>
                <input type="image" src="../../Images/TemplateSubmission.jpg" style="z-index:2; margin-left: 2%; margin-top:2%; position:absolute; width: 97%; height: 91%;" />
            </div>
        </div>
        <div id="SpecManagerTab">
            <label id="Speclbl">SPEC Manager</label>
            <div id="ProductSpecs" style="position:absolute; top:75px;">
                <table id="Specgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; ">
                    </table>
                <div id="pSpecgrid" ></div>
            </div>
            <div id="SpecButton" style="position:absolute; top:35px; width: 360px; display:none;">
                <input id="SpecAdd" type="button" class="export" style="height: 30px; width:70px" value="ADD" runat="server" />
                <input type="text" id="SpecErrorText" style="position:relative; top: -25px; left: 80px;border: none;font-weight: 800;color: red;display: none; width:300px;">
            </div>

        </div>
    </div>
    <div style="Z-INDEX: 102; LEFT: -15px; POSITION: relative; TOP: 102px; width: 330px; border-right-color: #869EB1; border-style: solid; border-top-color: white; border-left-color: white; border-bottom-color: white; height: 250px;">
        <div id="tablehold" style="position:relative; Z-INDEX: 104; top: 0px; width: 285px; left: 0px;" >

                <table id="wijgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; ">
        
                </table>
        <div id="gridpager" style="Z-INDEX: 105; position:absolute; z-index: 105;" ></div>
                <input name="select-2" id="TemplateId" style="width: 142px; height: 20px; display:none;" class="inputelement" ></input>
                <%--<input id="submit" type="button" class="export" style="position:relative; top: 60px; height: 30px; width:142px" value="SUBMIT TEMPLATE" />--%>
                
                <input id="DefectType_add" type="button" class="export" style="position:relative; top: 75px; height: 30px; width:142px" value="MAINTAIN DEFECTS" />
                <%--<input id="edit" type="button" class="export" style="position:absolute; top: 100px; height: 30px; width:142px" value="MANAGE EXISTING TEMPLATES" />--%>
            
</div>        
    </div>
    

    <div id="dialog" title="Add tab" style="display:none; width: 330px;" class="dia" > 
                    <fieldset class="ui-helper-reset"> 
                        <div style="position:absolute;">
                            <label for="tab_title"> 
                                Title</label> 
                            <input type="text" name="tab_title" id="tab_title" value="" class="ui-widget-content ui-corner-all" /> 
                        </div>
                        <%--<div id = "SpecAdd" class=SheetClass1 style="position:absolute; top:-5px; left: 220px;">
                                        <input id="SpecAddValue" type="checkbox" class=chkbox runat=server />Add Spec's?</div>--%>
                       <%-- <div id="ProductSpecs" style="position:absolute; top:75px; width: 260px; display:none;">
                            <table id="Specgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; ">
                                </table>
                        
                            <div id="pSpecgrid" ></div>
                        </div>--%>
                        <%--<div id="SpecButton" style="position:absolute; top:35px; width: 360px; display:none;">
                            <input id="SpecAdd" type="button" class="export" style="height: 30px; width:70px" value="ADD" runat="server" />
                            <input type="text" id="SpecErrorText" style="position:relative; top: -25px; left: 80px;border: none;font-weight: 800;color: red;display: none; width:300px;">
                        </div>--%>
                    </fieldset> 
                </div> 
    
    <div id="DefectMaint_dialog" title="Maintain Defects" style="display:none; width:330px;">
        <fieldset class="ui-helper-reset">
            <div id="DefectTypes" style="position:absolute; top:5px; width: 300px;">
                            <table id="DefectTypesgrid" style=" font-size:smaller; Z-INDEX: 104; font-weight:800; ">
                                </table>
                        
                            <div id="pDefectTypesgrid" ></div>
                        
                        </div>
                    <div id="DefectTypesButton" style="position:relative; top:7px; width: 360px;">
                            
                            <input type="text" id="DefectErrorText" style="position:relative; top: -25px; left: 80px;border: none;font-weight: 800;color: red;display: none; width:300px;">
                        </div>
        </fieldset>
    </div>
    <div id="DefectType_dialog" title="Add Defect Type" style="width: auto; min-height: 36px; max-height: none; height: 70px; display:none;" class="dia" > 
                    
                    <fieldset class="ui-helper-reset"> 
                        
                        <label for="DefectType_Name"> 
                            Defect Code</label> 
                        <input type="text" name="DefectType_Name" id="DefectType_Name" value="" class="ui-widget-content ui-corner-all" /> 
                        <label style="position:absolute; top:40px; left: 12px;" for="DefectType_Text"> 
                            Defect Name Text</label> 
                        <input type="text" name="DefectType_Text" id="DefectType_Text" style="position:absolute; top:38px; left: 90px;" value="" class="ui-widget-content ui-corner-all" /> 
                        <label id="CodeError" style="position:relative; float:left; color:red;top: 45px; display:none;">CODE ALREADY EXISTS</label>
                    </fieldset> 
                </div> 
    <div id="dialog2" title="Add Button" style="display:none" class="dialogdiv"> 
            <div style="position:relative; float:right; border:2px solid #8f8f8f; border-radius:3px; left:-10px; width:140px; height: 31px;">
                <label style="position:absolute; left: 0px; top:4px;" for="DefectType">DefectType: </label> 
                   
                            <select id="DefectType" name="DefectType" style="position:absolute; left:70px; top:4px;">
                              <option value="Major">Major</option>
                              <option value="Minor">Minor</option>
                              <option value="Repairs">Repairs</option>
                              <option value="Scrap">Scrap</option>
                              <option value="Time">Time</option>
                              <option value="Upgrade">Upgrade</option>
                              <option value="Fix">Fix</option>
                            </select>
            </div>
            <div style="position:relative; float:right; left:-20px; border:2px solid #8f8f8f; border-radius:3px; width: 110px; height: 31px;">
                 <input type="checkbox" id="dialog2_Timer" name="Timer"> Enable Timer<br>
            </div>
                    <fieldset class="ui-helper-reset"> 
                        
                        <label for="tab_title"> 
                            Title</label> 
                        <div id="list" style="height:200px;"> 
                            </div>
                        
                    </fieldset> 
                </div> 
        <div id="dialog3" title="Edit Button" style="display:none" class="dialogdiv"> 
            <div id="EditDefectType_Div" style="position:relative; float:right;">
                <label style="position:absolute; left: -80px;" for="DefectType">DefectType</label> 
                   
                            <select id="EditDefectType" name="DefectType">
                              <option value="Major">Major</option>
                              <option value="Minor">Minor</option>
                              <option value="Repairs">Repairs</option>
                              <option value="Scrap">Scrap</option>
                              <option value="Time">Time</option>
                              <option value="Upgrade">Upgrade</option>
                              <option value="Fix">Fix</option>
                            </select>
            </div>
                    <fieldset class="ui-helper-reset"> 
                        <label for="tab_title"> 
                            Title</label> 
                        <div id="list2" style="height:200px;"> 
                            </div>
                        
                        
                    </fieldset> 
                </div> 
    <div id="dialog4" title="Add Template" style="display:none" class="dialogdiv"> 
                    <fieldset class="ui-helper-reset"> 
                        <div style="margin-bottom:10px;">
                            <label for="templatename"> 
                            Template Name</label> 
                            <input id="Name" name="templatename" style="height:30px;"> 
                        </div>
                        <div style="margin-bottom:10px;">
                            <label for="LineType_pop2">LineType:</label>
                        <select id="LineType_pop2" style="position:relative; left:34px;" name="lineType_pop2" ></select>
                        </div>
                        <div style="margin-bottom:10px;">
                            <label id="dialog4_error" style="color:red; font-weight:800;" ></label>
                        </div>
                        
                    </fieldset> 
                </div> 
    <input id="TemplateStatus" type="hidden" value="test123" runat="server" />
    <input id="TabArray_Hidden" type="hidden" value="0" runat="server" />
    <input id="ButtonArray_Hidden" type="hidden" value="0" runat="server" />
    <input id="TemplateId_Hidden" type ="hidden" value="0" runat="server" />
    <input id="LineType_Hidden" type ="hidden" value="FINAL AUDIT" runat="server" />

    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
        <script src="http://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
        <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.min.js" type="text/javascript"></script>

        <!--Theme-->
        <link href="http://cdn.wijmo.com/themes/arctic/jquery-wijmo.css" rel="stylesheet" type="text/css" />
    

    <!--Wijmo Widgets CSS-->
    <link href="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.css" rel="stylesheet" type="text/css" />


    <!--Wijmo Widgets JavaScript-->
    <script src="http://cdn.wijmo.com/jquery.wijmo-open.all.3.20141.34.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/jquery.wijmo-pro.all.3.20141.34.min.js" type="text/javascript"></script>
    <script src="http://cdn.wijmo.com/interop/wijmo.data.ajax.3.20141.34.js" type="text/javascript"></script>
    <link href="../../Styles/ui.jqgrid.css" rel="stylesheet" type="text/css" />
<%--    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css">--%>
    <style>
        .grid .ui-jqgrid-htable th,
        .grid .ui-jqgrid-btable .jqgrow td {
            height: 5em !important;
        }
        /*.ui-jqgrid .ui-jqgrid-titlebar {
        padding: .3em 16.4em .2em .3em;
        position: relative;
        font-size: 12px;
        border-left: 0 none;
        border-right: 0 none;
        border-top: 0 none;
        width: 100px !important;
        }*/

        .ui-corner-top {
        width: 130px !important;
        height: 57px;
        }
        .ui-jqgrid-view {
            background-color: #A3B4CD;
        }
    </style>
<script src="../../Scripts/jquery.layout.js" type="text/javascript"></script>
<script src="../../Scripts/grid.locale-en.js" type="text/javascript"></script>
<%--<script src="../../Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>--%>
<script src="../../Scripts/jquery.jqGrid.js" type="text/javascript"></script>
<script src="../../Scripts/select2.js"></script>
<link href="../../Scripts/select2.css" rel="stylesheet" type="text/css"></link>
<style type="text/css">
    .ui-corner-top {
        width:100px;
    }
    .ui-icon-close {
        margin-left:auto;
    }
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
    a {
        outline: none;
    }

</style>
<style>
    .nooutlineclass {
        outline:none;
    }

</style>
        
<script type="text/javascript">
    var buttonarray = new Array();
    var tabselect = 0;
    var UtilityOperation;
    var lastproductspec_sel;
    var GlobalTemplateId;
    var PostedTemplateId;
    var PostedName;
    var ButtonLibrary;
    var lastSel = 0;
    var lastsel2;
    var lastAddDefType = Number(0); 
    var GridManagerTable;
    var RenderSpecFlag = false;
    var ProductSpecifications = '';
    var InspectionTypesArray = <%=InspectionTypesArray%>;
    var UnsavedButtonCount = 0;
    $(document).ready(function () {
        var screenheight = $(window).width() - 64;
     

        $("a").addClass("nooutlineclass")
        $('.dialogdiv').css({ display: "inline" });
        $('.main').css({ height: $(window).height() + 300 + "px" });
        $('#SectionRibbon').css({ height: $(window).height() + 300 + "px" });
        datahandler.selectedtab = Number(0)
    });
    $(function () {
        var $tab_title_input = $('#tab_title'),
                    tab_counter = 2;
        var $button_title_input = $('#tab_title');
        var $DefectType_Name = $('#DefectType_Name');
        var $DefectType_Text = $('#DefectType_Text');
        var list = $("#list");
        var list2 = $("#list2");
        var buttoncount = buttonarray.length;
        var tabsobj = $('#tabs');
        var CodeErrorFree = true;
        var DefectTypesMaintLoadCount = 0; 
        
        var $sectiontabs = $('#SectionRibbon').wijtabs({
            select: function (e, args){ 

                if (args.index == 1 && RenderSpecFlag == false) { 
                    dbtrans.RenderProductSpecGrid();
                } 
            }
        });

        if (InspectionTypesArray) { 
            var ithtml = [];
            var ehtml = []; 

            $.each(InspectionTypesArray, function (k, value) { 

                ithtml.push('<option value="' + value.Name + '">' + value.Name + '</option>');
                ehtml.push('<option value="' + value.Name + '">' + value.Name + '</option>');
            });
            ithtml.push('<option value="" disabled selected>No Selection</option>'); 
            $("#LineType_pop").html(ithtml.join(''));
            $("#LineType_pop").prop('disabled', 'disabled'); 
            ehtml.push('<option value="" disabled selected>Select A LineType</option>')
            $("#LineType_pop2").html(ehtml.join(''));
        }

        var $tabs = $('#tabs').wijtabs({
            tabTemplate: '<li><a href="#{href}">#{label}</a> <span class="ui-icon ui-icon-close">Remove Tab</span></li>',
            add: function (event, ui) {
                var tab_content = 'Tab test content.';
               
                controlhandler.addbuttontotab(datahandler.tabbuttonarray.length, ui)
                if (datahandler.newtabflag == true) { datahandler.newtabflag = false }
            },
            select: function (e, args) {
                var tablength = $('#tabs').wijtabs("length");

                if (datahandler.newbuttonflag != true) {
                    datahandler.selectedtab = new Number(args.index)
                    
                    tabselect = args.index;

                }         
                
            }
            
        });
        
        var buttonarray2 = new Array();
        PostedTemplateId = <%=PostedTemplateId%>;
        ButtonLibrary = <%=DefectCodeList%>;
        PostedName = '<%=PostedName%>'
        dbtrans.GetGridManagerData();

        if (PostedTemplateId > 0) {
            $('#ActiveTemplateTitle').val(PostedName)
            GlobalTemplateId = PostedTemplateId;
            datahandler.selectedtemplateid = Number(PostedTemplateId)
            $("#MainContent_TemplateId_Hidden").val(PostedTemplateId);
            dbtrans.GetTemplateCollection()
        }
        datahandler.GetButtonLibrary();
        list.wijlist({
            selected: function (event, ui) {
                var selectedItem = ui.item;
                console.log(ui);
                //datahandler.selectedList.push({ id: selectedItem.id, label: selectedItem.label })
                datahandler.selectedList[0].id = selectedItem.id;
                datahandler.selectedList[0].label = selectedItem.label;
                datahandler.selectedList[0].DefectCode = selectedItem.value;

            }
        });
        list2.wijlist({
            selected: function (event, ui) {
                var selectedItem = ui.item;
                var str = selectedItem.label;
                
                datahandler.selectededit = str;
                datahandler.selectededitid = selectedItem.id;

            }
        });
        returnsValue = tabsobj.wijtabs("remove", 0);

        var $dialog = $('#dialog').wijdialog({
            showStatus: false,
            showControlBox: false,
            autoOpen: false,
            modal: true,
            width: new Number(400),
            height: new Number(350),
            captionButtons: {
                pin: { visible: false },
                refresh: { visible: false },
                toggle: { visible: false },
                minimize: { visible: false },
                maximize: { visible: false }
            },
            buttons: {
                'Add': function () {
                    UtilityOperation = "TabAddition";
                    var titleinput = $tab_title_input.val();
                    var existsflag = false; 
                    $.each(controlhandler.tabarray, function (index, value) { 
                        if (titleinput == value.title) { 
                            existsflag = true; 
                        }
                    }); 

                    if (existsflag == true) { alert("A Tab already exists with the same name"); $(this).wijdialog('close'); return } 
                    if (datahandler.selectedtemplateid == 1 || datahandler.selectedtemplateid == null) { alert("Adding a Tab Requires Selecting a Template"); $(this).wijdialog('close'); return }
                    if (controlhandler.tabarray.length < 9) {
                        var UserDiv = $('#UserDirections');
                        
                        if (UserDiv.is(':visible') == true) { UserDiv.toggle() }
                        controlhandler.tabarray.push({ title: $tab_title_input.val() });
                        controlhandler.addTab($tab_title_input.val());
                        var idarray = $('#Specgrid').jqGrid('getDataIDs');
                        $.each(idarray, function (index, value) {
                            jQuery('#Specgrid').jqGrid('saveRow', value, true);
                            var rowdata = $('#Specgrid').jqGrid('getLocalRow', value);
                            if (rowdata.Spec_Description == "" && rowdata.Upper_Spec_Value == "" && rowdata.Lower_Spec_Value == "") {
                                $('#Specgrid').jqGrid('delRowData', value);
                            }

                        });
                        
                        <%--var TabJsonString = JSON.stringify(controlhandler.tabarray);
                        var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
                     
                       
                        $.when(
                            
                            $.ajax({
                                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                                type: 'GET',  
                                data: { method: 'SubmitTemplate', args: { TemplateId: datahandler.selectedtemplateid, TabArray: TabJsonString, ButtonArray: ButtonJsonString } },
                                success: function (data) {
                     
                                    alert("Tab Successfully Saved");
                                },
                                error: function (a, b, c) {
                                    console.log(a);
                                    return false
                                }

                            })
                                ).done(function (a1) {
                                    console.log(titleinput);
                                    console.log('whencomplete');
                                    //dbtrans.UpdateSpecTable(titleinput);
                            });--%>
                        
                    } else {
                        alert("Tab Limit Reached")
                    }
                    $(this).wijdialog('close');
                },
                'Cancel': function () {
                    $(this).wijdialog('close');
                }
            },
            open: function () {
                dbtrans.RenderProductSpecGrid();
 
                $("#MainContent_SpecAddValue").change(function () {
                    $("#ProductSpecs").toggle();
                    $("#SpecButton").toggle();
                });

                $('#MainContent_SpecAdd').click(function () {
           
                    var idarray = $('#Specgrid').jqGrid('getDataIDs');
                    $.each(idarray, function (index, value) {

                        var errorflag = 0;
                        var rowdata = $('#Specgrid').jqGrid('getLocalRow', value);
                        jQuery('#Specgrid').jqGrid('saveRow', value, true);

                        if (rowdata.Spec_Description == "") {
                            $("#SpecErrorText").val('Spec Description Required: Do it Again');
                            errorflag = 1;
                        }
                        if (isNumber(rowdata.value) == false && errorflag == 0) {
                            $("#SpecErrorText").val('Number required at value: Do it Again');
                            errorflag = 1;
                        }
                        if (isNumber(rowdata.Upper_Spec_Value) == false && errorflag == 0) {
                            $("#SpecErrorText").val('Number required at Upper Value: Do it Again');
                            errorflag = 1;
                        }
                        if (isNumber(rowdata.Lower_Spec_Value) == false && errorflag == 0) {
                            $("#SpecErrorText").val('Number required at Lower Value: Do it Again');
                            errorflag = 1;
                        }
                        if (errorflag == 0) {
                            var uppval = new Number(rowdata.Upper_Spec_Value);
                            var lowval = new Number(rowdata.Lower_Spec_Value);
                            if (lowval > uppval) {
                                $("#SpecErrorText").val('Need UppVal > LowVal: Do it Again');
                                errorflag = 1;
                            }
                        }

                        $('#SpecErrorText').toggle();
                        window.setTimeout(function () { $('#SpecErrorText').toggle(); }, 3000);
                        if (errorflag == 1) {
                            $('#Specgrid').jqGrid('delRowData', value);
                        }

                    });
                    $("#Specgrid").restoreRow(lastproductspec_sel);
                    $("#Specgrid").addRow(lastproductspec_sel, true);
                  
                });

                $tab_title_input.focus();

            },
            close: function () {
                $form.find('input').val("").end();
                var TabJsonString = JSON.stringify(controlhandler.tabarray);
                var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
             
                if (TabJsonString.length > 0 && ButtonJsonString.length > 0) {
                    $("#MainContent_TabArray_Hidden").val(TabJsonString);
                    $("#MainContent_ButtonArray_Hidden").val(ButtonJsonString);
                }
                var url;
                url = "<%=Session("BaseUri")%>" + '/APP/DataEntry/SPCInspectionUtility.aspx?prTid=' + GlobalTemplateId;
                //window.location.assign(url);
                
                
            }
        });

        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        var $DefectType_dialog = $('#DefectMaint_dialog').wijdialog({
            showStatus: false,
            showControlBox: false,
            autoOpen: false,
            modal: true,
            captionButtons: {
                pin: { visible: false },
                refresh: { visible: false },
                toggle: { visible: false },
                minimize: { visible: false },
                maximize: { visible: false }
            },
            height: "550",
            width: "380",
            buttons: {
                'Add': function (e) {
                    var idarray = $('#DefectTypesgrid').jqGrid('getDataIDs');
                   
                    var newtimestamp = Number(e.timeStamp) - Number(lastAddDefType);
                    lastAddDefType = e.timeStamp; 

                    if (CodeErrorFree == true) { 
                        if (newtimestamp == 0 || newtimestamp > 400) { 
                        $('#DefectTypesgrid').addRow('new_row', true);
                        }
                        //datahandler.AddDefectType($DefectType_Name.val(), $DefectType_Text.val());
                    } else { 
                        $("#CodeError").toggle();
                        setTimeout(function() { $("#CodeError").toggle(); },2000);
                    }
                    
                },
                'Close': function () {
                    var idarray = $('#DefectTypesgrid').jqGrid('getDataIDs');
                    var counter = 1; 
                    console.log(idarray); 
                    $.each(idarray, function (index, value) {

                        var find = value.indexOf("jqg")
                        var nameLength = $("#" + value + "_Name").val();


                        if (find != -1  && nameLength != null) { 
                            $('#DefectTypesgrid').jqGrid('saveRow', value, true, null, null, "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility_DefTyp.ashx', {}, datahandler.GetButtonLibrary() );
                        }
                           
                    });
                    window.setTimeout(function () { datahandler.GetButtonLibraryGrid(); datahandler.GetButtonLibrary() }, 1000);
                    
                    $(this).wijdialog('close');
                }
            },
            open: function () {
                $tab_title_input.focus();

                DefectTypesMaintLoadCount =  ++DefectTypesMaintLoadCount; 
                if (DefectTypesMaintLoadCount = 1) { 
                dbtrans.RenderDefectTypesGrid();
                } else if (DefectTypesMaintLoadCount > 1) { 
                    dbtrans.RefreshDefectMaint()
                }
                var $deft = $("#DefectTypesgrid");

                $("#DefectSave").click(function() { 

                    var idarray = $deft.jqGrid('getDataIDs');
                    $.each(idarray, function (index, value) {

                        var errorflag = 0;
                        var rowdata = $deft.jqGrid('getLocalRow', value);
                        
                        if (rowdata.Name == "") {
                            $("#DefectErrorText").val('Name Required');
                            errorflag = 1;
                        }
                        $('#SpecErrorText').toggle();
                        window.setTimeout(function () { $('#DefectErrorText').toggle(); }, 3000);
                        if (errorflag == 1) {
                            $deft.jqGrid('delRowData', value);
                        } else { 
                            $deft.jqGrid('saveRow', value, true);
                        }
                    });
                });

            },
            close: function () {
                $form.find('input').val("").end();
                setTimeout(function () { dbtrans.RefreshManagerGrid();}, 1000);
            }
        });
        var $dialog2 = $('#dialog2').wijdialog({
            showStatus: false,
            showControlBox: false,
            autoOpen: false,
            modal: true,
            captionButtons: {
                pin: { visible: false },
                refresh: { visible: false },
                toggle: { visible: false },
                minimize: { visible: false },
                maximize: { visible: false }
            },
            buttons: {
                'Add': function () {
                    if (controlhandler.tabarray.length > 0) {
                        var editdefectypeval = $("#DefectType").val();
                        var TimerFlag = $("#dialog2_Timer").prop('checked');
                        GlobalTimerFlag = TimerFlag; 
                        var arrayval = "1";
                        if (editdefectypeval == "Minor") { 
                            arrayval = "0";
                        } else if (editdefectypeval == "Repairs") { 
                            arrayval = "repairs";
                        } else if (editdefectypeval == "Scrap") { 
                            arrayval = "scrap";
                        } else if (editdefectypeval == "Time") { 
                            arrayval = "Time";
                        } else if (editdefectypeval == "Upgrade") { 
                            arrayval = "Upgrade";
                        } else if (editdefectypeval == "Fix") { 
                            arrayval = "Fix";
                        }

                        datahandler.tabbuttonarray.push({ text: datahandler.selectedList[0].label, tabindex: datahandler.selectedtab, id: datahandler.buttoncount, ButtonId: datahandler.selectedList[0].id, DefectType: arrayval, DefectCode: datahandler.selectedList[0].DefectCode, id: -1, Hide: false, ButtonLibraryId: datahandler.selectedList[0].id, Timer: TimerFlag });
                        UtilityOperation = "ButtonAddition";
                        var height = $(window).height()
                        $('.main').css({ height: height + 210 + "px" });
                        $('#SectionRibbon').css({ height: height + 210 + "px" });
                       

                        controlhandler.addbutton();
                    } else {
                        alert('Adding a button requires a Tab');
                    }
                    $(this).wijdialog('close');
                },
                'Cancel': function () {
                    $(this).wijdialog('close');
                }
            },
            open: function () {
                $button_title_input.focus();
       
                list.wijlist('setItems', datahandler.buttonarray);
                list.wijlist('renderList');
                list.wijlist('refreshSuperPanel');

            },
            close: function () {
                var TabJsonString = JSON.stringify(controlhandler.tabarray);
                var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
                if (TabJsonString.length != 0 && ButtonJsonString.length != 0) {
                    $("#MainContent_TabArray_Hidden").val(TabJsonString);
                    $("#MainContent_ButtonArray_Hidden").val(ButtonJsonString);
                }

                $form.find('input').val("").end();

            }
        });
        var $dialog3 = $('#dialog3').wijdialog({
            showStatus: false,
            showControlBox: false,
            autoOpen: false,
            modal: true,
            captionButtons: {
                pin: { visible: false },
                refresh: { visible: false },
                toggle: { visible: false },
                minimize: { visible: false },
                maximize: { visible: false }
            },
            buttons: {
                'Edit': function () {
                    datahandler.editbuttons('edit')
                    UtilityOperation = "Edit"
                    var TabJsonString = JSON.stringify(controlhandler.tabarray);
                    var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
                    if (TabJsonString != "" && ButtonJsonString !="") {
                        $("#MainContent_TabArray_Hidden").val(TabJsonString);
                        $("#MainContent_ButtonArray_Hidden").val(ButtonJsonString);
                    }
                    $(this).wijdialog('close');
                },
                'hide/unhide': function () {
                    datahandler.newbuttonflag = true;
                    UtilityOperation = "Delete";
                    datahandler.editbuttons('delete')
                    var TabJsonString = JSON.stringify(controlhandler.tabarray);
                    var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
                    if (TabJsonString.length > 0 && ButtonJsonString.length > 0) {
                        $("#MainContent_TabArray_Hidden").val(TabJsonString);
                        $("#MainContent_ButtonArray_Hidden").val(ButtonJsonString);
                    };
                    $(this).wijdialog('close');
                }
            },
            open: function () {
                $button_title_input.focus();
                
                list2.wijlist('setItems', datahandler.buttonarray);
                list2.wijlist('renderList');
                list2.wijlist('refreshSuperPanel');
                
                if (datahandler.tabbuttonarray.length > 0) {
                    for (i = 0; i < datahandler.tabbuttonarray.length; i++) {

                        var buttonid = 'button' + datahandler.tabbuttonarray[i].id.toString()
                        
                        if (buttonid == datahandler.editid) {
                            if (datahandler.tabbuttonarray[i].DefectType == "0") { 
                                setTimeout(function () { 
                                    $("#EditDefectType").val("Minor");
                                },100);
                            } else if (datahandler.tabbuttonarray[i].DefectType == "1") { 
                                console.log(true);
                                setTimeout(function () { 
                                    $("#EditDefectType").val("Major");
                                },100);
                            } else if (datahandler.tabbuttonarray[i].DefectType == "repairs") { 
                                setTimeout(function () { 
                                    $("#EditDefectType").val("Repairs");
                                },100);
                            } else if (datahandler.tabbuttonarray[i].DefectType == "scrap") { 
                                setTimeout(function () { 
                                    $("#EditDefectType").val("Scrap");
                                },100);
                            } else {
                                if (datahandler.tabbuttonarray[i]) {
                                    $("#EditDefectType").val(datahandler.tabbuttonarray[i].DefectType);
                                }    

                            }
                        }
                    }
                } else {
                    $("#EditDefectType_Div").css("display", "none");
                }
            },
            close: function () {
                $form.find('input').val("").end();

            }
        });
        var $dialog4 = $('#dialog4').wijdialog({
            showStatus: false,
            showControlBox: false,
            autoOpen: false,
            modal: true,
            captionButtons: {
                pin: { visible: false },
                refresh: { visible: false },
                toggle: { visible: false },
                minimize: { visible: false },
                maximize: { visible: false }
            },
            buttons: {
                'Add': function () {
                    var EnteredName = $('#Name').val().trim();
                    $.ajax({
                        url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                        type: 'GET',  
                        data: { method: 'GetTemplates' },
                        success: function (data) {
                            var json = $.parseJSON(data); 
                            var errorLabel = $("#dialog4_error");
                            var existsflag = false; 
                            $.each(json, function(index, value) { 

                                if (EnteredName == value.text) { 
                                    existsflag = true;
                                }
                            }); 
                            console.log(EnteredName.length);
                            if (EnteredName.length === 0) {
                                errorLabel.text('Invalid Template Name'); 
                                return false;
                            }
                            if (existsflag == false) { 
                                var LineType = $("#LineType_pop2").val();
                                console.log(LineType);
                                if (LineType == null || LineType.toString().trim() == '')
                                {
                                    errorLabel.text("Please select a LineType");
                                    return false;
                                }
                                dbtrans.InsertTemplate(EnteredName, LineType)
                                if (EnteredName) {
                                    $('#ActiveTemplateTitle').val("  Template:" + EnteredName)
                                }
                            } else { 
                                errorLabel.text("Template Name already Exists") 
                                return false;
                            }
                            $("#LineType_pop").val($("#LineType_pop2").val());
                            $("#LineType_pop").prop('disabled', 'disabled');
                            $("#dialog4").wijdialog('close');
                            return true;
                        },
                        error: function (a, b, c) {
                            console.log(a);
                            errorLabel.text("Error Inserting TemplateName:" + a); 
                            return false
                        }

                    })
                    

                },
                'Cancel': function () {

                    $(this).wijdialog('close');
                }
            },
            open: function () {
                $button_title_input.focus();


            },
            close: function () {
                $form.find('input').val("").end();

            }
        });
        $('#loading').hide();
        var $form = $('fieldset', $dialog).submit(function () {
            controlhandler.addTab();
            var TabJsonString = JSON.stringify(controlhandler.tabarray);
            var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
            if (TabJsonString.length > 0 && ButtonJsonString.length > 0) {
                $("#MainContent_TabArray_Hidden").val(TabJsonString);
                $("#MainContent_ButtonArray_Hidden").val(ButtonJsonString);
            }
            $dialog.wijdialog('close');
            return false;
        });
        var $form1 = $('fieldset', $dialog2).submit(function () {
            controlhandler.addbutton();
            var TabJsonString = JSON.stringify(controlhandler.tabarray);
            var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
            if (TabJsonString.length > 0 && ButtonJsonString.length > 0) {
                $("#MainContent_TabArray_Hidden").val(TabJsonString);
                $("#MainContent_ButtonArray_Hidden").val(ButtonJsonString);
            }
            $dialog2.wijdialog('close');
            return false;
        });

        datahandler.GetTemplates()
        //
        
        $('#add')
            .click(function () {
            $dialog.wijdialog('open');
            datahandler.newtabflag = true;
          
        });
        $('#addbutton').click(function () {
            datahandler.newbuttonflag = true;
            $dialog2.wijdialog('open');
        });
        $('#sData').click(function () {
            setTimeout(function () { dbtrans.GetTemplateCollection() }, 1000);
        });
        $('#DefectType_add').click(function () {
            
            $DefectType_dialog.wijdialog('open');
        });
        $('#EditDefectType').bind("change", function () { 
            var valuech = $('#EditDefectType').val(); 
       
        });
        $('#LineType_pop').bind("change", function () { 
            $('#MainContent_LineType_Hidden').val($(this).val()); 
            
        });
        $(".ui-tabs-panel").on('click', '.buttontemplate', function () {
            var buttonid = $(this).attr('id');
            var keyvalue = localStorage.getItem("defect.text" + buttonid.toString())
            if (keyvalue) {
                var splitval = keyvalue.split(":")
                var defectnumber = new Number(splitval[1]) + 1
                keyvalue = this.value + ":" + defectnumber.toString()
            
             
            } else {
                keyvalue = this.value + ":1"
            }
           
            //localStorage.setItem("defect.text" + buttonid.toString(), keyvalue)
            //alert(localStorage.getItem("defect.text" + buttonid.toString()));
        });
        $(".ui-tabs-panel").on('dblclick', '.buttontemplate', function () {
  
            datahandler.editid = $(this).attr('id');
            $dialog3.wijdialog('open');
        });
        $tabs.on('click', 'span.ui-icon-close', function (e) {
  
            var index = $('li', $tabs).index($(this).parent());

            var cfm = confirm("Confirm to Delete Tab");

            if (cfm == true) {
                dbtrans.DeleteTemplate(index);
            }
        });
        $('#TemplateId').on('select2-selecting', function (e) {
            var selectedid = e.val
            switch (selectedid) {
                case 1:
                    $dialog4.wijdialog('open');
                    var length = controlhandler.tabarray.length
                    for (i = 0; i < length; i++) {
                        $(".selector").wijtabs("remove", i);
                    }
                    controlhandler.tabarray.length = 0;
                    datahandler.tabbuttonarray.length = 0;
                    break;
                default:
                    datahandler.selectedtemplateid = Number(selectedid)
                    dbtrans.GetTemplateCollection()
            }
        });
        $('#submit').on('click', function (e) {
           
            if (datahandler.selectedtemplateid) {
                dbtrans.SubmitTemplate()
            } else {
                alert("Please Select a Template")
            }
        });

    });
    function isOdd(num) { return num % 2; }
    var datahandler = {
        GetTemplates: function () {
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',
                data: { method: 'GetTemplates' },
                success: function (data) {
                    var json = $.parseJSON(data);

                    if (json.length > 0) {
                        $("#TemplateId").select2({
                            data: json
                        });

                    }
                    else {
                        return false
                    }
                },
                error: function (a, b, c) {
                    alert(c);
                    console.log('failed');
                }
            });
        },
        GetButtonLibrary: function () {
            
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',   
                data: { method: 'GetButtonLibrary' },
                success: function (data) {
                    var json = $.parseJSON(data);
                   
                    if (json.length > 0) {
                        datahandler.buttonarray = json;
   
                    }
                    else {
                        return false
                    }
                },
                error: function (a, b, c) {
                    alert(c);
                    console.log('failed');
                }
            });
        },
        GetButtonLibraryGrid: function () {
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',   
                data: { method: 'GetButtonLibraryGrid' },
                success: function (data) {
                    var json = $.parseJSON(data);
                    
                    if (json.length > 0) {
                        ButtonLibrary = json; 
                    }
                    else {
                        return false
                    }
                },
                error: function (a, b, c) {
                    alert(c);
                    console.log('failed');
                }
            });
        },
        buttonarray: Array(),
        tabbuttonarray: Array(),
        selectedbutton: Object,
        selectedList: Array({id: 1, label: "this", DefectCode: ""}),
        selectededit: "edit",
        selectededitid: 0,
        buttoncount: 1,
        selectedtab: Number(),
        newtabflag: Boolean(false),
        newbuttonflag: Boolean(false),
        editid: 'string',
        selectedtemplateid:1,
        editbuttons: function (method) {

            switch (method) {
                case 'edit':
                    var editdefectypeval = $("#EditDefectType").val();
                    for (i = 0; i < datahandler.tabbuttonarray.length; i++) {

                        var buttonid = 'button' + datahandler.tabbuttonarray[i].id.toString();

                        if (buttonid == datahandler.editid) {
                            
                            if (editdefectypeval == "Minor") { 
                                datahandler.tabbuttonarray[i].DefectType = "0"
                            } else if (editdefectypeval == "Major") { 
                                datahandler.tabbuttonarray[i].DefectType = "1"
                            } else if (editdefectypeval == "Repairs") { 
                                datahandler.tabbuttonarray[i].DefectType = "repairs"
                            } else if (editdefectypeval == "Scrap") { 
                                datahandler.tabbuttonarray[i].DefectType = "scrap"
                            } else { 
                                datahandler.tabbuttonarray[i].DefectType = editdefectypeval;
                            }
                            if (datahandler.tabbuttonarray[i].DefectType == "0") { 
                                $('#' + buttonid).css("background-color", "#93C2FF");
                            } else if (datahandler.tabbuttonarray[i].DefectType == "1") { 
                                $('#' + buttonid).css("background-color", "#4875AE");
                            } else if (datahandler.tabbuttonarray[i].DefectType == "repairs") { 
                                $('#' + buttonid).css("background-color", "#B78B28");
                            } else if (datahandler.tabbuttonarray[i].DefectType == "scrap") { 
                                $('#' + buttonid).css("background-color", "rgba(0,0,0,0.5)");
                            } else if (datahandler.tabbuttonarray[i].DefectType == "Time") { 
                                $('#' + buttonid).css("background-color", "#33ccd2");
                            } else if (datahandler.tabbuttonarray[i].DefectType == "Upgrade") { 
                                $('#' + buttonid).css("background-color", "#14b71e");
                            } else if (datahandler.tabbuttonarray[i].DefectType == "Fix") { 
                                $('#' + buttonid).css("background-color", "#95ea9a");
                            }
                            if (datahandler.selectededit != "edit") {
                                datahandler.tabbuttonarray[i].text = datahandler.selectededit;
                                datahandler.tabbuttonarray[i].ButtonId = datahandler.selectededitid;
                                $('#' + buttonid).val(datahandler.selectededit);
                            }
                            datahandler.selectededit = "edit";
                        }
                    }
                    break;
                case 'delete':
                    console.log('editbuttons-delete')

                    for (i = 0; i < datahandler.tabbuttonarray.length; i++) {

                        var buttonid = 'button' + datahandler.tabbuttonarray[i].id.toString()
                        if (buttonid == datahandler.editid) {
                            if (datahandler.tabbuttonarray[i].Hide == false ) { 
                                datahandler.tabbuttonarray[i].Hide = true
                                $('#' + buttonid).css("background-color", "#9C9C9C");
                            } else { 
                                datahandler.tabbuttonarray[i].Hide = false
                                if (datahandler.tabbuttonarray[i].DefectType == false){
                                    $('#' + buttonid).css("background-color", "#93C2FF");
                                } else { 
                                    $('#' + buttonid).css("background-color", "#4875AE");
                                }
                            }
                            //controlhandler.addbutton()
                        }
                    }
                   
            }
        },
        AddDefectType: function (DefectTypeName, DefectTypeText) {
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',
                data: { method: 'AddDefectType', args: { Code: DefectTypeName, Text: DefectTypeText } },
                success: function (data) {
                var json = data;
                if (json > 0) {
                    datahandler.GetButtonLibrary();
                    alert("Defect Type Added");
                } else {
                    alert("Failed To Add DefectType");
                }
                },
                error: function (a, b, c) {
                    alert(c);
                    console.log('failed');
                }
            });
        }
    };
    var controlhandler = {
        $tab_title_input: $('#tab_title'),
        $tabs: $('#tabs'),
        tab_counter: 2,
        tabarray: new Array(),
        addbuttontotab: function (totalcount, ui) {
            
            var totalcountnum = new Number(totalcount) - 1;
            var localarray = new Array(datahandler.tabbuttonarray);
            var tabbuttoncount = controlhandler.gettabbuttoncount(tabselect, localarray);
            var buttonsize = controlhandler.sizebutton(tabbuttoncount)
            var counter = 0;
            var placementstring;
            var buttoncolor = "#CF0D39";
            var TimerStringhtml = '';
            for (i = 0; i < localarray[0].length; i++) {
                try
                {

                    if (datahandler.tabbuttonarray[i].tabindex.toString() == tabselect.toString() && datahandler.newtabflag == false) {
              
                        if (localarray[0][i]) { 
                            if (localarray[0][i].DefectType == "0") { 
                                buttoncolor = "#B7B328";
                            } else if (localarray[0][i].DefectType == "1") { 
                                buttoncolor = "#CF0D39";
                            } else if (localarray[0][i].DefectType == "repairs") { 
                                buttoncolor = "rgba(0,0,0,0.5)";
                            } else if (localarray[0][i].DefectType == "scrap") { 
                                buttoncolor = "#0C0D0C";
                            } else if (localarray[0][i].DefectType == "Time") { 
                                buttoncolor = "#33ccd2";
                            } else if (localarray[0][i].DefectType == "Upgrade") { 
                                buttoncolor =  "#14b71e";
                            } else if (localarray[0][i].DefectType == "Fix") { 
                                buttoncolor =  "#95ea9a";
                            }

                            if (localarray[0][i].Hide == true) { 
                                buttoncolor = "#9C9C9C";
                            }

         
                            if (buttonsize[0].height < 100) {buttonsize[0].height = 100}

                    
                            placementstring = controlhandler.placebutton(counter, buttonsize[0].height, buttonsize[0].width)
                            timerplacementstring = controlhandler.placetimerbutton(counter, buttonsize[0].height, buttonsize[0].width, "START");
                            timerstopplacementstring = controlhandler.placetimerbutton(counter, buttonsize[0].height, buttonsize[0].width, "STOP");
                            if (localarray[0][i].Timer == true) { 
                                TimerStringhtml = '<button id="start_button_' + localarray[0][i].id.toString() + ' type="button" style="width:' + (buttonsize[0].width * .15).toString() + 'px;height:' + (buttonsize[0].height * .85).toString() + 'px; z-index: 1000; position:absolute; ' + timerplacementstring + '; font-size:1.2em; background-color:#85b2cb;">START</button><button id="stop_button_' + localarray[0][i].id.toString() + ' type="button" style="width:' + (buttonsize[0].width * .15).toString() + 'px;height:' + (buttonsize[0].height * .85).toString() + 'px; z-index: 1000; position:absolute; ' + timerstopplacementstring + '; font-size:1.2em; background-color:rgba(111, 106, 107, 0.68);">STOP</button>';
                            }
                            var appendstring = '<input id="button' + localarray[0][i].id.toString() + '" type = "button" class="buttontemplate" value="' + localarray[0][i].text + '&#13;&#10;' + localarray[0][i].DefectCode + '" style="width:' + buttonsize[0].width + 'px;height:' + buttonsize[0].height + 'px; position:absolute; ' + placementstring + '; font-size:1.5em; background-color:' + buttoncolor + ';"></input>';
                            $(ui.panel).append(TimerStringhtml + '<button id="button' + localarray[0][i].id.toString() + '" type="button" class="buttontemplate" style="width:' + buttonsize[0].width + 'px;height:' + buttonsize[0].height + 'px; position:absolute; ' + placementstring + '; font-size:1.2em; background-color:' + buttoncolor + ';">' + localarray[0][i].ButtonLibraryId.toString() + '.<br />' + localarray[0][i].text + '</button>');
                            //
                            counter = counter + 1;
                            datahandler.buttoncount = datahandler.buttoncount + 1;
                        }
                    }
                }
                catch (e) 
                {

                }
            }

            datahandler.newbuttonflag = false;
        },
        gettabbuttoncount: function (index, localarray) {
            var count = 0;
          
            for (i = 0; i < localarray[0].length; i++) {
              
                if (localarray[0][i].tabindex == index || index.toString() == localarray[0][i].tabindex.toString()) { count = count + 1 }
            }
            return count
        },
        addTab: function (TabTitle) { 
            var tab_title;
            if (TabTitle == null || TabTitle == "") {
                tab_title = controlhandler.$tab_title_input.val() || 'Tab ' + controlhandler.tab_counter;
            } else {
                tab_title = TabTitle;
            }
            controlhandler.$tabs.wijtabs('add', '#tabs-' + controlhandler.tab_counter, tab_title);
            controlhandler.tab_counter++;
            $("a").addClass("nooutlineclass");
        }, 
        addbutton: function () { 
            var returnsValue; // Type:  wijtabs
            // Parameters
            var index = datahandler.selectedtab; // Type:  number
            tabselect = index;
            returnsValue = controlhandler.$tabs.wijtabs("remove", index);
            returnsValue = controlhandler.$tabs.wijtabs("add", '#tabs-' + index.toString() + '', controlhandler.tabarray[index].title, index);
            returnsValue = controlhandler.$tabs.wijtabs("select", index);
            
        },
        placebutton: function (count, buttonheight, buttonwidth) { 
            var height = controlhandler.$tabs.height()
            var width = controlhandler.$tabs.width()
            var left;
            var top;
            var countnumber = new Number(count + 1);
            var butheight = new Number(buttonheight);
            var butwidth = new Number(buttonwidth);

           
            if (isOdd(countnumber) == 1) {
                left = 15;
                top = 76 + ((countnumber - 1) * butheight + 1) / 2
            }
            else {
                left = 18 + butwidth;
                top = 76 + ((countnumber - 2) * butheight + 1) / 2
            }

            return 'left: ' + left.toString() + 'px; top: ' + top.toString() + 'px;'
        },
        placetimerbutton: function (count, buttonheight, buttonwidth, type) { 
            var height = controlhandler.$tabs.height()
            var width = controlhandler.$tabs.width()
            var left;
            var top;
            var countnumber = new Number(count + 1);
            var butheight = new Number(buttonheight);
            var butwidth = new Number(buttonwidth);

           
            if (isOdd(countnumber) == 1) {
                if (type == "STOP") { 
                    left = 35 + butwidth  - (butwidth * .18);;
                } else { 
                    left = 15;
                }
                top = 86 + ((countnumber - 1) * butheight + 1) / 2
            }
            else {
                if (type == "STOP") { 
                    left = 35 + (butwidth * 2) - (butwidth * .18);
                } else { 
                    left = 18 + butwidth;
                }
                top = 86 + ((countnumber - 2) * butheight + 1) / 2
            }

            return 'left: ' + left.toString() + 'px; top: ' + top.toString() + 'px;'
        },
        sizebutton: function (totalcount) { 
            var height = controlhandler.$tabs.height()
            var width = controlhandler.$tabs.width()
            var totalcountnumber = new Number(totalcount);
            var distancecount = Math.ceil(totalcountnumber / 2);
            var returnarray = new Array();
            if (totalcountnumber > 0) {
                var buttonheight = ((height - 60) / distancecount);
                var buttonwidth = (width - 10) / 2
                returnarray.push({ width: buttonwidth, height: buttonheight })
                
            }
            else {
                returnarray.push({ width: 200, height: 200 })
            }

            return returnarray

        },
        format_created: function (cellvalue, options, rowobject) {

            if (rowobject.Status == 'False') {
                return "<span style='color: gray; font-weight:normal'>" + cellvalue + "</span>";
            } else {
                return "<span style='color: black; font-weight: bolder;'>" + cellvalue + "</span>";
            }

        }


    };
    var dbtrans = {
        InsertTemplate: function (name, LineType) {
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                //data: { method: 'InsertTemplate', args: { Name: name, methodname: "<%=Session("Username")%>" } },
                data: { method: 'InsertTemplate', args: { Name: name, Username: "QA User", InspectionType: LineType } },
                success: function (data) {
                    
                    if (datahandler.selectedtemplateid == Number(0)) {
                        alert("Template -" + name + "- already exist please select a different name")
                    } else {
                        datahandler.selectedtemplateid = new Number(data);
                        $("#MainContent_TemplateId_Hidden").val(datahandler.selectedtemplateid);
              
                        for (i = 0; i < controlhandler.tabarray.length; i++) {
                            $('#tabs').wijtabs('remove', i);
                        }
                        controlhandler.tabarray.length = 0;
                        datahandler.tabbuttonarray.length = 0;

                    }
                    
                },
                error: function (a, b, c) {
                    console.log("dbtrans.InsertTemplateError");
                }

                })
        },
        SubmitTemplate: function () {

           // if (datahandler.tabbuttonarray.length > 0 && controlhandler.tabarray.length > 0) {
                var TabJsonString = JSON.stringify(controlhandler.tabarray);
                var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
     
                $('#loading').toggle();
                $('.export').attr("disabled", "disabled");
                $('.inputelement').attr("disabled", "disabled");
                return $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                    type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                    data: { method: 'SubmitTemplate', args: { TemplateId: datahandler.selectedtemplateid, TabArray: TabJsonString, ButtonArray: ButtonJsonString } },
                    success: function (data) {

                        if (data == "Failed") {
                            alert("SubmissionFailed.")
                            //location.reload();
                        } else {
                            alert(data);
                        }
                        //dbtrans.RefreshGrid();
                    },
                    error: function (a, b, c) {
                        console.log("dbtrans.InsertTemplateError");
                    }

                });
                $('#loading').toggle();
                $('.export').removeAttr("disabled");
                $('.inputelement').removeAttr("disabled");

       //     }

        },
        GetTemplateCollection: function () {
            var UserDiv = $('#UserDirections');
            var buttonarray = new Array();
            var tabarray = new Array();
            if (UserDiv.is(':visible') == true) { UserDiv.toggle() }
            for (i = 0; i < controlhandler.tabarray.length; i++) {
                $('#tabs').wijtabs('remove', 0);
            }
            controlhandler.tabarray.length = 0;
            datahandler.tabbuttonarray.length = 0;

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                data: { method: 'GetTemplateCollection_Admin', args: { TemplateId: datahandler.selectedtemplateid} },
                success: function (data) {
       
                    var selectedtab = tabselect;
                    if (data != "false" && data != "empty") {
                        var json = $.parseJSON(data)
                        var length = json.length - 1;
                        var tabnumber = 99;
                        var returnsValue;
                        var counter = 0;
                        var buttoncounter = 1;
                     
                        if (json.length == 0) { return }
                        $('#loading').toggle();
                        $('.export').attr("disabled", "disabled");
                        $('.inputelement').attr("disabled", "disabled");
                        var originaltabvalue = tabselect;
                        var refreshId = setInterval(function () {
                       
                            if (counter == length || counter > 100) {
                                clearInterval(refreshId);
                            }
                            if (tabnumber != json[counter].TabNumber) {
                                tabselect = json[counter].TabNumber;
                                var lastrow = json.length - 1; 
                                for (i = 0; i < json.length; i++) {
                                    if (json[i].TabNumber == json[counter].TabNumber) {
                                        if (json[i].ButtonId != 0 && json[i].ButtonName != "NaN") {
                                            datahandler.tabbuttonarray.push({ text: json[i].ButtonName, tabindex: json[counter].TabNumber, id: buttoncounter, ButtonId: json[i].ButtonId, DefectType: json[i].DefectType, ButtonTemplateId: json[i].id, DefectCode: json[i].DefectCode, id: json[i].id, Hide: json[i].Hide, ButtonLibraryId: json[i].ButtonLibraryId, Timer: json[i].Timer  });

                                            var ButtonJsonString = JSON.stringify(datahandler.tabbuttonarray);
                                            if (ButtonJsonString.length > 0) {
                                                $("#MainContent_ButtonArray_Hidden").val(ButtonJsonString);
                                            }        
                                           
                                        }
                                        buttoncounter++;
                                    }
                                }
                                tabnumber = json[counter].TabNumber;
                                controlhandler.$tabs.wijtabs('add', '#tab-' + json[counter].TabNumber.toString(), json[counter].Name);
                                controlhandler.tabarray.push({ title: json[counter].Name.toString(), TabTemplateid: json[counter].TabTemplateId });
                                var TabJsonString = JSON.stringify(controlhandler.tabarray);
                                if (TabJsonString.length > 0) {
                                    $("#MainContent_TabArray_Hidden").val(TabJsonString);
                                }      
                            }

                            counter++;

                        }, 70);
                        tabselect = 0;
                        
                        $("#MainContent_LineType_Hidden").val(json[0].LineType);
                        $("#LineType_pop").val(json[0].LineType);
                        var loaderswitch = setTimeout(function () {
                            $('#loading').toggle();
                            $('.export').removeAttr("disabled");
                            $('.inputelement').removeAttr("disabled");
                        }, 70 * (length + 1));

                        
                       
                    } else {
                        datahandler.selectedtab = new Number(0);
                        datahandler.buttoncount = 1;
                        tabselect = 0;
                    }
                   
                },
                error: function (a, b, c) {
                    console.log("dbtrans.GetTemplateCollectionError");
                }

            })

        },
        DeleteTemplate: function (TabNumber) {

            var TabTemplateId = controlhandler.tabarray[TabNumber].TabTemplateid;
            if (TabTemplateId) {
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                    type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                    data: { method: 'DeleteTab', args: { TabNumber: TabNumber, TemplateId: datahandler.selectedtemplateid, TabTemplateId: TabTemplateId } },
                    success: function (data) {
             
                        if (data != "false") {
                            $('#tabs').wijtabs('remove', TabNumber);
                            for (var i = 0; i < datahandler.tabbuttonarray.length; i++) {
                                if (datahandler.tabbuttonarray[i].tabindex == TabNumber) {
                                    datahandler.tabbuttonarray.splice(i, 1)
                                }
                            }
                            var TabTemplateId = controlhandler.tabarray[TabNumber].TabTemplateid;

                            for (var i = 0; i < controlhandler.tabarray.length; i++) {
                                if (controlhandler.tabarray[i].TabTemplateid == TabTemplateId) {
                                    controlhandler.tabarray.splice(i,1)
                                }
                            }

                        }
                    },
                    error: function (a, b, c) {
                        console.log("dbtrans.DeleteTemplateError");
                        alert("Tab Not Deleted - applying auto Refresh")
                    }

                });


            } 
            
        },
        RefreshSpecGrid: function (TabTemplateIdVal) {

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                    type: 'POST',
                    data: { method: 'GetProductSpecs', args: {TabTemplateId: TabTemplateIdVal} },
                    success: function (data) {

                        var json;
                        var $wijgrid = $("#Specgrid")[0];
                        json = $.parseJSON(data);
                        jQuery("#Specgrid").jqGrid('clearGridData', true);
                        jQuery("#Specgrid").jqGrid('setGridParam', { data: json, page: 1 }).trigger('reloadGrid');

                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });

        },
        RefreshDefectMaint: function () {

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',   
                data: { method: 'GetButtonLibraryGrid' },
                success: function (data) {
                  
                    var json;
                    json = $.parseJSON(data);
                    jQuery("#DefectTypesgrid").jqGrid('clearGridData', true);
                    jQuery("#DefectTypesgrid").jqGrid('setGridParam', { data: json, page: 1 }).trigger('reloadGrid');

                },
                error: function (a, b, c) {
                    alert(c);
                }
            });

        },
        RefreshManagerGrid: function () {

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'GET',
                data: { method: 'GetTemplateTable' },
                success: function (data) {

                    var json;
                    
                    json = $.parseJSON(data);
                    GridManagerTable = json
                    $("#wijgrid").jqGrid('clearGridData', true);
                    $("#wijgrid").jqGrid('setGridParam', { data: json, page: 1 }).trigger('reloadGrid');

                },
                error: function (a, b, c) {
                    alert(c);
                }
            });

        },
        RenderGridManager: function () { 
            var selectedGridId= 0; 
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'POST',
                data: { method: 'GetTemplateTable' },
                success: function (data) {
                    GridManagerTable = $.parseJSON(data);
                   
                    $("#wijgrid").jqGrid({
                        datatype: "local",
                        editurl: "<%=Session("BaseUri")%>" + '/handlers/JqGrid_Edit.ashx',
                        colNames: ['TemplateId', 'Name', 'Owner', 'DateCreated', 'Active'],
                        colModel: [
                                { name: 'TemplateId', index: 'TemplateId', hidden: true, editable: true },
                                { name: 'Name', index: 'name', sortable: false, width: 25, formatter: controlhandler.format_created },
                                { name: 'Owner', index: 'owner', hidden:true, sortable: false, width: 16, formatter: controlhandler.format_created },
                                { name: 'DateCreated', index: 'datecreated', sortable: false, sorttype: 'date', width: 12, formatter: controlhandler.format_created, formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'} },
                                { name: 'Active', index: 'Active', sortable: false, width: 7, formatter: controlhandler.format_created }
                        ],
                        pager: '#gridpager',
                        caption: "Template Manager",
                        multiselect: false,
                        loadonce: false,
                        gridview: true,
                        rowNum: 25,
                        scroll: false,
                        viewrecords: true,
                        data: GridManagerTable,
                        height: "100%",
                        width: new Number(300),
                        ondblClickRow: function (rowid) {
                            var mydata = $("#wijgrid").jqGrid('getGridParam','data');
                            var rowind = $("#wijgrid").jqGrid('getInd', rowid);
                            var rowdata = $("#wijgrid").find("td[aria-describedby='wijgrid_TemplateId']");
                          
                            var TemplateId = mydata[rowind - 1].TemplateId;
                            GlobalTemplateId = TemplateId;
                            var name = mydata[rowind - 1].Name;
                            var pageNumber = $("#wijgrid").getGridParam('page');
                           
                            switch (name) {
                                case "New Template":
                                    if (pageNumber == 1) { 
                                        var length = $("#tabs").wijtabs("length");
                                    
                                        for (i = 0; i < length; i++) {  
                                            $("#tabs").wijtabs("remove", 0);
                                   
                                        }
                                        controlhandler.tabarray.length = 0;
                                        datahandler.tabbuttonarray.length = 0;
                                        $('#dialog4').wijdialog('open');
                                    }
                                    break;
                                default:
                                    $('#ActiveTemplateTitle').val("  Template: " + name);
                                    datahandler.selectedtemplateid = Number(TemplateId)
                                    $("#MainContent_TemplateId_Hidden").val(TemplateId);
                                    dbtrans.GetTemplateCollection()
                            }

                        },
                        afterSaveCell: function(rowid,name,val,iRow,iCol) {
                            alert("alert1!");
                        },
                        afterEditCell: function (id,name,val,iRow,iCol){
                            alert("alert2!");
                        },
                        onSelectRow: function(id){
                            var mydata = $("#wijgrid").jqGrid('getGridParam','data');
                            var rowind = $("#wijgrid").jqGrid('getInd', id);


                        }
                        
                    });
                    
                    $('#wijgrid').jqGrid('navGrid', '#gridpager',
                        {
                            edit: true,
                            add: false,
                            del: true,
                            search: false
                        },
                        {
                            afterSubmit: processAddEdit,
                            afterEditCell: function(rowid,celname,value,iRow,iCol){ alert('afteredit_')},
                            closeAfterAdd: true,
                            closeAfterEdit: true,
                            reloadAfterSubmit: true,
                            beforeShowForm: function (formid) {
                            
                                $('#Loc_CAR').prop('checked', GridManagerTable[selectedGridId -1].Loc_CAR); 
                                $('#Loc_STT').prop('checked', GridManagerTable[selectedGridId -1].Loc_STT);
                                $('#Loc_STJ').prop('checked', GridManagerTable[selectedGridId- 1].Loc_STJ);
                                $('#Loc_SPA').prop('checked', GridManagerTable[selectedGridId- 1].Loc_SPA);
                                $('#Loc_CDC').prop('checked', GridManagerTable[selectedGridId- 1].Loc_CDC);
                                $('#Loc_LINYI').prop('checked', GridManagerTable[selectedGridId- 1].Loc_LINYI);
                                $('#Loc_FSK').prop('checked', GridManagerTable[selectedGridId- 1].Loc_FSK);
                                $('#Loc_PCE').prop('checked', GridManagerTable[selectedGridId- 1].Loc_PCE);
                                $('#Loc_FNL').prop('checked', GridManagerTable[selectedGridId- 1].Loc_FNL);
                                $('#Loc_FPC').prop('checked', GridManagerTable[selectedGridId- 1].Loc_FPC);
                                //if (GridManagerTable[selectedGridId -1].Loc_CAR == true) { 
                                     
                                //    $('#Loc_CAR').prop('checked', true); 
                                //}
                                //if (GridManagerTable[selectedGridId -1].Loc_STT == true) { 
                                    
                                //    $('#Loc_STT').prop('checked', true);
                                //}
                                //if (GridManagerTable[selectedGridId- 1].Loc_STJ == true) { 
                                   
                                //    $('#Loc_STJ').prop('checked', true);
                                //}
                            }
                        }
                            );
                },
                error: function (a, b, c) {
                    alert(c);
                }
            });
        },
        GetGridManagerData: function () {
            var selectedGridId= 0; 
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                type: 'POST',
                data: { method: 'GetTemplateTable' },
                success: function (data) {
                    GridManagerTable = $.parseJSON(data);
                   
                    $("#wijgrid").jqGrid({
                        datatype: "local",
                        editurl: "<%=Session("BaseUri")%>" + '/handlers/JqGrid_Edit.ashx',
                        colNames: ['TemplateId', 'Name', 'Owner', 'STT', 'CAR', 'STJ', 'SPA' , 'CDC', 'LINYI', 'FSK', 'FNL','FPC', 'PCE', 'DateCreated', 'Active'],
                        colModel: [
                                { name: 'TemplateId', index: 'TemplateId', hidden: true, editable: true },
                                { name: 'Name', index: 'name', sortable: false, width: 25, formatter: controlhandler.format_created },
                                { name: 'Owner', index: 'owner', hidden:false, sortable: false, width: 16, formatter: controlhandler.format_created },
                                { name: 'Loc_STT', index: 'Loc_STT', sortable: false, width: 7, hidden:true,formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_CAR', index: 'Loc_CAR', sortable: false, width: 7, hidden:true,formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_STJ', index: 'Loc_STJ', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_SPA', index: 'Loc_SPA', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_CDC', index: 'Loc_CDC', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_LINYI', index: 'Loc_LINYI', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_FSK', index: 'Loc_FSK', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_FNL', index: 'Loc_FNL', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_FPC', index: 'Loc_FPC', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'Loc_PCE', index: 'Loc_PCE', sortable: false, width: 7, hidden:true,editrules: {edithidden:true}, formatter: controlhandler.format_created, editable: true,edittype:"checkbox",editoptions: {value:"Yes:No"} },
                                { name: 'DateCreated', index: 'datecreated', sortable: false, sorttype: 'date', width: 12, formatter: controlhandler.format_created, formatter: 'date', formatoptions: {srcformat: 'ISO8601Long', newformat: 'm/d/y H:i A'} },
                                { name: 'Active', index: 'Active', hidden: true, sortable: false, width: 7, formatter: controlhandler.format_created }
                        ],
                        pager: '#gridpager',
                        caption: "Template Manager",
                        multiselect: false,
                        loadonce: false,
                        gridview: true,
                        rowNum: 21,
                        scroll: false,
                        viewrecords: true,
                        data: GridManagerTable,
                        height: "100%",
                        width: new Number(300),
                        ondblClickRow: function (rowid) {
                            var mydata = $("#wijgrid").jqGrid('getGridParam','data');
                            var rowind = $("#wijgrid").jqGrid('getInd', rowid);
                            var rowdata = $("#wijgrid").find("td[aria-describedby='wijgrid_TemplateId']");
                            var namedata = $("#wijgrid").find("td[aria-describedby='wijgrid_Name']");

                            var TemplateId = rowdata[rowid - 1].title;
                            GlobalTemplateId = TemplateId;
                            var name = namedata[rowid - 1].title;
                            var pageNumber = $("#wijgrid").getGridParam('page');
         
                            switch (name) {
                                case "New Template":
                                    if (pageNumber == 1) { 
                                        var length = $("#tabs").wijtabs("length");
                                    
                                        for (i = 0; i < length; i++) {  
                                            $("#tabs").wijtabs("remove", 0);
                                   
                                        }
                                        controlhandler.tabarray.length = 0;
                                        datahandler.tabbuttonarray.length = 0;
                                        $('#dialog4').wijdialog('open');
                                    }
                                    break;
                                default:
                                    $('#ActiveTemplateTitle').val(  "Template: " + name)
                                    datahandler.selectedtemplateid = Number(TemplateId)
                                    $("#MainContent_TemplateId_Hidden").val(TemplateId);
                                    dbtrans.GetTemplateCollection()
                            }

                        },
                        afterSaveCell: function(rowid,name,val,iRow,iCol) {
                            alert("alert1!");
                        },
                        afterEditCell: function (id,name,val,iRow,iCol){
                            alert("alert2!");
                        },
                        onSelectRow: function(id){
                            var mydata = $("#wijgrid").jqGrid('getGridParam','data');
                            var rowind = $("#wijgrid").jqGrid('getInd', id);

                            if(id && id!==lastsel2 && mydata[rowind - 1].Name != 'New Template'){
                                $("#wijgrid").jqGrid('restoreRow',lastsel2);
                                $("#wijgrid").jqGrid('editRow',id,true, null, null, "<%=Session("BaseUri")%>" + '/handlers/JqGrid_Edit.ashx',{}, reload);
                                lastsel2=rowind;
                                selectedGridId = rowind; 

                                if (selectedGridId != 0) { 
                                    if (GridManagerTable[rowind -1].Loc_CAR == true) { 
                                        $('#' + id.toString() + '_Loc_CAR').prop('checked', true); 
                                        $('#Loc_CAR').prop('checked', true); 
                                    }
                                    if (GridManagerTable[rowind -1].Loc_STT == true) { 
                                        $('#' + id.toString() + '_Loc_STT').prop('checked', true);
                                        $('#Loc_STT').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_STJ == true) { 
                                        $('#' + id.toString() + '_Loc_STJ').prop('checked', true);
                                        $('#Loc_STJ').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_SPA == true) { 
                                        $('#' + id.toString() + '_Loc_SPA').prop('checked', true);
                                        $('#Loc_SPA').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_CDC == true) { 
                                        $('#' + id.toString() + '_Loc_CDC').prop('checked', true);
                                        $('#Loc_CDC').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_LINYI == true) { 
                                        $('#' + id.toString() + '_Loc_LINYI').prop('checked', true);
                                        $('#Loc_LINYI').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_FSK == true) { 
                                        $('#' + id.toString() + '_Loc_FSK').prop('checked', true);
                                        $('#Loc_FSK').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_PCE == true) { 
                                        $('#' + id.toString() + '_Loc_PCE').prop('checked', true);
                                        $('#Loc_PCE').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_FNL == true) { 
                                        $('#' + id.toString() + '_Loc_FNL').prop('checked', true);
                                        $('#Loc_FNL').prop('checked', true);
                                    }
                                    if (GridManagerTable[rowind- 1].Loc_FPC == true) { 
                                        $('#' + id.toString() + '_Loc_FPC').prop('checked', true);
                                        $('#Loc_FPC').prop('checked', true);
                                    }
                                    
                                }
                            }
                        },
                        subGrid: true,
                        subGridRowExpanded: function (subgrid_id, row_id) { 
                            var subgridquerystr; 

                            var rowdata = $("#wijgrid #" + row_id).find("td[aria-describedby='wijgrid_TemplateId']").html();
                      
                            if (rowdata) { 
                                if (rowdata.length > 1) { 
                                    subgridquerystr = "TemplateId=" + rowdata
                                }
                            }

                             var subgrid_table_id;
                            subgrid_table_id = subgrid_id+"_t";
                            jQuery("#"+subgrid_id).html("<table id='"+subgrid_table_id+"' class='scroll'></table><div id='"+subgrid_table_id+"_pager'></div>");
                            jQuery("#"+subgrid_table_id).jqGrid({
                                url:"<%=Session("BaseUri")%>" + '/handlers/DataEntry/TempMangSubgrid_Load.ashx?' + subgridquerystr,
                                editurl: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/TempMangSubgrid_Load.ashx?' + 'TemplateId_Val=' + rowdata,
                                datatype: "json",
                                colNames: ['ac', "LocationMaster_id","Abr", "Name", "LiveStatus"],
                                colModel: [
                                  { name: 'edit', index: 'edit', hidden: false, width: 30, formatter: 'actions', formatoptions : { 
                                      onSuccess: function() {  alert("success"); $("#" + subgrid_id).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');},
                                      actions: { editbutton: true, editParams: {editval:'test1'}, delbutton: false, keys: false}} },
                                  {name:"LocationMaster_id",index:"LocationMaster_id",width:85,hidden:true, key:true},
                                  {name:"Abr",index:"Abr",width:45,key:true},
                                  {name:"Name",index:"Name",width:90},
                                  {name:"LiveStatus",index:"LiveStatus", editable:true, edittype:'select', editoptions:{value:"0:false;1:true"}, width:45,align:"right"}
                                ],
                                height: '100%',
                                postData: {
                                    TemplateId_Val: function () { 
                                        return rowdata;
                                    }
                                },
                                pager: subgrid_table_id + '_pager',
                                onSelectRow: function (id) {

                                    var rowdata = $("#"+subgrid_table_id).find("td[aria-describedby='" + subgrid_table_id + "_LocationMaster_id']");
     
                                    if (rowdata) { 
                                        if (rowdata.length > 0 && id > 0) {
                                            subgridquerystr = "LocationMaster_id=" + id;
                                        }
                                    
                                    
                                    }
    
                        
                                },
                                gridComplete: function () { 
                                    var tableht = $("#"+subgrid_table_id).height() - 25;
                                    $('.ui-inline-del').css('display', 'none');
                                    if (tableht > 46){
                                        $("#"+subgrid_id).css('height', tableht.toString() + "px");
                                        
                                    }
                                
                                }
                            });
                            jQuery("#"+subgrid_table_id).jqGrid('inlineNav', "#"+subgrid_table_id + '_pager',{
                                editParams: {
                                    keys: true,
                                    extraparam: {
                                        IjsId: function () { 
                                            return '12345';
                                        }
                                    },
                                    successfunc: function( response ) {
                                        console.log('success save!')
                                    }
                                }
                            });
                        }
                    });
                    
                    $('#wijgrid').jqGrid('navGrid', '#gridpager',
                        {
                            edit: true,
                            add: false,
                            del: true,
                            search: false
                        },
                        {
                            afterSubmit: processAddEdit,
                            afterEditCell: function(rowid,celname,value,iRow,iCol){ alert('afteredit_')},
                            closeAfterAdd: true,
                            closeAfterEdit: true,
                            reloadAfterSubmit: true,
                            beforeShowForm: function (formid) {
                        
                                $('#Loc_CAR').prop('checked', GridManagerTable[selectedGridId -1].Loc_CAR); 
                                $('#Loc_STT').prop('checked', GridManagerTable[selectedGridId -1].Loc_STT);
                                $('#Loc_STJ').prop('checked', GridManagerTable[selectedGridId- 1].Loc_STJ);
                                $('#Loc_SPA').prop('checked', GridManagerTable[selectedGridId- 1].Loc_SPA);
                                $('#Loc_CDC').prop('checked', GridManagerTable[selectedGridId- 1].Loc_CDC);
                                $('#Loc_LINYI').prop('checked', GridManagerTable[selectedGridId- 1].Loc_LINYI);
                                $('#Loc_FSK').prop('checked', GridManagerTable[selectedGridId- 1].Loc_FSK);
                                $('#Loc_PCE').prop('checked', GridManagerTable[selectedGridId- 1].Loc_PCE);
                                $('#Loc_FNL').prop('checked', GridManagerTable[selectedGridId- 1].Loc_FNL);
                                $('#Loc_FPC').prop('checked', GridManagerTable[selectedGridId- 1].Loc_FPC);
                                //if (GridManagerTable[selectedGridId -1].Loc_CAR == true) { 
                                     
                                //    $('#Loc_CAR').prop('checked', true); 
                                //}
                                //if (GridManagerTable[selectedGridId -1].Loc_STT == true) { 
                                    
                                //    $('#Loc_STT').prop('checked', true);
                                //}
                                //if (GridManagerTable[selectedGridId- 1].Loc_STJ == true) { 
                                   
                                //    $('#Loc_STJ').prop('checked', true);
                                //}
                            }
                        }
                            );
                },
                error: function (a, b, c) {
                    alert(c);
                }
            });
        },
        RenderProductSpecGrid: function () {
            var initialarray = new Array({"SpecId": 0, "TabTemplateId": 0, "Spec_Description": "Desc Here", "Upper_Spec_Value": -.99, "Lower_Spec_Value": .99});
      
            $("#Specgrid").jqGrid({
                datatype: "json",
                url:     "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility_SpecLoad.ashx',
                editurl: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility_Spec.ashx',
                colNames: ['SpecId', 'TabTemplateId', 'DataNo', 'ProductType', 'Spec Description', 'HowTo', 'value', 'Upper_Spec_Value', 'Lower_Spec_Value', 'GlobalSpec',  'SpecSource'],
                colModel: [
                        { name: 'SpecId', index: 'SpecId',  editable: true, editoptions:{readonly:true}, sorttype:'int' },
                        { name: 'TabTemplateId', index: 'TabtemplateId', hidden: true },
                        { name: 'DataNo', index: 'DataNo', editable: true},
                        { name: 'ProductType', index: 'PrudctType', editable: true},
                        { name: 'Spec_Description', index: 'Spec_Description', sortable: false,  editable: true },
                        { name: 'HowTo', index: 'HowTo', sortable: false,  editable: true },
                        { name: 'value', index: 'value', sortable: false, editable: true,editrules:{number:true},sorttype:'number',formatter:'number' },
                        { name: 'Upper_Spec_Value', index: 'Upper_Spec_Value', sortable: false,  editable: true,editrules:{number:true},sorttype:'number',formatter:'number' },
                        { name: 'Lower_Spec_Value', index: 'Lower_Spec_Value', sortable: false,  editable: true,editrules:{number:true},sorttype:'number',formatter:'number' }, 
                        { name: 'GlobalSpec', index: 'GlobalSpec',  editable: true, sorttype:'text', edittype: 'checkbox', editoptions: { value:"1:0" } }, 
                        { name: 'SpecSource', index: 'SpecSource', sortable: true, sorttype:'text',  editable: true, edittype:'select', editoptions: { value: 'user:standard; Interiors:Interiors' } }
                ],
                pager: '#pSpecgrid',
                caption: "Product Spec Entry",
                multiselect: false,
                loadonce: true,
                rowNum: 30,
                viewrecords: true,
                sortorder: "desc",
                width: new Number($("#SectionRibbon").width() - 20),
                gridview: true,
                height: "100%",
                ondblClickRow: function (id) {
                    var idarray = $('#Specgrid').jqGrid('getDataIDs');

                    $.each(idarray, function (index, value) {
                        jQuery('#Specgrid').jqGrid('saveRow', value, true);
                        var rowdata = $('#Specgrid').jqGrid('getLocalRow', value);
                    
                        if (rowdata.Spec_Description == "" && rowdata.Upper_Spec_Value == "" && rowdata.Lower_Spec_Value == "") {
                            $('#Specgrid').jqGrid('delRowData', value);
                        }
                        
                  
                    });

                },
                gridComplete: function () {
                    //$('#gridpager').css('display', 'none');
                    RenderSpecFlag = true;
                    
                }
            });
            jQuery("#Specgrid").jqGrid('navGrid','#pSpecgrid',{
                edit: true,
                add: true,
                del: true,
                search: true,
                searchtext: "Search",
                addtext: "Add",
                edittext: "Edit",
                deltext:"Delete"
            },
            {
                closeOnEscape: true,//Closes the popup on pressing escape key
                reloadAfterSubmit: true,
                afterSubmit: function (response, postdata) { 
 
                    if (response.responseText == "") {
                        $(this).jqGrid('setGridParam', 
                          { datatype: 'json' }).trigger('reloadGrid');//Reloads the grid after edit
                        return [true, '']
                    }
                    else {
                        $(this).jqGrid('setGridParam', 
                          { datatype: 'json' }).trigger('reloadGrid'); //Reloads the grid after edit
                        return [false, response.responseText]
                        //Captures and displays the response text on th Edit window
                    }
                }
            },
            {
                closeOnEscape: true,//Closes the popup on pressing escape key
                reloadAfterSubmit: true,
                afterSubmit: function (response, postdata) { 

                    if (response.responseText == "") {
                        $(this).jqGrid('setGridParam', 
                          { datatype: 'json' }).trigger('reloadGrid')//Reloads the grid after Add
                        return [true, '']
                    }
                    else {
                        $(this).jqGrid('setGridParam', 
                          { datatype: 'json' }).trigger('reloadGrid')//Reloads the grid after Add
                        return [false, response.responseText]
                    }
                }
            },
            {
                closeOnEscape: true,//Closes the popup on pressing escape key
                reloadAfterSubmit: true,
                afterSubmit: function (response, postdata) { 

                    if (response.responseText == "") {
                        $(this).jqGrid('setGridParam', 
                          { datatype: 'json' }).trigger('reloadGrid')//Reloads the grid after Add
                        return [true, '']
                    }
                    else {
                        $(this).jqGrid('setGridParam', 
                          { datatype: 'json' }).trigger('reloadGrid')//Reloads the grid after Add
                        return [false, response.responseText]
                    }
                },
                delData: {
                    SpecId: function () {
                        var sel_id = $("#Specgrid").jqGrid('getGridParam', 'selrow');
                        var value = $("#Specgrid").jqGrid('getCell', sel_id, 'SpecId');
                        return value;
                    }
                }
            },
            {//SEARCH
                closeOnEscape: true
            });
            //jQuery("#Specgrid").jqGrid('navGrid', "#pSpecgrid", { edit: false, add: false, del: false });
            //jQuery("#Specgrid").jqGrid('inlineNav', "#pSpecgrid");
        },
        UpdateSpecTable: function (TabTitleInsert) {
            var mydata = $('#Specgrid').jqGrid('getGridParam', 'data');
            //var TabTemplateId = controlhandler.tabarray[datahandler.selectedtab].TabTemplateid;
            
            if (mydata.length > 0) {
                var SpecJsonString = JSON.stringify(mydata);
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
                    type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
                    data: { method: 'UpdateSpecTable', args: { TabTitle: TabTitleInsert, JsonString: SpecJsonString, TemplateId: datahandler.selectedtemplateid } },
                    success: function (data) {
                        
                    },
                    error: function (a, b, c) {
                        console.log("dbtrans.DeleteTemplateError");
                        alert("SpecTable NotSaved")
                    }

                });

            }
        },
        RenderDefectTypesGrid: function () { 
            var $deft = $("#DefectTypesgrid");
            $("#DefectTypesgrid").jqGrid({
                datatype: "local",
                editurl: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility_DefTyp.ashx',
                colNames: ['Actions', 'ButtonId', 'DefectCode', 'Name', 'Hide'],
                colModel: [
                        {name:'act',index:'act', width:75,sortable:false},
                        { name: 'ButtonId', index: 'ButtonId', editable: true, hidden: true},
                        { name: 'DefectCode', index: 'DefectCode', editable: true, width: 50 },
                        { name: 'Name', index: 'Name', sortable: false, width: 200, editable: true },
                        { name: 'Hide', index: 'value', sortable: false, width: 50, editable: true,edittype:"checkbox",editoptions: {value:"true:false"} }
                ],
                pager: '#pDefectTypesgrid',
                caption: "Defect Types Maintenance",
                multiselect: false,
                loadonce: false,
                rowNum: 10,
                viewrecords: true,
                sortorder: "desc",
                width: new Number(300),
                gridview: true,
                height: "100%",
                data: ButtonLibrary,
                ondblClickRow: function (id) {
                    if(id && id!==lastSel){ 
                        jQuery("#DefectTypesgrid").restoreRow(lastSel); 
                        lastSel=id; 
                    }
                    jQuery("#DefectTypesgrid").editRow(id, true);

                },
                gridComplete: function () {
                    //$('#gridpager').css('display', 'none');
                    var ids = jQuery("#DefectTypesgrid").jqGrid('getDataIDs');
                    for(var i=0;i < ids.length;i++){
                        var cl = ids[i];
                        be = "<input style='height:22px;width:20px;' type='button' value='E' onclick=\"jQuery('#DefectTypesgrid').editRow('"+cl+"');\"  />"; 
                        se = "<input style='height:22px;width:20px;' type='button' value='S' onclick=\"jQuery('#DefectTypesgrid').saveRow('"+cl+"');\"  />";  
                        jQuery("#DefectTypesgrid").jqGrid('setRowData',ids[i],{act:be+se});
                    }	
                }
            });
        }
    
    };
    function processAddEdit() {
        dbtrans.RefreshManagerGrid();
        
        $(".ui-icon-closethick").trigger('click');
        //alert("* " + response.responseText + " *");
    }
    function reload() { 
        
        $.ajax({
            url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionUtility.ashx',
            type: 'POST',
            data: { method: 'GetTemplateTable' },
            success: function (data) {

                var json;
                   
                GridManagerTable = $.parseJSON(data);
               
            },
            error: function (a, b, c) {
                alert(c);
            }
        });

    }
    
    </script>


</asp:Content>

