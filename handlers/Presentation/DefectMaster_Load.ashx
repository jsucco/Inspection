<%@ WebHandler Language="VB" Class="core.DefectMaster_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core
    Public Class DefectMasterLoad
        Public Property CID As String
        Public Property fromdate As String
        Public Property todate As String
    End Class
    
    Public Class DefectMaster_Load : Implements IHttpHandler, IRequiresSessionState
        
        Private II As New InspectionInputDAO
        Private IU As New InspectionUtilityDAO
        
        Dim objdm As New DefectMasterLoad
        
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim jser As New JavaScriptSerializer
            jser.MaxJsonLength = Int32.MaxValue
            Dim listdm As New List(Of SPCInspection.DefectMasterDisplay)
            
            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of DefectMasterLoad)
                
                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                If Len(objdm.fromdate) > 0 And Len(objdm.todate) > 0 And Len(objdm.CID) > 0 Then
                    Dim fromdate As DateTime = Date.Parse(objdm.fromdate)
                    Dim todate As DateTime = Date.Parse(objdm.todate)
                    Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd")
                    Try

                        Dim cacheobj As List(Of SPCInspection.DefectMasterDisplay) = context.Cache("DefectMasterLoad" + CacheString)
                        If IsNothing(cacheobj) = False Then
                            Dim lastdefectid As Object = New Object() {}
                            Dim lasttabledefectid As Integer
                            Dim lastCacheObjID As Object
                            Dim listso As New List(Of SingleObject)
                            Dim bmapso As New BMappers(Of SingleObject)
                            
                            If objdm.CID = "999" Then
                                If objdm.todate = Date.Now.ToString("MM/dd/yyyy") Then
                                    lastdefectid = (From v In InspectionInputDAO.InspectionCache Select v Order By v.LastDefectID Descending).ToArray()
                                Else
                                    listso = bmapso.GetInspectObject("SELECT TOP(1) DefectID AS Object1 FROM DefectMaster WHERE DefectTime >= CAST( '" & fromdate.ToString("yyyy-MM-dd") & "' AS DATETIME) AND DefectTime <= CAST( '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "' AS DATETIME) ORDER BY DefectID DESC")
                                End If
                                lastCacheObjID = (From v In cacheobj Select v Order By v.DefectID Descending).ToArray()
                            Else
                                If objdm.todate = Date.Now.ToString("MM/dd/yyyy") Then
                                    lastdefectid = (From v In InspectionInputDAO.InspectionCache Where v.CID = objdm.CID Select v).ToArray()
                                Else
                                    listso = bmapso.GetInspectObject("SELECT TOP(1) DefectID AS Object1 FROM DefectMaster WHERE Location = '" & objdm.CID.ToString() & "' AND DefectTime >= CAST( '" & fromdate.ToString("yyyy-MM-dd") & "' AS DATETIME) AND DefectTime <= CAST( '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "' AS DATETIME) ORDER BY DefectID DESC")
                                End If
                                lastCacheObjID = (From v In cacheobj Where v.Location = objdm.CID Select v Order By v.DefectID Descending).ToArray()
                            End If
                            
                            If listso.Count > 0 Then
                                lasttabledefectid = CType(listso.ToArray()(0).Object1, Integer)
                            Else
                                lasttabledefectid = 0
                            End If
                            
                            If lastdefectid.Length > 0 Then
                                If lastdefectid(0).LastDefectID <= lastCacheObjID(0).DefectID Then
                                    If objdm.CID = "999" Then
                                        listdm = cacheobj
                                    Else
                                        listdm = (From v In cacheobj Where v.Location = objdm.CID Select v).ToList()
                                    End If
                                End If
                            ElseIf listso.Count > 0 Then
                                If lasttabledefectid <= lastCacheObjID(0).DefectID Then
                                    If objdm.CID = "999" Then
                                        listdm = cacheobj
                                    Else
                                        listdm = (From v In cacheobj Where v.Location = objdm.CID Select v).ToList()
                                    End If
                                End If
                            ElseIf lastdefectid.Length = 0 And lastCacheObjID.Length > 0 Then
  
                                InspectionInputDAO.InspectionCache.Add(New SPCInspection.InspectionCacheVar With {.CID = objdm.CID, .LastDefectID = lastCacheObjID(0).DefectID, .LastDefectIDTimeStamp = Date.Now})
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    If listdm.Count = 0 Then
                        Dim wholelist As New List(Of SPCInspection.DefectMasterDisplay)
                        
                        wholelist = IU.GetDefectMasterData3(fromdate, todate)
                        
                        context.Cache.Insert("DefectMasterLoad" + CacheString, wholelist, Nothing, Date.Now.AddDays(14), System.Web.Caching.Cache.NoSlidingExpiration)
                        
                        If objdm.CID = "999" Then
                            listdm = wholelist
                        Else
                            listdm = (From v In wholelist Where v.Location = objdm.CID Select v).ToList()
                        End If
                    End If
                End If
                
                
            End If
            context.Response.Write(jser.Serialize(listdm))
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    
    
End Namespace
