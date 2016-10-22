Imports System.Web.Script.Serialization
Imports System.Globalization
Imports C1.C1Excel
Imports System.IO
Imports System.Data
Imports OfficeOpenXml
Imports System.Reflection

Namespace core

    Partial Class APP_DataEntry_TemplateManager
        Inherits core.APRWebApp

        Public LocationNames As String
        Public LocationMasterId As Integer
        Public SelectedId As Integer = 1
        Public SelectedName As String
        Public SelectedAb As String
        Public TemplateDefectMaster As String = "[0]"
        Public DefectMasterMonthlyCount As String
        Public fromdatestring As String
        Public todatestring As String
        Public DefectMasterHistogram As String = "[0]"
        Public ScatterPlotJson As String = "[0]"
        Public DefectByTypeChart As String = "[0]"
        Public DefectByEmployeeNoChart As String = "[0]"
        Public HasProductSpecs As Boolean
        Public ProductDisplaySpecCollection As String = "[0]"
        Public InspectStats As String = "[0]"
        Public HasNoDefects As String = "False"
        Public HasReports As String = "False"
        Public ReportList As String = "[0]"
        Public Chart2Array As String = "[0]"
        Public Location As String = ""
        Public DefectImageDisplayArray As String = "[0]"
        Private Property Inspect As New InspectionUtilityDAO
        Private Property DA As New InspectionInputDAO
        Private Property DU As New InspectionUtilityDAO
        Private Property jser As New JavaScriptSerializer
        Private Property SPC As New SPCReports
        Private Property Util As New Utilities
        Dim selectChange As Boolean = False
        Dim replist As New List(Of InputArray)
        Dim DefectMasterCache As New List(Of SPCInspection.DefectMasterDisplay)
        'Dim ResultsGraphCache As New List(Of SPCInspection.StackedDefectLineType)
        Dim ScatterGraphCache As New List(Of SPCInspection.InspectionScatterPlot)
        Dim PieChartCache1 As New List(Of SPCInspection.PieTable)
        Dim ProductspecCache As New List(Of SPCInspection.ProductDisplaySpecCollection)
        Dim WorkOrderReportCache As New List(Of SPCInspection.InspectionJobSumarryReport)
        Dim WorkOrderComplianceCache As New List(Of SPCInspection.WorkOrderCompliance)
        Dim Rollheaderlist As New List(Of SPCInspection.RollInspectionSummaryHeaders)
        Dim Rolldetaillist As New List(Of SPCInspection.RollInspectionDetailTable)
        Dim InspectionReports As New InspectionReporting
        Dim LastDefectID As Integer
        Dim lastCachedDefectID As Integer
        Dim WOCList1 As New List(Of SPCInspection.WorkOrderCompliance)
        Dim WOCList2 As New List(Of SPCInspection.WorkOrderCompliance)

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

            Response.Redirect("~/APP/Presentation/InspectionVisualizer.aspx")

            Dim fromdate As DateTime = DateTime.Now.AddDays(-20)
            Dim todate As DateTime = DateTime.Now
            Dim jser As New JavaScriptSerializer
            Dim UserParse As Object = Request.UserAgent
            Dim QueryTemplateid As String = Request.QueryString("LocationId")

            LoadTemplateNames()

            If IsNothing(QueryTemplateid) = False And IsNothing(Request.QueryString("selectChange")) = False Then
                selectChange = True
                SelectedId = Convert.ToInt32(QueryTemplateid)

                TemplateId_Hidden.Value = SelectedId.ToString()
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
                    Response.Cookies("SPCInspectionReporter")("TemplateId_LastSelected") = SelectedId.ToString()
                    Response.Cookies("SPCInspectionReporter")("lastVisit") = DateTime.Now.ToString()
                    Response.Cookies("SPCInspectionReporter").Expires = DateTime.Now.AddDays(60)
                End If

            End If

            fromdatestring = fromdate.ToString("MM/dd/yyyy") + " 00:00:00"
            todatestring = todate.ToString("MM/dd/yyyy") + " 23:59:59"
            If selectChange = False Then
                If Not Request.Cookies("SPCInspectionReporter") Is Nothing Then
                    'Dim TemplateIdCookie As String = Server.HtmlEncode(Request.Cookies("SPCInspectionReporter")("TemplateId_LastSelected")).ToString()
                    Dim TemplateIdCookie As Dictionary(Of String, String) = GetCookie("SPCInspectionReporter", "TemplateId_LastSelected")
                    Dim lastVisitCookie As String = Server.HtmlEncode(Request.Cookies("SPCInspectionReporter")("lastVisit")).ToString()
                    'If IsNumeric(TemplateIdCookie) = True Then
                    '    SelectedId = Convert.ToInt32(TemplateIdCookie)
                    'End If
                    If TemplateIdCookie.Count > 0 Then
                        SelectedId = Convert.ToInt16(TemplateIdCookie("SPCInspectionReporter"))
                    End If
                    TemplateId_Hidden.Value = SelectedId.ToString()
                End If

                '    Dim bmap_col As New BMappers(Of InfoSchema)
                '    Dim listcol As List(Of InfoSchema) = bmap_col.GetInspectObject("select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = N'TemplateName'")
                '    If listcol.Count > 0 Then
                '        Dim colarray = listcol.ToArray()
                '        For Each item In colarray
                '            If item.COLUMN_NAME.Contains("Ins") Then
                '                Dim bmap_tds As New BMappers(Of SingleObject)
                '                Dim tdslist As List(Of SingleObject) = bmap_tds.GetInspectObject("SELECT " & item.COLUMN_NAME & " as Object1 FROM Templatename WHERE TemplateId = " & SelectedId.ToString() & "")
                '                If tdslist.Count > 0 Then
                '                    Dim tdsarray = tdslist.ToArray()
                '                    If tdsarray(0).Object1 = True Then
                '                        replist.Add(New InputArray With {.key = item.COLUMN_NAME, .value = True})

                '                    Else
                '                        replist.Add(New InputArray With {.key = item.COLUMN_NAME, .value = False})
                '                    End If
                '                End If
                '            End If
                '        Next
                '        Dim reportselector As Integer = 1
                '        If replist.Count > 0 Then
                '            ReportList = jser.Serialize(replist)
                '            Dim rearray = replist.ToArray()
                '            For Each item In rearray
                '                Select Case reportselector
                '                    Case 1
                '                        reportname1_hidden.Value = item.key

                '                        ReportLabel1.InnerText = item.key
                '                    Case 2
                '                        reportname2_hidden.Value = item.key

                '                        ReportLabel2.InnerText = item.key
                '                End Select

                '                reportselector += 1
                '            Next
                '        End If
                '    End If
            End If


            HasReports = "True"
            reportstatus2_hidden.Value = "True"
            reportstatus1_hidden.Value = "True"


            HasProductSpecs = False 'DU.CheckSpecTemplate(SelectedId)

            If Page.IsPostBack = True Then
                fromdate = DateTime.Parse(DateFrom_Hidden.Value)
                todate = DateTime.Parse(DateTo_Hidden.Value)
            End If
            LoadPageDataTables(fromdate, todate, SelectedId)




        End Sub
        Dim lastrow As Integer = 0
        'Protected Sub GoButton1_Click(sender As Object, e As System.EventArgs) Handles GoButton.Click

        '    Using epack As New ExcelPackage

        '        Try
        '            Dim workbook As ExcelWorkbook = epack.Workbook

        '            If WorkOrderReportCache.Count > 0 Then
        '                workbook = InspectionReports.InsertInspectionJobSummary(workbook, WorkOrderReportCache, Inspect.LocationNames)
        '            End If

        '            If DefectMasterCache.Count > 0 Then
        '                workbook = SPC.GetDefectMaster(workbook, DefectMasterCache, "DefectMaster")
        '            End If

        '        Catch ex As Exception

        '        End Try

        '        If epack.Workbook.Worksheets.Count = 0 Then
        '            epack.Workbook.Worksheets.Add("New Tab")
        '        End If

        '        Response.Clear()
        '        Response.ClearContent()
        '        Response.ClearHeaders()
        '        Response.Cookies.Clear()
        '        Response.Cache.SetCacheability(HttpCacheability.Private)
        '        Response.CacheControl = "private"
        '        Response.Charset = System.Text.UTF8Encoding.UTF8.WebName
        '        Response.ContentEncoding = System.Text.UTF8Encoding.UTF8
        '        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        '        'Response.ContentType = "application/vnd.ms-excel"
        '        'Response.AddHeader("content-disposition", "attachment;  filename=InspectionReport.xlsx;" + "size=" + epack.GetAsByteArray().Length.ToString() + "; " +
        '        '    "creation-date=" + DateTime.Now.ToString("R") + "; " +
        '        '    "modification-date=" + DateTime.Now.ToString("R") + "; ")
        '        Response.AddHeader("content-disposition", "attachment;  filename=InspectionReport.xlsx;")
        '        Dim stream As MemoryStream = New MemoryStream(epack.GetAsByteArray())

        '        Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)

        '        Response.Flush()

        '        Response.Close()

        '    End Using

        'End Sub

        'Protected Sub GoButton_Click(sender As Object, e As System.EventArgs) Handles GoButton.Click
        '    Dim book As C1XLBook = New C1XLBook()
        '    Dim sheet1 As XLSheet
        '    Dim datefrom As String = Request.Form("datefrom")
        '    Dim dateto As String = Request.Form("dateto")
        '    Dim TemplateIdString = Request.Form("LocationId").Split(",")(0)
        '    Dim TemplateId As Integer
        '    Dim DM As New DefectMaseter
        '    Dim tabledata As String = TableData_Hidden.Value
        '    Dim DataNoString As String = DataNo_Hidden.Value
        '    Dim SpecgridData As String = SpecgridData_Hidden.Value
        '    Dim InspectionId As Integer = Convert.ToInt32(InspectionId_Hidden.Value)
        '    Dim SpecInspectionId As Integer = Convert.ToInt32(SpecgridInspectionId_Hidden.Value)
        '    Dim inputelementarray As List(Of SPCInspection.DefectMasterDisplay)
        '    Dim SpecgridList As New List(Of SPCInspection.ProductDisplaySpecCollection)
        '    Dim filteredArray As List(Of SPCInspection.DefectMasterDisplay)
        '    Dim filterSpecArray As List(Of SPCInspection.ProductDisplaySpecCollection)


        '    If Not (datefrom Is Nothing) And (dateto Is Nothing) And (TemplateIdString Is Nothing) Then
        '        Exit Sub
        '    Else
        '        datefrom = datefrom + " 00:00:00"
        '        dateto = dateto + " 23:59:59"
        '        TemplateId = Convert.ToInt16(TemplateIdString)
        '    End If

        '    book.Load(Server.MapPath(".") + "\Book1.xlsx")
        '    If tabledata <> "" And tabledata <> "0" Then
        '        inputelementarray = jser.Deserialize(Of List(Of SPCInspection.DefectMasterDisplay))(tabledata)

        '        If DataNoString <> "NONE" And DataNoString <> "" And InspectionId <> -1 Then
        '            filteredArray = (From v In inputelementarray Where v.InspectionId = InspectionId And v.DataNo = DataNoString).ToList()
        '        ElseIf DataNoString <> "NONE" And DataNoString <> "" And InspectionId = -1 Then
        '            filteredArray = (From v In inputelementarray Where v.DataNo = DataNoString).ToList()
        '        ElseIf DataNoString = "NONE" And DataNoString = "" And InspectionId <> -1 Then
        '            filteredArray = (From v In inputelementarray Where v.InspectionId = InspectionId).ToList()
        '        Else
        '            filteredArray = inputelementarray
        '        End If
        '        book = DM.LoadExportDataFromList(book, datefrom, dateto, filteredArray)
        '    Else
        '        book = DM.LoadExportData(book, datefrom, dateto, TemplateId, SelectedName, SelectedAb)
        '    End If

        '    If SpecgridData <> "" And SpecgridData <> "[0]" Then
        '        SpecgridList = jser.Deserialize(Of List(Of SPCInspection.ProductDisplaySpecCollection))(SpecgridData)

        '        If SpecInspectionId <> -1 And SpecInspectionId <> 0 Then
        '            filterSpecArray = (From v In SpecgridList Where v.InspectionId = SpecInspectionId).ToList()
        '        Else
        '            filterSpecArray = SpecgridList
        '        End If

        '        book = DM.LoadSpecDataFromList(book, datefrom, dateto, filterSpecArray)
        '    End If

        '    Dim objStream As System.IO.MemoryStream = New System.IO.MemoryStream

        '    book.Save(objStream)

        '    Dim byteArr As Byte() = Array.CreateInstance(GetType(Byte), objStream.Length)
        '    objStream.Position = 0
        '    objStream.Read(byteArr, 0, CType(objStream.Length, Integer))
        '    objStream.Close()
        '    Response.Clear()

        '    If Session("ExcelFormat2007") = True Then
        '        Response.AddHeader("content-disposition", "attachment; filename=InspectionExport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx")
        '    Else
        '        Response.AddHeader("content-disposition", "attachment; filename=InspectionExport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls")
        '    End If
        '    Response.BinaryWrite(byteArr)
        '    Response.End()


        'End Sub

        Protected Sub GetReport1_Click(sender As Object, e As System.EventArgs) Handles GetReport1.Click
            Dim datefrom As String = Request.Form("datefrom")
            Dim dateto As String = Request.Form("dateto")
            Dim fromdate As DateTime = DateTime.Parse(datefrom)
            Dim todate As DateTime = DateTime.Parse(dateto)
            Dim TemplateIdString = Request.Form("TemplateId")
            'Dim TemplateId As Integer = Convert.ToInt16(TemplateIdString)
            Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd") + "." + SelectedId.ToString()
            If Not (datefrom Is Nothing) And (dateto Is Nothing) Then
                Exit Sub
            Else
                datefrom = datefrom + " 00:00:00"
                dateto = dateto + " 23:59:59"
                'TemplateId = Convert.ToInt16(TemplateIdString)
            End If

            SelectedId = CType(CID_Hidden.Value, Integer)
            Using epack As New ExcelPackage
                Dim workbook As ExcelWorkbook = epack.Workbook
                Dim reparray = replist.ToArray()
                Dim ReportName1 As Object = reportname1_hidden.Value
                Dim Report1Status As String = reportstatus1_hidden.Value
                Dim Report2Status As String = reportstatus2_hidden.Value
                'If reparray.Length = 2 Then
                If Report1Status = "True" And check1.Checked = True Then
                    Try
                        Select Case ReportName1
                            Case "Ins_GriegeBatch"
                                workbook = SPC.getGriegeInspectionReport_1(workbook, Rollheaderlist, Rolldetaillist, datefrom, dateto)
                            Case "Ins_WorkOrderInspection"
                                workbook = InspectionReports.InsertInspectionJobSummary(workbook, WorkOrderReportCache, Inspect.LocationNames)
                        End Select
                    Catch ex As Exception

                    End Try

                End If
                If Report2Status = "True" And check2.Checked = True Then
                    Dim ReportName2 As Object = reportname2_hidden.Value
                    Try
                        Select Case ReportName2
                            Case "Ins_GriegeBatch"
                                workbook = SPC.getGriegeInspectionReport_1(workbook, Rollheaderlist, Rolldetaillist, datefrom, dateto)
                            Case "Ins_WorkOrderInspection"
                                workbook = InspectionReports.InsertInspectionJobSummary(workbook, WorkOrderReportCache, Inspect.LocationNames)
                        End Select
                    Catch ex As Exception

                    End Try

                End If
                If check3.Checked = True Then
                    Dim ReportName3 As Object = reportname3_hidden.Value
                    Try
                        workbook = InspectionReports.WorkOrderCompiance2(workbook, WOCList1, WOCList2, SelectedId)
                    Catch ex As Exception

                    End Try

                End If
                If check4.Checked = True Then
                    Dim ReportName4 As Object = reportname4_hidden.Value
                    Try
                        'Dim CacheString2 As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd") + "." + System.Web.HttpContext.Current.Session.SessionID
                        'Dim cacheobj As List(Of SPCInspection.DefectMasterDisplay) = Context.Cache("DefectMasterReport" + CacheString2)
                        'Dim filtcacheobj As List(Of SPCInspection.DefectMasterDisplay)
                        'If SelectedId <> 999 Then
                        '    filtcacheobj = (From v In cacheobj Where v.Location.Trim() = SelectedId.ToString() Select v).ToList()
                        'Else
                        '    filtcacheobj = cacheobj
                        'End If

                        If DefectMasterCache.Count > 0 Then
                            workbook = SPC.GetDefectMaster(workbook, DefectMasterCache, "DefectMaster")
                        End If
                    Catch ex As Exception

                    End Try

                End If
                If check5.Checked = True Then
                    Dim ReportName4 As Object = reportname5_hidden.Value
                    Try
                        Dim Speccacheobj As List(Of SPCInspection.ProductDisplaySpecCollection) = Context.Cache("SpecMasterLoad" + CacheString)
                        Dim filtcacheobj As List(Of SPCInspection.ProductDisplaySpecCollection)
                        If SelectedId <> 999 Then
                            filtcacheobj = (From v In Speccacheobj Where v.Location.Trim() = SelectedId.ToString() Select v).ToList()
                        Else
                            filtcacheobj = Speccacheobj
                        End If
                        If Speccacheobj.Count > 0 Then
                            workbook = InspectionReports.GetProductSpecDisplay(workbook, filtcacheobj)
                        End If
                    Catch ex As Exception

                    End Try

                End If
                'End If

                If workbook.Worksheets.Count = 0 Then
                    workbook.Worksheets.Add("Sheet1")
                End If
                Response.Clear()
                Response.ClearContent()
                Response.ClearHeaders()
                Response.Cookies.Clear()
                Response.Cache.SetCacheability(HttpCacheability.Private)
                Response.CacheControl = "private"
                Response.Charset = System.Text.UTF8Encoding.UTF8.WebName
                Response.ContentEncoding = System.Text.UTF8Encoding.UTF8
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;  filename=InspectionReport.xlsx;")
                Dim stream As MemoryStream = New MemoryStream(epack.GetAsByteArray())

                Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)

                Response.Flush()

                Response.Close()
            End Using

        End Sub
        'Protected Sub GoImage_Click(sender As Object, e As System.EventArgs) Handles GetImage.Click
        '    Dim ImageBytes As Byte()
        '    Dim DefectId_Value As Integer
        '    Dim ImageList As Array

        '    If IsNumeric(DefectId_Value_Hidden.Value) = True Then
        '        DefectId_Value = DefectId_Value_Hidden.Value

        '        If IsNothing(Inspect.GetDefectImage(DefectId_Value)) = False Then
        '            ImageList = Inspect.GetDefectImage(DefectId_Value).ToArray()

        '            If ImageList.Length <> 0 Then
        '                ImageBytes = ImageList(0).DefectImage
        '                Response.Clear()
        '                Response.AddHeader("content-disposition", "attachment; filename=" & Convert.ToString(ImageList(0).DefectImage_Filename))
        '                Response.BinaryWrite(ImageBytes)
        '                Response.End()
        '                Exit Sub
        '            End If

        '        End If

        '    End If



        'End Sub

        Protected Sub GoDate_Click(sender As Object, e As System.EventArgs) Handles GODate.Click
            Dim datefrom As String = Request.Form("datefrom")
            Dim dateto As String = Request.Form("dateto")
            'Dim TemplateIdString = Request.Form("TemplateId")
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
                    'SelectId_Hidden.Value = SelectedId.ToString()
                End If
            End If
            LoadTemplateNames()
            LoadPageDataTables(fromdatetime, todatetime, SelectedId)

            fromdatestring = fromdatetime.ToString("MM/dd/yyyy")
            todatestring = todatetime.ToString("MM/dd/yyyy")

        End Sub

        Private Sub LoadPageDataTables(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Locationid As Integer)
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Dim expired As DateTime = Now.AddDays(7)
            Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd") + "." + SelectedId.ToString()
            Dim countwoobj As Object = 0
            Dim counttdobj As Object = 0
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Try
                'check cache first for defect objects on the date range and location.
                If IsNumeric(CID_Hidden.Value) = True And CID_Hidden.Value <> "0" Then
                    SelectedId = CType(CID_Hidden.Value, Integer)
                End If
                Dim results = (From x In Inspect.LocationNames Where x.CID = SelectedId.ToString() Select x.text).ToArray()
                If SelectedId = 999 Then
                    listso = bmapso.GetInspectObject("SELECT TOP(1) DefectID AS Object1 FROM DefectMaster WHERE DefectTime >= CAST( '" & fromdatestring & "' AS DATETIME) AND DefectTime <= CAST( '" & todatestring & "' AS DATETIME) ORDER BY DefectID DESC")
                Else
                    listso = bmapso.GetInspectObject("SELECT TOP(1) DefectID AS Object1 FROM DefectMaster WHERE Location = '" & SelectedId.ToString() & "' AND DefectTime >= CAST( '" & fromdatestring & "' AS DATETIME) AND DefectTime <= CAST( '" & todatestring & "' AS DATETIME) ORDER BY DefectID DESC")
                End If

                If listso.Count > 0 Then
                    LastDefectID = CType(listso.ToArray()(0).Object1, Integer)
                Else
                    LastDefectID = 0
                End If
                Dim CacheObjDefectID = Context.Cache("LastDefectID" + CacheString)
                If CacheObjDefectID <> Nothing Then
                    lastCachedDefectID = CType(CacheObjDefectID, Integer)
                End If
                SelectedName = results(0).ToString().Trim()

                Dim abresults = (From x In Inspect.LocationNames Where x.CID = SelectedId.ToString() Select x.Abreviation).ToArray()
                Dim cidresults = (From x In Inspect.LocationNames Where x.CID = SelectedId.ToString() Select x.CID).ToArray()
                SelectedAb = abresults(0).ToString().Trim()

                'If selectChange = True Then
                '    DefectMasterCache = Cache("DefectMaster" + CacheString)
                '    ResultsGraphCache = Cache("ResultsGraph" + CacheString)
                '    ProductspecCache = Cache("ProductSpec" + CacheString)
                '    WorkOrderReportCache = Cache("WorkOrderReport" + CacheString)
                '    ScatterGraphCache = Cache("ScatterGraph" + CacheString)
                '    Rolldetaillist = Cache("RollDetaillist" + CacheString)
                '    Rollheaderlist = Cache("Rollheaderlist" + CacheString)
                'Else
                'DefectMasterCache = Nothing
                'ResultsGraphCache = Nothing
                ProductspecCache = Nothing
                WorkOrderReportCache = Nothing
                Rollheaderlist = Nothing
                Rolldetaillist = Nothing
                ScatterGraphCache = Nothing
                'End If
                Dim ExportFlag As Object = ExportFlag_Hidden.Value
                Dim ceck1 As Object = check1.Checked
                Dim ceck2 As Object = check2.Checked
                Dim ceck3 As Object = check3.Checked


                'If DefectMasterCache Is Nothing Then
                '    DefectMasterCache = Inspect.GetDefectMasterData(fromdate, todate, SelectedId.ToString(), SelectedAb)

                '    If DefectMasterCache.Count > 0 Then
                '        Context.Cache.Insert("DefectMaster" + CacheString, DefectMasterCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                '    End If
                'End If
                If check1.Checked = True Then
                    Dim CacheObjDefectID1 = Context.Cache("RollheaderlistLastDefectID" + CacheString)

                    'If Not CacheObjDefectID1 Is Nothing Then
                    '    If CacheObjDefectID1 = LastDefectID Then
                    '        Rollheaderlist = Context.Cache("Rollheaderlist" + CacheString)
                    '        GoTo 1011
                    '    End If
                    Rollheaderlist = Inspect.GetRollInspectionSummaryHeaders(fromdate, todate)
                    Rolldetaillist = Inspect.GetRollInspectionDetailTable(fromdate, todate)
                    'If Rollheaderlist.Count > 0 Then
                    '    Context.Cache.Insert("Rollheaderlist" + CacheString, Rollheaderlist, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    '    Context.Cache.Insert("RollheaderlistLastDefectID" + CacheString, LastDefectID, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    'End If
                End If
1011:
                'Dim CacheObjDefectID2 = Context.Cache("RollDetaillistLastDefectID" + CacheString)

                'If Rolldetaillist Is Nothing And CacheObjDefectID2 = LastDefectID Then
                '    Rolldetaillist = Inspect.GetRollInspectionDetailTable(fromdate, todate)

                '    If Rolldetaillist.Count > 0 Then
                '        Context.Cache.Insert("RollDetaillist" + CacheString, Rolldetaillist, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                '        Context.Cache.Insert("RollDetaillistLastDefectID" + CacheString, LastDefectID, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                '    End If
                'End If
                'End If
1012:
                If check2.Checked = True Then
                    Dim CacheObjDefectID1 = Context.Cache("WorkOrderReportLastDefectID" + CacheString)

                    If Not CacheObjDefectID1 Is Nothing Then
                        If CacheObjDefectID1 = LastDefectID Then
                            WorkOrderReportCache = Context.Cache("WorkOrderReport" + CacheString)
                            GoTo 102
                        End If
                    End If
                    WorkOrderReportCache = InspectionReports.GetInspectionJobSummary(fromdate, todateform, SelectedId, Inspect.LocationNames)

                    If WorkOrderReportCache.Count > 0 Then
                        Context.Cache.Insert("WorkOrderReport" + CacheString, WorkOrderReportCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                        Context.Cache.Insert("WorkOrderReportLastDefectID" + CacheString, LastDefectID, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)

                    End If

                End If
102:
                If check3.Checked = True Then
                    Dim CacheObjDefectID1 = Context.Cache("WorkOrderComplianceLastDefectID1" + CacheString)

                    If Not CacheObjDefectID1 Is Nothing Then
                        If CacheObjDefectID1 = LastDefectID Then
                            WOCList1 = Context.Cache("WorkOrderCompliance1" + CacheString)
                            GoTo 103
                        End If
                    End If
                    Dim sql1 As String
                    Dim bmapwoc As New BMappers(Of SPCInspection.WorkOrderCompliance)
                    If SelectedId = 999 Then
                        sql1 = "select *, cast(A.ItemFailCount as decimal) / CAST(A.TotalItemsInspected as decimal) * 100 AS DHUAVG FROM(" & vbCrLf &
                                    "SELECT JobNumber as WorkOrder_Inspected, MIN(Inspection_Started) as Inspection_Started, SUM(TotalInspectedItems) AS TotalItemsInspected, SUM(ItemFailCount) AS ItemFailCount FROM InspectionJobSummary" & vbCrLf &
                                    "WHERE (JobType = 'WorkOrder') AND (TotalInspectedItems is not null) AND (TotalInspectedItems > 0) AND (Inspection_Started >= DATEADD(m, DATEDIFF(m, 0, '" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "'), 0)) AND (Inspection_Started <= DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,'" & todate.ToString("yyyy-MM-dd H:mm:ss") & "')+1,0))) GROUP BY JobNumber) AS A"
                    Else
                        sql1 = "select *, cast(A.ItemFailCount as decimal) / CAST(A.TotalItemsInspected as decimal) * 100 AS DHUAVG FROM(" & vbCrLf &
                                    "SELECT JobNumber as WorkOrder_Inspected, MIN(Inspection_Started) as Inspection_Started, SUM(TotalInspectedItems) AS TotalItemsInspected, SUM(ItemFailCount) AS ItemFailCount FROM InspectionJobSummary" & vbCrLf &
                                    "WHERE (JobType = 'WorkOrder') AND (CID = '" & SelectedId.ToString() & "') AND (TotalInspectedItems is not null) AND (TotalInspectedItems > 0) AND (Inspection_Started >= DATEADD(m, DATEDIFF(m, 0, '" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "'), 0)) AND (Inspection_Started <= DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,'" & todate.ToString("yyyy-MM-dd H:mm:ss") & "')+1,0))) GROUP BY JobNumber) AS A"
                    End If
                    WOCList1 = bmapwoc.GetInspectObject(sql1)
                    If WOCList1.Count > 0 Then
                        Context.Cache.Insert("WorkOrderCompliance1" + CacheString, WOCList1, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                        Context.Cache.Insert("WorkOrderComplianceLastDefectID1" + CacheString, LastDefectID, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
103:
                    Dim CacheObjDefectID2 = Context.Cache("WorkOrderComplianceLastDefectID2" + CacheString)

                    If Not CacheObjDefectID2 Is Nothing Then
                        If CacheObjDefectID2 = LastDefectID Then
                            WOCList2 = Context.Cache("WorkOrderCompliance2" + CacheString)
                            GoTo 104
                        End If
                    End If
                    WOCList2 = Inspect.Getas400WOByBranch(fromdate, todate, SelectedId)
                    If WOCList1.Count > 0 Then
                        Context.Cache.Insert("WorkOrderCompliance2" + CacheString, WOCList1, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                        Context.Cache.Insert("WorkOrderComplianceLastDefectID2" + CacheString, LastDefectID, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
104:
                End If

                If check4.Checked = True Then
                    DefectMasterCache = Inspect.GetDefectMasterData2(fromdate, todate, SelectedId)
                End If

                'If Not DefectMasterCache Is Nothing Then
                '    countwoobj = (From x In DefectMasterCache Select x.WorkOrder).Distinct().Count()
                '    counttdobj = (From x In DefectMasterCache Where x.DataType <> "InspectionStart" Select x.DefectID).Count()
                'End If
                'If ResultsGraphCache Is Nothing Then
                '    Dim LocmId = (From x In Inspect.LocationNames Where x.CID = SelectedId.ToString() Select x.id).ToArray()
                '    If LocmId.Length > 0 Then
                '        LocationMasterId = LocmId(0)
                '    End If

                '    ResultsGraphCache = Inspect.GetStackedDefectLineType(fromdate, todateform, LocationMasterId, cidresults(0).ToString())

                '    If ResultsGraphCache.Count > 0 Then
                '        Context.Cache.Insert("ResultsGraph" + CacheString, ResultsGraphCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                '    End If
                'End If
                'If ResultsGraphCache Is Nothing Then
                '    Dim LocmId = (From x In Inspect.LocationNames Where x.CID = SelectedId.ToString() Select x.id).ToArray()
                '    If LocmId.Length > 0 Then
                '        LocationMasterId = LocmId(0)
                '    End If

                '    ResultsGraphCache = Inspect.GetMainResultsGraph(0, fromdate, todateform, LocationMasterId, cidresults(0).ToString())

                '    If ResultsGraphCache.Count > 0 Then
                '        Context.Cache.Insert("ResultsGraph" + CacheString, ResultsGraphCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                '    End If
                'End If

                'If ProductspecCache Is Nothing Then
                '    ProductspecCache = DU.GetDisplaySpecCollection(SelectedId.ToString())
                'End If

                'If ProductspecCache.Count > 0 Then
                '    Context.Cache.Insert("ProductSpec" + CacheString, ProductspecCache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                'End If

                jser.MaxJsonLength = Int32.MaxValue
                'TemplateDefectMaster = jser.Serialize(DefectMasterCache)
                'DefectMasterHistogram = jser.Serialize(ResultsGraphCache)
                'ProductDisplaySpecCollection = jser.Serialize(ProductspecCache)
                'InspectStartCount = Inspect.GetDefectMasterDataTypeCount(Locationid, fromdate, todateform, "InspectionStart")


                Dim dilist As New List(Of SPCInspection.DefectImageDisplay)
                Dim bmap_di As New BMappers(Of SPCInspection.DefectImageDisplay)
                Dim fromdatestr As String = fromdate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 00:00:00"
                Dim todatestr As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"

                If Locationid = 999 Then
                    dilist = bmap_di.GetInspectObject("SELECT dm.DefectID, dm.DefectTime, dm.DefectDesc, dm.WorkOrder, lm.Name AS Location, dm.DefectImage_Filename FROM  DefectMaster dm INNER JOIN AprManager.dbo.LocationMaster lm ON lm.CID = '000' + dm.Location WHERE  (LEN(dm.DefectImage) > 0) AND (dm.DefectTime >= CONVERT(DATETIME, '" & fromdatestr & "', 102)) AND (dm.DefectTime <= CONVERT(DATETIME, '" & todatestr & "', 102))")
                Else
                    dilist = bmap_di.GetInspectObject("SELECT DefectID, DefectTime, DefectDesc, WorkOrder, Location, DefectImage_Filename FROM  DefectMaster WHERE  (LEN(DefectImage) > 0) AND (DefectTime >= CONVERT(DATETIME, '" & fromdatestr & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestr & "', 102)) AND Location = '" & Locationid.ToString() & "'")
                End If


                If dilist.Count > 0 Then
                    Try
                        Dim cilist As New List(Of CarouselImage)
                        Dim diarray = dilist.ToArray()

                        For Each item In diarray
                            cilist.Add(New CarouselImage With {.imageUrl = Session("BaseUri") + "/APP/Utility/GetDefectImage.aspx?DefectID=" + item.DefectID.ToString(), .linkUrl = Session("BaseUri") + "/APP/Utility/GetDefectImage.aspx?DefectID=" + item.DefectID.ToString(), .content = "", .caption = "<span>" & item.DefectDesc & " AT " & item.Location & ", Time: " & item.DefectTime.ToString("yy-MM-dd hh:mm") + "</span>"})

                        Next
                        DefectImageDisplayArray = jser.Serialize(cilist)
                    Catch ex As Exception

                    End Try

                End If

            Catch ex As Exception

            End Try

            'If IsNumeric(counttdobj) = True And IsNumeric(countwoobj) = True And countwoobj > 0 Then
            '    DefectsPerWorkOrder = counttdobj / countwoobj
            'End If

            'Dim statlst As New List(Of InputArray)

            'statlst.Add(New InputArray With {.key = "TotalDefects", .value = counttdobj})
            'statlst.Add(New InputArray With {.key = "WorkOrderCount", .value = countwoobj})
            'statlst.Add(New InputArray With {.key = "DefectsPerWorkOrder", .value = DefectsPerWorkOrder.ToString("N2")})

            'InspectStats = jser.Serialize(statlst.ToArray())
        End Sub

        Private Sub LoadChart2Titles()

            Dim sql As String

            If SelectedId <> 999 Then
                sql = "SELECT DISTINCT TOP(4) tm.Name as Object1, js.TemplateId as Object2, COUNT(js.id) as Object3" & vbCrLf &
                    "FROM InspectionJobSummary js INNER JOIN TemplateName tm ON js.TemplateId = tm.TemplateId" & vbCrLf &
                    "WHERE js.CID = '" & SelectedId.ToString() & "' and tm.LineType <> 'ROLL' and js.Inspection_Started > = '" & fromdatestring & "' and js.Inspection_Started <= '" & todatestring & "'" & vbCrLf &
                    "GROUP BY js.TemplateId, tm.Name" & vbCrLf &
                    "ORDER BY Object3 desc"
            Else
                sql = "SELECT DISTINCT TOP(4) tm.Name as Object1, js.TemplateId as Object2, COUNT(js.id) as Object3" & vbCrLf &
                    "FROM InspectionJobSummary js INNER JOIN TemplateName tm ON js.TemplateId = tm.TemplateId" & vbCrLf &
                    "WHERE tm.LineType <> 'ROLL' and js.Inspection_Started > = '" & fromdatestring & "' and js.Inspection_Started <= '" & todatestring & "'" & vbCrLf &
                    "GROUP BY js.TemplateId, tm.Name" & vbCrLf &
                    "ORDER BY Object3 desc"
            End If
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)

            listso = bmapso.GetInspectObject(sql)

            Chart2Array = jser.Serialize(listso)
        End Sub

        Private Sub LoadTemplateNames()
            Dim TemplateArray As Array = Inspect.GetLocations(True).ToArray()
            If selectChange = False Then
                SelectedId = Inspect.NEWALLID
            End If
            Dim serlist As New List(Of selector2array)(TemplateArray)
            Dim cnt As Integer = 0

            LocationNames = jser.Serialize(Inspect.LocationNames)
            
            'LocationNames = jser.Serialize(serlist.ToArray())

            If LocationNames Is Nothing Then
                Response.Redirect("~/ErrorPage.aspx")
            End If

        End Sub

        'Private Function AddSPCReporterExport(ByVal selectlist As List(Of selector2array)) As List(Of selector2array)
        '    Dim CIDList As List(Of CID) = Me.Session("CID_Info")

        '    If CIDList.Count > 0 Then
        '        Dim CIDArray = CIDList.ToArray()
        '        If CIDArray(0).CorporateName = "STC Thomaston Plant" Then

        '        End If
        '    End If

        '    Return returnlist
        'End Function

        Private Function tdsarray(p1 As String) As Boolean
            Throw New NotImplementedException
        End Function

    End Class


End Namespace
