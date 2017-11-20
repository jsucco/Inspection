Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic


Namespace core
    Public Class AprManagerDAO

        Public Property dlayer As New dlayer


        Public Function GetInsAlerts() As SPCInspection.AlertEmails()
            Dim rs() As SPCInspection.AlertEmails = {}

            Dim con As New SqlConnection(dlayer.APRConnectionString(2))
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim listso As New List(Of SPCInspection.AlertEmails)
            Dim bmap As New BMappers(Of SPCInspection.AlertEmails)
            Dim retobj As Double = 0
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetAlertEmails", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@code", SqlDbType.VarChar, 15).Direction = ParameterDirection.Input
                        cmd.Parameters("@code").Value = "INS"

                        cmd.CommandTimeout = 10000

                        listso = bmap.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception

            End Try

            Return listso.ToArray()

        End Function
    End Class
End Namespace

