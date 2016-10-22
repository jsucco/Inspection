Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Data.Odbc
'Imports IBM.Data.DB2
'Imports IBM.Data.DB2.iSeries


Namespace core

    Public Class AS400DAO

        'Private conn As New iDB2Connection("Datasource=Prod;userid=SUCCO_T;password=SUCCO_T;")
        Private Property dl As New dlayer


        'Private Function iDB2Reader(ByVal cmd As iDB2Command) As List(Of Object)
        '    Dim record As IDataRecord
        '    Dim readerlist As New List(Of Object)
        '    Dim dr As iDB2DataReader

        '    Try
        '        conn.Open()
        '        dr = cmd.ExecuteReader()
        '    Catch ex As Exception
        '        Return readerlist
        '    End Try


        '    While dr.Read
        '        record = CType(dr, IDataRecord)
        '        Dim length As Integer = record.FieldCount
        '        For i = 0 To (length - 1)
        '            Dim testitem As Object = record.Item(i)
        '            readerlist.Add(testitem)
        '        Next

        '    End While

        '    dr.Close()
        '    cmd.Dispose()
        '    conn.Close()

        '    Return readerlist

        'End Function

        Public Sub UpdateCostDesc(ByVal ijsid As Integer, ByVal DataNo As String, ByVal CID As String)

            Using connection As New SqlConnection(dl.InspectConnectionString)
                connection.Open()
                Dim sqlcommand As SqlCommand = connection.CreateCommand()

                sqlcommand = New SqlCommand("SP_GetDataNoStart", connection)
                sqlcommand.CommandType = CommandType.StoredProcedure

                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add("@DataNo", SqlDbType.VarChar, 20).Direction = ParameterDirection.Input
                sqlcommand.Parameters.Add("@cid", SqlDbType.Bit).Direction = ParameterDirection.Input
                sqlcommand.Parameters.Add("@ijsid", SqlDbType.Int).Direction = ParameterDirection.Input

                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@DataNo").Value = DataNo
                sqlcommand.Parameters("@cid").Value = CID
                sqlcommand.Parameters("@ijsid").Value = ijsid
                sqlcommand.ExecuteNonQuery()
                connection.Close()
            End Using

        End Sub


        Private Function SQLReader(ByVal cmd As SqlCommand) As List(Of Object)
            Dim record As IDataRecord
            Dim readerlist As New List(Of Object)
            Dim dr As SqlDataReader
            Dim con As New SqlConnection(dl.CtxConnectionString)
            Try
                'con.Open()
                dr = cmd.ExecuteReader()
            Catch ex As Exception
                Return readerlist
            End Try


            While dr.Read
                record = CType(dr, IDataRecord)
                Dim length As Integer = record.FieldCount
                For i = 0 To (length - 1)
                    Dim testitem As Object = record.Item(i)
                    readerlist.Add(testitem)
                Next

            End While

            dr.Close()
            cmd.Dispose()
            con.Close()

            Return readerlist

        End Function

        Public Function GetGriegeNo(ByVal RollNo As String, ByVal Auto As Boolean) As List(Of SPCInspection.Roll_Ledge)

            Dim returnstring As String = ""
            Dim rolllst As New List(Of SPCInspection.Roll_Ledge)
            Using connection As New SqlConnection(dl.InspectConnectionString)
                connection.Open()
                Dim sqlcommand As SqlCommand = connection.CreateCommand()

                sqlcommand = New SqlCommand("SP_AS400_GetGreigeNumber", connection)
                sqlcommand.CommandType = CommandType.StoredProcedure

                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add("@ROLL", SqlDbType.VarChar, 20).Direction = ParameterDirection.Input
                sqlcommand.Parameters.Add("@AUTOMATED", SqlDbType.Bit).Direction = ParameterDirection.Input
                sqlcommand.Parameters.Add("@GRIEGENUMBER", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                sqlcommand.Parameters.Add("@TICKETYARDS", SqlDbType.Decimal).Direction = ParameterDirection.Output
                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@ROLL").Value = RollNo
                sqlcommand.Parameters("@AUTOMATED").Value = Auto
                sqlcommand.ExecuteNonQuery()

                Dim GREIGETEMP As Object = sqlcommand.Parameters("@GRIEGENUMBER").Value.ToString()
                Dim TICKETTEMP As Object = sqlcommand.Parameters("@TICKETYARDS").Value
                If IsNumeric(TICKETTEMP) = True Then
                    rolllst.Add(New SPCInspection.Roll_Ledge With {.GREIGENUMBER = GREIGETEMP, .TICKETYARDS = Convert.ToDecimal(TICKETTEMP)})
                End If
                connection.Close()
            End Using
            Return rolllst


        End Function

        Public Function GetRollDataNumber(ByVal RollNumber As String) As String
            Dim con As New SqlConnection(dl.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim DataNo As String = ""
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetRollDataNumber", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@RollNo", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@OUT", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                        cmd.Parameters("@RollNo").Value = RollNumber
                        cmd.CommandTimeout = 5000

                        cmd.ExecuteNonQuery()

                        DataNo = cmd.Parameters("@OUT").Value.ToString()

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return ""
            End Try

            Return DataNo
        End Function

        Public Function GetInspectionWorkOrder(ByVal workorder As String) As List(Of SPCInspection.InspectionWorkOrder)
            Dim con As New SqlConnection(dl.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim bmap As New BMappers(Of SPCInspection.InspectionWorkOrder)
            Dim returnlist As New List(Of SPCInspection.InspectionWorkOrder)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetWorkOrderData_1", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@WONUMBER", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@WONUMBER").Value = workorder
                        cmd.CommandTimeout = 5000

                        returnlist = bmap.GetSpcSP(cmd)

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return returnlist
        End Function

        Public Function GetInspectionWorkOrder_1(ByVal workorder As String) As List(Of SPCInspection.as400WorkOrder)
            Dim con As New SqlConnection(dl.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim bmap As New BMappers(Of SPCInspection.as400WorkOrder)
            Dim returnlist As New List(Of SPCInspection.as400WorkOrder)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SPC_GetWorkOrder_SP ", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@WO", SqlDbType.VarChar, 20).Direction = ParameterDirection.Input
                        cmd.Parameters("@WO").Value = workorder
                        cmd.CommandTimeout = 5000

                        returnlist = bmap.GetSpcSP(cmd)

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return returnlist
        End Function

        Public Function GetCasePackConv(ByVal datanumber As String) As Decimal
            Dim con As New SqlConnection(dl.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim returnint As Decimal = 0
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetCasePackConv", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@DATANUMBER", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CONV", SqlDbType.Decimal).Direction = ParameterDirection.Output
                        cmd.Parameters("@DATANUMBER").Value = datanumber
                        cmd.CommandTimeout = 5000

                        cmd.ExecuteNonQuery()

                        returnint = cmd.Parameters("@CONV").Value.ToString()

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return returnint
        End Function

        Private Function GetPO(ByVal workorder As String) As Array
            Dim con As New SqlConnection("Data Source=stcaprtrn1;Initial Catalog=SpcSTT;Persist Security Info=True;User ID=bnr;Password=cerf28?")
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetPurchaseOrderNumber", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@WORKORDER", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters("@WORKORDER").Value = workorder
                        cmd.CommandTimeout = 5000

                        Dim returnlist As New List(Of SPCInspection.DefectMaster)
                        Dim socheckarray As Array = SQLReader(cmd).ToArray()

                        Return socheckarray

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try



        End Function

        Public Function GetInputCartonData(ByVal CartonNumber As String) As List(Of SPCInspection.InspectionCarton)
            Dim con As New SqlConnection(dl.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim bmap As New BMappers(Of SPCInspection.InspectionCarton)
            Dim cartonlist As List(Of SPCInspection.InspectionCarton)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetInspectionCarton", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@CARTONNUMBER", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@CARTONNUMBER").Value = CartonNumber
                        cmd.CommandTimeout = 5000

                        cartonlist = bmap.GetSpcSP(cmd)

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return cartonlist
        End Function

        Public Function Getas400UnitDesc(ByVal DataNo As String) As String
            Dim con As New SqlConnection(dl.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()

            Dim retobj As String = ""
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetUnitDesc", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@DATANUMBER", SqlDbType.VarChar, 20).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DESCRIPTION", SqlDbType.NVarChar, 300).Direction = ParameterDirection.Output
                        cmd.Parameters("@DATANUMBER").Value = DataNo

                        cmd.CommandTimeout = 4000
                        cmd.ExecuteReader()

                        retobj = cmd.Parameters("@DESCRIPTION").Value

                    End Using
                End Using
            Catch ex As Exception

            End Try

            Return retobj

        End Function
    End Class

End Namespace

