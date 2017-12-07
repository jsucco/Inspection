Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Data
Imports System.Data.Entity
Imports OfficeOpenXml
Imports System.Drawing

Namespace core

    Public Class BMappers(Of t As {Class, New})
        Sub New()
            Dim obj As New t()
            Properties = obj.GetType().GetProperties()

        End Sub
        Private Property dl As New dlayer
        Private Property dreader As SqlDataReader
        Private Property Properties As PropertyInfo()
        Private Property _DAOFactory As New DAOFactory
        Public Property ErrorMessage As String
        Public RowReturnIdentity As Integer = 0

        Public Function GetCtxMangObject(ByVal Sql As String) As List(Of t)
            Dim returnlist As New List(Of t)
            Try
                Using con As New SqlConnection(dl.ctxmangConnectionString)
                    con.Open()
                    Dim cmd As New SqlCommand(Sql, con)
                    dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If dreader.FieldCount > 0 Then
                        returnlist = Start()
                    End If
                End Using

            Catch ex As Exception
                ErrorMessage = ex.Message
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return returnlist

        End Function
        Public Function GetAprMangObject(ByVal Sql As String) As List(Of t)
            Dim returnlist As New List(Of t)
            Try
                Using con As New SqlConnection(dl.APRConnectionString(2))
                    con.Open()
                    Dim cmd As New SqlCommand(Sql, con)
                    dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If dreader.FieldCount > 0 Then
                        returnlist = Start()
                    End If
                End Using

            Catch ex As Exception
                ErrorMessage = ex.Message
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return returnlist

        End Function

        Public Function GetInspectObject(ByVal Sql As String) As List(Of t)
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
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return returnlist

        End Function

        Public Function GetSpcObject(ByVal Sql As String) As List(Of t)
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
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return returnlist

        End Function
        Public Function GetPDMObject(ByVal Sql As String) As List(Of t)
            Dim returnlist As New List(Of t)
            Try
                Using con As New SqlConnection(dl.PDMConnectionString())
                    con.Open()
                    Dim cmd As New SqlCommand(Sql, con)
                    dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If dreader.FieldCount > 0 Then
                        returnlist = Start()
                    End If
                End Using

            Catch ex As Exception
                ErrorMessage = ex.Message
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return returnlist

        End Function
        Public Function GetSpcSP(ByVal cmd As SqlCommand) As List(Of t)
            Dim returnlist As New List(Of t)
            Try
                dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                If Not dreader Is Nothing Then
                    If dreader.FieldCount > 0 Then
                        returnlist = Start()
                    End If
                Else
                    Throw New Exception("data reader came back as nothing")
                End If
                
            Catch ex As Exception
                ErrorMessage = ex.Message
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return returnlist

        End Function
        Public Function GetReqParamAsObject(ByVal RequestParams As NameValueCollection) As t
            Dim hashtable As New Hashtable()
            Dim Obj As New t()
            Dim bustype As Type = Obj.GetType()
            Dim Util As New Utilities

            Properties = bustype.GetProperties()

            If Not Properties Is Nothing Then
                For Each info As PropertyInfo In Properties
                    hashtable(info.Name.ToUpper()) = info
                Next

                Dim keys As ICollection = hashtable.Keys
                If hashtable.Count > 0 Then
                    For Each key In keys
                        Try
                            Dim info As PropertyInfo = hashtable(key)
                            If IsNothing(info) = False Then
                                If info.CanWrite Then
                                    If IsNothing(RequestParams.GetValues(key)) = False And IsDBNull(RequestParams.GetValues(key)) = False Then
                                        Dim rpvalue As Object = Util.ConvertType(RequestParams.GetValues(key)(0), info.PropertyType.Name)
                                        info.SetValue(Obj, rpvalue, Nothing)
                                    End If

                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    Next
                End If
            End If
            Return Obj
        End Function
        Private Function Start() As List(Of t)
            Dim hashtable As New Hashtable()
            Dim Obj As New t()
            Dim bustype As Type = Obj.GetType()
            Dim ReturnList As New List(Of t)
            Properties = bustype.GetProperties()
            Dim record As IDataRecord
            For Each info As PropertyInfo In Properties
                hashtable(info.Name.ToUpper()) = info
            Next
            Dim testobjasdf As Object
            While dreader.Read
                Dim newObj As New t()
                record = CType(dreader, IDataRecord)
                For i = 0 To record.FieldCount - 1
                    testobjasdf = dreader.GetName(i).ToUpper()
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
        Public Function InsertSpcObject(ByVal sql As String, Optional ByVal obj As t = Nothing, Optional ByVal RetId As Boolean = False) As Boolean
            Dim returnint As Integer = 0
            Dim cnt As Integer = 0
            'If ParaStringArray.Length > 0 Then
            Using conn As New SqlConnection(dl.InspectConnectionString())
                If RetId = True Then
                    sql = sql + " SELECT @@IDENTITY;"
                End If
                Dim cmd As New SqlCommand(sql, conn)
                If IsNothing(obj) = False Then
                    Properties = obj.GetType().GetProperties()
                    Dim ParaStringArray As String() = GetQueryStringParam(sql)
                    Dim newobj As Object = ProcessRecordSet(obj)

                    For Each item In ParaStringArray
                        Dim fieldprop As PropertyInfo = obj.GetType.GetProperty(item)
                        If Not fieldprop Is Nothing Then
                            cmd.Parameters.Add(_DAOFactory.GetRefparameter("@" + item, fieldprop.PropertyType().FullName()))
                            cmd.Parameters("@" + item).Value = fieldprop.GetValue(newobj, Nothing)
                        End If
                    Next
                End If
                Try
                    cmd.Connection.Open()
                    If RetId = True Then
                        returnint = Convert.ToInt32(cmd.ExecuteScalar())
                    Else
                        returnint = Convert.ToInt32(cmd.ExecuteNonQuery())
                    End If


                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                End Try
            End Using
            'End If

            If returnint = 1 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function InsertSpcObject_RetNum(ByVal sql As String, Optional ByVal obj As t = Nothing, Optional ByVal RetId As Boolean = False) As Boolean
            Dim returnint As Boolean = False
            Dim cnt As Integer = 0
            'If ParaStringArray.Length > 0 Then
            Using conn As New SqlConnection(dl.InspectConnectionString())
                If RetId = True And sql.IndexOf("INSERT ") >= 0 Then
                    sql = sql + " SELECT @@IDENTITY;"
                End If
                Dim cmd As New SqlCommand(sql, conn)
                If IsNothing(obj) = False Then
                    Properties = obj.GetType().GetProperties()
                    Dim ParaStringArray As String() = GetQueryStringParam(sql)
                    Dim newobj As Object = ProcessRecordSet(obj)

                    For Each item In ParaStringArray
                        Dim fieldprop As PropertyInfo = obj.GetType.GetProperty(item)
                        cmd.Parameters.Add(_DAOFactory.GetRefparameter("@" + item, fieldprop.PropertyType().FullName()))
                        cmd.Parameters("@" + item).Value = fieldprop.GetValue(newobj, Nothing)
                    Next
                End If
                Try
                    cmd.Connection.Open()
                    If RetId = True Then
                        RowReturnIdentity = Convert.ToInt32(cmd.ExecuteScalar())
                        If RowReturnIdentity > 0 Then
                            returnint = True
                        End If
                    Else
                        If Convert.ToInt32(cmd.ExecuteNonQuery()) > 0 Then
                            returnint = True
                        End If

                    End If


                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                    ErrorMessage = ex.Message
                End Try
            End Using

            Return returnint

        End Function

        Public Function InsertAprManagerObject(ByVal sql As String, ByVal CID As Integer, Optional ByVal obj As t = Nothing) As Boolean
            Dim returnint As Integer = 0
            Dim cnt As Integer = 0
            'If ParaStringArray.Length > 0 Then
            Using conn As New SqlConnection(dl.AprManagerConnectionString())
                Dim cmd As New SqlCommand(sql, conn)
                If IsNothing(obj) = False Then
                    Properties = obj.GetType().GetProperties()
                    Dim ParaStringArray As String() = GetQueryStringParam(sql)
                    Dim newobj As Object = ProcessRecordSet(obj)

                    For Each item In ParaStringArray
                        Dim fieldprop As PropertyInfo = obj.GetType.GetProperty(item)
                        cmd.Parameters.Add(_DAOFactory.GetRefparameter("@" + item, fieldprop.PropertyType().FullName()))
                        cmd.Parameters("@" + item).Value = fieldprop.GetValue(newobj, Nothing)
                    Next
                End If
                Try
                    cmd.Connection.Open()
                    returnint = Convert.ToInt32(cmd.ExecuteNonQuery())

                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                End Try
            End Using
            'End If

            If returnint = 1 Then
                Return True
            Else
                Return False
            End If


        End Function

        Private Function GetQueryStringParam(ByVal Sql As String) As String()
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

        Private Function ProcessRecordSet(ByRef obj As Object) As Object
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
        Public Function InjectSubstats(ByRef epack As ExcelPackage, ByVal cslist As List(Of t), ByVal todate As DateTime, ByVal TabName As String) As ExcelWorkbook
            Dim sheet1 As ExcelWorksheet
            Dim aphaarray As Char() = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()

            Dim firstchar = aphaarray(4)

            sheet1 = epack.Workbook.Worksheets(TabName)

            If cslist.Count > 0 Then
                Dim newobj As New t
                Dim objtype As Type = newobj.GetType()
                Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                Dim colint As Integer = 1
                Dim fieldproplist As Object
                sheet1.Cells(lastrow, 1).Value = todate
                sheet1.Cells(lastrow, 1).Style.Font.Bold = True
                sheet1.Cells(lastrow, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                sheet1.Cells(lastrow, 1).Style.Fill.BackgroundColor.SetColor(Color.Yellow)
                sheet1.Cells(lastrow, 1).Style.Numberformat.Format = "yyyy-mm-dd"

                Dim staticfields = objtype.GetFields(BindingFlags.Static Or BindingFlags.Public)
                For Each field As FieldInfo In staticfields
                    Dim fieldname = field.Name
                    If fieldname = "FuncPropList" Then
                        fieldproplist = field.GetValue(Nothing)
                    End If
                Next

                For Each info As PropertyInfo In objprop
                    Dim propname As String = info.PropertyType().Name
                    If IsNothing(fieldproplist) = False Then
                        If fieldproplist.ContainsKey(info.Name) Then
                            If fieldproplist(info.Name) = "AVG" Then
                                sheet1.Cells(lastrow, colint).Formula = "AVERAGE(" + aphaarray(colint - 1) + "2:" + aphaarray(colint - 1) + Convert.ToString(lastrow - 1) + ")"
                            ElseIf fieldproplist(info.Name) = "SUM" Then
                                sheet1.Cells(lastrow, colint).Formula = "SUM(" + aphaarray(colint - 1) + "2:" + aphaarray(colint - 1) + Convert.ToString(lastrow - 1) + ")"
                            End If
                            sheet1.Cells(lastrow, colint).Style.Numberformat.Format = "#.##"
                            sheet1.Cells(lastrow, colint).Style.Font.Bold = True
                            sheet1.Cells(lastrow, colint).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                            sheet1.Cells(lastrow, colint).Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
                        Else
                            sheet1.Cells(lastrow, colint).Style.Font.Bold = True
                            sheet1.Cells(lastrow, colint).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                            sheet1.Cells(lastrow, colint).Style.Fill.BackgroundColor.SetColor(Color.LightGreen)
                        End If
                    End If
                    colint += 1
                Next

            End If

            Return epack.Workbook
        End Function
        Dim lastrow As Integer
        Public Function LoadTableToExcelGrid(ByRef epack As ExcelPackage, ByVal cslist As List(Of t), ByVal TabName As String) As ExcelWorkbook
            epack.Workbook.Worksheets.Add(TabName)
            Try

            
            If cslist.Count > 0 Then
                Dim sheet1 As ExcelWorksheet = epack.Workbook.Worksheets(TabName)
                Dim newobj As New t
                Dim objtype As Type = newobj.GetType()
                Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                Dim colint As Integer = 1
                Dim rowint As Integer = 2
                Dim objarray = cslist.ToArray()

                For Each info As PropertyInfo In objprop
                    sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                    colint += 1
                Next
                sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True

                For Each obj In objarray
                    Dim propkey As Integer = 1
                    Dim newrowobj As t = obj
                    Dim newrowobjprop As System.Reflection.PropertyInfo() = newrowobj.GetType().GetProperties()
                    colint = 1
                    For Each info As PropertyInfo In newrowobjprop
                        sheet1.Cells(rowint, colint).Value = info.GetValue(newrowobj, Nothing)
                        If info.PropertyType().Name = "DateTime" Then
                            sheet1.Cells(rowint, colint).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss"
                        End If
                        colint += 1

                    Next
                    rowint += 1
                    lastrow = rowint
                Next
                sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()
            End If

            Catch ex As Exception

            End Try
            Return epack.Workbook
        End Function
        Public Function LoadTableToExcelGrid2(ByRef workbook As ExcelWorkbook, ByVal cslist As List(Of t), ByVal TabName As String) As ExcelWorkbook
            'Dim epack As New ExcelPackage
            workbook.Worksheets.Add(TabName)
            Try


                If cslist.Count > 0 Then
                    Dim sheet1 As ExcelWorksheet = workbook.Worksheets(TabName)
                    Dim newobj As New t
                    Dim objtype As Type = newobj.GetType()
                    Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                    Dim colint As Integer = 1
                    Dim rowint As Integer = 2
                    Dim objarray = cslist.ToArray()

                    For Each info As PropertyInfo In objprop
                        sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                        colint += 1
                    Next
                    sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True

                    For Each obj In objarray
                        Dim propkey As Integer = 1
                        Dim newrowobj As t = obj
                        Dim newrowobjprop As System.Reflection.PropertyInfo() = newrowobj.GetType().GetProperties()
                        colint = 1
                        For Each info As PropertyInfo In newrowobjprop
                            sheet1.Cells(rowint, colint).Value = info.GetValue(newrowobj, Nothing)
                            If info.PropertyType().Name = "DateTime" Then
                                sheet1.Cells(rowint, colint).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss"
                            End If
                            colint += 1

                        Next
                        rowint += 1
                        lastrow = rowint
                    Next
                    'sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()
                End If

            Catch ex As Exception

            End Try
            Return workbook
        End Function
    End Class

End Namespace