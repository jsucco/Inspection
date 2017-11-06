Imports System.Data
Imports System.Web.Script.Serialization

Namespace core

    Partial Class APP_DataEntry_SPCInspectionInput
        Inherits core.APRWebApp
        Public LocationNames As String
        Public MachineNames As String
        Public LastLocation As String
        Public TemplateCollection As String
        Public ProductSpecCollection As String = "[0]"
        Public TemplateNames As String
        Public SelectedId As Integer = -1
        Public SelectedName As String
        Public IsWorkOrderError As Boolean
        Public IsSPCMacine As String = "false"
        Public SPCMachineName As String
        Public WorkOrderError As String
        Public InspectionUser As String
        Public Selected_Inspectionid As Integer
        Public IsMobile As String = "false"
        Public WOQuantityValue As Integer = 0
        Public HasTargetCount As Boolean = False
        Public CountToTarget As Integer = 0
        Public CurrentCount As Integer = 0
        Public OpenOrderLoadFlag As String = "False"
        Public InspectionStartedFlag As String = "false"
        Public InspectionConfirmFlag As String = "false"
        Public InspectionConfirmMessage As String = ""
        Public TemplateTabCount As Integer = 0
        Public OpenRollInfoString As String = "0"
        'Public CorpSelList As String = "[0]"
        Public CID As String
        Public CIDnum As Integer
        Public HasCID As Boolean = False
        Public LocationName As String
        Public LineType As String = "EOL"
        Public ColumnCount As Integer = 2
        Public EmailJobSummId As Integer = 0
        Public SessionId As String
        Public WorkRoomArr As String
        Public IsNew As Boolean = False
        Public Property cypherclass As New cypher

        Private Property util As New Utilities
        Private Property Inspect As New InspectionUtilityDAO
        Private Property InspectInput As New InspectionInputDAO
        Private Property jser As New JavaScriptSerializer
        Private Property DA As New InspectionInputDAO
        Private Property as400 As New AS400DAO
        Public AQL As String = "2.5"
        Public Dimensions As String = ",,,,,,,"
        Private Acceptance As Integer
        Private Rejecter As Integer
        Private corp As New core.corporate
        Private IsPageRefresh As Boolean = False
        Dim TemplateCollectionCache As String
        Private IstestMode As Boolean = False
        Private CurrentTemplateNames As New List(Of selector2array)
        Private IU As New InspectionUtilityDAO
        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

            Dim QueryUsername As String = Request.QueryString("Username")
            Dim GoodCount As Object = Session("Good")
            Dim QueryWorkOrder As String = Request.QueryString("WorkOrder")
            Dim QueryRollNumber As String = Request.QueryString("RollNumber")
            Dim QueryCartonNumber As String = Request.QueryString("CartonNumber")
            Dim Query400Req As String = Request.QueryString("400REQ")
            Dim QueryAQL As String = Request.QueryString("AQL")
            Dim QueryConfirm As String = Request.QueryString("CONFIRM")

            If IsNothing(QueryConfirm) = False Then
                InspectionConfirmFlag = "true"
                InspectionConfirmMessage = QueryConfirm
                Dim qryVars = New NameValueCollection(Request.QueryString)
                qryVars.Remove("CONFIRM")
            End If

            SessionId = Session.SessionID

            If Page.IsPostBack = False Then
                If util.DetectDeviceType(Request.UserAgent) = True Then
                    FileUploadControl.Visible = False
                    IsMobile = "true"
                End If

                Good.Value = "0"
                Bad_Local.Value = "0"
                RE.Value = "0"
                AC.Value = "0"
                SampleSize.Value = "0"
            End If

            If setInspectionLocation() Then
                CurrentTemplateNames = Inspect.GetTemplateListByLocation_2(CIDnum)
                TemplateNames = jser.Serialize(CurrentTemplateNames)
                LoadMachineNames()
            Else
                CurrentTemplateNames = Inspect.GetTemplateList()
                TemplateNames = jser.Serialize(CurrentTemplateNames)
            End If

            Dim QueryTemplateid As String = Request.QueryString("TemplateId")

            If IsNothing(QueryTemplateid) = False Then
                InitializeTemplate(QueryTemplateid)
            Else
                initializeTemplateFromCookie()
            End If

            initializeInspectionLocations()
            initializeWorkrooms(CID)
            If TemplateNames Is Nothing Or LocationNames Is Nothing Then
                Response.Redirect("~/ErrorPage.aspx")
            End If
            If IsNothing(Query400Req) = False Then
                If IsNothing(Request.QueryString("OpenWO")) = False Then
                    OpenOrderLoadFlag = "True"
                End If
                Dim standard As String = Request.QueryString("standard")
                Select Case Query400Req
                    Case "SPCMachine"
                        Dim Querymachine As String = Request.QueryString("SPCMachine")
                        If Querymachine.ToString.Trim.Length > 0 Then
                            Dim listso As New List(Of SingleObject)
                            Dim bmapso As New BMappers(Of SingleObject)
                            listso = bmapso.GetSpcObject("SELECT TOP(1) WorkOrder FROM LiveProduction WHERE Machine = '" & Querymachine.Trim() & "'")
                            If listso.Count > 0 Then
                                If SetWorkOrderInfo(listso.ToArray()(0).Object1.ToString().Trim, 100, standard) = False Then
                                    IsWorkOrderError = True
                                Else
                                    IsSPCMacine = "true"
                                    SPCMachineName = Querymachine.Trim()
                                    InspectionStartedFlag = "True"
                                End If
                            End If
                        End If
                    Case "WORKORDER"
                        If IsNothing(QueryAQL) = False Then
                            If SetWorkOrderInfo(QueryWorkOrder.Trim(), CType(QueryAQL, Decimal), standard) = False Then
                                IsWorkOrderError = True
                            End If
                            AQL = QueryAQL
                        End If
                    Case "ROLLNUMBER"
                        SetLastUserInputs()
                        SetRollNumberInfo(QueryRollNumber.Trim())
                        RollNumber.Value = QueryRollNumber.Trim()

                    Case "CARTON"
                        If IsNothing(QueryAQL) = False Then
                            SetCartonNumberInfo(QueryCartonNumber, CType(QueryAQL, Decimal))
                            InspectionStartedFlag = "true"
                            AQL = QueryAQL
                        End If
                    Case "ROLL"
                        InspectionStartedFlag = "true"
                    Case "OPENWO"
                        Dim queryijs As String = Request.QueryString("IJS")
                        If IsNumeric(queryijs) = True Then
                            Dim bmapso As New BMappers(Of SingleObject)
                            Dim listso As New List(Of SingleObject)
                            Selected_Inspectionid = queryijs
                            listso = bmapso.GetInspectObject("SELECT id AS Object1, ProdMachinename AS Object3 FROM InspectionJobSummary WHERE id = " & queryijs & " AND ProdMachineName is not null")
                            If listso.Count > 0 Then
                                IsSPCMacine = "true"
                                SPCMachineName = listso.ToArray()(0).Object3.ToString().Trim()
                            End If

                            SetOpenWOInfo(CType(queryijs, Integer))
                        End If
                    Case "OPENRL"
                        Dim queryijs As String = Request.QueryString("IJS")
                        If IsNumeric(queryijs) = True Then
                            Selected_Inspectionid = queryijs
                            SetOpenRollinfo(CType(queryijs, Integer))
                        End If
                    Case Else
                        SetLastUserInputs()
                End Select
            Else
                Try
                    Dim aqlobj As Object = GetCookie2("LastAQLCompleted", "value")
                    Dim aqltemp As Decimal = 2.5


                    If IsNothing(aqlobj) = False And aqlobj.count > 0 Then
                        If Decimal.TryParse(aqlobj.Item("LastAQLCompleted"), aqltemp) Then
                            Select Case aqltemp
                                Case 1
                                    AQL = "1"
                                Case 1.5
                                    AQL = "1.5"
                                Case 2.5
                                    AQL = "2.5"
                                Case 4
                                    AQL = "4"
                                Case 100
                                    AQL = "100"
                                Case Else
                                    AQL = "2.5"
                            End Select
                        End If
                    End If
                    If Request.QueryString("NewInspection") Is Nothing Then
                        SetLastUserInputs()
                    Else
                        IsNew = True
                    End If
                Catch ex As Exception

                End Try
            End If
            If Not QueryUsername Is Nothing Then
                If QueryUsername.Trim.Length > 0 And QueryUsername <> "New Name" And QueryUsername <> "SELECT OPTION" Then
                    RegisterUserCookie(QueryUsername, "InspectionUser")
                End If
            End If



        End Sub
        Private Function getLastUserInputs(SessionId As String) As core.SPCInspection.UserInputs
            Dim UserData As New core.SPCInspection.UserInputs
            Dim lastInputs = HttpRuntime.Cache("InspectionInput_LastEnteredInputs_" + SessionId)
            If Not lastInputs Is Nothing Then
                UserData = lastInputs
            End If
            Return UserData
        End Function
        Private Sub SetLastUserInputs()
            Dim UserData = getLastUserInputs(SessionId)
            If Not UserData Is Nothing Then
                WorkOrder.Value = UserData.MainContent_WorkOrder
                DataNumber.Value = UserData.MainContent_DataNumber
                workroom.Value = UserData.MainContent_workroom
                AuditorName.Value = UserData.MainContent_AuditorName
                'CPNumber.Value = UserData.
                WOQuantityValue = UserData.WOQuantity
                RollNumber.Value = UserData.MainContent_RollNumber
                LoomNumber.Value = UserData.MainContent_LoomNumber
                Inspector.Value = UserData.MainContent_Inspector
            End If
        End Sub
        Private Function setInspectionLocation() As Boolean
            'Dim CIDCookie = GetAPRKeepMeIn()
            Dim CIDCookie = GetCookie("APRKeepMeIn", "CID_Print")

            If IstestMode = True Then
                SetLocationForTest()
                HasCID = True
            Else
                If CIDCookie.ContainsKey("APRKeepMeIn") Then
                    CID = CIDCookie("APRKeepMeIn")
                    CIDnum = CType(CID, Integer)
                    HasCID = True
                    Return HasCID
                End If
