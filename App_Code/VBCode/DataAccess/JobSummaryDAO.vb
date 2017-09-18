
Namespace core
    Public Class JobSummaryDAO
        Private Shared mapper As New BMappers(Of SPCInspection.InspectionJobSummary)

        Public Shared Function UpdateFailedCount(ByVal JobId As Integer, ByVal ItemCount As Integer) As Boolean

            Dim sqlis As String = "UPDATE InspectionJobSummary SET ItemFailCount = @ItemFailCount WHERE (id = " & JobId.ToString() & ")"
            Dim objis As New SPCInspection.InspectionJobSummary
            objis.ItemFailCount = ItemCount

            Return mapper.InsertSpcObject(sqlis, objis)
            Return False
        End Function

        Public Shared Sub UpdateFailedCountAsync(ByVal JobId As Integer, ByVal ItemCount As Integer)
            Try
                Dim t As System.Threading.Tasks.Task = System.Threading.Tasks.Task.Run(Sub()
                                                                                           UpdateFailedCount(JobId, ItemCount)
                                                                                       End Sub)
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End Sub

    End Class
End Namespace


