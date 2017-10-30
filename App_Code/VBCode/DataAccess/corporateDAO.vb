Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports core.Environment
Imports System.Data
Imports System.Globalization

Namespace core

    Public Class CID

        Public CID As Integer
        Public CID_Print As String
        Public UserID As String
        Public CorporateName As String
        Public DBname As String
        Public subscriptdate As DateTime
        Public LoginTime As DateTime
        Public IPAddress As String

    End Class

    Public Class corporate

        Public Property dl As New dlayer
        Public Property Util As New Utilities
        Private Property DAOFactory As New DAOFactory
        Public cidclass As New List(Of CID)

        Dim ctxmanstring As String
        Dim ctxmanReader As SqlDataReader
        Dim ctxmancmdBuilder As SqlCommandBuilder
        Dim ctxmancmd As SqlCommand
        Dim ctxmanConnection As SqlConnection

        Public Function IsAprContext(ByVal CID As String) As Boolean
            Dim IPaddress As String
            Dim query As String = "select CID, Name, DBName  from dbo.LocationMaster where CID = '" + CID + "'"
            Using conn As New SqlConnection(dl.AprManagerConnectionString)
                conn.Open()
                Dim cmd As New SqlCommand(query, conn)
                ctxmanReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                Dim irec As IDataReader
                Try
                    IPaddress = APRWebApp.GetIPAddress()
                Catch ex As Exception
                    IPaddress = "000.000.00"
                    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                End Try
                Try
                    While ctxmanReader.Read
                        irec = CType(ctxmanReader, IDataRecord)
                        Dim DBname As String = ""
                        If IsNothing(irec(2)) = False Then
                            DBname = irec(2).ToString.Trim()
                        End If
                        Dim CorpName As String = ""
                        If IsNothing(irec(1)) = False Then
                            CorpName = irec(1).ToString.Trim()
                        End If
                        cidclass.Add(New CID With {.CID = Convert.ToInt32(irec(0)), .CID_Print = CID, .CorporateName = CorpName, .DBname = DBname, .subscriptdate = Date.Now, .LoginTime = DateTime.Now, .IPAddress = IPaddress})

                    End While
                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                End Try

            End Using
            If cidclass.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function GetDBuser(ByVal CID As String) As Boolean
            Dim IPaddress As String
            ctxmanstring = "select CID, CorporateName, DBName, SubscriptionExpiration  from dbo.Corporate where CID = '" + CID + "'"
            ctxmanConnection = New SqlConnection(dl.ctxmangConnectionString)
            ctxmanConnection.Open()
            ctxmancmd = New SqlCommand(ctxmanstring, ctxmanConnection)
            ctxmanReader = ctxmancmd.ExecuteReader(CommandBehavior.CloseConnection)
            Dim record As IDataReader
            Try

                While ctxmanReader.Read

                    record = CType(ctxmanReader, IDataRecord)
                    Try
                        IPaddress = APRWebApp.GetIPAddress()
                    Catch ex As Exception
                        IPaddress = "000.000.00"
                    End Try


                    cidclass.Add(New CID With {.CID = Convert.ToInt32(record(0)), .CID_Print = CID, .CorporateName = record(1), .DBname = record(2), .subscriptdate = Date.Parse(record(3), CultureInfo.InvariantCulture), .LoginTime = DateTime.Now, .IPAddress = IPaddress})

                End While
                If cidclass.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try

            If ctxmanConnection.State = ConnectionState.Open Then
                ctxmanConnection.Close()
            End If

        End Function

        Public Function GetCorporateName() As String
            Dim dlayer As New dlayer
            Dim CorporateNameSet As DataSet = New DataSet
            Dim CID As String = "000" + Convert.ToString(dlayer.GetSessionCID)
            Dim CorporateNamestring As String = ""
            Dim sqlstring As String = "select CorporateName from dbo.Corporate where CID = '" & CID & "'"
            Using connection As New SqlConnection(dl.ctxmangConnectionString)

                Dim _Command = DAOFactory.GetCommand(sqlstring, connection)

                Dim da As IDbDataAdapter = _DAOFactory.GetAdapter(_Command)
                Try

                    DAOFactory.AdapterFill(da, CorporateNameSet, "CorporateNameSet")
                    If CorporateNameSet.Tables.Count > 0 Then
                        If CorporateNameSet.Tables(0).Rows.Count > 0 Then
                            CorporateNamestring = CorporateNameSet.Tables(0).Rows(0)("CorporateName")
                        End If
                    End If

                Catch e As Exception
                    Return False

                End Try

            End Using

            Return CorporateNamestring

        End Function

        Public Function GetFullCorporateList() As List(Of selector2array)
            Dim dlayer As New dlayer
            Dim CorporateNameList As New List(Of selector2array)
            Dim CorporateNamestring As String = ""
            Dim sqlstring As String = "SELECT CID, CorporateName" & vbCrLf &
                        "FROM corporate" & vbCrLf &
                        "WHERE (APRPM_Enabled = 1) OR" & vbCrLf &
                         "(APRUtility_Enabled = 1) OR" & vbCrLf &
                         "(APRLoom_Enabled = 1) OR" & vbCrLf &
                         "(APRInspection_Enabled = 1)"
            CorporateNameList = DAOFactory.getSelector2_Ctxmang(sqlstring)

            Return CorporateNameList

        End Function
        
    End Class


End Namespace