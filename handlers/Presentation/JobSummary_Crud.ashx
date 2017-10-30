<%@ WebHandler Language="VB" Class="core.JobSummary_Crud" %>

Imports System
Imports System.Web
Imports System.Data.Entity
Imports System.Threading.Tasks
Imports System.Web.Script.Serialization

Namespace core
    Public Class ijscrud
        Public Property Id As Integer
        Public Property IjsId As Integer
        Public Property JobNumber As String
        Public Property LineType As String
        Public Property Location As String
        Public Property TotalInspectedItems As Integer
        Public Property APR_UserActivityLog As String
        Public Property ItemPassCount As Integer
        Public Property ItemFailCount As Integer
        Public Property WOQuantity As Integer
        Public Property PRP_Code As String
        Public Property WorkOrderPieces As Integer
        Public Property AQL_Level As String
        Public Property SampleSize As Integer
        Public Property RejectLimiter As Integer
        Public Property Technical_PassFail As Object
        Public Property UnitCost As Double
        Public Property Comments As String
        Public Property SessionId As String
        Public Property oper As Object
    End Class

    Public Class JobSummary_Crud : Implements IHttpHandler, IRequiresSessionState
        Private _db As Inspection_Entities
        Private _Manager As AprManager_Entities

        Public Sub New()
            _db = New Inspection_Entities()
            _Manager = New AprManager_Entities()
        End Sub

        Dim ErrMessage As String
        Dim ErrFlag As Boolean = False
        Dim ijsObj As ijscrud
        Dim RequestParams As NameValueCollection
        Dim listijs As InspectionJobSummary
        Dim orinalijs As String = ""
        Dim finalijs As String = ""
        Dim UserActivityLogId As Integer = 0
        Dim RowsAffected As Integer
        Dim jser As New javascriptserializer

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            RequestParams = context.Request.Params
            context.Response.ContentType = "text/plain"

            If RequestParams.Count > 0 Then
                Dim bmappss As New BMappers(Of ijscrud)
                ijsObj = bmappss.GetReqParamAsObject(RequestParams)

                If IsNothing(ijsObj) = False Then
                    ' ParseUserCookie() ' undo when live
                    If ijsObj.IjsId = 0 Then
                        If ijsObj.Id = 0 Then
                            Throw New Exception("both Id and IjsId cannot be zero")
                        End If
                        ijsObj.IjsId = GetijsidByRow(ijsObj.Id)
                    End If
                    If ijsObj.oper = "edit" Then
                        Try
                            EditIjsOject()
                            If RowsAffected > 0 Then
                                context.Response.Write("success")
                            End If
                        Catch ex As Exception
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                        End Try
                    ElseIf ijsObj.oper = "del" Then
                        Try
                            DeldmObject()
                            DelSpecObjects()
                            DelIjsObject()
                            If RowsAffected > 0 Then
                                context.Response.Write("success")
                            End If
                        Catch ex As Exception
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                        End Try
                    End If

                End If
            End If
            If ErrFlag = True Then
                context.Response.Write("failure")
            End If


        End Sub

        Private Sub DelIjsObject()
            listijs = (From x In _db.InspectionJobSummaries Where x.id = ijsObj.IjsId Select x).FirstOrDefault()
            Dim newList As New List(Of InspectionJobSummary)
            newList.Add(listijs)
            DelijsCace(newList)
            Try

                _db.InspectionJobSummaries.RemoveRange(_db.InspectionJobSummaries.Where(Function(x) x.id = ijsObj.IjsId).ToList())
                RowsAffected += _db.SaveChanges()
                If RowsAffected > 0 Then
                    LogTransaction("Delete")
                End If

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try

        End Sub

        Private Sub DelSpecObjects()

            Try

                _db.SpecMeasurements.RemoveRange(_db.SpecMeasurements.Where(Function(x) x.InspectionJobSummaryId = ijsObj.IjsId).ToList())
                _db.SaveChanges()

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End Sub
        Private Sub DeldmObject()

            Try

                _db.DefectMasters.RemoveRange(_db.DefectMasters.Where(Function(x) x.InspectionJobSummaryId = ijsObj.IjsId).ToList())
                RowsAffected += _db.SaveChanges()

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try

        End Sub
        Private Sub DelijsCace(targetList As List(Of InspectionJobSummary))
            If Not targetList Is Nothing Then
                Try
                    Dim tardate As DateTime
                    If Not targetList.ToArray()(0) Is Nothing Then
                        tardate = targetList.ToArray()(0).Inspection_Started
                        Dim listijs As List(Of SPCInspection.InspectionSummaryDisplay) = HttpContext.Current.Cache("JobSummaryLoad1" + tardate.Year.ToString + "." + tardate.DayOfYear.ToString())
                        If listijs.Count > 0 Then
                            If ijsObj.oper = "del" Then
                                Dim listret As List(Of SPCInspection.InspectionSummaryDisplay) = (From x In listijs Where x.ijsid <> ijsObj.IjsId Select x).ToList()
                                HttpRuntime.Cache.Insert("JobSummaryLoad1" + tardate.Year.ToString + "." + tardate.DayOfYear.ToString(), listret, Nothing, Date.Now.AddDays(4), System.Web.Caching.Cache.NoSlidingExpiration)
                            ElseIf ijsObj.oper = "edit" Then
                                Dim listret As New List(Of SPCInspection.InspectionSummaryDisplay)
                                listret = (From x In listijs Where x.ijsid = ijsObj.IjsId Select x).ToList()
                                If Not targetList Is Nothing Then
                                    If targetList.Count > 0 And listret.Count > 0 Then
                                        Dim TarArray = targetList.ToArray()
                                        For Each item In listret
                                            item.TotalInspectedItems = TarArray(0).TotalInspectedItems
                                            item.ItemPassCount = TarArray(0).ItemPassCount
                                            item.WOQuantity = TarArray(0).WOQuantity
                                            item.WorkOrderPieces = TarArray(0).WorkOrderPieces
                                            item.AQL_Level = TarArray(0).AQL_Level
                                            item.SampleSize = TarArray(0).SampleSize
                                            item.RejectLimiter = TarArray(0).RejectLimiter
                                        Next
                                        HttpRuntime.Cache.Insert("JobSummaryLoad1" + tardate.Year.ToString + "." + tardate.DayOfYear.ToString(), listret, Nothing, Date.Now.AddDays(4), System.Web.Caching.Cache.NoSlidingExpiration)
                                    End If
                                End If

                            End If
                        End If
                    End If
                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                End Try

            End If

        End Sub

        Private Sub EditIjsOject()
            Dim DA As New InspectionInputDAO
            Dim UpdateFlag As Boolean = False
            Dim InspectVars As New List(Of SPCInspection.InspectionVaribles)

            listijs = (From x In _db.InspectionJobSummaries Where x.id = ijsObj.IjsId Select x).FirstOrDefault()
            If Not listijs Is Nothing Then
                If listijs.AQL_Level <> ijsObj.AQL_Level Then
                    UpdateFlag = True
                End If
                orinalijs = GetijsSerial(listijs)
                'Dim LocObj = (From x In _Manager.LocationMasters Where x.Abreviation = ijsObj.Location Select x)
                'If Not LocObj Is Nothing Then
                '    Dim locret = (From x In LocObj Select x.CID).ToArray()
                '    If locret.Length > 0 Then
                '        listijs.CID = ijsObj.Location
                '    End If
                'End If
                listijs.CID = ijsObj.Location.Trim()
                listijs.JobNumber = ijsObj.JobNumber
                'listijs.JobType = ijsObj.LineType
                listijs.TotalInspectedItems = ijsObj.TotalInspectedItems
                listijs.ItemPassCount = ijsObj.ItemPassCount
                listijs.ItemFailCount = ijsObj.ItemFailCount
                listijs.PRP_Code = ijsObj.PRP_Code
                listijs.WOQuantity = ijsObj.WOQuantity
                listijs.WorkOrderPieces = ijsObj.WorkOrderPieces
                listijs.AQL_Level = ijsObj.AQL_Level

                If ijsObj.UnitCost > 0 Then
                    listijs.UnitCost = ijsObj.UnitCost
                End If

                If UpdateFlag = True Then
                    Dim jser As New JavaScriptSerializer()
                    Select Case listijs.Standard
                        Case "Reduced"
                            InspectVars = DA.SetReducedSamplesSize(listijs.WOQuantity, ijsObj.AQL_Level)
                        Case "Regular"
                            InspectVars = DA.SetSampleSize(listijs.WOQuantity, ijsObj.AQL_Level)
                        Case "Tightened"
                            InspectVars = DA.SetTightSamplesSize(listijs.WOQuantity, ijsObj.AQL_Level)
                        Case Else
                            InspectVars = DA.SetTightSamplesSize(listijs.WOQuantity, ijsObj.AQL_Level)
                    End Select
                    If Not InspectVars Is Nothing Then
                        If InspectVars.Count > 0 Then
                            listijs.SampleSize = InspectVars.ToArray()(0).SampleSize
                            listijs.RejectLimiter = InspectVars.ToArray()(0).RE
                        End If
                    End If

                Else
                    listijs.SampleSize = ijsObj.SampleSize
                End If

                listijs.RejectLimiter = ijsObj.RejectLimiter
                RowsAffected += _db.SaveChanges()
                Try
                    Dim listin As New List(Of InspectionJobSummary)
                    listin.Add(listijs)
                    DelijsCace(listin)

                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                End Try


                finalijs = GetijsSerial(listijs)
            Else
                Elmah.ErrorSignal.FromCurrentContext.Raise(New Exception("could not find InspectionSummary row in _db ijsid: " + ijsObj.IjsId.ToString()))
            End If
            If RowsAffected > 0 Then
                LogTransactionAsync("Edit", listijs)
            End If
        End Sub
        Private Function GetijsSerial(listin As InspectionJobSummary) As String

            Dim listretobj As New InspectionJobSummary

            listretobj.TotalInspectedItems = listin.TotalInspectedItems
            listretobj.ItemPassCount = listin.ItemPassCount
            listretobj.WOQuantity = listin.WOQuantity
            listretobj.WorkOrderPieces = listin.WorkOrderPieces
            listretobj.AQL_Level = listin.AQL_Level
            listretobj.SampleSize = listin.SampleSize

            Return jser.Serialize(listretobj)

        End Function

        Private Sub LogTransactionAsync(TransType As String, Optional finalobject As Object = "")
            Try
                Dim t As System.Threading.Tasks.Task = System.Threading.Tasks.Task.Run(Sub()
                                                                                           LogTransaction(TransType, finalobject)
                                                                                       End Sub)
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End Sub
        Public Sub LogTransaction(TransType As String, Optional finalobject As Object = "")
            Dim aprapp As New APRWebApp
            'Dim usercookie = aprapp.GetCookie("APR_UserActivityLog", "PrimaryKey")
            Dim returnint As Integer = -1
            Dim ualobject = GetUserActivityLog()
            If Not ualobject Is Nothing Then
                Dim ualobject_new As New UserActivityLog
                ualobject_new.DBOrigin = "Inspection"
                ualobject_new.UserID = ualobject.UserID
                ualobject_new.EntryTimestamp = DateTime.Now
                ualobject_new.DeviceType = "WebApp"
                ualobject_new.IPAddress = ""
                ualobject_new.CID = listijs.CID
                ualobject_new.ActivityType = TransType
                _Manager.UserActivityLogs.Add(ualobject_new)
                Dim sveobj = _Manager.SaveChanges()
                If ualobject_new.id > 0 Then
                    Dim ucalobject As New UserCrudActivityLog
                    ucalobject.oper = TransType
                    ucalobject.UserActivityLogId = ualobject_new.id
                    ucalobject.Timestamp = DateTime.Now

                    ucalobject.OriginalObject = orinalijs  ' jser.Serialize(OriginalList)
                    ucalobject.FinalObject = finalijs
                    ucalobject.DataBase = "Inspection"
                    ucalobject.Table = "InspectionJobSummary"
                    ucalobject.PrimaryKeyTarget = listijs.id
                    _Manager.UserCrudActivityLogs.Add(ucalobject)
                    Dim sveobj2 = _Manager.SaveChanges()

                End If
            End If

        End Sub
        Private Function GetUserActivityLog() As UserActivityLog
            Return (From v In _Manager.UserActivityLogs Where v.id = UserActivityLogId Select v).FirstOrDefault()
        End Function
        Private Function SaveChangesAsync() As Task(Of Integer)
            Return Task.Factory.StartNew(Function()
                                             Return _Manager.SaveChanges()
                                         End Function)

        End Function
        Private Function GetijsidByRow(rownumber As Integer) As Integer
            Dim returnint As Integer = 0

            Dim caceobj = HttpContext.Current.Cache("TableJobSummaryDisplayed." + HttpContext.Current.Session.SessionID.ToString())
            If Not caceobj Is Nothing Then
                Dim ijslist As List(Of SPCInspection.InspectionSummaryDisplay)
                ijslist = caceobj
                If ijslist.Count > 0 Then
                    Dim query = ijslist.Select(Function(ijs, index) New With {index, .ijsid = ijs.ijsid}).Where(Function(x) x.index = ijsObj.Id - 1).Select(Function(x) x.ijsid).ToArray()
                    If query.Length > 0 Then
                        Return query(0)
                    End If
                End If
            End If

            Return returnint
        End Function
        Private Sub ParseUserCookie()
            Dim Splitstring = ijsObj.APR_UserActivityLog.ToString().Split("&")
            If Splitstring.Length > 0 Then
                Dim splobj = Splitstring(0).Split("=")
                If splobj.Length > 0 Then
                    UserActivityLogId = Convert.ToInt64(splobj(1))
                End If
            End If
        End Sub


        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
End Namespace
