<%@ WebHandler Language="VB" Class="core.SPC_InspectionVisualizer" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports Newtonsoft.Json
Namespace core

    Public Class SPC_InspectionVisualizer
        Inherits BaseHandler

        Dim jser As New JavaScriptSerializer()
        Dim Inspect As New InspectionUtilityDAO

        Public Function GetDHU_Scat() As String
            Dim _todate As DateTime = DateTime.Now
            Dim _formdate As DateTime = DateTime.Now.AddDays(-14)
            'Dim _todate As DateTime = DateTime.Parse(Todate)
            'Dim _formdate As DateTime = DateTime.Parse(Fromdate)

            Dim listds As New List(Of SPCInspection.InspectionScatterPlot)
            listds = Inspect.GetScatterPlotData(_todate, _formdate, "999")

            Return jser.Serialize(listds)
        End Function
        Public Function DrawWRChart(ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal wr As String, ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.DrawWRChart(fac, gt, tp, wr, from, toDate, DN, WO, AT))


        End Function
        Public Function DrillDownWR(ByVal dt As String, ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal wr As String, ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.DrillDownWR(dt, fac, gt, tp, wr, from, toDate, DN, WO, AT))


        End Function
        Public Function DrillDown(ByVal dt As String, ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.DrillDown(dt, fac, gt, tp, from, toDate, DN, WO, AT))


        End Function
        Public Function DrawChart(ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.DrawChart(fac, gt, tp, from, toDate, DN, WO, AT))


        End Function
        Public Function GetOverallDataArray(ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.GetOverallDataArray(from, toDate, DN, WO, AT))



        End Function
        Public Function GetInteriorsDataArray(ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.GetInteriorsDataArray(from, toDate, DN, WO, AT))



        End Function
        Public Function GetDomesticDataArray(ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.GetDomesticDataArray(from, toDate, DN, WO, AT))



        End Function
        Public Function GetGlobalDataArray(ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String



            Return JsonConvert.SerializeObject(Inspect.GetGlobalDataArray(from, toDate, DN, WO, AT))



        End Function
        Public Function GetDataArray(ByVal array As List(Of Integer), ByVal from As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As String
            If array IsNot Nothing Then


                Return JsonConvert.SerializeObject(Inspect.GetDataArray(array, from, toDate, DN, WO, AT))
            Else
                Return Nothing
            End If

        End Function
        Public Function GetStackedDefectLineType(ByVal fromdate As String, ByVal todate As String, ByVal DataNo As String, ByVal AuditType As String, ByVal LocArray As String) As String
            Dim _todate As DateTime = DateTime.Parse(todate)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)
            Dim listds As New List(Of BarChartObject)
            Dim listts As New List(Of SingleObject)
            Dim bampts As New BMappers(Of SingleObject)
            Dim AuditTypeString As String = ""
            Dim inputelementarray = jser.Deserialize(Of List(Of ActiveLocations))(LocArray)
            _todate = _todate.AddHours(23)
            _todate = _todate.AddMinutes(58)

            listds = Inspect.GetStackedDefectLineType2(_formdate, _todate, DataNo, AuditType.ToUpper(), inputelementarray)

            If AuditType <> "ALL" Then
                AuditTypeString = " WHERE Abreviation = '" & AuditType & "' "
            End If

            listts = bampts.GetInspectObject("select Name as Object1, Abreviation as Object3 FROM InspectionTypes " & AuditTypeString & " order by id asc")

            Return jser.Serialize(listds) + "%%%" + jser.Serialize(listts)

        End Function

        Public Function GetScatterPlotData(ByVal fromdate As String, ByVal todate As String, ByVal cid As String) As String
            Dim _todate As DateTime = DateTime.Parse(todate)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)
            Dim ScatterGraphCache As New List(Of SPCInspection.InspectionScatterPlot)

            ScatterGraphCache = Inspect.GetScatterPlotData(_todate, _formdate, cid)

            Return jser.Serialize(ScatterGraphCache) + "%%%" + LoadChart2Titles(_formdate, _todate, cid)

        End Function

        Public Function GetLocationDataNoFilter(ByVal LocArray As List(Of Integer)) As Object

            Dim retstrg As String = "AND ("


            For index As Integer = 0 To LocArray.Count - 1
                retstrg = retstrg + "CID ='" & LocArray(index) & "'"
                If index <> LocArray.Count - 1 Then
                    retstrg = retstrg + " OR "
                End If

            Next
            retstrg = retstrg + ")"




            Return retstrg
        End Function
        Public Function IVGetLocationDataNoFilter(ByVal LocArray As List(Of ActiveLocations)) As Object

            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" Then
                        retstrg = retstrg + " AND (InspectionJobSummary.CID <> N'" & item.CID & "')"
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg
        End Function
        Public Function IVGetWorkOrders(ByVal fromdate As String, ByVal todate As String, ByVal LocArray As String, ByVal AuditType As String) As String

            Dim returnobj As New List(Of selectorobject)
            Dim bmapso As New BMappers(Of selectorobject)
            Dim _todate As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)
            Dim inputelementarray = jser.Deserialize(Of List(Of ActiveLocations))(LocArray)
            Dim Sql As String
            Dim locsql As String = ""
            Dim auditsql As String = ""

            locsql = IVGetLocationDataNoFilter(inputelementarray)

            If AuditType <> "ALL" Then

                auditsql = "(it.Name = N'" & AuditType.ToUpper().Trim & "') AND "

            End If

            Sql = "SELECT DISTINCT InspectionJobSummary.JobNumber AS id, InspectionJobSummary.UnitDesc as text FROM InspectionJobSummary INNER JOIN" & vbCrLf &
                    "TemplateName ON InspectionJobSummary.TemplateId = TemplateName.TemplateId INNER JOIN InspectionTypes it ON it.id = TemplateName.LineTypeId WHERE " & auditsql & " (InspectionJobSummary.Inspection_Started >= cast( '" & _formdate.ToString("yyyy-MM-dd") & "' as datetime)) AND LEN(InspectionJobSummary.UnitDesc) > 0 AND (InspectionJobSummary.Inspection_Started <= cast('" & _todate.ToString("yyyy-MM-dd") & "' as datetime)) " + locsql
            returnobj = bmapso.GetInspectObject(Sql)

            Return jser.Serialize(returnobj)

        End Function
        Public Function IVGetDataNos(ByVal fromdate As String, ByVal todate As String, ByVal LocArray As String, ByVal AuditType As String) As String

            Dim returnobj As New List(Of selectorobject)
            Dim bmapso As New BMappers(Of selectorobject)
            Dim _todate As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)
            Dim inputelementarray = jser.Deserialize(Of List(Of ActiveLocations))(LocArray)
            Dim Sql As String
            Dim locsql As String = ""
            Dim auditsql As String = ""

            locsql = IVGetLocationDataNoFilter(inputelementarray)

            If AuditType <> "ALL" Then

                auditsql = "(it.Name = N'" & AuditType.ToUpper().Trim & "') AND "

            End If

            Sql = "SELECT DISTINCT InspectionJobSummary.DataNo AS id, InspectionJobSummary.UnitDesc as text FROM InspectionJobSummary INNER JOIN" & vbCrLf &
                    "TemplateName ON InspectionJobSummary.TemplateId = TemplateName.TemplateId INNER JOIN InspectionTypes it ON it.id = TemplateName.LineTypeId WHERE " & auditsql & " (InspectionJobSummary.Inspection_Started >= cast( '" & _formdate.ToString("yyyy-MM-dd") & "' as datetime)) AND LEN(InspectionJobSummary.UnitDesc) > 0 AND (InspectionJobSummary.Inspection_Started <= cast('" & _todate.ToString("yyyy-MM-dd") & "' as datetime)) " + locsql
            returnobj = bmapso.GetInspectObject(Sql)

            Return jser.Serialize(returnobj)

        End Function
        Public Function GetWorkOrders(ByVal fromdate As String, ByVal todate As String, ByVal LocArray As List(Of Integer), ByVal AuditType As String) As String

            Dim returnobj As New List(Of selectorobject)
            Dim bmapso As New BMappers(Of selectorobject)
            Dim _todate As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)

            Dim Sql As String
            Dim locsql As String = ""
            Dim auditsql As String = ""

            locsql = GetLocationDataNoFilter(LocArray)

            If AuditType <> "ALL" Then

                auditsql = "(InspectionType = N'" & AuditType.ToUpper().Trim & "') AND "

            End If

            'Sql = "SELECT DISTINCT InspectionJobSummary.DataNo AS id, InspectionJobSummary.UnitDesc as text FROM InspectionJobSummary INNER JOIN" & vbCrLf &
            '"TemplateName ON InspectionJobSummary.TemplateId = TemplateName.TemplateId INNER JOIN InspectionTypes it ON it.id = TemplateName.LineTypeId WHERE " & auditsql & " (InspectionJobSummary.Inspection_Started >= cast( '" & _formdate.ToString("yyyy-MM-dd") & "' as datetime)) AND LEN(InspectionJobSummary.UnitDesc) > 0 AND (InspectionJobSummary.Inspection_Started <= cast('" & _todate.ToString("yyyy-MM-dd") & "' as datetime)) " + locsql
            Sql = "Select DISTINCT JobNumber as id from dbo.InspectionJobSummaryYearly WHERE " & auditsql & " Inspection_Finished BETWEEN cast( '" & _formdate.ToString("yyyy-MM-dd") & "' as datetime) AND cast('" & _todate.ToString("yyyy-MM-dd") & "' as datetime) " + locsql
            returnobj = bmapso.GetInspectObject(Sql)

            Return jser.Serialize(returnobj)

        End Function
        Public Function GetDataNos(ByVal fromdate As String, ByVal todate As String, ByVal LocArray As List(Of Integer), ByVal AuditType As String) As String

            Dim returnobj As New List(Of selectorobject)
            Dim bmapso As New BMappers(Of selectorobject)
            Dim _todate As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)

            Dim Sql As String
            Dim locsql As String = ""
            Dim auditsql As String = ""

            locsql = GetLocationDataNoFilter(LocArray)

            If AuditType <> "ALL" Then

                auditsql = "(InspectionType = N'" & AuditType.ToUpper().Trim & "') AND "

            End If

            'Sql = "SELECT DISTINCT InspectionJobSummary.DataNo AS id, InspectionJobSummary.UnitDesc as text FROM InspectionJobSummary INNER JOIN" & vbCrLf &
            '"TemplateName ON InspectionJobSummary.TemplateId = TemplateName.TemplateId INNER JOIN InspectionTypes it ON it.id = TemplateName.LineTypeId WHERE " & auditsql & " (InspectionJobSummary.Inspection_Started >= cast( '" & _formdate.ToString("yyyy-MM-dd") & "' as datetime)) AND LEN(InspectionJobSummary.UnitDesc) > 0 AND (InspectionJobSummary.Inspection_Started <= cast('" & _todate.ToString("yyyy-MM-dd") & "' as datetime)) " + locsql
            Sql = "Select DISTINCT DataNo as id from dbo.InspectionJobSummaryYearly WHERE " & auditsql & " Inspection_Finished BETWEEN cast( '" & _formdate.ToString("yyyy-MM-dd") & "' as datetime) AND cast('" & _todate.ToString("yyyy-MM-dd") & "' as datetime) " + locsql
            returnobj = bmapso.GetInspectObject(Sql)

            Return jser.Serialize(returnobj)

        End Function

        Public Function GetDHULineChart(ByVal todate As String, ByVal fromdate As String, ByVal DataNo As String, ByVal AuditType As String, ByVal LocArray As String) As String
            Dim _todate As DateTime = DateTime.Parse(todate)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)
            Dim LocationLineChartCache As New List(Of SPCInspection.LocationLineChart)
            Dim inputelementarray = jser.Deserialize(Of List(Of ActiveLocations))(LocArray)
            _todate = _todate.AddHours(23)
            _todate = _todate.AddMinutes(58)

            LocationLineChartCache = Inspect.GetDHUByLocation(_todate, _formdate, DataNo, AuditType.ToUpper(), inputelementarray)

            Return jser.Serialize(LocationLineChartCache) + "%%%" + LoadResultsChartTitles(_formdate, _todate, DataNo, AuditType.ToUpper(), inputelementarray)

        End Function
        Public Function GetREJLineChart(ByVal fromdate As String, ByVal todate As String) As String
            Dim _todate As DateTime = DateTime.Parse(todate)
            Dim _formdate As DateTime = DateTime.Parse(fromdate)
            Dim LocationLineChartCache As New List(Of SPCInspection.LocationLineChart)

            LocationLineChartCache = Inspect.GetREJByLocation(_todate, _formdate)

            Return jser.Serialize(LocationLineChartCache) + "%%%" '+ LoadResultsChartTitles(_formdate, _todate)

        End Function
        Private Function LoadChart2Titles(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal cid As String) As String

            Dim sql As String
            If DateDiff(DateInterval.Day, fromdate, todate) < 7 Then
                fromdate = todate.AddDays(-7)
            End If

            If cid <> "999" Then
                sql = "SELECT DISTINCT TOP(4) tm.Name as Object1, js.TemplateId as Object2, COUNT(js.id) as Object3" & vbCrLf &
                    "FROM InspectionJobSummary js INNER JOIN TemplateName tm ON js.TemplateId = tm.TemplateId" & vbCrLf &
                    "WHERE js.CID = '" & cid & "' and js.Inspection_Started > = cast( '" & fromdate.ToString("yyyy-MM-dd") & "' as datetime) and js.Inspection_Started <= cast( '" & todate.ToString("yyyy-MM-dd") & "' as datetime)" & vbCrLf &
                    "GROUP BY js.TemplateId, tm.Name" & vbCrLf &
                    "ORDER BY Object3 desc"
            Else
                sql = "SELECT DISTINCT TOP(4) tm.Name as Object1, js.TemplateId as Object2, COUNT(js.id) as Object3" & vbCrLf &
                    "FROM InspectionJobSummary js INNER JOIN TemplateName tm ON js.TemplateId = tm.TemplateId" & vbCrLf &
                    "WHERE js.Inspection_Started > = cast('" & fromdate.ToString("yyyy-MM-dd") & "' as datetime ) and js.Inspection_Started <= cast('" & todate.ToString("yyyy-MM-dd") & "' as datetime)" & vbCrLf &
                    "GROUP BY js.TemplateId, tm.Name" & vbCrLf &
                    "ORDER BY Object3 desc"
            End If
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)

            listso = bmapso.GetInspectObject(sql)

            If listso.Count = 0 Then
                listso.Add(New SingleObject With {.Object1 = "NoTemplates", .Object2 = 1, .Object3 = "1"})
            End If

            Return jser.Serialize(listso)
        End Function

        Private Function LoadResultsChartTitles(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal DataNo As String, ByVal AuditType As String, ByVal LocArray As List(Of ActiveLocations)) As String

            Dim sql As String
            Dim filter1sql As String = ""
            Dim filter2sql As String = ""
            If DataNo <> "ALL" Then
                filter1sql = " dm.DataNo = '" & DataNo & "' AND "
            End If
            If AuditType <> "ALL" Then
                filter2sql = " tn.LineType = '" & AuditType & "' AND "
            End If

            todate = todate.AddDays(1)

            sql = "SELECT  distinct lm.Abreviation as Object1, COUNT(dm.DefectID) as Object3 FROM AprManager.dbo.LocationMaster lm " & vbCrLf &
                    "INNER JOIN dbo.DefectMaster dm ON dm.Location = cast(lm.CID as int) INNER JOIN dbo.TemplateName tn ON dm.TemplateId = tn.TemplateId" & vbCrLf &
                    "WHERE dm.DefectTime > = '" & fromdate.ToString("yyyy-MM-dd") & "' and tn.LineType <>  'ROLL' and dm.DefectTime <= '" & todate.ToString("yyyy-MM-dd") & "' and " & filter1sql & filter2sql & Inspect.GetLocationMasterFilter(LocArray) & " lm.InspectionResults = 1" & vbCrLf &
                    "GROUP BY lm.Abreviation, cast(lm.CID as int)" & vbCrLf &
                    "ORDER BY Object3 desc"

            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)

            listso = bmapso.GetInspectObject(sql)

            If listso.Count = 0 Then
                listso.Add(New SingleObject With {.Object1 = "NoTemplates", .Object2 = 1, .Object3 = "1"})
            End If

            Return jser.Serialize(listso)
        End Function

        Public Function GetDefectImageDescList(ByVal fromdate As String, ByVal todate As String) As String
            Dim returnstring As String = ""
            Dim dmobj As New List(Of SPCInspection.DefectImageDisplay_)

            Try

                Dim fromdatedt As DateTime = DateTime.Parse(fromdate)
                Dim todatedt As DateTime = DateTime.Parse(todate)

                dmobj = Inspect.GetDefectImageDescList(fromdatedt, todatedt)
                returnstring = jser.Serialize(dmobj)
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                returnstring = "-1:" + ex.Message
            End Try

            Return returnstring
        End Function
        Public Function GetInspectionImageBase64(ByVal fromdate As String, ByVal todate As String, ByVal ActiveLocationStr As String, ActiveFilterStr As String) As String
            Dim returnstring As String = ""
            Dim jser As New JavaScriptSerializer
            jser.MaxJsonLength = Int32.MaxValue
            Dim fromdatedt As DateTime = DateTime.Parse(fromdate)
            Dim todatedt As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim FilterList As New List(Of ActiveFilterObject)
            Dim LocationList As New List(Of ActiveLocations)
            Dim dmobj As New List(Of SPCInspection.DefectImageDisplay_)
            Try
                If IsNothing(ActiveFilterStr) = False And ActiveFilterStr.Length > 0 Then
                    FilterList = jser.Deserialize(Of List(Of ActiveFilterObject))(ActiveFilterStr)
                End If
                If IsNothing(ActiveLocationStr) = False And ActiveLocationStr.Length > 0 Then
                    LocationList = jser.Deserialize(Of List(Of ActiveLocations))(ActiveLocationStr)
                End If
                dmobj = HttpRuntime.Cache("DefectImages_" + fromdatedt.ToShortDateString() + "_" + todatedt.ToShortDateString())

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try


            If Not dmobj Is Nothing Then

                Using _db As New Inspection_Entities

                    Dim LastCacheDefectID = (From v In dmobj Order By v.DefectID_ Descending Select v.DefectID_).Take(1).ToArray()
                    If Not LastCacheDefectID Is Nothing Then
                        If LastCacheDefectID.Length > 0 Then
                            Dim LastDefectId As Integer = Convert.ToInt64(LastCacheDefectID(0))

                            Dim cntrecs = (From v In _db.DefectMasters Where v.DefectID > LastDefectId And v.DefectTime <= todatedt And Not v.DefectImage Is Nothing Select v.DefectID).Count()
                            If cntrecs Then
                                Dim dmmiss = Inspect.GetDefectImageDisplay(fromdatedt, todatedt, LastDefectId)

                                If Not dmmiss Is Nothing Then
                                    dmobj.AddRange(dmmiss)
                                End If
                            End If
                        End If
                    End If

                End Using
            Else
                dmobj = Inspect.GetDefectImageDisplay(fromdatedt, todatedt)
                If dmobj.Count > 0 Then
                    HttpRuntime.Cache.Insert("DefectImages_" + fromdatedt.ToShortDateString() + "_" + todatedt.ToShortDateString(), dmobj)
                End If

            End If

            If Not FilterList Is Nothing Then
                If FilterList.Count > 0 Then
                    dmobj = FilterDefectImageDisplayByType(dmobj, FilterList)
                End If
            End If

            If Not LocationList Is Nothing Then
                If LocationList.Count > 0 Then
                    Dim dmfalseCnt = (From v In LocationList Where v.status = "False" Select v).Count()
                    If dmfalseCnt > 0 Then
                        dmobj = FilterDefectImageByLocation(dmobj, LocationList)
                    End If

                End If
            End If

            Dim cilist As New List(Of CarouselImage)

            For Each item In dmobj
                If item.Image.Count > 0 Then
                    Try
                        Dim BaseStr As String = Convert.ToBase64String(item.Image)

                        cilist.Add(New CarouselImage With {.imageUrl = "data:image/png;base64," + BaseStr, .linkUrl = "data:image/png;base64," + BaseStr, .content = "", .caption = item.DefectDesc_ & " AT " + item.DefectTime_.ToString("M/dd hh:mm"), .DataNo = item.DataNo_, .LocationCID = item.Location_, .AuditType = item.AuditType, .prpcode = item.Prp_Code, .DefectDesc = item.DefectDesc_})
                    Catch ex As Exception
                        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                    End Try

                End If
            Next

            Return jser.Serialize(cilist)
        End Function

        Private Function FilterDefectImageDisplayByType(ByRef listijs As List(Of SPCInspection.DefectImageDisplay_), filterlist As List(Of ActiveFilterObject)) As List(Of SPCInspection.DefectImageDisplay_)
            Dim ijsColl As New List(Of SPCInspection.DefectImageDisplay_)

            filterlist = (From v In filterlist Select v Order By v.id Ascending).ToList()

            For Each item In filterlist
                Dim ijsvals As New List(Of SPCInspection.DefectImageDisplay_)

                Select Case item.Name
                    Case "gs_ijsgrid_JobNumber"
                        listijs = (From v In listijs Where v.JobNumber_ = item.value Select v).ToList()
                    Case "pf_AuditType"
                        listijs = (From v In listijs Where v.AuditType = item.value Select v).ToList()
                    Case "gs_ijsgrid_ijsid"
                        listijs = (From v In listijs Where v.ijsid_ = item.value Select v).ToList()
                    Case "gs_ijsgrid_UnitDesc"
                        listijs = (From v In listijs Where v.UnitDesc_ = item.value Select v).ToList()
                    Case "gs_ijsgrid_Name"
                        listijs = (From v In listijs Where v.TemplateName_ = item.value Select v).ToList()
                    Case "gs_ijsgrid_Technical_PassFail"
                        listijs = (From v In listijs Where v.Technical_PassFail_ = item.value Select v).ToList()
                    Case "pf_DataNumber"
                        listijs = (From v In listijs Where v.DataNo_ = item.value Select v).ToList()
                    Case "pf_prp"
                        listijs = (From v In listijs Where v.Prp_Code = item.value Select v).ToList()
                    Case "tf_DefectType-Photos"
                        listijs = (From v In listijs Where v.DefectDesc_ = item.value Select v).ToList()
                End Select


            Next

            ijsColl = listijs

            Return ijsColl
        End Function

        Private Function FilterDefectImageByLocation(ByRef listijs As List(Of SPCInspection.DefectImageDisplay_), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.DefectImageDisplay_)


            For Each item In locationarray
                If item.status = "False" Then
                    listijs = (From v In listijs Where v.Location_ <> item.CID Select v).ToList()
                End If
            Next

            Return listijs

        End Function

        Public Function GetDashBoardImageArray(ByVal Locationid As String, ByVal todate As String, fromdate As String, ByVal SessionUri As String) As String

            Dim dilist As New List(Of SPCInspection.DefectImageDisplay)
            Dim bmap_di As New BMappers(Of SPCInspection.DefectImageDisplay)
            Dim fromdatedt As DateTime = DateTime.Parse(fromdate)
            Dim todatedt As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim fromdatestr As String = fromdatedt.ToString("yyyy-MM-dd H:mm:ss")
            Dim todatestr As String = todatedt.ToString("yyyy-MM-dd H:mm:ss")
            Dim cilist As New List(Of CarouselImage)


            dilist = bmap_di.GetInspectObject("SELECT dm.DefectID, dm.InspectionJobSummaryId, dm.DefectTime, dm.DefectDesc, dm.DataNo, it.Name AS LineType, lm.CID, dm.WorkOrder, lm.Name AS Location, dm.DefectImage_Filename FROM  DefectMaster dm INNER JOIN AprManager.dbo.LocationMaster lm ON lm.CID = '000' + dm.Location  INNER JOIN Inspection.dbo.TemplateName tn ON dm.TemplateId = tn.TemplateId INNER JOIN SpcMAIN.dbo.InspectionTypes it ON it.id = tn.LineTypeId WHERE  (LEN(dm.DefectImage) > 0) AND (dm.DefectTime >= CONVERT(DATETIME, '" & fromdatestr & "', 102)) AND (dm.DefectTime <= CONVERT(DATETIME, '" & todatestr & "', 102)) ORDER BY dm.DefectTime DESC")


            If dilist.Count > 0 Then
                Try

                    Dim diarray = dilist.ToArray()

                    For Each item In diarray
                        cilist.Add(New CarouselImage With {.imageUrl = SessionUri + "/APP/Utility/GetDefectImage.aspx?DefectID=" + item.DefectID.ToString(), .linkUrl = SessionUri + "/APP/Utility/GetDefectImage.aspx?DefectID=" + item.DefectID.ToString(), .content = "", .caption = item.DefectDesc.ToUpper() & " AT " & item.Location + " " + item.DefectTime.ToString("M/dd hh:mm"), .DataNo = item.DataNo, .LocationCID = item.CID.ToString().Substring(3, 3), .AuditType = item.Linetype})

                    Next

                Catch ex As Exception

                End Try

            End If

            Return jser.Serialize(cilist)

        End Function

        Public Function GetImageArray(ByVal Locationid As String, ByVal fromdate As String, ByVal todate As String, ByVal SessionUri As String) As String

            Dim dilist As New List(Of SPCInspection.DefectImageDisplay)
            Dim bmap_di As New BMappers(Of SPCInspection.DefectImageDisplay)
            Dim fromdatedt As DateTime = DateTime.Parse(fromdate)
            Dim todatedt As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim fromdatestr As String = fromdatedt.ToString("yyyy-MM-dd H:mm:ss")
            Dim todatestr As String = todatedt.ToString("yyyy-MM-dd H:mm:ss")
            Dim cilist As New List(Of CarouselImage)
            If Locationid = "999" Then
                dilist = bmap_di.GetInspectObject("SELECT dm.DefectID, dm.InspectionJobSummaryId, dm.DefectTime, dm.DefectDesc, dm.WorkOrder, lm.Name AS Location, dm.DefectImage_Filename FROM  DefectMaster dm INNER JOIN AprManager.dbo.LocationMaster lm ON lm.CID = '000' + dm.Location WHERE  (LEN(dm.DefectImage) > 0) AND (dm.DefectTime >= CONVERT(DATETIME, '" & fromdatestr & "', 102)) AND (dm.DefectTime <= CONVERT(DATETIME, '" & todatestr & "', 102))")
            Else
                dilist = bmap_di.GetInspectObject("SELECT dm.DefectID, dm.InspectionJobSummaryId, dm.DefectTime, dm.DefectDesc, dm.WorkOrder, lm.Name AS Location, dm.DefectImage_Filename FROM  DefectMaster dm INNER JOIN AprManager.dbo.LocationMaster lm ON lm.CID = '000' + dm.Location WHERE  (LEN(dm.DefectImage) > 0) AND (dm.DefectTime >= CONVERT(DATETIME, '" & fromdatestr & "', 102)) AND (dm.DefectTime <= CONVERT(DATETIME, '" & todatestr & "', 102)) AND (dm.Location = '" & Locationid & "')")
            End If


            If dilist.Count > 0 Then
                Try

                    Dim diarray = dilist.ToArray()

                    For Each item In diarray
                        cilist.Add(New CarouselImage With {.imageUrl = SessionUri + "/APP/Utility/GetDefectImage.aspx?DefectID=" + item.DefectID.ToString(), .linkUrl = SessionUri + "/APP/Utility/GetDefectImage.aspx?DefectID=" + item.DefectID.ToString(), .content = "", .caption = "<span>DefectID: " & item.DefectID & " JobSummaryId: " & item.InspectionJobSummaryId.ToString() & " ," & item.DefectDesc & " AT " & item.Location & ", Time: " & item.DefectTime.ToString("yy-MM-dd hh:mm") + "</span>"})

                    Next

                Catch ex As Exception

                End Try

            End If

            Return jser.Serialize(cilist)

        End Function

        Public Function GetDefectMasterDisplay(ByVal fromdate As String, ByVal todate As String) As String
            Dim fromdatedt As DateTime = DateTime.Parse(fromdate)
            Dim todatedt As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim ca As New CacheAccessDAO
            Dim listdm As New List(Of SPCInspection.DefectMasterDisplay)
            Dim jser As New JavaScriptSerializer
            jser.MaxJsonLength = Int32.MaxValue

            listdm = ca.GetDefectMasterList(fromdatedt, todatedt)

            Return jser.Serialize(listdm)

        End Function
        Public Function GetJobSummaryDisplay(ByVal fromdate As String, ByVal todate As String) As String
            Dim fromdatedt As DateTime = DateTime.Parse(fromdate)
            Dim todatedt As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim ca As New CacheAccessDAO
            Dim listdm As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim jser As New JavaScriptSerializer
            jser.MaxJsonLength = Int32.MaxValue

            listdm = ca.JobSummaryList(fromdatedt, todatedt)

            Return jser.Serialize(listdm)

        End Function

        Public Function GetResultsOverview(ByVal todate As String, ByVal CID As Integer) As String
            Dim todatedt As DateTime = DateTime.Parse(todate).AddDays(1)
            Dim thismonthfd As DateTime = New DateTime(todatedt.Year, todatedt.Month, 1)
            Dim thismonthld As DateTime = New DateTime(todatedt.Year, todatedt.Month, 1, 23, 59, 59).AddMonths(1).AddDays(-1)
            Dim prevmonthfd As DateTime = New DateTime(todatedt.Year, todatedt.AddMonths(-1).Month, 1)
            Dim prevmonthld As DateTime = New DateTime(prevmonthfd.Year, prevmonthfd.Month, 1, 23, 59, 59).AddMonths(1).AddDays(-1)
            Dim listot As New List(Of SPCInspection.OverTable)

            'listot.Add(New SPCInspection.OverTable With {.Type = "LotAcceptibility", .ytd = Inspect.GetLotAcc(prevmonthfd, prevmonthld, CID), .mtd = Inspect.GetLotAcc(thismonthfd, thismonthld, CID)})

            'listot.Add(New SPCInspection.OverTable With {.Type = "Compliance", .ytd = Inspect.Getas400MatchPerc(prevmonthfd, prevmonthld, CID), .mtd = Inspect.Getas400MatchPerc(thismonthfd, thismonthld, CID)})

            Return jser.Serialize(listot)

        End Function
    End Class

End Namespace