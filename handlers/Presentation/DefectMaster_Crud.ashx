<%@ WebHandler Language="VB" Class="core.DefectMaster_Crud" %>

Imports System
Imports System.Web
Imports System.Data.Entity
Imports System.Threading.Tasks
Imports System.Web.Script.Serialization

Namespace core

    Public Class dmcrud
        Public Property id As Integer
        Public Property dmId As Integer
        Public Property DefectID As Integer
        Public Property oper As Object
        Public Property EmployeeNo As String
        Public Property DefectDesc As String
        Public Property Product As String
        Public Property WorkRoom As String
        Public Property DefectClass As String
    End Class

    Public Class DefectMaster_Crud : Implements IHttpHandler, IRequiresSessionState

        Private _db As Inspection_TestEntities
        Private InspectInput As New InspectionInputDAO
        Private _Manager As AprManager_Entities

        Public Sub New()
            _db = New Inspection_TestEntities()
            _Manager = New AprManager_Entities()

        End Sub

        Dim ErrMessage As String
        Dim ErrFlag As Boolean = False
        Dim dmObj As dmcrud
        Dim RequestParams As NameValueCollection
        Dim RowsAffected As Integer
        Dim ijsid As Integer
        Dim ijsObj As InspectionJobSummary
        Dim orinaldm As DefectMaster
        Dim originaldmString As String
        Dim finaldmString As String
        Dim jser As New JavaScriptSerializer

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            context.Response.ContentType = "text/plain"

            RequestParams = context.Request.Params

            If RequestParams.Count > 0 Then
                Dim bmappss As New BMappers(Of dmcrud)
                dmObj = bmappss.GetReqParamAsObject(RequestParams)
                If Not dmObj Is Nothing Then
                    If dmObj.id = 0 Then
                        If dmObj.DefectID > 0 Then
                            dmObj.id = dmObj.DefectID
                        Else
                            Throw New Exception("DmId cannot be zero while editing or deleting a row.")
                            ErrFlag = True
                        End If
                    End If
                    setijsId()
                    If dmObj.id > 0 Then
                        setdmObj()
                    End If

                    If dmObj.oper = "del" Then
                        Try
                            DeldmObject()
                            If RowsAffected > 0 Then
                                UpdateInspectionJob()
                                context.Response.Write("success")
                            End If
                        Catch ex As Exception
                            ErrFlag = True
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                        End Try
                    ElseIf dmObj.oper = "edit" Then
                        EditdmObject()
                        If RowsAffected > 0 Then
                            context.Response.Write("success")
                        End If
                    End If

                End If

            End If
            If ErrFlag = True Then
                context.Response.Write("failure")
            End If

        End Sub

        Private Sub DeldmObject()
            Dim listdm As DefectMaster = (From x In _db.DefectMasters Where x.DefectID = dmObj.id Select x).FirstOrDefault()
            originaldmString = GetdmSerial(orinaldm)
            _db.DefectMasters.Remove(listdm)
            RowsAffected = _db.SaveChanges()
            If RowsAffected > 0 Then
                UpdateInspectionJob()
                LogTransaction("Delete", Nothing)
            End If
        End Sub

        Private Sub EditdmObject()
            Dim updateJobFlag As Boolean = False
            If Not orinaldm.DefectClass.ToUpper().Trim.ToString() = dmObj.DefectClass.ToUpper().Trim.ToString() Then
                updateJobFlag = True
            End If
            originaldmString = GetdmSerial(orinaldm)
            orinaldm.EmployeeNo = dmObj.EmployeeNo
            orinaldm.DefectDesc = dmObj.DefectDesc
            orinaldm.Product = dmObj.Product
            orinaldm.WorkRoom = dmObj.WorkRoom
            orinaldm.DefectClass = dmObj.DefectClass
            RowsAffected += _db.SaveChanges()
            finaldmString = GetdmSerial(orinaldm)
            If RowsAffected > 0 Then
                LogTransaction("Delete", orinaldm)
            End If
            If updateJobFlag = True Then
                UpdateInspectionJob()
            End If

        End Sub
        Private Function GetdmSerial(listin As DefectMaster) As String

            Dim listretobj As New DefectMaster

            listretobj.EmployeeNo = listin.EmployeeNo
            listretobj.DefectDesc = listin.DefectDesc
            listretobj.Product = listin.Product
            listretobj.WorkRoom = listin.WorkRoom
            listretobj.DefectClass = listin.DefectClass

            Return jser.Serialize(listretobj)

        End Function
        Private Sub UpdateInspectionJob()
            If Not ijsObj Is Nothing Then
                If ijsObj.Inspection_Finished < ijsObj.Inspection_Started Or ijsObj.Inspection_Finished Is Nothing Then
                    ijsObj.ItemFailCount = (From x In _db.DefectMasters Where x.InspectionJobSummaryId = ijsid Select x).Count()
                    Dim retin = _db.SaveChanges()
                Else
                    ijsObj.MajorsCount = InspectInput.GetDefectCountByType(ijsid.ToString(), "MAJOR")
                    ijsObj.MinorsCount = InspectInput.GetDefectCountByType(ijsid.ToString(), "MINOR")
                    ijsObj.RepairsCount = InspectInput.GetDefectCountByType(ijsid.ToString(), "REPAIRS")
                    ijsObj.ScrapCount = InspectInput.GetDefectCountByType(ijsid.ToString(), "SCRAP")
                    ijsObj.ItemFailCount = (From x In _db.DefectMasters Where x.InspectionJobSummaryId = ijsid Select x).Count()

                    Dim retin = _db.SaveChanges()
                End If

            End If
        End Sub

        Public Async Sub LogTransaction(TransType As String, Optional finalobject As Object = "")
            Dim aprapp As New APRWebApp
            'Dim usercookie = aprapp.GetCookie("APR_UserActivityLog", "PrimaryKey")
            Dim returnint As Integer = -1

            Dim ualobject = Await GetUserActivityLog()
            If Not ualobject Is Nothing Then
                Dim ualobject_new As New UserActivityLog
                ualobject_new.DBOrigin = "Inspection"
                ualobject_new.UserID = ualobject.UserID
                ualobject_new.EntryTimestamp = DateTime.Now
                ualobject_new.DeviceType = "WebApp"
                ualobject_new.IPAddress = ""
                ualobject_new.CID = ijsObj.CID
                ualobject_new.ActivityType = TransType
                _Manager.UserActivityLogs.Add(ualobject_new)
                Dim sveobj = Await SaveChangesAsync()
                If ualobject_new.id > 0 Then
                    Dim ucalobject As New UserCrudActivityLog
                    ucalobject.oper = TransType
                    ucalobject.UserActivityLogId = ualobject_new.id
                    ucalobject.Timestamp = DateTime.Now
                    ucalobject.OriginalObject = originaldmString
                    ucalobject.FinalObject = finaldmString
                    ucalobject.DataBase = "Inspection"
                    ucalobject.Table = "DefectMaster"
                    ucalobject.PrimaryKeyTarget = orinaldm.DefectID
                    _Manager.UserCrudActivityLogs.Add(ucalobject)
                    Dim sveobj2 = Await SaveChangesAsync()

                End If
            End If

        End Sub
        Private Function GetUserActivityLog() As Task(Of UserActivityLog)
            Return Task.Factory.StartNew(Function()
                                             Return (From v In _Manager.UserActivityLogs Where v.id = 13688 Select v).FirstOrDefault()
                                         End Function)
        End Function
        Private Function SaveChangesAsync() As Task(Of Integer)
            Return Task.Factory.StartNew(Function()
                                             Return _Manager.SaveChanges()
                                         End Function)

        End Function

        Private Sub setijsId()

            Dim arraydm = (From x In _db.DefectMasters Where x.DefectID = dmObj.id Select x.InspectionJobSummaryId).ToArray()

            If arraydm.Length > 0 Then
                ijsid = arraydm(0).Value
                If ijsid > 0 Then
                    setijsObj()
                End If
            Else
                ijsid = -1
            End If

        End Sub
        Private Sub setijsObj()
            ijsObj = (From x In _db.InspectionJobSummaries Where x.id = ijsid Select x).FirstOrDefault()
        End Sub
        Private Sub setdmObj()
            orinaldm = (From x In _db.DefectMasters Where x.DefectID = dmObj.id Select x).FirstOrDefault()
        End Sub
        Private Function GetdmidByRow(rownumber As Integer) As Integer
            Dim returnint As Integer = 0

            Dim caceobj = HttpContext.Current.Cache("ijsSubGrid_Active." + HttpContext.Current.Session.SessionID.ToString())
            If Not caceobj Is Nothing Then
                Dim dmlist As List(Of SPCInspection.DefectMasterSubgrid)
                dmlist = caceobj
                If dmlist.Count > 0 Then
                    Dim query = dmlist.Select(Function(dm, index) New With {index, .dmid = dm.DefectID}).Where(Function(x) x.index = dmObj.id - 1).Select(Function(x) x.dmid).ToArray()
                    If query.Length > 0 Then
                        Return query(0)
                    End If
                End If
            End If

            Return returnint
        End Function

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
End Namespace