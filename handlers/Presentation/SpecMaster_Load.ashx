<%@ WebHandler Language="VB" Class="core.SpecMaster_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core
    Public Class SpecMasterLoad
        Public Property CID As String
        Public Property fromdate As String
        Public Property todate As String
    End Class
    
    Public Class SpecMaster_Load : Implements IHttpHandler, IRequiresSessionState
        
        Private II As New InspectionInputDAO
        Private IU As New InspectionUtilityDAO
        
        Dim objdm As New SpecMasterLoad
        
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim jser As New JavaScriptSerializer
            Dim listdm As New List(Of SPCInspection.ProductDisplaySpecCollection)
            
            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of SpecMasterLoad)
                
                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                If Len(objdm.fromdate) > 0 And Len(objdm.todate) > 0 And Len(objdm.CID) > 0 Then
                    Dim fromdate As DateTime = Date.Parse(objdm.fromdate)
                    Dim todate As DateTime = Date.Parse(objdm.todate)
                    Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd")
                    Try

                        Dim cacheobj As List(Of SPCInspection.ProductDisplaySpecCollection) = context.Cache("SpecMasterLoad" + CacheString)
                        If IsNothing(cacheobj) = False Then
                            Dim lastdefectid As Object = New Object() {}
                            Dim lasttabledefectid As Integer
                            Dim lastCacheObjID As Object
                            Dim listso As New List(Of SingleObject)
                            Dim bmapso As New BMappers(Of SingleObject)
                            
                            If objdm.CID = "999" Then
                                If objdm.todate = Date.Now.ToString("MM/dd/yyyy") Then
                                    lastdefectid = (From v In InspectionInputDAO.SpecCache Select v Order By v.LastDefectID Descending).ToArray()
                                Else
                                    listso = bmapso.GetInspectObject("SELECT TOP(1) id AS Object1 FROM SpecMeasurements WHERE Timestamp >= CAST( '" & fromdate.ToString("yyyy-MM-dd") & "' AS DATETIME) AND Timestamp <= CAST( '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "' AS DATETIME) ORDER BY id DESC")
                                End If
                                lastCacheObjID = (From v In cacheobj Select v Order By v.DefectId Descending).ToArray()
                            Else
                                If objdm.todate = Date.Now.ToString("MM/dd/yyyy") Then
                                    lastdefectid = (From v In InspectionInputDAO.SpecCache Where v.CID.Trim() = objdm.CID Select v).ToArray()
                                Else
                                    listso = bmapso.GetInspectObject("SELECT TOP(1) sm.id AS Object1 FROM SpecMeasurements sm INNER JOIN InspectionJobSummary ijs ON sm.InspectionJobSummaryId = ijs.id WHERE ijs.CID = '" & objdm.CID & "' AND sm.Timestamp >= CAST( '" & fromdate.ToString("yyyy-MM-dd") & "' AS DATETIME) AND sm.Timestamp <= CAST( '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "' AS DATETIME) ORDER BY sm.id DESC")
                                End If
                                
                                lastCacheObjID = (From v In cacheobj Where v.Location.Trim() = objdm.CID Select v Order By v.DefectId Descending).ToArray()
                            End If
                            
                            If listso.Count > 0 Then
                                lasttabledefectid = CType(listso.ToArray()(0).Object1, Integer)
                            Else
                                lasttabledefectid = 0
                            End If
                            
                            If lastdefectid.Length > 0 Then
                                If lastdefectid(0).LastDefectID <= lastCacheObjID(0).id Then
                                    If objdm.CID = "999" Then
                                        listdm = cacheobj
                                    Else
                                        listdm = (From v In cacheobj Where v.Location.Trim() = objdm.CID Select v).ToList()
                                    End If
                                End If
                            ElseIf listso.Count > 0 Then
                                If lasttabledefectid <= lastCacheObjID(0).DefectID Then
                                    If objdm.CID = "999" Then
                                        listdm = cacheobj
                                    Else
                                        listdm = (From v In cacheobj Where v.Location.Trim() = objdm.CID Select v).ToList()
                                    End If
                                End If
                            ElseIf lastdefectid.Length = 0 And lastCacheObjID.Length > 0 Then
                                InspectionInputDAO.SpecCache.Add(New SPCInspection.InspectionCacheVar With {.CID = objdm.CID, .LastDefectID = lastCacheObjID(0).id, .LastDefectIDTimeStamp = Date.Now})
                              
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    If listdm.Count = 0 Then
                        Dim wholelist As New List(Of SPCInspection.ProductDisplaySpecCollection)
                        
                        wholelist = IU.GetDisplaySpecCollection2(objdm.CID, objdm.fromdate, objdm.todate)
                        
                        context.Cache.Insert("SpecMasterLoad" + CacheString, wholelist, Nothing, Date.Now.AddDays(14), System.Web.Caching.Cache.NoSlidingExpiration)
                        
                        If objdm.CID = "999" Then
                            listdm = wholelist
                        Else
                            listdm = (From v In wholelist Where v.Location.Trim() = objdm.CID Select v).ToList()
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
