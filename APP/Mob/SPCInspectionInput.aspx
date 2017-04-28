<%@ Page Title="APR" Language="VB" EnableEventValidation="false" MasterPageFile="~/APP/MasterPage/Mobile.master" AutoEventWireup="false" CodeFile="SPCInspectionInput.aspx.vb" Inherits="core.APP_DataEntry_SPCInspectionInput" %>
<%@ Import Namespace="System.Web.Optimization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent"  Runat="Server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="hidden">
            <input type="hidden" value ="False" id="Authenticated_hidden" runat="server" />
           </div> 
                <div id="loginfrm" style="Z-INDEX: 110; height: 320px; width: 425px; position: absolute; top:125px; left:38%; display:none" >
				    <div id="LocationSelection">
                    <ASP:LABEL id="lblUserID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 30px"
					runat="server" font-bold="True" Font-Size="Large">CORPORATE NAME:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: relative; TOP: 30px">
                    <div style="Z-INDEX: 102; LEFT: 100px; POSITION: relative; TOP: -5px">
                        <select id="LocationNames_pop" name="LocationNames_pop" style="width: 162px; height: 55px; "></select>
               
                                </div>
                            </div>
                        </div>
                    <div id="MachineSelection">
                    <ASP:LABEL id="lblMachineName" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 30px"
					runat="server" font-bold="True" Font-Size="Large">MACHINE NAME:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: relative; TOP: 30px">
                    <div style="Z-INDEX: 102; LEFT: 100px; POSITION: relative; TOP: -5px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
                        <select id="MachineNames_pop" name="MachineNames_pop" style="width: 162px; height: 55px; "></select>
               
                                </div>
                            </div>
                        
                        </div>
                    <div id="WorkOrderSelection">
                        <input id="closeout1" type="button" class="export closebox" value="X" style="position:relative; float:right;TOP: 5px; width:30px; height:30px;" />
                    <ASP:LABEL id="lblWorkOrderID" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 30px"
					runat="server" font-bold="True" Font-Size="Large">OPEN WORK ORDERS:</ASP:LABEL>
                <div style="Z-INDEX: 105; LEFT: 100px; POSITION: relative; TOP: 70px">
                    <div style="Z-INDEX: 102; LEFT: -70px; POSITION: relative; TOP: -5px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
                        <select id="WorkOrder_pop" name="WorkOrder_pop" style="width: 325px; height: 55px; "></select>
                                
                                </div>
                            <label style="left: -40px; position: relative;-webkit-appearance: none; width: 70px;height: 70px;background: white;border-radius: 5px;zoom: 1.7;border: 1.5px solid #555;"><input id="MachineLinkCheck" type="checkbox" name="checkbox" value="value" >Link To Machine</label>
                    <ASP:LABEL runat="server" Width="280px" Style="word-wrap: normal; position:relative; left:-46px; word-break: break-all;" > Linking a Machine will automatically set the WorkOrder Information Per Machine Continuously.  click NEW or Exit Page to Exit.</ASP:LABEL>
                            </div>
                        </div>
                    <div id="JobConfirmation">
                        <div id="JobInfoDiv">
                            <input id="closeout2" type="button" class="export closebox" value="X" style="position:relative; float:right;TOP: 5px; width:30px;  height:30px;" />
                            <ASP:LABEL id="lblConfirmation" style="Z-INDEX: 104; POSITION: relative; float:right;left:-15px; TOP: 5px"
					        runat="server" font-bold="True" Font-Size="Large">JOB CONFIRMATION</ASP:LABEL>
                            
                                <ASP:LABEL id="lblJobNumber" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 40px"
					        runat="server" Font-Size="medium">JobNumberID:</ASP:LABEL>
                                <label id="JobNumberValue" style="font-size:medium; position: absolute; top:42px; left:340px;">0</label>
                                <ASP:LABEL id="lblJobFailureCount" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 70px"
					        runat="server" Font-Size="medium">Job Failure Count:</ASP:LABEL>
                                <label id="FailCountValue" style="font-size:medium; position: absolute; top:70px; left:340px;">0</label>
                            <ASP:LABEL id="lblJobPassCount" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 100px"
					        runat="server" Font-Size="medium">Job Pass Count:</ASP:LABEL>
                                <label id="PassCountValue" style="font-size:medium; position: absolute; top:100px; left:340px;">0</label>
                            <ASP:LABEL id="lblJobTotalCount" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 145px"
					        runat="server" Font-Size="medium">Total Items Inspected:</ASP:LABEL>
                            <div id="TotalCountValueDiv" style="position:absolute; top:130px; left:50%;">
                                <input type="text" id="TotalCountValue" value=0 style="width: 157px; height: 57px; position:relative; top: -3px;" />
                            </div>
                            </div>
                        <div id="JobAttachDiv" style="Z-INDEX: 105; LEFT: 0px; POSITION: relative; TOP: 150px; width:100%;">
                            <div style="Z-INDEX: 102; LEFT: 10px; POSITION: relative; TOP: 55px">
                                <ASP:LABEL id="lblcomments" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: -14px"
					                    runat="server" font-bold="True" Font-Size="Small">JOB COMMENTS</ASP:LABEL>
                                <textarea id="JobMessage" name="JobMessage" style="width:90%;height:55px; position: absolute; left:1%;" runat="server"></textarea>
                                        <div id = "EmailAlertFlag" style="position:absolute; top:-200px; left: 5px;">
                                                <input id="AddEmailFlag" type="checkbox" class=chkbox runat=server />Alert Email</div>
                                        </div>
                                    </div>
                        <asp:Button ID="Confirm" runat="server" OnClientClick="SubmitOnce()" style="position:absolute; top:265px; left:50%; background:rgb(116,180, 116); color: white; text-align: center; width: 100px;" class="export" text="CONFIRM" />
                        </div>
                    <div id="RollConfirmation">
                        <div id="RollInfoDiv">
                            <input id="closeout5" type="button" class="export closebox" value="X" style="position:relative; float:right;TOP: 5px; width:30px;  height:30px;" />
                            <ASP:LABEL id="lblRollConfirmation" style="Z-INDEX: 104; POSITION: relative; float:right;left:-15px; TOP: 5px"
					        runat="server" font-bold="True" Font-Size="Large">JOB CONFIRMATION</ASP:LABEL>
                             <div id = "EmailAlertFlagRoll" style="position:absolute; top:-5px; left: 5px;">
                                                <input id="AddEmailFlagRoll" type="checkbox" class=chkbox runat=server />Alert Email</div>
                                <ASP:LABEL id="lblRollNumber" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 40px"
					        runat="server" Font-Size="medium">RollNumber:</ASP:LABEL>
                                <label id="RollNumberValue" style="font-size:medium; position: absolute; top:42px; left:310px;">0</label>
                                <ASP:LABEL id="lblRollFailureCount" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 70px"
					        runat="server" Font-Size="medium">Job Failure Count:</ASP:LABEL>
                                <label id="FailRollValue" style="font-size:medium; position: absolute; top:70px; left:310px;">0</label>
                            <ASP:LABEL id="lblRollTotalCount" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: 105px"
					        runat="server" Font-Size="medium">Total Yards Inspected:</ASP:LABEL>
                            <div id="TotalYardValueDiv" style="position:absolute; top:100px; left:250px;">
                                <input type="text" id="TotalYardValue" value=0 style="width: 157px; height: 57px; position:relative; top: -3px;" />
                            </div>
                            <asp:Label ID="lblWeaverShiftYards" style="z-index:104; left:10px; position:absolute; top: 155px"
                            runat="server" Font-Size="Medium">Weaver Shift Yards:</asp:Label>
                            <div style="position:absolute; top:150px; left: 250px;">
                                <input type="text" id="WeaverShiftYards" value=0 style="width:157px; height:57px; position:relative; top:-3px;" />
                            </div>
                            </div>
                        <div id="RollAttachDiv" style="Z-INDEX: 105; LEFT: 0px; POSITION: relative; TOP: 150px">
                            <div style="Z-INDEX: 102; LEFT: 10px; POSITION: relative; TOP: 55px">
                                <ASP:LABEL id="lblcommentsRoll" style="Z-INDEX: 104; LEFT: 10px; POSITION: absolute; TOP: -14px"
					                    runat="server" font-bold="True" Font-Size="Small">JOB COMMENTS</ASP:LABEL>
                                <textarea id="RollMessage" name="RollMessage" style="width:380px;height:55px; position: absolute; left:10px;" runat="server"></textarea>
                                       
                                        </div>
                                    </div>
                        <asp:Button ID="ConfirmRoll" runat="server" style="position:absolute; top:265px; left:300px; background:rgb(116,180, 116); color: white; text-align: center; width: 100px;" class="export" text="CONFIRM" />
                        </div>
                    &nbsp;&nbsp;
                  </div>
                  <div id="JobStart-confirm" style="display:none;" title="Existing Job Found.">
                  <p><span class="ui-icon ui-icon-alert" style="float:left; margin:12px 12px 20px 0;"></span>This WorkOrder is already open and has the ID <label id="JobStart-confirm-jobid"></label>, AQL <label id="JobStart-confirm-aql"></label> Would you like to continue the previous or begin a new AQL. Please select an option below.</p>
                </div>
    <input type="hidden" id="HiddenProduct" runat="server" />
    <input type="hidden" id="InspectionState" class="inputelement" runat="server" />
    <input type="hidden" id="workorder_hidden" runat="server" />
    <input type="hidden" id="workroom_hidden" value ="nan" class="inputelement" runat="server" />
    <input type="hidden" id="woquantity_hidden" runat="server"  class="inputelement" value="0" />
    <input type="hidden" id="wopieces_hidden" class="inputelement" runat="server" value="0" />
    <input type="hidden" id="inspectionjobsummaryid_hidden" class="inputelement" runat="server" value="0" />
    <input type="hidden" id="WeaverShiftId_hidden" class="inputelement" runat="server" value="0" />
    <input type="hidden" id="jobconfirmation_hidden" class="inputelement" runat="server" value="0" />
    <input type="hidden" id="Weaver_Names_hidden" class="inputelement" runat="server" value="0" />
    <input type="hidden" id="WeaverShiftYards_hidden" runat="server" value="0" />
    <input id="_AQLevel" type="hidden" runat="server" value="2.5" class="inputelement" />
    <input id="aqlstandard" type="hidden" runat="server" value="regular" class="inputelement" />
    <input id="totalinspecteditems" type="hidden" runat="server" value="0" />
    <input id="totalinspectedyards" type="hidden" runat="server" value="0" />
    <input id="Templatename" type="hidden" runat="server" value="NoName" />
    <input id="AuditorNameHidden" type="hidden" runat="server" value="NoName" />
    <input id="SampleSizeHidden" type="hidden" runat="server" value="0" />
    <input id="DHYHidden" type="hidden" runat="server" value="0" />
    <input id="REHidden" type="hidden" runat="server" value="0" />
    <div id="CompleteDiv" style="position:absolute; left: 360px; top: -10px; width: 350px;">
        <%--<asp:Button ID="Pass" runat="server" style="position:absolute; top: 25px; background:rgb(116,180, 116); color: white; text-align: center; width: 100px;" class="export" text="PASS" />--%>
        <input id="completeInspec" type="button" style="position:relative; top: 25px; background:rgb(116,180, 116); color: white; text-align: center; width: 180px;" class="export endjob leftsidedropobj" value="INSPECTION COMPLETE"></input>
    </div>
        <div id="NewPageDiv" style="position:absolute; left: 560px; top: 15px; width: 350px;" class="">
            <input id="NewPage" type="button" value="NEW" class="export" style="position:relative;height: 35px; width:90px; height: 52px; ""></input>
        </div>
        
             <%--<input id="Fail" style="position:relative; top: -27px; left:120px; background:rgb(227, 10, 10); color: white; text-align: center; width: 100px;" class="export endjob" value="FAIL JOB"/>--%>
       <%-- <asp:Button ID="NewJob" runat="server" style="position:absolute; top: 25px; left: 120px; width: 100px; color: white; text-align: center;" class="export" text="NEW INS."/>
        <asp:Button ID="NewSample" runat="server" style="position:absolute; top: 25px; left: 240px; width: 105px; color: white; text-align: center;" class="export" text="NEW SAMPLE" OnClick="NewJob_Click"/>--%>
        
    
    <div id="scorelabels" style="position:absolute; left:69%; top:-5px; width: 400px; height: 100px; display:none;">
        <label id="DHULabel" style="position:absolute; top:3px; left: 0px; font-size:medium; z-index:100; color:black; width: 150px;">DHU</label>
        <input id="DHU" readonly  class="inputelement inputbox" type="text" style="top:25px; left: 0px; width: 60px; height: 53px;" value="0" runat="server"  />
        <label id="SampleSizeLabel" style="position:absolute; top:3px; left: 130px; font-size:medium; z-index:100; color:black; width: 150px;">SAMPLE SIZE</label>
        <input id="SampleSize" readonly  class="inputelement inputbox" type="text" style="top:25px; left: 100px; width: 60px; height: 53px;" runat="server"  />
        <label style="position:absolute; top:3px; left: 230px; font-size:medium; z-index:100; color:black; width: 150px;">AC</label>
        <input id="AC" readonly  class="inputelement inputbox" type="text" style="top:25px; left: 220px; width: 40px; height: 16px;" runat="server"  />
        <label style="position:absolute; top:43px; left: 230px; font-size:medium; z-index:100; color:black; width: 150px;">GOOD</label>
        <input id="Good" readonly  class="inputelement inputbox" type="text" style="top:65px; left: 220px; width: 40px; height: 16px;" runat="server" value="0" />
        <label style="position:absolute; top:3px; left: 315px; font-size:medium; z-index:100; color:black; width: 150px;">RE</label>
        <input id="RE" readonly  class="inputelement inputbox" type="text" style="top:25px; width: 40px; height: 16px; left: 305px;" runat="server"   />
        <label style="position:absolute; top:43px; left: 315px; font-size:medium; z-index:100; color:black; width: 150px;">BAD</label>
        <input id="Bad_Local" readonly  class="inputelement inputbox" type="text" style="top:65px; width: 20px; padding-left: 10px; height: 16px; left: 305px" runat="server" value="0"   />
        <input id="Bad_Group" readonly  class="inputelement inputbox" type="text" style="top:65px; width: 20px; padding-left: 10px; height: 16px; left: 345px" runat="server" value="0"   />
    </div>
    <div style="Z-INDEX: 102; LEFT: 10px; POSITION: absolute; TOP: 100px">
                <%--<input name="select-2" id="TemplateId" style="display:none; width: 162px; height: 20px; " class="inputelement" ></input>--%>
        
        <button id="InspectionType" type="button" style="background-color: rgb(149, 187, 210); display:none; height: 35px; width: 75px; position: absolute; left:170px; top: 0px;">WORK ORDER</button>   
            </div>
    <div id="menudiv" style="display: none; position: relative; top: 55px;left: 2px; width: 99.9%; height:50px; ">
    <ul id="menu" style="position:relative; top:-45px;" >
            <li><a>MAIN MENU</a>
            </li>
            <li><a>INSPECTION</a>
                <ul>
                   <li><a>INPUT DEFECTS</a></li>
                   <li><a>TEMPLATE UTILITY</a></li>
                   <li><a>RESULTS</a></li>
                </ul>
            </li>
           
            <li><a>DASHBOARD</a></li>
        </ul>
        <div id="MachineDiv" style="position:relative; top: -47px;width: 170px;float:right;border-right-style: solid;border-color: rgb(123, 122, 122);border-width: thick;z-index: 200;height: 34px; display:none;">
            <label id="MachineLbl" style="position: relative; top: 4px; left: 5px; font-size:medium; font-weight:700; z-index:100; color:black; width: 150px;border-left-width: think;/* border-color: rgb(131, 134, 136); *//* border-right-style: solid; *//* border-left-style: solid; */height: 34px;">  </label>
            
        </div>
        <div style="position:relative;top: -47px;width: 170px;float:right;border-right-style: solid;border-left-style: solid;border-color: rgb(123, 122, 122);border-width: thick;z-index: 200;height: 34px;">
            <label style="position: relative; top: 4px; left: 5px; font-size:medium; font-weight:700; z-index:100; color:black; width: 150px;border-left-width: think;/* border-color: rgb(131, 134, 136); *//* border-right-style: solid; *//* border-left-style: solid; */height: 34px;">   Inspection ID: </label>
            <input id="InspectionId" class="inputelement" runat="server" style="position:relative; top:4px; left: 4px; font-size:medium; font-weight:700; z-index:100; color:black; width: 50px; border:none; background-color: transparent;" value="0" />
            <span id="jobIdSpinner" style="position:relative; float:right; top:-30px; right:170px; display: none">
                <img style="position:relative; float:right; width:75px;" src="../../Images/load-indicator.gif" />
            </span>
            
        </div>
        <div id="CountToTargetDiv" style="position: relative;display:none;top: -47px;float:right; width: 150px;border-right-style: solid; border-left-style: solid;border-color: rgb(123, 122, 122);border-width: thick;z-index: 200;height: 34px;">
            <label style="position: relative; top: 4px; left: 5px; font-size:medium; font-weight:700; z-index:100; color:black; width: 150px;border-left-width: think;/* border-color: rgb(131, 134, 136); *//* border-right-style: solid; *//* border-left-style: solid; */height: 34px;">   Ins. To Target: </label>
            <input id="CountToTarget_Input" class="inputelement" runat="server" style="position:relative; top:4px; left: 4px; font-size:medium; font-weight:700; z-index:100; color:black; width: 30px; border:none; background-color: transparent;" value="0" />
        </div>
        <input id="EnterSpec" type="button" value="Enter Spec" class="export" style="position:absolute; width: 90px; height: 35px;font-size: .85em; left: 91%; top:60px;"></input>
        
        <input id="EnterProductSpec" type="button" value="REPORT SPEC" class="export" style="position:absolute; width: 90px; height: 35px;font-size: .85em; left: 91%; top:150px;"></input>

        <div style="position:absolute; width: 90px; height: 35px;font-size: .85em; left: 91%; top:260px;">
            <img src="../../Images/global-technical-specs.jpg" id="GlobalSpecsImage" style="height:80px; z-index: 1000;" />
        </div>

    </div>
    <div id="tabs" style="width:63%; top:135px; height:75%; left:260px; position:absolute;">
        <ul> </ul>


    </div>
    <div id="loading" style="width:74%; top:137px; height:75%; left:264px; position:absolute; display: none;">
        <div style="width:100%; top:7px; height:100%; position:absolute;z-index:0; background-color:lightgray; opacity:.4;"></div>
        <input type="image" src="../../Images/load-indicator.gif" style="z-index:3; margin-left: 41%; margin-top:20%; position:absolute;" />
    </div>
    <div id="Default" style="width:74%; top:137px; height:75%; left:264px; position:absolute; display: none;">
        <div style="width:100%; top:7px; height:100%; position:absolute;z-index:0; background-color:lightgray; opacity:.4;"></div>
        <input type="image" src="../../Images/TemplateInspection.jpg" style="z-index:3; margin-left: 8%; margin-top:8%; height: 75%; position:absolute;" />
    </div>
    <div id="SpecTable" style="display:none; z-index:210; position:absolute; float:left; width:19.7%; height:1100px; top:40px; left:5%; background: transparent !important;">
        <div style="position:absolute; top:0px;">
            <input id="closeout3" type="button" class="export closebox" value="X" style="position:relative; float:right;TOP: 0px; left: 35px; width:30px; height:30px;" />
        <table id="Specgrid" style=" font-size:medium; Z-INDEX: 104; font-weight:800; ">
                                </table>
            <input id="NextItem" type="button" class="export" value="NEXT ITEM" style="position:relative; float:LEFT;TOP: 20px; left: 15px; width:100px; height:50px;" />
            <label id="ItemNumberLabel" for="NextItem" style="color:white; font-size:xx-large; position: relative;left: 50px; top: 30px;">Item #: 1</label>
            <div id="pSpecgrid" ></div>
        </div>
        <%--<div id="SpecButton" style="position:relative; top:95px; left:295px; width: 360px;">
                            <input id="SpecAdd" type="button" class="export" style="height: 30px; width:70px" value="SAVE" runat="server" />
                            <input type="text" id="SpecErrorText" style="position:relative; top: -25px; left: 80px;border: none;font-weight: 800;color: red;display: none; width:300px;">
                        </div>--%>
    </div>
    <div id="LoadWorkOrderDiv" class="leftsidedropobj" style="left: 0px;width: 250px;height: 100px;top:180px;position:absolute;">
        <select id="selectNames" style="width: 97%; top: -40px; height: 35px; position: absolute; left: 10px; "></select>
        <%--<input id="LoadSPCMachine" type="button" value="SPCMachine" class="export" style="position:absolute; width: 90px; height: 35px;font-size: .85em; left: 150px;"></input>--%>
        <input id="StartInspection" type="button" value="START" class="export" style="position:absolute; width: 97%; height: 35px;font-size: .85em; left: 10px;"></input>
        
    </div>
    <div id="workorderDiv" style="left: 0px;width: 250px;height: 100px;top:240px;position:absolute;" class="leftsidedropobj">
            <input id="LoadOpenWorkOrder" type="button" value="Open WorkOrders" class="export" style="position:relative;height: 35px; width:97%; left: 10px;"></input>
    </div>
    <div id="InputHolder" style="left: 0px; top: 295px; position: absolute; display: none;">
        <div id="workdiv" class="inputpad leftsidepanel workorder-inspection">
            <label for="WorkOrderL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">WORK ORDER</label>
            <label for="PullDataL" class="label-tab">Pull Data</label>

        </div>
        <div id="rolldiv" class="inputpad leftsidepanel roll-inspection" style="display: none;">
            <label for="rollL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">ROLL NUMBER</label>
            <label for="PullDataL" class="label-tab">Pull Data</label>

        </div>
        <div id="itemdiv" class="inputpad leftsidepanel" style="display: none;">
            <label for="itemL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">ITEM NUMBER</label>           
        </div>
        <div id="workinputholder" style="position:absolute; left:0px; width: 250px" class="leftsidepanel workorder-inspection">
            <input id="WorkOrder" class="inputelement inputbox  leftsideobj" type="text" runat="server"  />
        </div>
        <div id="rollnumberholder" style="position:absolute; left:0px; width: 250px; display:none;" class="leftsidepanel roll-inspection">
            <input id="RollNumber" class="inputelement inputbox leftsideobj" type="text" runat="server"  />
        </div>
        <div id="itemnumberholder" style="position:absolute; left:0px; width: 250px; display:none;" class="leftsidepanel">
            <input id="ItemNumber" class="inputelement inputbox leftsideobj" type="text" runat="server"  />
        </div>

        <div id="cartondiv" class="inputpad leftsidepanel" style="position:absolute; top: 64px; display:none;">
            <label for="CartonNumberL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">CARTON NUMBER</label>
            <input id="CartonNumber" class="inputelement inputbox leftsideobj" type="text" runat="server"   />

        </div>
        <div id="workroomdiv" class="inputpad leftsidepanel workorder-inspection" style="position:absolute; top: 64px;">
            <label for="workroomL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">WORKROOM</label>
            <input id="workroom" class="inputelement inputbox leftsideobj" type="text" runat="server"   />

        </div>
        <div id="loomdiv" class="inputpad leftsidepanel roll-inspection" style="position:absolute; top: 64px; display:none;">
            <label for="LoomNumberL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">LOOM NUMBER</label>
            <input id="LoomNumber" class="inputelement inputbox leftsideobj" type="text" runat="server"   />

        </div>
        
        <div id="cpdiv" class="inputpad leftsidepanel workorder-inspection" style="position:absolute; top: 128px;">
            <label for="CPNumberL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">CASE PACK</label>
            <input id="CPNumber" class="inputelement inputbox leftsideobj" type="text" runat="server"   />

        </div>
        <div id="inspectordiv" class="inputpad leftsidepanel roll-inspection" style="position:absolute; top: 128px; display:none;">
            <label for="InspectorL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">INSPECTOR</label>
            <input id="Inspector" class="inputelement inputbox leftsideobj" type="text" runat="server" value=""  />

        </div>
        
        <div id="datanumberdiv" class="inputpad leftsidepanel" style="position:absolute; top: 192px;">
            <label for="DataNumber" id="DataNumberL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">DATA NUMBER</label>
            <input id="DataNumber" class="inputelement inputbox leftsideobj" type="text" runat="server"   />

        </div>
        <div class="inputpad leftsidepanel" id="auditdiv" style="position:absolute; top: 256px;">
            <label for="AuditorName" id="AuditorNameL" class="workorder-inspection" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">AUDITOR NAME</label>
            <input id="AuditorName" class="inputelement inputbox" type="hidden" runat="server"   />
            <select id="Auditor_Name" class="inputelement inputbox leftsidedropobj  workorder-inspection" style="width: 185px;"></select>
            <label for="Weaver_Names" id="WeaverNamesL" class="roll-inspection" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">WEAVER NAMES</label>
            <input id="Weaver_Names" class="inputelement inputbox leftsideobj roll-inspection" runat="server" type="text" />
            <div style="position:relative; top:20%; margin-right:10px;">
                <label id="AddWeaverL" class="roll-inspection label-tab">Add</label>
            </div>
            
        </div>
        <div class="inputpad leftsidepanel" style="position:absolute; top: 320px;">
            <label for="LocationL" style="position:absolute; top:3px; left:10px; font-size:smaller; z-index:100; color:white;">LOCATION</label>

            <select id="Location" disabled runat="server" style="width:185px;"    class="inputelement inputbox leftsidedropobj"></select>
        </div>
        <div class="inputpad leftsidepanel" style="position:absolute; top: 385px;" id="LotSizeDiv">
            <label for="WOQuantity" id="WOQuantityL" style="position:absolute; top:3px; left: 10px; font-size:smaller; z-index:100; color:white;">WO QUANTITY</label>
            <div style="position: relative; top:15px; left:10px;">
                <input id="WOQuantity" class="inputelement leftsidedropobj" style="height:30px; width:149px; padding-left: 30px; text-align: left;" type="text" value ="0"   />
            </div>
        </div>
        <div id="AQdiv" style="Z-INDEX: 102; LEFT: 10px; POSITION: absolute; TOP: 450px; width:250px;" class="leftsidepanel">
                <input name="select-3" id="AQLevel" style="width: 162px; height: 20px; display:none;" class="inputelement " ></input>
                <select id="AQ_Level" style="width:97%; height: 35px; " class="inputelement leftsidedropobj"></select>
                
            </div>
        <div id="ImageCapture" style="z-index:102; left:-9px; position:absolute; top:490px; display:none;">
            <span class="fileUpload btn btn-primary">
                <span class="uploadButton">Add Picture</span>
                    <form method="post" enctype="multipart/form-data" id="myImageForm">
                      <asp:FileUpload id="FileUploadControl" runat="server" />
                        <asp:Button runat="server" id="UploadButton" text="Upload" OnClientClick="setnavcop(true)"/>
                        <br /><br />
                        <asp:Label runat="server" id="StatusLabel" text="Upload status: " style="position:absolute; left:0px;" />
                    </form>
                     <%-- <input id="ImageSubmit" value="Upload" class="buttontemplate" style="width: 100px; height: 50px; font-size: 15px; margin: 4px 0px 0px; padding: 1px 1px; text-align:center;">--%>
                    <label id="StatusResult" runat="server" style="position:absolute; left:90px;"></label>
                    <label id="DefectFlag" style="position:relative; top:20px; float: left;" >[No Selection]</label>

                </span>
        </div>
        
            <input type="hidden" runat="server" id="DefectID_Value" value ="0" />
        
        <div id="dialog" title="SPC Inspection">
            <p>You found a defect!  Are you Sure?</p>
            <div id = "Chk1" style="position:absolute; top: 120px; left: 5px; padding: 0px;" class=SheetClass1>
                                        <input id="Skip" name="ChkBx1" type="checkbox" class=chkbox2 />Skip All Confirmations.</div>
        </div>
        <div id="StartInspectiondialog" title="START Inspection?">
            <div id="StartInspectionRollDiv">
                <p>TO START AN INSPECTION CLICK CONFIRM.</p>
            </div>
            <div id="StartInspectionWODiv">
                <p>TO START AN INSPECTION CHOOSE AN AQL LEVEL AND STANDARD.</p>
                <div style="position:absolute; right:50px;">
                    <label style="position:absolute; left: -80px;" for="AQL_Level_Dialog">AQL Level</label>  
                                <select id="AQL_Level_Dialog" name="AQL_Level_Dialog">
                                </select>
                </div>
                <div style="position:absolute; top:95px; right:50px;">
                    <label style="position:absolute; left: -110px;" for="AQL_Level_Dialog">AQL Standard</label>  
                                <select id="AQL_Standard_Dialog" name="AQL_Standard_Dialog" style="position: relative;">
                                </select>
                </div>
                
            </div>
            <div id = "Chk1" style="position:absolute; top: 150px; left:5px; padding: 0px;" class=SheetClass1>
                                            <input id="Skip" name="ChkBx1" type="checkbox" class=chkbox2 />Skip All Confirmations.</div>
        </div>
        <div id="ProductSpecEntrydialog" title="Manual Spec Entry">
            <p>Product Spec Info below will be locally added now.  Globally added on QA Manager approval</p>

            <div style="position:absolute; float:right; left:165px; top:75px;">
                <label style="position:absolute; left: -140px;" for="MS_ProductType">Product Type</label>  
                    <input id="MS_ProductType" type="text" style="height:40px;" />
                    
            </div>
            <div style="position:absolute; float:right; left:165px; top: 150px;">
                <label style="position:absolute; left: -140px;" for="MS_ProductDesc">Spec Description</label>  
                    <input id="MS_ProductDesc" type="text" style="height:40px;" />
                    
            </div>
            <div style="position:absolute; left:165px; top: 217px;">
                <label style="position:absolute; left: -140px;" for="MS_ProductValue">Spec Value</label>  
                    <input id="MS_ProductValue" type="text" style="height:40px; width:162px; text-align:left;" />
                    
            </div>
            <div style="position:absolute; left:165px;top: 290px;">
                <label style="position:absolute; left: -140px;" for="MS_ProductValue">Upper Spec Value</label>  
                    <input id="MS_Upper_Spec_Value" type="text" style="height:40px; width:162px; text-align:left;" />
                    
            </div>
            <div style="position:absolute; left:165px; top: 367px;">
                <label style="position:absolute; left: -140px;" for="MS_ProductValue">Lower Spec Value</label>  
                    <input id="MS_Lower_Spec_Value" type="text" style="height:40px; width:162px; text-align:left;" />
                    
            </div>
            <div style="position:absolute; left:165px; top: 450px;">
                <label id="MS_ErrorLbl" style="position:absolute; width:400px; left: -140px; color:red"></label>  
            </div>
        </div>
       <div id="NewAuditorName" title="Add AuditorName" style="display:none" class="dialogdiv"> 
                    <fieldset class="ui-helper-reset"> 
                        <div class="Auditor-addname">
                            <label id="AuditorLabel" class="Auditor-addname" > 
                                Auditor Name</label> 
                            <input id="Name" class="Auditor-addname" style="height:34px !important;"> 
                        </div>
                        <div class="Weaver-addname" style="display:none;"> 
                            <div style="position:relative; height:100px; margin-bottom:20px; display: none;" class="Weaver-addyards"> 
                                <label id="WeaverYardsLabel" for="input-weaveryards">
                                previous Weaver Shift Yards: 
                                </label>
                                <input type="number" id="weaveryards" name="yards" min="1" style="height:34px !important; margin-left:30px; margin-top:10px;"/>
                            </div>
                            <div style="position:relative; height:170px; width:100%;">
                                <label id="WeaverLabel" class="Weaver-addname Weaver-addname-label" for="select-weavername">
                                Select New Weaver Name:</label>
                                <select id="select-weavername" class="select-mobile" ></select>
                                <div style="position:absolute; height:90px; top:75px; left: 30px;"> 
                                    <label id="PreConfirmWeaver1Label" class="Weaver-addname-label" style="color:rgba(128,126, 126, 0.84); font-style:italic!important; margin-top:0px; position:absolute;"></label>
                                    <label id="PreConfirmWeaver2Label" class="Weaver-addname-label" style="color:rgba(128,126, 126, 0.84); font-style:italic!important; margin-top:25px; position:absolute;"></label>
                                </div>   
                                <p style="position:relative; top:80px; left:-100px;"><a href="http://inspection.standardtextile.com/Manage/employees">Add Weaver Here</a></p>                 
                            </div>
                            
                            
                        </div>
                    </fieldset> 
                </div> 
    </div>
    <div id="ActionHolder" style="position:absolute;top:800px;">
        <div id="MobileDirectorDiv" style="Z-INDEX: 102; LEFT: 10px; POSITION: relative; TOP: 0px; width:250px;" class="">
            <input id="MobileDirector" type="button" class="export" style="position:relative; height: 35px; width:97%;" value="PHOTO" />
        </div>
        
        
    </div>
    <input id="LocationSelected_Hidden" runat="server" type="hidden" value ="0" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" Runat="Server">
