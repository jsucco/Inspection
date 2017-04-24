<%@ WebHandler Language="VB" Class="SPC_InspectionInput_JobDispatch" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Data.Entity
Imports System.Object
Imports System.Data.Objects

Public Class SPC_InspectionInput_JobDispatch
    Inherits BaseHandler
    Private JobNumber As String
    Private que As List(Of dispatchJob)
    Dim jser As New JavaScriptSerializer
    Public Function OpenJobIfExists(TargetNumber As String, TemplateId As Integer, ByVal InspectionType As String, ByVal AQLVAL As String) As String
        Dim returnMessage As String = "NOJOBS"
        If IsNothing(TargetNumber) = False And TargetNumber.Length > 0 Then
            JobNumber = TargetNumber
            loadQueWithJobs(TemplateId)
            If que.Count > 0 Then
                returnMessage = ProcessJobsInQue(AQLVAL)
            End If
        End If

        Return returnMessage
    End Function

    Public Function GetNewWeaverShiftId() As String
        Return "1"
    End Function

    Public Function StartNewWeaverShift(ByVal shiftId As Integer, ByVal JobSummaryId As Integer, ByVal WeaverInfo As String, ByVal Yards As Integer, ByVal CurrentShiftNumber As Integer) As String
        Dim response As String = "0"

        Dim weavers As New core.SPCInspection.Weavers

        If shiftId = 0 Then
            Elmah.ErrorSignal.FromCurrentContext.Raise(New Exception("shiftId cannot be zero"))
            Return response
        End If

        If JobSummaryId = 0 Then
            Elmah.ErrorSignal.FromCurrentContext.Raise(New Exception("JobSummaryId cannot be zero"))
            Return response
        End If

        If CurrentShiftNumber = 0 Then
            Elmah.ErrorSignal.FromCurrentContext.Raise(New Exception("Current Shift cannot be zero."))
        End If

        EndPreviousShiftAsync(shiftId, Yards)

        If WeaverInfo.Length > 0 Then
            Try
                weavers = jser.Deserialize(Of core.SPCInspection.Weavers)(WeaverInfo)

                Dim NewShiftId = CreateNewShift(CurrentShiftNumber)

                CreateWeaverProductionRecordsAsync(weavers, JobSummaryId, NewShiftId)

                response = NewShiftId.ToString()

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

        Else
            Elmah.ErrorSignal.FromCurrentContext.Raise(New Exception("No WeaverInfo sent"))
        End If

        Return response
    End Function

    Private Sub CreateWeaverProductionRecordsAsync(ByVal weaverInfo As core.SPCInspection.Weavers, ByVal JobSummaryId As Integer, ByVal ShiftId As Integer)
        System.Threading.Tasks.Task.Run(Sub() CreateWeaverProductionRecords(weaverInfo, JobSummaryId, ShiftId))
    End Sub

    Private Sub CreateWeaverProductionRecords(ByVal weaverInfo As core.SPCInspection.Weavers, ByVal JobSummaryId As Integer, ByVal ShiftId As Integer)

        If weaverInfo Is Nothing Then
            Return
        End If
        Using _db As New Inspection_Entities
            Dim weaver1 As New WeaverProduction

            If weaverInfo.Weaver1ID > 0 Then
                weaver1.JobSummaryId = JobSummaryId
                weaver1.ShiftId = ShiftId
                weaver1.EmployeeNoId = weaverInfo.Weaver1ID

                _db.WeaverProductions.Add(weaver1)
            End If


            If weaverInfo.Weaver2ID > 0 Then
                Dim weaver2 As New WeaverProduction

                weaver2.JobSummaryId = JobSummaryId
                weaver2.ShiftId = ShiftId
                weaver2.EmployeeNoId = weaverInfo.Weaver2ID

                _db.WeaverProductions.Add(weaver2)
            End If

            _db.SaveChanges()
        End Using
    End Sub

    Private Function CreateNewShift(ByVal shiftNumber As Integer) As Integer
        Dim shiftId As Integer = 0

        Using _db As New Inspection_Entities
            Dim shift As New WeaverShift

            shift.Start = DateTime.Now
            shift.Shift = shiftNumber + 1
            _db.WeaverShifts.Add(shift)
            _db.SaveChanges()

            shiftId = shift.Id
        End Using

        Return shiftId
    End Function

    Private Sub EndPreviousShiftAsync(ByVal shiftId As Integer, Yards As Integer)
        System.Threading.Tasks.Task.Run(Sub() EndPreviousShift(shiftId, Yards))
    End Sub

    Public Sub EndPreviousShift(ByVal shiftId As Integer, ByVal Yards As Integer)

        Using _db As New Inspection_Entities
            Dim productionRecs = (From x In _db.WeaverProductions Where x.ShiftId = shiftId Select x).ToList()

            For Each shift In productionRecs
                shift.Yards = Yards
            Next

            Dim lastShift = (From x In _db.WeaverShifts Where x.Id = shiftId Select x).FirstOrDefault()

            lastShift.Finish = DateTime.Now

            _db.SaveChanges()
        End Using
    End Sub
    Private Sub loadQueWithJobs(TemplateId As Integer)
        Using _db As New Inspection_Entities
            Dim Summaries As New List(Of dispatchJob)
            que = (From x In _db.InspectionJobSummaries Join t In _db.TemplateNames On x.TemplateId Equals t.TemplateId
                   Where x.JobNumber = JobNumber And x.TemplateId = TemplateId And x.WOQuantity <> 0 Order By x.Inspection_Started Descending
                   Select New dispatchJob With {.id = x.id, .JobNumber = x.JobNumber, .TemplateId = x.TemplateId, .LineType = t.LineType, .InspectionStarted = x.Inspection_Started, .Technical_PassFail = x.Technical_PassFail, .InspectionFinished = x.Technical_PassFail_Timestamp, .WOQuantity = x.WOQuantity, .AQL_Level = x.AQL_Level, .ItemFailCount = x.ItemFailCount, .RejectLimiter = x.RejectLimiter, .SampleSize = x.SampleSize, .Standard = x.Standard}).ToList()
        End Using

    End Sub
    Private Sub ReopenInlineJob()
        Using _db As New Inspection_Entities
            Dim queRecords As dispatchJob()
            queRecords = (From o In que Where o.LineType = "IL").Take(1).ToArray()
            If IsNothing(queRecords) = False And queRecords.Length > 0 Then
                Dim InspectionJob As New InspectionJobSummary
                Dim ijsid = queRecords(0).id
                InspectionJob = (From x In _db.InspectionJobSummaries Where x.id = ijsid).FirstOrDefault()
                If Not InspectionJob.Inspection_Finished Is Nothing Then
                    UpdatedispatchQue()
                    InspectionJob.TotalInspectedItems = Nothing
                    InspectionJob.Technical_PassFail = Nothing
                    InspectionJob.Technical_PassFail_Timestamp = Nothing
                    InspectionJob.UserConfirm_PassFail = Nothing
                    InspectionJob.UserConfirm_PassFail_Timestamp = Nothing
                    InspectionJob.Inspection_Finished = Nothing
                    _db.SaveChanges()
                End If
            End If

        End Using
    End Sub
    Private Sub UpdatedispatchQue()
        que = (From o In que Where o.LineType = "IL").Take(1).ToList()
        que = (From o In que Select New dispatchJob With {.id = o.id, .JobNumber = o.JobNumber, .InspectionStarted = o.InspectionStarted, .LineType = o.LineType, .TemplateId = o.TemplateId, .Technical_PassFail = Nothing, .InspectionFinished = Nothing, .WOQuantity = o.WOQuantity, .AQL_Level = o.AQL_Level, .ItemFailCount = o.ItemFailCount, .RejectLimiter = o.RejectLimiter, .SampleSize = o.SampleSize, .Standard = o.Standard}).ToList()
    End Sub
    Private Function ProcessJobsInQue(ByVal AQLVAL As String) As String
        Dim returnmsg As String = "NOJOBS"
        Try
            Dim Aql As Decimal = 0

            If Decimal.TryParse(AQLVAL, Aql) = True Then
                Dim matchcnt = (From x In que Where x.AQL_Level = Aql Select x).Count()

                If matchcnt > 0 Then
                    que = (From x In que Where x.AQL_Level = Aql Select x).Take(1).ToList()
                    Return jser.Serialize(que)
                End If
            End If

            'If LineType = "IL" Then
            '    ReopenInlineJob()
            'End If
            que = (From o In que Order By o.InspectionStarted Descending Select o).Take(2).ToList()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
        End Try
        If que.Count > 0 Then
            returnmsg = jser.Serialize(que)
        End If
        Return returnmsg
    End Function

End Class

Public Class dispatchJob
    Public Property id As Integer
    Public Property AQL_Level As String
    Public Property RejectLimiter As Integer
    Public Property WOQuantity As Integer
    Public Property SampleSize As Integer
    Public Property ItemFailCount As Integer
    Public Property Standard As String
    Public Property JobNumber As String
    Public Property LineType As String
    Public Property TemplateId As Integer
    Public Property InspectionStarted As DateTime
    Public Property InspectionFinished As DateTime?
    Public Property Technical_PassFail As Boolean?
End Class