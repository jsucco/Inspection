Imports System.Web.Script.Serialization
Imports System.Globalization
Imports C1.C1Excel
Imports System.IO
Imports System.Data
Imports OfficeOpenXml

Namespace core


    Partial Class APP_DataEntry_TemplateManager
        Inherits core.APRWebApp
        Public LocationNames As String
        Public SelectedId As Integer = 1
        Public SelectedName As String
        Public WorkOrderData As String
        Public RollData As String
        Public HourlyData As String
        Public OpertorData As String
        Public fromdatestring As String
        Public todatestring As String
        Public InspectStats As String = "[0]"
        Public HasNoWorkOrders As String = "false"
        Public HasNoRolls As String = "false"
        Public HasNoHours As String = "false"
        Public HasReports As String = "false"
        Public ReportList As String = "[0]"
        Public HP_ChartRange As String = "[0]"
        Public ChartMedType As String

        Public CID As Integer = 0
        Private Property Prod As New ProductionReporterDAO
        Private Property ProdRep As New ProductionReporting
        Private Property jser As New JavaScriptSerializer
        Private Property SPC As New SPCReports
        Private Property Inspect As New InspectionUtilityDAO
        ' Private Property as400 As New AS400DAO
        Private Property Util As New Utilities

        Dim selectChange As Boolean = False
        Dim replist As New List(Of InputArray)
        Dim WorkOrderCache As New List(Of Production.WorkOrderProductionSTT)
        Dim RollProdCache As New List(Of Production.RollProductionSTT)
        Dim HourlyCache As New List(Of Production.HourlyProductionSTT)
        Dim OperatorCache As New List(Of Production.OperatorProduction)
        Dim HP_ChartRangelistSTT As New List(Of Production.HP_ChartRangeSTT)
        Dim HP_ChartRangelistCAR As New List(Of Production.HP_ChartRangeCAR)
        Dim HP_ChartRangelistALL As New List(Of Production.HP_ChartRangeALL)
        Dim Isrefreshed As Boolean
        Dim UserActivityLogPK As Integer
        Dim AL As New AppLog
        Dim fromdate As DateTime
        Dim todate As DateTime
        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            fromdate = DateTime.Now.AddDays(-7)
            todate = DateTime.Now
            Dim jser As New JavaScriptSerializer
            Dim UserParse As Object = Request.UserAgent
            jser.MaxJsonLength = Int32.MaxValue
            LoadTemplateNames()

            If IsNothing(Me.Session("APRISMOBILE")) = False Then
                If Me.Session("APRISMOBILE") = "True" Then
                    Response.Redirect("~/Mobile/APREntry.aspx")
                End If
            End If

            Dim QueryTemplateid As String = Request.QueryString("LocationId")

            If IsNothing(QueryTemplateid) = False And IsNothing(Request.QueryString("selectChange")) = False Then
                selectChange = True
                SelectedId = Convert.ToInt32(QueryTemplateid)

                If SelectedId > 0 Then
                    Dim QueryDateFrom As String = Request.QueryString("datefrom")
                    Dim QueryDateTo As String = Request.QueryString("dateto")

                    If IsDate(QueryDateFrom) = True And IsDate(QueryDateTo) = True Then
                        fromdate = Convert.ToDateTime(QueryDateFrom)
                        todate = Convert.ToDateTime(QueryDateTo)

                        If fromdate.Ticks > todate.Ticks Then
                            fromdate = DateTime.Parse(fromdatestring)
                            todate = DateTime.Parse(todatestring)
                        End If
                    End If

                    Response.Cookies("SPCProductionReporter")("TemplateId_LastSelected") = SelectedId.ToString()
                    Response.Cookies("SPCProductionReporter")("lastVisit") = DateTime.Now.ToString()
                    Response.Cookies("SPCProductionReporter").Expires = DateTime.Now.AddDays(60)

                End If

            End If

            fromdatestring = fromdate.ToString("MM/dd/yyyy") + " 00:00:00"
            todatestring = todate.ToString("MM/dd/yyyy") + " 23:59:59"

            If Page.IsPostBack = False And selectChange = False Then
                If Not Request.Cookies("SPCInspectionReporter") Is Nothing Then
                    If Request.Cookies.AllKeys.Contains("TemplateId_LastSelected") Then
                        'Dim TemplateIdCookie As String = Server.HtmlEncode(Request.Cookies("SPCProductionReporter")("TemplateId_LastSelected")).ToString()
                        Dim TemplateIdCookie As Dictionary(Of String, String) = GetCookie("SPCProductionReporter", "TemplateId_LastSelected")
                        Dim lastVisitCookie As String = Server.HtmlEncode(Request.Cookies("SPCProductionReporter")("lastVisit")).ToString()
                        'If IsNumeric(TemplateIdCookie) = True Then
                        '    SelectedId = Convert.ToInt32(TemplateIdCookie)
                        'End If
                        If TemplateIdCookie.Count > 0 Then
                            SelectedId = Convert.ToInt16(TemplateIdCookie("SPCProductionReporter"))
                        End If
                        SelectId_Hidden.Value = SelectedId.ToString()
                    End If
                   
                End If

            End If
            HasReports = "True"
            reportstatus1_hidden.Value = "True"

            If IsPostBack = False Then
                'SelectId_Hidden.Value = SelectedId.ToString()

                Try
                    AL.AddToAppLog("PageLoad", "ProductionReporter", "Dates: " + fromdate.ToShortDateString() + "-" + todate.ToShortDateString(), UserActivityLogPK)
                Catch ex As Exception

                End Try
            End If
            Try
                Dim PrimaryKey = GetCookie("APR_UserActivityLog", "PrimaryKey")
                If PrimaryKey.Count > 0 Then
                    If IsNumeric(PrimaryKey.Item("APR_UserActivityLog")) Then
                        UserActivityLogPK = CType(PrimaryKey.Item("APR_UserActivityLog"), Integer)
                    End If
                End If
            Catch ex As Exception

            End Try
            SelectId_Hidden.Value = SelectedId.ToString()
            LoadPageDataTables(fromdate, todate)
            reportstatus1_hidden.Value = "True"
        End Sub

        Protected Sub GoButton_Click(sender As Object, e As System.EventArgs) Handles GoButton.Click

            Using epack As New ExcelPackage
                Dim workbook As ExcelWorkbook
                Dim dateto As String = Request.Form("dateto")
                Dim todate As DateTime
                If IsDate(dateto) = True Then
                    todate = Convert.ToDateTime(dateto)
                Else
                    todate = Date.Now
                End If
                Try
                    AL.AddToAppLog("ExportToExcel", "ProductionReporter", "Export TabGrids: " + fromdate.ToShortDateString() + "-" + todate.ToShortDateString(), UserActivityLogPK)
                Catch ex As Exception

                End Try

                Try
                    If Production.WorkOrderProductionSTT.FuncPropList.Count = 0 Then
                        Production.WorkOrderProductionSTT.FuncPropList.Add("JobSheets", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("JobYds", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("JobOverLengthInches", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("ScheduledTime", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("DownTime", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("RunTime", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("AvgSheetsPerHour", "AVG")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("JDECOMP", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("JDESCRAP", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("JDETOTREC", "SUM")
                        Production.WorkOrderProductionSTT.FuncPropList.Add("DIFF_PERC", "AVG")
                    End If
                    Dim epackbmapper As New BMappers(Of Production.WorkOrderProductionSTT)

                    'workbook = epackbmapper.LoadTableToExcelGrid(epack, WorkOrderCache, "Work Orders")
                    workbook = ProdRep.GetWorkOrderProduction(epack.Workbook, WorkOrderCache)
                    workbook = epackbmapper.InjectSubstats(epack, WorkOrderCache, todate, "Work Orders")
                Catch ex As Exception
                    AL.AddToAppLog("ExportToExcel", "ProductionReporter", "Export TabGrids: " + ex.Message, UserActivityLogPK)
                End Try
                Try
                    Dim epackbmapper As New BMappers(Of Production.RollProductionSTT)

                    If Production.RollProductionSTT.FuncPropList.Count = 0 Then
                        Production.RollProductionSTT.FuncPropList.Add("TotalYds", "SUM")
                        Production.RollProductionSTT.FuncPropList.Add("TotalSheets", "SUM")
                        Production.RollProductionSTT.FuncPropList.Add("TicketYds", "SUM")
                        Production.RollProductionSTT.FuncPropList.Add("TicketOverYds", "SUM")
                    End If

                    'workbook = epackbmapper.LoadTableToExcelGrid(epack, RollProdCache, "Roll Production")
                    workbook = ProdRep.GetRollProduction(epack.Workbook, RollProdCache)
                    workbook = epackbmapper.InjectSubstats(epack, RollProdCache, todate, "Roll Production")
                Catch ex As Exception
                    AL.AddToAppLog("ExportToExcel", "ProductionReporter", "Export TabGrids: " + ex.Message, UserActivityLogPK)
                End Try

                Try
                    Dim epackbmapper As New BMappers(Of Production.OperatorProduction)

                    If Production.OperatorProduction.FuncPropList.Count = 0 Then
                        Production.OperatorProduction.FuncPropList.Add("ScheduledTime", "SUM")
                        Production.OperatorProduction.FuncPropList.Add("DownTime", "SUM")
                        Production.OperatorProduction.FuncPropList.Add("RunTime", "SUM")
                        Production.OperatorProduction.FuncPropList.Add("TotalYds", "SUM")
                        Production.OperatorProduction.FuncPropList.Add("TotalSheets", "SUM")
                        Production.OperatorProduction.FuncPropList.Add("Efficeincy", "SUM")
                        Production.OperatorProduction.FuncPropList.Add("AvgSheetsPerMin", "AVG")
                        Production.OperatorProduction.FuncPropList.Add("AvgYdsPerMin", "AVG")
                        Production.OperatorProduction.FuncPropList.Add("OverLengthInches", "SUM")
                    End If
                    Dim LocationAbb = (From x In Prod.LocationNames Where x.id = SelectedId Select x.Abreviation).ToArray()

                    If LocationAbb.Count > 0 Then
                        'OperatorCache = Prod.GetSPCOperatorProduction(fromdate, todatestring, LocationAbb(0))

                        'workbook = epackbmapper.LoadTableToExcelGrid(epack, OperatorCache, "Operator Production")
                        workbook = ProdRep.GetOperatorProduction(epack.Workbook, OperatorCache)
                        workbook = epackbmapper.InjectSubstats(epack, OperatorCache, todate, "Operator Production")
                    End If
                Catch ex As Exception
                    ' AL.AddToAppLog("ExportToExcel", "ProductionReporter", "Export TabGrids: " + ex.Message, UserActivityLogPK)
                End Try
                'Try
                '    Dim epackbmapper As New BMappers(Of Production.HourlyProductionSTT)

                '    If Production.HourlyProductionSTT.FuncPropList.Count = 0 Then
                '        Production.HourlyProductionSTT.FuncPropList.Add("ProductCount", "SUM")
                '        Production.HourlyProductionSTT.FuncPropList.Add("OverLengthInches", "SUM")
                '        Production.HourlyProductionSTT.FuncPropList.Add("HourlyYds", "SUM")
                '        Production.HourlyProductionSTT.FuncPropList.Add("CutLengthSpec", "AVG")
                '        Production.HourlyProductionSTT.FuncPropList.Add("RunTime", "SUM")
                '        Production.HourlyProductionSTT.FuncPropList.Add("DownTime", "SUM")
                '    End If
                '    workbook = epackbmapper.LoadTableToExcelGrid(epack, HourlyCache, "Hourly Production")
                '    workbook = epackbmapper.InjectSubstats(epack, HourlyCache, todate, "Hourly Production")
                'Catch ex As Exception
                '    AL.AddToAppLog("ExportToExcel", "ProductionReporter", "Export TabGrids: " + ex.Message, UserActivityLogPK)
                'End Try
                If epack.Workbook.Worksheets.Count = 0 Then
                    epack.Workbook.Worksheets.Add("New Tab")
                End If

                Response.Clear()
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

                'Response.ContentType = "application/vnd.ms-excel"
                Response.AddHeader("content-disposition", "attachment;  filename=ProductionReport.xlsx")

                Dim stream As MemoryStream = New MemoryStream(epack.GetAsByteArray())

                Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)

                Response.Flush()

                Response.Close()

            End Using

        End Sub

        Protected Sub GetReport1_Click(sender As Object, e As System.EventArgs) Handles GetReport1.Click
            Dim datefrom As String = Request.Form("datefrom")
            Dim dateto As String = Request.Form("dateto")
            Dim fromdatetime As DateTime
            Dim todatetime As DateTime
            Dim TemplateIdString = Request.Form("TemplateId")
            Dim TemplateId As Integer = Convert.ToInt16(TemplateIdString)
            Dim InspectionReports As New InspectionReporting
            Dim stream As MemoryStream
            If Not (datefrom Is Nothing) And (dateto Is Nothing) And (TemplateIdString Is Nothing) Then
                Exit Sub
            Else
                datefrom = datefrom + " 00:00:00"
                dateto = dateto + " 23:59:59"
                fromdatetime = DateTime.Parse(datefrom)
                todatetime = DateTime.Parse(dateto)
                TemplateId = Convert.ToInt16(TemplateIdString)
            End If

            Using epack As New ExcelPackage
                Dim workbook As ExcelWorkbook = epack.Workbook

                Dim ReportName1 As Object = reportname1_hidden.Value
                Dim Report1Status As String = reportstatus1_hidden.Value
                If IsNumeric(SelectId_Hidden.Value) = True Then
                    Dim Locationid As Integer = CType(SelectId_Hidden.Value, Integer)

                    Dim Locationarray = (From x In Prod.LocationNames Where x.id = Locationid Select x.Abreviation).ToArray()

                    If Report1Status = "True" And check1.Checked = True Then
                        workbook.Worksheets.Add("Work Order Summary")
                        workbook = SPC.GetSpcReport(epack, todatetime, fromdatetime, Locationarray(0).Trim())

                    End If

                End If

                If workbook.Worksheets.Count = 0 Then
                    workbook.Worksheets.Add("Sheet1")
                End If

                stream = New MemoryStream(epack.GetAsByteArray())
                
            End Using

            Try
                AL.AddToAppLog("ExportToExcel", "ProductionReporter", "Export WorkOrderSummary Report: " + fromdate.ToShortDateString() + "-" + todate.ToShortDateString(), UserActivityLogPK)
            Catch ex As Exception

            End Try
            Response.Clear()
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.AddHeader("content-disposition", "attachment;  filename=ProductionReport.xlsx")
            'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"
            Response.AddHeader("content-disposition", "attachment;  filename=ProductionDef_Summary.xlsx")



            Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)

            Response.Flush()

            Response.Close()

        End Sub
        
        Protected Sub GoDate_Click(sender As Object, e As System.EventArgs) Handles GODate.Click
            Dim datefrom As String = Request.Form("datefrom")
            Dim dateto As String = Request.Form("dateto")
            Dim fromdatetime As DateTime
            Dim todatetime As DateTime

            If Not (datefrom Is Nothing) And (dateto Is Nothing) Then
                Exit Sub
            Else
                datefrom = datefrom + " 00:00:00"
                dateto = dateto + " 23:59:59"
                fromdatetime = DateTime.Parse(datefrom)
                todatetime = DateTime.Parse(dateto)
                If Not Request.QueryString("LocationId") Is Nothing Then
                    SelectedId = CType(Request.QueryString("LocationId"), Integer)
                    SelectId_Hidden.Value = SelectedId.ToString()
                End If
            End If

            LoadPageDataTables(fromdatetime, todatetime)
            fromdatestring = fromdatetime.ToString("MM/dd/yyyy")
            todatestring = todatetime.ToString("MM/dd/yyyy")

            Try
                AL.AddToAppLog("ExportToExcel", "ProductionReporter", "DateChange: " + fromdate.ToShortDateString() + "-" + todate.ToShortDateString())
            Catch ex As Exception

            End Try

        End Sub
        Private Sub LoadTemplateNames()
            Dim TemplateArray As Array = Prod.GetProductionLocations().ToArray()
            If selectChange = False Then
                SelectedId = Prod.NEWALLID
            End If
            If TemplateArray.Length > 0 Then
                If Prod.LocationsSelectors.Count > 0 Then
                    LocationNames = jser.Serialize(Prod.LocationsSelectors)
                End If
            End If
            If LocationNames Is Nothing Then
                Response.Redirect("~/ErrorPage.aspx")
            End If

        End Sub
        Private Sub LoadPageDataTables(ByVal fromdate As DateTime, ByVal todate As DateTime)
            Dim WorkOrderCount As New List(Of SingleObject)
            Dim RollProductionCount As New List(Of SingleObject)
            Dim TotalYds As New List(Of SingleObject)
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Dim countwoobj As Integer
            Dim countrpobj As Integer
            Dim sumhyobj As Decimal
            Dim expired As DateTime = Now.AddMinutes(5)
            Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd") + "." + SelectedId.ToString
            Try
                If SelectedId = 0 Or SelectedId = Prod.NEWALLID Then
                    SelectedName = "ALL"
                Else
                    Dim selectednameresult = (From x In Prod.LocationNames Where x.id = SelectedId Select x.Abreviation).ToArray()
                    SelectedName = selectednameresult(0).ToString().Trim()
                End If
                If selectChange = True Then
                    WorkOrderCache = Cache("WorkOrderProduction" + CacheString)
                    RollProdCache = Cache("RollProduction" + CacheString)
                    HourlyCache = Cache("HourlyProduction" + CacheString)
                    OperatorCache = Cache("OperatorProduction" + CacheString)
                Else
                    WorkOrderCache = Nothing
                    RollProdCache = Nothing
                    HourlyCache = Nothing
                    OperatorCache = Nothing
                End If

                If WorkOrderCache Is Nothing Then
                    Dim dayscount As Long = DateDiff(DateInterval.Day, fromdate, todate)

                    WorkOrderCache = Prod.GetSPCWorkOrderProduction(fromdate, todate, SelectedName)

                    If WorkOrderCache.Count > 0 Then
                        Context.Cache.Insert("WorkOrderProduction" + CacheString, WorkOrderCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If
                If Not WorkOrderCache Is Nothing Then
                    countwoobj = (From x In WorkOrderCache Select x.WorkOrder).Distinct().Count()
                Else
                    countwoobj = 0
                End If
                WorkOrderData = jser.Serialize(WorkOrderCache)
                If RollProdCache Is Nothing Then
                    RollProdCache = Prod.GetSPCRollProduction(fromdate, todateform, SelectedName)

                    If WorkOrderCache.Count > 0 Then
                        Context.Cache.Insert("RollProduction" + CacheString, RollProdCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If
                RollData = jser.Serialize(RollProdCache)
                If Not RollProdCache Is Nothing Then
                    countrpobj = (From x In RollProdCache Select x.RollNo).Distinct().Count()
                Else
                    countrpobj = 0
                End If

                If HourlyCache Is Nothing Then
                    HourlyCache = Prod.GetSPCHourlyProduction(fromdate, todateform, SelectedName)

                    If HourlyCache.Count > 0 Then
                        Context.Cache.Insert("HourlyProduction" + CacheString, HourlyCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If
                HourlyData = jser.Serialize(HourlyCache)
                If OperatorCache Is Nothing Then
                    OperatorCache = Prod.GetSPCOperatorProduction(fromdate, todate, SelectedName)

                    If OperatorCache.Count > 0 Then
                        Context.Cache.Insert("OperatorProduction" + CacheString, OperatorCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If
                OpertorData = jser.Serialize(OperatorCache)


                If Not HourlyCache Is Nothing Then
                    sumhyobj = (From x In HourlyCache Select x.HourlyYds).Sum()
                Else
                    sumhyobj = 0
                End If

                Dim bmap_cnt As New BMappers(Of core.SingleObject)
                'WorkOrderCount = bmap_cnt.GetSpcObject("SELECT COUNT(*) AS Object1 FROM  WorkOrderProduction WHERE (FinishTime <= CONVERT(DATETIME, '" & todate.ToShortDateString() & "', 102)) AND (StartTime >= CONVERT(DATETIME, '" & fromdate.ToShortDateString() & "', 102))")
                'RollProductionCount = bmap_cnt.GetSpcObject("SELECT COUNT(*) AS Object1 FROM RollProduction WHERE (EndTime <= CONVERT(DATETIME, '" & todate.ToShortDateString() & "', 102)) AND (StartTime >= CONVERT(DATETIME, '" & fromdate.ToShortDateString() & "', 102))")
                'TotalYds = bmap_cnt.GetSpcObject("SELECT SUM(HourlyYds) AS Object1 FROM HourlyProduction WHERE  (HourBegin <= CONVERT(DATETIME, '" & todate.ToShortDateString() & "', 102)) AND (HourBegin >= CONVERT(DATETIME, '" & fromdate.ToShortDateString() & "', 102))")


                If SelectedName = "STT" Then
                    Dim Hp_ChartRangeFormedList As New List(Of Production.HP_ChartRangeSTT_ST)
                    ChartMedType = "Sheets"
                    HP_ChartRangelistSTT = Prod.GetHourlyProdRange(fromdate, todate, SelectedName)
                    Dim HP_ChartRangeArray = HP_ChartRangelistSTT.ToArray()
                    For Each item In HP_ChartRangeArray
                        Hp_ChartRangeFormedList.Add(New Production.HP_ChartRangeSTT_ST With {.HOURBEGIN = item.HOURBEGIN.ToShortDateString() + " " + item.HOURBEGIN.ToLongTimeString(), .STT_AKAB2 = item.STT_AKAB2, .STT_PILLOW1 = item.STT_PILLOW1, .STT_PILLOW2 = item.STT_PILLOW2, .STT_TEXPA2 = item.STT_TEXPA2, .STT_TEXPA1 = item.STT_TEXPA1, .STT_TEXPA3 = item.STT_TEXPA3, .STT_AKAB1 = item.STT_AKAB1})
                    Next
                    HP_ChartRange = jser.Serialize(Hp_ChartRangeFormedList)
                ElseIf SelectedName = "CAR" Then
                    Dim Hp_ChartRangeFormedList As New List(Of Production.HP_ChartRangeCAR_ST)
                    ChartMedType = "Yards"
                    HP_ChartRangelistCAR = Prod.GetHourlyProdRange_Yards(fromdate, todate, SelectedName)
                    Dim HP_ChartRangeArray = HP_ChartRangelistCAR.ToArray()
                    For Each item In HP_ChartRangeArray
                        Hp_ChartRangeFormedList.Add(New Production.HP_ChartRangeCAR_ST With {.HOURBEGIN = item.HOURBEGIN.ToShortDateString() + " " + item.HOURBEGIN.ToLongTimeString(), .CAR_PREPENTRY = item.CAR_PREPENTRY, .CAR_PREPEXIT = item.CAR_PREPEXIT, .CAR_TENTER1 = item.CAR_TENTER1, .CAR_TENTER2 = item.CAR_TENTER2})
                    Next
                    HP_ChartRange = jser.Serialize(Hp_ChartRangeFormedList)


                ElseIf SelectedName = "ALL" Then
                    Dim Hp_ChartRangeFormedList As New List(Of Production.HP_ChartRangeALL_ST)
                    ChartMedType = "Yards"
                    HP_ChartRangelistALL = Prod.GetHourlyProdRange_Yards(fromdate, todate, SelectedName)
                    Dim HP_ChartRangeArray = HP_ChartRangelistALL.ToArray()
                    For Each item In HP_ChartRangeArray
                        Hp_ChartRangeFormedList.Add(New Production.HP_ChartRangeALL_ST With {.HOURBEGIN = item.HOURBEGIN.ToShortDateString() + " " + item.HOURBEGIN.ToLongTimeString(), .CAR_PREPENTRY = item.CAR_PREPENTRY, .CAR_PREPEXIT = item.CAR_PREPEXIT, .CAR_TENTER1 = item.CAR_TENTER1, .CAR_TENTER2 = item.CAR_TENTER2, .STT_AKAB2 = item.STT_AKAB2, .STT_PILLOW1 = item.STT_PILLOW1, .STT_PILLOW2 = item.STT_PILLOW2, .STT_TEXPA2 = item.STT_TEXPA2, .STT_TEXPA1 = item.STT_TEXPA1, .STT_TEXPA3 = item.STT_TEXPA3, .STT_AKAB1 = item.STT_AKAB1})
                    Next
                    HP_ChartRange = jser.Serialize(Hp_ChartRangeFormedList)
                End If
                Dim statlst As New List(Of InputArray)

                statlst.Add(New InputArray With {.key = "TotalYards", .value = sumhyobj})
                statlst.Add(New InputArray With {.key = "WorkOrderCount", .value = countwoobj})
                statlst.Add(New InputArray With {.key = "RollCount", .value = countrpobj})


                InspectStats = jser.Serialize(statlst.ToArray())

                If WorkOrderCache.Count = 0 Then
                    HasNoWorkOrders = "true"
                End If
                If RollProdCache.Count = 0 Then
                    HasNoRolls = "true"
                End If
                If HourlyCache.Count = 0 Then
                    HasNoHours = "tue"
                End If


            Catch ex As Exception

            End Try



        End Sub

    End Class


End Namespace
