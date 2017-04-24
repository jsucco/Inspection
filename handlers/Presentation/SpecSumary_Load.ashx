<%@ WebHandler Language="VB" Class="core.SpecSummary_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core
    Public Class SpecSummaryLoad
        Public Property CID As String
        Public Property fromdate As String
        Public Property todate As String
        Public Property rowNum As Integer
        Public Property FlagCnt As Integer
        Public Property SessionId As String
        Public Property NextFlag As String
        Public Property FilterFlag As String
        
    End Class
   
    Public Class SpecSummary_Load : Implements IHttpHandler, IRequiresSessionState
        
        Private II As New InspectionInputDAO
        Private IU As New InspectionUtilityDAO
        
        Dim objdm As New SpecSummaryLoad
        Dim fromdate As DateTime
        Dim todate As DateTime
        Dim jsonData As New jsonData
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim jser As New JavaScriptSerializer
            Dim listdm As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim listret As New List(Of SPCInspection.ProductDisplaySpecCollection)
            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of SpecSummaryLoad)
                
                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                If Len(objdm.fromdate) > 0 And Len(objdm.todate) > 0 And Len(objdm.CID) > 0 Then
                    fromdate = Date.Parse(objdm.fromdate)
                    todate = Date.Parse(objdm.todate)
                    Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd")
                    Try
                        If objdm.NextFlag = "false" And objdm.FilterFlag = "false" Then
                            listdm = AssembleObject()
                        ElseIf objdm.FilterFlag = "true" Then
                            
                        Else
                            listdm = HttpRuntime.Cache("TableSpecSummary." + objdm.SessionId)
                        End If

                    Catch ex As Exception

                    End Try
                    If listdm.Count = 0 Then

                        listdm = IU.GetDisplaySpecCollection3(fromdate, todate.AddDays(1))
                        
                    End If
                    If objdm.NextFlag = "false" Then
                        InsertIntoCache(listdm)
                    End If
                End If
                If listdm.Count > 0 Then
                    Dim listso As New List(Of SingleObject)
                    
                    listso = getBoudingIds(objdm.rowNum, objdm.FlagCnt, (From v In listdm Select v Order By v.id Descending).ToList())
                    If listso.Count > 0 Then
                        If objdm.CID = "999" Then
                            listret = (From v In listdm Where v.id <= listso.ToArray()(0).Object2 And v.id >= listso.ToArray()(0).Object1 Select v).Distinct.ToList()
                            HttpRuntime.Cache.Insert("TableSpecSummary." + objdm.SessionId, listdm, Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                        Else
                            listret = (From v In listdm Where v.Location = objdm.CID And v.id >= listso.ToArray()(0).Object2 And v.id <= listso.ToArray()(0).Object1 Select v).ToList()
                            HttpRuntime.Cache.Insert("TableSpecSummary." + objdm.SessionId, (From v In listdm Where v.Location = objdm.CID Select v).ToList(), Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                        End If
                
                    End If
                    jsonData.total = Math.Round(listdm.Count / objdm.rowNum)
                    jsonData.page = objdm.FlagCnt
                    jsonData.records = listdm.Count
                    jsonData.rows = (From v In listret Select v Order By v.Timestamp Descending).ToList()
                End If
            End If
            Dim teststring As String = jser.Serialize(jsonData)
            context.Response.Write(jser.Serialize(jsonData))
        End Sub
        
        Private Function getBoudingIds(ByVal rowNum As Integer, ByVal pagecnt As Integer, ByVal listin As List(Of SPCInspection.ProductDisplaySpecCollection)) As List(Of SingleObject)
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
        
        Private Sub InsertIntoCache(ByVal listijs As List(Of SPCInspection.ProductDisplaySpecCollection))
            Dim intdate As DateTime = objdm.fromdate
            Dim Cachestring As String
            Do While intdate <= objdm.todate
                Cachestring = intdate.Year.ToString + "." + intdate.DayOfYear.ToString()
                Dim ijsdays As New List(Of SPCInspection.ProductDisplaySpecCollection)
                ijsdays = (From v In listijs Where DateTime.Parse(v.Timestamp).DayOfYear = intdate.DayOfYear Select v).ToList()
                If ijsdays.Count > 0 Then
                    HttpRuntime.Cache.Insert("SpecSummaryLoad1" + Cachestring, ijsdays, Nothing, Date.Now.AddDays(4), System.Web.Caching.Cache.NoSlidingExpiration)
                End If
                intdate = intdate.AddDays(1)
            Loop
        End Sub
        
        Public Function AssembleObject() As List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim listret As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim listdc As New List(Of DayCache)
            Dim lastSpecid As Object = New Object() {}
            listdc = RetrieveCacheObjects()
            
            For Each item In listdc
                If IsNothing(item.ListObj) = False Then
                    Dim cachelist As New List(Of SPCInspection.ProductDisplaySpecCollection)
                    cachelist = item.ListObj
                    cachelist = (From v In cachelist Select v Order By v.SpecId Descending).Distinct().ToList()
                    If item.UnitDate.DayOfYear = Date.Now.DayOfYear And cachelist.Count > 0 Then
                        lastSpecid = (From v In InspectionInputDAO.SpecCache Select v Order By v.LastDefectID Descending).ToArray()
                        If lastSpecid Then
                            If lastSpecid(0).LastDefectID > cachelist.ToArray()(0).SpecId Then
                                listret = IU.GetUpdatedDisplaySpecCollection(cachelist.ToArray()(0).SpecId)
                            End If
                        End If
                        If listret.Count = 0 Then GoTo 101
                    Else
                        listret.AddRange(item.ListObj)
                    End If
                Else
101:
                    listret.AddRange(IU.GetDisplaySpecCollection3(item.UnitDate, item.UnitDate))
                End If
            Next
            
            Return (From v In listret Select v Order By v.Timestamp Ascending).ToList()
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
