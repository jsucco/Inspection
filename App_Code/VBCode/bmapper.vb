Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Data

Namespace core

    Public Class BMapper(Of t As {Class, New})
        Sub New()
            Dim obj As New t()
            Properties = obj.GetType().GetProperties()
        End Sub
        Private Shared dl As New dlayer
        Private Shared dreader As SqlDataReader
        Private Shared Properties As PropertyInfo()
        Private Shared _DAOFactory As New DAOFactory
        Public Shared ErrorMessage As String

        Public Shared Function GetCtxObject(ByVal Sql As String) As List(Of t)
            Dim returnlist As New List(Of t)
            Try
                Using con As New SqlConnection(dl.CtxConnectionString)
                    con.Open()
                    Dim cmd As New SqlCommand(Sql, con)
                    dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If dreader.FieldCount > 0 Then
                        returnlist = Start()
                    End If
                End Using

            Catch ex As Exception
                ErrorMessage = ex.Message
            End Try

            Return returnlist

        End Function


        Public Shared Function GetInspectObject(ByVal Sql As String) As List(Of t)
            Dim returnlist As New List(Of t)
            Try
                Using con As New SqlConnection(dl.InspectConnectionString())
                    con.Open()
                    Dim cmd As New SqlCommand(Sql, con)
                    dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If dreader.FieldCount > 0 Then
                        returnlist = Start()
                    End If
                End Using

            Catch ex As Exception
                ErrorMessage = ex.Message
            End Try

            Return returnlist

        End Function

        Public Shared Function GetSpcObject(ByVal Sql As String) As List(Of t)
            Dim returnlist As New List(Of t)
            Try
                Using con As New SqlConnection(dl.SPCConnectionString)
                    con.Open()
                    Dim cmd As New SqlCommand(Sql, con)
                    dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If dreader.FieldCount > 0 Then
                        returnlist = Start()
                    End If
                End Using

            Catch ex As Exception
                ErrorMessage = ex.Message
            End Try

            Return returnlist

        End Function

        Private Shared Function Start() As List(Of t)
            Dim hashtable As New Hashtable()
            Dim Obj As New t()
            Dim bustype As Type = Obj.GetType()
            Dim ReturnList As New List(Of t)
            Properties = bustype.GetProperties()
            Dim record As IDataRecord
            For Each info As PropertyInfo In Properties
                hashtable(info.Name.ToUpper()) = info
            Next

            While dreader.Read
                Dim newObj As New t()
                record = CType(dreader, IDataRecord)
                For i = 0 To record.FieldCount - 1
                    Dim info As PropertyInfo = hashtable(dreader.GetName(i).ToUpper())
                    If IsNothing(info) = False Then
                        If info.CanWrite And IsDBNull(record(i)) = False Then
                            Dim testobj As Object = record(i)
                            info.SetValue(newObj, record(i), Nothing)
                        End If
                    End If
                Next
                ReturnList.Add(ProcessRecordSet(newObj))
            End While

            Return ReturnList
        End Function
        Public Shared Function InsertSpcObject(ByVal sql As String, ByVal obj As t) As Boolean

            Properties = obj.GetType().GetProperties()
            Dim returnint As Integer = 0
            Dim cnt As Integer = 0
            Dim ParaStringArray As String() = GetQueryStringParam(sql)
            Dim newobj As Object = ProcessRecordSet(obj)

            If ParaStringArray.Length > 0 Then
                Using conn As New SqlConnection(dl.SPCConnectionString())
                    Dim cmd As New SqlCommand(sql, conn)

                    For Each item In ParaStringArray
                        Dim fieldprop As PropertyInfo = obj.GetType.GetProperty(item)
                        cmd.Parameters.Add(_DAOFactory.GetRefparameter("@" + item, fieldprop.PropertyType().FullName()))
                        cmd.Parameters("@" + item).Value = fieldprop.GetValue(newobj, Nothing)
                    Next

                    Try
                        cmd.Connection.Open()
                        returnint = Convert.ToInt32(cmd.ExecuteNonQuery())

                    Catch ex As Exception

                    End Try
                End Using
            End If

            If returnint = 1 Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Shared Function GetQueryStringParam(ByVal Sql As String) As String()
            Dim returnParam As New List(Of String)

            If Sql <> "" Then
                Dim stringsep() As String = Sql.Split("@")
                For i = 0 To stringsep.Length - 1
                    If i > 0 Then
                        If Not String.IsNullOrWhiteSpace(stringsep(i).Split(",")(0)) And stringsep(i).Split(",").Length > 1 Then
                            returnParam.Add(stringsep(i).Split(",")(0))
                        ElseIf Not String.IsNullOrWhiteSpace(stringsep(i).Split(" ")(0)) And stringsep(i).Split(" ").Length > 1 Then
                            returnParam.Add(stringsep(i).Split(" ")(0))
                        ElseIf Not String.IsNullOrWhiteSpace(stringsep(i).Split(")")(0)) And stringsep(i).Split(")").Length > 1 Then
                            returnParam.Add(stringsep(i).Split(")")(0))
                        End If
                    End If
                Next
                Return returnParam.ToArray()
            Else
                Return Nothing
            End If

        End Function

        Private Shared Function ProcessRecordSet(ByRef obj As Object) As Object
            Dim returndict As New Dictionary(Of String, Object)

            If IsNothing(obj) = False Then

                For Each mfield As PropertyInfo In Properties
                    If IsNothing(mfield.GetValue(obj, Nothing)) = True Then
                        Dim fdatatype = mfield.PropertyType.ToString()
                        Select Case fdatatype
                            Case "System.String"
                                mfield.SetValue(obj, "", Nothing)
                            Case "System.Int32", "System.Int64"
                                mfield.SetValue(obj, 0, Nothing)
                            Case "System.Boolean"
                                mfield.SetValue(obj, True, Nothing)
                            Case "System.Object"
                                mfield.SetValue(obj, "", Nothing)
                            Case Else
                                mfield.SetValue(obj, Nothing, Nothing)
                        End Select
                    End If
                Next
            End If

            Return obj
        End Function

    End Class

End Namespace