233:
                Dim cidqs As String = Request.QueryString("CID_Info")
                If IsNothing(cidqs) = False Then
                    If cidqs.Length >= 3 Then
                        CID = cidqs
                        CIDnum = Convert.ToInt32(cidqs)
                        HasCID = True
                        SetCookie("APRKeepMeIn", "CID_Print", CIDnum)
                    End If
                End If
            End If
            Return HasCID
        End Function
        Private Sub SetLocationForTest()
            CID = "000485"
            CIDnum = CType(CID, Integer)
            HasCID = True
        End Sub

        Private Function initializeTemplateFromCookie() As Boolean
            Dim initStatus As Boolean = False
            Dim inspectionCookie As HttpCookie = Request.Cookies("SPCInspectionInput")
            If hasPageInspectionCookie(inspectionCookie) Then
                Dim TemplateIdCookie As String = Server.HtmlEncode(inspectionCookie("TemplateId_LastSelected")).ToString()
                If CurrentTemplateNames.Count > 0 Then
                    Dim tempcnt = (From v In CurrentTemplateNames Where v.id = CType(TemplateIdCookie, Integer) Select v).Count()
                    If tempcnt > 0 Then
                        initStatus = InitializeTemplate(TemplateIdCookie)
                    End If
                End If
            End If

        End Function

        Private Function InitializeTemplate(ByVal Template As String) As Boolean
            Dim initStatus As Boolean = False
            Try
                RegisterInspectionObjects(Template)
                CheckLineObject(Template)
                SelectedId = CType(Template, Integer)
                initStatus = True
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return initStatus
        End Function
        Private Function hasPageInspectionCookie(inspectionCookie As HttpCookie) As Boolean
            Dim hasCookie As Boolean = False
            If Not Request Is Nothing Then

                'Inspection Cookie object is overridden by any templateId specified in the query string
                If Not inspectionCookie Is Nothing And IsNothing(Request.QueryString("TemplateId")) = False Then
                    hasCookie = IsNothing(inspectionCookie("TemplateId_LastSelected"))
                Else
                    hasCookie = False
                End If
            End If

            Return hasCookie
        End Function
        Private Sub CheckLineObject(ByVal TemplateId As String)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As List(Of SingleObject)
            Dim compare As String

            If Not TemplateId = -1 And Not TemplateId = 0 Then
                listso = bmapso.GetInspectObject("SELECT LineType as Object1, ColumnCount as Object2 FROM TemplateName WHERE TemplateId =" & TemplateId)

                If listso.Count > 0 Then
                    compare = listso.ToArray()(0).Object1.ToString().Trim.ToUpper()
                    ColumnCount = listso.ToArray()(0).Object2
                Else
                    Throw New Exception("Failed To Retieve Template Type from Server")
                End If
                If compare.Length > 0 Then
                    LineType = compare
                End If
            End If

        End Sub
        Private Sub RegisterInspectionObjects(ByVal TemplateIdString As String)
            If IsNumeric(TemplateIdString) = True Then
                SelectedId = Convert.ToInt32(TemplateIdString)
                If SelectedId > 0 Then
                    Dim selectValues As New List(Of SPCInspection.TemplateCollection)()
                    Dim SpecList As New List(Of SPCInspection.ProductSpecCollection)()

                    HasTargetCount = True

                    TemplateCollectionCache = Nothing

                    If TemplateCollectionCache Is Nothing Or TemplateCollectionCache = "[]" Then
                        selectValues = Inspect.GetInputTemplateCollection(SelectedId)
                        If Not selectValues Is Nothing Then
                            TemplateCollectionCache = jser.Serialize(selectValues)
                            Cache.Insert(SelectedId.ToString() + "TemplateCollection", TemplateCollectionCache, Nothing, Now.AddDays(2), System.Web.Caching.Cache.NoSlidingExpiration)
                            Me.Session("TemplateCollCacheString") = SelectedId.ToString()
                        End If
                    Else
                        Me.Session("TemplateCollCacheString") = SelectedId.ToString()
                    End If
                    If Not selectValues Is Nothing Then
                        If selectValues.Count > 0 Then
                            Dim tabcount = (From v In selectValues Order By v.TabNumber Descending Select v.TabNumber).ToList()
                            TemplateTabCount = tabcount.ToArray()(0) + 1
                        End If
                    End If
                    TemplateCollection = TemplateCollectionCache

                End If

                Response.Cookies("SPCInspectionInput")("TemplateId_LastSelected") = SelectedId.ToString()
                Response.Cookies("SPCInspectionInput")("lastVisit") = DateTime.Now.ToString()
                Response.Cookies("SPCInspectionInput").Expires = DateTime.Now.AddDays(60)
            End If
        End Sub
        Protected Sub COnfirm2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Confirm.Click, ConfirmRoll.Click
            Dim returnmessage As String = ""
            Dim ErrorFlag As Boolean = False
            Dim cstext2 As String = ""
            Dim curijs As New InspectionJobSummary
            Dim ijsnum As Integer = 0
            Dim DHY As Decimal = 0
            Dim JobPassFail As String = "Pass"
            Dim ErrorMessage As String = ""

            Try
                ijsnum = CType(inspectionjobsummaryid_hidden.Value, Integer)
            Catch ex As Exception

                Response.Redirect("~/APP/Mob/SPCInspectionInput.aspx?TemplateId=" + SelectedId.ToString() + "&CONFIRM=" + returnmessage)
                Exit Sub
            End Try
            Try
                Using _db As New Inspection_Entities
                    curijs = (From v In _db.InspectionJobSummaries Where v.id = ijsnum).FirstOrDefault()

                    If IsNothing(curijs) = False Then
                        Select Case InspectionState.Value
                            Case "WorkOrder"

                                curijs.ItemFailCount = GetFailCount(ijsnum.ToString(), InspectionState.Value.Trim())
                                curijs.TotalInspectedItems = util.ConvertType(totalinspecteditems.Value, "Integer") ' CType(totalinspecteditems.Value, Integer)

                                If curijs.ItemFailCount >= curijs.RejectLimiter Then
                                    curijs.Technical_PassFail = False
                                    JobPassFail = "Fail"
                                Else
                                    curijs.Technical_PassFail = True
                                    JobPassFail = "Pass"
                                End If
                                curijs.ItemPassCount = curijs.TotalInspectedItems - curijs.ItemFailCount

                            Case "RollNumber"

                                DHY = util.ConvertType(DHYHidden.Value, "Decimal")
                                curijs.ItemFailCount = GetFailCount(ijsnum.ToString(), InspectionState.Value.Trim())
                                curijs.ItemPassCount = -1
                                If DHY > 10 Then
                                    curijs.Technical_PassFail = False
                                    JobPassFail = "Fail"
                                Else
                                    curijs.Technical_PassFail = True
                                    JobPassFail = "Pass"
                                End If
                                curijs.TotalInspectedItems = CType(totalinspectedyards.Value, Integer)

                        End Select
                        curijs.MajorsCount = InspectInput.GetDefectCountByType(ijsnum.ToString(), "MAJOR")
                        curijs.MinorsCount = InspectInput.GetDefectCountByType(ijsnum.ToString(), "MINOR")
                        curijs.RepairsCount = InspectInput.GetDefectCountByType(ijsnum.ToString(), "REPAIRS")
                        curijs.ScrapCount = InspectInput.GetDefectCountByType(ijsnum.ToString(), "SCRAP")
                        curijs.UserConfirm_PassFail = True
                        curijs.UserConfirm_PassFail_Timestamp = Date.Now
                        curijs.Inspection_Finished = Date.Now
                        curijs.JobType = InspectionState.Value
                        curijs.Comments = JobMessage.Value

                        Dim rowsaff As Integer = IU.setISRow(ijsnum, curijs.ItemFailCount, curijs.TotalInspectedItems, curijs.Technical_PassFail, curijs.ItemPassCount, curijs.MajorsCount, curijs.MinorsCount, curijs.RepairsCount, curijs.ScrapCount, curijs.UserConfirm_PassFail, curijs.UserConfirm_PassFail_Timestamp, curijs.Inspection_Finished, curijs.JobType, curijs.Comments)

                        If rowsaff > 0 Then
                            SetCookieExp("LastAQLCompleted", "value", curijs.AQL_Level, 1)
                            If AddEmailFlag.Checked = True Or AddEmailFlagRoll.Checked = True Then
                                SendEmailAlertsAsync(curijs, ijsnum, JobPassFail, DHY)
                            End If
                            returnmessage = "JOBNUMBER." + curijs.JobNumber + ".CONFIRMATION-SUCCESS." + JobPassFail + "ed"

                            WorkOrder.Value = ""
                            Good.Value = "0"
                            Bad_Local.Value = "0"
                            Bad_Group.Value = "0"
                            RE.Value = "0"
                            AC.Value = "0"
                            DHU.Value = "0"
                            SampleSize.Value = "0"
                            DataNumber.Value = ""
                            CartonNumber.Value = ""
                            'CPNumber.Value = ""
                            RollNumber.Value = ""
                            LoomNumber.Value = ""
                            woquantity_hidden.Value = "0"
                            ItemNumber.Value = ""
                            OpenOrderLoadFlag = "False"

                        Else
                            returnmessage = "JOB NOT COMPLETE-FAILED.Framework Error 0 Rows Affected."
                            ErrorFlag = True
                        End If

                    Else
                        returnmessage = "JOB NOT COMPLETE-Failed to retrieve Inspection Information"
                        ErrorFlag = True
                    End If
                End Using
            Catch ex As Exception
                returnmessage = "JOB NOT COMPLETE-" + ex.Message
                ErrorFlag = True
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try

            Response.Clear()
            Response.Redirect("~/APP/Mob/SPCInspectionInput.aspx?TemplateId=-1")

        End Sub

        Private Sub SendEmailAlertsAsync(ByVal curijs As InspectionJobSummary, ByVal ijsnum As Integer, ByVal JobPassFail As String, Optional ByVal DHY As Decimal = 0)
            'sendEmailAlerts(curijs, ijsnum, JobPassFail, DHY)
            Dim t As System.Threading.Tasks.Task = System.Threading.Tasks.Task.Run(Sub()
                                                                                       sendEmailAlerts(curijs, ijsnum, JobPassFail, DHY)
                                                                                   End Sub)
        End Sub

        Private Sub sendEmailAlerts(ByVal curijs As InspectionJobSummary, ByVal ijsnum As Integer, ByVal JobPassFail As String, Optional ByVal DHY As Decimal = 0)

            Dim listso As New List(Of SingleObject)
            Dim emaillist As New List(Of Emails)
            Dim listem As New List(Of Emails)
            Dim listemex As New List(Of Emails)
            Dim bmapem As New BMappers(Of Emails)

            'emaillist = bmapem.GetAprMangObject("SELECT Address, INS_ALERT_EMAIL, ADMIN, HomeLocation FROM EmailMaster")
            emaillist.Add(New Emails With {.Address = "jsucco@standardtextile.com", .INS_ALERT_EMAIL = 1, .HomeLocation = "ALL"})
            emaillist.Add(New Emails With {.Address = "John.Succo@gmail.com", .INS_ALERT_EMAIL = 1, .HomeLocation = "ALL"})
            emaillist.Add(New Emails With {.Address = "kbredwell@standardtextile.com", .INS_ALERT_EMAIL = 1, .HomeLocation = "ALL"})
            emaillist.Add(New Emails With {.Address = "jaho@standardtextile.com", .INS_ALERT_EMAIL = 1, .HomeLocation = "ALL"})
            Try
                If IsNothing(curijs) = True Then
                    Return
                End If

                If IsNothing(emaillist) = True Then
                    Return
                End If
                If emaillist.Count > 0 Then
                    Dim bmapso As New BMappers(Of SingleObject)
                    listso = bmapso.GetAprMangObject("SELECT Name AS Object1 FROM LocationMaster WHERE (CID = '000" & LastLocation & "')")
                    Dim locationString = ""
                    listso.Clear()
                    If listso.Count > 0 Then
                        locationString = "AT " + listso.ToArray()(0).Object1
                        listem = (From v In emaillist Where v.HomeLocation.Trim() = listso.ToArray()(0).Object1.ToString().Trim() Or v.HomeLocation.ToString().Trim() = "ALL").ToList()
                    Else
                        listem = (From v In emaillist Where v.HomeLocation.Trim() = "ALL").ToList()
                    End If
                    Dim DHUCalc As Decimal = InspectInput.CalculateDHU(InspectionState.Value, curijs.JobNumber, ijsnum, curijs.TotalInspectedItems)
                    Dim body As String
                    If InspectionState.Value = "WorkOrder" Then
                        body = "[" + curijs.JobType + ": " & curijs.JobNumber & "][WOQuantity: " & curijs.WOQuantity.ToString() & "][AQL Level: " & curijs.AQL_Level.ToString() & "][TemplateName: " & Templatename.Value.ToString() & "][PassCount: " & curijs.ItemPassCount.ToString() & "][FailCount: " & curijs.ItemFailCount.ToString() & "][SampleSize: " & curijs.SampleSize.ToString() & "][RejectLimitor: " & curijs.RejectLimiter & "][DHU: " & DHUCalc.ToString("F2") & " %][Auditor Name: " & AuditorNameHidden.Value.ToString() & "]<br /><br /> Comments: " + curijs.Comments
                    Else
                        DHY = CType(DHYHidden.Value, Decimal)
                        body = "[" + curijs.JobType + ": " & curijs.JobNumber & "][Yards: " & curijs.WOQuantity.ToString() & "][TemplateName: " & Templatename.Value.ToString() & "][FailCount: " & curijs.ItemFailCount.ToString() & "][DHY: " & DHY.ToString("F2") & " %][Auditor Name: " & AuditorNameHidden.Value.ToString() & "]<br /><br /> Comments: " + curijs.Comments
                    End If

                    Dim subject As String = "ALERT JOB " + JobPassFail + "ed " + locationString

                    If listem.Count > 0 Then
                        util.SendMail(subject, body, listem)
                    End If
                    EmailJobSummId = ijsnum
                    If DHUCalc > 50 And InspectionState.Value = "WorkOrder" Then
                        listemex = (From v In emaillist Where v.HomeLocation.ToString().Trim() = "EX" Or v.HomeLocation.ToString().Trim() = "ALL").ToList()
                        If listemex.Count > 0 Then
                            util.SendMail(subject, body, listemex)
                        End If
                    End If
                End If


            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End Sub

        Private Function GetFailCount(ByVal JobSummaryId As String, ByVal JobType As String) As Integer
            Dim listso1 As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim returnint As Integer = 0

            Dim countstring As String = "SELECT COUNT(DefectID) AS Object1 FROM DefectMaster INNER JOIN ButtonTemplate ON DefectMaster.ButtonTemplateId = ButtonTemplate.id WHERE (DefectMaster.InspectionJobSummaryId = " & JobSummaryId & ")  AND (ButtonTemplate.DefectType <> '0')"
            listso1 = bmapso.GetInspectObject(countstring)
            If listso1.Count > 0 Then
                returnint = CType(listso1.ToArray()(0).Object1, Integer)
            Else
                returnint = CType(Bad_Group.Value, Integer)
            End If

            Return returnint

        End Function

        Private Function GetCurrentInspectionObject(ByVal ijsid As String) As InspectionJobSummary

            Dim newobj As New InspectionJobSummary
            Dim ijsidnum As Integer

            Try
                ijsidnum = CType(ijsid, Integer)

                Using _db As New Inspection_Entities
                    newobj = (From v In _db.InspectionJobSummaries Where v.id = ijsidnum).FirstOrDefault()
                End Using
            Catch ex As Exception
                ijsidnum = 0
            End Try


            Return newobj

        End Function

        Protected Sub UploadButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles UploadButton.Click
            Dim uri As String = Me.Session("BaseUri")
            Dim UserParse As Object = Request.UserAgent

            If util.DetectDeviceType(UserParse) = True And DefectID_Value.Value <> "0" Then
                Dim DefectID As String = Convert.ToString(DefectID_Value.Value)
                Dim query_string As String = "DefectID=" + DefectID
                Dim hash As String = cypherclass.HashQueryString(query_string)
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "window.open('../../Mobile/DataEntry/DefectImageEntry.aspx?DefectID=" & DefectID & "&qtpval=" & hash & "', '_newtab');", True)
            Else
                If FileUploadControl.HasFile = True And DefectID_Value.Value <> "0" Then
                    Dim file As HttpPostedFile
                    Dim DefectID As Integer = Convert.ToInt32(DefectID_Value.Value)
                    file = FileUploadControl.PostedFile
                    If InspectInput.InsertDefectImageById(file, DefectID) Then
                        StatusResult.InnerText = "Success"
                    Else
                        StatusResult.InnerText = "Failure"
                    End If

                End If
            End If

            Me.Session("InspectionInput_Bad") = Bad_Local.Value

        End Sub

        Private Sub SetCartonNumberInfo(ByVal CartonNumber_input As String, ByVal aql As Decimal)

            Dim CartonNumberArray = as400.GetInputCartonData(CartonNumber_input).ToArray()
            If CartonNumberArray.Length > 0 Then
                DataNumber.Value = CartonNumberArray(0).DATAN
                'CPNumber.Value = CartonNumberArray(0).CASEPACK
                WOQuantityValue = CartonNumberArray(0).WOQUANTITY
                WorkOrder.Value = CartonNumberArray(0).WONUM
                CartonNumber.Value = CartonNumber_input
                Location.Value = CartonNumberArray(0).LOCATION
                SetSamplesSize(CartonNumberArray(0).WOQUANTITY.ToString(), aql)
            End If

        End Sub
        Private Sub SetOpenRollinfo(ByVal ijsid As Integer)
            Dim bmapijs As New BMappers(Of SPCInspection.OpenRollInfo)
            Dim listijs As New List(Of SPCInspection.OpenRollInfo)
            Dim sql As String
            sql = "SELECT ijs.JobNumber, ijs.WOQuantity, ijs.AQL_Level, ijs.Standard, ijs.EmployeeNo FROM InspectionJobSummary WHERE ijs.id = " & ijsid.ToString() + " order by ShiftId desc"

            listijs = bmapijs.GetInspectObject(sql)
            If listijs.Count > 0 Then
                Dim rollnumberinput = listijs.ToArray()(0).JobNumber
                Dim rolllist As New List(Of SPCInspection.Roll_Ledge)
                DataNumber.Value = as400.GetRollDataNumber(rollnumberinput)
                rolllist = as400.GetGriegeNo(rollnumberinput, False)
                Inspector.Value = listijs.ToArray()(0).EmployeeNo

                If rolllist.Count = 0 Then
                    rolllist = as400.GetGriegeNo(rollnumberinput, True)
                End If
                If rolllist.Count > 0 Then
                    RollNumber.Value = rollnumberinput.ToString()
                    WOQuantityValue = rolllist.ToArray()(0).TICKETYARDS
                End If
            End If
        End Sub
        Private Function SetRollNumberInfo(ByVal rollnumberinput As String) As Boolean

            If rollnumberinput.Length > 0 Then
                Dim rolllist As New List(Of SPCInspection.Roll_Ledge)
                DataNumber.Value = as400.GetRollDataNumber(rollnumberinput)
                rolllist = as400.GetGriegeNo(rollnumberinput, False)
                If rolllist.Count = 0 Then
                    rolllist = as400.GetGriegeNo(rollnumberinput, True)
                End If
                If rolllist.Count > 0 Then
                    WOQuantityValue = rolllist.ToArray()(0).TICKETYARDS
                End If
            End If

        End Function
        Private Sub SetOpenWOInfo(ByVal InspectionJobSummaryId As Integer)
            Dim bmapijs As New BMappers(Of SPCInspection.InspectionJobSummary)
            Dim listijs As New List(Of SPCInspection.InspectionJobSummary)
            Dim sql As String
            Dim bmapso As New BMappers(Of SingleObject)
            If IsNothing(Request.QueryString("LocationId")) = False Then
                Response.Cookies("SPCLocation")("STCLocation") = Request.QueryString("LocationId")
                LastLocation = Request.QueryString("LocationId")
                Response.Cookies("SPCLocation").Expires = DateTime.Now.AddDays(60)
            End If

            sql = "SELECT JobNumber, DataNo, WOQuantity, AQL_Level, Standard, SampleSize, RejectLimiter, WorkRoom, CasePack, EmployeeNo FROM InspectionJobSummary WHERE id = " & InspectionJobSummaryId.ToString()

            listijs = bmapijs.GetInspectObject(sql)
            If listijs.Count > 0 Then
                Dim wonumber As String
                wonumber = listijs.ToArray()(0).JobNumber
                WorkOrder.Value = wonumber
                Dim workorderarray As Array = as400.GetInspectionWorkOrder(wonumber).ToArray()
                inspectionjobsummaryid_hidden.Value = InspectionJobSummaryId
                CartonNumber.Value = ""
                DataNumber.Value = listijs.ToArray()(0).DataNo
                workroom_hidden.Value = listijs.ToArray()(0).WorkRoom
                'workroom.Value = listijs.ToArray()(0).WorkRoom
                'CPNumber.Value = listijs.ToArray()(0).CasePack
                AuditorName.Value = listijs.ToArray()(0).EmployeeNo
                If workorderarray.Length > 0 Then
                    'Dim casepackCalc As Integer = workorderarray(0).CASEPACK / 10
                    'CPNumber.Value = workorderarray(0).CASEPACK.ToString.Trim()
                    'workroom_hidden.Value = Trim(workorderarray(0).WORKROOM)
                    'workroom.Value = Trim(workorderarray(0).WORKROOM)
                    'DataNumber.Value = workorderarray(0).DATAN.Trim()
                    'If workorderarray(0).DATAN.Trim().ToString.Length > 0 Then
                    '    CPNumber.Value = as400.GetCasePackConv(workorderarray(0).DATAN.Trim().ToString)
                    'End If

                    wopieces_hidden.Value = workorderarray(0).WOPIECES
                End If
                WOQuantityValue = listijs.ToArray()(0).WOQuantity
                aqlstandard.Value = listijs.ToArray()(0).Standard
                If CType(listijs.ToArray()(0).AQL_Level, Integer) = 100 Then
                    AQL = "100"
                    _AQLevel.Value = "100"
                ElseIf CType(listijs.ToArray()(0).AQL_Level, Integer) = 1 Then
                    AQL = "1"
                    _AQLevel.Value = "1"
                ElseIf CType(listijs.ToArray()(0).AQL_Level, Integer) = 4 Then
                    AQL = "4"
                    _AQLevel.Value = "4"
                Else
                    _AQLevel.Value = listijs.ToArray()(0).AQL_Level.ToString()
                    AQL = listijs.ToArray()(0).AQL_Level.ToString()
                End If

                SampleSize.Value = listijs.ToArray()(0).SampleSize
                RE.Value = listijs.ToArray()(0).RejectLimiter.ToString()
                REHidden.Value = RE.Value
                AC.Value = Convert.ToString(listijs.ToArray()(0).RejectLimiter - 1)

                Exit Sub
            Else
                WorkOrderError = "WorkOrder Must be Numieric"
                Exit Sub
            End If

        End Sub

        Private Function SetWorkOrderInfo(ByVal _workorder As String, ByVal aqlin As Decimal, ByVal standard As String) As Boolean
            Dim trimstring = _workorder.TrimStart("0"c)

            If IsNothing(Request.QueryString("LocationId")) = False Then
                Response.Cookies("SPCLocation")("STCLocation") = Request.QueryString("LocationId")
                LastLocation = Request.QueryString("LocationId")
                Response.Cookies("SPCLocation").Expires = DateTime.Now.AddDays(60)
            End If

            If IsNumeric(trimstring) = True Then
                Dim wonumber As String
                If trimstring.Length > 4 Then
                    wonumber = trimstring
                Else
                    WorkOrderError = "IncorrectFormat Error"
                    Return False
                End If
                WorkOrder.Value = wonumber.Trim()
                Dim workorderarray As Array = as400.GetInspectionWorkOrder(wonumber).ToArray()

                If workorderarray.Length = 0 Then
                    WorkOrderError = "Could not find WorkOrder Info"
                    Return False
                End If
                CartonNumber.Value = ""
                If IsNothing(workorderarray) = False Then
                    'Dim casepackCalc As Integer = workorderarray(0).CASEPACK / 10
                    'CPNumber.Value = workorderarray(0).CASEPACK.ToString.Trim()
                    DataNumber.Value = workorderarray(0).DATAN.Trim()
                    If workorderarray(0).DATAN.Trim().ToString.Length > 0 Then
                        'CPNumber.Value = as400.GetCasePackConv(workorderarray(0).DATAN.Trim().ToString)
                    End If
                    Location.Value = workorderarray(0).LOCATION.Trim()
                    WOQuantityValue = workorderarray(0).WOQUANTITY
                    workroom_hidden.Value = Trim(workorderarray(0).WORKROOM)
                    wopieces_hidden.Value = workorderarray(0).WOPIECES
                End If

                Select Case standard
                    Case "Reduced"
                        SetReducedSamplesSize(workorderarray(0).WOQUANTITY.ToString(), aqlin)
                        aqlstandard.Value = "Reduced"
                    Case "Regular"
                        SetSamplesSize(workorderarray(0).WOQUANTITY.ToString(), aqlin)
                        aqlstandard.Value = "Regular"
                    Case "Tight"
                        SetTightSamplesSize(workorderarray(0).WOQUANTITY.ToString(), aqlin)
                        aqlstandard.Value = "Tight"
                End Select
                REHidden.Value = RE.Value
                If CType(aqlin, Integer) = 100 Then
                    AQL = "100"
                    _AQLevel.Value = "100"
                ElseIf CType(aqlin, Integer) = 1 Then
                    AQL = "1"
                    _AQLevel.Value = "1"
                ElseIf CType(aqlin, Integer) = 4 Then
                    AQL = "4"
                    _AQLevel.Value = "4"
                Else
                    _AQLevel.Value = aqlin.ToString()
                    AQL = aqlin.ToString()
                End If

                Return True
            Else
                WorkOrderError = "WorkOrder Must be Numieric"
                Return False
            End If

            Return True
        End Function

        Private Sub RegisterUserCookie(ByVal value As String, ByVal CookieSubkey As String)
            Dim Cookename As String = "SPCInspection"

            If Not Request.Cookies(Cookename) Is Nothing Then
                Dim InspectCookie As String = Server.HtmlEncode(Request.Cookies(Cookename)(CookieSubkey)).ToString()
                Select Case CookieSubkey
                    Case "InspectionUser"
                        If value <> InspectCookie And IsNothing(value) = False Then
                            Response.Cookies(Cookename)(CookieSubkey) = value
                            Response.Cookies(Cookename)("lastVisit") = DateTime.Now.ToString()
                            Response.Cookies(Cookename).Expires = DateTime.Now.AddDays(60)
                            AuditorName.Value = value
                        ElseIf IsNothing(InspectCookie) = False And IsNothing(value) = True Then
                            AuditorName.Value = InspectCookie
                        Else
                            AuditorName.Value = value
                        End If
                    Case "AQLevel"
                End Select
            Else
                Response.Cookies(Cookename)(CookieSubkey) = value
                Response.Cookies(Cookename)("lastVisit") = DateTime.Now.ToString()
                Response.Cookies(Cookename).Expires = DateTime.Now.AddDays(60)
                AuditorName.Value = value
            End If
        End Sub

        Private Sub LoadMachineNames()
            Dim MachineArray As Array = Inspect.GetSPCMachineNames(CID).ToArray()
            MachineNames = jser.Serialize(MachineArray)
        End Sub

        Private Sub initializeInspectionLocations()
            Dim TemplateArray As List(Of selector2array)

            TemplateArray = Inspect.GetLocations(False)

            If TemplateArray Is Nothing Then
                Throw New Exception("TemplateArray Cannot be nothing")
            End If

            Dim serlist As New List(Of selector2array)(TemplateArray.ToArray())
            Dim cnt As Integer = 0

            LocationNames = jser.Serialize(Inspect.LocationNames)

            If LocationNames Is Nothing Then
                'Response.Redirect("~/ErrorPage.aspx")   'Log Error Here 8.4.15
            End If

            If Request.Cookies.AllKeys.Contains("SPCLocation") = True Then
                LastLocation = Request.Cookies("SPCLocation")("STCLocation").ToString()
            End If

        End Sub
        Public Sub initializeWorkrooms(CID As String)
            WorkRoomArr = WorkRoomsApi.GetResult(CID)
        End Sub
        Private Sub SetTightSamplesSize(ByVal lotsizenumber As Integer, ByVal AQLevel As Decimal)
            If AQLevel = 1 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 8) Then
                    SampleSize.Value = 3
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize.Value = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize.Value = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize.Value = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 20
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 32
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 50
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 80
                    Acceptance = 1
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 125
                    Acceptance = 2
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 200
                    Acceptance = 3
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 315
                    Acceptance = 5
                ElseIf (lotsizenumber >= 100001) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 500
                    Acceptance = 8
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 800
                    Acceptance = 13
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 2000
                    Acceptance = 18
                End If
            End If
            If AQLevel = 1.5 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 8) Then
                    SampleSize.Value = 3
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize.Value = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize.Value = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize.Value = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 20
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 32
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 50
                    Acceptance = 1
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 80
                    Acceptance = 2
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 125
                    Acceptance = 3
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 200
                    Acceptance = 5
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 315
                    Acceptance = 8
                ElseIf (lotsizenumber >= 100001) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 500
                    Acceptance = 12
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 800
                    Acceptance = 13
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 2000
                    Acceptance = 18
                End If

            End If
            If AQLevel = 2.5 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 8) Then
                    SampleSize.Value = 3
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize.Value = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize.Value = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize.Value = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 20
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 32
                    Acceptance = 1
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 50
                    Acceptance = 2
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 80
                    Acceptance = 3
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 125
                    Acceptance = 5
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 200
                    Acceptance = 8
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 315
                    Acceptance = 12
                ElseIf (lotsizenumber >= 100001) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 500
                    Acceptance = 18
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 800
                    Acceptance = 18
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 2000
                    Acceptance = 18
                End If
            End If
            If AQLevel = 4 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 8) Then
                    SampleSize.Value = 3
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize.Value = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize.Value = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize.Value = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 20
                    Acceptance = 1
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 32
                    Acceptance = 2
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 50
                    Acceptance = 3
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 80
                    Acceptance = 5
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 125
                    Acceptance = 8
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 200
                    Acceptance = 12
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 315
                    Acceptance = 18
                ElseIf (lotsizenumber >= 100001) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 500
                    Acceptance = 18
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 800
                    Acceptance = 18
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 2000
                    Acceptance = 18
                End If

            End If
            If AQLevel = 100 Then

                SampleSize.Value = lotsizenumber

                Acceptance = lotsizenumber

            End If
            AC.Value = Acceptance.ToString()
            RE.Value = Convert.ToString(CType(AC.Value, Integer) + 1)
            SampleSizeHidden.Value = SampleSize.Value
        End Sub

        Private Sub SetReducedSamplesSize(ByVal lotsizenumber As Integer, ByVal AQLevel As Decimal)
            If AQLevel = 1 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 2
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 13
                    Acceptance = 1
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 20
                    Acceptance = 1
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 32
                    Acceptance = 2
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 80
                    Acceptance = 4
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 125
                    Acceptance = 5
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 200
                    Acceptance = 7
                End If
            End If
            If AQLevel = 1.5 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 2
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 13
                    Acceptance = 1
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 20
                    Acceptance = 2
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 32
                    Acceptance = 3
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 80
                    Acceptance = 5
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 125
                    Acceptance = 7
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 200
                    Acceptance = 9
                End If
            End If
            If AQLevel = 2.5 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 2
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 13
                    Acceptance = 2
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 20
                    Acceptance = 3
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 32
                    Acceptance = 4
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 80
                    Acceptance = 7
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 125
                    Acceptance = 9
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 200
                    Acceptance = 12
                End If

            End If
            If AQLevel = 4 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 2
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 5
                    Acceptance = 1
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 8
                    Acceptance = 2
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 13
                    Acceptance = 3
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 20
                    Acceptance = 4
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 32
                    Acceptance = 5
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 80
                    Acceptance = 9
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 125
                    Acceptance = 12
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 200
                    Acceptance = 12
                End If

            End If
            If AQLevel = 100 Then

                SampleSize.Value = lotsizenumber

                Acceptance = lotsizenumber

            End If
            AC.Value = Acceptance.ToString()
            RE.Value = Convert.ToString(CType(AC.Value, Integer) + 1)
            SampleSizeHidden.Value = SampleSize.Value
        End Sub



        Private Sub SetSamplesSize(ByVal lotsizenumber As Integer, ByVal AQLevel As Decimal)
            If AQLevel = 1 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 13
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 50
                    Acceptance = 1
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 80
                    Acceptance = 2
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 125
                    Acceptance = 3
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 200
                    Acceptance = 5
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 315
                    Acceptance = 7
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 500
                    Acceptance = 10
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize.Value = 800
                    Acceptance = 14
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize.Value = 1250
                    Acceptance = 21
                End If
            End If
            If AQLevel = 1.5 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 8
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 32
                    Acceptance = 1
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 50
                    Acceptance = 2
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 80
                    Acceptance = 3
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 125
                    Acceptance = 5
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 200
                    Acceptance = 7
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 315
                    Acceptance = 10
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize.Value = 500
                    Acceptance = 14
                ElseIf (lotsizenumber >= 150001) Then
                    SampleSize.Value = 800
                    Acceptance = 21
                End If
            End If
            If AQLevel = 2.5 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 50) Then
                    SampleSize.Value = 5
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 20
                    Acceptance = 1
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 32
                    Acceptance = 2
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 50
                    Acceptance = 3
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 80
                    Acceptance = 5
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 125
                    Acceptance = 7
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 200
                    Acceptance = 10
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize.Value = 315
                    Acceptance = 14
                ElseIf (lotsizenumber >= 35001) Then
                    SampleSize.Value = 500
                    Acceptance = 21
                End If
            End If
            If AQLevel = 4 Then
                If (lotsizenumber >= 2) And (lotsizenumber <= 50) Then
                    SampleSize.Value = 5
                    If SampleSize.Value > lotsizenumber Then
                        SampleSize.Value = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize.Value = 13
                    Acceptance = 1
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize.Value = 20
                    Acceptance = 2
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize.Value = 32
                    Acceptance = 3
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize.Value = 50
                    Acceptance = 5
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize.Value = 80
                    Acceptance = 7
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize.Value = 125
                    Acceptance = 10
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize.Value = 200
                    Acceptance = 14
                ElseIf (lotsizenumber >= 10001) Then
                    SampleSize.Value = 315
                    Acceptance = 21
                End If
            End If
            If AQLevel = 100 Then

                SampleSize.Value = lotsizenumber

                Acceptance = lotsizenumber

            End If
            AC.Value = Acceptance.ToString()
            RE.Value = Convert.ToString(CType(AC.Value, Integer) + 1)
            SampleSizeHidden.Value = SampleSize.Value
        End Sub
        Private Sub SetSamplesSize_Old(ByVal _lotsize As Integer, ByVal aql As Decimal, ByVal standard As String)

            Dim AQLevel As Decimal = aql
            Dim lotsizenumber As Integer = _lotsize

            If (lotsizenumber >= 2) And (lotsizenumber <= 8) Then
                SampleSize.Value = 2
                If AQLevel = 4 Then
                    Acceptance = 0
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 0
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 0
                End If
                'If standard = "Reduced" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Regular" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Tightened" Then
                '    Rejecter = Acceptance + 1
                'End If
            End If
            If (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                SampleSize.Value = 3
                If AQLevel = 4 Then
                    Acceptance = 0
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 0
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 0
                End If
                'If standard = "Reduced" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Regular" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Tightened" Then
                '    Rejecter = Acceptance + 1
                'End If
            End If
            If (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                SampleSize.Value = 5
                If AQLevel = 4 Then
                    Acceptance = 0
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 0
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 0
                End If
                'If standard = "Reduced" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Regular" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Tightened" Then
                '    Rejecter = Acceptance + 1
                'End If
            End If
            If (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                SampleSize.Value = 8
                If AQLevel = 4 Then
                    Acceptance = 1
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 0
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 1
                End If
                'If standard = "Reduced" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Regular" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Tightened" Then
                '    Rejecter = Acceptance + 1
                'End If

            End If
            If (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                SampleSize.Value = 13
                If AQLevel = 4 Then
                    Acceptance = 1
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 0
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 1
                End If
                'If standard = "Reduced" Then
                '    Rejecter = Acceptance + 2
                'ElseIf standard = "Regular" Then
                '    Rejecter = Acceptance + 1
                'ElseIf standard = "Tightened" Then
                '    Rejecter = Acceptance + 1
                'End If
            End If
            If (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                SampleSize.Value = 20
                If AQLevel = 1 Then
                    Acceptance = 2
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 0
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 1
                End If
            End If
            If (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                SampleSize.Value = 32
                If AQLevel = 4 Then
                    Acceptance = 3
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 1
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 2
                End If
            End If
            If (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                SampleSize.Value = 50
                If AQLevel = 4 Then
                    Acceptance = 4
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 2
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 3
                End If
            End If
            If (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                SampleSize.Value = 80
                If AQLevel = 4 Then
                    Acceptance = 7
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 3
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 5
                End If
            End If
            If (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                SampleSize.Value = 125
                If AQLevel = 4 Then
                    Acceptance = 10
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 5
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 7
                End If
            End If
            If (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                SampleSize.Value = 200
                If AQLevel = 4 Then
                    Acceptance = 14
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 7
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 10
                End If
            End If
            If (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                SampleSize.Value = 315
                If AQLevel = 4 Then
                    Acceptance = 21
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 10
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 14
                End If
            End If
            If (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                SampleSize.Value = 500
                If AQLevel = 4 Then
                    Acceptance = 21
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 14
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 21
                End If
            End If
            If (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                SampleSize.Value = 800
                If AQLevel = 4 Then
                    Acceptance = 21
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 21
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 21
                End If
            End If
            If (lotsizenumber >= 500001) Then
                SampleSize.Value = 1250
                If AQLevel = 4 Then
                    Acceptance = 21
                End If
                If AQLevel = 1 Then
                    SampleSize.Value = lotsizenumber.ToString()
                    Acceptance = lotsizenumber
                End If
                If AQLevel = 1.5 Then
                    Acceptance = 21
                End If
                If AQLevel = 2.5 Then
                    Acceptance = 21
                End If
            End If

            AC.Value = Acceptance.ToString()
            RE.Value = Convert.ToString(CType(AC.Value, Integer) + 1)
            'LotSize.Value = lotsizenumber.ToString()

        End Sub

        Private Sub CheckRequiredFields()
            If WorkOrder.Value.Length <= 2 Then
                WorkOrder.Style.Add("border-color", "red")
                WorkOrder.Style.Add("border-width", "4px")
            Else
                WorkOrder.Style.Add("border-color", "rgb(200,200,200)")
                WorkOrder.Style.Add("border-width", "2px")
            End If
            If CartonNumber.Value.Length <= 2 Then
                CartonNumber.Style.Add("border-color", "red")
                CartonNumber.Style.Add("border-width", "4px")
            Else
                CartonNumber.Style.Add("border-color", "rgb(200,200,200)")
                CartonNumber.Style.Add("border-width", "2px")
            End If
            If AuditorName.Value.Length = 0 Then
                AuditorName.Style.Add("border-color", "red")
                AuditorName.Style.Add("border-width", "4px")
            Else
                AuditorName.Style.Add("border-color", "rgb(200,200,200)")
                AuditorName.Style.Add("border-width", "2px")
            End If
            'If LotSize.Value.Length = 0 Then
            '    LotSize.Style.Add("border-color", "red")
            '    LotSize.Style.Add("border-width", "4px")
            'Else
            '    LotSize.Style.Add("border-color", "rgb(200,200,200)")
            '    LotSize.Style.Add("border-width", "2px")
            'End If
        End Sub

    End Class


End Namespace