<asp:PlaceHolder runat="server">
    <%: Scripts.Render("~/bundles/InspectionInput_groupA") %>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server">
    <%: Styles.Render("~/bundles/InspectionInput_styles") %>
</asp:PlaceHolder>
<%--<link href="../../Styles/wijmo/jquery-wijmo.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/wijmo/jquery.wijmo-pro.all.3.20141.34.min.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/ui.jqgrid.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">--%>
<meta charset=utf-8 />
<meta name="viewport" content="width=device-width, initial-scale=1">
   
<style>
        .grid .ui-jqgrid-htable th,
        .grid .ui-jqgrid-btable .jqgrow td {
            height: 5em !important;
        }
        .ui-jqgrid .ui-jqgrid-titlebar {
        padding: .3em 14.95em .2em .3em;
        position: relative;
        font-size: 12px;
        border-left: 0 none;
        border-right: 0 none;
        border-top: 0 none;
        width: 100px;
        }
        .ui-widget-overlay {
      background: transparent !important;
      opacity: .30;
      filter: Alpha(Opacity=30);
    }
    .wijmo-wijsuperpanel-statecontainer {
        height: 60px !important;
    }
    .wijmo-wijsuperpanel-panelContainer {
        height: 60px !important;
    }
    .ui-tabs-top .scrollWrap {
        border-bottom-width: 0px;
        height: 60px !important;
    }
    .ui-spinner .ui-widget .ui-widget-content .ui-corner-all {
        height: 45px !important;
    }
    option {
        height: 55px;
        padding: 5px 5px;
    }
    .jqgrow {
        height:46px;
    }
    .ui-jqgrid-title {
        position:absolute !important;
    }
    .ui-state-default .ui-corner-top {
        height: 56px;
    }.ui-jqgrid-view {
            background-color: #cccccc;
        }
     .select-mobile {
        height: 34px !important;
        margin-left: 30px;
        margin-top: 10px;
        position: relative;
        float: left;
        top: 10px;
    }
    </style>
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/InspectionInput_groupB") %>
    </asp:PlaceHolder>
<%--<script src="../../Scripts/jquery.layout.js" type="text/javascript"></script>--%>
<%--<script src="../../Scripts/grid.locale-en.js" type="text/javascript"></script>--%>
<%--<script src="../../Scripts/jquery.jqGrid.js" type="text/javascript"></script>--%>

<style type="text/css">
        .ui-icon.ui-icon-triangle-1-s {
            position:relative;
            right:-5px;
        }
        .ui-icon.ui-icon-triangle-1-n {
            position:relative;
            right:-5px;
        }
        .ui-widget {
            height:92%;
        }
        .wijmo-wijinput {
            right:0%;
            position: relative;
            height: 45px !important;
        }
        .wijmo-wijinput-wrapper {
            margin-right: 50%;
            margin-top:5%;
            display: block;
            padding: 5px;
            overflow: hidden;
        }
        .wijmo-wijinput .wijmo-wijinput-spinner-right {
            position: absolute;
            right: 0%;
            top: 0px;
            width: 57px;
            cursor: pointer;
            height: 100%;
            overflow: hidden;
        }
        .ui-icon {
            width: 16px;
            height: 16px;
            margin-right: 40%;
        }
        .wijmo-wijinput-spinup {
            border-bottom: none;
            display: block;
            height: 50%;
            width: 56px;
            position: relative;
        }
        .wijmo-wijinput-spindown {
            border-top: none;
            display: block;
            height: 50%;
            width: 56px;
            position: relative;
        }
        .elementerror {
            border-color: red; 
            border-width:medium;
        }
        .ui-jqgrid .ui-jqgrid-pager{
                height: 75px !important;
        }
        .ui-jqgrid .ui-pg-table {
            height: 74px !important;
        }
        .ui-jqgrid .ui-pg-button {
            height: 19px !important;
            width: 80px !important;
        }
        .ui-spinner-button {
            width: 16px;
            height: 50% !important;
            /* font-size: .5em !important; */
            padding: 0;
            margin: 0;
            text-align: center;
            position: absolute;
            cursor: default;
            display: block;
            overflow: hidden;
            right: 0;
        }
        .ui-spinner.ui-widget.ui-widget-content.ui-corner-all {
            border: none;
        }
    .ui-corner-top {
        width: 130px;
        height: 57px;
    }
    .ui-icon-close {
        margin-left:auto;
    }
    .workclick:hover {
        background-color: rgb( 137, 165, 174);
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
    .inputbox {
        
      border-radius: 3px;
      border-color: rgb(200, 200, 200);
      box-shadow: 0 1px 2px rgba(0,0,0,0.2) inset, 0 -1px 0 rgba(0,0,0,0.05) inset;
      transition: all 0.2s linear;
      font-family: "Helvetica Neue", sans-serif;
      font-size: 13px;
      color: #222222;
      position: relative;
      height: 36px;
      width: 150px;
      padding-left: 30px;
      position:absolute; 
      left:10px; 
      top:16px;
      z-index:3;

    }
    .inputpad {
        position:absolute;
        background-color: RGB(93, 135, 161);
        height:62px;
        left:0px;
        width: 250px;
        z-index:1;
        
    }
    .inputpad:hover {
            background-color: rgb( 137, 165, 174);
    }
    .select2-container {
        zoom: 1.3;
    }
    .select2-drop {
        zoom: 1.3;
    }
    .ui-widget-header{
        border: 1px solid #aaaaaa;
        background: #cccccc url(images/ui-bg_highlight-soft_75_cccccc_1x100.png) 50% 50% repeat-x;
        color: #222222;
        font-weight: bold;
        height: 25px;

    }
    #loginfrm {
	        BORDER-RIGHT: 0px; BORDER-TOP: #95A0A9 1px solid; BACKGROUND: #87A7AA; LEFT: expression(document.body.clientWidth / 2 - this.offsetWidth / 2); BORDER-LEFT: 0px; WIDTH: 100%px; BORDER-BOTTOM: #95A0A9 1px solid; POSITION: absolute; TOP: 150px; HEIGHT: 326px;
             box-shadow: 5px 15px 15px rgba(0,0,0,.7);
             -webkit-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
             -moz-box-shadow: 5px 10px 10px rgba(0,0,0,.7);
             border-radius: 4px;
        }
    div.hidden {
        display: none;
        position: fixed;
        z-index:106;
    }
    .ui-jqgrid tr.jqgrow {
        outline-style: none;
        height: 70px;
    }
    .ui-pg-div {
        font-size: medium;
    }
    .label-tab{
        position: relative;
        top: 11px;
        float: right;
        font-weight: 800;
        font-size: small;
        z-index: 100;
        color: white;
        max-width: 40px;
        word-wrap: break-word;
        display: block;
    }
    .Weaver-addname-label { 
        width: 200px;
        margin-right: 0px;
        top: 10px;
        position: relative;
    }
    .dialog-select { 
        height: 30px;
        width: 130px;
        position: relative;
        float: right;
        top: -10px;
    }

