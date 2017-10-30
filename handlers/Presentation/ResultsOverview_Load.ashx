<%@ WebHandler Language="VB" Class="core.ResultsOverview_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core

    Public Class ResultsOverview
        Public Property SessionID As String
        Public Property CID As String
        Public Property todate As String
        Public Property DataNumber As String
        Public Property AuditType As String
        Public Property PrpCode As String
        Public Property ActiveFilterField As String
        Public Property FilterFlag As String
        Public Property FilterColumnName As String
        Public Property Filterqvalue As Object
        Public Property FilterSource As String
        Public Property ytd_DateFrom_fc As String
        Public Property ytd_DateTo_fc As String
        Public Property mtd_DateFrom_fc As String
        Public Property mtd_DateTo_fc As String
        Public Property LocationArrayString As String
        Public Property FilterArrayString As String
        Public Property FilterListstring As String
    End Class

    Public Class ResultsOverview_Load : Implements IHttpHandler, IRequiresSessionState

        Dim jser As New JavaScriptSerializer()
        Dim Inspect As New InspectionUtilityDAO
        Dim objro As New ResultsOverview
        Dim listot As New List(Of SPCInspection.OverTable)
        'Dim listwocthismonth As New List(Of SPCInspection.WorkOrderCompliance)
        'Dim listwocprevmonth As New List(Of SPCInspection.WorkOrderCompliance)
        Dim ytdLot As Object = 0
        Dim mtdLot As Object = 0
        Dim mtdRR As Object = 0
        Dim ytdRR As Object = 0
        Dim mtdDHU As Object = 0
        Dim ytdDHU As Object = 0
        Dim ytdCom As Object = 0
        Dim mtdhCom As Object = 0
        Dim ytdLot_fc As Object = 0
        Dim mtdLot_fc As Object = 0
        Dim mtdRR_fc As Object = 0
        Dim ytdRR_fc As Object = 0
        Dim mtdDHU_fc As Object = 0
        Dim ytdDHU_fc As Object = 0
        Dim ytdCom_fc As Object = 0
        Dim mtdhCom_fc As Object = 0
        Dim CurrentLastId As Object = 0
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim bmapso As New BMappers(Of SingleObject)

            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of ResultsOverview)

                objro = bmapsl.GetReqParamAsObject(RequestParams)
                Dim locationarray As List(Of ActiveLocations)
                Dim filterarray As List(Of ActiveFilterObject)

                Try
                    If Not objro.FilterListstring Is Nothing Then
                        If objro.FilterListstring.Length > 0 Then
                            filterarray = jser.Deserialize(Of List(Of ActiveFilterObject))(objro.FilterListstring)
                        End If
                    End If
                    If Not objro.LocationArrayString Is Nothing Then
                        If objro.LocationArrayString.Length > 0 Then
                            locationarray = jser.Deserialize(Of List(Of ActiveLocations))(objro.LocationArrayString)
                        End If
                    End If


                Catch ex As Exception

                End Try

                If objro.FilterSource = "ColumnFilters" Then


                    Dim mtd_todatedt As DateTime = DateTime.Parse(objro.mtd_DateTo_fc)
                    Dim mtd_fromdatedt As DateTime = DateTime.Parse(objro.mtd_DateFrom_fc)
                    Dim ytd_todatedt As DateTime = DateTime.Parse(objro.ytd_DateTo_fc)
                    Dim ytd_fromdatedt As DateTime = DateTime.Parse(objro.ytd_DateFrom_fc)


                    Try

                        ytdLot_fc = Inspect.GetSingleLotAcc(ytd_fromdatedt, ytd_todatedt, objro.CID, objro.DataNumber, objro.AuditType)
                        ytdRR_fc = Inspect.GetSingleRejectRate(ytd_fromdatedt, ytd_todatedt, objro.CID, objro.DataNumber, objro.AuditType)
                        ytdDHU_fc = Inspect.GetSingleDHURate(ytd_fromdatedt, ytd_todatedt, objro.CID, objro.DataNumber, objro.AuditType)
                        ytdCom_fc = Inspect.GetInspectionCompliancePerc(ytd_fromdatedt, ytd_todatedt, objro.AuditType, objro.DataNumber, Inspect.GetInspectionCompliancePrpCodeFilter(filterarray), Inspect.GetInspectionComplianceLocationFilter(locationarray))
                        mtdLot_fc = Inspect.GetSingleLotAcc(mtd_fromdatedt, mtd_todatedt, objro.CID, objro.DataNumber, objro.AuditType)
                        mtdRR_fc = Inspect.GetSingleRejectRate(mtd_fromdatedt, mtd_todatedt, objro.CID, objro.DataNumber, objro.AuditType)
                        mtdDHU_fc = Inspect.GetSingleDHURate(mtd_fromdatedt, mtd_todatedt, objro.CID, objro.DataNumber, objro.AuditType)
                        mtdhCom_fc = Inspect.GetInspectionCompliancePerc(mtd_fromdatedt, mtd_todatedt, objro.AuditType, objro.DataNumber, Inspect.GetInspectionCompliancePrpCodeFilter(filterarray), Inspect.GetInspectionComplianceLocationFilter(locationarray))

                        Dim CachedStaticTags As List(Of SPCInspection.OverTable) = HttpRuntime.Cache("ResultsOverviewThisMonth.FirstLoad" + objro.SessionID.ToString())

                        If IsNothing(CachedStaticTags) = False Then
                            Dim cstarray = CachedStaticTags.ToArray()
                            If cstarray.Length = 4 Then
                                ytdLot = cstarray(0).ytd
                                ytdRR = cstarray(1).ytd
                                ytdDHU = cstarray(2).ytd
                                ytdCom = cstarray(3).ytd
                                mtdLot = cstarray(0).mtd
                                mtdRR = cstarray(1).mtd
                                mtdDHU = cstarray(2).mtd
                                mtdhCom = cstarray(3).mtd
                            End If
                        End If
                    Catch ex As Exception
                        ytdLot_fc = -1
                        ytdRR_fc = -1
                        ytdDHU_fc = -1
                        ytdCom_fc = -1
                        mtdLot_fc = -1
                        mtdRR_fc = -1
                        mtdDHU_fc = -1
                        mtdhCom_fc = -1
                    End Try
                Else
                    Dim todatedt As DateTime = DateTime.Parse(objro.todate)
                    Dim mtdfd As DateTime = New DateTime(todatedt.Year, DateTime.Now.Month, 1)
                    Dim mtdld As DateTime = DateTime.Now
                    Dim ytdfd As DateTime = New DateTime(todatedt.AddMonths(-1).Year, 1, 1, 0, 0, 0)
                    Dim ytdld As DateTime = DateTime.Now


                    Try
                        If objro.LocationArrayString.Length > 0 Then
                            Dim FilterFlag As Boolean = False

                            If objro.FilterSource = "PageFilter" Then
                                FilterFlag = True
                            End If

                            ytdLot = Inspect.GetLotAcc(ytdfd, ytdld, locationarray, objro.DataNumber, objro.AuditType)
                            ytdRR = Inspect.GetRejectRate(ytdfd, ytdld, locationarray, objro.DataNumber, objro.AuditType)
                            ytdDHU = Inspect.GetDHURate(ytdfd, ytdld, locationarray, objro.DataNumber, objro.AuditType)
                            'ytdCom = Inspect.GetComplianceRate(ytdfd, ytdld, locationarray, FilterFlag, objro.DataNumber, objro.AuditType)
                            ytdCom = Inspect.GetInspectionCompliancePerc(ytdfd, ytdld, objro.AuditType, objro.DataNumber, Inspect.GetInspectionCompliancePrpCodeFilter(filterarray), Inspect.GetInspectionComplianceLocationFilter(locationarray))
                            mtdLot = Inspect.GetLotAcc(mtdfd, mtdld, locationarray, objro.DataNumber, objro.AuditType)
                            mtdRR = Inspect.GetRejectRate(mtdfd, mtdld, locationarray, objro.DataNumber, objro.AuditType)
                            mtdDHU = Inspect.GetDHURate(mtdfd, mtdld, locationarray, objro.DataNumber, objro.AuditType)
                            'mtdhCom = Inspect.GetComplianceRate(mtdfd, mtdld, locationarray, FilterFlag, objro.DataNumber, objro.AuditType)
                            mtdhCom = Inspect.GetInspectionCompliancePerc(mtdfd, mtdld, objro.AuditType, objro.DataNumber, Inspect.GetInspectionCompliancePrpCodeFilter(filterarray), Inspect.GetInspectionComplianceLocationFilter(locationarray))
                        End If

                    Catch ex As Exception
                        ytdLot = -1
                        ytdRR = -1
                        ytdDHU = -1
                        ytdCom = -1
                        mtdLot = -1
                        mtdRR = -1
                        mtdDHU = -1
                        mtdhCom = -1
                    End Try
                End If

                listot.Add(New SPCInspection.OverTable With {.Type = "LotAcceptibility", .ytd = ytdLot, .mtd = mtdLot, .F_ytd = ytdLot_fc, .F_mtd = mtdLot_fc})
                listot.Add(New SPCInspection.OverTable With {.Type = "RejectRate", .ytd = ytdRR, .mtd = mtdRR, .F_ytd = ytdRR_fc, .F_mtd = mtdRR_fc})
                listot.Add(New SPCInspection.OverTable With {.Type = "DHU", .ytd = ytdDHU, .mtd = mtdDHU, .F_ytd = ytdDHU_fc, .F_mtd = mtdDHU_fc})
                listot.Add(New SPCInspection.OverTable With {.Type = "Compliance", .ytd = ytdCom, .mtd = mtdhCom, .F_ytd = ytdCom_fc, .F_mtd = mtdhCom_fc})
                If IsNothing(listot) = False And IsNothing(objro.SessionID) = False Then
                    If listot.Count > 0 Then
                        HttpRuntime.Cache.Insert("ResultsOverviewThisMonth.FirstLoad" + objro.SessionID.ToString(), listot, Nothing, Date.Now.AddDays(3), System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If
            End If

            context.Response.Write(jser.Serialize(listot))
        End Sub


        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace