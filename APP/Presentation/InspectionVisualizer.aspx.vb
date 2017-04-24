Imports System.Web.Script.Serialization
Imports System.Data
Imports System.Data.Entity
Imports System.Web.Services
Imports OfficeOpenXml
Imports System.IO

Namespace core

    Partial Class APP_Presentation_InspectionVisualizer
        Inherits core.APRWebApp
        Public fromdate As String = DateTime.Now.AddDays(-15).ToShortDateString()
        Public todate As String = DateTime.Now.ToShortDateString()
        Public ytdfromdate As String
        Public mtdfromdate As String
        Public CurrentDate As String
        Public LocationNames As String
        Public LocationNamesDrop As String = ""
        Public EmployeeNames As String = ""
        Public DefectDescs As String = ""
        Public DefectTypes As String = ""
        Public ADMINFLAG As String = "false"
        Public UserActivityLogId As Integer = 0
        Private Inspect As New InspectionUtilityDAO
        Private II As New InspectionInputDAO
        Private SPC As New SPCReports
        Dim InspectionReports As New InspectionReporting
        Dim jser As New JavaScriptSerializer
        Public SessionId As String
        Dim listis As New List(Of SPCInspection.InspectionSummaryDisplay)
        Dim reportFilePath As FileInfo
        Dim listdm As New List(Of SPCInspection.DefectMasterDisplay)
        Dim listss As New List(Of SPCInspection.SpecSummaryDisplay)
        Dim listssg As New List(Of SPCInspection.SpecsSubgrid)
        Dim listwoc As New List(Of SPCInspection.InspectionCompliance_Local)
        Dim listdd As New List(Of SPCInspection.Dump)
        Dim serlist As New List(Of Locationarray)
        Public InsTimerReport As MemoryStream
        Public SpecSummaryReport As MemoryStream
        Public InspectionTypesArray As String

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            SessionId = Session.SessionID

            Dim UserParse As Object = Request.UserAgent

            Try
                LoadTemplateNames()
                LoadReportData()
                InspectionTypesArray = jser.Serialize(GetInspectionTypes())
                SetDates()
                SetUserPermissions()
                LoadDropDowndata()

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            HttpRuntime.Cache.Remove("TableJobSummary." + SessionId.ToString())
            HttpRuntime.Cache.Remove("TableSpecSummary." + SessionId.ToString())


        End Sub

        Public Function GetInspectionTypes() As List(Of SPCInspection.InspectionTypes)

            Dim sql As String
            Dim bmap_so As New BMappers(Of SPCInspection.InspectionTypes)
            Dim listso As New List(Of SPCInspection.InspectionTypes)

            sql = "select distinct it.Name as Name, tn.LineType as Abreviation from TemplateName tn INNER JOIN InspectionTypes it ON tn.LineType = it.Abreviation"

            listso = bmap_so.GetInspectObject(sql)

            Return listso
        End Function

        Private Sub SetDates()

            mtdfromdate = New DateTime(Date.Now.Year, Date.Now.Month, 1).ToShortDateString()
            CurrentDate = DateTime.Now.ToShortDateString()
            ytdfromdate = New DateTime(Date.Now.AddMonths(-1).Year, 1, 1, 0, 0, 0).ToShortDateString()
        End Sub


        Private Sub SetUserPermissions()
            Using AprManager As New AprManager_Entities()

                Dim UserCookie = GetCookie("APR_UserActivityLog", "PrimaryKey")

                Dim listam As UserActivityLog

                If UserCookie.Count > 0 Then
                    If IsNumeric(UserCookie.Item("APR_UserActivityLog")) Then
                        UserActivityLogId = Convert.ToInt64(UserCookie.Item("APR_UserActivityLog"))
                        listam = (From v In AprManager.UserActivityLogs Where v.id = UserActivityLogId).FirstOrDefault()
                        If Not listam Is Nothing Then
                            Dim userFormed = FormatUserName(listam.UserID.ToString())
                            Dim listem = (From v In AprManager.EmailMasters Where v.Address = userFormed + "@standardtextile.com" Select v).ToArray()
                            If listem.Count > 0 Then
                                If listem(0).ADMIN = True Then
                                    ADMINFLAG = "true"
                                End If
                            End If
                        End If
                    End If
                End If

            End Using


        End Sub

        Private Function FormatUserName(ByVal username As String) As String

            Dim userFormed As String = ""
            Dim userSplit As String()
            If IsNothing(username) = False Then
                userSplit = username.Split("\").ToArray()
                If userSplit.Count = 1 Then
                    userFormed = username
                ElseIf userSplit.Count > 1 Then
                    userFormed = userSplit(1)
                End If
            End If
            Return userFormed
        End Function

        Protected Sub ReportCallBack_Click(sender As Object, e As System.EventArgs) Handles ReportCallBack.Click

            Response.Redirect("http://m.standardtextile.com/dataautomations/launch.aspx?ReportType=A")
            Exit Sub
            'Dim TemplateIdString = Request.Form("TemplateId")

            'If ActiveReportId.Value <> "none" Then
            '    Dim exportedmem As New MemoryStream

            '    Select Case ActiveReportId.Value
            '        Case "InsTimerReport"

            '            Exit Sub
            '        Case "InsSummary"
            '            Response.Redirect("http://m.standardtextile.com/dataautomations/launch.aspx?ReportType=B&Database=Inspection")
            '            Exit Sub
            '        Case "InsDefects"
            '            Response.Redirect("http://m.standardtextile.com/dataautomations/launch.aspx?ReportType=B&Database=Inspection")
            '            Exit Sub
            '        Case Else
            '            If reportFilePath Is Nothing Or reportFilePath.Name.Length = 0 Then
            '                Exit Sub
            '            End If
            '    End Select

            '    Response.Clear()
            '    Response.ClearContent()
            '    Response.ClearHeaders()
            '    Response.Cookies.Clear()
            '    Response.Cache.SetCacheability(HttpCacheability.Private)
            '    Response.CacheControl = "private"
            '    Response.ContentEncoding = System.Text.UTF8Encoding.UTF8
            '    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            '    Response.AddHeader("Content-Disposition",
            '                       "attachment; filename=" + reportFilePath.Name)
            '    Response.WriteFile(reportFilePath.FullName)
            '    Response.Flush()
            '    Response.End()
            '    If reportFilePath.Exists Then
            '        reportFilePath.Delete()
            '        reportFilePath = Nothing
            '    End If

            'End If


        End Sub

        Private Sub LoadReportData()
            
            If ActiveReportId.Value <> "none" And IsNothing(SessionId) = False Then
                Dim locationarray As New Object
                If SelectedCID.Value.Length > 0 Then
                    locationarray = jser.Deserialize(Of List(Of ActiveLocations))(SelectedCID.Value)
                End If
                Dim FilterList As New List(Of ActiveFilterObject)
                If FilterListTag.Value.Length > 0 Then
                    FilterList = jser.Deserialize(Of List(Of ActiveFilterObject))(FilterListTag.Value)
                End If

                Select Case ActiveReportId.Value
                    Case "InsSummary"
                        reportFilePath = New FileInfo(Path.GetTempPath() + "\\InspectionSummary.xlsx")
                        If reportFilePath.Exists Then
                            reportFilePath.Delete()
                        End If
                        If IsNothing(HttpRuntime.Cache("TableJobSummary." + SessionId.ToString())) = False Then
                            listis = HttpRuntime.Cache("TableJobSummary." + SessionId.ToString())
                            listis = (From v In listis Select v Order By v.ijsid Descending).ToList()
                        Else
                            listis = Inspect.GetInspectionSummaryDay(DateTime.Parse(DateFrom_Hidden.Value), DateTime.Parse(DateTo_Hidden.Value))
                            If SelectedCID.Value.Length > 0 Then
                                listis = FilterObjectByLocation(listis, Locationarray)
                                If IsNothing(FilterList) = False Then
                                    listis = FilterObjectByList(listis, FilterList)
                                End If
                            End If
                        End If

                        If IsNothing(listis) = False Then
                            Using epack As New ExcelPackage(reportFilePath)
                                Dim workbook As ExcelWorkbook = epack.Workbook
                                Dim bmapss As New BMappers(Of SPCInspection.InspectionSummaryDisplay)

                                workbook = InspectionReports.InsertInspectionJobSummaryDisplay(workbook, listis, Inspect.LocationNames)

                                epack.Save()
                            End Using
                        End If
                    Case "InsDefects"
                        listdm = Inspect.GetDefectMasterData3(DateTime.Parse(DateFrom_Hidden.Value), DateTime.Parse(DateTo_Hidden.Value), FilterList)
                        reportFilePath = New FileInfo(Path.GetTempPath() + "\\DefectMaster.xlsx")
                        If reportFilePath.Exists Then
                            reportFilePath.Delete()
                        End If
                        If SelectedCID.Value.Length > 0 Then
                            listdm = FilterdmObjectByLocation(listdm, Locationarray)
                        End If

                        If IsNothing(listdm) = False Then
                            Using epack As New ExcelPackage(reportFilePath)
                                Dim workbook As ExcelWorkbook = epack.Workbook

                                workbook = SPC.GetDefectMaster(workbook, listdm, "Defects")
                                epack.Save()
                            End Using
                        End If

                    Case "InsComp"

                        listwoc = Inspect.Getas400WOInspectionCompliance(DateTime.Parse(DateFrom_Hidden.Value), DateTime.Parse(DateTo_Hidden.Value))
                        reportFilePath = New FileInfo(Path.GetTempPath() + "\\ComplianceSummary.xlsx")
                        If reportFilePath.Exists Then
                            reportFilePath.Delete()
                        End If
                        If locationarray.Count > 0 Then
                            listwoc = FilterCompObjectByLocation(listwoc, locationarray)
                        End If

                        If IsNothing(listwoc) = False Then
                            Using epack As New ExcelPackage(reportFilePath)
                                Dim workbook As ExcelWorkbook = epack.Workbook
                                workbook = InspectionReports.InsertCompliance(workbook, listwoc)
                                epack.Save()
                            End Using
                        End If
                    Case "InsSpec"
                        If IsNothing(HttpRuntime.Cache("TableSpecSummary." + SessionId.ToString())) = False Then
                            listss = HttpRuntime.Cache("TableSpecSummary." + SessionId.ToString())
                        Else
                            Dim listss_fl As New List(Of SPCInspection.SpecSummaryDisplay)

                            listss = Inspect.GetSpecSummary(DateTime.Parse(DateFrom_Hidden.Value), DateTime.Parse(DateTo_Hidden.Value))

                            If listss.Count > 0 Then
                                If SelectedCID.Value.Length > 0 Then
                                    listss = FilterSpecObjectByLocation(listss, locationarray)
                                End If
                            End If
                            If IsNothing(FilterList) = False And FilterList.Count > 0 Then
                                listss = FilterSpecObjectByList(listss, FilterList)
                            End If
                        End If

                        listssg = Inspect.GetSpecsByLocation(Locationarray, DateTime.Parse(DateFrom_Hidden.Value), DateTime.Parse(DateTo_Hidden.Value))
                        reportFilePath = New FileInfo(Path.GetTempPath() + "\\SpecSummary.xlsx")
                        If reportFilePath.Exists Then
                            reportFilePath.Delete()
                        End If
                        If IsNothing(listss) = False And IsNothing(listssg) = False Then
                            Using epack As New ExcelPackage(reportFilePath)
                                Dim workbook As ExcelWorkbook = epack.Workbook
                                workbook = InspectionReports.GetSpecSummaryDisplay(workbook, listss)

                                If IsNothing(workbook) = False Then
                                    workbook = InspectionReports.GetSpecsSubgrid(workbook, listssg)
                                End If
                                epack.Save()
                            End Using
                        End If
                    Case "InsDump"
                        Dim listdd As New List(Of SPCInspection.Dump)

                        listdd = Inspect.getDumpData(DateTime.Parse(DateFrom_Hidden.Value), DateTime.Parse(DateTo_Hidden.Value))

                        If SelectedCID.Value.Length > 0 Then
                            listdd = FilterdumpObjectByLocation(listdd, Locationarray)
                        End If

                        If IsNothing(listdd) = False Then
                            reportFilePath = New FileInfo(Path.GetTempPath() + "\\InspectionSummary.xlsx")
                            If reportFilePath.Exists Then
                                reportFilePath.Delete()
                            End If
                            If listdd.Count > 0 Then
                                Using epack As New ExcelPackage(reportFilePath)
                                    Dim workbook As ExcelWorkbook = epack.Workbook

                                    workbook = SPC.GetDataDump(workbook, listdd, "Dump")
                                    epack.Save()
                                End Using
                            End If
                        End If
                    Case "InsTimerReport"
                End Select
            End If
        End Sub
        Private Sub LoadTemplateNames()
            Dim TemplateArray As List(Of selector2array) = Inspect.GetLocations(True)

            Dim cnt As Integer = 0

            serlist = (From v In Inspect.LocationNames Order By v.id Descending Select v).ToList()

            LocationNames = jser.Serialize(serlist)

            If LocationNames Is Nothing Then
                Response.Redirect("~/ErrorPage.aspx")
            End If

        End Sub

        Private Sub LoadDropDowndata()
            Dim _db As New Inspection_Entities
            Try
                Dim listem As List(Of String) = (From x In _db.DefectMasters Order By x.EmployeeNo Descending Select x.EmployeeNo).Distinct().ToList()
                If Not listem Is Nothing Then
                    Dim i As Integer = 1
                    For Each item In listem
                        If Not item Is Nothing Then
                            If item.ToString().Length > 1 Then
                                ' item = item.Replace(" ", "")

                                EmployeeNames += " " + item + ":" + item + ";"

                            End If
                        End If
                    Next
                    If EmployeeNames.Length > 2 Then
                        EmployeeNames = EmployeeNames.ToString().Remove(EmployeeNames.Length - 1, 1)
                    End If

                End If
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Try
                Dim listem As List(Of String) = (From x In _db.ButtonLibraries Order By x.Name Descending Select x.Name).Distinct().ToList()
                If Not listem Is Nothing Then
                    For Each item In listem
                        If Not item Is Nothing Then
                            DefectDescs += " " + item + ":" + item + ";"
                        End If
                    Next
                    If DefectDescs.Length > 2 Then
                        DefectDescs = DefectDescs.ToString().Remove(DefectDescs.Length - 1, 1)
                    End If
                End If
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Try

                For Each Item In serlist

                    If Not Item Is Nothing Then
                        LocationNamesDrop += " " + Item.CID.ToString() + ":" + Item.Abreviation + ";"
                    End If

                Next
                If LocationNamesDrop.Length > 2 Then
                    LocationNamesDrop = LocationNamesDrop.ToString().Remove(LocationNamesDrop.Length - 1, 1)
                End If
            Catch ex As Exception

            End Try

            Try
                Dim listem As List(Of String) = (From x In _db.DefectMasters Order By x.DefectClass Descending Select x.DefectClass).Distinct().ToList()
                If Not listem Is Nothing Then
                    For Each item In listem
                        If Not item Is Nothing Then
                            DefectTypes += " " + item + ":" + item + ";"
                        End If
                    Next
                    If DefectTypes.Length > 2 Then
                        DefectTypes = DefectTypes.ToString().Remove(DefectTypes.Length - 1, 1)
                    End If
                End If
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try
        End Sub

        Public Function FilterSpecObjectByLocation(ByRef listijs As List(Of SPCInspection.SpecSummaryDisplay), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.SpecSummaryDisplay)


            For Each item In locationarray
                If item.status = "False" Then
                    listijs = (From v In listijs Where v.CID <> item.CID Select v).ToList()
                End If
            Next

            Return listijs

        End Function

        Public Function FilterCompObjectByLocation(ByVal listijs As List(Of SPCInspection.InspectionCompliance_Local), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.InspectionCompliance_Local)
            Dim iu As New InspectionUtilityDAO
            For Each item In locationarray
                If item.status = "False" Then
                    Dim as400Loc = iu.getAS400LocationName(item.CID)
                    If as400Loc.Count > 0 Then
                        listijs = (From v In listijs Where v.Location <> as400Loc.ToArray()(0).Object1 Select v).ToList()
                    End If
                End If
            Next

            Return listijs

        End Function

        Public Function FilterObjectByLocation(ByRef listijs As List(Of SPCInspection.InspectionSummaryDisplay), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.InspectionSummaryDisplay)


            For Each item In locationarray
                If item.status = "False" Then
                    listijs = (From v In listijs Where v.CID <> item.CID Select v).ToList()
                End If
            Next

            Return listijs

        End Function

        Public Function FilterObjectByList(ByRef listijs As List(Of SPCInspection.InspectionSummaryDisplay), ByVal FilterListin As List(Of ActiveFilterObject)) As List(Of SPCInspection.InspectionSummaryDisplay)

            FilterListin = (From v In FilterListin Select v Order By v.id Ascending).ToList()

            For Each item In FilterListin

                Select Case item.Name
                    Case "pf_AuditType"
                        listijs = (From v In listijs Where v.LineType = item.value Select v).ToList()
                    Case "pf_DataNumber"
                        listijs = (From v In listijs Where v.DataNo = item.value Select v).ToList()
                End Select

            Next

            Return listijs

        End Function

        Public Function FilterSpecObjectByList(ByRef listijs As List(Of SPCInspection.SpecSummaryDisplay), ByVal FilterListin As List(Of ActiveFilterObject)) As List(Of SPCInspection.SpecSummaryDisplay)

            FilterListin = (From v In FilterListin Select v Order By v.id Ascending).ToList()

            For Each item In FilterListin

                Select Case item.Name
                    Case "pf_AuditType"
                        listijs = (From v In listijs Where v.LineType = item.value Select v).ToList()
                    Case "pf_DataNumber"
                        listijs = (From v In listijs Where v.DataNo = item.value Select v).ToList()
                End Select

            Next

            Return listijs

        End Function

        Public Function FilterdmObjectByLocation(ByRef listijs As List(Of SPCInspection.DefectMasterDisplay), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.DefectMasterDisplay)


            For Each item In locationarray
                If item.status = "False" Then
                    listijs = (From v In listijs Where v.Location.Trim() <> item.CID Select v).ToList()
                End If
            Next

            Return listijs

        End Function

        Public Function FilterdumpObjectByLocation(ByRef listijs As List(Of SPCInspection.Dump), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.Dump)


            For Each item In locationarray
                If item.status = "False" Then
                    listijs = (From v In listijs Where v.CID <> item.CID Select v).ToList()
                End If
            Next

            Return listijs

        End Function

        Public Function FilterTimerByLocation(ByVal listdtr As List(Of SPCInspection.TimerReport), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.TimerReport)
            For Each item In locationarray
                If item.status = "False" Then
                    listdtr = (From v In listdtr Where v.CID <> item.CID Select v).ToList()
                End If
            Next

            Return listdtr
        End Function
    End Class

End Namespace
