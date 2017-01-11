<%@ WebHandler Language="VB" Class="core.JobSummary_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Reflection

Namespace core
    Public Class JobSummaryLoad
        Public Property CID_Posted As String
        Public Property fromdate As String
        Public Property todate As String
        Public Property rowNum As Integer
        Public Property FlagCnt As Integer
        Public Property SessionId As String
        Public Property NextFlag As String
        Public Property FilterFlag As String
        Public Property FilterColumnName As String
        Public Property Filterqvalue As Object
        Public Property SelectFilterValues As Object
        Public Property LocationArrayString As String
        Public Property FilterListstring As String
    End Class

    Public Class FilterColumnValues
        Public Property col As Object
        Public Property val As Object
    End Class

    Public Class JobSummary_Load : Implements IHttpHandler, IRequiresSessionState

        Private II As New InspectionInputDAO
        Private IU As New InspectionUtilityDAO

        Dim jser As New JavaScriptSerializer
        Dim objdm As New JobSummaryLoad
        Dim fromdate As DateTime
        Dim todate As DateTime
        Dim jsonData As New jsonData
        Dim TableRowCnt As Integer = 0
        Dim TotRowCnt As Integer = 0

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params

            Dim listdm As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim listret As New List(Of SPCInspection.InspectionSummaryDisplay)
            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of JobSummaryLoad)
                Dim FilterList As New List(Of ActiveFilterObject)
                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                If IsNothing(objdm) = False Then
                    If Len(objdm.fromdate) > 0 And Len(objdm.todate) > 0 And Len(objdm.CID_Posted) > 0 Then

                        If objdm.FilterListstring.Length > 0 Then
                            FilterList = jser.Deserialize(Of List(Of ActiveFilterObject))(objdm.FilterListstring)
                        End If
                        If IsNothing(FilterList) = False Then
                            fromdate = Date.Parse(objdm.fromdate)
                            todate = Date.Parse(objdm.todate)
                            Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd")
                            Try

                                If objdm.NextFlag = "false" And FilterList.Count = 0 Then
                                    listdm = IU.GetInspectionSummaryDay(fromdate, todate)
                                    InsertIntoCache(listdm)
                                ElseIf FilterList.Count > 0 And IsNothing(objdm.FilterListstring) = False Then
                                    If IsNothing(listdm) = False Then
                                        listdm = AssembleObject()

                                        If objdm.FilterListstring.Length > 0 Then
                                            listdm = FilterObjectByType(listdm, FilterList)
                                        End If
                                        'listdm = FilterObject(objdm.FilterColumnName, objdm.Filterqvalue, listdm)
                                        listdm = (From v In listdm Select v Order By v.ijsid Descending).ToList()
                                    End If
                                ElseIf objdm.NextFlag = "true" Then
                                    listdm = HttpRuntime.Cache("TableJobSummary." + objdm.SessionId)
                                    listdm = (From v In listdm Select v Order By v.ijsid Descending).ToList()
                                End If

                            Catch ex As Exception

                            End Try
                        End If
                        'If listdm.Count = 0 And FilterList.Count = 0 Then

                        '    listdm = IU.GetInspectionSummary(fromdate, todate)

                        'End If

                    End If
                End If
                If IsNothing(listdm) = False Then
                    If objdm.LocationArrayString.Length > 0 Then
                        Dim locationarray = jser.Deserialize(Of List(Of ActiveLocations))(objdm.LocationArrayString)
                        Dim listso As New List(Of SingleObject)

                        If IsNothing(locationarray) = False Then
                            If locationarray.ToArray().Length > 0 Then
                                listdm = FilterObjectByLocation(listdm, locationarray)
                            End If
                        End If

                        listso = getBoudingIds(objdm.rowNum, objdm.FlagCnt, (From v In listdm Select v Order By v.ijsid Descending).ToList())
                        TableRowCnt = Math.Round(listdm.Count / objdm.rowNum)
                        TotRowCnt = listdm.Count
                        If listso.Count > 0 Then
                            listret = (From v In listdm Where v.ijsid >= listso.ToArray()(0).Object1 And v.ijsid <= listso.ToArray()(0).Object2 Select v).ToList()
                            HttpRuntime.Cache.Insert("TableJobSummaryDisplayed." + objdm.SessionId, (From v In listret Select v Order By v.STARTED Descending).ToList(), Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                        End If

                        HttpRuntime.Cache.Insert("TableJobSummary." + objdm.SessionId, listdm, Nothing, Date.Now.AddMinutes(240), System.Web.Caching.Cache.NoSlidingExpiration)

                        'If objdm.CID_Posted = "999" Then
                        '    listso = getBoudingIds(objdm.rowNum, objdm.FlagCnt, (From v In listdm Select v Order By v.id Descending).ToList())
                        '    TableRowCnt = Math.Round(listdm.Count / objdm.rowNum)
                        '    TotRowCnt = listdm.Count
                        '    If listso.Count > 0 Then
                        '        listret = (From v In listdm Where v.id >= listso.ToArray()(0).Object1 And v.id <= listso.ToArray()(0).Object2 Select v).ToList()
                        '    End If
                        '    HttpRuntime.Cache.Insert("TableJobSummary." + objdm.SessionId, listdm, Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                        'Else
                        '    listso = getBoudingIds(objdm.rowNum, objdm.FlagCnt, (From v In listdm Where v.CID = objdm.CID_Posted Select v Order By v.id Descending).ToList())
                        '    TotRowCnt = (From v In listdm Where v.CID = objdm.CID_Posted Select v).Count()
                        '    TableRowCnt = Math.Round(TotRowCnt / objdm.rowNum)
                        '    If listso.Count > 0 Then
                        '        listret = (From v In listdm Where v.CID = objdm.CID_Posted And v.id >= listso.ToArray()(0).Object1 And v.id <= listso.ToArray()(0).Object2 Select v).ToList()
                        '    End If
                        '    HttpRuntime.Cache.Insert("TableJobSummary." + objdm.SessionId, (From v In listdm Where v.CID = objdm.CID_Posted Select v).ToList(), Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                        'End If
                    End If
                    jsonData.total = TableRowCnt
                    jsonData.page = objdm.FlagCnt
                    jsonData.userdata = jser.Serialize(GetFilterColumnNames(listdm))
                    jsonData.records = TotRowCnt
                    jsonData.rows = (From v In listret Select v Order By v.STARTED Descending).ToList()
                End If
            End If

            context.Response.Write(jser.Serialize(jsonData))
        End Sub
        Private Function FilterObject(ByVal ColumnName As String, ByVal qvalue As Object, ByRef listin As List(Of SPCInspection.InspectionSummaryDisplay)) As List(Of SPCInspection.InspectionSummaryDisplay)


            If listin.Count > 0 And ColumnName.Length > 0 Then
                Select Case ColumnName
                    Case "gs_ijsgrid_JobNumber"
                        listin = (From v In listin Where v.JobNumber = qvalue Select v).ToList()
                    Case "pf_AuditType"
                        listin = (From v In listin Where v.LineTypeVariable = qvalue Select v).ToList()
                    Case "gs_ijsgrid_ijsid"
                        listin = (From v In listin Where v.ijsid = qvalue Select v).ToList()
                    Case "gs_ijsgrid_UnitDesc"
                        listin = (From v In listin Where v.UnitDesc = qvalue Select v).ToList()
                    Case "gs_ijsgrid_Name"
                        listin = (From v In listin Where v.Name = qvalue Select v).ToList()
                    Case "gs_ijsgrid_Technical_PassFail"
                        listin = (From v In listin Where v.Technical_PassFail = qvalue Select v).ToList()
                    Case "pf_DataNumber"
                        listin = (From v In listin Where v.DataNo = qvalue Select v).ToList()
                End Select
            End If

            If IsNothing(listin) = False Then
                HttpRuntime.Cache.Insert("TableJobSummary." + objdm.SessionId, listin, Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
            End If

            Return listin

        End Function

        Public Function FilterObjectByType(ByRef listijs As List(Of SPCInspection.InspectionSummaryDisplay), filterlist As List(Of ActiveFilterObject)) As List(Of SPCInspection.InspectionSummaryDisplay)
            Dim ijsColl As New List(Of SPCInspection.InspectionSummaryDisplay)

            filterlist = (From v In filterlist Select v Order By v.id Ascending).ToList()

            For Each item In filterlist
                Dim ijsvals As New List(Of SPCInspection.InspectionSummaryDisplay)

                Select Case item.Name
                    Case "gs_ijsgrid_JobNumber"
                        listijs = (From v In listijs Where v.JobNumber = item.value Select v).ToList()
                    Case "pf_AuditType"
                        listijs = (From v In listijs Where v.LineTypeVariable = item.value Select v).ToList()
                    Case "gs_ijsgrid_ijsid"
                        listijs = (From v In listijs Where v.ijsid = item.value Select v).ToList()
                    Case "gs_ijsgrid_UnitDesc"
                        listijs = (From v In listijs Where v.UnitDesc = item.value Select v).ToList()
                    Case "gs_ijsgrid_Name"
                        listijs = (From v In listijs Where v.Name = item.value Select v).ToList()
                    Case "gs_ijsgrid_Technical_PassFail"
                        listijs = (From v In listijs Where v.Technical_PassFail = item.value Select v).ToList()
                    Case "pf_DataNumber"
                        listijs = (From v In listijs Where v.DataNo = item.value Select v).ToList()
                    Case "pf_prp"
                        listijs = (From v In listijs Where v.PRP_Code = item.value Select v).ToList()
                End Select


            Next

            ijsColl = listijs

            Return ijsColl
        End Function

        Public Function FilterObjectByLocation(ByRef listijs As List(Of SPCInspection.InspectionSummaryDisplay), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.InspectionSummaryDisplay)


            For Each item In locationarray
                If item.status = "False" Then
                    listijs = (From v In listijs Where v.CID <> item.CID Select v).ToList()
                End If
            Next

            Return listijs

        End Function

        Private Function GetFilterColumnNames(listin As List(Of SPCInspection.InspectionSummaryDisplay)) As Object
            Dim colcnt As Integer = 1

            Dim jqobj As New jqgridFilterList
            Dim Filterarray = jser.Deserialize(Of List(Of FilterColumnValues))(objdm.SelectFilterValues)

            If Filterarray.Count > 0 Then
                For Each info As PropertyInfo In jqobj.GetType().GetProperties()

                    Select Case info.Name.ToUpper()
                        Case "COL1"
                            jqobj.Col1 = (From v In listin Select v.ijsid Distinct).ToArray()
                            jqobj.Col1Name = info.Name
                            jqobj.selectedVal1 = Filterarray(0).val
                        Case "COL2"
                            jqobj.Col2 = (From v In listin Order By v.JobNumber Ascending Select v.JobNumber Distinct).ToArray()
                            jqobj.Col2Name = info.Name
                            jqobj.selectedVal2 = Filterarray(1).val
                        Case "COL3"
                            jqobj.Col3 = (From v In listin Select v.UnitDesc Distinct).ToArray()
                            jqobj.Col3Name = info.Name
                            jqobj.selectedVal3 = Filterarray(2).val
                        Case "COL5"
                            jqobj.Col5 = (From v In listin Order By v.Name Ascending Select v.Name Distinct).ToArray()
                            jqobj.Col5Name = info.Name
                            jqobj.selectedVal5 = Filterarray(3).val
                        Case "COL7"
                            jqobj.Col7 = (From v In listin Select v.Technical_PassFail Distinct).ToArray()
                            jqobj.Col7Name = info.Name
                            jqobj.selectedVal7 = Filterarray(4).val
                    End Select
                Next

            End If
            Return jqobj
        End Function

        Private Function getBoudingIds(ByVal rowNum As Integer, ByVal pagecnt As Integer, ByVal listin As List(Of SPCInspection.InspectionSummaryDisplay)) As List(Of SingleObject)
            Dim arraydm = listin.ToArray()
            Dim retobj As New List(Of SingleObject)
            If arraydm.Length > 0 Then
                Dim NumofPages As Integer = listin.Count / rowNum
                If NumofPages > pagecnt Then
                    retobj.Add(New SingleObject With {.Object1 = arraydm(rowNum * pagecnt).ijsid, .Object2 = arraydm(rowNum * pagecnt - rowNum).ijsid})
                Else
                    If arraydm.Length < rowNum Then
                        retobj.Add(New SingleObject With {.Object1 = arraydm(arraydm.Length - 1).ijsid, .Object2 = arraydm(0).ijsid})
                    Else
                        retobj.Add(New SingleObject With {.Object1 = arraydm(arraydm.Length - 1).ijsid, .Object2 = arraydm(rowNum * pagecnt - rowNum).ijsid})
                    End If
                End If
            End If

            Return retobj
        End Function

        Private Sub InsertIntoCache(ByVal listijs As List(Of SPCInspection.InspectionSummaryDisplay))
            Dim intdate As DateTime = objdm.fromdate
            Dim Cachestring As String
            Do While intdate <= objdm.todate
                Cachestring = intdate.Year.ToString + "." + intdate.DayOfYear.ToString()
                Dim ijsdays As New List(Of SPCInspection.InspectionSummaryDisplay)
                ijsdays = (From v In listijs Where DateTime.Parse(v.STARTED).DayOfYear = intdate.DayOfYear Select v).ToList()
                If ijsdays.Count > 0 Then
                    HttpRuntime.Cache.Insert("JobSummaryLoad1" + Cachestring, ijsdays, Nothing, Date.Now.AddDays(4), System.Web.Caching.Cache.NoSlidingExpiration)
                End If
                intdate = intdate.AddDays(1)
            Loop
        End Sub

        Public Function AssembleObject() As List(Of SPCInspection.InspectionSummaryDisplay)
            Dim listret As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim listdc As New List(Of DayCache)
            listdc = RetrieveCacheObjects()

            For Each item In listdc
                If IsNothing(item.ListObj) = False Then
                    listret.AddRange(UpdateJSList2(item.ListObj, item.UnitDate))

                Else
                    listret.AddRange(IU.GetInspectionSummaryDay(item.UnitDate, item.UnitDate))
                End If
            Next

            Return (From v In listret Select v Order By v.STARTED Ascending).ToList()
        End Function

        Private Function UpdateJSList2(ByVal list As List(Of SPCInspection.InspectionSummaryDisplay), ByVal intdate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)
            Dim listret As New List(Of SPCInspection.InspectionSummaryDisplay)

            listret = (From v In list Where v.FINISHED = Date.MinValue Select v).ToList()

            Try
                listret.AddRange(IU.GetInspectionSummaryDayUnFinished(intdate, intdate))
            Catch ex As Exception

            End Try

            listret = (From v In list Select v Order By v.ijsid Descending).ToList()

            Return listret

        End Function

        Private Function UpdateJSList(ByVal list As List(Of SPCInspection.InspectionSummaryDisplay)) As List(Of SPCInspection.InspectionSummaryDisplay)
            Dim listret As New List(Of SPCInspection.InspectionSummaryDisplay)

            listret = (From v In list Where v.FINISHED = Date.MinValue Select v).ToList()

            Try
                listret.AddRange(IU.GetInspectionSummaryDayUnFinished(fromdate, todate))
            Catch ex As Exception

            End Try

            listret = (From v In list Select v Order By v.STARTED Descending).ToList()

            Return listret

        End Function

        Private Function RetrieveCacheObjects() As List(Of DayCache)
            Dim intdate As DateTime = objdm.fromdate
            Dim daycache As New List(Of DayCache)
            Dim Cachestring As String
            Do While intdate <= objdm.todate
                Cachestring = intdate.Year.ToString() + "." + intdate.DayOfYear.ToString()
                daycache.Add(New DayCache With {.UnitDate = intdate, .ListObj = HttpRuntime.Cache("JobSummaryLoad1" + Cachestring)})

                intdate = intdate.AddDays(1)
            Loop

            Return daycache
        End Function

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class


End Namespace
