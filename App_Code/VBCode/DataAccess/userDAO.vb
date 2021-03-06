﻿Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports core.Environment
Imports System.Data
Imports System.Globalization


Namespace core


    Public Class UserProfile


        Public Username As String
        Public CID As Integer
        Public MFBID As Integer

    End Class



    Public Class userDAO

        Public Property dl As New dlayer
        Public Property _DAOFactory As New DAOFactory

        Dim Securitystring As String
        Dim SecurityReader As SqlDataReader
        Dim SecuritycmdBuilder As SqlCommandBuilder
        Dim Securitycmd As SqlCommand
        Dim SecurityConnection As SqlConnection
        Dim APR As New APRWebApp
        Public Property UserId As String

        Public Function AutenticateUser(ByVal user As String, password As String, ByVal PassCID As Integer) As Boolean

            Securitystring = "Select * from dbo.Security where UserID = '" + user + "' and Password = '" + password + "'"
            SecurityConnection = New SqlConnection(dl.SetCtxConnectionString(PassCID))
            SecurityConnection.Open()
            Securitycmd = New SqlCommand(Securitystring, SecurityConnection)
            SecurityReader = Securitycmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim returnvalue As Boolean = SecurityReader.HasRows

            If SecurityConnection.State = ConnectionState.Open Then
                SecurityConnection.Close()
            End If

            Return returnvalue

        End Function

        Public Function LogUserActivity(ByVal Username As String, ByVal DeviceType As String, Optional ByVal CID As String = "000000") As Integer

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            'dl.GetCtxConnectionString()
            'dbOrigin = dl.Ctxconnection

            SQL = "INSERT INTO dbo.UserActivityLog (DBOrigin,UserID, EntryTimestamp, DeviceType, IPAddress, CID) VALUES(@DBOrigin, @UserID, @EntryTimestamp, @DeviceType, @IPAddress, @CID) SELECT @@IDENTITY;"


            Using connection As New SqlConnection(dl.APRConnectionString(2))


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@DBOrigin", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@UserID", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@EntryTimestamp", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@DeviceType", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@IPAddress", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@CID", DbType.String))
                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@DBOrigin").Value = "APR"
                sqlcommand.Parameters("@UserID").Value = Username
                sqlcommand.Parameters("@EntryTimestamp").Value = DateTime.Now
                sqlcommand.Parameters("@DeviceType").Value = DeviceType
                sqlcommand.Parameters("@IPAddress").Value = APRWebApp.GetIPAddress()
                sqlcommand.Parameters("@CID").Value = CID
                Try

                    sqlcommand.Connection.Open()
                    returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())

                Catch e As Exception


                End Try

            End Using

            Return returnint
        End Function

    End Class

End Namespace
