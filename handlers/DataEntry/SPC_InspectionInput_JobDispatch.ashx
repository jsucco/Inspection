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
    Public Function OpenJobIfExists(TargetNumber As String, TemplateId As Integer) As String
        Dim returnMessage As String = "NOJOBS"
        If IsNothing(TargetNumber) = False And TargetNumber.Length > 0 Then
            JobNumber = TargetNumber
            loadQueWithJobs(TemplateId)
            If que.Count > 0 Then
                returnMessage = ProcessJobsInQue()
            End If
        End If

        Return returnMessage
    End Function

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
    Private Function ProcessJobsInQue() As String
        Dim returnmsg As String = "NOJOBS"
        Try
            ReopenInlineJob()
            que = (From o In que Where IsNothing(o.InspectionFinished) = True And IsNothing(o.Technical_PassFail) = True Order By o.InspectionStarted Descending Select o).Take(1).ToList()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Return "NOJOBS"
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