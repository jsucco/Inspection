Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data

Namespace core
    Public Class AppLog

        Public Shared Machine As String = "WEBSVR01"

        Public Function ConnectionString() As String

            Dim constrg As String = ConfigurationManager.ConnectionStrings("applogconnectionstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = Machine

            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = 5
            End If

            Return sb.ToString

        End Function

        ''returns an opened connection

        Public Function GetSqlConnection() As SqlConnection

            Dim conn As New SqlConnection(ConnectionString)
            conn.Open()
            Return conn

        End Function

        '' Summary
        '' Overrides the connection timeout


        Public Property ConnectionTimeout As Int32 = 4

        '' summary
        '' Property Used to override the name of the application

        Public Property ApplicationName As String = "APR_WEBAPPLICATION"

        Public Sub AddToAppLog(ByVal type As String, ByVal target As String, ByVal message As String, Optional ByVal UserPK As Integer = 0)

            Using conn As New SqlConnection(ConnectionString)
                conn.Open()
                Dim cmd As New SqlCommand()
                'Using cmd As New SqlCommand
                cmd.Connection = conn
                cmd.CommandText = "SP_ApplicationLogInsert_1"
                cmd.CommandType = System.Data.CommandType.StoredProcedure

                cmd.Parameters.Add("@message", SqlDbType.VarChar).Direction = ParameterDirection.Input
                cmd.Parameters.Add("@type", SqlDbType.VarChar).Direction = ParameterDirection.Input
                cmd.Parameters.Add("@target", SqlDbType.VarChar).Direction = ParameterDirection.Input
                cmd.Parameters.Add("@UserPK", SqlDbType.Int).Direction = ParameterDirection.Input
                cmd.Parameters.Add("@numRowAff", SqlDbType.Int).Direction = ParameterDirection.Output

                cmd.Parameters("@message").Value = message
                cmd.Parameters("@type").Value = type
                cmd.Parameters("@target").Value = target
                cmd.Parameters("@UserPK").Value = UserPK

                Dim res As Int16

                cmd.ExecuteNonQuery()

                res = cmd.Parameters("@numRowAff").Value

                'End Using
            End Using

        End Sub



    End Class

End Namespace