</style>
<script src="//oss.maxcdn.com/jquery.form/3.50/jquery.form.min.js"></script>
<%--<script type="text/javascript" src="../../handlers/Utility/cypher.ashx"></script>--%>
<script type="text/javascript">
    
    //window.alert = function(e) {alert(e)};
    var TemplateCollection;
    var SpecCollection;
    var tabselect = 0;
    var SelectedId;
    var SelectedName;
    var SelectedTab;
    var UserSelectedTabNumber;
    var NavCop = new Boolean();
    var AutoConfirm = false;
    var buttonid; 
    var buttonvalue;
    var DefectID = 0;
    var $workorder = $('#MainContent_WorkOrder');
    var $rollnumber = $('#MainContent_RollNumber');
    var $itemnumber = $('#MainContent_ItemNumber');
    var $CartonNumber = $('#MainContent_CartonNumber');
    var $CPNumber = $('#MainContent_CPNumber');
    var $WorkRoom = $('#MainContent_workroom');
    var $purchaseorder = $('#MainContent_CartonNumber');
    var $auditorname = $('#MainContent_AuditorName');
    var $auditornameSel = $('#Auditor_Name');
    var $Location = $('#MainContent_Location');
    var $LotSize = $('#WOQuantity');
    var $SampleSize = $('#MainContent_SampleSize');
    var $goodcount = $('#MainContent_Good');
    var $RE = $('#MainContent_RE');
    var $badcount = $('#MainContent_Bad_Local');
    var $DataNo = $('#MainContent_DataNumber');
    var $aql = $('#MainContent__AQLevel');
    var $loginfrm = $('#loginfrm');
    var hiddenSection = $('div.hidden');
    var IsMobile;
    var IsSPCMachine; 
    var HasProductSpecs = [];
    var SpecGridEditId;
    var InspectionId = 0;
    var DefectsPerhundredYards = 0;
    var RollYards = 1000;
    var InspectionTypes = ["WorkOrder", "RollNumber", "Carton"];
    var InspectionState = 0;
    var InspectionTypeState = "WorkOrder";
    var TargetOrderInput; 
    var WOQuantityValue = 0;
    var CountToTarget;
    var HasTargetCount;
    var selectedCID; 
    var selectedCIDnum;
    var HasCID = 'False'; 
    var RemoteCounts = 0; 
    var LocalCounts = 0; 
    var AQLValue = 2.5;
    var LastRemoteCount = 0; 
    var InspectionJobSummaryIdPage = 0;
    var OpenOrderFlag = "False";
    var SelectSPCMachineName = "";
    var lastsel2;
    var InspectionStartedVal = false;
    var pageBehindInspectionStarted;
    var SpecItemCounter = 1;
    var LineType = '<%=LineType%>';
    var SessionID = '<%=SessionId%>';
    var MS_ProductValue = 0;
    var MS_Lower_Spec_Value = 0;
    var MS_Upper_Spec_Value = 0;
    var OpenWorkOrderArray = []; 
    var TemplateTabCount = 0;
    var IsPhoneSize = false;
    var IsTableSize = false;
    var InspectionConfirmFlag = '<%=InspectionConfirmFlag%>';
    var InspectionConfirmMessage = '<%=InspectionConfirmMessage%>';
    var buttonid;
    var buttonvalue;
    var buttonname;
    function isOdd(num) { return num % 2; }
    $(function () {
        var screenwidth = screen.width - 200;
        var json = new Array();   
        
        TargetOrderInput = $('#MainContent_WorkOrder');
        var $_aql = $('#MainContent__AQLevel');
        TemplateCollection = <%=TemplateCollection%>
        SpecCollection = <%=ProductSpecCollection%>
        SelectedId = <%=SelectedId%>
        SelectedName = "<%=SelectedName%>" 
        LocationNames = <%=LocationNames%>
        LastLocation = '<%=LastLocation%>'
        TemplateTabCount = <%=TemplateTabCount%>
        AQLValue = '<%=AQL%>';
        IsSPCMachine = new Boolean(<%=IsSPCMacine%>); 
        IsMobile = new Boolean(<%=IsMobile%>);
        HasTargetCount = "<%=HasTargetCount%>";
        WOQuantity = <%=WOQuantityValue%>
        selectedCID = '<%=CID%>';
        selectedCIDnum = '<%=CIDnum%>';
        HasCID = '<%=HasCID%>';
        OpenOrderFlag = '<%=OpenOrderLoadFlag%>';
        IsSPCMachine = <%=IsSPCMacine%>;
        SelectSPCMachineName = '<%=SPCMachineName%>';
        pageBehindInspectionStarted = '<%=InspectionStartedFlag%>';

        RenderEngine.SizeChecker();
        eventshandler.UserKeyPress.Init();
        $(".scrollWrap, .wijmo-wijsuperpanel, .ui-widget, .ui-widget-content, .ui-corner-all").css("height", "60px");
        dialogs.InitProductSpecEntry(); 
        dialogs.InitStartDialog(); 
        dialogs.InitDefectsEntry(); 
        dialogs.InitAuditorName(); 
        controls.InitTemplateDropDown(<%=TemplateNames%>);
        controls.InitLocationDropDown(<%=LocationNames%>, selectedCIDnum, selectedCID);
        controls.InitMachineNameDropdowns(<%=MachineNames%>);
        controls.InitWeaversDropDown(pageData.WeaversList); 
        controls.InitMenu();
        controls.InitAqlDropDown(AQLValue);
        controls.InitAqlStandardDropDown(); 
        controls.SetAuditFromCookie();
        controls.InitWOQuantity(); 
        controls.InitNumbers(); 
        eventshandler.InitPageEventHandlers();

        
        
        if (InspectionConfirmFlag == 'true') { 
            alert(InspectionConfirmMessage);
        }
        
        if (selectedCID) { datahandler.GetAuditorNames(); }
        
        if (IsSPCMachine == 'true') { 
            setInterval('window.location.reload()', 120000);
        }
            
        controlhandler.RenderProductSpecTable();
        controlhandler.InitInspectionDisplay(LineType); 
       
        var aqlstring = "AQL Level " + $_aql.val().toString()
        //CODE detached 4.23.17 JJS
        //if (SelectedId != 0) {
        //    $("#TemplateId").select2("data", { id: SelectedId.toString(), text: SelectedName });
        //}
        var screenheight = $( window ).height() + 200;
       //$("body").css({ "height": screenheight.toString() + 'px' });
       
        var $tabs = $('#tabs').wijtabs({
            tabTemplate: '<li><a href="#{href}">#{label}</a> <span class="ui-icon ui-icon-close">Remove Tab</span></li>',
            add: function (event, ui) {
                var tab_content = 'Tab test content.';
                controlhandler.addbuttontotab(tabdatahandler.tabbuttonarray.length, ui)
            },
            select: function (e, args) {
                var tablength = $('#tabs').wijtabs("length");
                SelectedTab = args.tab.innerText;
                $('#HiddenProduct').val(SelectedTab);
     
                UserSelectedTabNumber = args.index;
            },
            scrollable: true
        });
        
        $(".ui-tabs-panel").on('click', '.timerbutton', function (e) {
            var buttonid_ = $(this).attr('id');
            var buttonvalue_ = $(this).text();
            var buttonname_ = $(this).attr('name');
            var TimerActiveFlag;
            var AntiButton; 
            var ThisButton;
            
            if (InspectionStartedVal == false) { 
                alert('Inspection Not Started.')
                return false;
            }
            if ($(this).css('background-color') == 'rgba(111, 106, 107, 0.682353)') { 
                alert("not active");
                return false;
            } 

            var stopbuttonidAr = buttonid_.split("_");
            if (stopbuttonidAr.length != 3) {
                alert("timer button id parsing error");
                return false;
            }
            
            if (buttonvalue_ == "START") {
                TimerActiveFlag = true;
                AntiButton = "stop"
                ThisButton = "start"
                var Defectbutton = $("#button" + stopbuttonidAr[2]);
                var  Antibuttonid = AntiButton + "_" + stopbuttonidAr[1] + "_" + stopbuttonidAr[2];
                $.when(datahandler.GetSelectElements()).done(function (elementarrayval) { 
                    var JsonString = JSON.stringify(elementarrayval);
                    
                    datahandler.SubmitDefect_2(Defectbutton.attr('id'), Defectbutton.text(), Defectbutton.attr('name'), InspectionJobSummaryIdPage, InspectionId, JsonString, true, buttonname_,buttonvalue_ , stopbuttonidAr[2], $(this), $("#" + Antibuttonid));
                });
            } else { 
                TimerActiveFlag = false;
                AntiButton = "start"
                ThisButton = "stop"
                datahandler.ToggleTimerStatus(buttonname_,buttonvalue_ , 0, stopbuttonidAr[2]);
                

            }
            controlhandler.toggleButtonColor($(this), $("#" + AntiButton + "_button_" + + stopbuttonidAr[2]))
            
            return false;
        });
        $(".ui-tabs-panel").on('click','.buttontemplate', function (e) {
            var buttonid_ = $(this).attr('id');
            //var buttonvalue_ = $(this).attr('value');
            var buttonvalue_ = $(this).text();
            var buttonname_ = $(this).attr('name');
 
            if ($Location.val() != "" || $LotSize.val() != "" || $AuditorName.Val() != "" || $DataNo.Val() != "") {
                if (buttonvalue_) {
                    $('#DefectFlag').text('[' + buttonvalue_ + ']');
                    
                    var GoodCount = new Number($goodcount.val());
                    var BadCount = new Number($badcount.val());
                    var total = GoodCount + BadCount;
                    var splitarr = buttonvalue_.split(".")

                    if(splitarr.length > 0) { 
                        var strsum = '';
                        for (i = 0; i < splitarr.length; i++) { 
                            if (i > 0) { 
                                strsum = strsum + splitarr[i];
                            }
                        }
                        buttonvalue = strsum;
                    } else { 
                        buttonvalue = buttonvalue_.trim()
                    }
                    if (InspectionStartedVal == false) { 
                        alert("Please Start an Inspection by Filling in Required info and Clicking Start");
                    } else {
                        if (AutoConfirm == false) { 
                            buttonid = buttonid_;
                            buttonname = buttonname_;
                            //datahandler.SubmitDefect(buttonid_, buttonvalue_);
                        
                            $("#dialog").wijdialog("open");
                      

                        } else {
                            datahandler.SubmitDefect(buttonid_, buttonvalue, buttonname_, InspectionJobSummaryIdPage, InspectionId);
                        }
                    }

                }
            } else {
                alert("****Please fill out either Location, Lot Size, DataNo OR Auditor Name  information before starting inspections");
            }
        })
        .css({ height:"100%", overflow: "auto" });

        $("#StartInspection").click(function (e) { 
            if (InspectionStartedVal == true) { 
                alert('An Inspection Is already In Progress.  Please Clear current Inspection or Complete the Current one');
                return;
            }
            
            switch (LineType) { 
                case 'ROLL': 
                    var Inspector = $("#MainContent_Inspector").val();
                    if (Inspector.toString().trim().length == 0) { 
                        alert("Inspector Name cannot be blank");
                        return; 
                    }
                    if ($rollnumber.val().length > 1 && $DataNo.val().length > 1 && $Location.val().length > 1 && InspectionStartedVal == false && InspectionState == 1) { 
                        
                        Template.SetCookie("InspectorName", Inspector, 14);
                        var InspectionType = InspectionTypeState
                        Template.SetCookie("InspectionType", InspectionState, 14);
                        $("#StartInspectionWODiv").css('display', 'none');
                        $("#StartInspectiondialog").wijdialog("open");
                    } else  if (InspectionState == 1) { 
                        alert("Roll Number, RM#, Location and Yards Need to be greater than 1 digit.");
                        return;
                    }
                    break;
                default: 
                    var Quantity = new Number($LotSize.val()); 
                    if ( $auditornameSel.val() != "SELECT OPTION" && $auditornameSel.val() != "New Name") { 
                        if ($workorder.val().length > 1 && $DataNo.val().length > 1 && Quantity > 0 && $Location.val().length > 1 && InspectionStartedVal == false && InspectionState == 0) { 
                            var AuditorName = $("#MainContent_AuditorName").val();
                            Template.SetCookie("AuditorName", AuditorName, 14);
                            var InspectionType = InspectionTypeState
                            Template.SetCookie("InspectionType", InspectionState, 14);
                            $("#StartInspectionRollDiv").css('display', 'none');
                            $("#StartInspectiondialog").wijdialog("open");
                        }else if (InspectionState == 0) { 
                            alert("Work Order, DataNo, Location and Quantity Need to be greater than 1 digit.");
                            return;
                        }

                
                    } else { 
                        alert("You are required to put in an Auditor Name");
                    }
            }
                   
        });
       
        
        $("#MachineLinkCheck").click(function (e) { 
            var WOArray = OpenWorkOrderArray; 
            var $Checkele = $("#MachineLinkCheck");
            var $Workpop = $("#WorkOrder_pop");
            var MachineCheckVal = $Checkele.prop('checked');
            var html3 = [];
            var name;

            $Workpop.empty();
            for (var i = 0; i < OpenWorkOrderArray.length; i++) { 
                name = OpenWorkOrderArray[i];
              
                var TemplateName = name.Name;
                if (!TemplateName) { 
                    TemplateName = "NoName"; 
                }
                if (name.JobNumber == "SELECT OPTION") { 
                    $Workpop.append('<option value="' + name.id + '">' + name.JobNumber + '</option>');
                } else { 
                    if (name.IsSPC == true && MachineCheckVal == true) { 
                        $Workpop.append('<option value="' + name.id + '">SPCMachine ' + name.ProdMachineName + ' ID: ' + name.id + ' WO: ' + name.JobNumber + ' -> ' + TemplateName + ' Started: ' + name.Inspection_StartedString +'</option>');
                    } 
                    if (name.IsSPC == false && MachineCheckVal == false){
                        $Workpop.append('<option value="' + name.id + '">ID: ' + name.id + ' WO: ' + name.JobNumber + ' -> ' + TemplateName + ' Started: ' + name.Inspection_StartedString +'</option>');
                    }
                }
            }
            $Workpop.val(1000);


        });
        $("#LoadOpenWorkOrder").click(function (e) { 

            if (IsSPCMachine == true) { 
                alert("This Page is currently Linked to an Machine. Click NEW or Exit the page to escape");
                return;
            }

            var InspectionCanStart = false; 
            var ErrorMessage = ""; 
            var AuditorName = $('#Auditor_Name').val();

            switch(LineType) { 
                case 'ROLL': 
                    if ($("#MainContent_Inspector").val().toString().trim().length > 0) { 
                        InspectionCanStart = true;
                    }
                    ErrorMessage = "Inspection Must include an Inspector.";
                    break; 
                default: 
                    if (AuditorName.toString().trim() != 'New Name' && AuditorName.toString().trim().length > 0 && $auditornameSel.val() != "SELECT OPTION") { 
                        InspectionCanStart = true;
                    }
                    ErrorMessage = "Inspection Must include an Auditor.";
            }

            
            if (InspectionCanStart == true) { 
                var OpenWOArray = new Array(); 
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'POST',
                    data: { method: 'LoadOpenWorkOrders', args: { InspectionState: InspectionTypeState, PassCID: selectedCIDnum } },
                    success: function (data) {
                        var html3 = [];
                        var name;
                        var parsedata = JSON.parse(data);
                        OpenWorkOrderArray = parsedata; 
                             
                        if (parsedata) { 
                            for(var i = 0; i < parsedata.length; i++){
                                name = parsedata[i];
                                var TemplateName = name.Name;
                                if (!TemplateName) { 
                                    TemplateName = "NoName"; 
                                }
                                // var date = new Date(JSON.parse(name.Inspection_Started));
                         
                                var MachineCheckVal = $("#MachineLinkCheck").prop('checked');
                           
                                // var str = (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear() + " " +  date.getHours() + ":" + date.getMinutes();

                                if (name.JobNumber == "SELECT OPTION") { 
                                    html3.push('<option value="' + name.id + '">' + name.JobNumber + '</option>');
                                } else { 
                                    if (name.IsSPC == true && MachineCheckVal == true) { 
                                        html3.push('<option value="' + name.id + '">SPCMachine ' + name.ProdMachineName + ' ID: ' + name.id + ' WO: ' + name.JobNumber + ' -> ' + TemplateName + ' Started: ' + name.Inspection_StartedString +'</option>');
                                    } 
                                    if (name.IsSPC == false && MachineCheckVal == false){
                                        html3.push('<option value="' + name.id + '">ID: ' + name.id + ' WO: ' + name.JobNumber + ' -> ' + TemplateName + ' Started: ' + name.Inspection_StartedString +'</option>');
                                    }
                                }
                            }
                        }
                        $("#WorkOrder_pop").html(html3.join('')).bind("change", function(){

                            var selectedid = $(this).val();
                            var selectedtext = $(this).text();
                            var selectedIndex = 0;
                            var SelectedTemplateId = 0; 
 
                            if (selectedid) { 
                                var selectId = new Number($(this).val());
                                for (var j = 0; j < OpenWorkOrderArray.length; j++) { 
  
                                    if (selectId == OpenWorkOrderArray[j].id) {  
                                        SelectedTemplateId = OpenWorkOrderArray[j].TemplateId;

                                    }
                                }
                                if (SelectedTemplateId == 0) {
                                    SelectedTemplateId = SelectedId.toString();
                                }
                                
                                switch (InspectionTypeState) { 
                                    case "WorkOrder": 
                                        
                                        var SelectedWorkOrder = "";
                                        for(var i = 0; i < parsedata.length; i++) { 
                                            if (parsedata[i].id == selectId) { 
                                                selectedWorkOrder = parsedata[i].JobNumber
                                            }
                                        }
                                        
                                        var querystring = "TemplateId=" +  SelectedTemplateId.toString() + "&IJS=" + selectedid.toString() + "&Username=" + $auditorname.val().toString() + "&400REQ=OPENWO&LocationId=" + $("#MainContent_Location").val() + "&OpenWO=True&CID_Info=000" + selectedCIDnum
                                        window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?" + querystring)
                                            
                                        break;
                                    case "RollNumber":
                                        var querystring = "TemplateId=" +  SelectedTemplateId.toString() + "&IJS=" + selectedid.toString() + "&Username=" + $auditorname.val().toString() + "&400REQ=OPENRL&LocationId=" + $("#MainContent_Location").val() + "&OpenWO=True&CID_Info=000" + selectedCIDnum
                                        window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?" + querystring)
                                        break;
                                }
                            }
                        });
                        $("#WorkOrder_pop").val(1000);    
                        $("#LocationSelection").css("display","none");
                        $("#JobConfirmation").css("display","none");
                        $("#MachineSelection").css("display", "none"); 
                        $("#SpecTable").css("display", "none"); 
                        $("#RollConfirmation").css("display", "none");
                        $("#loginfrm").fadeIn();
                        $("#WorkOrderSelection").fadeIn();
                        hiddenSection.fadeIn()
                            .css({ 'display':'block' })
                            // set to full screen
                            .css({ width: $(window).width() + 'px', height: '100%' })
                            .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
                                left:($(window).width() - hiddenSection.width())/2 + 'px' })
                            // greyed out background
                            .css({ 'background-color': 'rgba(0,0,0,0.5)' });
                    },
                    error: function (a, b, c) {
                        alert(c);
                        
                    }
                });
            } else { 
                alert(ErrorMessage);
            }
            
        }); 

        
        //$('.ui-spinner-button').css('font-size','1px');
        $(".endjob").click(function (e) {
            var buttonid_ = $(this).attr('id');
            if (IsSPCMachine == true) { 
                alert("This Page is currently Linked to an Machine. End the Job at the Machine PC. Click NEW or Exit the page to escape");
            } else {
                $("#MainContent_JobConfirmation_hidden").val(buttonid_);
                var samesizenum = new Number($('#MainContent_SampleSizeHidden').val());
           
                if (InspectionTypeState == 'WorkOrder' && samesizenum == 0) { 
                    alert('Sample Size Not Set'); 
                    return;
                }
                $.when(datahandler.SetSampleSize()).done(function() { 
                    if (($workorder.val().length > 2  && InspectionJobSummaryIdPage > 0 && InspectionStartedVal == true) || ($rollnumber.val().length > 2 && InspectionJobSummaryIdPage > 0 && InspectionStartedVal == true)) { 
                        if ($LotSize.val() != "0" && SelectedId != 0) {

                            $.when(datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage)).done(function () { 
                                var SampleCount = new Number($SampleSize.val());
                                var RejCount = new Number($("#MainContent_Bad_Group").val());
                                var jobnumber;
                                var LimiterNo = new Number($("#MainContent_REHidden").val());
                                switch (InspectionState) { 
                                    case 0: 
                                        jobnumber = $workorder.val(); 
                                        $("#MainContent_Good").val(SampleCount - RejCount);
                                        $("#FailCountValue").text(RejCount.toString())
                                        $("#PassCountValue").text($("#MainContent_Good").val());
                                        //$("#TotalCountValue").val($SampleSize.val());
                                        $("#TotalCountValue").wijinputnumber("option", "value",  $('#MainContent_SampleSizeHidden').val()); 
                                        $("#TotalCountValue").wijinputnumber('option',  "minValue", RejCount);
                                        $('#MainContent_totalinspecteditems').val($('#MainContent_SampleSizeHidden').val());
                                        $("#JobNumberValue").text(jobnumber);
                                        $("#LocationSelection").css("display","none");
                                        $("#WorkOrderSelection").css("display","none");
                                        $("#MachineSelection").css("display", "none");
                                        $("#RollConfirmation").css("display", "none");
                                        $("#JobConfirmation").css("display", "block");
                            
                                        if (RejCount >= LimiterNo) { 
                                            $("#MainContent_AddEmailFlag").prop("checked", true);
                                        }
                                        break; 
                                    case 1: 
                                        jobnumber = $rollnumber.val(); 
                                        $("#RollNumberValue").text(jobnumber);
                                        $("#FailRollValue").text(RejCount.toString())
                                        $("#TotalYardValue").wijinputnumber("option", "value",  $('#WOQuantity').val());
                                        $('#MainContent_totalinspectedyards').val($('#WOQuantity').val());
                                        $("#LocationSelection").css("display","none");
                                        $("#WorkOrderSelection").css("display","none");
                                        $("#MachineSelection").css("display", "none");
                                        $("#RollConfirmation").css("display", "block");
                                        $("#JobConfirmation").css("display", "none");
                                        var DHY = new Number($('#MainContent_SampleSize').val());
                                        if (DHY > 10) { 
                                            $("#MainContent_AddEmailFlagRoll").prop("checked", true);
                                        }
                                        break; 
                                        //case 2: 
                                        //    jobnumber = $itemnumber.val(); 
                                        break; 
                                }

                            
                                $loginfrm.css("height", "320px");
                                $loginfrm.fadeIn();
                                hiddenSection.fadeIn()
                                    .css({ 'display':'block' })
                                    // set to full screen
                                    .css({ width: $(window).width() + 'px', height: '100%' })
                                    .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
                                        left:($(window).width() - hiddenSection.width())/2 + 'px' })
                                    // greyed out background
                                    .css({ 'background-color': 'rgba(0,0,0,0.5)' });
                            });
                        }
                
                    }
                });
            }
        });
        var InspectionStartedAlertFlag = false; 
        $(".inputelement.inputbox.leftsideobj").on( "keydown", function (e) { 
            
            if (InspectionJobSummaryIdPage != 0 && LineType != 'ROLL') { 
                alert("There is an inspection in progress! Are you trying to open another Inspection? If yes please use the OPEN WORK ORDER or NEW button to navigate away from the current active inspection. Click OK to continue the current active inspection.");
                e.preventDefault();
                return false;
            }
            
        });
        $(".inputpad").click(function (e) {
            var selectedtarget = e.currentTarget.id;
  
            var classname = e.currentTarget.className
            var $auditorname = $('#MainContent_AuditorName');
            //var $lotsize = $('#WOQuantity'); 
          
            e.preventDefault();
            if (IsSPCMachine == true) { 
                alert("This Page is currently Linked to an Machine.  Click NEW or Exit the page to escape");
            } else if (InspectionJobSummaryIdPage != 0 && selectedtarget != 'auditdiv' && LineType != 'ROLL') { 
           
                //alert("There is an inspection in progress! Are you trying to open another Inspection? If yes please use the OPEN WORK ORDER or NEW button to navigate away from the current active inspection. Click OK to continue the current active inspection.");
            } else {
                NavCop = true;
                switch (selectedtarget) { 
                    case "workroomdiv":
                        var querystring = "TemplateId=" + SelectedId.toString() + "&workroom=" + $("#MainContent_workroom").val().toString() + "&400REQ=WORKROOM&AQL=" + $_aql.val() + "&CID_Info=000" + selectedCIDnum
                        if ($("#MainContent_workroom").val().length > 4) {
                            window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?" + querystring)
                        }
                    
                        break;
                    case "cpdiv": 
                        if ($purchaseorder.val().length > 4 & classname!= "inputelement inputbox") { 

                        } else if (classname != "inputelement inputbox") { 
                            togglevalidation("workdiv")
                        }
                        break;
                    case "workdiv": 
               
                        if ($workorder.val().length > 2) {
                            var querystring = "TemplateId=" + SelectedId.toString() + "&WorkOrder=" + $workorder.val().toString() + "&Username=" + $auditorname.val().toString() + "&AQL=" + $aql.val() + "&400REQ=WORKORDER&LocationId=" + $("#MainContent_Location").val() + "&standard=" + $("#MainContent_aqlstandard").val() + "&CID_Info=000" + selectedCIDnum; 
                            window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?" + querystring)
                        } else if (classname == "inputpad") { 
                            togglevalidation("workdiv")
                        };
                        break;
                    case "rolldiv": 
                        if ($rollnumber.val().length > 2) { 
                            var querystring = "TemplateId=" + SelectedId.toString() + "&RollNumber=" + $rollnumber.val().toString() + "&Username=" + $auditorname.val().toString() + "&400REQ=ROLLNUMBER&LocationId=" + $("#MainContent_Location").val() + "&CID_Info=000" + selectedCIDnum;
                            window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?" + querystring)
                        } else { 
                            togglevalidation("rolldiv")
                        }
                        break;
                    case "auditdiv": 
                        if (LineType == "ROLL") { 
                            $(".Weaver-addname").css('display', 'block');
                            $(".Auditor-addname").css('display', 'none'); 
                            $('#NewAuditorName').wijdialog('open');
                        }
                        break;
                }
            }
        });
        
        $(".export").click(function () {
            var buttonid = $(this).attr('id');

            switch (buttonid) {
                case "MainContent_Pass":
                    NavCop = true
                    break;
                case "MainContent_NewJob":
                    NavCop = true
                    break;
                case "MainContent_NewSample":
                    NavCop = true
                    break;
            }
        });
        
        //$("#menudiv").toggle();
        $("#InputHolder").toggle();

        var now = new Date();

        var night = new Date(
            now.getFullYear(),
            now.getMonth(),
            now.getDate() + 1, // the next day, ...
            0, 0, 0 // ...at 00:00:00 hours
        );

        var msTillMidnight = night.getTime() - now.getTime();

        setTimeout(function () { location.reload(); }, msTillMidnight);

        if (HasCID == 'False') { 
            var html = [];
            var name;
            html.push('<option value="999">select option</option>');
            if (LocationNames[0] != 0) { 
                for(var i = 0; i < LocationNames.length; i++){
                    name = LocationNames[i];
                    html.push('<option value="'+name.CID+'">'+name.text+'</option>');
                }
            }

            $("#LocationNames_pop").html(html.join('')).bind("change dblclick", function(){

                var selectedid = $(this).val();
                if (selectedid && selectedid != '999') { 
                    window.location.assign("<%=Session("BaseUri")%>" + '/APP/Mob/SPCInspectionInput.aspx?CID_Info=' + selectedid  + "&AQL=" + $_aql.val())
                }
            });
            $("#LocationNames_pop").val(selectedCID);
            $("#MachineSelection").css("display","none");
            $("#WorkOrderSelection").css("display","none");
            $("#JobConfirmation").css("display","none");
            $("#RollConfirmation").css("display","none");
            $("#loginfrm").fadeIn();
            hiddenSection.fadeIn()
                .css({ 'display':'block' })
                // set to full screen
                .css({ width: $(window).width() + 'px', height: '100%' })
                .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
                    left:($(window).width() - hiddenSection.width())/2 + 'px' })
                // greyed out background
                .css({ 'background-color': 'rgba(0,0,0,0.5)' });
        }
         
        if (OpenOrderFlag == "True") { 
            $.when(datahandler.GetInspectionId()).done(function () { 
                InspectionJobSummaryIdPage = <%=Selected_Inspectionid%>;
                $("#MainContent_inspectionjobsummaryid_hidden").val(InspectionJobSummaryIdPage);
                datahandler.GetOpenTimers();
                //datahandler.GetInspectionJobSummaryId(TargetOrderInput.val(), buttonid, buttonvalue, buttonname, false); 
                $("#<%=InspectionId.ClientID%>").val(InspectionJobSummaryIdPage);
                pageBehindInspectionStarted = "true";
                InspectionStartedVal = true;
 
                $("#AQL_Level_Dialog").prop('disabled', true);
                $("#AQ_Level").prop('disabled', true);
                $("#AQL_Standard_Dialog option").prop('disabled', true);
                $("#selectNames").prop("disabled", true);

                if (LineType == "ROLL") { 
                    var weaverInfoObj = <%=OpenRollInfoString%>;
          
                    if (weaverInfoObj != null) { 
                        Inspection.WeaverShiftId = weaverInfoObj[0].ShiftId
                        if (weaverInfoObj.Length == 1 && weaverInfoObj[0].EmployeeNoId != null) { 
                            Inspection.Weavers.Weaver1ID = weaverInfoObj[0].EmployeeNoId; 
                            Inspection.Weavers.Weaver1Initials = weaverInfoObj[0].Initials; 
                        }
                        if (weaverInfoObj.Length == 2 && weaverInfoObj[1].EmployeeNoId != null && weaverInfoObj[0].ShiftId == weaverInfoObj[1].ShiftId) { 
                            Inspection.Weavers.Weaver2ID = weaverInfoObj[1].EmployeeNoId; 
                            Inspection.Weavers.Weaver2Initials = weaverInfoObj[1].Initials; 
                        }
                    }
                    
                } else { 
                    $("#Auditor_Name").prop('disabled', true); 
                }

                if (IsPhoneSize == true) { 
                    RenderEngine.ShowActiveInspectionMobile();
                }
                setInterval(
                        function () { 
                            datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage) }
                        , 10000);
                if (IsSPCMachine == true) { 
                    datahandler.GetInspectionJobSummaryId(TargetOrderInput.val(), false);
 
                    $("#MachineDiv").toggle(); 
                    $("#MachineLbl").text(SelectSPCMachineName);
                    setInterval(function () { 
                        datahandler.GetSPCWorkOrder(SelectSPCMachineName); 
                    }, 10000)
                }
            });
        } else { 
            $("#MainContent_InspectionId").val("0");
        }
        
        
    });
    var tabdatahandler = {
        buttonarray: Array(),
        tabbuttonarray: Array(),
        newtabflag: Boolean(false),
        buttoncount: 1
    }
    var supportsOrientationChange = "onorientationchange" in window,
    orientationEvent = supportsOrientationChange ? "orientationchange" : "resize";

    window.addEventListener(orientationEvent, function() {
   
        var TabNum = $("#tabs").wijtabs("length");

        for (i=0;i<=TabNum;i++) { 
            controlhandler.$tabs.wijtabs("remove", 0 );
        } 
        RenderEngine.SizeChecker();
    }, false);

    var RenderEngine = { 
        ShowActiveInspectionMobile: function () { 
            $("#LoadWorkOrderDiv").css("display", "none");
            $("#InputHolder").css("display","none");
            $("#tabs").css({left:"0px", display: 'block', width: (screen.width - 20).toString() + 'px', height: ($('body').height() - 20).toString() + 'px', top: '185px'}); 
            if (TemplateCollection.length > 0) { Template.Load();}
            $("#mendiv").css({display:'none'});
            $("#ActionHolder").css({top:'120px', display: 'block'});
            $("#NewPageDiv").css({display:'block', left: '255px'});
            $('#NewPage').css('width', (screen.width * .28).toString() + 'px');
            $('#completeInspect').css('width', (screen.width * .55).toString() + 'px');
            $("#workorderDiv").css({display:'none'});

        }, 
        ShowUnActiveMobile: function () { 
            $("#scorelabels").css("display", "none");
            $("#tabs").css("display", "none");
            $("body").css("width", screen.width.toString() + "px");
            $("article").css("width", screen.width.toString() + "px");
            $("#InputHolder").css("width", screen.width.toString() + "px");
            $("#menudiv").css("display", "none");
            $(".leftsidepanel").css("width", "100%");
            $(".leftsideobj").css("width", "75%");
            $(".leftsidedropobj").css("width", "85%");
            $("#ActionHolder").css({display: 'none'});
            $("#NewPageDiv").css({display:'none'});
        },
        SizeChecker: function () { 
            
            if (screen.width < 740) {
                IsPhoneSize = true;
                var $body = $(document);
                $body.bind('scroll', function() {
                    // "Disable" the horizontal scroll.
                    if ($body.scrollLeft() !== 0) {
                        $body.scrollLeft(0);
                    }
                });
                $("#loginfrm").css({left: '0%', width: '99%'});
                $("#GlobalSpecsImage").css("display", "none");
                $("#Image1").css("display", "none"); 
                $("#loginView").empty();
                $("#completeInspect").css("left", "-350px");
                $("#ActionHolder").css({top:'100px', display: 'block', width: screen.width.toString() + "px"});
                $('#MobileDirector').css('width', '100px');
                //$('#NewPage').css('width', (screen.width * .28).toString() + 'px');
                $('#completeInspect').css('width', (screen.width * .55).toString() + 'px');

                if (pageBehindInspectionStarted == 'false') { 
                    RenderEngine.ShowUnActiveMobile();
                } else { 
                    Template.Load();
                }
            } else if (screen.width < 1110) { 
                $("#menudiv").css("display", "block");
                $("#GlobalSpecsImage").css("display", "none");
                $("#Image1").css("display", "none"); 
                $("#loginView").empty();
                $("#CompleteDiv").css("left", "40px");
                $("#NewPageDiv").css("left", "230px");
                $("#scorelabels").css({display: 'block', left: "340px"});
                $("body").css("min-width", "900px");
               if (TemplateCollection.length > 0) { Template.Load();}
            } else { 
                $("#scorelabels").css({display: 'block', left: "69%"});
                $("#menudiv").css("display", "block");
                $("body").css("min-width", "900px");
                $("#tabs").css("width", "68%");
                if (TemplateCollection.length > 0) { Template.Load();}
            }
        }
    };
    var Inspection = { 
        JobSummaryId: 0, 
        Weavers: { Weaver1ID: 0, Weaver1Initials: "", Weaver2ID: 0, Weaver2Initials: "" },
        PreConfirmWeavers: { Weaver1ID: 0, Weaver1Initials: "", Weaver2ID: 0, Weaver2Initials: "" },
        SetWeaversHTML: function () { 
                
            if (Inspection.PreConfirmWeavers.Weaver1ID != 0) { 
                $("#MainContent_Weaver_Names").val(Inspection.PreConfirmWeavers.Weaver1Initials); 
            } 
            if (Inspection.PreConfirmWeavers.Weaver2ID != 0) { 
                $("#MainContent_Weaver_Names").val($("#MainContent_Weaver_Names").val() + ", " + Inspection.PreConfirmWeavers.Weaver2Initials); 
            }
            $("#PreConfirmWeaver1Label").text(''); 
            $("#PreConfirmWeaver2Label").text('');
            Inspection.Weavers = Inspection.PreConfirmWeavers;
            Inspection.PreConfirmWeavers = { Weaver1ID: 0, Weaver1Initials: "", Weaver2ID: 0, Weaver2Initials: "" }; 
        },
        WeaverShiftNumber: 1, 
        WeaverShiftId: 0
    };
    var pageData = { 
        WeaversList: new Object()
    };
    var dialogs = { 
        InitProductSpecEntry: function() { 
            $("#ProductSpecEntrydialog").wijdialog({
                buttons: { 
                    Confirm: function () { 

                        var $prodType = $("#MS_ProductType"); 
                        var $specDesc = $("#MS_ProductDesc"); 
                        var $ErrorLbl = $("#MS_ErrorLbl");
                        var Inputval = 0; 
                        var InputUp = 0; 
                        var InputLw = 0; 
                        var ErrorFlag = false; 
                        var i = 0; 
                        var Inputarray = new Array({value: Inputval, Upper_Spec_Value: InputUp, Lower_Spec_Value:InputLw, ProductType: $prodType.val(), Spec_Description: $specDesc.val(), DataNo: $DataNo.val()});
                        if ($prodType.val().trim().length < 2) { 
                            ErrorFlag = true; 
                            $prodType.css({"border-color":"red", "border-width": "medium"});
                            $ErrorLbl.html("A Product Type is Required");
                        }
                        if ($specDesc.val().trim().length < 2) { 
                            ErrorFlag = true; 
                            $specDesc.css({"border-color":"red", "border-width": "medium"});
                            $ErrorLbl.html("A Spec Description is Required");
                        }

                        $(this).find(".wijmo-wijinput").each(function() { 
                            var input = $(this).find("input");
                        
                            var reformed;
                            switch(input[0].id){
                                case "MS_ProductValue": 
                                    if (input[0].value <= 0) { 
                                        ErrorFlag = true; 
                                        $(this).css({"border-color": "red", "border-width":"medium"});
                                        $ErrorLbl.html("Value Must be Greater Than Zero");
                                    } else { 
                                        $(this).css({"border-color": "black", "border-width":"thin"});
                                        Inputarray[0].value = input[0].value;
                                    }
                                    break;
                                case "MS_Upper_Spec_Value": 
                                    if (input[0].value <= 0) { 
                                        ErrorFlag = true; 
                                        Inputarray[0].Upper_Spec_Value = input[0].value;
                                        $(this).css({"border-color": "red", "border-width":"medium"});
                                        $ErrorLbl.html("Value Must be Greater Than Zero");
                                    } else { 
                                        $(this).css({"border-color": "black", "border-width":"thin"});
                                        Inputarray[0].Upper_Spec_Value = input[0].value;
                                    }
                                    break;
                                case "MS_Lower_Spec_Value": 
                                    if (input[0].value >= 0) { 
                                        ErrorFlag = true; 
                                        Inputarray[0].Lower_Spec_Value = input[0].value;
                                        $(this).css({"border-color": "red", "border-width":"medium"});
                                        $ErrorLbl.html("Value Must be Less Than Zero");
                                    } else { 
                                        $(this).css({"border-color": "black", "border-width":"thin"});
                                        Inputarray[0].Lower_Spec_Value = input[0].value;
                                    }
                                    break;
                            }
                           
                            i++;
                            if (i==3 && ErrorFlag == false) { 
                           
                                datahandler.InsertProductSpec(Inputarray); 
                            }
                        });
                    

                        if (ErrorFlag == false) { 
                            $(this).wijdialog("close");
                        }
                    },
                    Cancel: function () { 
                        $(this).wijdialog("close"); 
                    }
                },
                captionButtons: { 
                    pin: { visible: false }, 
                    refresh: { visible: false }, 
                    toggle: { visible: false }, 
                    minimize: { visible: false }, 
                    maximize: { visible: false } 
                },
                height: 594,
                width: 450,
                autoOpen: false, 
                open: function (e) { 
                    if ($DataNo.val().length > 2 ) { 
                        datahandler.GetItemInfo(); 
                    } else { 
                        alert("No Data Number Entered"); 
                        $(this).wijdialog("close"); 
                    }
                }, 
                close: function (e) {
                    var $prodType = $("#MS_ProductType"); 
                    var $specDesc = $("#MS_ProductDesc"); 
                    var $ErrorLbl = $("#MS_ErrorLbl");

                    $(this).find(".wijmo-wijinput").each(function() { 
                        var input = $(this).find("input");
                    
                        
                        switch(input[0].id){
                            case "MS_ProductValue": 
                                $("#MS_ProductValue").wijinputnumber("setValue", 0.00, true)
                                break;
                            case "MS_Upper_Spec_Value": 
                                $("#MS_Upper_Spec_Value").wijinputnumber("setValue", 0.00, true)
                                break;
                            case "MS_Lower_Spec_Value": 
                                $("#MS_Lower_Spec_Value").wijinputnumber("setValue", 0.00, true)
                                break;
                        }
                        $ErrorLbl.html("");
                    });

                    $prodType.css({"border-color": "black", "border-width":"thin"});
                    $prodType.val("");
                    $specDesc.css({"border-color": "black", "border-width":"thin"});
                    $specDesc.val("");
                }
            });
        }, 
        InitStartDialog: function () { 
            $("#StartInspectiondialog").wijdialog({
                buttons: { 
                    Confirm: function () { 
                        var GoodCount = new Number($goodcount.val());
                        var BadCount = new Number($badcount.val());
                        var total = GoodCount + BadCount;
                        var AQLDlevel = $('#AQL_Level_Dialog').val();
                        var AQLDstandard = $('#AQL_Standard_Dialog').val();
                        var AuditorName = $('#Auditor_Name').val();
                        AQLValue = AQLDlevel;
                        var ErrorMessage = "";
                        // Template.SetCookie("AQLLevel", AQLDlevel, 14);
                        Template.SetCookie("AQLStandard", AQLDstandard, 14); 

                        var InspectionCanStart = false; 

                        switch(LineType) { 
                            case 'ROLL': 
                                if ((Inspection.Weavers.Weaver1ID > 0 || Inspection.Weavers.Weaver2ID > 0) && $("#MainContent_Inspector").val().toString().trim().length > 0) { 
                                    InspectionCanStart = true;
                                }
                                ErrorMessage = "Inspection Must include an Inspector and at least 1 weaver";
                                break; 
                            default: 
                                if (AuditorName.toString().trim() != 'New Name' && AuditorName.toString().trim().length > 0) { 
                                    InspectionCanStart = true;
                                }
                                ErrorMessage = "Inspection Must include an Auditor.";
                        }


                        if (InspectionCanStart == true) { 
                            $("#selectNames").prop("disabled", true);
                            if (total == 0 && OpenOrderFlag == "False") { 
                                $.when(datahandler.GetInspectionId()).done(function () { 
                                    if (InspectionJobSummaryIdPage == 0) { 
                                        datahandler.GetInspectionJobSummaryId(TargetOrderInput.val(), false); 
                                    }
                                    $("#ItemNumberLabel").text("Item #: 1");
                                });
                                if (InspectionState == 0) { 
                                    datahandler.SetSampleSize();
                                }
                            } 

                        } else { 
                            alert(ErrorMessage);
                        }
                        $(this).wijdialog("close"); 
                    },
                    Cancel: function () { 
                        $(this).wijdialog("close"); 
                    }
                },
                captionButtons: { 
                    pin: { visible: false }, 
                    refresh: { visible: false }, 
                    toggle: { visible: false }, 
                    minimize: { visible: false }, 
                    maximize: { visible: false } 
                },
                height: 294,
                width: 450,
                autoOpen: false, 
                open: function (e) { 
                    $("#Skip").prop('checked', AutoConfirm);
                }, 
                create: function (e) {
                    $('#Skip').change(function(e) {
          
                        var value = e.currentTarget.checked;
                        AutoConfirm = value;
                    });
                }
            });
        }, 
        InitDefectsEntry: function () { 
            $("#dialog").wijdialog({
                buttons: { 
                    Confirm: function () { 
                        var GoodCount = new Number($goodcount.val());
                        var BadCount = new Number($badcount.val());
                        var total = GoodCount + BadCount;
                        if (InspectionJobSummaryIdPage > 0 ) { 
                            datahandler.SubmitDefect(buttonid, buttonvalue, buttonname, InspectionJobSummaryIdPage, InspectionId);
                        } else { 
                            alert("InspectionId must be greater than zero")
                        }
                        $(this).wijdialog("close"); 
                    },
                    Cancel: function () { 
                        $(this).wijdialog("close"); 
                    }
                },
                captionButtons: { 
                    pin: { visible: false }, 
                    refresh: { visible: false }, 
                    toggle: { visible: false }, 
                    minimize: { visible: false }, 
                    maximize: { visible: false } 
                },
                height: 244,
                autoOpen: false, 
                open: function (e) { 
                    $("#Skip").prop('checked', AutoConfirm);
                }, 
                create: function (e) {
                    $('#Skip').change(function(e) {
          
                        var value = e.currentTarget.checked;
                        AutoConfirm = value;
                    });
                }
            });
        }, 
        InitAuditorName: function () { 
            var $dialog4 = $('#NewAuditorName').wijdialog({
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
                        
                        var AuditorDisplay = $(".Auditor-addname").css('display'); 
                        var EnteredName = $('#Name').val();
                         
                        switch (AuditorDisplay) { 
                            case 'none': 
                                var WeaverId = $("#select-weavername").val();
                                var existingName = $("#MainContent_Weaver_Names").val().toString().trim();
                                var WeaverInitial = "";

                                $.each(pageData.WeaversList, function (index, value) { 
                                    if (value.Id == WeaverId) { 
                                        WeaverInitial = value.Initials;
                                    }
                                });
                                if (existingName.length > 0) {existingName = existingName + ", "};
                                if (Inspection.PreConfirmWeavers.Weaver1ID == 0) { 
                                    Inspection.PreConfirmWeavers.Weaver1ID = WeaverId; 
                                    Inspection.PreConfirmWeavers.Weaver1Initials = WeaverInitial;
                                    $("#PreConfirmWeaver1Label").text( "New Weaver 1: " + WeaverInitial);
                                } else if (Inspection.PreConfirmWeavers.Weaver2ID == 0) { 
                                    Inspection.PreConfirmWeavers.Weaver2ID = WeaverId; 
                                    Inspection.PreConfirmWeavers.Weaver2Initials = WeaverInitial;
                                    $("#PreConfirmWeaver2Label").text( "New Weaver 2: " + WeaverInitial);
                                } else { 
                                    alert('Only two weavers allowed'); 
                                }                             
                                break; 
                            case 'block': 
                                if (EnteredName) {                        
                                    var x = document.getElementById("Auditor_Name");
                                    var option = document.createElement("option");
                                    option.text = EnteredName;
                                    option.value = EnteredName;
                                    x.add(option);
                                    $('#Auditor_Name').val(EnteredName);
                            
                                }
                                break;
                        }
                        $(this).wijdialog('close');               
                    },
                    'Close': function () {
                        var Enteredyards = new Number($("#weaveryards").val());
                        if (InspectionStartedVal == true && Enteredyards == 0) { 
                            alert("Enter Previous Weaver(s) Yards");
                        } else if (InspectionStartedVal == true && Inspection.PreConfirmWeavers.Weaver1ID == 0 &&  Inspection.PreConfirmWeavers.Weaver2ID == 0) {
                            alert("Please enter at least one weaver");
                        } else if (InspectionStartedVal == true && Enteredyards > 0) { 
                            datahandler.StartNewWeaverShift(); 
                        } else { 
                            Inspection.SetWeaversHTML(); 
                            $(this).wijdialog('close');
                            
                        }        
                    }
                },
                open: function () {
                    $('#Name').focus();
                    $('#Name').val('');

                    if (InspectionStartedVal == true) { 
                        $(".Weaver-addyards").css('display', 'block');
                    } else { 
                        $(".Weaver-addyards").css('display', 'none');
                    }

                },
                close: function () {
                    //$form.find('input').val("").end();
                    $("#PreConfirmWeaver1Label").text('');
                    $("#PreConfirmWeaver2Label").text('');

                    Inspection.PreConfirmWeavers = { Weaver1ID: 0, Weaver1Initials: "", Weaver2ID: 0, Weaver2Initials: "" };
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
            //$(ui.panel).empty();
            var totalcountnum = new Number(totalcount)
            var localarray = new Array(tabdatahandler.tabbuttonarray);
            var tabbuttoncount = controlhandler.gettabbuttoncount(tabselect, localarray);
            var buttonsize = controlhandler.sizebutton(tabbuttoncount)
            var counter = 0;
            var placementstring;
            //var buttoncolor = "#4875AE";
            var buttoncolor = "#CF0D39";
     
            for (i = 0; i < totalcount; i++) {
                if (localarray[0][i].tabindex == tabselect) {
                    var TimerStringhtml = '';
                    var spacer = "."; 

                    if (localarray && i < localarray[0].length && localarray[0][i].DefectType) { 
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
                            buttoncolor = "#14b71e";
                        } else if (localarray[0][i].DefectType == "Fix") { 
                            buttoncolor = "#95ea9a";
                        }
                    }
                    

                    if (localarray[0][i].DefectCode.length == 0) {spacer = ""}
                    if (buttonsize[0].height < 100) {buttonsize[0].height = 100}

                    placementstring = controlhandler.placebutton(counter, buttonsize[0].height, buttonsize[0].width)
                    
                    if (localarray[0][i].Timer == true) { 
                        timerplacementstring = controlhandler.placetimerbutton(counter, buttonsize[0].height, buttonsize[0].width, "START");
                        timerstopplacementstring = controlhandler.placetimerbutton(counter, buttonsize[0].height, buttonsize[0].width, "STOP");
                        timerlabelplacementstring = controlhandler.placetimerlabel(counter, buttonsize[0].height, buttonsize[0].width);
                        TimerStringhtml = '<button id="start_button_' + localarray[0][i].id.toString() + '" name="' + localarray[0][i].ButtonTemplateId.toString() + '" type="button" class="timerbutton" style="width:' + (buttonsize[0].width * .15).toString() + 'px;height:' + (buttonsize[0].height * .85).toString() + 'px; z-index: 100; position:absolute; ' + timerplacementstring + '; font-size:1.2em; background-color:#85b2cb;">START</button><button id="stop_button_' + localarray[0][i].id.toString() + '" type="button" name="' + localarray[0][i].ButtonTemplateId.toString()  + '" class="timerbutton" style="width:' + (buttonsize[0].width * .15).toString() + 'px;height:' + (buttonsize[0].height * .85).toString() + 'px; z-index: 100; position:absolute; ' + timerstopplacementstring + '; font-size:1.2em; background-color:rgba(111, 106, 107, 0.68);">STOP</button><input id="hiddenTimerId_' + localarray[0][i].id.toString() + '" type="hidden" name="TimerId" value="0"><label id="start_label_' + localarray[0][i].id.toString() + '" style="position:absolute; ' + timerlabelplacementstring + '; font-size:1.35em; z-index:110; font-weight:900; color: white;"></label>';
                    }
                    $(ui.panel).append(TimerStringhtml + '<button id="button' + localarray[0][i].id.toString() + '" name="' + localarray[0][i].ButtonTemplateId.toString() + '" type="button" class="buttontemplate" style="width:' + buttonsize[0].width + 'px;height:' + buttonsize[0].height + 'px; position:absolute; ' + placementstring + '; font-size:1.2em; background-color:' + buttoncolor + ';">' + localarray[0][i].ButtonLibraryId.toString() + '.<br />' + localarray[0][i].text + '</button>');
                    counter = counter + 1;
                    tabdatahandler.buttoncount = tabdatahandler.buttoncount + 1;
                }
            }
            tabdatahandler.newbuttonflag = false;
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
                    left = 15 + butwidth  - (butwidth * .18);
                } else { 
                    left = 15;
                }
                top = 86 + ((countnumber - 1) * butheight + 1) / 2;
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
        placetimerlabel: function (count, buttonheight, buttonwidth) { 
            var height = controlhandler.$tabs.height()
            var width = controlhandler.$tabs.width()
            var left;
            var top;
            var countnumber = new Number(count + 1);
            var butheight = new Number(buttonheight);
            var butwidth = new Number(buttonwidth);

           
            if (isOdd(countnumber) == 1) {
                left = 15;
                top = butheight + ((countnumber - 1) * butheight + 1) / 2;
            }
            else {
                left = 18 + butwidth;
                top = butheight + ((countnumber - 2) * butheight + 1) / 2
            }

            return 'left: ' + left.toString() + 'px; top: ' + top.toString() + 'px;'
        },
        toggleButtonColor: function (ui, antiui) { 
            if (ui.css('background-color') == 'rgb(133, 178, 203)') { 
                ui.css('background-color', 'rgba(111, 106, 107, 0.682353)')
                antiui.css('background-color', 'rgb(133, 178, 203)')
            
            } else { 
                alert("not active")
            }
        },
        gettabbuttoncount: function (index, localarray) {
            var count = 0;
            for (i = 0; i < localarray[0].length; i++) {
                if (localarray[0][i].tabindex == index) { count = count + 1 }
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
            var index = tabdatahandler.selectedtab; // Type:  number
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
                left = 5;
                top = 60 + ((countnumber - 1) * butheight + 1) / 2
            }
            else {
                left = 12 + butwidth;
                top = 60 + ((countnumber - 2) * butheight + 1) / 2
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
        RenderProductSpecTable: function () {
            $("#Specgrid").jqGrid({
                datatype: "json",
                url:      "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput_SpecLoad.ashx',
                editurl: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput_SpecSave.ashx',
                colNames: ['SpecId', 'TabTemplateId', 'Desc.', 'Val.', 'Upper Tol.', 'Lower Tol.', 'Meas.'],
                colModel: [
                        { name: 'SpecId', index: 'SpecId', hidden: true, editable: false },
                        { name: 'DataNo', index: 'DataNo', hidden: true },
                        { name: 'Spec_Description', index: 'Spec_Description', sortable: false, width: 26, editable: false },
                        { name: 'value', index: 'value', sortable: false, width: 10, editable: false },
                        { name: 'Upper_Spec_Value', index: 'Upper_Spec_Value', sortable: false, width: 10, editable: false },
                        { name: 'Lower_Spec_Value', index: 'Lower_Spec_Value', sortable: false, width: 10, editable: false },
                        { name: 'Measured_Value', index: 'Measured_Value', sortable: false, width: 30,  editable: true,editrules:{number:true},sorttype:'number',formatter:'number', editoptions: { dataInit: function(element) {   var row = 0; 
                            var elementid = $(element).attr('id'); 
                            var stringsplit = elementid.split("_");
                            if ($.isNumeric(stringsplit[0]) == true) { row = new Number(stringsplit[0]);  }
                            var rowdata = $('#Specgrid').jqGrid('getLocalRow', row);
 
                            if (rowdata && $.isNumeric(rowdata.value) ==true){
                                $(element).wijinputnumber({ 
                                    type: 'numeric', 
                                    minValue: 0, 
                                    maxValue: 10000000, 
                                    decimalPlaces: 2, 
                                    increment: 25, 
                                    showSpinner: true,
                                    value: rowdata.value,
                                    valueChanged : function (e, data) {
 
                                        var specdelta = data.value - rowdata.value
 
                                        var specnum = new Number(specdelta); 
                                        var uppnum = new Number(rowdata.Upper_Spec_Value); 
                                        var lownum = new Number(rowdata.Lower_Spec_Value); 
                                        var rowclass = $('.ui-state-highlight');
                                        if (specnum > uppnum || specnum < lownum) { 
 
                                            rowclass.css('border','1px solid #333');
                                            rowclass.css('background','rgb(116, 0, 0) 50% 50% repeat-x'); 
                                            rowclass.css('background-color','rgba(114, 0, 0, 0.48)'); 
                                            rowclass.css('color','#363636'); 
                                        } else { 
                                            rowclass.css('border','1px solid #fcefa1');
                                            rowclass.css('background','#fbf9ee url(images/ui-bg_glass_55_fbf9ee_1x400.png) 50% 50% repeat-x'); 
                                            rowclass.css('background-color',''); 
                                            rowclass.css('color','#363636'); 
                                        }
                                    }
                                });
                            }
                        }} }
                ],
                pager: '#pSpecgrid',
                caption: "Product Spec Entry",
                multiselect: false,
                loadonce: true,
                rowNum: 10,
                viewrecords: true,
                sortorder: "desc",
                width: new Number(1050),
                postData: { 
                    DataNo: function () { 
                        return $("#MainContent_DataNumber").val();
                    }, 
                    WorkOrder: function () { 
                        return $workorder.val();
                    },
                    CID: function () { 
                        return selectedCID;
                    },
                    SessionID: function () { 
                        return SessionID; 
                    }
                },
                height: "100%",
                ondblClickRow: function (id) {
                    var rowdata = $('#Specgrid').jqGrid('getLocalRow', id);

                    //jQuery('#Specgrid').jqGrid('editRow', id, true, null, null, "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput_SpecSave.ashx', {DefectId: DefectID, InspectionId: InspectionId, TemplateId: SelectedId, TabName: SelectedTab, TabNumber: UserSelectedTabNumber, SpecId: rowdata.SpecId});
                    SpecGridEditId = id;
                   
    //                jQuery("#Specgrid").jqGrid('editGridRow', id, {
    //                    addCaption: "Add Record",
    //                    editCaption: "Add Measurement",
    //                    bSubmit: "Submit",
    //                    bCancel: "Cancel",
    //                    bClose: "Close",
    //                    bYes : "Yes",
    //                    bNo : "No",
    //                    bExit : "Cancel"
                    //} );
                    
              
                },
                gridComplete: function () {
                    var mydata = $("#Specgrid").jqGrid('getGridParam','data');
                    if (mydata.length > 0) { 
                        $("#EnterSpec").val("Specs (" + mydata.length + ")");
                    }
                    
                }
            });
            
            jQuery("#Specgrid").jqGrid('navGrid','#pSpecgrid',{
                edit: false,
                add: false,
                del: false,
                search: false,
                edittext: "Edit",
                refresh: false
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
                },
                editData: {
                    SpecId: function () { 
                        var sel_id = $("#Specgrid").jqGrid('getGridParam', 'selrow');
                        var value = $("#Specgrid").jqGrid('getCell', sel_id, 'SpecId');
                        return value;
                    },
                    InspectionSummaryId: function () { 
                        return InspectionJobSummaryIdPage;
                    },
                    InspectionId: function () { 
                        return InspectionId;
                    }
                }
            });
            jQuery("#Specgrid").jqGrid('inlineNav','#pSpecgrid', {
                edit: true,
                editicon: '',
                edittext: 'edit',
                add: false,
                addicon:"ui-icon-plus",
                save: true,
                savetext:"save",
                cancel: false,
                cancelicon:"ui-icon-cancel",
                addParams: {SpecId: 123},
                editParams: {
                    keys: true,
                    extraparam: { 
                        SpecId: function () { 
                            var sel_id = $("#Specgrid").jqGrid('getGridParam', 'selrow');
                            var value = $("#Specgrid").jqGrid('getCell', sel_id, 'SpecId');
                            return value;
                        },
                        InspectionSummaryId: function () { 
                            return InspectionJobSummaryIdPage;
                        },
                        InspectionId: function () { 
                            return InspectionId;
                        },
                        WorkOrder: function () {
                            return $workorder.val();
                        },
                        WOQuantity: function () { 
                            return $LotSize.val(); 
                        },
                        AQL: function () { 
                            return 1.5;
                        },
                        Inspector: function () { 
                            return $auditorname.val(); 
                        },
                        TemplateId: function () { 
                            return SelectedId;
                        },
                        Location: function () {
                            return selectedCID.toString();
                        },
                        SampleSize: function () { 
                            return $SampleSize.val();
                        },
                        DataNo: function () { 
                            return $DataNo.val();
                        },
                        RejectLimiter: function () { 
                            return $RE.val();
                        },
                        InspectionState: function() { 
                            return InspectionState;
                        },
                        CPNumber: function () { 
                            return $CPNumber.val(); 
                        }, 
                        SpecItemCount: function () { 
                            return SpecItemCounter; 
                        }
                    },
                    aftersavefunc: function () { 
       
                        var sel_id = $("#Specgrid").jqGrid('getGridParam', 'selrow');
                        var Mvalue = new Number($("#Specgrid").jqGrid('getCell', sel_id, 'Measured_Value'));
                        var Uvalue = new Number($("#Specgrid").jqGrid('getCell', sel_id, 'Upper_Spec_Value'));
                        var Lvalue = new Number($("#Specgrid").jqGrid('getCell', sel_id, 'Lower_Spec_Value'));
                        var Svalue = new Number($("#Specgrid").jqGrid('getCell', sel_id, 'value'));
 

                        if (Mvalue && Uvalue && Lvalue && Svalue) { 
                            var specdelta = Mvalue - Svalue; 
                            if (specdelta <= Uvalue && specdelta >= 0) { 
 
                                var rowclass = $('.ui-state-highlight');
                                    rowclass.css('border','1px solid #333');
                                    rowclass.css('background','rgba(16, 168, 28, 0.478431) 50% 50% repeat-x'); 
                                    rowclass.css('background-color','rgba(16, 168, 28, 0.478431);'); 
                                    rowclass.css('color','#363636'); 
                                 
                            }
                            if (specdelta >= Lvalue && specdelta < 0) { 
  
                                var rowclass = $('.ui-state-highlight');
                                rowclass.css('border','1px solid #333');
                                rowclass.css('background','rgba(16, 168, 28, 0.478431) 50% 50% repeat-x'); 
                                rowclass.css('background-color','rgba(16, 168, 28, 0.478431);'); 
                                rowclass.css('color','#363636'); 
                                 
                            }
                            if (Mvalue == Svalue) { 
                                var rowclass = $('.ui-state-highlight');
                                rowclass.css('border','1px solid #333');
                                rowclass.css('background','rgb(116, 0, 0) 50% 50% repeat-x'); 
                                rowclass.css('background-color','rgba(16, 168, 28, 0.478431);'); 
                                rowclass.css('color','#363636'); 
                            }
                        }
                    }
                }
            });
        },
        SpecActivator: function (TabNumber) {
            var SpecCount = new Number(0);
            var specColl = SpecCollection;
            var gridData = [];
            var mydata = $('#Specgrid').jqGrid('getGridParam', 'data');
          
            $.each(specColl, function (index, value) {
  
                var rowid = index + 1;
                if (TabNumber == value.TabNumber && SpecCount == 0) {
                    $('#SpecTable').css('display', 'inline');
                    
                }
                if (TabNumber == value.TabNumber) {
                    gridData.push(value);
                   
                    SpecCount = SpecCount + 1;
                }

            });
            jQuery("#Specgrid").jqGrid('clearGridData', true).trigger('reloadGrid');
            jQuery("#Specgrid").jqGrid('setGridParam', { data: gridData }).trigger('reloadGrid');

            if (SpecCount == 0) {
                $('#SpecTable').css('display', 'none');
            }
        }, 
        InitInspectionDisplay: function (LineType) { 
            var InspectionTypeSelector = -1;
            switch(LineType) { 
                case 'ROLL':
                    $("#AQdiv").css('display', 'none'); 
                    $("#AQ_Level").val('100');
                    $("#AQL_Level_Dialog").val('100');
                    AQLValue = '100';
                    $("#MainContent__AQLevel").val('100');
                    $("#AQL_Level_Dialog option:selected").attr('disabled', 'disabled');
                    $("#AQ_Level option:selected").attr('disabled', 'disabled');
                    $("#AQL_Standard_Dialog option:selected").attr('disabled', 'true');
                    InspectionTypeSelector = 1;
                    datahandler.GetWeaverNames(); 
                    break;
                case 'IL': 
                    $("#AQ_Level").val('100');
                    $("#AQL_Level_Dialog").val('100');
                    AQLValue = '100';
                    $("#MainContent__AQLevel").val('100');
                    $("#MainContent_aqlstandard").val('Regular');
                    $("#AQL_Standard_Dialog").val('Regular');
                    $("#AQL_Level_Dialog").prop('disabled', true);
                    $("#AQ_Level").prop('disabled', true);
                    $("#AQL_Standard_Dialog").prop('disabled', true);
                    InspectionTypeSelector = 0;
                    if (OpenOrderFlag == "False") { 
                        //eventshandler.UserKeyPress.GetLastUserInputs();
                    }
                    break;
                case 'EOL':
                    var NameCookie1 = '2.5' 
                    //Template.GetCookie("AQLLevel");
                    $("#AQL_Level_Dialog").prop('disabled', false);
                    $("#AQ_Level").prop('disabled', false);

                    $("#AQL_Standard_Dialog").prop('disabled', false);
                    if (OpenOrderFlag == "False") { 
                        //    eventshandler.UserKeyPress.GetLastUserInputs();
                        var NameCookie2 = Template.GetCookie("AQLStandard");
                        if (NameCookie2.length > 1 && NameCookie2 != null) { 
                            $("#AQL_Standard_Dialog").val(NameCookie2);
                            $("#MainContent_aqlstandard").val(NameCookie2);
                        } else { 
                            $("#MainContent_aqlstandard").val('Regular');
                            $("#AQL_Standard_Dialog").val('Regular');
                        }
                    } else {
                        $("#AQL_Level_Dialog").prop('disabled', true);
                        $("#AQ_Level").prop('disabled', true);
                        $("#AQL_Standard_Dialog").prop('disabled', true);
                    }
                    InspectionTypeSelector = 0;
                    break;
            };
            controlhandler.setInspectionType(InspectionTypeSelector);
        },
        setInspectionType: function(InspectionTypeSelector) { 
            switch (InspectionTypeSelector) {
                case 0:
                    InspectionTypeState = "WorkOrder";
                    TargetOrderInput = $('#MainContent_WorkOrder');            
                    $(".workorder-inspection").css('display', 'block'); 
                    $(".roll-inspection").css('display', 'none'); 
                    $("#MainContent_CPNumber").prop('disabled', false);
                    $("#InspectionType").prop('innerText', "WORK ORDER");
                    $("#WOQuantityL").text("WO QUANTITY");
                    $("#DataNumberL").prop('innerText', "DATA NUMBER");
                    $("#AuditorNameL").prop('innerText', "AUDITOR NAME");
                    $("#InspectionType").css("top", "-7px");
                    $("#SampleSizeLabel").text("SAMPLE SIZE");
                    InspectionState = 0;
                    break;
                case 1:
                    InspectionTypeState = "RollNumber";
                    TargetOrderInput = $('#MainContent_RollNumber');
                    $(".roll-inspection").css('display', 'block'); 
                    $(".workorder-inspection").css('display', 'none'); 
                    $("#MainContent_CPNumber").prop('disabled', true);
                    $("#InspectionType").prop('innerText', "ROLL");
                    $("#WOQuantityL").text("YARDS");
                    $("#DataNumberL").prop('innerText', "RM #");
                    $("#AuditorNameL").prop('innerText', "OPERATOR");
                    $("#InspectionType").css("top", "-1px");
                    RollYards = new Number($("#WOQuantity").val());
                    $("#WOQuantity").change(function () {
                        RollYards = new Number($("#WOQuantity").val());
                    });
                    InspectionState = 1;
                    $("#SampleSizeLabel").text("DHY");
                    break;
                default:
                    InspectionTypeState = "WorkOrder";
                    TargetOrderInput = $('#MainContent_WorkOrder');
                    $(".workorder-inspection").css('display', 'block'); 
                    $(".roll-inspection").css('display', 'none'); 
                    $("#MainContent_CPNumber").prop('disabled', false);
                    $("#InspectionType").prop('innerText', "WORK ORDER");
                    $("#WOQuantityL").text("WO QUANTITY");
                    $("#DataNumberL").prop('innerText', "DATA NUMBER");
                    $("#AuditorNameL").prop('innerText', "AUDITOR NAME");
                    $("#InspectionType").css("top", "-7px");
                    $("#SampleSizeLabel").text("SAMPLE SIZE");
                    InspectionState = 0;
            };
            $("#MainContent_InspectionState").val(InspectionTypeState);
        }


    };

    var controls = { 
        InitTemplateDropDown: function (TemplateNames) { 
            var html = [];
            var name;
            for(var i = 0; i < TemplateNames.length; i++){
                name = TemplateNames[i];
                html.push('<option value="'+name.id+'">'+name.text+'</option>');
            }
            $("#selectNames").html(html.join('')).bind("change", function(){
           
                var selectedid = $(this).val();
                if (SelectedName == "") {
                    NavCop = true;
                } else { 
                    NavCop = false;
                }
                $("#MainContent_Templatename").val($("#selectNames option:selected").text());
                if (selectedid) {
                    var querystring = "TemplateId="+ selectedid.toString() + "&AQL=" + $aql.val()
                    var hash;
                    $.when( $.ajax({
                        url: "<%=Session("BaseUri")%>" + '/handlers/Utility/cypher.ashx',
                        type: 'POST',
                        data: {"querystring": querystring},
                        success: function (data) {
                        
                            hash = data;
                        },
                        error: function (a, b, c) {

                        }
                    }) ).done(function ( v1) {
              
                        window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?" + querystring + "&CID_Info=000" + selectedCIDnum + "&qtpval=" + hash.toString())
                    });
                } 
            })
            .val(SelectedId);
            $("#MainContent_Templatename").val($("#selectNames option:selected").text());
        }, 
        InitLocationDropDown: function (LocationNames, selectedCIDnum, selectedCID) { 
            var html = [];
            html.length = 0;
            var name;
            for(var i = 0; i < LocationNames.length; i++){
                name = LocationNames[i];
                html.push('<option value="'+name.CID+'">'+name.text+'</option>');
            }
      
            $("#MainContent_Location").html(html.join('')).bind("change", function(){

                var selectedid = $(this).val();

            
                $("#MainContent_LocationSelected_Hidden").val(selectedCID);
            });
        
            $("#MainContent_Location").val(selectedCIDnum);
            $('#MainContent_SampleSizeHidden').val($('#MainContent_SampleSize').val())
            $("#MainContent_LocationSelected_Hidden").val($("#MainContent_Location").val());
        }, 
        InitWeaversDropDown: function (WeaverNames) { 
            var html = []; 
            html.length = 0; 
           
            if (WeaverNames != null) { 
                for (var i = 0; i < WeaverNames.length; i++) { 
                    html.push('<option value="'+ WeaverNames[i].Id+'">'+ WeaverNames[i].FirstName + ' ' + WeaverNames[i].LastName + '</option>');
                }
                $("#select-weavername").html(html.join(''));            
            }
        },
        InitMachineNameDropdowns: function (MachineNames) { 
            var html = [];
            html.length = 0 
            var machiename;
            if (machiename != null) { 
                for(var i = 0; i < MachineNames.length; i++) { 
                    machinename = MachineNames[i];
                    html.push('<option value="' + machiename.id + '">' + name.text + '</option>');
                }
                $("#MachineNames_pop").html(html.join('')).bind("change", function(){

                    var selectedid = $(this).val();
                    var selectedtext = $(this).text();

                    Template.SetCookie("InspectionType", 0, 14);
                    if (selectedtext) { 
                        var querystring = "TemplateId=" + SelectedId.toString() + "&Username=" + $auditorname.val().toString() + "&SPCMachine=" + selectedtext + "&AQL=" + $aql.val() + "&AS400REQ=SPCMachine&LocationId=" + $("#MainContent_Location").val()
                        window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?" + querystring)
                    }
      
                });
            }
        }, 
        InitMenu: function () { 
            $("#menu").wijmenu({
                checkable: true,
                maxHeight: new Number(50),
                select: function (e, data) {
                
                    var selectedtext = e.currentTarget.innerText;
                    //GetCypherHash("<%=Session("BaseUri")%>")
         
                    switch (selectedtext) {
                        case "INPUT DEFECTS":
                            window.location.assign("<%=Session("BaseUri")%>" + '/APP/Mob/SPCInspectionInput.aspx');
                            break;
                        case "TEMPLATE UTILITY":
                            window.location.assign("<%=Session("BaseUri")%>" + '/APP/DataEntry/SPCInspectionUtility.aspx');
                            break;
                        case "RESULTS":
                            window.location.assign("<%=Session("BaseUri")%>" + '/APP/Presentation/InspectionVisualizer.aspx');
                            break;
                        case "MAIN MENU":
                            var hash = 'aprtrue';
                            $.when( $.ajax({
                                url: "<%=Session("BaseUri")%>" + '/handlers/Utility/cypher.ashx',
                                type: 'POST',
                                data: {"querystring": 'aprtrue'},
                                success: function (data) {
                           
                                    hash = data;
                                },
                                error: function (a, b, c) {

                                }
                            }) ).done(function ( v1) {
                                // v1 is undefined
                                window.location.assign("<%=Session("BaseUri")%>" + '/APP/APR_SiteEntry.aspx?zrt=' + hash);
                            });
                        
                            break;
                        case "MAINTENANCE":
                            window.location.assign('maintenance.standardtextile.com?CID=' + selectedCID);
                            break;
                        case "DASHBOARD":
                            url = 'dashboard.standardtextile.com';
                            window.location.assign(url);
                            Destination = "dashboard";
                            break;
                    }
                }
            });
        }, 
        InitAqlDropDown: function (AQLValue) { 
            var aqarray = [{ id: 4, text: 'AQL Level 4.0' },
                       { id: 1, text: 'AQL Level 1.0' },
                       { id: 1.5, text: 'AQL Level 1.5' },
                       { id: 2.5, text: 'AQL Level 2.5' },
                       { id: 100, text: '100% Inspection'}];
            var html2 = [];
            var level;
        
            for(var i = 0; i < aqarray.length; i++){
                level = aqarray[i];
                html2.push('<option value="'+level.id+'">'+level.text+'</option>');
            }
 
            $("#AQ_Level").html(html2.join('')).bind("change", function(){
                $("#AQL_Level_Dialog").val($(this).val());
                $("#MainContent__AQLevel").val($(this).val());
                AQLValue = $(this).val();
 
            });
            $("#AQ_Level").val(AQLValue);
            $("#MainContent__AQLevel").val(AQLValue);
            
            $("#AQL_Level_Dialog").html(html2.join('')).bind("change", function(){
                $("#AQ_Level").val($(this).val());
                $("#MainContent__AQLevel").val($(this).val());
                AQLValue = $(this).val();
             
            });
            $("#AQL_Level_Dialog").val(AQLValue);

        }, 
        InitAqlStandardDropDown: function () { 
            var aqstandardarray = [{ id: 'Reduced', text: 'Reduced' },
                       { id: 'Regular', text: 'Regular' },
                       { id: 'Tightened', text: 'Tightened' }];
            var html3 = [];
            var level1;
            for(var i = 0; i < aqstandardarray.length; i++){
                level1 = aqstandardarray[i];
                html3.push('<option value="'+level1.id+'">'+level1.text+'</option>');
            }
            $("#AQL_Standard_Dialog").html(html3.join('')).bind("change", function(){
                $("#MainContent_aqlstandard").val($(this).val());
            });
            $("#AQL_Standard_Dialog").val('Regular');
        }, 
        InitWOQuantity: function () { 
            var WOQCookieVal = Template.GetCookie("WOQuantity")
   
            if (WOQuantity == "" || WOQuantity == 0 || WOQuantity == "0" || WOQuantity == null) { 
                var cookiequantity = new Number(WOQCookieVal)
                if (cookiequantity > 0) { 
                    WOQuantity = WOQCookieVal
                }
            }
            $("#WOQuantity").wijinputnumber({ 
                type: 'numeric', 
                minValue: 0, 
                maxValue: 10000000, 
                decimalPlaces: 0, 
                showSpinner: false,
                value: new Number(WOQuantity)
            });
            if (WOQuantity) {
                $("#MainContent_woquantity_hidden").val(WOQuantity);
            }
        }, 
        SetAuditFromCookie: function () { 
            var NameCookie = Template.GetCookie("AuditorName");
            if (NameCookie.length > 1) { 
            
                if (NameCookie != "New Name" && NameCookie.toString().length > 0) {
                    $("#Auditor_Name").val(NameCookie);
                    $("#MainContent_AuditorName").val(NameCookie);
                    $("#Auditor_Name option[value='SELECT OPTION']").remove();
                    //$("#Auditor_Name option[value='New Name']").remove();
                } else {
                    $('#Auditor_Name').val('SELECT OPTION');
                }
            }
        }, 
        InitNumbers: function () { 
            $("#TotalCountValue").wijinputnumber({ 
                type: 'numeric', 
                minValue: 0, 
                maxValue: 10000000, 
                decimalPlaces: 0, 
                showSpinner: true, 
                valueChanged : function (e, data) {
                    var rejcount = new Number($("#MainContent_Bad_Group").val()); 
                    var Inspected = new Number(data.value); 
                    var PassCount = new Number(Inspected - rejcount); 
                    $("#PassCountValue").text(PassCount);
                    $("#MainContent_Good").val(PassCount); 
                    $('#MainContent_totalinspecteditems').val(data.value);

                }
            });
            $("#TotalCountValue").height(50);
            $("#TotalYardValue").wijinputnumber({ 
                type: 'numeric', 
                minValue: 0, 
                maxValue: 10000000, 
                decimalPlaces: 0, 
                showSpinner: true, 
                valueChanged : function (e, data) {
                    $('#MainContent_totalinspectedyards').val(data.value);

                }
            });
            $("#TotalYardValue").height(30);
            $("#WeaverShiftYards").wijinputnumber({ 
                type: 'numeric', 
                minValue: 0, 
                maxValue: 10000000, 
                decimalPlaces: 0, 
                showSpinner: true, 
                valueChanged : function (e, data) {
                    $('#MainContent_WeaverShiftYards_hidden').val(data.value);

                }
            });
            $("#WeaverShiftYards").height(30);
        }

    };
    var Template = {
        Load: function () {
            var json = TemplateCollection;
  
            if (!json) { 
                $('#Default').toggle(); return
            }
            var length = json.length - 1;
            var tabnumber = 99;
            var returnsValue;
            var counter = 0;
            var buttoncounter = 1;
            var selectedtab = tabselect;
            $('#loading').toggle();
            var refreshId = setInterval(function () {

                if (counter == length || length < 1) {
                    clearInterval(refreshId);
                }
                if (json[counter]) {
                    if (tabnumber != json[counter].TabNumber) {
                        tabselect = json[counter].TabNumber;
                        if (tabdatahandler.tabbuttonarray.length > 0) { 
                            tabdatahandler.tabbuttonarray.length = 0;
                        }
                
                        for (i = 0; i < json.length; i++) {
                            if (json[i].TabNumber == json[counter].TabNumber) {
                                if (json[i].ButtonId != 0 && json[i].ButtonName != "NaN") {
                                    
                                    tabdatahandler.tabbuttonarray.push({ text: json[i].ButtonName, tabindex: json[counter].TabNumber, id: buttoncounter, ButtonId: json[i].ButtonId, DefectType: json[i].DefectType, ButtonTemplateId: json[i].id, DefectCode: json[i].DefectCode, ButtonLibraryId: json[i].ButtonLibraryId, Timer: json[i].Timer });
                                }
                                buttoncounter++;
                            }
                        }
       
                        tabnumber = json[counter].TabNumber;
                        if (json[counter].ProductSpecs = true || json[counter].ProductSpecs == "true") {
                            HasProductSpecs.push(json[counter].TabNumber);
                        }
                        if (json[counter].Name != null) {
                            controlhandler.$tabs.wijtabs('add', '#tab-' + json[counter].TabNumber.toString(), json[counter].Name);
                            controlhandler.tabarray.push({ title: json[counter].Name.toString(), TabTemplateid: json[counter].TabTemplateId });
                            if (controlhandler.tabarray.length > 0) {
                                SelectedTab = controlhandler.tabarray[0].title
                            }
                        }
                    
                    }
                }
                counter++;

            }, 50);
            tabselect = selectedtab;
            
            var loaderswitch = setTimeout(function () {
                $('#loading').toggle();
                if (TemplateCollection == null) {
                    $('#Default').toggle();
                }
            }, 50 * (length + 1));
        }, 
        SetCookie: function(cname, cvalue, exdays) { 
            var d = new Date();
            d.setTime(d.getTime() + (exdays*24*60*60*1000));
            var expires = "expires="+d.toUTCString();
            document.cookie = cname + "=" + cvalue + "; " + expires;
        },
        GetCookie: function(cname) { 
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for(var i=0; i<ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0)==' ') c = c.substring(1);
                if (c.indexOf(name) == 0) return c.substring(name.length,c.length);
            }
            return "";
        }
    };
    var datahandler = {
        SubmitDefect: function (ButtonId, ButtonValue, ButtonName, InspectionJobSummaryId, InspectionId) {
            var FirstInspectionFlag = false;
            var returnnum = new Number(0);
            if (InspectionJobSummaryId == -99) { 
                FirstInspectionFlag = true;
            }

            var mycolor = $("#" + ButtonId).css("background-color");
            if (mycolor) { 
                $("#" + ButtonId).css("background-color", '#A6ACB4');
            }
            
            $.when(datahandler.GetSelectElements()).done(function (elementarrayval) { 
                var JsonString = JSON.stringify(elementarrayval);
                var IsMajor = true;
            
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'POST',
                    data: { method: 'InsertDefect', args: { id: ButtonId, text: ButtonValue, JsonString: JsonString, ButtonTemplateId: ButtonName, InspectionJobSummaryId: InspectionJobSummaryId, InspectionId: InspectionId, WeaverShiftIdVal: Inspection.WeaverShiftId  } },
                    success: function (data) {
                        
                        returnnum = new Number(data[0].DefectId);
                        var datarray = JSON.parse(data);
       
                        if (data && data != -1) { 
                            $("#MainContent_inspectionjobsummaryid_hidden").val(datarray[0].InspectionJobSummary);
                            InspectionJobSummaryIdPage = datarray[0].InspectionJobSummary
                            DefectID = datarray[0].DefectId

                            var dhunumber = new Number(datarray[0].DHU);
                            $("#MainContent_DHU").val(dhunumber.toFixed(3).toString());
                            $("#<%=InspectionId.ClientID%>").val(datarray[0].InspectionJobSummary);
                            if (FirstInspectionFlag == true) { 
                                setInterval(
                                    function () { 
                                        datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage) }
                                    , 10000);
                            }
                            
                            if (returnnum != -1 || returnnum != 0) { 
                                var $bad = $('#MainContent_Bad_Local');
                                var $bad_Group = $('#MainContent_Bad_Group');
                                if (datarray[0].DefectType != 'MINOR' && datarray[0].DefectType != 'TIME' && datarray[0].DefectType != 'FIX' && datarray[0].DefectType != 'UPGRADE') { 
                                    LocalCounts++; 
                                    
                                    $bad_Group.val(new Number($bad_Group.val()) + 1); 
                                    var badval = new Number($bad_Group.val()) 
                                    $bad.val(LocalCounts);
                                }
                                $("#" + ButtonId).css("background-color", mycolor)
                                $("#MainContent_InspectionState").val(InspectionTypes[InspectionState]);
                                if (InspectionState == 1) {
                                    DefectsPerhundredYards = (badval /( RollYards/100));
                                    $("#MainContent_SampleSize").val(DefectsPerhundredYards);
                                    $("#MainContent_DHYHidden").val(DefectsPerhundredYards); 
                                    if (DefectsPerhundredYards > 10) {
                                        $("#MainContent_SampleSize").css("background-color", "red");
                                        $("#MainContent_SampleSize").css("color", "white");
                                        $("#MainContent_SampleSize").css("font-weight", "900");
                                    }
                                }
                                var WOQuantityval = $("#WOQuantity").val();
                                if (WOQuantityval) {
                                    Template.SetCookie("WOQuantity", WOQuantityval, 100)
                                }
                            
                                $("#MainContent_workorder_hidden").val($("#MainContent_WorkOrder").val());
                            }
                        }
                        
                        
                        $('#MainContent_DefectID_Value').val(returnnum.toString());
                    },
                    error: function (a, b, c) {
                        alert(c);
                        
                    }
                });

            });
            return returnnum;
        },
        SubmitDefect_2: function (ButtonId, ButtonValue, ButtonName, InspectionJobSummaryId, InspectionId, JsonString, TimerFlag, ButtonTemplateId, TimerValue, ButtonLocationId, ui, antiui) {
            var FirstInspectionFlag = false;
            var returnnum = new Number(0);
            if (InspectionJobSummaryId == -99) { 
                FirstInspectionFlag = true;
            }
            var IsMajor = true;
            var mycolor = $("#" + ButtonId).css("background-color");
            if (mycolor) { 
                $("#" + ButtonId).css("background-color", '#A6ACB4');
            }
           
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                type: 'POST',
                data: { method: 'InsertDefect', args: { id: ButtonId, text: ButtonValue, JsonString: JsonString, ButtonTemplateId: ButtonName, InspectionJobSummaryId: InspectionJobSummaryId, InspectionId: InspectionId} },
                success: function (data) {
                        
                    returnnum = new Number(data[0].DefectId);
                    var datarray = JSON.parse(data);
       
                    if (data && data != -1) { 
                        $("#MainContent_inspectionjobsummaryid_hidden").val(datarray[0].InspectionJobSummary);
                        InspectionJobSummaryIdPage = datarray[0].InspectionJobSummary
                        DefectID = datarray[0].DefectId

                        var dhunumber = new Number(datarray[0].DHU);
                        $("#MainContent_DHU").val(dhunumber.toFixed(3).toString());
                        $("#<%=InspectionId.ClientID%>").val(datarray[0].InspectionJobSummary);
                        if (FirstInspectionFlag == true) { 
                            setInterval(
                                function () { 
                                    datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage) }
                                , 10000);
                        }
 
                        if (returnnum != -1 || returnnum != 0) { 
                            var $bad = $('#MainContent_Bad_Local');
                            var $bad_Group = $('#MainContent_Bad_Group');
                            if (datarray[0].DefectType != 'MINOR' && datarray[0].DefectType != 'TIME' && datarray[0].DefectType != 'FIX' && datarray[0].DefectType != 'UPGRADE') { 
                                LocalCounts++; 
                                    
                                $bad_Group.val(new Number($bad_Group.val()) + 1); 
                                var badval = new Number($bad_Group.val()) 
                                $bad.val(LocalCounts);
                            }
                            if (TimerFlag == true) {
                                try { 
                                    datahandler.ToggleTimerStatus(ButtonTemplateId, TimerValue, datarray[0].DefectId, ButtonLocationId, ui, antiui);
                                } catch(err) {
                                    alert(err)
                                }
                            }
                            $("#" + ButtonId).css("background-color", mycolor)
                            $("#MainContent_InspectionState").val(InspectionTypes[InspectionState]);
                            if (InspectionState == 1) {
                                DefectsPerhundredYards = (badval /( RollYards/100));
                                $("#MainContent_SampleSize").val(DefectsPerhundredYards);
                                $("#MainContent_DHYHidden").val(DefectsPerhundredYards); 
                                if (DefectsPerhundredYards > 10) {
                                    $("#MainContent_SampleSize").css("background-color", "red");
                                    $("#MainContent_SampleSize").css("color", "white");
                                    $("#MainContent_SampleSize").css("font-weight", "900");
                                }
                            }
                            var WOQuantityval = $("#WOQuantity").val();
                            if (WOQuantityval) {
                                Template.SetCookie("WOQuantity", WOQuantityval, 100)
                            }
                            
                            $("#MainContent_workorder_hidden").val($("#MainContent_WorkOrder").val());
                        }
                    }
                        
                        
                    $('#MainContent_DefectID_Value').val(returnnum.toString());
                },
                error: function (a, b, c) {
                    alert(c);
                        
                }
            });
            return returnnum;
        },
        ToggleTimerStatus: function (ButtonTemplateId, StatusValue, DefectID, ButtonLocationId, ui, antiui) { 

            var TimerId; 
            var TimerVal = 0; 
          
            if (ButtonLocationId) {
                TimerId = $("#hiddenTimerId_" + ButtonLocationId.toString()); 
               
                if (TimerId) {
                    TimerVal = TimerId.val();
                }
            }
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx', 
                type: 'GET',
                data: { method: 'ToggleTimerStatus', args: {InspectionJobSummaryId: InspectionJobSummaryIdPage, ButtonTemplateId: ButtonTemplateId, StatusValue: StatusValue, DefectID: DefectID, SessionId: SessionID, ButtonLocationId: ButtonLocationId, TimerId: TimerVal}},
                success: function (data) {

                    try { 
                        if (StatusValue == "START" && data) { 
                            var parseArr = $.parseJSON(data);
                           
                            if ($("#hiddenTimerId_" + ButtonLocationId.toString()) && parseArr) { 
                                $("#hiddenTimerId_" + ButtonLocationId.toString()).val(parseArr.TimerId);
                                var startdate = Date.parse(parseArr.StartTime);

                                $("#start_label_" + ButtonLocationId.toString()).text("startDate: " + convertTime(startdate));
                            } else {alert("hidden field not found")}
                            
                        } else { 
                            if (data == "false") { 
                                alert("TimerStatus Entry Failed"); 
                                return false; 
                            } else if (data == "true") { 
                                $("#start_label_" + ButtonLocationId.toString()).text('');
                                $("#hiddenTimerId_" + ButtonLocationId.toString()).val(0);
                                return true; 
                            }
                        }
                    } catch (err) { 
                        alert(err);
                    }
                    
                    
                }, 
                error: function (a,b,c) { 
                    return false;
                }
            });

        },
        InspectionEnd: function () {

            $.when(datahandler.GetSelectElements()).done(function (elementarrayval) { 
                var JsonString = JSON.stringify(elementarrayval);

                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'GET',
                    data: { method: 'InspectionEnd', args: {JsonString: JsonString } },
                    success: function (data) {

                    },
                    error: function (a, b, c) {

                    }
                });
            });
        },
        GetSelectElements: function () { 
            var elementarrayval = [];
            $(".inputelement").each(function () {
                var elementid = $(this).attr('id');
                var elementval = $(this).val();
                var splitelement = elementid.split("_")
                if (elementid != undefined || elementid != null) {
                    if (splitelement[0] !== "s2id") {
                        elementarrayval.push({ value: elementval, key: elementid });

                    }
                }
            });
            elementarrayval.push({ value: "", key: "Comment" });
            elementarrayval.push({ value: "", key: "Dimensions" });
            elementarrayval.push({ value: SelectedId, key: "TemplateId" });
            if (typeof SelectedTab === "undefined") {
                if (controlhandler.$tabs[0].firstChild.children.length > 0) {
                    SelectedTab = controlhandler.$tabs[0].firstChild.children[0].firstChild.innerText
                    elementarrayval.push({ value: SelectedTab, key: "Product" });
                } else {
                    elementarrayval.push({ value: "None", key: "Product" });
                }
            } else {
                elementarrayval.push({ value: SelectedTab, key: "Product" });
            }
            return elementarrayval

        },
        GetInspectionId: function () { 
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                 type: 'GET',
                 data: { method: 'GetInspectionId'},
                 success: function (data) {
                     if (data) { 
                         
                             var returnnum = new Number(data);
                     
                             //$("#<%=InspectionId.ClientID%>").val(data);
                             if (returnnum > 0 ) { 

                                 $("#MaintContent_InspectionId").val(returnnum.toString());
                                 InspectionId = returnnum;
                             }
                     }
                 },
                 error: function (a, b, c) {
                     alert(c);
                     
                 }
            });
        },
        GetSPCWorkOrder: function (MachineNameInput) { 
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                type: 'GET',
                data: { method: 'GetCurrentSPCMachineWorkOrder', args: {ijsid: InspectionJobSummaryIdPage, MachineName: MachineNameInput}},
                success: function (data) {
                    if (data) { 
                        var parsedata = $.parseJSON(data); 
                  
                        if(parsedata[0] != 0){ 

                             $workorder.val(parsedata[0].JobNumber); 
                             $DataNo.val(parsedata[0].DataNo); 
                             $('#WOQuantity').val(parsedata[0].WOQuantity); 
                             $('#MainContent_workorder_hidden').val(parsedata[0].JobNumber); 
                             $("#MainContent_inspectionjobsummaryid_hidden").val(InspectionJobSummaryIdPage);
                             InspectionJobSummaryIdPage = parsedata[0].id; 
                             $("#<%=InspectionId.ClientID%>").val(parsedata[0].id);
                             $badcount.val('0'); 
                             $RE.val(parsedata[0].WOQuantity);
                             $('#MainContent_AC').val(parsedata[0].WOQuantity);
                             $('#MainContent_REHidden').val(parsedata[0].WOQuantity);
                             LocalCounts = 0;
                             $("#Specgrid").jqGrid('setGridParam', 
                                { datatype: 'json' }).trigger('reloadGrid');
                         }
                     }
                    
                 },
                error: function (a, b, c) {
                    alert(c);
               
                }
            });
        },
        InspectionArray: new Array(),
        GetInspectionJobSummaryId: function (TargetNumberIn, IsDefect) { 
            $("#jobIdSpinner").toggle();
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput_JobDispatch.ashx',
                type: 'GET',
                data: { method: 'OpenJobIfExists', args: { TargetNumber: TargetNumberIn, TemplateId: SelectedId, InspectionType: LineType, AQLVAL: AQLValue}},
                success: function (data) {
                    
                    if (data != 'NOJOBS' && data.length > 0) { 
                        var InspectionArray = $.parseJSON(data);
                        datahandler.InspectionArray = InspectionArray; 
                         
                        var returnnum = InspectionArray[0].id;
                        $("#<%=InspectionId.ClientID%>").val(InspectionArray[0].id);
                        var woq = InspectionArray[0].WOQuantity;
                        var userconfirm = true;
                        if (IsSPCMachine == false && InspectionJobSummaryIdPage == 0) { 
                            
                            if (InspectionArray[0].AQL_Level.trim() == "100.0" && AQLValue == '100')
                            { 
                                userconfirm = true; 
                                alert("This WorkOrder already has an Inspection open.  Since the AQL is 100 it will be auto loaded.");
                                datahandler.LoadExistingJob(); 

                            } else { 
                                $("#JobStart-confirm-jobid").text(returnnum.toString()); 
                                $("#JobStart-confirm-aql").text(InspectionArray[0].AQL_Level.toString()); 
                                $( "#JobStart-confirm" ).dialog( "open" );
                                //code detached 4.20.17 JJS
                                //userconfirm = confirm("This WorkOrder is already open and has the ID: " + returnnum.toString() + ", AQL: " + InspectionArray[0].AQL_Level.toString() + ".  Click OK to load or cancel to create another InspectionID");
                            }                          
                        } 
                        
                        //else if ( IsSPCMachine == false && InspectionJobSummaryIdPage == 0 && InspectionArray[0].LineType != "IL") { 
                        //    alert("This WorkOrder is already open and has the ID: " + returnnum.toString() + ", AQL: " + InspectionArray[0].AQL_Level.toString() + ".  This Inspection is automatically being loaded.");
                        //    userconfirm = true;
                        //} else { 
                        //    userconfirm = true;
                        //}
                        //code detached 4.20.17 JJS
                        //if (userconfirm == true) { 
                            
                        //    pageBehindInspectionStarted = "true";
                        //    InspectionJobSummaryIdPage = returnnum;
                        //    $("#MainContent_inspectionjobsummaryid_hidden").val(InspectionJobSummaryIdPage);
                        //    InspectionStartedVal = true; 
                        //    var AQLNumber;
                        //    if (InspectionArray[0].AQL_Level != null) { 
                        //        AQLNumber = new Number(InspectionArray[0].AQL_Level); 
                        //    }
                            
                        //    if (AQLNumber != null && AQLNumber == 100) { 
                        //        InspectionArray[0].AQL_Level = '100'; 
                        //    }                             

                        //    $("#AQ_Level").val(InspectionArray[0].AQL_Level);
                        //    $("#AQ_Level").prop('disabled', true);
                        //    $("#Auditor_Name").prop('disabled', true); 
                        //    $("#MainContent__AQLevel").val(InspectionArray[0].AQL_Level);
                        //    AQLValue = InspectionArray[0].AQL_Level;
                        //    $("#WOQuantity").val(InspectionArray[0].WOQuantity);
                        //    $("#Specgrid").jqGrid('setGridParam', 
                        //      { datatype: 'json' }).trigger('reloadGrid');
              
                        //    var mydata = $("#Specgrid").jqGrid('getGridParam','data');
            
                        //    if (mydata.length > 0) { 
                        //        $("#EnterSpec").val("Specs (" + mydata.length + ")");
                        //    }
                        //    if (IsPhoneSize == true) { 
                        //        RenderEngine.ShowActiveInspectionMobile();
                        //    }
                        //    //if (IsDefect == true) { 
                        //    //    datahandler.SubmitDefect(buttonid, buttonvalue, buttonname, returnnum, InspectionId);
                        //    //}
                        //    if (OpenOrderFlag == "false") { 
                        //        datahandler.SetSampleSize();
                        //    }
                        //    datahandler.GetOpenTimers();
                        //    datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage)
                        //    setInterval(
                        //    function () { 
                        //        datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage) }
                        //    , 10000);
                        //    $("#MainContent_inspectionjobsummaryid_hidden").val(returnnum);
                        //    $("#jobIdSpinner").css('display', 'none');
                        //} else { 
                        //    if (data) { 
                        //        datahandler.CreateInspectionJobSummaryId(IsDefect);
                        //    } else { 
                        //        alert("invalid server response.  Refresh network may be slow.")
                        //    }
                        //}
                    
                        
                    } else { 
                        if (OpenOrderFlag == "False" && data) { 
                            datahandler.CreateInspectionJobSummaryId(IsDefect);
                            datahandler.InspectionArray = new Array(); 
                        }
                    }
                   
                 },
                error: function (a, b, c) {
                    alert(c);
                    console.log('failed');
                    $("#jobIdSpinner").css('display', 'none');
                }
            });
        },
        LoadExistingJob: function() {
            var InspectionArray = datahandler.InspectionArray;
             
            if (InspectionArray == null || InspectionArray.Length == 0) 
                alert("Failed loading existing job.  Error loading job info."); 
            
            var returnnum = InspectionArray[0].id;
            pageBehindInspectionStarted = "true";
            InspectionJobSummaryIdPage = returnnum;
            $("#MainContent_inspectionjobsummaryid_hidden").val(InspectionJobSummaryIdPage);
            InspectionStartedVal = true; 
            var AQLNumber;
            if (InspectionArray[0].AQL_Level != null) { 
                AQLNumber = new Number(InspectionArray[0].AQL_Level); 
            }
                            
            if (AQLNumber != null && AQLNumber == 100) { 
                InspectionArray[0].AQL_Level = '100'; 
            }                             

            $("#AQ_Level").val(InspectionArray[0].AQL_Level);
            $("#AQ_Level").prop('disabled', true);
            $("#Auditor_Name").prop('disabled', true); 
            $("#MainContent__AQLevel").val(InspectionArray[0].AQL_Level);
            AQLValue = InspectionArray[0].AQL_Level;
            $("#WOQuantity").val(InspectionArray[0].WOQuantity);
            $("#Specgrid").jqGrid('setGridParam', 
              { datatype: 'json' }).trigger('reloadGrid');
              
            var mydata = $("#Specgrid").jqGrid('getGridParam','data');
            
            if (mydata.length > 0) { 
                $("#EnterSpec").val("Specs (" + mydata.length + ")");
            }
            if (IsPhoneSize == true) { 
                RenderEngine.ShowActiveInspectionMobile();
            }
            //if (IsDefect == true) { 
            //    datahandler.SubmitDefect(buttonid, buttonvalue, buttonname, returnnum, InspectionId);
            //}
            if (OpenOrderFlag == "false") { 
                datahandler.SetSampleSize();
            }
            datahandler.GetOpenTimers();
            datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage)
            setInterval(
            function () { 
                datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage) }
            , 10000);
            $("#MainContent_inspectionjobsummaryid_hidden").val(returnnum);
            $("#jobIdSpinner").css('display', 'none');
            datahandler.InspectionArray = new Array(); 
        },
        CreateInspectionJobSummaryId: function (buttonid, buttonvalue, buttonname,IsDefect) { 

            //$.when(datahandler.GetSelectElements()).done(function (elementarrayval) { 
            //    var Inputvars = JSON.stringify(elementarrayval);
            var JobNumber = ''; 
            var Datanumber = ''; 
            var AuditorName = $('#Auditor_Name').val();
            if (LineType == 'ROLL') { 
                JobNumber = $rollnumber.val(); 
                AuditorName = $("#MainContent_Inspector").val().toString().trim();
            } else { 
                JobNumber = $workorder.val();
            }
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                type: 'GET',
                data: { method: 'CreateJobSummaryId', 
                    args: { jobtype: InspectionTypeState,  AQLStandard: $("#MainContent_aqlstandard").val(), IsDefect: IsDefect, JobNumber: JobNumber, WOQuantity: $LotSize.val(), AQL: AQLValue, Location: $Location.val(), TemplateId: SelectedId, DataNo: $DataNo.val().trim(), CID: selectedCIDnum, Auditor: AuditorName, CasePack: $CPNumber.val(), WorkRoom: $WorkRoom.val(), WeaverNamesString: JSON.stringify(Inspection.Weavers) }},
                success: function (data) {
                    var JobObj = JSON.parse(data);
                    //var returnnum = new Number(data);
                    if (data == "-98") { 
                        $("#jobIdSpinner").css('display', 'none');
                        $("#MainContent_inspectionjobsummaryid_hidden").val(0);
                        $("#<%=InspectionId.ClientID%>").val(0);
                        InspectionJobSummaryIdPage = 0;
                        alert("This Jobnumber is already complete.");
                        return;
                    }
                    if (JobObj.JobSummaryId != -99) { 
                        $("#<%=InspectionId.ClientID%>").val(JobObj.JobSummaryId);
                        pageBehindInspectionStarted = "true";
                        $("#AQ_Level").prop('disabled', true);
                        InspectionJobSummaryIdPage = JobObj.JobSummaryId;
                        $("#MainContent_inspectionjobsummaryid_hidden").val(InspectionJobSummaryIdPage);
                        $("#Auditor_Name").prop('disabled', true); 
                        InspectionStartedVal = true; 
                        if (IsPhoneSize == true) { 
                            RenderEngine.ShowActiveInspectionMobile();
                        }
                        Inspection.WeaverShiftId = JobObj.WeaverShiftId;
                        $("#MainContent_WeaverShiftId_hidden").val(JobObj.WeaverShiftId);

                    }
                    
                    
                                     
                    if (data && JobObj.JobSummaryId !=-99 ) { 
                        
                        datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage)
                        setInterval(
                        function () { 
                            datahandler.UpdateRejectionCount(InspectionState, InspectionJobSummaryIdPage) }
                        , 10000);
                    }
                    $("#jobIdSpinner").css('display', 'none');
                },
                error: function (a, b, c) {
                    alert(c);
                    console.log('failed');
                    $("#jobIdSpinner").css('display', 'none');
                }
            });
            //});
        },
        GetOpenTimers: function() { 

            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                type: 'GET',
                data:{ method: 'GetOpenTimers', args: { InspectionJobSummaryId: InspectionJobSummaryIdPage, SessionId: SessionID}},
                success: function (data) {
                    var otarray = $.parseJSON(data);
    
                    $.each(otarray, function (index, value) { 
                        try {
                            controlhandler.toggleButtonColor($("#start_button_" + value.ButtonLocationId), $("#stop_button_" + value.ButtonLocationId))
                            
                            $("#hiddenTimerId_" + value.ButtonLocationId.toString()).val(value.TimerId);
                            if (value.TimerStart) { 
                                       
                                var startdate = Date.parse(value.TimerStart);

                                $("#start_label_" + value.ButtonLocationId.toString()).text("startDate: " + convertTime(startdate));

                            } else {alert("Timerstart property null")}
                            
                        }
                        catch (err)
                        {
                            console.log(err);
                        }
                    });
                    
                },
                error: function (a,b,c) {

                }
            });
        },
        SetSampleSize: function () {
            var Lotsize = $("#WOQuantity").val();
          
            var AQLStandard = $('#MainContent_aqlstandard').val(); 
 
            
            if (InspectionState == 0 && OpenOrderFlag == "False") { 
 
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'GET',
                    data: { method: 'SetSampleSize', args: { _lotsize: Lotsize, _AQLevel: AQLValue, Standard: AQLStandard } },
                    success: function (data) {
                        var datajson = $.parseJSON(data);
                    
                        if (InspectionTypeState == "WorkOrder") {
                            $("#MainContent_SampleSize").val(datajson[0].SampleSize);
                            $("#MainContent_SampleSizeHidden").val(datajson[0].SampleSize);
                            $("#MainContent_AC").val(datajson[0].AC);
                            $("#MainContent_RE").val(datajson[0].RE);
                            $("#MainContent_REHidden").val(datajson[0].RE);
                            $("#SampleSizeLabel").text("SAMPLE SIZE");
                        } else {
                            $("#SampleSizeLabel").text("DHY");
                        }
                        return true;
                    },
                    error: function (a, b, c) {
                        alert(c);
                        console.log('failed');
                    }
                });
            }

            return true;
        },
        GetWeaverNames: function () { 
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                type: 'POST',
                data: { method: 'GetWeaverNames', args: { LocationId: selectedCID } },
                success: function (data) { 
                    var json = $.parseJSON(data); 
                    pageData.WeaversList = json; 
                    controls.InitWeaversDropDown(pageData.WeaversList)
                }, 
                error: function (a, b , c) { 

                }
            });
        },
        GetAuditorNames: function () {
 
            if (selectedCID.length > 0) { 
                if (selectedCID.length == 3) {selectedCID = '000' + selectedCID;}
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'POST',
                    data: { method: 'GetAuditorNames', args: { LocationId: selectedCID } },
                    success: function (data) {
                        var json = $.parseJSON(data);
 
                        var html = [];
                        var name;
                        for (var i = 0; i < json.length; i++) {
                            name = json[i];
                            if (i == 0) { 

                            } else { 
                                if (name.toString().length > 1) { 
                                    html.push('<option value="' + name + '">' + name + '</option>');
                                }
                            }
                        }
                        var initalval = $('#MainContent_AuditorName').val();
                        if (initalval != "") { html.push('<option value="' + initalval + '">' + initalval + '</option>'); }
                    
                        $("#Auditor_Name").empty();
                        $("#Auditor_Name").html(html.join('')).bind("change dblclick", function () {
                            var selectedval = $(this).val();
                    
                            if (selectedval == "New Name") {
                                $(".Weaver-addname").css('display', 'none');
                                $(".Auditor-addname").css('display', 'block'); 
                                $('#NewAuditorName').wijdialog('open');
                            }
                            $('#MainContent_AuditorName').val(selectedval);
                        });
                        $('#Auditor_Name').val("SELECT OPTION");
                        if (initalval != "New Name" && initalval != "" && initalval != "_") {
                            $('#Auditor_Name').val(initalval);
                            $("#Auditor_Name option[value='SELECT OPTION']").remove();
                            //$("#Auditor_Name option[value='New Name']").remove();
                        } else {
                            var tempstring = Template.GetCookie("AuditorName");
                            
                            if (tempstring != "null" && tempstring.toString().trim().length > 0 && tempstring.trim() != "New Name" && tempstring.trim() != "SELECT OPTION") { 
                                $('#Auditor_Name').val(tempstring);
                                $("#Auditor_Name option[value='SELECT OPTION']").remove();
                                //$("#Auditor_Name option[value='New Name']").remove();
                            }
                        }
                        $('#MainContent_AuditorName').val($('#Auditor_Name option:selected').val());
                        $('#MainContent_AuditorNameHidden').val($('#Auditor_Name option:selected').val());
 
                    },
                    error: function (a, b, c) {
                        alert(c);
                    }
                });
            }
        },
        UpdateRejectionCount: function(InspectionState, InspectionJobSummaryId) { 

            var TargetOrder = "";
            var failurecount = 0;
            
            var InspectionId_Array = $("#MainContent_InspectionId").val().split("."); 
            var InspectionId_sel = InspectionId_Array[1];

            switch (InspectionState) { 
                case 0: 
                    TargetOrder = $("#MainContent_WorkOrder").val(); 
                    break; 
                case 1: 
                    TargetOrder = $("#MainContent_RollNumber").val(); 
                    break; 
                case 2: 
                    TargetOrder = $("#MainContent_ItemNumber").val(); 
                    break; 
            }

            if (TargetOrder != "" && InspectionStartedVal == true) { 
                var InspectNumber = new Number(InspectionId_sel); 
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'POST',
                    data: { method: 'GetRejectionCount', args: { InspectionState: InspectionTypeState, TargetOrder: TargetOrder, InspectionJobSummaryId: InspectionJobSummaryId } },
                    success: function (data) {

                        if (data != -1) { 
                            var $bad = $("#MainContent_Bad_Group");
                            var badnum = new Number($bad.val()); 
                            RemoteCounts = new Number(data); 
                            
                            if (RemoteCounts.toString() != LastRemoteCount.toString()) { 

                                $bad.fadeTo('slow', 0.3, function()
                                {
                                    $(this).css("background-color", "cadetblue");
                                }).fadeTo('slow', 1);
                                setTimeout(
                                    $bad.fadeTo('slow', 0.3, function()
                                    {
                                    $(this).css("background-color", "white");
                                    }).fadeTo('slow', 1), 1)
                                $bad.val(RemoteCounts);
                                LastRemoteCount = RemoteCounts
                            }

                            
                        }
                    },
                    error: function (a, b, c) {
                        failurecount ++;
                    }
                });
            }
        }, 
        GetItemInfo: function () { 

            $("#MS_ProductValue").wijinputnumber({ 
                type: 'numeric', 
                minValue: 0, 
                maxValue: 10000000, 
                decimalPlaces: 2, 
                increment: 25, 
                showSpinner: true,
                value: 0,
                valueChanged : function (e, data) {
                    MS_ProductValue = data.value;
                }
            });
            $("#MS_Upper_Spec_Value").wijinputnumber({ 
                type: 'numeric', 
                minValue: 0, 
                maxValue: 10000000, 
                decimalPlaces: 2, 
                increment: 25, 
                showSpinner: true,
                value: 0,
                valueChanged : function (e, data) {
                    MS_Upper_Spec_Value = data.value;
                }
            });
            $("#MS_Lower_Spec_Value").wijinputnumber({ 
                type: 'numeric', 
                minValue: -10000, 
                maxValue: 0, 
                decimalPlaces: 2, 
                increment: 25, 
                showSpinner: true,
                value: 0,
                valueChanged : function (e, data) {
                    MS_Lower_Spec_Value = data.value;
                }
            });
            $.ajax({
                url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                type: 'GET',
                data: { method: 'GetItemInfo', args: { DataNo: $DataNo.val()}},
                success: function (data) {
                    var infoar = $.parseJSON(data); 
                    if (infoar.length > 0) { 
                        $("#MS_ProductType").val(infoar[0].Type); 
                    }
                },
                error: function (a, b, c) {
                    alert(c);
               
                }
            });
        }, 
        InsertProductSpec: function(Inputarray) { 
 
            if (Inputarray && Inputarray.length ==1) { 
                var ArrayString = JSON.stringify(Inputarray);  
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput.ashx',
                    type: 'GET',
                    data: { method: 'InsertProductSpec', args: { InputAr: ArrayString, SessionId: SessionID}},
                    success: function (data) {
        
                        if (data == "success") { 
                            $("#Specgrid").jqGrid('setGridParam', 
                                    { datatype: 'json' }).trigger('reloadGrid');
                            alert("Product Spec Entry Success"); 

                        } else { 
                            alert("Product Spec entry failed"); 
                        }
                    },
                    error: function (a, b, c) {
                        alert("Product Spec entry failed"); 
               
                    }
                });
            } else { 
                alert("Product Spec entry failed"); 
            }
        }, 
        StartNewWeaverShift: function () { 
            if (Inspection.WeaverShiftId != null && Inspection.WeaverShiftId > 0) { 
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + "/handlers/DataEntry/SPC_InspectionInput_JobDispatch.ashx", 
                    type: 'POST', 
                    data: { method: 'StartNewWeaverShift', args: {shiftId: Inspection.WeaverShiftId, JobSummaryId: InspectionJobSummaryIdPage, WeaverInfo: JSON.stringify(Inspection.PreConfirmWeavers), Yards: $("#weaveryards").val(), CurrentShiftNumber: Inspection.WeaverShiftNumber}}
                }).done(function (data) { 
                    Inspection.WeaverShiftId = new Number(data);  
                    Inspection.WeaverShiftNumber++; 
                    $("#MainContent_WeaverShiftId_hidden").val(Inspection.WeaverShiftId);
                    Inspection.SetWeaversHTML(); 
                    $('#NewAuditorName').wijdialog('close');
                });
            }
        }
    };
    var eventshandler = { 
        UserKeyPress: {
            Init: function () { 
                $(".inputelement").keydown(function (event) { 
                    eventshandler.UserKeyPress.UpdateServerSessionTimeout(event);
                });
            },
            UpdateServerSessionTimeout: function (event) { 
                if (event) { 
                    var parsedElementName = event.currentTarget.id;
                    if (eventshandler.UserKeyPress.Timeout_id > 0) { 
                        clearTimeout(eventshandler.UserKeyPress.Timeout_id);
                    }        
                    eventshandler.UserKeyPress.Timeout_id = setTimeout(function() {   
                        eventshandler.UserKeyPress.UpdateUserInputs(parsedElementName); 
                        eventshandler.UserKeyPress.PostInputsToServer();
                    }, 3000);
                }
            },
            PostInputsToServer: function () { 
              $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput_UserInputs.ashx',
                    type: 'GET',
                    data: { method: 'UpdateLastUserInputs', args: { InputArString: JSON.stringify(eventshandler.UserKeyPress.ExistingUserInputs), SessionId: SessionID}},
                    success: function (data) {
                    },
                    error: function (a, b, c) {
                    }
              });
            },
            UpdateUserInputs: function(propName) { 
                $.each(eventshandler.UserKeyPress.ExistingUserInputs, function (obj, index) {       
                    if (obj == propName) {
                        eventshandler.UserKeyPress.ExistingUserInputs[obj] = $("#" + propName).val();
                    } else { 
                        eventshandler.UserKeyPress.ExistingUserInputs[obj] = $("#" + obj).val();
                    }
                });
            },
            SetUserInputs: function() { 
                $.each(eventshandler.UserKeyPress.ExistingUserInputs, function (obj, index) {       
                    $("#" + obj).val(eventshandler.UserKeyPress.ExistingUserInputs[obj]);       
                });
            },
            GetLastUserInputs: function() { 
                
                $.ajax({
                    url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput_UserInputs.ashx',
                    type: 'GET',
                    data: { method: 'getLastUserInputs', args: { SessionId: SessionID}},
                    success: function (data) {
                        
                        if (data.length > 0) { 
                            var InputObject = $.parseJSON(data);
                            eventshandler.UserKeyPress.ExistingUserInputs = InputObject;
                            eventshandler.UserKeyPress.SetUserInputs();
                          
                        }
                    },
                    error: function (a, b, c) {
                    }
              });
            },
            ExistingUserInputs: {MainContent_WorkOrder: "", MainContent_workroom: "", MainContent_CPNumber: "", MainContent_DataNumber: "", MainContent_AuditorName: "", MainContent_Location: "", WOQuantity: "", MainContent_RollNumber: "", MainContent_LoomNumber: "", MainContent_Inspector: ""},
            Timeout_id: 0

        }, 
        InitPageEventHandlers: function() { 

                $( "#MainContent_DataNumber" ).change(function() {
                    if ($DataNo.val().length > 2) { 
                        var mydata = $("#Specgrid").jqGrid('getGridParam','data');
                        if (mydata.length > 0) { 
                            $("#EnterSpec").val("Specs (" + mydata.length + ")");
                        }
                    }
                });

                $("#MainContent_SpecAdd").click(function (e) {
                    var rowdata = $('#Specgrid').jqGrid('getLocalRow', SpecGridEditId);

                    jQuery("#Specgrid").jqGrid('saveRow', SpecGridEditId, null, "<%=Session("BaseUri")%>" + '/handlers/DataEntry/SPC_InspectionInput_SpecSave.ashx', { DefectId: DefectID, InspectionId: InspectionId, TemplateId: SelectedId, TabName: SelectedTab, TabNumber: UserSelectedTabNumber, SpecId: rowdata.SpecId }, null, null);
                });
                $("#GlobalSpecsImage").click(function (e) {
                    window.open("http://m.standardtextile.com/PDFSearch/Search.aspx"); 
                });

                 $("#MobileDirector").click(function (e) { 
                    if (selectedCIDnum) { 
                        if (selectedCIDnum > 0) { 
                            var JobNumber = ""
                            if (InspectionState == 0) { 
                                JobNumber = "JobNumber=" + $workorder.val();
                            } else { 
                                JobNumber = "JobNumber=" + $rollnumber.val();
                            }
                            window.location.assign("<%=Session("BaseUri")%>" + "/Mobile/DataEntry/DefectImageEntry.aspx?DefectID=" + DefectID.toString() + "&InspectionID=" + InspectionJobSummaryIdPage.toString() + "&CID=" + selectedCIDnum.toString() + "&InspectionStarted=" + InspectionStartedVal.toString() + "&" +JobNumber);
                        }
                    }
                });
                $("#NewPage").click(function (e) { 
            
                    if (SelectedId && SelectedId.toString().length > 0) { 
                        var r = confirm("This will clear out the current Inspection.  Are you Sure?");
                        if (r == true) { 
                            window.location.assign("<%=Session("BaseUri")%>" + "/APP/Mob/SPCInspectionInput.aspx?TemplateId=" + SelectedId.toString());
                        }
                    } else { 
                        alert("Template Not Selected"); 
                    }
            
                });
        
                $("#EnterProductSpec").click(function (e) { 
            
                    if (InspectionStartedVal == true && $DataNo.val().length > 2) { 
                        $("#ProductSpecEntrydialog").wijdialog("open");
                    } else { 
                        alert("Inspection Not Started and/or DataNo Not Set"); 
                    }
            
                });

                $("#EnterSpec").click(function (e) { 
                    if (InspectionStartedVal == true) { 
                        $("#WorkOrderSelection").css("display","none");   
                        $("#LocationSelection").css("display","none");
                        $("#JobConfirmation").css("display","none");
                        $("#RollConfirmation").css("display","none");
                        $("#loginfrm").css("display","none");
                        $("#SpecTable").fadeIn();
                        hiddenSection.fadeIn()
                            .css({ 'display':'block' })
                            // set to full screen
                            .css({ width: $(window).width() + 'px', height: '100%' })
                            .css({ top:($(window).height() - hiddenSection.height())/2 + 'px',
                                left:($(window).width() - hiddenSection.width())/2 + 'px' })
                            // greyed out background
                            .css({ 'background-color': 'rgba(0,0,0,0.5)' });
                    } else { 
                        alert('Please START an Inspection or Load an Open WorkOrder'); 
                    }
                    controlhandler.RenderProductSpecTable(); 
                });
                $("#NextItem").click(function (e) { 
                    var rowclass = $('#2');

                    rowclass.css('background','#FDF9F9 url(images/ui-bg_flat_75_ffffff_40x100.png) 50% 50% repeat-x !important;');
                    $("#Specgrid").jqGrid('setGridParam', 
                                 { datatype: 'json' }).trigger('reloadGrid');
                    SpecItemCounter++;
                    $("#ItemNumberLabel").text("Item #: " + SpecItemCounter.toString());
                });

                $("#ImageSubmit").click(function (e) { 
                    e.preventDefault();
           
                    $("#myImageForm").ajaxSubmit({ url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/DefectImage_handler.ashx?DefectID=' + DefectID.toString(), type: 'post', success: function (data) { alert(data); } })
                   // $("#myImageForm").ajaxSubmit({url: "<%=Session("BaseUri")%>" + '/handlers/DataEntry/DefectImage_handler.ashx', type: 'post' })
          
                });

                $(".closebox").click(function (e) {

                    if (e.currentTarget.id == 'closeout2'){ 
                        $('#MainContent_Good').val(0); 
                    }
                    $("#loginfrm").fadeOut();
                    $("#LocationSelection").fadeOut();
                    $("#JobConfirmation").fadeOut();
                    $("#MachineSelection").fadeOut();
                    $("#SpecTable").fadeOut();
                    $("#RollConfirmation").fadeOut();
                    hiddenSection.fadeOut()
                });

                $( "#JobStart-confirm" ).dialog({
                    resizable: false,
                    autoOpen: false,
                    height: "auto",
                    width: 500,
                    modal: true,
                    buttons: {
                        "CONTINUE": function() {
                            datahandler.LoadExistingJob(); 
                            $( this ).dialog( "close" );
                        },
                        "NEW AQL": function() {
                            if (OpenOrderFlag == "False") { 
                                datahandler.CreateInspectionJobSummaryId(false);                      
                            } else { 
                                $("#jobIdSpinner").css('display', 'none');
                            }
                            datahandler.InspectionArray = new Array(); 
                            $( this ).dialog( "close" );
                        }
                    }, 
                    close: function(event, ui) { 
                        $("#JobStart-confirm-jobid").text("NA"); 
                        $("#JobStart-confirm-aql").text("1"); 
                    }
                });
            }
    };
    function setnavcop(setvalue) { 
        NavCop = setvalue;
    }
    var clicks = 0; 
    function SubmitOnce()
    {
        clicks++;
   
        if (clicks ==1) {
            return true;
        } else { 
            $("#MainContent_Confirm").prop('disabled', true);
        }
    }
    function convertTime(Date_) {
        var d = new Date(Date_); 
        d.setHours( d.getHours() + 2 ); // offset from local time
        var h = (d.getHours() % 12) || 12; // show midnight & noon as 12
        return (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear() + " " + (
            ( h < 10 ? '0' : '') + h +
            ( d.getMinutes() < 10 ? ':0' : ':') + d.getMinutes() +
                    // optional seconds display
            // ( d.getSeconds() < 10 ? ':0' : ':') + d.getSeconds() + 
            //(Date.getMonth() + 1) + "/" + Date.getDate() + "/" + Date.getFullYear() + " " + 
            ( d.getHours() < 12 ? ' AM' : ' PM' )
        );
	
    }
    function togglevalidation(divname) { 
        $("#" + divname).toggle(function () {
            $("#" + divname).css({ 'background-color': 'red', 'display': 'inline' });
        });
        var timeo = setTimeout(function () {
            $("#" + divname).toggle(function () {
                $("#" + divname).css({ 'background-color': 'RGB(93, 135, 161)', 'display': 'inline' });
            });
        }, 1200);

    }
</script>



</asp:Content>

