<%@ WebHandler Language="VB" Class="core.SpecSummary_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Reflection

Namespace core
    Public Class SpecSummaryLoad
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
   
    Public Class SpecSummary_Load : Implements IHttpHandler, IRequiresSessionState
        
        Private II As New InspectionInputDAO
        Private IU As New InspectionUtilityDAO
        
        Dim jser As New JavaScriptSerializer
        Dim objdm As New SpecSummaryLoad
        Dim fromdate As DateTime
        Dim todate As DateTime
        Dim jsonData As New jsonData
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            
            Dim listdm As New List(Of SPCInspection.SpecSummaryDisplay)
            Dim listret As New List(Of SPCInspection.SpecSummaryDisplay)
            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of SpecSummaryLoad)
                
                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                If Len(objdm.fromdate) > 0 And Len(objdm.todate) > 0 And Len(objdm.CID_Posted) > 0 Then
                    Dim FilterList As New List(Of ActiveFilterObject)
                    If objdm.FilterListstring.Length > 0 Then
                        FilterList = jser.Deserialize(Of List(Of ActiveFilterObject))(objdm.FilterListstring)
                    End If
                    fromdate = Date.Parse(objdm.fromdate)
                    todate = Date.Parse(objdm.todate)
                    Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd")
                    Try
                        If objdm.NextFlag = "false" And FilterList.Count = 0 Then
                            listdm = AssembleObject()
                        ElseIf FilterList.Count > 0 And IsNothing(objdm.FilterListstring) = False Then
                            If IsNothing(listdm) = False Then
                                listdm = AssembleObject()
                                
                                If objdm.FilterListstring.Length > 0 Then
                                    listdm = FilterObjectByType(listdm, FilterList)
                                End If
                                listdm = (From v In listdm Select v Order By v.id Descending).ToList()
                            End If
                            'If IsNothing(HttpRuntime.Cache("TableSpecSummary." + objdm.SessionId)) = False Then
                            '    listdm = FilterObject(objdm.FilterColumnName, objdm.Filterqvalue, HttpRuntime.Cache("TableSpecSummary." + objdm.SessionId))
                            'End If
                        ElseIf objdm.NextFlag = "true" Then
                            listdm = HttpRuntime.Cache("TableSpecSummary." + objdm.SessionId)
                        End If

                    Catch ex As Exception

                    End Try
                    If listdm.Count = 0 Then

                        listdm = IU.GetSpecSummary(fromdate, todate)
                        
                    End If
                    If objdm.NextFlag = "false" And IsNothing(FilterList) = True Then
                        InsertIntoCache(listdm)
                    End If
                End If
                If listdm.Count > 0 Then
                    If objdm.LocationArrayString.Length > 0 Then
                        Dim locationarray = jser.Deserialize(Of List(Of ActiveLocations))(objdm.LocationArrayString)
                        Dim listso As New List(Of SingleObject)
                        
                        If IsNothing(locationarray) = False Then
                            If locationarray.ToArray().Length > 0 Then
                                listdm = FilterObjectByLocation(listdm, locationarray)
                            End If
                        End If
                        
                        listso = getBoudingIds(objdm.rowNum, objdm.FlagCnt, (From v In listdm Select v Order By v.id Descending).ToList())
                        If listso.Count > 0 Then
                            
                            listret = (From v In listdm Where v.id <= listso.ToArray()(0).Object2 And v.id >= listso.ToArray()(0).Object1 Select v).ToList()
                
                        HttpRuntime.Cache.Insert("TableSpecSummary." + objdm.SessionId, listdm, Nothing, Date.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration)
                        End If
                    End If
                    
                    jsonData.total = Math.Round(listdm.Count / objdm.rowNum)
                    jsonData.page = objdm.FlagCnt
                    jsonData.userdata = jser.Serialize(GetFilterColumnNames(listdm))
                    jsonData.records = listdm.Count
                    jsonData.rows = (From v In listret Select v Order By v.id Descending).ToList()
                End If
            End If
            Dim teststring As String = jser.Serialize(jsonData)
            context.Response.Write(jser.Serialize(jsonData))
        End Sub
        
        Public Function FilterObjectByType(ByRef listijs As List(Of SPCInspection.SpecSummaryDisplay), filterlist As List(Of ActiveFilterObject)) As List(Of SPCInspection.SpecSummaryDisplay)
            
            filterlist = (From v In filterlist Select v Order By v.id Ascending).ToList()
            
            For Each item In filterlist
          
                Select Case item.Name
                    Case "gs_Specgrid_JobNumber"
                        listijs = (From v In listijs Where v.JobNumber = item.value Select v).ToList()
                    Case "gs_Specgrid_id"
                        listijs = (From v In listijs Where v.id = item.value Select v).ToList()
                    Case "gs_Specgrid_UnitDesc"
                        listijs = (From v In listijs Where v.UnitDesc = item.value Select v).ToList()
                    Case "gs_Specgrid_DataNo"
                        listijs = (From v In listijs Where v.DataNo = item.value Select v).ToList()
                    Case "pf_AuditType"
                        listijs = (From v In listijs Where v.LineTypeVariable = item.value Select v).ToList()
                    Case "pf_DataNumber"
                        listijs = (From v In listijs Where v.DataNo = item.value Select v).ToList()
                End Select
                
            Next
            
            Return listijs
        End Function
        
        Public Function FilterObjectByLocation(ByRef listijs As List(Of SPCInspection.SpecSummaryDisplay), locationarray As List(Of ActiveLocations)) As List(Of SPCInspection.SpecSummaryDisplay)
            
            
            For Each item In locationarray
                If item.status = "False" Then
                    listijs = (From v In listijs Where v.CID <> item.CID Select v).ToList()
                End If
            Next
            
            Return listijs
            
        End Function
        
        Private Function FilterObject(ByVal ColumnName As String, ByVal qvalue As Object, ByRef listin As List(Of SPCInspection.SpecSummaryDisplay)) As List(Of SPCInspection.SpecSummaryDisplay)
            
            If listin.Count > 0 And ColumnName.Length > 0 Then
                Select Case ColumnName
                    Case "gs_Specgrid_JobNumber"
                        listin = (From v In listin Where v.JobNumber = qvalue Select v).ToList()
                    Case "gs_Specgrid_id"
                        listin = (From v In listin Where v.id = qvalue Select v).ToList()
                    Case "gs_Specgrid_UnitDesc"
                        listin = (From v In listin Where v.UnitDesc = qvalue Select v).ToList()
                    Case "gs_Specgrid_DataNo"
                        listin = (From v In listin Where v.DataNo = qvalue Select v).ToList()
                End Select
            End If
          
            Return listin
            
        End Function
        
        Private Function GetFilterColumnNames(listin As List(Of SPCInspection.SpecSummaryDisplay)) As Object
            Dim colcnt As Integer = 1

            Dim jqobj As New jqgridFilterList
            Dim Filterarray = jser.Deserialize(Of List(Of FilterColumnValues))(objdm.SelectFilterValues)
            
            If Filterarray.Count > 0 Then
                For Each info As PropertyInfo In jqobj.GetType().GetProperties()
                
                    Select Case info.Name.ToUpper()
                        Case "COL1"
                            jqobj.Col1 = (From v In listin Select v.id Distinct).ToArray()
                            jqobj.Col1Name = info.Name
                            jqobj.selectedVal1 = Filterarray(0).val
                        Case "COL2"
                            jqobj.Col2 = (From v In listin Select v.JobNumber Distinct).ToArray()
                            jqobj.Col2Name = info.Name
                            jqobj.selectedVal2 = Filterarray(1).val
                        Case "COL3"
                            jqobj.Col3 = (From v In listin Select v.UnitDesc Distinct).ToArray()
                            jqobj.Col3Name = info.Name
                            jqobj.selectedVal3 = Filterarray(2).val
                        Case "COL4"
                            jqobj.Col4 = (From v In listin Select v.DataNo Distinct).ToArray()
                            jqobj.Col4Name = info.Name
                            jqobj.selectedVal4 = Filterarray(3).val

                    End Select
                Next
                
            End If
            Return jqobj
        End Function
 
        Private Function getBoudingIds(ByVal rowNum As Integer, ByVal pagecnt As Integer, ByVal listin As List(Of SPCInspection.SpecSummaryDisplay)) As List(Of SingleObject)
            Dim arraydm = listin.ToArray()
            Dim retobj As New List(Of SingleObject)
            If arraydm.Length > 0 Then
                Dim NumofPages As Integer = listin.Count / rowNum
                If NumofPages > pagecnt Then
                    retobj.Add(New SingleObject With {.Object1 = arraydm(rowNum * pagecnt).id, .Object2 = arraydm(rowNum * pagecnt - rowNum).id})
                Else
                    If arraydm.Length < rowNum Then
                        retobj.Add(New SingleObject With {.Object1 = arraydm(arraydm.Length - 1).id, .Object2 = arraydm(0).id})
                    Else
                        retobj.Add(New SingleObject With {.Object1 = arraydm(arraydm.Length - 1).id, .Object2 = arraydm(rowNum * pagecnt - rowNum).id})
                    End If
                End If
            End If
            
            Return retobj
        End Function
        
        Private Sub InsertIntoCache(ByVal listijs As List(Of SPCInspection.SpecSummaryDisplay))
            Dim intdate As DateTime = objdm.fromdate
            Dim Cachestring As String
            Do While intdate <= objdm.todate
                Cachestring = intdate.Year.ToString + "." + intdate.DayOfYear.ToString()
                Dim ijsdays As New List(Of SPCInspection.SpecSummaryDisplay)
                ijsdays = (From v In listijs Where DateTime.Parse(v.Inspection_Started).DayOfYear = intdate.DayOfYear Select v).ToList()
                If ijsdays.Count > 0 Then
                    HttpRuntime.Cache.Insert("SpecSummaryLoad1" + Cachestring, ijsdays, Nothing, Date.Now.AddDays(4), System.Web.Caching.Cache.NoSlidingExpiration)
                End If
                intdate = intdate.AddDays(1)
            Loop
        End Sub
        
        Public Function AssembleObject() As List(Of SPCInspection.SpecSummaryDisplay)
            Dim listret As New List(Of SPCInspection.SpecSummaryDisplay)
            Dim listdc As New List(Of DayCache)
            listdc = RetrieveCacheObjects()
            
            For Each item In listdc
                If IsNothing(item.ListObj) = False Then
                    listret.AddRange(UpdateJSList2(item.ListObj, item.UnitDate))
                    
                Else
                    listret.AddRange(IU.GetSpecSummary(item.UnitDate, item.UnitDate))
                End If
            Next
            
            Return (From v In listret Select v Order By v.Inspection_Started Ascending).ToList()
        End Function
        
        Private Function UpdateJSList2(ByVal list As List(Of SPCInspection.SpecSummaryDisplay), ByVal intdate As DateTime) As List(Of SPCInspection.SpecSummaryDisplay)
            Dim listret As New List(Of SPCInspection.SpecSummaryDisplay)
            
            listret = (From v In list Where v.Inspection_Finished.Length > 0 Select v).ToList()
            
            Try
                If listret.Count <> list.Count Then
                    listret.AddRange(IU.GetSpecSummaryUnFinished(intdate, intdate))
                End If
            Catch ex As Exception

            End Try
            
            listret = (From v In list Select v Order By v.id Descending).ToList()
            
            Return listret
            
        End Function
        
        Private Function RetrieveCacheObjects() As List(Of DayCache)
            Dim intdate As DateTime = objdm.fromdate
            Dim daycache As New List(Of DayCache)
            Dim Cachestring As String
            Do While intdate <= objdm.todate
                Cachestring = intdate.Year.ToString() + "." + intdate.DayOfYear.ToString()
                daycache.Add(New DayCache With {.UnitDate = intdate, .ListObj = HttpRuntime.Cache("SpecSummaryLoad1" + Cachestring)})

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
