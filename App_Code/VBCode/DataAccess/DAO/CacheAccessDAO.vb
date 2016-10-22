Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.Caching

Namespace core

    Public Class CacheAccessDAO : Implements IRequiresSessionState

        Private II As New InspectionInputDAO
        Private IU As New InspectionUtilityDAO

        Public Function GetDefectMasterList(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.DefectMasterDisplay)

            Dim listdm As New List(Of SPCInspection.DefectMasterDisplay)
            Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd") + "." + System.Web.HttpContext.Current.Session.SessionID
            Try

                Dim cacheobj As List(Of SPCInspection.DefectMasterDisplay) = HttpRuntime.Cache("DefectMasterLoad" + CacheString)
                If IsNothing(cacheobj) = False Then
                    Dim lastdefectid As Object = New Object() {}
                    Dim lasttabledefectid As Integer
                    Dim lastCacheObjID As Object
                    Dim listso As New List(Of SingleObject)
                    Dim bmapso As New BMappers(Of SingleObject)


                    If todate = Date.Now.ToString("MM/dd/yyyy") Then
                        lastdefectid = (From v In InspectionInputDAO.InspectionCache Select v Order By v.LastDefectID Descending).ToArray()
                    Else
                        listso = bmapso.GetInspectObject("SELECT TOP(1) DefectID AS Object1 FROM DefectMaster WHERE DefectTime >= CAST( '" & fromdate.ToString("yyyy-MM-dd") & "' AS DATETIME) AND DefectTime <= CAST( '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "' AS DATETIME) ORDER BY DefectID DESC")
                    End If
                    lastCacheObjID = (From v In cacheobj Select v Order By v.DefectID Descending).ToArray()

                    If listso.Count > 0 Then
                        lasttabledefectid = CType(listso.ToArray()(0).Object1, Integer)
                    Else
                        lasttabledefectid = 0
                    End If

                    If lastdefectid.Length > 0 Then
                        If lastdefectid(0).LastDefectID <= lastCacheObjID(0).DefectID Then
                            listdm = cacheobj
                        End If
                    ElseIf listso.Count > 0 Then
                        If lasttabledefectid <= lastCacheObjID(0).DefectID Then
                            listdm = cacheobj
                        End If
                    ElseIf lastdefectid.Length = 0 And lastCacheObjID.Length > 0 Then

                        InspectionInputDAO.InspectionCache.Add(New SPCInspection.InspectionCacheVar With {.LastDefectID = lastCacheObjID(0).DefectID, .LastDefectIDTimeStamp = Date.Now})
                    End If
                End If
            Catch ex As Exception

            End Try
            If listdm.Count = 0 Then
                Dim wholelist As New List(Of SPCInspection.DefectMasterDisplay)

                wholelist = IU.GetDefectMasterData3(fromdate, todate)



                listdm = wholelist

            End If
            '    End If
            HttpRuntime.Cache.Insert("DefectMasterReport" + CacheString, listdm, Nothing, Date.Now.AddDays(14), System.Web.Caching.Cache.NoSlidingExpiration)

            Return listdm

        End Function

        Public Function JobSummaryList(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)
            Dim lastdefectidFinal As Integer
            Dim listdm As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim CacheString As String = todate.ToString("yy-MM-dd") + "." + fromdate.ToString("yy-MM-dd")
            Try

                Dim cacheobj As List(Of SPCInspection.InspectionSummaryDisplay) = HttpRuntime.Cache("JobSummaryLoad" + CacheString)
                Dim Lastidcacheobj As Integer = HttpRuntime.Cache("JobSummaryLoad_Lastid" + CacheString)
                If IsNothing(cacheobj) = False And IsNothing(Lastidcacheobj) = False Then
                    Dim lastdefectid As Object = New Object() {}
                    Dim lasttabledefectid As Integer

                    Dim lastCacheObjID As Object
                    Dim listso As New List(Of SingleObject)
                    Dim bmapso As New BMappers(Of SingleObject)


                    If todate = Date.Now.ToString("MM/dd/yyyy") Then
                        lastdefectid = (From v In InspectionInputDAO.InspectionCache Select v Order By v.LastDefectID Descending).ToArray()
                    Else
                        listso = bmapso.GetInspectObject("SELECT TOP(1) DefectID AS Object1 FROM DefectMaster WHERE DefectTime >= CAST( '" & fromdate.ToString("yyyy-MM-dd") & "' AS DATETIME) AND DefectTime <= CAST( '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "' AS DATETIME) ORDER BY DefectID DESC")
                    End If
                    lastCacheObjID = Lastidcacheobj

                    If listso.Count > 0 Then
                        lasttabledefectid = CType(listso.ToArray()(0).Object1, Integer)
                    Else
                        lasttabledefectid = 0
                    End If

                    If lastdefectid.Length > 0 Then
                        lastdefectidFinal = lastdefectid(0).LastDefectID
                        If lastdefectid(0).LastDefectID <= lastCacheObjID(0).DefectID Then
                            listdm = cacheobj
                        End If
                    ElseIf listso.Count > 0 Then
                        lastdefectidFinal = lasttabledefectid
                        If lasttabledefectid <= lastCacheObjID(0).DefectID Then
                            listdm = cacheobj
                        End If
                    ElseIf lastdefectid.Length = 0 And lastCacheObjID.Length > 0 Then

                        InspectionInputDAO.InspectionCache.Add(New SPCInspection.InspectionCacheVar With {.LastDefectID = lastCacheObjID(0).DefectID, .LastDefectIDTimeStamp = Date.Now})
                    End If
                End If
            Catch ex As Exception

            End Try
            If listdm.Count = 0 Then
                Dim wholelist As New List(Of SPCInspection.InspectionSummaryDisplay)

                wholelist = IU.GetInspectionSummary(fromdate, todate)

                HttpRuntime.Cache.Insert("DefectMasterLoad" + CacheString, wholelist, Nothing, Date.Now.AddDays(14), System.Web.Caching.Cache.NoSlidingExpiration)
                HttpRuntime.Cache.Insert("JobSummaryLoad_Lastid" + CacheString, lastdefectidFinal, Nothing, Date.Now.AddDays(14), System.Web.Caching.Cache.NoSlidingExpiration)

                listdm = wholelist

            End If
            '    End If

            Return listdm

        End Function

    End Class

End Namespace

